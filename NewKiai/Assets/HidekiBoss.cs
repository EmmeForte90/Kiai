using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;
using Spine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;
using UnityEngine.UI;

public class HidekiBoss : MonoBehaviour, IDamegable
{
    [Header("Questo boss è il numero...?")]
    public int id;

    [Header("Sistema Di HP")]
    public float maxHealth = 100f;
    public float currentHealth;
    public Scrollbar healthBar;
    public float maxStamina = 100f;
    public float currentStamina;
    public Scrollbar staminaBar;
    public Color originalColor;
    public float colorChangeDuration;

    [Header("VFX")]
    [SerializeField] public Transform slashpoint;
    [SerializeField] public Transform CrushP;
    [SerializeField] public Transform hitpoint;
    [SerializeField] public GameObject Rock;
    [SerializeField] public GameObject SlashDX;
    [SerializeField] public GameObject SlashSX;
    [SerializeField] GameObject attack;
    [SerializeField] GameObject attack_h;
    [SerializeField] GameObject attack_B;
    [SerializeField] GameObject VFXSdeng;
    [SerializeField] GameObject VFXHurt;
    [SerializeField] GameObject TextFin;

    [Header("Attacks")]
    public int CountAtk = 1;
    private Transform player;
    [SerializeField] LayerMask playerlayer;
    public int AttackDamage = 20; // soglia di distanza per iniziare l'inseguimento
    public int AttackDamageLow = 5; // soglia di distanza per iniziare l'inseguimento
    public float damagestamina = 5; // danno d'attacco
    public int GuardChance = 1; // Se il numero casuale è compreso tra 1 e 8 (80% di probabilità), aggiungi 5 di essenza
    public int randomChanceEN; // Genera un numero casuale compreso tra 1 e 10
    Vector2 moveDirection;
    public float jumpSpeed = 10f;
    public float jumpHeight = 4f;
    public float waitTime = 1f;
    private int jumpCount = 0;
    private int ThrowCount = 0;
    private int HitCount = 0;
    private float tireTimer = 3f;
    private float maxtiredTimer = 3f;

    private Vector2 lastPosition;
    public Rigidbody2D rb;


    [Header("Abilitations")]
    private bool isMove = false;
    private bool isHurt = false;
    private bool isTired = false;
    private bool isDie = false;
    private bool isGuard = false;
    private bool isJump = false;
    private bool isThrow = false;
    private bool isCharge = false;
    private bool isWait = false;
    private bool isAttack = false;
    public bool StartCharging = false;
    private bool isJumpAttacking = false;
    private bool Schiaccio = true;
    private float waitTimer = 0f;
    private float waitDuration = 2f;
    private bool take = false;
    private bool ChooseAtk = true;    
    private bool improve = true; 
    private bool stopAnim = true;
    private bool RestoAnm = false;
    [Header("Animations")]
    [SpineAnimation][SerializeField] private string idleAnimationName;
    [SpineAnimation][SerializeField] private string walkAnimationName;
    [SpineAnimation][SerializeField] private string runAnimationName;
    [SpineAnimation][SerializeField] private string attack1AnimationName;
    [SpineAnimation][SerializeField] private string attack2AnimationName;
    [SpineAnimation][SerializeField] private string attack3AnimationName;
    [SpineAnimation][SerializeField] private string hurtAnimationName;
    [SpineAnimation][SerializeField] private string ThrowAnmAnimationName;
    [SpineAnimation][SerializeField] private string NearAttkAnmAnimationName;
    [SpineAnimation][SerializeField] private string JumpAnimationName;
    [SpineAnimation][SerializeField] private string GuardAnimationName;
    [SpineAnimation][SerializeField] private string CrushAnmAnimationName;
    [SpineAnimation][SerializeField] private string JumpCrushAnmAnimationName;
    [SpineAnimation][SerializeField] private string CrushRageAnmAnimationName;
    [SpineAnimation][SerializeField] private string DieAnmAnimationName;
    [SpineAnimation][SerializeField] private string TiredAnmAnimationName;
    [SpineAnimation][SerializeField] private string restoAnimationName;
    [SpineAnimation][SerializeField] private string PCAnmAnimationName;
    private string currentAnimationName;
    public SkeletonAnimation _skeletonAnimation;
    private Spine.AnimationState _spineAnimationState;
    private Spine.Skeleton _skeleton;
    Spine.EventData eventData;

