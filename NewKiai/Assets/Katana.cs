using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;
using Spine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;
using UnityEngine.UI;
public class Katana : MonoBehaviour
{    
    private GameObject toy; // Variabile per il player
    [Header("Vita")]
    private float vitalita;
    public float vitalita_max = 50;
    public Scrollbar HPBar;
    private bool isDead = false;

    [Header("Movimenti")]
    [SerializeField] private Vector3[] posizioni;
    private int index_posizioni;
    public float velocita = 2f;
    public float velocita_corsa = 4f;    
    private bool isWalk = true;
    private bool isChase = false;  
    private bool Follow = true; 
    private bool  isAttack = false;
    private bool isHurt = false;
    private float horizontal;
    [Header("Timer")]
    [Tooltip("Il tempo tra il ripristino della stamina o dell'attacco")]
    public float TimeRestoreAtk = 1f;
    public float TimeRestoreStamina = 2f;

    [Header("Stamina")]
    [Tooltip("Il nemico possiede una stamina?")]
    public bool IsStamina = false;
    private float stamina;
    public float stamina_max = 50;
    public float DamageStamina;
    [SerializeField] GameObject Staminaobj;
    [SerializeField] GameObject StaminaVFX;

    public Scrollbar staminaBar;
   

    [Header("Knockback")]
    [Tooltip("Il nemico fa knockback?")]
    public bool isKnock = false;
    private bool KnockbackAt = false;
    private bool KnockbackAtL = false;
    public float knockForceShort = 3f;
    public float knockForceLong = 5f;
    private float dashTime;
    [SerializeField] private Rigidbody2D rb;
    
    [Header("Attacks")]
    [Tooltip("A quale distanza attacca il nemico?")]
    public float attackrange = 2f;
    [SerializeField] GameObject attack;

    [Tooltip("A quale distanza il nemico insegue il player?")]
    public float chaseThreshold = 2f; // soglia di distanza per iniziare l'inseguimento

    [Header("VFX")]
    [SerializeField] public Transform slashpoint;
    [SerializeField] public Transform hitpoint;
    [SerializeField] GameObject VFXSdeng;
    [SerializeField] GameObject VFXHurt;
    [SerializeField] GameObject VFXSlash;

    private bool vfx = false;
    private float vfxTimer = 0.5f;

    [Header("Audio")]
    [HideInInspector] public float basePitch = 1f;
    [HideInInspector] public float randomPitchOffset = 0.1f;
    [SerializeField] public AudioClip[] listSound; // array di AudioClip contenente tutti i suoni che si vogliono riprodurre
    private AudioSource[] bgm; // array di AudioSource che conterrà gli oggetti AudioSource creati
    public AudioMixer SFX;
    //private bool sgmActive = false;

    [Header("Drop")]
    public GameObject coinPrefab; // prefab per la moneta
    private bool  SpawnC = false;
    [SerializeField] public Transform CoinPoint;
    public int maxCoins = 5; // numero massimo di monete che possono essere rilasciate
    private float coinForce = 5f; // forza con cui le monete saltano
    private Vector2 coinForceVariance = new Vector2(1, 0); // varianza della forza con cui le monete saltano
    
    [Header("Animations")]
    public GameObject EnmContent;
    [SerializeField] GameObject Enemy;
    private SkeletonAnimation skeletonAnimation;
     
    private string currentAnimationName;
    private Spine.AnimationState spineAnimationState;
    private Spine.Skeleton skeleton;
    Spine.EventData eventData;
    //private bool  anmDie = false;
    [SpineAnimation][SerializeField] string idle;
    [SpineAnimation][SerializeField] string walk;
    [SpineAnimation][SerializeField] string run;
    [SpineAnimation][SerializeField] string tired;
    [SpineAnimation][SerializeField] string guard;
    [SpineAnimation][SerializeField] string idle_battle;
    [SpineAnimation][SerializeField] string die;
    [SpineAnimation][SerializeField] string AttackV;

    [Header("Fatality")]
    private int result;
    public bool isFatality;
    [SerializeField] GameObject Fatality;



