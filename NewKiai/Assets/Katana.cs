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
    public bool isDead = false;

    [Header("Movimenti")]
    [SerializeField] private Vector3[] posizioni;
    private int index_posizioni;
    public float velocita = 2f;
    public float velocita_corsa = 4f;    
    public bool isWalk = true;
    public bool isChase = false;  
    public bool Follow = true; 
    public bool  isAttack = false;
    public bool isHurt = false;
    private float horizontal;
    private bool facingR = true;
    private Vector2 xTarget;

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
    public float attackrange = 2f;
    [SerializeField] GameObject attack;
    public float chaseThreshold = 2f; // soglia di distanza per iniziare l'inseguimento

    [Header("VFX")]

    [SerializeField] public Transform slashpoint;
    [SerializeField] public Transform hitpoint;
    [SerializeField] GameObject VFXSdeng;
    [SerializeField] GameObject VFXHurt;

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
 // Aggiunge i canali audio degli AudioSource all'output del mixer
        foreach (AudioSource audioSource in bgm)
        {
        audioSource.outputAudioMixerGroup = SFX.FindMatchingGroups("Master")[0];
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
        Staminaobj.gameObject.SetActive(true);
        StaminaVFX.gameObject.SetActive(true);
        } 
        else if(!IsStamina)
        {
        Staminaobj.gameObject.SetActive(false);
        StaminaVFX.gameObject.SetActive(false);
        }

        //Se la stamina è a zero si stanca ed è vulnerabile
        if(!IsStamina)
        {
        if(stamina <= 0)
        {
        StaminaVFX.gameObject.SetActive(false);
        TiredAnm();        
        StartCoroutine(ripristina_Stamina());
        }
        }
#endregion
        ////////////////////////////////////////////////
        #region Move
        if(isWalk && !GameplayManager.instance.battle && !isHurt)
        {
        isChase = false;   
        isAttack = false;
        WalkAnm();
        Enemy.transform.position = Vector2.MoveTowards(Enemy.transform.position,posizioni[index_posizioni], Time.deltaTime * velocita);
        if (Enemy.transform.position.x<posizioni[index_posizioni].x){horizontal = -1;}
        else {horizontal = 1;}
                
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
        if (Vector2.Distance(transform.position, toy.transform.position) < chaseThreshold && !isAttack && !isHurt)
        {
        isChase = true;   
        isWalk = false;
        isAttack = false;
        if(isChase){Chase();}  
        }}
        #endregion
        ////////////////////////////////////////////////
        #region Attacca il Player
        if (Vector2.Distance(transform.position, toy.transform.position) < attackrange && !isHurt)
        {
        isChase = false;   
        isWalk = false;
        isAttack = true;
        Follow = false;
        if(isAttack){Attack();}
        }else{isWalk = true;}
        #endregion
        ////////////////////////////////////////////////
        }
        
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
            if (horizontal < 0)
        {

           rb.AddForce(Enemy.transform.right * knockForceShort, ForceMode2D.Impulse);
            dashTime -= Time.deltaTime;
        }
        else if (horizontal > 0)
        {
            rb.AddForce(-Enemy.transform.right * knockForceShort, ForceMode2D.Impulse);
            dashTime -= Time.deltaTime;
        }
         else if (horizontal == 0)
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
            if (horizontal < 0)
        {

           rb.AddForce(Enemy.transform.right * knockForceLong, ForceMode2D.Impulse);
            dashTime -= Time.deltaTime;
        }
        else if (horizontal > 0)
        {
            rb.AddForce(-Enemy.transform.right * knockForceLong, ForceMode2D.Impulse);
            dashTime -= Time.deltaTime;
        }
         else if (horizontal == 0)
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
                    PlayerHealth.Instance.currentStamina -=30;
                    StartCoroutine(ripristina_Posa());                    
                    Instantiate(VFXSdeng, hitpoint.position, transform.rotation);
                    if(isKnock)
                    {KnockbackAt = true;}
        }else
        {Damage();}}
        Damage();
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


#region Die
private void Die()
    {
        SpawnCoins();
        KiaiGive();
        DieAnm();
        StartCoroutine(DestroyEnm());
    }

     private IEnumerator DestroyEnm(){
        yield return new WaitForSeconds(1f);
        Destroy(gameObject);
    }
#endregion

#region Direction
private void Flip()
    {
        if (facingR && horizontal > 0f || !facingR && horizontal < 0f)
        {
            facingR = !facingR;
            Vector3 localScale = transform.localScale;
            localScale.x *= -1f;
            transform.localScale = localScale;
        }
    }

void FacePlayer()
    {
        if (toy != null)
        {
            if (toy.transform.position.x > Enemy.transform.position.x)
            {
                transform.localScale = new Vector3(-1, 1, 1);
            }
            else
            {
                transform.localScale = new Vector3(1, 1, 1);
            }
        }
    }
#endregion

private void Wait()
{
    rb.velocity = new Vector3(0, 0, 0);
    isAttack = false;
    Follow = true;
    IdleBattleAnm();
}

 private void Damage()
    {
        vitalita -= HitboxPlayer.Instance.Damage; 
        PlayMFX(1);
        skeletonAnimation.Skeleton.SetColor(Color.red);
        Instantiate(VFXHurt, hitpoint.position, transform.rotation);
        StartCoroutine(ripristina_colore());
        if(isKnock){ KnockbackAtL = true;}
    }

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
    yield return new WaitForSeconds(1f);
    isAttack = false;  
    Wait();    
    yield return new WaitForSeconds(0.5f);
    Follow = true;
    }

private IEnumerator ripristina_Stamina()
    {
        yield return new WaitForSeconds(2f);
        stamina = stamina_max;
        StaminaVFX.gameObject.SetActive(true);
        isHurt = false;
        IdleBattleAnm();    
    }

    private IEnumerator ripristina_Posa()
    {
        yield return new WaitForSeconds(1f);
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
    if (e.Data.Name == "slash_h2_normal") 
    {     
    
    
    }

}
#endregion

}