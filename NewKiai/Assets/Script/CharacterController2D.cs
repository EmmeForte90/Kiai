using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;
using Spine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;

public class CharacterController2D : MonoBehaviour
{
    [Header("Move")]

    [SerializeField] public float moveSpeed = 5f; // velocità di movimento
    [SerializeField] public float jumpForce = 5f; // forza del salto
    [SerializeField] public float runMultiplier = 2f; // moltiplicatore di velocità per la corsa
    private int jumpCounter = 0;
    private float jumpDuration = 2f;

    public float wallJumpForce = 10f; // La forza da applicare per il Wall Jump
    public float wallJumpDelay = 0.2f; // Il ritardo in secondi prima di poter fare il Wall Jump di nuovo
    private float wallJumpTimer = 0f; // Il timer per il ritardo del Wall Jump
    private bool isWallJumping = false; // Il flag per indicare se il personaggio sta facendo il Wall Jump

    private bool isWallSliding = false; // il personaggio sta scivolando sul muro
    private bool isTouchingWall = false; // il personaggio sta toccando il muro
    private int wallDirection = 0; // direzione del muro (1 per muro a destra, -1 per muro a sinistra)
    public float wallSlideSpeed = 2f; // La velocità di scorrimento lungo la parete



    private int maxJumps = 2;
    public float fallMultiplier = 2.5f;
    [SerializeField] public float jumpHeight = 2f;
    float coyoteTime = 0.1f;
    float coyoteCounter = 0f;
    public LayerMask groundLayer;
    public float groundCheckDistance = 0.1f;
    private readonly Vector3 raycastColliderOffset = new (0.25f, 0, 0);
    private const float distanceFromGroundRaycast = 0.3f;



    Vector2 playerPosition;
    Vector2 HitPosition;
    public GameObject Hit;

    [Header("HP")]
    [SerializeField]public float health = 100f; // salute del personaggio
    PlayerHealth Less;


    [Header("Respawn")]
    //[HideInInspector]
    private Transform respawnPoint; // il punto di respawn del giocatore
    public string sceneName; // il nome della scena in cui si trova il punto di respawn
    
    [Header("Animations")]
    private Animator anim; // componente Animator del personaggio
    private int state = 0;

    [Header("Attacks")]
    private float currentCooldown; // contatore del cooldown attuale
    [SerializeField] float nextAttackTime = 0f;
    [SerializeField] float attackRate = 2f;
    [SerializeField] public float attackCooldown = 0.5f; // tempo di attesa tra gli attacchi
    [SerializeField] public float comboTimer = 2f; // tempo per completare una combo
    [SerializeField] public int comboCounter = 0; // contatore delle combo
    [SerializeField] public int maxCombo = 3; // numero massimo di combo
    [SerializeField] public float shootTimer = 2f; // tempo per completare una combo
    [SerializeField] private GameObject bullet;
    public float maxChargeTime = 3f; // Tempo massimo di carica in secondi
    public float maxDamage = 30f; // Danno massimo dell'attacco
    private float chargeTime;
    private bool isCharging;
    public Transform slashpoint;
    public int facingDirection = 1; // La direzione in cui il personaggio sta guardando: 1 per destra, -1 per sinistra

    [Header("VFX")]
    // Variabile per il gameobject del proiettile
    [SerializeField] GameObject blam;
    [SerializeField] public Transform gun;
    [SerializeField] GameObject Circle;
    [SerializeField] public Transform circlePoint;

    [Header("Abilitations")]
    [SerializeField] public GameplayManager gM;
    private bool IsKnockback = false;
    private bool stopInput = false;
    private bool isGrounded = false; // vero se il personaggio sta saltando
    private bool isJumping = false; // vero se il personaggio sta saltando
    private bool isFall = false; // vero se il personaggio sta saltando
    private bool isHurt = false; // vero se il personaggio sta saltando
    private bool isLoop = false; // vero se il personaggio sta saltando
    private bool isAttacking = false; // vero se il personaggio sta attaccando
    private bool isLanding = false; // vero se il personaggio sta attaccando
    private bool isRunning = false; // vero se il personaggio sta correndo
    private float currentSpeed; // velocità corrente del personaggio
   

    private Rigidbody2D rb; // componente Rigidbody2D del personaggio

    [SerializeField] public static bool playerExists;
    [SerializeField] public bool blockInput = false;
   
    public SkeletonMecanim skeletonM;
    public float moveX;
[Header("Audio")]
[SerializeField] AudioSource SwSl;
[SerializeField] AudioSource Smagic;
[SerializeField] AudioSource SRun;



public static CharacterController2D instance;
public static CharacterController2D Instance
        {
            //Se non trova il componente lo trova in automatico
            get
            {
                if (instance == null)
                    instance = GameObject.FindObjectOfType<CharacterController2D>();
                return instance;
            }
        }

