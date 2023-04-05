using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;
using Spine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;


public class AiEnemysword : Health, IDamegable
{


[Header("Enemy")]
[SerializeField] GameObject Brain;
private Health health;
private Transform player;
[SerializeField] LayerMask playerlayer;
private float timeBeforeDestroying = 3f;

[Header("Moving")]
public float moveSpeed = 2f; // velocità di movimento
[SerializeField] float atckForward = 5; // velocità di movimento
[SerializeField] float pauseDuration = 0.5f; // durata della pausa
private float pauseTimer; // timer per la pausa
[SerializeField] private Vector3[] positions;
private int id_positions;
private float horizontal;
private bool direction_x = true;
private Rigidbody2D rb;

[Header("Attack")]
public float chaseSpeed = 4f; // velocità di inseguimento
public float WaitAfterAtk = 2f;
public float attackDamage = 10; // danno d'attacco
public float sightRadius = 5f; // raggio di vista del nemico
public float chaseThreshold = 2f; // soglia di distanza per iniziare l'inseguimento
public float attackrange = 2f;
public float attackCooldown = 2f; // durata del cooldown dell'attacco
private float attackTimer;
public float InvincibleTime = 1f;

[Header("Abilitations")]
private bool isChasing = false; // indica se il nemico sta inseguendo il player
private bool isMove = false;
private bool isAttacking = false;
private bool isKnockback = false;
private bool isHurt = false;
private bool isDie = false;
private bool pauseAtck = false;
private bool canAttack = true;
private bool firstattack = true;
private bool isPlayerInAttackRange = false;
private bool activeActions = true;
private bool OneDie = false;
public bool isSmall = false;
private float waitTimer = 0f;
private float waitDuration = 2f;

[Header("Knockback")]
    private bool kb = false;
    public float knockbackForce; // la forza del knockback
    public float knockbackTime; // il tempo di knockback
    public float jumpHeight; // l'altezza del salto
    public float fallTime; // il tempo di caduta

 [Header("Audio")]
 [HideInInspector] public float basePitch = 1f;
    [HideInInspector] public float randomPitchOffset = 0.1f;
[SerializeField] public AudioClip[] listmusic; // array di AudioClip contenente tutti i suoni che si vogliono riprodurre
private AudioSource[] bgm; // array di AudioSource che conterrà gli oggetti AudioSource creati
   public AudioMixer SFX;

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
    [SerializeField] GameObject Sdeng;



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
    
    private string currentAnimationName;
    public SkeletonAnimation _skeletonAnimation;
    private Spine.AnimationState _spineAnimationState;
    private Spine.Skeleton _skeleton;
    Spine.EventData eventData;

private enum State { Moving, Chase, Attack, Knockback, Dead, Hurt, Wait }
private State currentState;

public static AiEnemysword instance;
    

