using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;
using Spine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;
using UnityEngine.UI;
using Cinemachine;

public class BossThief : MonoBehaviour, IDamegable
{
[SerializeField] public GameObject Boss;
    
    [Header("Sistema Di HP")]
    public GameObject Camera;
    private CinemachineVirtualCamera virtualCamera; //riferimento alla virtual camera di Cinemachine

    [Header("Sistema Di HP")]
    public float maxHealth = 100f;
    public float currentHealth;
    public Scrollbar healthBar;
    public float maxStamina = 100f;
    public float currentStamina;
    public Scrollbar staminaBar;
    public Color originalColor;
    public float colorChangeDuration;
    //public float chaseThreshold = 2f; // soglia di distanza per iniziare l'inseguimento
   

    [Header("Moving")]
    public float moveSpeed = 2f; // velocità di movimento
    private Transform player;
    [SerializeField] LayerMask playerlayer;
    public Transform JumpPoint;
    [SerializeField] float pauseDuration = 0.5f; // durata della pausa
    private float pauseTimer; // timer per la pausa
    [SerializeField] private Vector3[] positions;
    private int id_positions;
    private float horizontal;
    private bool direction_x = true;
    public float JumpForce = 30f;
    public float CrushForce = 10f;
    public float YValue = -3.1f;



    [Header("Attacks")]
    public int AttackDamage = 20; // soglia di distanza per iniziare l'inseguimento
    public int AttackDamageLow = 5; // soglia di distanza per iniziare l'inseguimento
    public float damagestamina = 5; // danno d'attacco
    //public float chaseSpeed = 4f; // velocità di inseguimento
    //public float WaitAfterAtk = 2f;
    //public float sightRadius = 5f; // raggio di vista del nemico
    //public float attackrange = 2f;
    //public float attackCooldown = 1f; // durata del cooldown dell'attacco
    //private float attackTimer;
    public float InvincibleTime = 1f;
    public int GuardChance = 1; // Se il numero casuale è compreso tra 1 e 8 (80% di probabilità), aggiungi 5 di essenza
    public int randomChanceEN; // Genera un numero casuale compreso tra 1 e 10
    Vector2 moveDirection;

    [Header("Time for choose atk")]
    public float TimeForAtk; //decrementa il timer ad ogni frame
    public float TimeForAtkMAX = 2f;
   

    public float jumpSpeed = 10f;
    public float jumpHeight = 4f;
    public float fallTime = 0.5f;
    public float waitTime = 1f;

    private int jumpCount = 0;
    private int ThrowCount = 0;
    private int HitCount = 0;

    private Vector2 lastPosition;
    public Rigidbody2D rb;

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

    [Header("Abilitations")]
    private bool isMove = false;
    private bool isAttacking = false;
    private bool isHurt = false;
    private bool isDie = false;
    private bool isGuard = false;
    private bool isJump = false;
    private bool isThrow = false;
    private bool isCharge = false;
    private bool canAttack = true;
    private bool isPlayerInAttackRange = false;
    private bool isWait = false;
    public bool StartCharging = false;
    private bool activeActions = true;
    private bool isJumpAttacking = false;
    private bool StopJ = false;
    private bool Schiaccio = true;
    private float waitTimer = 0f;
    private float waitDuration = 2f;
    private bool take = false;

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
    [SpineAnimation][SerializeField] private string ChargeCrushAnmAnimationName;

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

    private enum State { Idle, Moving, Attack, Jump, Crush, Hurt, Wait, Guard, Death, Throw }
    private State currentState;