    void Start()
    {
        isGrounded = true; // vero se il personaggio sta saltando
        playerPosition = transform.position;
        HitPosition = Hit.transform.position;
        Less = GetComponent<PlayerHealth>();
        rb = GetComponent<Rigidbody2D>();
        if (gM == null)
        {
            gM = GetComponent<GameplayManager>();
        }
        SetState(0);
        anim = GetComponent<Animator>();
        currentCooldown = attackCooldown;
        
        if (playerExists) {
        Destroy(gameObject);
    }
    else {
        playerExists = true;
        DontDestroyOnLoad(gameObject);
    }


    }

    void Update()
    {
         

        if (Less.currentHealth <= 0)
        {
            Respawn();
        }

        if(!gM.PauseStop || IsKnockback)
        {

#region  Move
        moveX = Input.GetAxis("Horizontal");


        if (state == 0)
        {
        if (moveX != 0)
        { 
        SetState(1);
        }else if (moveX == 0)
        {
        SetState(0);

        }
        }
        
        currentSpeed = moveSpeed;
        if (isRunning && !isAttacking)
        {
            if (moveX != 0)
    { 
    if (isRunning)
    {
        currentSpeed = moveSpeed * runMultiplier;
        SetState(2);
    }
    else
    {
        currentSpeed = moveSpeed;
        SetState(1);
    }
    }
    else
    {
    currentSpeed = 0;
    SetState(0);
    }


    
/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        }
        if (isAttacking || isLanding)
        {   
        rb.velocity = new Vector2(0f, 0f);
        }else
        {
        rb.velocity = new Vector2(moveX * currentSpeed, rb.velocity.y);
        }

        Flip();
#endregion


// gestione dell'input dello sparo
if (Input.GetButtonDown("Fire2"))
{
Blast();   
}


  // Utilizzare un raycast per determinare se il personaggio è su un terreno solido o meno
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, groundCheckDistance, groundLayer);
        isGrounded = hit.collider != null;

        // Se il personaggio non è più su un terreno solido, aumentare il contatore coyote
        if (!isGrounded)
        {
            coyoteCounter += Time.deltaTime;
        }else
        {
            isFall = false;
            isJumping = false;
            
        }

        // Se il pulsante di salto viene premuto e il personaggio ha ancora salti disponibili o sta ancora entro il tempo di coyote, saltare
if ((Input.GetButtonDown("Jump") && (jumpCounter < maxJumps || coyoteCounter < coyoteTime)) || isWallJumping)
{
    if (isWallJumping)
    {
        // Resetta il flag per indicare che il personaggio non sta facendo il Wall Jump
        isWallJumping = false;
    }
    else
    {
        // Controlla se il personaggio sta toccando un muro
        isTouchingWall = Physics2D.Raycast(transform.position, Vector2.right, transform.localScale.x, LayerMask.GetMask("Wall"))
            || Physics2D.Raycast(transform.position, Vector2.left, transform.localScale.x, LayerMask.GetMask("Wall"));

        // Se il personaggio sta toccando un muro, fai il Wall Jump
        if (isTouchingWall)
        {
            SetState(15);
            // Imposta il flag per indicare che il personaggio sta facendo il Wall Jump
            isWallJumping = true;
            isJumping = false;
            isFall = false;

            // Imposta il salto massimo a 1 per evitare il doppio salto durante il Wall Jump
            maxJumps = 1;

            // Imposta il ritardo per il Wall Jump a 0 per avere una risposta immediata
            wallJumpDelay = 0f;

            // Applica la forza per il Wall Jump
            rb.velocity = new Vector2(0f, 0f);
            rb.AddForce(new Vector2(wallJumpForce * -facingDirection, wallJumpForce), ForceMode2D.Impulse);

            // Cambia la direzione del personaggio
            Flip();

            // Avvia la coroutine per il Wall Jump
            StartCoroutine(WallJumpCoroutine());
        }
        else
        {
            // Altrimenti, fai il salto normale
            isJumping = true;
            SetState(3);
            jumpCounter++;
            rb.velocity = Vector2.up * Mathf.Sqrt(jumpHeight * -2f * Physics2D.gravity.y);

            if (jumpCounter == 2)
            {
                SetState(3);
                Smagic.Play();
                Instantiate(Circle, circlePoint.transform.position, transform.rotation);
            }

            StartCoroutine(JumpDurationCoroutine(jumpDuration));
        }
    }
}

// Se la velocità verticale del personaggio è negativa, attivare la modalità di caduta
if (rb.velocity.y < 0)
{
    isFall = true;
    isLoop = true;
    rb.velocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
}

// Se viene premuto il pulsante di attacco e il personaggio sta saltando, attaccare
if (Input.GetButtonDown("Fire1") && isJumping)
{
    isAttacking = true;
    isJumping = false;
    isFall = true;
    isLoop = true;
}

// Se il personaggio sta saltando o facendo il Wall Jump, aggiornare lo stato
if (isJumping || isWallJumping)
{
    if (Input.GetButtonDown("Fire1"))
    {
        Attack();
        isAttacking = true;
        isJumping = false;
    }
    else
    {
        SetState(4);
        isLoop = true;
    }
}

if (!isGrounded && !isAttacking && !isJumping && !isWallJumping)
{
SetState(12);
isLoop = true;
}



        if (Input.GetButtonDown("Fire1"))
        {
            Attack();
        }
            shootTimer -= Time.deltaTime;
            if (shootTimer <= 0)
            {
                isAttacking= false;
                shootTimer = 0.5f;
            }
        // gestione del timer della combo
        if (comboCounter > 0)
        {
            comboTimer -= Time.deltaTime;
            if (comboTimer <= 0)
            {
                isAttacking= false;
                comboCounter = 0;
                comboTimer = 0.5f;
            }
        }

        // gestione del cooldown dell'attacco
        if (currentCooldown > 0)
        {
            currentCooldown -= Time.deltaTime;
        }

        // gestione dell'input della corsa
        if (Input.GetButton("Fire3"))
        {
            isRunning = true;
        }
        else
        {
            isRunning = false;
        }

if (Input.GetKey(KeyCode.X) && !isCharging)
        {
            isCharging = true;
            chargeTime = 0f;
            SetState(13);

            //animator.Play(chargeAnimation.name);
        }

        if (Input.GetKey(KeyCode.X) && isCharging)
        {
            chargeTime += Time.deltaTime;

            if (chargeTime > maxChargeTime)
            {
                chargeTime = maxChargeTime;
            }
        }

        if (Input.GetKeyUp(KeyCode.X) && isCharging)
        {
            float chargeRatio = chargeTime / maxChargeTime;
            float damage = maxDamage * chargeRatio;
            Debug.Log("Charge ratio: " + chargeRatio + ", Damage: " + damage);
            SetState(14);

            //animator.Play(attackAnimation.name);
            isCharging = false;
        }



        // gestione dell'animazione del personaggio
        UpdateAnimation();
        }

        
// gestione dell'input del Menu 
        if (Input.GetButtonDown("Pause") && !stopInput)
        {
            gM.Pause();
            stopInput = true;
            //myAnimator.SetTrigger("idle");
            //SFX.Play(0);
            rb.velocity = new Vector2(0f, 0f);
        }
        else if(Input.GetButtonDown("Pause") && stopInput)
        {
            gM.Resume();
            stopInput = false;
        }
    }