    // Start is called before the first frame update
    void Start(){
        vitalita = vitalita_max;
        if(IsStamina)
        {stamina = stamina_max;}
        toy = GameObject.FindWithTag("Player");
        skeletonAnimation = GetComponent<SkeletonAnimation>();
        if (skeletonAnimation == null) {
            Debug.LogError("Componente SkeletonAnimation non trovato!");
        }   
        spineAnimationState = GetComponent<Spine.Unity.SkeletonAnimation>().AnimationState;
        spineAnimationState = skeletonAnimation.AnimationState;
        skeleton = skeletonAnimation.skeleton;

         bgm = new AudioSource[listSound.Length]; // inizializza l'array di AudioSource con la stessa lunghezza dell'array di AudioClip
        for (int i = 0; i < listSound.Length; i++) // scorre la lista di AudioClip
        {
            bgm[i] = gameObject.AddComponent<AudioSource>(); // crea un nuovo AudioSource come componente del game object attuale (quello a cui è attaccato lo script)
            bgm[i].clip = listSound[i]; // assegna l'AudioClip corrispondente all'AudioSource creato
            bgm[i].playOnAwake = false; // imposto il flag playOnAwake a false per evitare che il suono venga riprodotto automaticamente all'avvio del gioco
            bgm[i].loop = false; // imposto il flag playOnAwake a false per evitare che il suono venga riprodotto automaticamente all'avvio del gioco

        }
        //Aggiunge i canali audio degli AudioSource all'output del mixer
        foreach (AudioSource audioSource in bgm)
        {
        audioSource.outputAudioMixerGroup = SFX.FindMatchingGroups("Master")[0];
        }
        if(isFatality)
        {
        // Genera un numero casuale tra 1 e 2
        float randomNumber = Random.Range(1f, 2f);
        // Converte il numero in intero
        result = Mathf.RoundToInt(randomNumber);
        Debug.Log("result"+ result);
        }
        if(IsStamina)
        {
        staminaBar.size = stamina / stamina_max;
        staminaBar.size = Mathf.Clamp(staminaBar.size, 0.01f, 1);
        Staminaobj.gameObject.SetActive(true);
        StaminaVFX.gameObject.SetActive(true);
        } 
        else if(!IsStamina)
        {
        Staminaobj.gameObject.SetActive(false);
        StaminaVFX.gameObject.SetActive(false);
        }
    }
 void Update(){
        
        ////////////////////////////////////////////////
         if (!GameplayManager.instance.PauseStop || isDead)
        {
            //Se gli HP sono a zero è mort
        HPBar.size = vitalita / vitalita_max;
        HPBar.size = Mathf.Clamp(HPBar.size, 0.01f, 1);
        #region StaminaLogic
        if(IsStamina)
        {
        staminaBar.size = stamina / stamina_max;
        staminaBar.size = Mathf.Clamp(staminaBar.size, 0.01f, 1);
        //Se la stamina è a zero si stanca ed è vulnerabile
        if(stamina <= 0)
        {
        StaminaVFX.gameObject.SetActive(false);
        TiredAnm();        
        StartCoroutine(ripristina_Stamina());
        }}
#endregion
        ////////////////////////////////////////////////
        #region Move
        if(isWalk && !GameplayManager.instance.battle && !isHurt && !isDead)
        {
        isChase = false;   
        isAttack = false;
        WalkAnm();
        Enemy.transform.position = Vector2.MoveTowards(Enemy.transform.position,posizioni[index_posizioni], Time.deltaTime * velocita);
        if (Enemy.transform.position.x<posizioni[index_posizioni].x){horizontal = 1;}
        else {horizontal = -1;}
                
        if (Enemy.transform.position == posizioni[index_posizioni]){
        if (index_posizioni == posizioni.Length -1)
        {index_posizioni = 0;} else {index_posizioni++;}
        }
        Flip();
        }
#endregion
        ////////////////////////////////////////////////
        #region RilevaPlayer e Insegue il Player
        if (Follow)
        {
        if (Vector2.Distance(transform.position, toy.transform.position) < chaseThreshold && !isAttack && !isHurt && !isDead)
        {
        isChase = true;   isWalk = false;  isAttack = false;
        if(isChase){Chase();}  
        }else if (Vector2.Distance(transform.position, toy.transform.position) > chaseThreshold && !isAttack && !isHurt && !isDead)
        {Follow = true; isChase = false;  isWalk = true; isAttack = false;}
        }
        #endregion
        ////////////////////////////////////////////////
        #region Attacca il Player
        if (Vector2.Distance(transform.position, toy.transform.position) < attackrange && !isHurt && !isDead)
        {
        isChase = false;   isWalk = false; isAttack = true; Follow = false;
        if(isAttack){Attack();}
        }else if (Vector2.Distance(transform.position, toy.transform.position) > attackrange && isAttack && !isHurt && !isDead)
        {Follow = true; isChase = false;  isWalk = true; isAttack = false;}
        #endregion
        ////////////////////////////////////////////////
        }
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        if(vfx)
        {vfxTimer -= Time.deltaTime; //decrementa il timer ad ogni frame
        if (vfxTimer <= 0f) {
        VFXSlash.gameObject.SetActive(false);
        vfx = false;
        }}
///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        
 }
 