    [Header("Audio")]
    [HideInInspector] public float basePitch = 1f;
    [HideInInspector] public float randomPitchOffset = 0.1f;
    [SerializeField] public AudioClip[] listSound; // array di AudioClip contenente tutti i suoni che si vogliono riprodurre
    private AudioSource[] bgm; // array di AudioSource che conterrà gli oggetti AudioSource creati
    public AudioMixer SFX;
    private bool sgmActive = false;

    public static HidekiBoss instance;

private void Awake()
    {

    if (_skeletonAnimation == null) 
    {
        Debug.LogError("Componente SkeletonAnimation non trovato!");
    }
    _spineAnimationState = _skeletonAnimation.AnimationState;
    _skeleton = _skeletonAnimation.skeleton;
    if (instance == null)
    {
        instance = this;
    }
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

void Start()
{
 if (GameplayManager.instance == null) return;
    
    if (GameplayManager.instance.Easy)
    {
        maxHealth /= 2;
        maxStamina -= 50;
    }
    else if (GameplayManager.instance.Hard)
    {
        maxHealth *= 2;
        maxStamina += 50;
    }
    currentHealth = maxHealth;
    currentStamina = maxStamina;
    player = GameObject.FindWithTag("Player").transform;  
   
}

private void Update()
    {
        if (!GameplayManager.instance.PauseStop)
        {
        Buttontest();
        healthBar.size = currentHealth / maxHealth;
        healthBar.size = Mathf.Clamp(healthBar.size, 0.01f, 1);
        staminaBar.size = currentStamina / maxStamina;
        staminaBar.size = Mathf.Clamp(staminaBar.size, 0.01f, 1);

        FacePlayer();

        if (currentHealth <= 0) 
        { // controlla se il personaggio è morto
            ResetColor();
            isMove = false;
            isGuard = false;
            isDie = true;
            isJumpAttacking = false; 
            isThrow = false;
            isCharge = false;
            StartDie();
        }

        if(!isTired)
        {
            if(CountAtk == 1)
            {
                isJumpAttacking = true;
                isThrow = false;
                isCharge = false;
                StartCharging = false;

            }else if(CountAtk == 2)
            {            
                isJumpAttacking = false; 
                isThrow = true;
                isCharge = false;

            }else if(CountAtk == 3)
            {   
                isJumpAttacking = false; 
                isThrow = false;
                isCharge = true;

            }else if(CountAtk == 4)
            {   
                isJumpAttacking = false; 
                isThrow = false;
                isCharge = false;
                CountAtk = 1;
            }
        }



        if(!isWait || !isTired)
        {
            if(isJumpAttacking)
            {
            JumpAtk();
            //print("Salto");
            }else if(isThrow)
            {
            LanciaSasso();
            //print("Sasso");
            }else if(isCharge)
            {
           // print("Counter");
            Charge();
            }
        }
            }




 if(currentStamina <= 0) // verifica se la stamina corrente è minore o uguale a 0
{
     if(!RestoAnm) // verifica se la stamina corrente è minore o uguale a 0
    {
    Tired(); // chiama la funzione "Tired()"
    Stop(); // chiama la funzione "Stop()"
    isTired = true; // imposta la variabile "isTired" su true
    TextFin.gameObject.SetActive(true); // attiva l'oggetto "TextFin" per mostrare il testo
    RestoAnm = true;
    }

    if(isTired) // verifica se la variabile "isTired" è true
    {
        tireTimer -= Time.deltaTime; // decrementa "tireTimer" in base al tempo trascorso dall'ultimo frame

        if (tireTimer <= 0f) // verifica se "tireTimer" ha raggiunto il valore di zero
        {
            TextFin.gameObject.SetActive(false); // disattiva l'oggetto "TextFin" per nascondere il testo
            RestoreStamina(); // chiama la funzione "RestoreStamina()"
            tireTimer = maxtiredTimer; // ripristina "tireTimer" al valore massimo
            StartCoroutine(restoreSt());      
        }
    }
}

}

     private void Buttontest()
    {
       #region testForanysituation
            if(Input.GetKeyDown(KeyCode.C))
            {
                currentStamina -= 100;
                //currentHealth -= 60;

            } 
            #endregion
    }

IEnumerator StopD()
{
    yield return new WaitForSeconds(1f);
    // Controllo del conteggio degli attacchi
    if (improve && !isTired)
    {
        CountAtk++;
        improve = false; // Imposta la variabile di stato a false dopo l'incremento
    }
}

IEnumerator restoreSt()
{    
    yield return new WaitForSeconds(2f);
    isTired = false; // imposta la variabile "isTired" su false
}

IEnumerator Crushi()
    {
        yield return new WaitForSeconds(1f);
        CrushAnm();

    }

public void Stop()
    {
        rb.velocity = new Vector2(0f, 0f);
        //horDir = 0;
        //Swalk.Stop();
    }

    private void StartDie()
    {
        PlayMFX(0);    
        DieAnm();
    }

//Guarda verso il player
void FacePlayer()
    {
        if (player != null)
        {
            if (player.transform.position.x > transform.position.x)
            {
                transform.localScale = new Vector3(1, 1, 1);
            }
            else
            {
                transform.localScale = new Vector3(-1, 1, 1);
            }
        }
    }
/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
//attacks

    private void Attack()
{
    if (isAttack && !isJumpAttacking && !isCharge && !isThrow ) // Se il personaggio sta attaccando...
    {
            int randomChanceATK = Random.Range(1, 3); // Genera un numero casuale compreso tra 1 e 3

            if (randomChanceATK == 1) // Se il numero casuale è compreso tra 1 e 5 (50% di probabilità)
            {
                    Attack1Anm();
                    // ...esegui l'animazione dell'attacco...            }
            } 
            else if (randomChanceATK == 2)
            {
                    Attack2Anm(); 
                    // ...esegui l'animazione dell'attacco...            }
            }
            else if (randomChanceATK == 3)
            {
                    Attack3Anm(); 
                    // ...esegui l'animazione dell'attacco...            }
            }
    }   
}

private void JumpAtk()
{
    if (isJumpAttacking && !isCharge && !isThrow && !isAttack)
{

    if (jumpCount < 2)
    {
        if (rb.velocity.y == 0f)
        {
            // il boss salta
            Jump();
            
            // determina la direzione di movimento
            Vector2 moveDirection = Vector2.zero;
            if (player.position.x < rb.position.x)
            {
                moveDirection = Vector2.left;
            }
            else
            {
                moveDirection = Vector2.right;
            }
            FacePlayer();
            // aggiungi forza per far muovere il boss verso la direzione di movimento
            rb.AddForce(moveDirection * jumpHeight, ForceMode2D.Impulse);
            
            // aggiungi forza verso l'alto per far saltare il boss
            rb.AddForce(Vector2.up * jumpSpeed, ForceMode2D.Impulse);
            
            Schiaccio = true;
            // incrementa il contatore dei salti
            jumpCount++;
        } else if (rb.velocity.y <= -6f)
        {
            if(Schiaccio)
            {
            //print("schiaccio!");
            JumpCrushAnm();
            Schiaccio = false;
            }
        }
    }
    else 
    {
        if(Schiaccio)
            {
            //print("schiaccioFINALE!");
            //JumpCrushAnm();
            Schiaccio = false;
            }
        
        // registra l'ultima posizione del boss
        lastPosition = rb.position;
        
        // resetta il contatore dei salti e l'indicatore di attacco a salto
        jumpCount = 0;
        //CountAtk = 2;
        isJumpAttacking = false;
        isWait = true;    
        improve = true; 
        StartCoroutine(StopD());

    }

}
}


private void LanciaSasso()
{
if (isThrow && !isJumpAttacking && !isCharge && !isAttack)
{
    improve = true; 

    if (currentHealth > 250)
    {
        if (ThrowCount < 1)
        {
            if (rb.velocity.y == 0f)
            {
                FacePlayer();
                Throw();
                // incrementa il contatore dei lanci
                ThrowCount++;
            } 
        }
        else 
        {
            // resetta il contatore dei lanci e l'indicatore di attacco a lancio
            ThrowCount = 0;
            //CountAtk = 3;
            isWait = true;    
        StartCoroutine(StopD());    
        }
    }
    else if (currentHealth < 250) 
    {
        if (ThrowCount < 3)
        {
            if (rb.velocity.y == 0f)
            {
                FacePlayer();
                Throw();
                // incrementa il contatore dei lanci
                ThrowCount++;
            } 
        }
        else 
        {
            // resetta il contatore dei lanci e l'indicatore di attacco a lancio
            ThrowCount = 0;
            //CountAtk = 3;
            isWait = true;  
        StartCoroutine(StopD());    
        }
    }
}
}

// La funzione "Charge" controlla lo stato delle variabili relative all'attacco e si occupa di avviare l'attacco in base alle condizioni.
private void Charge()
{
    // Verifica se il personaggio è in stato di "caricamento" e se non sta attualmente attaccando, saltando o lanciando qualcosa.
    if (isCharge && !isJumpAttacking && !isThrow && !isAttack)
    {
        // Se la vita del personaggio è maggiore di 250:
        if (currentHealth > 250)
        {
            // Controlla il numero di colpi inflitti all'avversario.
            if (HitCount < 1)
            {
                // Se il personaggio non sta ancora caricando l'attacco:
                if (!StartCharging)
                {
                    // Verifica che il personaggio non stia saltando.
                    if (rb.velocity.y == 0f)
                    {
                        // Prepara l'attacco di "crush", imposta il personaggio rivolto verso l'avversario e avvia la coroutine "Crushi()" per eseguire l'attacco.
                        FacePlayer();
                        PrepareCrush();
                        StartCoroutine(Crushi());
                        StartCharging = true; // Imposta il personaggio in stato di "caricamento".
                    } 
                }
            }
            else 
            {
                // Se il personaggio ha già inflitto almeno un colpo all'avversario:
                // Resetta il contatore dei colpi inflitti, imposta la variabile "isCharge" a false (il personaggio non sta più caricando l'attacco) e avvia la coroutine "StopD()" per "fermare" l'attacco.
                HitCount = 0;
                isCharge = false;
                isWait = true;    
                StartCoroutine(StopD());    
            }
        }
        // Se la vita del personaggio è inferiore o uguale a 250:
        else if (currentHealth < 250) 
        {
            // Controlla il numero di colpi inflitti all'avversario.
            if (HitCount < 1)
            {
                // Verifica che il personaggio non stia saltando.
                if (rb.velocity.y == 0f)
                {
                    // Esegue un attacco "furioso" (CrushRageAnm()) e incrementa il contatore dei colpi inflitti.
                    FacePlayer();
                    CrushRageAnm();
                    HitCount++;
                } 
            }
            else 
            {
                // Se il personaggio ha già inflitto almeno un colpo all'avversario:
                // Resetta il contatore dei colpi inflitti, imposta la variabile "isWait" a true e avvia la coroutine "StopD()" per "fermare" l'attacco.
                HitCount = 0;
                isCharge = false;
                isWait = true;    
                StartCoroutine(StopD());   
            }
        }
    }
}


void RandomicDefence()
{
    randomChanceEN = Random.Range(1, 10); // Genera un numero casuale compreso tra 1 e 10

    if (randomChanceEN <= GuardChance) // Se il numero casuale è compreso tra 1 e 8 (80% di probabilità)
    {
    isGuard = true; 
    }else if (randomChanceEN >= GuardChance) // Se il numero casuale è compreso tra 1 e 8 (80% di probabilità)
    {
    isGuard = false;
    //Subisce il danno 
    }
}

/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
//Damage

public void Damage(int damage)
{
    if (isDie) return;

    if (isWait || isThrow)
    {
    RandomicDefence();
    }

    if (isGuard) 
    {  
    Guard();
    Instantiate(VFXSdeng, slashpoint.position, transform.rotation);
    GameplayManager.instance.sbam();
    damage = 10;
    currentStamina -= 20;
    PlayMFX(2);    
    KiaiGive();

    return;
    }
    
    KiaiGive();
    currentHealth -= damage;
    TemporaryChangeColor(Color.red);
    Instantiate(VFXHurt, hitpoint.position, transform.rotation);
    PlayMFX(1);
    
}

void KiaiGive()
{
    int randomChance = Random.Range(1, 10); // Genera un numero casuale compreso tra 1 e 10

    if (randomChance <= 8) // Se il numero casuale è compreso tra 1 e 8 (80% di probabilità), aggiungi 5 di essenza
    {
        PlayerHealth.Instance.currentKiai += 5;
        PlayerHealth.Instance.IncreaseKiai(5);
    }
    else // Se il numero casuale è compreso tra 9 e 10 (20% di probabilità), aggiungi 10 di essenza
    {
        PlayerHealth.Instance.currentKiai += 10;
        PlayerHealth.Instance.IncreaseKiai(10);
    }
}

public void TemporaryChangeColor(Color color)
{
    _skeletonAnimation.Skeleton.SetColor(color);
    Invoke(nameof(ResetColor), colorChangeDuration);
}
private void ResetColor()
{
    _skeletonAnimation.Skeleton.SetColor(originalColor);
}

/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
//Animations


public void Attack1Anm()
{
            
   
    if (currentAnimationName != attack1AnimationName)
                {
                    _spineAnimationState.SetAnimation(2, attack1AnimationName, false);
                    currentAnimationName = attack1AnimationName;
                    _spineAnimationState.Event += HandleEvent;
                }
                // Add event listener for when the animation completes
               _spineAnimationState.GetCurrent(2).Complete += OnAttackAnimationComplete;
    
}
public void Attack2Anm()
{
            
   
    if (currentAnimationName != attack2AnimationName)
                {
                    _spineAnimationState.SetAnimation(2, attack2AnimationName, false);
                    currentAnimationName = attack2AnimationName;
                    _spineAnimationState.Event += HandleEvent;
                }
                // Add event listener for when the animation completes
               _spineAnimationState.GetCurrent(2).Complete += OnAttackAnimationComplete;
    
}

public void Attack3Anm()
{
            
   
    if (currentAnimationName != attack3AnimationName)
                {
                    _spineAnimationState.SetAnimation(2, attack3AnimationName, false);
                    currentAnimationName = attack3AnimationName;
                    _spineAnimationState.Event += HandleEvent;
                }
                // Add event listener for when the animation completes
               _spineAnimationState.GetCurrent(2).Complete += OnAttackAnimationComplete;
    
}
public void HurtAnm()
{
            
    if (currentAnimationName != hurtAnimationName)
                {
                    //TemporaryChangeColor(Color.red);
                    _spineAnimationState.SetAnimation(2, hurtAnimationName, false);
                    currentAnimationName = hurtAnimationName;
                    _spineAnimationState.Event += HandleEvent;
                }
                // Add event listener for when the animation completes
               _spineAnimationState.GetCurrent(2).Complete += OnAttackAnimationComplete;
    
}

public void Guard()
{
    if (currentAnimationName != GuardAnimationName)
                {    
                    //rb.velocity = new Vector3(0, 0);
                    _spineAnimationState.ClearTrack(2);
                    _spineAnimationState.SetAnimation(2, GuardAnimationName, true);
                    currentAnimationName = GuardAnimationName;
                    _spineAnimationState.Event += HandleEvent;
                }
                // Add event listener for when the animation completes
                _spineAnimationState.GetCurrent(2).Complete += OnAttackAnimationComplete;
}

public void MovingAnm()
{
    
    if (currentAnimationName != walkAnimationName)
                {
                    _spineAnimationState.SetAnimation(2, walkAnimationName, true);
                    currentAnimationName = walkAnimationName;
                    _spineAnimationState.Event += HandleEvent;
                }
                // Add event listener for when the animation completes
                _spineAnimationState.GetCurrent(2).Complete += OnAttackAnimationComplete;
}

public void RunAnm()
{
    
    if (currentAnimationName != runAnimationName)
                {
                    _spineAnimationState.SetAnimation(2, runAnimationName, true);
                    currentAnimationName = runAnimationName;
                    _spineAnimationState.Event += HandleEvent;
                }
                // Add event listener for when the animation completes
                _spineAnimationState.GetCurrent(2).Complete += OnAttackAnimationComplete;
}
public void RestoreStamina()
{
    
    if (currentAnimationName != restoAnimationName)
                {
                    _spineAnimationState.ClearTrack(2);
                    _spineAnimationState.SetAnimation(2, restoAnimationName, false);
                    currentAnimationName = restoAnimationName;
                    _spineAnimationState.Event += HandleEvent;
                }
                // Add event listener for when the animation completes
                _spineAnimationState.GetCurrent(2).Complete += OnAttackAnimationComplete;
}
public void IdleAnm()
{
    if (currentAnimationName != idleAnimationName)
                {
                    _spineAnimationState.SetAnimation(1, idleAnimationName, true);
                    currentAnimationName = idleAnimationName;
                    _spineAnimationState.Event += HandleEvent;
                }
                // Add event listener for when the animation completes
                //_spineAnimationState.GetCurrent(2).Complete += OnAttackAnimationComplete;
}

public void Jump()
{
    
    if (currentAnimationName != JumpAnimationName)
                {
                    _spineAnimationState.SetAnimation(2, JumpAnimationName, true);
                    currentAnimationName = JumpAnimationName;
                    _spineAnimationState.Event += HandleEvent;
                }
                // Add event listener for when the animation completes
                //_spineAnimationState.GetCurrent(2).Complete += OnAttackAnimationComplete;
}

public void PrepareCrush()
{
    if (currentAnimationName != PCAnmAnimationName)
                {
                    _spineAnimationState.SetAnimation(2, PCAnmAnimationName, true);
                    currentAnimationName = PCAnmAnimationName;
                    _spineAnimationState.Event += HandleEvent;
                }
                // Add event listener for when the animation completes
                //_spineAnimationState.GetCurrent(2).Complete += OnAttackAnimationComplete;
}

public void CrushAnm()
{
    if (currentAnimationName != CrushAnmAnimationName)
                {
                    _spineAnimationState.SetAnimation(2, CrushAnmAnimationName, false);
                    currentAnimationName = CrushAnmAnimationName;
                    _spineAnimationState.Event += HandleEvent;
                }
                // Add event listener for when the animation completes
                //_spineAnimationState.GetCurrent(2).Complete += OnAttackAnimationComplete;
}
public void CrushRageAnm()
{
    if (currentAnimationName != CrushRageAnmAnimationName)
                {
                    _spineAnimationState.SetAnimation(2, CrushRageAnmAnimationName, false);
                    currentAnimationName = CrushRageAnmAnimationName;
                    _spineAnimationState.Event += HandleEvent;
                    StartCharging = false;
                }
                // Add event listener for when the animation completes
                _spineAnimationState.GetCurrent(2).Complete += OnAttackAnimationComplete;
}


public void JumpCrushAnm()
{
    if (currentAnimationName != JumpCrushAnmAnimationName)
                {
                    _spineAnimationState.SetAnimation(2, JumpCrushAnmAnimationName, false);
                    currentAnimationName = JumpCrushAnmAnimationName;
                    _spineAnimationState.Event += HandleEvent;
                }
                // Add event listener for when the animation completes
               _spineAnimationState.GetCurrent(2).Complete += OnAttackAnimationComplete;
}

public void Throw()
{
    if (currentAnimationName != ThrowAnmAnimationName)
                {
                    _spineAnimationState.SetAnimation(2, ThrowAnmAnimationName, false);
                    currentAnimationName = ThrowAnmAnimationName;
                    _spineAnimationState.Event += HandleEvent;
                }
                // Add event listener for when the animation completes
                _spineAnimationState.GetCurrent(2).Complete += OnAttackAnimationComplete;
}

public void DieAnm()
{
    if (currentAnimationName != DieAnmAnimationName)
                {
                    _spineAnimationState.SetAnimation(1, DieAnmAnimationName, true);
                    currentAnimationName = DieAnmAnimationName;
                    _spineAnimationState.Event += HandleEvent;
                }
                // Add event listener for when the animation completes
                //_spineAnimationState.GetCurrent(2).Complete += OnAttackAnimationComplete;
}

public void NearAttk()
{
    if (currentAnimationName != NearAttkAnmAnimationName)
                {
                    _spineAnimationState.SetAnimation(1, NearAttkAnmAnimationName, false);
                    currentAnimationName = NearAttkAnmAnimationName;
                    _spineAnimationState.Event += HandleEvent;
                }
                // Add event listener for when the animation completes
                _spineAnimationState.GetCurrent(2).Complete += OnAttackAnimationComplete;
}

public void Tired()
{
    if (currentAnimationName != TiredAnmAnimationName)
                {
                    _spineAnimationState.SetAnimation(2, TiredAnmAnimationName, true);
                    currentAnimationName = TiredAnmAnimationName;
                    _spineAnimationState.Event += HandleEvent;
                }
                // Add event listener for when the animation completes
                //_spineAnimationState.GetCurrent(2).Complete += OnAttackAnimationComplete;
}

private void OnChargeAnimationComplete(Spine.TrackEntry trackEntry)
{
    // Remove the event listener
    trackEntry.Complete -= OnChargeAnimationComplete;

    // Clear the track 1 and reset to the idle animation
    _spineAnimationState.ClearTrack(2);
   _spineAnimationState.SetAnimation(2, CrushRageAnmAnimationName, false);
    currentAnimationName = CrushRageAnmAnimationName;
    _spineAnimationState.GetCurrent(2).Complete += OnEndAnimationComplete;

}
private void OnEndAnimationComplete(Spine.TrackEntry trackEntry)
{
    // Remove the event listener
    trackEntry.Complete -= OnEndAnimationComplete;

    // Clear the track 1 and reset to the idle animation
    _spineAnimationState.SetAnimation(2, idleAnimationName, true);
    currentAnimationName = idleAnimationName;
}

private void OnAttackAnimationComplete(Spine.TrackEntry trackEntry)
{
    // Remove the event listener
    trackEntry.Complete -= OnAttackAnimationComplete;

    // Clear the track 1 and reset to the idle animation
    _spineAnimationState.ClearTrack(2);
    //_spineAnimationState.ClearTrack(3);
    _spineAnimationState.SetAnimation(1, idleAnimationName, true);
    currentAnimationName = idleAnimationName;
    isCharge = false;

}
/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
//Audio
public void PlayMFX(int soundToPlay)
    {
        bgm[soundToPlay].Stop();
        // Imposta la pitch dell'AudioSource in base ai valori specificati.
        bgm[soundToPlay].pitch = basePitch + Random.Range(-randomPitchOffset, randomPitchOffset); 
        bgm[soundToPlay].Play();
    }
/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
//Event


void HandleEvent (TrackEntry trackEntry, Spine.Event e) 
    {

if (e.Data.Name == "SpawnFury") 
    {
        Instantiate(SlashDX, CrushP.position, transform.rotation);
        Instantiate(SlashSX, CrushP.position, transform.rotation);                             
        StartCoroutine(StopD());    
    }

if (e.Data.Name == "SpawnRock") 
    {
    Instantiate(Rock, slashpoint.position, transform.rotation);                                      
    }

if (e.Data.Name == "endCount") 
    {
    CountAtk = 4;    
    }
if (e.Data.Name == "RestoreAtk") 
    {
    RestoAnm = false;
    currentStamina = maxStamina; // ripristina la stamina corrente al valore massimo
    CountAtk = 1;
    }
    }
}
