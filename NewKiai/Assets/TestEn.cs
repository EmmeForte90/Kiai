using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;
using Spine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;



public class TestEn : Health, IDamegable
{



[Header("Enemy")]
[SerializeField] GameObject Brain;
private Transform player;
[SerializeField] LayerMask playerlayer;
private float timeBeforeDestroying = 3f;


[Header("Moving")]
public float moveSpeed = 2f; // velocità di movimento
[SerializeField] float pauseDuration = 2f; // durata della pausa
private float pauseTimer; // timer per la pausa
[SerializeField] private Vector3[] positions;
private int id_positions;
private float horizontal;
private bool direction_x = true;
public Rigidbody2D rb;

[Header("Specifiche del nemico")]
[Tooltip("Mettilo true per far in modo che il nemico insegua il player")]
public bool canChasesPlayer = false; // indica se il nemico sta inseguendo il player
[Tooltip("Mettilo true per far in modo che il nemico subisca un knockback quando colpito")]
public bool isSmall = false;

[Header("Attack")]
public float chaseSpeed = 4f; // velocità di inseguimento
public float WaitAfterAtk = 2f;
public float attackDamage = 10; // danno d'attacco
public float sightRadius = 5f; // raggio di vista del nemico
public float attackrange = 2f;
public float attackCooldown = 1f; // durata del cooldown dell'attacco
private float attackTimer;
public float InvincibleTime = 1f;
public int GuardChance = 3; // Se il numero casuale è compreso tra 1 e 8 (80% di probabilità), aggiungi 5 di essenza

[Header("Abilitations")]
private bool isChasing = false; // indica se il nemico sta inseguendo il player
private bool isMove = false;
private bool isAttacking = false;
private bool isHurt = false;
private bool isDie = false;
private bool pauseAtck = false;
private bool canAttack = true;
private bool firstattack = true;
private bool isPlayerInAttackRange = false;
private bool activeActions = true;
private bool OneDie = false;
private float waitTimer = 0f;
private float waitDuration = 2f;
private bool  isGuard = false;

[Header("Knockback")]

[SerializeField] private float knockForce = 1f;
private float KnockTime; //decrementa il timer ad ogni frame
public float Knockmax = 1f; //decrementa il timer ad ogni frame
private bool isKnockback = false;


 [Header("Audio")]
    [HideInInspector] public float basePitch = 1f;
    [HideInInspector] public float randomPitchOffset = 0.1f;
    [SerializeField] public AudioClip[] listSound; // array di AudioClip contenente tutti i suoni che si vogliono riprodurre
    private AudioSource[] bgm; // array di AudioSource che conterrà gli oggetti AudioSource creati
    public AudioMixer SFX;
    private bool sgmActive = false;

[Header("Drop")]
    public GameObject coinPrefab; // prefab per la moneta
    private bool  SpawnC = false;
    [SerializeField] public Transform CoinPoint;
    public int maxCoins = 5; // numero massimo di monete che possono essere rilasciate
    public float coinSpawnDelay = 5f; // ritardo tra la spawn di ogni moneta
    private int randomChance;
    private float coinForce = 5f; // forza con cui le monete saltano
    private Vector2 coinForceVariance = new Vector2(1, 0); // varianza della forza con cui le monete saltano
    private int coinCount; // conteggio delle monete

[Header("VFX")]

    [SerializeField] public Transform slashpoint;
    [SerializeField] public Transform hitpoint;
    [SerializeField] GameObject attack;
    [SerializeField] GameObject attack_h;
    [SerializeField] GameObject attack_B;
    [SerializeField] GameObject VFXSdeng;
    [SerializeField] GameObject VFXHurt;



[Header("Animations")]
    [SpineAnimation][SerializeField] private string idleAnimationName;
    [SpineAnimation][SerializeField] private string walkAnimationName;
    [SpineAnimation][SerializeField] private string runAnimationName;
    [SpineAnimation][SerializeField] private string attack1AnimationName;
    [SpineAnimation][SerializeField] private string attack2AnimationName;
    [SpineAnimation][SerializeField] private string attack3AnimationName;
    [SpineAnimation][SerializeField] private string hurtAnimationName;
    [SpineAnimation][SerializeField] private string diebackAnimationName;
    [SpineAnimation][SerializeField] private string diefrontAnimationName;
    [SpineAnimation][SerializeField] private string diealtAnimationName;
    [SpineAnimation][SerializeField] private string GuardAnimationName;