 private void FixedUpdate()
    {
        if(!GameplayManager.instance.PauseStop)
        {
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        #region Knockback
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        if (KnockbackAt)
        {
            if (Enemy.transform.localScale.x < 0)
        {

           rb.AddForce(Enemy.transform.right * knockForceShort, ForceMode2D.Impulse);
            dashTime -= Time.deltaTime;
        }
        else if (Enemy.transform.localScale.x > 0)
        {
            rb.AddForce(-Enemy.transform.right * knockForceShort, ForceMode2D.Impulse);
            dashTime -= Time.deltaTime;
        }
         else if (Enemy.transform.localScale.x == 0)
        {
            if (rb.transform.localScale.x == -1)
        {

           rb.AddForce(Enemy.transform.right * knockForceShort, ForceMode2D.Impulse);
            dashTime -= Time.deltaTime;
        }
        else if (rb.transform.localScale.x == 1)
        {
            rb.AddForce(-Enemy.transform.right * knockForceShort, ForceMode2D.Impulse);
            dashTime -= Time.deltaTime;
        }
        }
            if (dashTime <= 0)
            {
                KnockbackAt = false;
            }
        }
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        if (KnockbackAtL)
        {
            if (Enemy.transform.localScale.x < 0)
        {

           rb.AddForce(Enemy.transform.right * knockForceLong, ForceMode2D.Impulse);
            dashTime -= Time.deltaTime;
        }
        else if (Enemy.transform.localScale.x > 0)
        {
            rb.AddForce(-Enemy.transform.right * knockForceLong, ForceMode2D.Impulse);
            dashTime -= Time.deltaTime;
        }
         else if (Enemy.transform.localScale.x == 0)
        {
            if (rb.transform.localScale.x == -1)
        {

           rb.AddForce(Enemy.transform.right * knockForceLong, ForceMode2D.Impulse);
            dashTime -= Time.deltaTime;
        }
        else if (rb.transform.localScale.x == 1)
        {
            rb.AddForce(-Enemy.transform.right * knockForceLong, ForceMode2D.Impulse);
            dashTime -= Time.deltaTime;
        }
        }
            if (dashTime <= 0)
            {
                KnockbackAtL = false;
            }
        }
        #endregion
        
        if(vitalita <= 0)
        {isDead = true;} 
        } 
}

   void OnTriggerEnter2D(Collider2D other) 
{
        if(other.gameObject.tag == "Hitbox")
        {  
        if(!isDead)
        {
            if(IsStamina)
        {
            if(stamina > 0)
        {
                    isHurt = true;
                    vitalita -= 5; 
                    PlayMFX(2);
                    stamina -= 10;
                    GuardAnm();
                    PlayMFX(3);
                    PlayerHealth.Instance.currentStamina -=30;
                    StartCoroutine(ripristina_Posa());                    
                    Instantiate(VFXSdeng, hitpoint.position, transform.rotation);
                    if(isKnock) {KnockbackAt = true;}
        }else if(stamina <= 0)
        {Damage(); TiredAnm(); StaminaVFX.gameObject.SetActive(false); StartCoroutine(ripristina_Stamina());}
        } else if(!IsStamina){Damage();}
        } else{Die();}
}     
}

private void Chase()
{
    RunAnm();
    // inseguimento del giocatore
    if (toy.transform.position.x > Enemy.transform.position.x)
    {
        Enemy.transform.localScale = new Vector2(1f, 1f);
    }
    else if (toy.transform.position.x < Enemy.transform.position.x)
    {
        Enemy.transform.localScale = new Vector2(-1f, 1f);
    }
    Vector2 targetPosition = new Vector2(toy.transform.position.x, transform.position.y);
    Enemy.transform.position = Vector2.MoveTowards(Enemy.transform.position, targetPosition, velocita_corsa * Time.deltaTime);
}

private void Attack()
{
    AttackAnm();
    ripristina_Atk();
}

private void Wait()
{
    rb.velocity = new Vector3(0, 0, 0);
    isAttack = false;
    IdleBattleAnm();
}