void Flip()
{
    if (moveX < 0)
        {
            moveX = -1;
        transform.localScale = new Vector2(-1f, 1f);
        }
        else if (moveX > 0)
        {
            moveX = 1;
        transform.localScale = new Vector2(1f, 1f);
        }
}

    void UpdateAnimation()
    {
        switch (state)
        {
            case 0:
                anim.Play("Gameplay/idle");
                //anim.Play("Gameplay/idle");
                break;
            case 1:
                anim.Play("Gameplay/walk");
                break;
            case 2:
                anim.Play("Gameplay/run");
                break;
            case 3:
            if(isJumping && !isTouchingWall)
            {
                anim.Play("Gameplay/jump_start");
            }
                //state = 4;
                break;
            case 4:
                anim.Play("Gameplay/jump");
                break;
            case 5:
                anim.Play("Gameplay/landing");
                //state = 0;
                break;
            case 6:
                anim.Play("Gameplay/blast");
                break;
            case 7:
                anim.Play("CS/attack");
                break;
            case 8:
                anim.Play("CS/attack_h");
                break;
            case 9:
                anim.Play("CS/attack_l");
                break;
            case 10:
                anim.Play("Gameplay/hurt");
                break;
            case 11:
                anim.Play("Gameplay/die");
                break;
            case 12:
            if(isFall && !isTouchingWall)
            {
                anim.Play("Gameplay/fall");
            }
                break;
            case 13:
                anim.Play("Attack/ATTACK-COUNTERGUARD");
                break;
            case 14:
                anim.Play("Attack/ATTACK-DASHLUNGE");
                break;
            case 15:
            if(isTouchingWall && state == 4 || state == 12)
            {
                anim.Play("Gameplay/wallslidinghook");
            }
                break;
        }
        var currentAnimInfo = anim.GetCurrentAnimatorStateInfo(0);
    if(!isLoop)
    {
    if (currentAnimInfo.normalizedTime >= 1)
    {
        state = 0;
    }
    }
    }