    private string currentAnimationName;
    public SkeletonAnimation _skeletonAnimation;
    private Spine.AnimationState _spineAnimationState;
    private Spine.Skeleton _skeleton;
    Spine.EventData eventData;

private enum State { Moving, Chase, Attack, Knockback, Dead, Hurt, Wait, Guard }
private State currentState;

public static TestEn instance;
    

    private void Awake()
    {
    if (_skeletonAnimation == null) {
        Debug.LogError("Componente SkeletonAnimation non trovato!");
    }
    _spineAnimationState = _skeletonAnimation.AnimationState;
    _skeleton = _skeletonAnimation.skeleton;
    if (instance == null)
    {
        instance = this;
    }
    player = GameObject.FindWithTag("Player").transform;
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

private void Update()
    {
        if (!GameplayManager.instance.PauseStop)
        {
            CheckState();

            #region testDanno
            if(Input.GetKeyDown(KeyCode.B))
            {
            Debug.Log("Il pulsante è stato premuto!");
            rb.isKinematic = false;
            isKnockback = true;
            Knockback();
            //Damage(10);
            }
            #endregion
            
            switch (currentState)
            {
                case State.Moving:
                    Moving();
                    break;
                case State.Chase:
                    Chase();
                    break;
                case State.Attack:
                    Attack();
                    FacePlayer();
                    if(firstattack)
        {
        attackTimer = 0;
        firstattack = false;
        }
                    break;
                case State.Knockback:
                    break;
                case State.Dead:
                
                    break;
                case State.Hurt:
                Wait();
                
                    break;
                case State.Wait:
                Wait();
                    break;
                case State.Guard:
                Guard();
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
    isChasing = false;
    isAttacking = false;
    isMove = false;
    activeActions = false;
    isGuard = false;
    isDie = true;
    SpawnCoins();
    Die();
    currentState = State.Dead;
} else if (isKnockback) 
{ // controlla se il personaggio è in knockback
    isChasing = false;
    isAttacking = false;
    isMove = false;
    isGuard = false;
    Knockback();
    currentState = State.Knockback;
} else if (isHurt) 
{ // controlla se il personaggio è stato colpito
    isChasing = false;
    isAttacking = false;
    isMove = false;
    activeActions = false;
    isGuard = false;
    currentState = State.Hurt;
} else if (Vector2.Distance(transform.position, player.position) < attackrange) 
{ // controlla se il personaggio è dentro il raggio d'attacco
    ResetColor();
    isChasing = false;
    isAttacking = true;
    isMove = false;
    isPlayerInAttackRange = true;
    isGuard = false;
    currentState = State.Attack;
} else if (Vector2.Distance(transform.position, player.position) > attackrange && isPlayerInAttackRange) 
{ // controlla se il personaggio è appena uscito dal raggio e si avvia il timer di attesa
    ResetColor();
    isChasing = false;
    isAttacking = false;
    isMove = false;
    activeActions = false;
    isGuard = false;
    currentState = State.Wait;
    StartCoroutine(waitChase());
} else if (Vector2.Distance(transform.position, player.position) < chaseThreshold && !isPlayerInAttackRange) 
{ // controlla se il personaggio è nel raggio di inseguimento
    ResetColor();
    if(canChasesPlayer)
    {isChasing = true;}
    isAttacking = false;
    isMove = false;
    firstattack = true;
    isGuard = false;
    currentState = State.Chase;
} else if (Physics2D.Raycast(transform.position, transform.TransformDirection(Vector3.up), 5f, playerlayer)) 
{ // controlla se il personaggio è bloccato da un ostacolo
    ResetColor();
    isChasing = false;
    isAttacking = false;
    isMove = false;
    isGuard = false;
    currentState = State.Wait;
} else if (activeActions) 
{ // controlla se il personaggio può muoversi in autonomia
    ResetColor();
    isMove = true;
    isChasing = false;
    isAttacking = false;
    isGuard = false;
    currentState = State.Moving;
}

if(isAttacking){
if (Move.instance.isAttacking || Move.instance.isAttackingAir) 
{ // controlla se il personaggio sta attaccando e quindi randomicamente parerà
    ResetColor();
    RandomicDefence();
}}

if(isKnockback)
{
        KnockTime -= Time.deltaTime; //decrementa il timer ad ogni frame
        if (KnockTime <= 0f) 
        {
        isKnockback = false; 
        rb.isKinematic = true;
        //rb.velocity = new Vector2(0f, 0f);
        KnockTime = Knockmax;
        currentState = State.Wait;
        }
}


}

//Il nemico torna a inseguirlo dopo tot tempo
IEnumerator waitChase()
    {
        yield return new WaitForSeconds(WaitAfterAtk);
        isPlayerInAttackRange = false;
        activeActions = true;
    }

//Aspetta
private void Wait()
{
    rb.velocity = new Vector3(0, 0);
    IdleAnm();
}

#region Move
// Controlla se l'oggetto deve essere in movimento, il rigedbody è in KInematic
private void Moving()
{
    if(!GameplayManager.instance.ordalia)
        {
    if (isMove && !isAttacking && !isKnockback)
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
                id_positions++;
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
        transform.position = Vector2.MoveTowards(transform.position, positions[id_positions], moveSpeed * Time.deltaTime);

        // Controlla se l'oggetto è arrivato alla posizione obiettivo corrente
        if (Vector2.Distance(transform.position, positions[id_positions]) < 0.0001f)
        {
            // Imposta il timer di pausa
            pauseTimer = pauseDuration;
        }
    }
    else if (!isMove && !isAttacking) // Se l'oggetto non deve essere in movimento
    {
        // Ferma l'animazione di movimento e attiva l'animazione di idle
        IdleAnm();
    }
    }
    // Inverte l'orientamento dell'oggetto in base alla sua direzione di movimento
    Flip();

    // Esegui l'animazione di movimento
    MovingAnm();
}

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
#endregion

private void Chase()
{
    if(isChasing && !isAttacking && !isKnockback)
    {
    // inseguimento del giocatore
    if (player.transform.position.x > transform.position.x)
    {
        transform.localScale = new Vector2(1f, 1f);
    }
    else if (player.transform.position.x < transform.position.x)
    {
        transform.localScale = new Vector2(-1f, 1f);
    }
    Vector2 targetPosition = new Vector2(player.position.x, transform.position.y);

    transform.position = Vector2.MoveTowards(transform.position, targetPosition, chaseSpeed * Time.deltaTime);

    ChaseAnm();
    }
}


private void Attack()
{
    if (isAttacking && !isKnockback) // Se il personaggio sta attaccando...
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
            int randomChance = Random.Range(1, 10); // Genera un numero casuale compreso tra 1 e 10

            if (randomChance == 5) // Se il numero casuale è compreso tra 1 e 5 (50% di probabilità)
            {
                    Attack1Anm();
            } // ...esegui l'animazione dell'attacco...            }
            else if (randomChance <= 4)
            {
                    Attack2Anm(); // ...esegui l'animazione dell'attacco...            }
            }
            }
            else if (randomChance >= 6)
            {
                    Attack3Anm(); // ...esegui l'animazione dell'attacco...            }
            }
    }
           