    public static BossThief instance;
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


public void BossDosentExist()
{
Destroy(Boss);
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
    virtualCamera = GameObject.FindWithTag("MainCamera").GetComponent<CinemachineVirtualCamera>(); 

}


private void Update()
    {
        if (!GameplayManager.instance.PauseStop)
        {
        healthBar.size = currentHealth / maxHealth;
        healthBar.size = Mathf.Clamp(healthBar.size, 0.01f, 1);
        staminaBar.size = currentStamina / maxStamina;
        staminaBar.size = Mathf.Clamp(staminaBar.size, 0.01f, 1);
            CheckState();
            Button();

            switch (currentState)
            {
                case State.Idle:
                    break;
                case State.Moving:
                    break;
                case State.Jump:
                    break;
                case State.Attack:
                    break;
                case State.Crush:
                    break;
                case State.Throw:
                    break;
                case State.Death:
                    break;
                case State.Hurt:
                    break;
                case State.Wait:
                    break;
                case State.Guard:
                    break;
            }
        }
    }


private void CheckState()
{
    if (Move.instance.isDeath) 
    {
return; // esce immediatamente se il personaggio è morto
}

if (currentHealth <= 0) 
{ // controlla se il personaggio è morto
    ResetColor();
    isAttacking = false;
    isMove = false;
    isGuard = false;
    isDie = true;
    isJumpAttacking = false; 
    isThrow = false;
    isCharge = false;  
    currentState = State.Death;
}

if (isWait) 
{ // controlla se il personaggio è morto
    ResetColor();
    isJumpAttacking = false; 
    isThrow = false;
    isCharge = false;  
    isAttacking = false;
    isMove = false;
    isGuard = false;
    isDie = true;
    Wait();
    currentState = State.Wait;
}

if (isJumpAttacking && !isCharge && !isThrow)
{
    ResetColor();
    currentState = State.Jump;

    if (jumpCount < 3)
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
            print("schiaccio!");
            JumpCrushAnm();
            Schiaccio = false;
            }
        }
    }
    else 
    {
        if(Schiaccio)
            {
            print("schiaccioFINALE!");
            JumpCrushAnm();
            Schiaccio = false;
            }
        
        // registra l'ultima posizione del boss
        lastPosition = rb.position;
        
        // resetta il contatore dei salti e l'indicatore di attacco a salto
        jumpCount = 0;
        isJumpAttacking = false;
    }

}

if (isThrow && !isJumpAttacking && !isCharge)
{
    ResetColor();
    currentState = State.Throw;

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
            isThrow = false;
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
            isThrow = false;
        }
    }
}


if (isCharge && !isJumpAttacking && !isThrow)
{
        ResetColor();
        currentState = State.Crush;

    if (currentHealth > 250)
    {
        if (HitCount < 1)
        {
            if (rb.velocity.y == 0f)
            {
                //print("StartCharge");
                FacePlayer();
                CrushAnm();
                HitCount++;
            } 
        }
        else 
        {
            // resetta il contatore dei lanci e l'indicatore di attacco a lancio
            HitCount = 0;
            isCharge = false;
        }
    }
    else if (currentHealth < 250) 
    {
        if (HitCount < 1)
        {
            if (rb.velocity.y == 0f)
            {
                //print("Fury");
                FacePlayer();
                CrushRageAnm();
                HitCount++;
            } 
        }
        else 
        {
            // resetta il contatore dei lanci e l'indicatore di attacco a lancio
            HitCount = 0;
            isCharge = false;
        }
    }
}


/*
if (Vector2.Distance(hitpoint.transform.position, player.position) < attackrange && 
!isAttacking && !isJumpAttacking && !isCharge && !isThrow) 
{ // controlla se il personaggio è dentro il raggio d'attacco
    ResetColor();
    isAttacking = true;
    isJumpAttacking = false; 
    isThrow = false;
    isCharge = false;  
    //Attack();
    FacePlayer();
    isMove = false;
    isPlayerInAttackRange = true;
    isGuard = false;
    currentState = State.Attack;
} 
else if (Vector2.Distance(hitpoint.transform.position, player.position) > attackrange && isPlayerInAttackRange &&
!isJumpAttacking && !isCharge && !isThrow) 
{ // controlla se il personaggio è appena uscito dal raggio e si avvia il timer di attesa
    ResetColor();
    isAttacking = false;
    isJumpAttacking = false; 
    isThrow = false;
    isCharge = false;  
    isMove = false;
    activeActions = false;
    isGuard = false;
    currentState = State.Wait;
}*/

}

IEnumerator StopD()
    {
        yield return new WaitForSeconds(0.5f);
        take = false;
    }

private void Button()
{
    #region testDanno
            if(Input.GetKeyDown(KeyCode.B))
            {
                //isJumpAttacking = true;
                //isThrow = true;
            isCharge = true;

            //Debug.Log("Il pulsante è stato premuto!");
            }
            #endregion

}

 

//Il nemico torna al suo patter dattacco dopo tot tempo
/*IEnumerator waitChase()
    {
        yield return new WaitForSeconds(WaitAfterAtk);
        isPlayerInAttackRange = false;
        activeActions = true;
    }*/