//////////////////////////////////////////////////////////////////////////////////////////////////////////////

private void FixedUpdate()
    {
        // Controlla se il personaggio sta toccando un muro
        isTouchingWall = Physics2D.Raycast(transform.position, Vector2.right, transform.localScale.x, LayerMask.GetMask("Wall")) 
            || Physics2D.Raycast(transform.position, Vector2.left, transform.localScale.x, LayerMask.GetMask("Wall"));

        // Se il personaggio sta toccando il muro, controlla la direzione del muro
        if (isTouchingWall)
        {
            wallDirection = Physics2D.Raycast(transform.position, Vector2.right, transform.localScale.x, LayerMask.GetMask("Wall")) ? 1 : -1;
        }

        // Se il personaggio sta scivolando sul muro, applica la forza per lo scivolamento
        if (isWallSliding)
        {
            rb.velocity = new Vector2(rb.velocity.x, -wallSlideSpeed);
        }
    }

 IEnumerator WallJumpCoroutine()
    {
        // Aspetta il ritardo prima di poter fare il Wall Jump di nuovo
yield return new WaitForSeconds(wallJumpDelay);
// Resetta il salto massimo e il timer per il ritardo del Wall Jump
maxJumps = 2;
wallJumpTimer = 0f;

yield return null;
    }

 private IEnumerator JumpDurationCoroutine(float duration)
{
    float timer = 0f;
    while (timer < duration)
    {
        timer += Time.deltaTime;
        yield return null;
    }

    isJumping = false;
}
 public void SetState(int newState)
    {
        state = newState;
    }

void Blast()
{
    //if(Less.currentMana > 0)
      //  {
if (Time.time > nextAttackTime)
        {
        isAttacking = true;
        nextAttackTime = Time.time + 1f / attackRate;
        Smagic.Play();
        SetState(6);
        Instantiate(blam, gun.position, transform.rotation);
        Instantiate(bullet, gun.position, transform.rotation);
        }
        //}
}
    void Attack()
    {
        if (currentCooldown <= 0)
        {
            isAttacking = true;
            comboCounter++;
            if (comboCounter > maxCombo)
            {
                comboCounter = 1;
            }
            if (comboCounter == 1)
            {
                        SetState(7);
            }else if (comboCounter == 2)
            {
                        SetState(8);
            }else if (comboCounter == 3)
            {
                        SetState(9);
            }
            currentCooldown = attackCooldown;
            comboTimer = 0.5f;
            
        }
        
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true; // vero se il personaggio sta saltando
            isJumping = false;
            jumpCounter = 0;
            SetState(5);
            StartCoroutine(stopPlayer());

        }

    }

private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Respawn"))
        {
            RespawnObject respawnObject = collision.GetComponent<RespawnObject>();
            if (respawnObject != null)
            {
                respawnPoint = respawnObject.respawnPoint;
                sceneName = respawnObject.sceneName;
            }
        }


//Test per gestire il respawn
        if (collision.CompareTag("EditorOnly"))
        {
           Respawn();
        }
    }



IEnumerator stopPlayer()
{
isLanding = true; 
yield return new WaitForSeconds(0.5f);
isLoop = false;
isLanding = false;    
}

public void AnmHurt()
{
            SetState(10);
}

#region CambioMagia
    public void SetBulletPrefab(GameObject newBullet)
    //Funzione per cambiare arma
    {
       bullet = newBullet;
    }    
    
#endregion



    public void SoundSlash()
    {
        SwSl.Play();
    } 



public void Respawn()
{
    // Cambia la scena
    SceneManager.LoadScene(sceneName);

    // Aspetta che la nuova scena sia completamente caricata
    StartCoroutine(WaitForSceneLoad());
}
private bool onsGrounded()
    {
        //DOUBLE RAYCAST FOR GROUND: check if you touch the ground even with just one leg 
        return (
                Physics2D.Raycast(transform.position + raycastColliderOffset, Vector3.down, distanceFromGroundRaycast, groundLayer)
                ||
                Physics2D.Raycast(transform.position - raycastColliderOffset, Vector3.down, distanceFromGroundRaycast, groundLayer)
            );
    }
IEnumerator WaitForSceneLoad()
{
    yield return new WaitForSeconds(0);

    // Trova l'oggetto con il tag "respawn" nella nuova scena
    GameObject respawnPoint = GameObject.FindWithTag("Respawn");

    // Teletrasporta il giocatore alla posizione dell'oggetto "respawn"
    transform.position = respawnPoint.transform.position;
}
}



           