    private void Awake()
    {
    rb = GetComponent<Rigidbody2D>();
    health = GetComponent<Health>();
    _skeletonAnimation = GetComponent<SkeletonAnimation>();
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
    bgm = new AudioSource[listmusic.Length]; // inizializza l'array di AudioSource con la stessa lunghezza dell'array di AudioClip
    for (int i = 0; i < listmusic.Length; i++) // scorre la lista di AudioClip
    {
        bgm[i] = gameObject.AddComponent<AudioSource>(); // crea un nuovo AudioSource come componente del game object attuale (quello a cui è attaccato lo script)
        bgm[i].clip = listmusic[i]; // assegna l'AudioClip corrispondente all'AudioSource creato
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
            Damage(10);
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
            }
        }
    }


    private void CheckState()
{
    if (Move.instance.isDeath) 
    {
return; // esce immediatamente se il personaggio è morto
}

if (health.currentHealth <= 0) { // controlla se il personaggio è morto
    isChasing = false;
    isAttacking = false;
    isMove = false;
    activeActions = false;
    isDie = true;
    SpawnCoins();
    Die();
    currentState = State.Dead;
} else if (isKnockback) { // controlla se il personaggio è in knockback
    isChasing = false;
    isAttacking = false;
    isMove = false;
    currentState = State.Knockback;
} else if (isHurt) { // controlla se il personaggio è stato colpito
    isChasing = false;
    isAttacking = false;
    isMove = false;
    activeActions = false;
    currentState = State.Hurt;
} else if (Vector2.Distance(transform.position, player.position) < attackrange) { // controlla se il personaggio è dentro il raggio d'attacco
    isChasing = false;
    isAttacking = true;
    isMove = false;
    isPlayerInAttackRange = true;
    currentState = State.Attack;
} else if (Vector2.Distance(transform.position, player.position) > attackrange && isPlayerInAttackRange) { // controlla se il personaggio è appena uscito dal raggio e si avvia il timer di attesa
    isChasing = false;
    isAttacking = false;
    isMove = false;
    activeActions = false;
    currentState = State.Wait;
    StartCoroutine(waitChase());
} else if (Vector2.Distance(transform.position, player.position) < chaseThreshold && !isPlayerInAttackRange) { // controlla se il personaggio è nel raggio di inseguimento
    isChasing = true;
    isAttacking = false;
    isMove = false;
    firstattack = true;
    currentState = State.Chase;
} else if (Physics2D.Raycast(transform.position, transform.TransformDirection(Vector3.up), 5f, playerlayer)) { // controlla se il personaggio è bloccato da un ostacolo
    isChasing = false;
    isAttacking = false;
    isMove = false;
    currentState = State.Wait;
} else if (activeActions) { // controlla se il personaggio può muoversi in autonomia
    isMove = true;
    isChasing = false;
    isAttacking = false;
    currentState = State.Moving;
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

// Controlla se l'oggetto deve essere in movimento, il rigedbody è in KInematic
private void Moving()
{
    if (isMove && !isAttacking)
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


private void Chase()
{
    if(isChasing && !isAttacking)
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

        if (canAttack && Vector2.Distance(transform.position, player.position) < attackrange) // Se l'attacco è disponibile e il personaggio è abbastanza vicino al giocatore...
        {
            Attack1Anm(); // ...esegui l'animazione dell'attacco...
            attackTimer = attackCooldown; // ...e reimposta il timer dell'attacco.
        }
    }
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
private IEnumerator JumpBackCo(Rigidbody2D rb)
    {

        if (rb != null)
        {
            Vector2 knockbackDirection = new Vector2(0f, jumpHeight); // direzione del knockback verso l'alto
            if (rb.transform.position.x < player.transform.position.x) // se la posizione x del nemico è inferiore a quella del player
                knockbackDirection = new Vector2(-1, jumpHeight); // la direzione del knockback è verso sinistra
            else if (rb.transform.position.x > player.transform.position.x) // se la posizione x del nemico è maggiore a quella del player
                knockbackDirection = new Vector2(1, jumpHeight); // la direzione del knockback è verso destra
            rb.AddForce(knockbackDirection * knockbackForce, ForceMode2D.Impulse); // applica il knockback
            yield return new WaitForSeconds(knockbackTime); // aspetta il tempo di knockback
            isKnockback = false;

        }
    }

void EssenceGive()
{
    int randomChance = Random.Range(1, 11); // Genera un numero casuale compreso tra 1 e 10

    if (randomChance <= 8) // Se il numero casuale è compreso tra 1 e 8 (80% di probabilità), aggiungi 5 di essenza
    {
        PlayerHealth.Instance.currentEssence += 5;
    }
    else // Se il numero casuale è compreso tra 9 e 10 (20% di probabilità), aggiungi 10 di essenza
    {
        PlayerHealth.Instance.currentEssence += 10;
    }
}
    


private bool IsKnockback()
{
    // controllo se il nemico è in knockback
    StartCoroutine(JumpBackCo(rb));
    return isKnockback = false;
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

    health.currentHealth -= damage;
    TemporaryChangeColor(Color.red);
    Instantiate(Sdeng, hitpoint.position, transform.rotation);
    PlayMFX(1);


    if (isSmall)
    {
        HurtAnm();
        isKnockback = true;
        IsKnockback();
        currentState = State.Hurt;
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
            Random.Range(-coinForceVariance.x, coinForceVariance.x), 2// forza casuale lungo l'asse Y
            );
        newCoin.GetComponent<Rigidbody2D>().AddForce(randomForce * coinForce, ForceMode2D.Impulse);
    }
        SpawnC = true;
    }
}




public void Die()
{
    PlayMFX(2);
    //Drop
    // genera un numero casuale di monete da rilasciare
   // int coinsToSpawn = Random.Range(1, 10);
    //coinsToSpawn = Mathf.Min(maxCoins, coinsToSpawn); //Limita il numero di monete a "maxCoins" 
    EssenceGive();
    if(GameplayManager.instance.ordalia)
    {
        if(!OneDie)
        {
        GameplayManager.instance.EnemyDefeat();
        OneDie = true;
        }
       //TriggerOrdalia.instance.EnemyDefet();
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
        DieBack();

    }

    StartCoroutine(DestroyAfterDeath());
}

private IEnumerator DestroyAfterDeath()
{
    
    yield return new WaitForSeconds(timeBeforeDestroying);
    Destroy(gameObject);
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

                   // Debug.Log("Combo Count: " + comboCount + ", Playing Animation: combo_1");
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

                   // Debug.Log("Combo Count: " + comboCount + ", Playing Animation: combo_1");
                }
                // Add event listener for when the animation completes
                //_spineAnimationState.GetCurrent(1).Complete += OnAttackAnimationComplete;
}
public void Attack1Anm()
{
            
   
    if (currentAnimationName != attack1AnimationName)
                {
                    _spineAnimationState.SetAnimation(2, attack1AnimationName, false);
                    currentAnimationName = attack1AnimationName;
                    _spineAnimationState.Event += HandleEvent;
                    //attack.gameObject.SetActive(true);
                    // StartCoroutine(VFXCont());
                   // Debug.Log("Combo Count: " + comboCount + ", Playing Animation: combo_1");
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
                    
                   // Debug.Log("Combo Count: " + comboCount + ", Playing Animation: combo_1");
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

                   // Debug.Log("Combo Count: " + comboCount + ", Playing Animation: combo_1");
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

                   // Debug.Log("Combo Count: " + comboCount + ", Playing Animation: combo_1");
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

                   // Debug.Log("Combo Count: " + comboCount + ", Playing Animation: combo_1");
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

                   // Debug.Log("Combo Count: " + comboCount + ", Playing Animation: combo_1");
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

                   // Debug.Log("Combo Count: " + comboCount + ", Playing Animation: combo_1");
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

     // Reset the attack state
    //isAttacking = false;
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


IEnumerator VFXCont()
{   
    yield return new WaitForSeconds(0.5f);
    attack_h.gameObject.SetActive(false);
    attack_B.gameObject.SetActive(false);
    attack.gameObject.SetActive(false);

}


void HandleEvent (TrackEntry trackEntry, Spine.Event e) {

if (e.Data.Name == "VFXslash") {
        // Inserisci qui il codice per gestire l'evento.
        //Instantiate(attack, slashpoint.position, transform.rotation);
        attack.gameObject.SetActive(true);
                    StartCoroutine(VFXCont());
       PlayMFX(0);
    }

if (e.Data.Name == "VFXslash_h") {
        // Inserisci qui il codice per gestire l'evento.
        attack_h.gameObject.SetActive(true);
                    StartCoroutine(VFXCont());
        PlayMFX(0);
    }
if (e.Data.Name == "VFXSlashB") {
        // Inserisci qui il codice per gestire l'evento.
        attack_B.gameObject.SetActive(true);
                    StartCoroutine(VFXCont());
        PlayMFX(0);
    }
    if (e.Data.Name == "attack") {
        // Inserisci qui il codice per gestire l'evento.
        if(horizontal == 1)
        {
        transform.position += transform.right * atckForward * Time.deltaTime; //sposta il nemico in avanti
        } else if(horizontal == 1)
        {
        transform.position += transform.right * -atckForward * Time.deltaTime; //sposta il nemico in avanti
        } 

    }

   
}


}