 private void Damage()
    {
        vitalita -= GameplayManager.instance.Damage; 
        PlayMFX(1);
        skeletonAnimation.Skeleton.SetColor(Color.red);
        Instantiate(VFXHurt, hitpoint.position, transform.rotation);
        StartCoroutine(ripristina_colore());
        if(isKnock){ KnockbackAtL = true;}
    }


#region Die
private void Die()
    {
        SpawnCoins();
        KiaiGive();
        if(isFatality){
        if(result == 1)
        {DieAnm(); StartCoroutine(DestroyEnm());
        }else if(result == 2)
        {Fatality.gameObject.SetActive(true); 
        Fatality.transform.position = Enemy.transform.position;
        if(Enemy.transform.localScale.x > 0){Fatality.transform.localScale = new Vector2(1, 1);}
        else if(Enemy.transform.localScale.x < 0){Fatality.transform.localScale = new Vector2(-1, 1);}
        Enemy.gameObject.SetActive(false);}}
        else
        {DieAnm(); StartCoroutine(DestroyEnm());
        }
    }

     private IEnumerator DestroyEnm(){
        yield return new WaitForSeconds(1f);
        Destroy(EnmContent);
    }
#endregion

#region Direction
private void Flip()
    {
        if (horizontal > 0)
            Enemy.transform.localScale = new Vector2(1, 1);
        else if (horizontal < 0)
            Enemy.transform.localScale = new Vector2(-1, 1);
    }

void FacePlayer()
    {
        if (toy != null)
        {
            if (toy.transform.position.x > Enemy.transform.position.x)
            {
                transform.localScale = new Vector3(1, 1, 1);
            }
            else
            {
                transform.localScale = new Vector3(-1, 1, 1);
            }
        }
    }
#endregion

#region Gizmos
private void OnDrawGizmos()
    {
    Gizmos.color = Color.red;
    Gizmos.DrawWireSphere(transform.position, chaseThreshold);
    Gizmos.color = Color.blue;
    Gizmos.DrawWireSphere(transform.position, attackrange);
    }
#endregion

#region Timers
private IEnumerator ripristina_Atk()
    {
    yield return new WaitForSeconds(TimeRestoreAtk);
    isAttack = false;  
    Wait();    
    }

private IEnumerator ripristina_Stamina()
    {
        yield return new WaitForSeconds(TimeRestoreStamina);//2
        stamina = stamina_max;
        StaminaVFX.gameObject.SetActive(true);
        isHurt = false;
        IdleBattleAnm();    
    }