            attackTimer = attackCooldown; // ...e reimposta il timer dell'attacco.
}


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

public void Knockback()
    {
        if(isKnockback){
        rb.velocity = new Vector2(0f, 0f);
         // applica l'impulso del salto se il personaggio è a contatto con il terreno
        print("dovrebbefare il knockback");
         // applica l'impulso del salto se il personaggio è a contatto con il terreno
        if (transform.localScale.x < 0)
        {
        rb.AddForce(new Vector2(knockForce, 0f), ForceMode2D.Impulse);
        }
        else if (transform.localScale.x > 0)
        {
        rb.AddForce(new Vector2(-knockForce, 0f), ForceMode2D.Impulse);
        }
        }
    }

void RandomicDefence()
{
    int randomChance = Random.Range(1, 10); // Genera un numero casuale compreso tra 1 e 10

    if (randomChance <= GuardChance) // Se il numero casuale è compreso tra 1 e 8 (80% di probabilità), aggiungi 5 di essenza
    {
    isMove = false;
    isChasing = false;
    isAttacking = false;
    isGuard = true;
    Guard();
    currentState = State.Guard;
    }
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


#region Gizmos
private void OnDrawGizmos()
    {
    Gizmos.color = Color.red;
    Gizmos.DrawWireSphere(transform.position, chaseThreshold);
    Gizmos.color = Color.blue;
    Gizmos.DrawWireSphere(transform.position, attackrange);
        //Debug.DrawRay(transform.position, new Vector3(chaseThreshold, 0), Color.red);
    }
#endregion

public void Damage(int damage)
{
    if (isDie) return;

    if (isGuard) 
    {  
    Instantiate(VFXSdeng, slashpoint.position, transform.rotation);
    GameplayManager.instance.sbam();
    damage = 0;
    PlayMFX(0);
    return;
    }

    currentHealth -= damage;
    TemporaryChangeColor(Color.red);
    Instantiate(VFXHurt, hitpoint.position, transform.rotation);
    PlayMFX(2);
    if (isSmall)
    {
        HurtAnm();
        rb.isKinematic = false;
        isKnockback = true;
        Knockback();       
        //currentState = State.Hurt;
    }
    if (!isHurt)
    {
        StartCoroutine(WaitForHurt());
    }
}



private IEnumerator WaitForHurt()
{
    isHurt = true;
    yield return new WaitForSeconds(InvincibleTime);
    isHurt = false;
    activeActions = true;
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
public void Die()
{
    PlayMFX(2);
    //Drop
    KiaiGive();
    if(GameplayManager.instance.ordalia)
    {//Se è in un ordalia lo conteggia
        if(!OneDie)
        {
        GameplayManager.instance.EnemyDefeat();
        OneDie = true;
        }
    }

    if (horizontal == 1)
    {
        DieFront();
    }
    else if (horizontal == -1)
    {
        DieBack();
    }else if (horizontal == 0)
    {
        DieAlt();
    }

}

/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
//ANIMATIONS

public void DieFront()
{
    if (currentAnimationName != diefrontAnimationName)
                {    
                    _spineAnimationState.ClearTrack(2);
                    _spineAnimationState.SetAnimation(1, diefrontAnimationName, false);
                    currentAnimationName = diefrontAnimationName;
                    _spineAnimationState.Event += HandleEvent;
                }
                // Add event listener for when the animation completes
                //_spineAnimationState.GetCurrent(1).Complete += OnAttackAnimationComplete;
}
public void DieAlt()
{
    if (currentAnimationName != diealtAnimationName)
                {    
                    _spineAnimationState.ClearTrack(2);
                    _spineAnimationState.SetAnimation(1, diealtAnimationName, false);
                    currentAnimationName = diealtAnimationName;
                    _spineAnimationState.Event += HandleEvent;
                }
                // Add event listener for when the animation completes
                //_spineAnimationState.GetCurrent(1).Complete += OnAttackAnimationComplete;
}
public void DieBack()
{
    if (currentAnimationName != diebackAnimationName)
                {    
                    _spineAnimationState.ClearTrack(2);
                    _spineAnimationState.SetAnimation(1, diebackAnimationName, false);
                    currentAnimationName = diebackAnimationName;
                    _spineAnimationState.Event += HandleEvent;
                }
                // Add event listener for when the animation completes
                //_spineAnimationState.GetCurrent(1).Complete += OnAttackAnimationComplete;
}

public void Guard()
{
    if (currentAnimationName != GuardAnimationName)
                {    
                    rb.velocity = new Vector3(0, 0);
                    _spineAnimationState.ClearTrack(2);
                    _spineAnimationState.SetAnimation(1, GuardAnimationName, false);
                    currentAnimationName = GuardAnimationName;
                    _spineAnimationState.Event += HandleEvent;
                }
                // Add event listener for when the animation completes
                _spineAnimationState.GetCurrent(1).Complete += OnAttackAnimationComplete;
}

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
                    TemporaryChangeColor(Color.red);
                    _spineAnimationState.SetAnimation(2, hurtAnimationName, false);
                    currentAnimationName = hurtAnimationName;
                    _spineAnimationState.Event += HandleEvent;
                }
                // Add event listener for when the animation completes
               _spineAnimationState.GetCurrent(2).Complete += OnAttackAnimationComplete;
    
}
public void ChaseAnm()
{
    if (currentAnimationName != runAnimationName)
                {
                    _spineAnimationState.SetAnimation(2, runAnimationName, true);
                    currentAnimationName = runAnimationName;
                    _spineAnimationState.Event += HandleEvent;
                }
                // Add event listener for when the animation completes
                //_spineAnimationState.GetCurrent(1).Complete += OnAttackAnimationComplete;
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

private void OnAttackAnimationComplete(Spine.TrackEntry trackEntry)
{
    // Remove the event listener
    trackEntry.Complete -= OnAttackAnimationComplete;

    // Clear the track 1 and reset to the idle animation
    _spineAnimationState.ClearTrack(2);
    _spineAnimationState.SetAnimation(1, idleAnimationName, true);
    currentAnimationName = idleAnimationName;
}
///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
//EVENTS
//Non puoi giocare di local scale sui vfx perché sono vincolati dal localscale del player PERò puoi giocare sulla rotazione E ottenere gli
//stessi effetti
public void PlayMFX(int soundToPlay)
    {
        bgm[soundToPlay].Stop();
        // Imposta la pitch dell'AudioSource in base ai valori specificati.
        bgm[soundToPlay].pitch = basePitch + Random.Range(-randomPitchOffset, randomPitchOffset); 
        bgm[soundToPlay].Play();
    }

void HandleEvent (TrackEntry trackEntry, Spine.Event e) {

if (e.Data.Name == "VFXslash") {
        // Inserisci qui il codice per gestire l'evento.
        if(Brain.transform.localScale.x > 0)
        {Instantiate(attack, slashpoint.position, transform.rotation);}
        else if(Brain.transform.localScale.x < 0)
        {Instantiate(attack, slashpoint.position, transform.rotation);}
        PlayMFX(1);
    }

if (e.Data.Name == "Destroy") {
        // Inserisci qui il codice per gestire l'evento.
        Destroy(Brain);
    }

if (e.Data.Name == "VFXslash_h") {
        // Inserisci qui il codice per gestire l'evento.
        if(Brain.transform.localScale.x > 0)
        {Instantiate(attack_h, slashpoint.position, transform.rotation);}
        else if(Brain.transform.localScale.x < 0)
        {Instantiate(attack_h, -slashpoint.position, transform.rotation);}
        PlayMFX(1);
    }
if (e.Data.Name == "VFXSlashB") {
        // Inserisci qui il codice per gestire l'evento.
         if(Brain.transform.localScale.x > 0)
        {Instantiate(attack_B, slashpoint.position, transform.rotation);}
        else if(Brain.transform.localScale.x < 0)
        {Instantiate(attack_B, -slashpoint.position, transform.rotation);}
        PlayMFX(1);
    }
    if (e.Data.Name == "attack") {
        // Inserisci qui il codice per gestire l'evento.
        if(horizontal == 1)
        {
        //transform.position += transform.right * atckForward * Time.deltaTime; //sposta il nemico in avanti
        } else if(horizontal == 1)
        {
        //transform.position += transform.right * -atckForward * Time.deltaTime; //sposta il nemico in avanti
        } 
    }
}
}