//Aspetta
private void Wait()
{
    IdleAnm();
    rb.velocity = new Vector3(0, 0);
    TimeForAtk -= Time.deltaTime; //decrementa il timer ad ogni frame
    if (TimeForAtk <= 0f) 
    {
    TimeForAtk = TimeForAtkMAX;
    RandomicAttack();
    }

}

private void OnTriggerEnter2D(Collider2D collision)
    {
    if(!take)
        {
        if (collision.CompareTag("Player"))
        {
             take = true;
            StartCoroutine(StopD());
            if(!Move.instance.isGuard)
            {
            if (!Move.instance.isDeath)
            {
                if (!Move.instance.isHurt)
            {
            PlayerHealth.Instance.Damage(AttackDamageLow);
            //Move.instance.Knockback();     
            //print("Colpito");       

            }}}if(Move.instance.isGuard)
            {
                Move.instance.KnockbackLong(); 
                Move.instance.GuardHit(); 
                PlayerHealth.Instance.currentStamina -= damagestamina;           
            }

    }else if (collision.gameObject.tag == "Hitbox")
    {
            take = true;
        Move.instance.sbam();
        Move.instance.KnockbackLong(); 
                  
           StartCoroutine(StopD());

    }
    }
    }


//Si muove da punto a punto
private void Moving()
{
if (isMove && !isAttacking && !isJumpAttacking && !isCharge && !isThrow)
    {
        // Controlla se l'oggetto deve spostarsi verso destra o sinistra
        if (transform.position.x < positions[id_positions].x)
        {
            horizontal = 1;
        }
        else 
        {
            horizontal = -1;
        }

        // Controlla se l'oggetto è arrivato alla posizione obiettivo corrente
        if (transform.position == positions[id_positions])
        {
            // Se è l'ultima posizione obiettivo, torna alla prima posizione
            if (id_positions == positions.Length - 1)
            {
                id_positions = 0;
            } 
            else
            {
                // Vai alla prossima posizione obiettivo
                isWait = true;
            }
        }

        // Controlla se è necessario fare una pausa
        if (pauseTimer > 0)
        {
            // Ferma l'animazione di movimento e attendi
            IdleAnm();
            pauseTimer -= Time.deltaTime;
            return;
        }

        // Sposta gradualmente l'oggetto verso la posizione obiettivo
        Boss.transform.position = Vector2.MoveTowards(transform.position, positions[id_positions], moveSpeed * Time.deltaTime);

        // Controlla se l'oggetto è arrivato alla posizione obiettivo corrente
        if (Vector2.Distance(transform.position, positions[id_positions]) < 0.0001f)
        {
            // Imposta il timer di pausa
            pauseTimer = pauseDuration;
        }
    }
    
    // Inverte l'orientamento dell'oggetto in base alla sua direzione di movimento
    Flip();

    // Esegui l'animazione di movimento
    if(currentHealth > 250)
    { MovingAnm();}
    else if(currentHealth <= 250)
    { RunAnm();}

}