    private IEnumerator ripristina_Posa()
    {
        yield return new WaitForSeconds(1f);
        isHurt = false;
        IdleBattleAnm(); 
    }

private IEnumerator ripristina_colore(){
        yield return new WaitForSeconds(0.1f);
        skeletonAnimation.Skeleton.SetColor(Color.white);
    }
#endregion

#region Drops
void KiaiGive()
{
    int randomChance = Random.Range(1, 10); // Genera un numero casuale compreso tra 1 e 10

    if (randomChance <= 8) // Se il numero casuale è compreso tra 1 e 8 (80% di probabilità), aggiungi 5 di essenza
    {
        PlayerHealth.Instance.currentKiai += 5;
        PlayerHealth.Instance.IncreaseKiai(1);
    }
    else // Se il numero casuale è compreso tra 9 e 10 (20% di probabilità), aggiungi 10 di essenza
    {
        PlayerHealth.Instance.currentKiai += 10;
        PlayerHealth.Instance.IncreaseKiai(2);
    }
}

public void SpawnCoins()
{
    if(!SpawnC)
    {

    for (int i = 0; i < maxCoins; i++)
    {
        // crea una nuova moneta
        GameObject newCoin = Instantiate(coinPrefab, CoinPoint.position, Quaternion.identity);

        // applica una forza casuale alla moneta per farla saltare
        Vector2 randomForce = new Vector2(
            Random.Range(-coinForceVariance.x, coinForceVariance.x), 2);
        newCoin.GetComponent<Rigidbody2D>().AddForce(randomForce * coinForce, ForceMode2D.Impulse);
    }
        SpawnC = true;
    }
}
#endregion

#region Music
public void PlayMFX(int soundToPlay)
    {
        bgm[soundToPlay].Stop();
        // Imposta la pitch dell'AudioSource in base ai valori specificati.
        bgm[soundToPlay].pitch = basePitch + Random.Range(-randomPitchOffset, randomPitchOffset); 
        bgm[soundToPlay].Play();
    }
#endregion

#region Animations

public void IdleAnm()
{
    if (currentAnimationName != idle)
                {
                    spineAnimationState.SetAnimation(2, idle, true);
                    currentAnimationName = idle;
                }
                // Add event listener for when the animation completes
                spineAnimationState.GetCurrent(2).Complete += OnAttackAnimationComplete;
}

public void IdleBattleAnm()
{
    if (currentAnimationName != idle_battle)
                {
                    spineAnimationState.SetAnimation(2, idle_battle, true);
                    currentAnimationName = idle_battle;
                }
                // Add event listener for when the animation completes
                spineAnimationState.GetCurrent(2).Complete += OnAttackAnimationComplete;
}
public void WalkAnm()
{
    if (currentAnimationName != walk)
                {
                    spineAnimationState.SetAnimation(2, walk, false);
                    currentAnimationName = walk;
                }
                // Add event listener for when the animation completes
                spineAnimationState.GetCurrent(2).Complete += OnAttackAnimationComplete;
}
public void RunAnm()
{
    if (currentAnimationName != run)
                {
                    spineAnimationState.SetAnimation(2, run, false);
                    currentAnimationName = run;
                }
                // Add event listener for when the animation completes
                spineAnimationState.GetCurrent(2).Complete += OnAttackAnimationComplete;
}
public void GuardAnm()
{
    if (currentAnimationName != guard)
                {
                    spineAnimationState.SetAnimation(2, guard, true);
                    currentAnimationName = guard;
                }
                // Add event listener for when the animation completes
                spineAnimationState.GetCurrent(2).Complete += OnAttackAnimationComplete;
}
public void TiredAnm()
{
    if (currentAnimationName != tired)
                {
                    spineAnimationState.SetAnimation(2, tired, true);
                    currentAnimationName = tired;
                }
                // Add event listener for when the animation completes
                spineAnimationState.GetCurrent(2).Complete += OnAttackAnimationComplete;
}
public void DieAnm()
{
    if (currentAnimationName != die)
                {
                    spineAnimationState.SetAnimation(2, die, true);
                    currentAnimationName = die;
                }
                // Add event listener for when the animation completes
                spineAnimationState.GetCurrent(2).Complete += OnAttackAnimationComplete;
}

public void AttackAnm()
{
    if (currentAnimationName != AttackV)
                {
                    spineAnimationState.SetAnimation(2, AttackV, false);
                    currentAnimationName = AttackV;
                }
                // Add event listener for when the animation completes
                spineAnimationState.GetCurrent(2).Complete += OnAttackAnimationComplete;
                spineAnimationState.Event += HandleEvent;
}

private void OnAttackAnimationComplete(Spine.TrackEntry trackEntry)
{
    // Remove the event listener
    trackEntry.Complete -= OnAttackAnimationComplete;

    // Clear the track 1 and reset to the idle animation
    spineAnimationState.ClearTrack(2); 
                if (currentAnimationName != idle) 
                {
                    spineAnimationState.SetAnimation(1, idle, true);
                    currentAnimationName = idle;
                }
}

#endregion

#region Events
void HandleEvent (TrackEntry trackEntry, Spine.Event e) 
{
    if (e.Data.Name == "VFXslash") 
    {     
    
     if(!vfx)
        {
        vfxTimer = 0.5f;
        VFXSlash.gameObject.SetActive(true);
        PlayMFX(2);
        vfx = true;
        }


    }

}
#endregion

}