//Si gira nella direzione
private void Flip()
    {
        if (direction_x && horizontal < 0f || !direction_x && horizontal > 0f)
        {
            direction_x = !direction_x;
            Vector3 localScale = transform.localScale;
            localScale.x *= -1f;
            transform.localScale = localScale;
        }
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

/*
//Attacco
private void Attack()
{
    if (isAttacking) // Se il personaggio sta attaccando...
    {
        if (attackTimer > 0) // ...e l'attacco non è ancora disponibile...
        {
            attackTimer -= Time.deltaTime; // ...decrementa il timer dell'attacco...
            canAttack = false; // ...e imposta la variabile "canAttack" su "false".
            return; // Esci dalla funzione.
        }
        else // Altrimenti...
        {
            canAttack = true; // ...imposta la variabile "canAttack" su "true".
        }

        if (canAttack && Vector2.Distance(transform.position, player.position) < attackrange) 
        // Se l'attacco è disponibile e il personaggio è abbastanza vicino al giocatore...
        {
            int randomChance = Random.Range(1, 3); // Genera un numero casuale compreso tra 1 e 10

            if (randomChance == 1) // Se il numero casuale è compreso tra 1 e 5 (50% di probabilità)
            {
                    Attack1Anm();
                                //print("1att");

            } // ...esegui l'animazione dell'attacco...            }
            else if (randomChance == 2)
            {
                    Attack2Anm(); 
                               // print("2att");
// ...esegui l'animazione dell'attacco...            }
            }
            }
            else if (randomChance == 3)
            {
                    Attack3Anm(); 
                                //print("3att");
// ...esegui l'animazione dell'attacco...            }
            }
    }
           
            attackTimer = attackCooldown; // ...e reimposta il timer dell'attacco.
}*/

//Danno
public void Damage(int damage)
{
    if (isDie) return;

RandomicDefence();

    if (isGuard) 
    {  
    Guard();
    Instantiate(VFXSdeng, slashpoint.position, transform.rotation);
    Move.instance.sbam();
    damage = 10;
    currentStamina -= 10;
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

//Processo di morte
private void StartDie()
{


}

void RandomicAttack()
{
    int randomChance = Random.Range(1, 3); // Genera un numero casuale compreso tra 1 e 10

    if (randomChance == 1) // Se il numero casuale è compreso tra 1 e 8 (80% di probabilità), aggiungi 5 di essenza
    {
    isJumpAttacking = true; 
    isThrow = false;
    isCharge = false;  
    } 
    else if (randomChance == 2) // Se il numero casuale è compreso tra 1 e 8 (80% di probabilità), aggiungi 5 di essenza
    {
    isThrow = true;
    isJumpAttacking = false; 
    isCharge = false;     
    }
    else if (randomChance == 3) // Se il numero casuale è compreso tra 1 e 8 (80% di probabilità), aggiungi 5 di essenza
    {
    isCharge = true;  
    isThrow = true;
    isJumpAttacking = false;    
    }
}

void RandomicDefence()
{
    randomChanceEN = Random.Range(1, 10); // Genera un numero casuale compreso tra 1 e 10

    if (randomChanceEN <= GuardChance) // Se il numero casuale è compreso tra 1 e 8 (80% di probabilità)
    {
    isMove = false;
    isAttacking = false;
    isGuard = true;
    isThrow = false;
    isJumpAttacking = false; 
    isCharge = false;  
    currentState = State.Guard;
    }else if (randomChanceEN >= GuardChance) // Se il numero casuale è compreso tra 1 e 8 (80% di probabilità)
    {
    isMove = false;
    isAttacking = false;
    isGuard = false;
    isThrow = false;
    isJumpAttacking = false; 
    isCharge = false;  
    //Subisce il danno 
    }
}


void KiaiGive()
{
    int randomChance = Random.Range(1, 10); // Genera un numero casuale compreso tra 1 e 10

    if (randomChance <= 8) // Se il numero casuale è compreso tra 1 e 8 (80% di probabilità), aggiungi 5 di essenza
    {
        PlayerHealth.Instance.currentKiai += 5;
    }
    else // Se il numero casuale è compreso tra 9 e 10 (20% di probabilità), aggiungi 10 di essenza
    {
        PlayerHealth.Instance.currentKiai += 10;
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

#region Gizmos
private void OnDrawGizmos()
    {
    //Gizmos.color = Color.red;
    //Gizmos.DrawWireSphere(hitpoint.transform.position, chaseThreshold);
    //Gizmos.color = Color.blue;
    //Gizmos.DrawWireSphere(hitpoint.transform.position, attackrange);
    //Gizmos.DrawLine(lastPosition, lastPosition + Vector2.down * 2f);
    //Debug.DrawRay(transform.position, new Vector3(chaseThreshold, 0), Color.red);
    }
#endregion

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
                _spineAnimationState.GetCurrent(2).Complete += OnAttackAnimationComplete;
}

public void CrushAnm()
{
    if (currentAnimationName != CrushAnmAnimationName)
                {
                    _spineAnimationState.SetAnimation(2, CrushAnmAnimationName, false);
                    currentAnimationName = CrushAnmAnimationName;
                    _spineAnimationState.Event += HandleEvent;
                    StartCharging = false;
                }
                // Add event listener for when the animation completes
                _spineAnimationState.GetCurrent(2).Complete += OnAttackAnimationComplete;
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
public void ChargeCrushAnm()
{
    if (currentAnimationName != ChargeCrushAnmAnimationName)
                {
                    _spineAnimationState.SetAnimation(2, ChargeCrushAnmAnimationName, true);
                    currentAnimationName = ChargeCrushAnmAnimationName;
                    _spineAnimationState.Event += HandleEvent;
                }
                // Add event listener for when the animation completes
                _spineAnimationState.GetCurrent(2).Complete += OnChargeAnimationComplete;
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

    }

if (e.Data.Name == "SpawnRock") 
    {
    Instantiate(Rock, slashpoint.position, transform.rotation);                                      
    }


    }



}



