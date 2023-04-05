using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;
using Spine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;


public class Move : MonoBehaviour
{
    [SerializeField] public GameObject Player;


    [Header("Movement")]
    [SerializeField] private float speed;
    [SerializeField] private float acceleration;
    [SerializeField] private float deceleration;
    public float Test;
    public float InvincibleTime = 1f;
    [HideInInspector] public bool isHurt = false;
    [HideInInspector] public bool isBump = false;

    [HideInInspector] public float horDir;
    [HideInInspector] public float vertDir;
    [HideInInspector] public float DpadX;//DPad del joypad per il menu rapido
    [HideInInspector] public float DpadY;//DPad del joypad per il menu rapido
    public float L2;
    public float R2;

    public float runSpeedThreshold = 5f; // or whatever value you want
    [Header("Dash")]
    public float dashForce = 50f;
    public float dashDuration = 0.5f;
    private float dashTime;
    private bool dashing;
    private bool Atkdashing;
    private float dashForceAtk = 10f;
    private float upperForceAtk = 0.5f;
    private bool attackNormal;
    private bool attackUpper;
    public float dashCoolDown = 1f;
    private float coolDownTime;

    [Header("Jump")]
    [SerializeField] private float jumpForce;
    [SerializeField] private float bumpForce;
    [SerializeField] private float knockForce;
    bool canDoubleJump = false;
    public float groundDelay = 0.1f; // The minimum time before the player can jump again after touching the ground
    bool isTouchingWall = false;
    public LayerMask wallLayer;         // layer del muro
    public float wallJumpForce = 7f;    // forza del walljump
    public float wallSlideSpeed = 1f;   // velocità di scivolamento lungo il muro
    public float wallDistance = 0.5f;   // distanza dal muro per effettuare il walljump
    public bool canWallJump = false;
    bool wallJumped = false;


    
    float coyoteCounter = 0f;

    //COYOTE TIME: can jump for a short time after leave ground
    [SerializeField] private float coyoteTime;
    private float lastTimeGround;
    
    //JUMP DELAY: avoid jump only when touch ground
    [SerializeField] private float jumpDelay;
    private float lastTimeJump;

    [SerializeField] private float gravityOnJump;
    [SerializeField] private float gravityOnFall;
    
    private readonly Vector3 raycastColliderOffset = new (0.25f, 0, 0);
    private const float distanceFromGroundRaycast = 0.3f;
    [SerializeField] private LayerMask groundLayer;
   
    [HideInInspector] public bool slotR,slotL,slotU,slotB = false;
    [Header("Respawn")]
    //[HideInInspector]
    private Transform respawnPoint; // il punto di respawn del giocatore
    public string sceneName; // il nome della scena in cui si trova il punto di respawn

    [Header("VFX")]
    // Variabile per il gameobject del proiettile
    [SerializeField] GameObject blam;
    [SerializeField] public Transform gun;
    [SerializeField] public Transform top;
    [SerializeField] GameObject Circle;
    [SerializeField] public Transform circlePoint;
    [SerializeField] public Transform slashpoint;
    [SerializeField] GameObject attack;
    [SerializeField] GameObject attack_h;
    [SerializeField] GameObject attack_l;
    [SerializeField] GameObject attack_a;
    [SerializeField] GameObject pesante;
    [SerializeField] GameObject charge;
    [SerializeField] GameObject attack_air_bottom;
    [SerializeField] GameObject attack_air_up;
    [SerializeField] GameObject swordRain;
    [SerializeField] GameObject VFXHeal;
    [SerializeField] GameObject StopHeal;



    
   
    [Header("Animations")]
    [SpineAnimation][SerializeField] private string idleAnimationName;
    [SpineAnimation][SerializeField] private string walkAnimationName;
    [SpineAnimation][SerializeField] private string runAnimationName;
    [SpineAnimation][SerializeField] private string jumpAnimationName;
    [SpineAnimation][SerializeField] private string jumpDownAnimationName;
    [SpineAnimation][SerializeField] private string landingAnimationName;
    [SpineAnimation][SerializeField] private string walljumpAnimationName;
    [SpineAnimation][SerializeField] private string walljumpdownAnimationName;
    [SpineAnimation][SerializeField] private string dashAnimationName;
    [SpineAnimation][SerializeField] private string talkAnimationName;
    //////////////////////////////////////////////////////////////////////////
    [SpineAnimation][SerializeField] private string hurtAnimationName;
    [SpineAnimation][SerializeField] private string HealAnimationName;
    [SpineAnimation][SerializeField] private string HealEndAnimationName;
    [SpineAnimation][SerializeField] private string deathAnimationName;
    [SpineAnimation][SerializeField] private string RestAnimationName;
    [SpineAnimation][SerializeField] private string respawnRestAnimationName;
    [SpineAnimation][SerializeField] private string UpAnimationName;
    [SpineAnimation][SerializeField] private string respawnAnimationName;
    ///////////////////////////////////////////////////////////////////////////
    [SpineAnimation][SerializeField] private string attackAnimationName;
    [SpineAnimation][SerializeField] private string attack_lAnimationName;
    [SpineAnimation][SerializeField] private string attack_hAnimationName;
    [SpineAnimation][SerializeField] private string attack_aAnimationName;
    [SpineAnimation][SerializeField] private string chargeAnimationName;
    [SpineAnimation][SerializeField] private string pesanteAnimationName;
    [SpineAnimation][SerializeField] private string upatkjumpAnimationName;
    [SpineAnimation][SerializeField] private string downatkjumpAnimationName;
    /////////////////////////////////////////////////////////////////////
    [SpineAnimation][SerializeField] private string blastAnimationName;
    [SpineAnimation][SerializeField] private string bigblastAnimationName;
    [SpineAnimation][SerializeField] private string TornadoAnimationName;
    [SpineAnimation][SerializeField] private string SwordrainAnimationName;
    [SpineAnimation][SerializeField] private string multilungeAnimationName;
    [SpineAnimation][SerializeField] private string DashAttackAnimationName;
    [SpineAnimation][SerializeField] private string evocationAnimationName;
    [SpineAnimation][SerializeField] private string throwAnimationName;
    [SpineAnimation][SerializeField] private string upperAnimationName;
    [SpineAnimation][SerializeField] private string dashsawAnimationName;
    [SpineAnimation][SerializeField] private string SawinAnimationName;
    [SpineAnimation][SerializeField] private string SpecialAnimationName;






    
    
    
    


private string currentAnimationName;

//private CharacterState currentState = CharacterState.Idle;
private int comboCount = 0;
     

    [Header("Attacks")]
    public int Damage;
    [SerializeField] public int comboCounter = 0; // contatore delle combo
    [SerializeField] float nextAttackTime = 0f;
    [SerializeField] float attackRate = 0.5f;
    [SerializeField] public float shootTimer = 2f; // tempo per completare una combo
    [SerializeField] private GameObject bullet;
    // Dichiarazione delle variabili
    private int currentTime;
    private int timeLimit = 3; // Tempo massimo per caricare l'attacco
    private int maxDamage = 50; // Danno massimo dell'attacco caricato
    private int minDamage = 10; // Danno minimo dell'attacco non caricato
    private float timeSinceLastAttack = 0f;
    [HideInInspector]public bool isCharging;
    private bool touchGround;
    private bool isDashing;
    [HideInInspector]public bool isPray;//DPad del joypad per il menu rapido
    [HideInInspector]public bool isHeal;
    [HideInInspector]public bool isDeath;
    [HideInInspector]public bool isAttacking = false; // vero se il personaggio sta attaccando
    private bool isAttackingAir = false; // vero se il personaggio sta attaccando
    private bool isBlast = false; // vero se il personaggio sta attaccando

    public bool stopInput = false;
    public bool NotStrangeAnimationTalk = false;

    private int facingDirection = 1; // La direzione in cui il personaggio sta guardando: 1 per destra, -1 per sinistra
    
    [Header("Audio")]
    public float basePitch = 1f;
    public float randomPitchOffset = 0.1f;
    [SerializeField] AudioSource SwSl;
    [SerializeField] AudioSource Smagic;
    [SerializeField] AudioSource Swalk;
    [SerializeField] AudioSource Scharge;
    [SerializeField] AudioSource Sdash;
    [SerializeField] AudioSource SHeal;
    [SerializeField] AudioSource SHurt;



    private SkeletonAnimation _skeletonAnimation;
    private Spine.AnimationState _spineAnimationState;
    private Spine.Skeleton _skeleton;
    Spine.EventData eventData;



    public Rigidbody2D rb;

public static Move instance;


    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }

_skeletonAnimation = GetComponent<SkeletonAnimation>();
if (_skeletonAnimation == null) {
    Debug.LogError("Componente SkeletonAnimation non trovato!");
}        rb = GetComponent<Rigidbody2D>();
       
        _spineAnimationState = GetComponent<Spine.Unity.SkeletonAnimation>().AnimationState;
        _spineAnimationState = _skeletonAnimation.AnimationState;
        _skeleton = _skeletonAnimation.skeleton;
        rb = GetComponent<Rigidbody2D>();
    }
    
    private void Update()
    {

//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
if(!stopInput)
        {
        if(!isDeath)
        {
        if(!isHeal)
        {
        horDir = Input.GetAxisRaw("Horizontal");
        vertDir = Input.GetAxisRaw("Vertical");
        DpadX = Input.GetAxis("DPad X");
        DpadY = Input.GetAxis("DPad Y");
        L2 = Input.GetAxis("L2");
        R2 = Input.GetAxis("R2");

        }
        if (isGrounded())
        {
            //Debug.Log("isGrounded(): " + isGrounded());
            lastTimeGround = coyoteTime; 
            isAttackingAir = false;
            canDoubleJump = true;
        
            rb.gravityScale = 1;
        }
        else
        {
            lastTimeGround -= Time.deltaTime;
            modifyPhysics();
        }
//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

 // Controllo se il personaggio è a contatto con un muro
 if( GameplayManager.instance.unlockWalljump)
 {
        Vector2 direction = transform.localScale.x > 0 ? Vector2.right : Vector2.left;
isTouchingWall = Physics2D.Raycast(transform.position, direction, wallDistance, wallLayer);
 }

if (Input.GetButtonDown("Jump"))
{
            lastTimeJump = Time.time + jumpDelay;

        //Pre-interrupt jump if button released
        if (Input.GetButtonUp("Jump") && rb.velocity.y > 0)
        {
            lastTimeGround = 0; //Avoid spam button
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.5f);   
        }
    
    if (canDoubleJump && GameplayManager.instance.unlockDoubleJump)
    {
        // Double jump
        lastTimeJump = Time.time + jumpDelay;
        rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        Instantiate(Circle, circlePoint.position, transform.rotation);

        if(isTouchingWall)
        {
        canDoubleJump = true;
        wallSlide();
        }else 
        {
        canDoubleJump = false;
        }
    }
}



// Wallslide
        if (isTouchingWall && !isGrounded() && rb.velocity.y < 0 &&  GameplayManager.instance.unlockWalljump)
        {
            rb.velocity = new Vector2(rb.velocity.x, -wallSlideSpeed);
            wallSlidedown();
        }
        

        // Walljump
        if (Input.GetButtonDown("Jump") && isTouchingWall &&  GameplayManager.instance.unlockWalljump)
        {
           float horizontalVelocity = Mathf.Sign(transform.localScale.x) * wallJumpForce;
            rb.velocity = new Vector2(horizontalVelocity, jumpForce);
            wallJumped = true;
            canDoubleJump = true;
            Invoke("SetWallJumpedToFalse", 0.5f);
        }

//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        
        // controlla se il player è in aria e preme il tasto di attacco e il tasto direzionale basso
         if (!isGrounded() && Input.GetButtonDown("Fire1") && vertDir < 0)
        {
            isAttackingAir = true;
            DownAtk();

        } else  if (!isGrounded() && Input.GetButtonDown("Fire1") && vertDir > 0)
        {
            isAttackingAir = true;
            UpAtk();

        }      
                   
                    
                
                
////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////             

// gestione dell'input dello sparo
if (Input.GetButtonDown("Fire2") || L2 == 1 && isBlast && Time.time >= nextAttackTime)
{
    //Se non hai finito gli utilizzi
    if(UpdateMenuRapido.Instance.Vbottom > 0 ||
    UpdateMenuRapido.Instance.Vup > 0 ||
    UpdateMenuRapido.Instance.Vleft > 0 ||
    UpdateMenuRapido.Instance.Vright > 0)
    {
        //Se lo slot non è vuoto
    if(UpdateMenuRapido.Instance.idup > 0 || 
    UpdateMenuRapido.Instance.idright > 0 || 
    UpdateMenuRapido.Instance.idleft > 0 || 
    UpdateMenuRapido.Instance.idbottom > 0 )
       
    //L Animazione è gestita dagli script dei bullets visto che cambia a seconda del bullet
    Blast();
    isBlast = false;
    nextAttackTime = Time.time + 1f / attackRate;
    } else if(UpdateMenuRapido.Instance.Vbottom == 0 ||
    UpdateMenuRapido.Instance.Vup == 0 ||
    UpdateMenuRapido.Instance.Vleft == 0 ||
    UpdateMenuRapido.Instance.Vright == 0)
    {
        
    }
    
}
// ripristina la possibilità di attaccare dopo il tempo di attacco
if (!isBlast && Time.time >= nextAttackTime)
{
    isBlast = true;
}

////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
if (isHeal && PlayerHealth.Instance.currentEssence == 0 || isDeath) 
{
    isHeal = false;
    AnimationHealEnd();
}


if (PlayerHealth.Instance.currentEssence > 0) 
{
if (Input.GetButtonDown("Heal") && !isHeal && PlayerHealth.Instance.currentHealth != PlayerHealth.Instance.maxHealth)
{
    Stop();
    isHeal = true;
    AnimationHeal();
}
}


if (PlayerHealth.Instance.currentEssence > 0) 
{
if (Input.GetButtonUp("Heal"))
{
    isHeal = false;
    AnimationHealEnd();
}
}


////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
//Scelta della skill dal menu rapido
if (Input.GetButtonDown("SlotUp") || DpadY == 1)
{
    if (UpdateMenuRapido.Instance.idup > 0)
{
   UpdateMenuRapido.Instance.Selup();
    if(!GameplayManager.instance.StopDefaultSkill)
        {
        PlayerWeaponManager.instance.SetWeapon(GameplayManager.instance.idup);
        }
    else if(GameplayManager.instance.StopDefaultSkill)
        {
    PlayerWeaponManager.instance.SetWeapon(SkillMenu.Instance.idup);
        }

    slotU = true;
    slotB = false;
    slotL = false;
    slotR = false;
}
}
else if (Input.GetButtonDown("SlotRight") || DpadX == 1)
{
    if (UpdateMenuRapido.Instance.idright > 0)
{
      UpdateMenuRapido.Instance.Selright();
      //SkillMenu.Instance.AssignId();
        PlayerWeaponManager.instance.SetWeapon(SkillMenu.Instance.idright);
    slotU = false;
    slotB = false;
    slotL = false;
    slotR = true;
}
}
else if (Input.GetButtonDown("SlotLeft")|| DpadX == -1)
{
    if (UpdateMenuRapido.Instance.idleft > 0)
{
      UpdateMenuRapido.Instance.Selleft();
    PlayerWeaponManager.instance.SetWeapon(SkillMenu.Instance.idleft);
    slotU = false;
    slotB = false;
    slotL = true;
    slotR = false;
}
}
else if (Input.GetButtonDown("SlotBottom")|| DpadY == -1)
{
    if (UpdateMenuRapido.Instance.idbottom > 0)
    {
      UpdateMenuRapido.Instance.Selbottom();
    PlayerWeaponManager.instance.SetWeapon(SkillMenu.Instance.idbottom);
    slotU = false;
    slotB = true;
    slotL = false;
    slotR = false;
    }
}

///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////   

        if (Input.GetButtonDown("Fire1") && !isAttacking)
        {
            if(!isAttackingAir)
            {
            if(!NotStrangeAnimationTalk)
            {  
            //Se non sta facendo un attacco caricato
            if(!isCharging)
            {
            isAttacking = true;
            AddCombo();
            if(comboCount == 3)
            { comboCount = 0;}
            }
            }
            }

        }
///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    #region testForanysituation
            if(Input.GetKeyDown(KeyCode.C))
            {
                Debug.Log("Ok testiamo!");
                PlayerHealth.Instance.currentHealth = 10;
                //PlayerHealth.Instance.currentHealth = 0;
                //Respawn();
            }
if(Input.GetKeyDown(KeyCode.X))
            {
                Debug.Log("Recupero!");

                PlayerHealth.Instance.IncreaseEssence(10);
                //PlayerHealth.Instance.currentHealth = PlayerHealth.Instance.maxHealth;
                //PlayerHealth.Instance.currentEssence = PlayerHealth.Instance.maxEssence;
            }

            
            #endregion


///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////   
if(GameplayManager.instance.unlockCrash)
{
 if (Input.GetButtonDown("Fire3") && !isCharging && Time.time - timeSinceLastAttack > attackRate)
    {
        isCharging = true;
        AnimationCharge();
        Stop();
        // Inizializza il timer al tempo massimo
        currentTime = timeLimit;
        InvokeRepeating("CountDown", 1f, 1f);
    }

    if (Input.GetButtonDown("Fire3") && isCharging)
    {
        Stop();
        // Decrementa il timer di un secondo
        currentTime--;
        // Aggiorna il danno dell'attacco in base al tempo rimanente
        Damage = minDamage + (maxDamage - minDamage) * currentTime / timeLimit;
    }

    if (Input.GetButtonUp("Fire3") && isCharging)
    {
        if (currentTime == 0)
        {
            Damage = maxDamage;
        }
        else
        {
            Damage = minDamage + (maxDamage - minDamage) * currentTime / timeLimit;
        }
        AnimationChargeRelease();
        isCharging = false;
        Debug.Log("Charge ratio: " + (float)currentTime / timeLimit + ", Damage: " + Damage);
        timeSinceLastAttack = Time.time;
        CancelInvoke("CountDown");
    }

    if (isCharging)
    {
        Stop();
    }
}
///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
  if ( GameplayManager.instance.unlockDash)
        {
 if (Input.GetButtonUp("Dash") || R2 == 1 && !dashing && coolDownTime <= 0)
        {
            dashing = true;
            coolDownTime = dashCoolDown;
            dashTime = dashDuration;
            dashAnm();
        }

        if (coolDownTime > 0)
        {
            coolDownTime -= Time.deltaTime;
        }    
        }

 ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////   

        }
        }
        else if (stopInput)
        {//Bloccato
        }
if (!isPray)
        {
        // gestione dell'input del Menu 
        if (Input.GetButtonDown("Pause") && !stopInput)
        {
            GameplayManager.instance.Pause();
            StopinputTrue();
            Stooping();
            //InventoryManager.Instance.ListItems();
            Stop();
        }
        else if(Input.GetButtonDown("Pause") && stopInput)
        {
            GameplayManager.instance.Resume();
            StopinputFalse();
        }
        }
        checkFlip();
        moving();
    }

 ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////   


public void attackDash()
{
            attackNormal = true;
            coolDownTime = dashCoolDown;
            dashTime = dashDuration;
            DashAttack();        
}

public void attackupper()
{
            attackUpper = true;
            coolDownTime = dashCoolDown;
            dashTime = dashDuration;
            Upper();        
}


    private void FixedUpdate()
    {
        if(!GameplayManager.instance.PauseStop || !isAttacking || !isCharging || !touchGround || !isDashing || !isDeath)
        {


        if (isHeal)
        {
            PlayerHealth.Instance.currentEssence -= PlayerHealth.Instance.essencePerSecond * Time.fixedDeltaTime;
            PlayerHealth.Instance.IncreaseHP(PlayerHealth.Instance.hpIncreasePerSecond * Time.fixedDeltaTime);
        }

        float playerSpeed = horDir * speed;
        float accelRate = Mathf.Abs(playerSpeed) > 0.01f? acceleration : deceleration;
        rb.AddForce((playerSpeed - rb.velocity.x) * accelRate * Vector2.right);
        rb.velocity = new Vector2(Vector2.ClampMagnitude(rb.velocity, speed).x, rb.velocity.y); //Limit velocity
        
        if (lastTimeJump > Time.time && lastTimeGround > 0)
           { 
            jump();
           }
        if (dashing || Atkdashing)
        {
            if (horDir < 0)
        {

           rb.AddForce(-transform.right * dashForce, ForceMode2D.Impulse);
            dashTime -= Time.deltaTime;
        }
        else if (horDir > 0)
        {
            //anim.SetTrigger("Dash");

            rb.AddForce(transform.right * dashForce, ForceMode2D.Impulse);
            dashTime -= Time.deltaTime;
        }
        else if (horDir == 0)
        {
            if (rb.transform.localScale.x == -1)
        {

           rb.AddForce(-transform.right * dashForce, ForceMode2D.Impulse);
            dashTime -= Time.deltaTime;
        }
        else if (rb.transform.localScale.x == 1)
        {
            //anim.SetTrigger("Dash");

            rb.AddForce(transform.right * dashForce, ForceMode2D.Impulse);
            dashTime -= Time.deltaTime;
        }
                //dashing = false;
                //Atkdashing = false;
        }

            if (dashTime <= 0)
            {
                dashing = false;
                Atkdashing = false;

            }
        }

        if (attackNormal)
        {
            if (horDir < 0)
        {

           rb.AddForce(-transform.right * dashForceAtk, ForceMode2D.Impulse);
            dashTime -= Time.deltaTime;
        }
        else if (horDir > 0)
        {
            //anim.SetTrigger("Dash");

            rb.AddForce(transform.right * dashForceAtk, ForceMode2D.Impulse);
            dashTime -= Time.deltaTime;
        }
         else if (horDir == 0)
        {
            if (rb.transform.localScale.x == -1)
        {

           rb.AddForce(-transform.right * dashForce, ForceMode2D.Impulse);
            dashTime -= Time.deltaTime;
        }
        else if (rb.transform.localScale.x == 1)
        {
            //anim.SetTrigger("Dash");

            rb.AddForce(transform.right * dashForce, ForceMode2D.Impulse);
            dashTime -= Time.deltaTime;
        }
                //dashing = false;
                //Atkdashing = false;
        }

            if (dashTime <= 0)
            {
                dashing = false;
                attackNormal = false;

            }
        } else if (attackUpper)
        { 

            //Bisogna aggiungere un limite a questo punto
            if(dashTime > 0)
            {
        rb.AddForce(transform.up * upperForceAtk, ForceMode2D.Impulse);
        dashTime -= Time.deltaTime;
            }
            else if (dashTime <= 0)
            {
                dashing = false;
                attackUpper = false;
            }
        }



    }
    }
public void Bump()
    {
        if(isBump)
        {
         // applica l'impulso del salto se il personaggio è a contatto con il terreno
            rb.AddForce(new Vector2(0f, bumpForce), ForceMode2D.Impulse);
            isBump = false;
        }
    }
public void Knockback()
    {
         // applica l'impulso del salto se il personaggio è a contatto con il terreno
            if (transform.localScale.x < 0)
        {
        rb.AddForce(new Vector2(knockForce, 0f), ForceMode2D.Impulse);
        }
        else if (transform.localScale.x > 0)
        {
        rb.AddForce(new Vector2(-knockForce, 0f), ForceMode2D.Impulse);
        }
         else if (horDir == 0)
        {
        rb.AddForce(new Vector2(-knockForce, 0f), ForceMode2D.Impulse);
        }
       // lastTimeJump = Time.time + jumpDelay;
    }


// Metodo per ripristinare il valore di wallJumped dopo 0.5 secondi
    void SetWallJumpedToFalse()
    {
        wallJumped = false;
    }

    private void jump()
    {
        lastTimeJump = 0;
        lastTimeGround = 0;
        rb.velocity = new Vector2(rb.velocity.x, 0);
        rb.AddForce(new Vector2(rb.velocity.x, jumpForce), ForceMode2D.Impulse);
    }


private void modifyPhysics()
{
      if (rb.velocity.y > 0)
            rb.gravityScale = gravityOnJump;
        else if (rb.velocity.y < 0)
            rb.gravityScale = gravityOnFall;

//Se può fare walljump annulla la gravità
    if (canWallJump)
    {
        rb.velocity = new Vector2(horDir, 0f);
        rb.gravityScale = 0f; // disattiva la gravità durante il wall jump
    }
    else if (!canWallJump)
    {
        rb.gravityScale = gravityOnFall;

    }
}

public void StopinputTrue()
{
   stopInput = true;   
}
public void StopinputFalse()
{
    stopInput = false;   
}

    private bool isGrounded()
{
    //TRIPLE RAYCAST FOR GROUND: check if you touch the ground even with just one leg 
    return (
        
            Physics2D.Raycast(transform.position + raycastColliderOffset, Vector3.down, distanceFromGroundRaycast, groundLayer)
            ||
            Physics2D.Raycast(transform.position - raycastColliderOffset, Vector3.down, distanceFromGroundRaycast, groundLayer)
            ||
            Physics2D.Raycast(transform.position, Vector3.down, distanceFromGroundRaycast, groundLayer)
            
        );
}
    
    private void checkFlip()
    {
        
        if (horDir > 0)
            transform.localScale = new Vector2(1, 1);
        else if (horDir < 0)
            transform.localScale = new Vector2(-1, 1);
    }

public void Respawn()
{

    //Animazione di morte
    death();
    Stop();
    stopInput = true;
    isDeath = true;

    // Aspetta che la nuova scena sia completamente caricata
    StartCoroutine(WaitForSceneLoad());

}


 public void Stop()
    {
        rb.velocity = new Vector2(0f, 0f);
        horDir = 0;
        Swalk.Stop();
    }


IEnumerator WaitForSceneLoad()
{   
    yield return new WaitForSeconds(2f);
    GameplayManager.instance.FadeOut();
    yield return new WaitForSeconds(5f);
    // Cambia la scena
    SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
    SceneManager.sceneLoaded += OnSceneLoaded;

}

// Metodo eseguito quando la scena è stata caricata
private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
{

    SceneManager.sceneLoaded -= OnSceneLoaded;
    GameplayManager.instance.Restore();
     // Troviamo il game object del punto di spawn
        GameObject respawnPoint = GameObject.FindWithTag("Respawn");
        if (respawnPoint != null)
        {
            // Muoviamo il player al punto di spawn
            Player.transform.position = respawnPoint.transform.position;
            //yield return new WaitForSeconds(3f);
        }
    respawnRest(); 
    GameplayManager.instance.FadeIn();
StartCoroutine(wak());  
}

IEnumerator wak()
{   
    if (Player != null)
    {
    yield return new WaitForSeconds(2f);
    respawn();
    isDeath = false;
    stopInput = false; 
    }
    GameplayManager.instance.StopFade(); 

}
 

//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////


    public void SetBulletPrefab(GameObject newBullet)
    //Funzione per cambiare arma
    {
       bullet = newBullet;
    }    
    
void Blast()
{
        isBlast = true;
       // Debug.Log("il blast è partito");
        if(slotB)
        {
            if(UpdateMenuRapido.Instance.Vbottom > 0)
            {
        UpdateMenuRapido.Instance.Vbottom--;
        UpdateMenuRapido.Instance.SkillBottom_T.text = UpdateMenuRapido.Instance.Vbottom.ToString();
        Instantiate(blam, gun.position, transform.rotation);
        //Eccezioni di spawn
        if(UpdateMenuRapido.Instance.idbottom == 3 || 
        UpdateMenuRapido.Instance.idbottom == 2 || 
        UpdateMenuRapido.Instance.idbottom == 1)
        {
        Instantiate(bullet, transform.position, transform.rotation);
        }
        else if(UpdateMenuRapido.Instance.idbottom == 15)
        {
        Instantiate(bullet, top.position, transform.rotation);
        }
        else
        {
        Instantiate(bullet, gun.position, transform.rotation);
        }

            }
        }else if(slotU)
        {
            if(UpdateMenuRapido.Instance.Vup > 0)
            {
        UpdateMenuRapido.Instance.Vup--;
        UpdateMenuRapido.Instance.SkillUp_T.text = UpdateMenuRapido.Instance.Vup.ToString();
        Instantiate(blam, gun.position, transform.rotation);
        //Eccezioni di spawn
        if(UpdateMenuRapido.Instance.idup == 3 || 
        UpdateMenuRapido.Instance.idup == 2 || 
        UpdateMenuRapido.Instance.idup == 1 )
        {
        Instantiate(bullet, transform.position, transform.rotation);
        }else if(UpdateMenuRapido.Instance.idup == 15)
        {
        Instantiate(bullet, top.position, transform.rotation);
        }else
        {
        Instantiate(bullet, gun.position, transform.rotation);
        }            
        }
        }else if(slotL)
        {
            if(UpdateMenuRapido.Instance.Vleft > 0)
            {
        UpdateMenuRapido.Instance.Vleft--;
        UpdateMenuRapido.Instance.SkillLeft_T.text = UpdateMenuRapido.Instance.Vleft.ToString();
        Instantiate(blam, gun.position, transform.rotation);
        //Eccezioni di spawn
        if(UpdateMenuRapido.Instance.idleft == 3 || 
        UpdateMenuRapido.Instance.idleft == 2 ||
        UpdateMenuRapido.Instance.idleft == 1)
        {
        Instantiate(bullet, transform.position, transform.rotation);
        }else if(UpdateMenuRapido.Instance.idleft == 15)
        {
        Instantiate(bullet, top.position, transform.rotation);
        }else
        {
        Instantiate(bullet, gun.position, transform.rotation);
        }            
        }
        }else if(slotR)
        {
            if(UpdateMenuRapido.Instance.Vright > 0)
            {
        UpdateMenuRapido.Instance.Vright--;
        UpdateMenuRapido.Instance.SkillRight_T.text = UpdateMenuRapido.Instance.Vright.ToString();
        Instantiate(blam, gun.position, transform.rotation);
        //Eccezioni di spawn
        if(UpdateMenuRapido.Instance.idright == 3 || 
        UpdateMenuRapido.Instance.idright == 2 || 
        UpdateMenuRapido.Instance.idright == 1 )
        {
        Instantiate(bullet, transform.position, transform.rotation);
        }else if(UpdateMenuRapido.Instance.idright == 15)
        {
        Instantiate(bullet, top.position, transform.rotation);
        }else
        {
        Instantiate(bullet, gun.position, transform.rotation);
        }            
        }
        }
        
        
}

//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
public void AnimationHeal()
{
    if (currentAnimationName != HealAnimationName)
                {
                    _spineAnimationState.SetAnimation(2, HealAnimationName, true);
                    currentAnimationName = HealAnimationName;
                         _spineAnimationState.Event += HandleEvent;
                   // Debug.Log("Combo Count: " + comboCount + ", Playing Animation: combo_1");
                }
                // Add event listener for when the animation completes
                //_spineAnimationState.GetCurrent(2).Complete += OnAttackAnimationComplete;
}
public void AnimationHealEnd()
{
    if (currentAnimationName != HealEndAnimationName)
                {
                    _spineAnimationState.SetAnimation(2, HealEndAnimationName, false);
                    currentAnimationName = HealEndAnimationName;
                         _spineAnimationState.Event += HandleEvent;
                   // Debug.Log("Combo Count: " + comboCount + ", Playing Animation: combo_1");
                }
                // Add event listener for when the animation completes
                _spineAnimationState.GetCurrent(2).Complete += OnAttackAnimationComplete;
}
public void wallJump()
{
    if (currentAnimationName != walljumpAnimationName)
                {
                    _spineAnimationState.SetAnimation(1, walljumpAnimationName, true);
                    currentAnimationName = walljumpAnimationName;
                                        _spineAnimationState.Event += HandleEvent;

                   // Debug.Log("Combo Count: " + comboCount + ", Playing Animation: combo_1");
                }
                // Add event listener for when the animation completes
            _spineAnimationState.GetCurrent(1).Complete += OnJumpAnimationComplete;
    
}

public void UpAtk()
{
    if (currentAnimationName != upatkjumpAnimationName)
                {
                    _spineAnimationState.SetAnimation(2, upatkjumpAnimationName, true);
                    currentAnimationName = upatkjumpAnimationName;
                                        _spineAnimationState.Event += HandleEvent;

                   // Debug.Log("Combo Count: " + comboCount + ", Playing Animation: combo_1");
                }
                // Add event listener for when the animation completes
            _spineAnimationState.GetCurrent(2).Complete += OnAttackAnimationComplete;
    
}

public void DownAtk()
{
    if (currentAnimationName != downatkjumpAnimationName)
                {
                    _spineAnimationState.SetAnimation(2, downatkjumpAnimationName, true);
                    currentAnimationName = downatkjumpAnimationName;
                                        _spineAnimationState.Event += HandleEvent;

                   // Debug.Log("Combo Count: " + comboCount + ", Playing Animation: combo_1");
                }
                // Add event listener for when the animation completes
            _spineAnimationState.GetCurrent(2).Complete += OnAttackAnimationComplete;
    
}


private void wallSlide()
    {
        if (currentAnimationName != walljumpAnimationName) {
            _spineAnimationState.SetAnimation(1, walljumpAnimationName, true);
            currentAnimationName = walljumpAnimationName;
        }
    }

private void wallSlidedown()
    {
        if (currentAnimationName != walljumpdownAnimationName) {
            _spineAnimationState.SetAnimation(1, walljumpdownAnimationName, true);
            currentAnimationName = walljumpdownAnimationName;
        }
    } 

private void notWallSlide()
{
    if (currentAnimationName == jumpDownAnimationName || currentAnimationName == jumpAnimationName) {
        _spineAnimationState.SetAnimation(0, jumpDownAnimationName, false);
        currentAnimationName = jumpDownAnimationName;
        var currentAnimation = _spineAnimationState.GetCurrent(1);
        if (currentAnimation != null) {
            currentAnimation.Complete += OnJumpAnimationComplete;
        }
    }            
}

private void OnJumpAnimationComplete(Spine.TrackEntry trackEntry)
{
    // Remove the event listener
    trackEntry.Complete -= OnJumpAnimationComplete;

    // Clear the track 1 and reset to the idle animation
    _spineAnimationState.ClearTrack(1);
    _spineAnimationState.SetAnimation(1, idleAnimationName, true);
    currentAnimationName = idleAnimationName;

     // Reset the attack state
    isAttacking = false;
    isAttackingAir = false;
}

public void AnimationCharge()
{
    if (currentAnimationName != chargeAnimationName)
                {
                    _spineAnimationState.SetAnimation(2, chargeAnimationName, true);
                    currentAnimationName = chargeAnimationName;
                         _spineAnimationState.Event += HandleEvent;
                   // Debug.Log("Combo Count: " + comboCount + ", Playing Animation: combo_1");
                }
                // Add event listener for when the animation completes
               // _spineAnimationState.GetCurrent(2).Complete += OnAttackAnimationComplete;
}

public void dashAnm()
{
    if (currentAnimationName != dashAnimationName)
                {    
                    _spineAnimationState.ClearTrack(2);
                    _spineAnimationState.ClearTrack(1);
                    _spineAnimationState.SetAnimation(2, dashAnimationName, false);
                    currentAnimationName = dashAnimationName;
                                        _spineAnimationState.Event += HandleEvent;

                   // Debug.Log("Combo Count: " + comboCount + ", Playing Animation: combo_1");
                }
                // Add event listener for when the animation completes
            _spineAnimationState.GetCurrent(2).Complete += OnAttackAnimationComplete;
}
public void DashAttack()
{
    if (currentAnimationName != DashAttackAnimationName)
                {
                    _spineAnimationState.SetAnimation(2, DashAttackAnimationName, false);
                    currentAnimationName = DashAttackAnimationName;
                    
                   // Debug.Log("Combo Count: " + comboCount + ", Playing Animation: combo_1");
                }
                // Add event listener for when the animation completes
                _spineAnimationState.GetCurrent(2).Complete += OnAttackAnimationComplete;
}

public void DashSaw()
{
    if (currentAnimationName != dashsawAnimationName)
                {
                    _spineAnimationState.SetAnimation(2, dashsawAnimationName, false);
                    currentAnimationName = dashsawAnimationName;
                   // Debug.Log("Combo Count: " + comboCount + ", Playing Animation: combo_1");
                }
                // Add event listener for when the animation completes
                _spineAnimationState.GetCurrent(2).Complete += OnAttackAnimationComplete;
}
public void Blasting()
{
    if (currentAnimationName != blastAnimationName)
                {
                    _spineAnimationState.SetAnimation(2, blastAnimationName, false);
                    currentAnimationName = blastAnimationName;
                    Smagic.Play();
                   // Debug.Log("Combo Count: " + comboCount + ", Playing Animation: combo_1");
                }
                // Add event listener for when the animation completes
                _spineAnimationState.GetCurrent(2).Complete += OnAttackAnimationComplete;
}

public void Evocation()
{
    if (currentAnimationName != evocationAnimationName)
                {
                    _spineAnimationState.SetAnimation(2, evocationAnimationName, false);
                    currentAnimationName = evocationAnimationName;
                   // Debug.Log("Combo Count: " + comboCount + ", Playing Animation: combo_1");
                }
                // Add event listener for when the animation completes
                _spineAnimationState.GetCurrent(2).Complete += OnAttackAnimationComplete;
}

public void Upper()
{
    if (currentAnimationName != upperAnimationName)
                {
                    _spineAnimationState.SetAnimation(2, upperAnimationName, false);
                    currentAnimationName = upperAnimationName;
                   // Debug.Log("Combo Count: " + comboCount + ", Playing Animation: combo_1");
                }
                // Add event listener for when the animation completes
                _spineAnimationState.GetCurrent(2).Complete += OnAttackAnimationComplete;
}

public void SwordRain()
{
    if (currentAnimationName != SwordrainAnimationName)
                {
                    _spineAnimationState.SetAnimation(2, SwordrainAnimationName, false);
                    currentAnimationName = SwordrainAnimationName;
                   // Debug.Log("Combo Count: " + comboCount + ", Playing Animation: combo_1");
                }
                // Add event listener for when the animation completes
                _spineAnimationState.GetCurrent(2).Complete += OnAttackAnimationComplete;
}
public void Multilunge()
{
    if (currentAnimationName != multilungeAnimationName)
                {
                    _spineAnimationState.SetAnimation(2, multilungeAnimationName, false);
                    currentAnimationName = multilungeAnimationName;
                    
                   // Debug.Log("Combo Count: " + comboCount + ", Playing Animation: combo_1");
                }
                // Add event listener for when the animation completes
                _spineAnimationState.GetCurrent(2).Complete += OnAttackAnimationComplete;
}
public void Saw()
{
    if (currentAnimationName != SawinAnimationName)
                {
                    _spineAnimationState.SetAnimation(2, SawinAnimationName, false);
                    currentAnimationName = SawinAnimationName;
                   // Debug.Log("Combo Count: " + comboCount + ", Playing Animation: combo_1");
                }
                // Add event listener for when the animation completes
                _spineAnimationState.GetCurrent(2).Complete += OnAttackAnimationComplete;
}
public void Special()
{
    if (currentAnimationName != SpecialAnimationName)
                {
                    _spineAnimationState.SetAnimation(2, SpecialAnimationName, false);
                    currentAnimationName = SpecialAnimationName;
                   // Debug.Log("Combo Count: " + comboCount + ", Playing Animation: combo_1");
                }
                // Add event listener for when the animation completes
                _spineAnimationState.GetCurrent(2).Complete += OnAttackAnimationComplete;
}

public void Bigblast()
{
    if (currentAnimationName != bigblastAnimationName)
                {
                    _spineAnimationState.SetAnimation(2, bigblastAnimationName, false);
                    currentAnimationName = bigblastAnimationName;
                   // Debug.Log("Combo Count: " + comboCount + ", Playing Animation: combo_1");
                }
                // Add event listener for when the animation completes
                _spineAnimationState.GetCurrent(2).Complete += OnAttackAnimationComplete;
}

public void Slash()
{
    if (currentAnimationName != attackAnimationName)
                {
                    _spineAnimationState.SetAnimation(2, attackAnimationName, false);
                    currentAnimationName = attackAnimationName;
                   // Debug.Log("Combo Count: " + comboCount + ", Playing Animation: combo_1");
                }
                // Add event listener for when the animation completes
                _spineAnimationState.GetCurrent(2).Complete += OnAttackAnimationComplete;
}

public void Tornado()
{
    if (currentAnimationName != TornadoAnimationName)
                {
                    _spineAnimationState.SetAnimation(2, TornadoAnimationName, false);
                    currentAnimationName = TornadoAnimationName;
                   // Debug.Log("Combo Count: " + comboCount + ", Playing Animation: combo_1");
                }
                // Add event listener for when the animation completes
                _spineAnimationState.GetCurrent(2).Complete += OnAttackAnimationComplete;
}

public void Throw()
{
    if (currentAnimationName != throwAnimationName)
                {
                    _spineAnimationState.SetAnimation(2, throwAnimationName, false);
                    currentAnimationName = throwAnimationName;
                   // Debug.Log("Combo Count: " + comboCount + ", Playing Animation: combo_1");
                }
                // Add event listener for when the animation completes
                _spineAnimationState.GetCurrent(2).Complete += OnAttackAnimationComplete;
}
public void AnimationChargeRelease()
{
    if (currentAnimationName != pesanteAnimationName)
                {
                    _spineAnimationState.SetAnimation(2, pesanteAnimationName, false);
                    currentAnimationName = pesanteAnimationName;
                   // Debug.Log("Combo Count: " + comboCount + ", Playing Animation: combo_1");
                }
                // Add event listener for when the animation completes
                _spineAnimationState.GetCurrent(2).Complete += OnAttackAnimationComplete;
}

void CountDown()
{
    currentTime--;
    if (currentTime == 0)
    {
        Damage = maxDamage;
        AnimationChargeRelease();
        isCharging = false;
        Debug.Log("Charge ratio: 1.0, Damage: " + Damage);
        timeSinceLastAttack = Time.time;
        CancelInvoke("CountDown");
    }
}

public void AddCombo()
{
    //Se sta attaccando
    if (isAttacking)
    {
        //Il contatore aumenta ogni volta che si preme il tasto
        comboCount++;

        switch (comboCount)
        {
            //Setta lo stato d'animazione ed esegue l'animazione in base al conto della combo
            case 1:
                if (currentAnimationName != attackAnimationName)
                {Stop();
                    _spineAnimationState.SetAnimation(2, attackAnimationName, false);
                    currentAnimationName = attackAnimationName;
                    _spineAnimationState.Event += HandleEvent;

                    //Debug.Log("Combo Count: " + comboCount + ", Playing Animation: combo_1");
                }
                // Add event listener for when the animation completes
                _spineAnimationState.GetCurrent(2).Complete += OnAttackAnimationComplete;
                break;
            case 2:
                if (currentAnimationName != attack_hAnimationName)
                {Stop();
                    _spineAnimationState.SetAnimation(2, attack_hAnimationName, false);
                    currentAnimationName = attack_hAnimationName;
                    _spineAnimationState.Event += HandleEvent;

                    //Debug.Log("Combo Count: " + comboCount + ", Playing Animation: combo_1");
                }
                // Add event listener for when the animation completes
                _spineAnimationState.GetCurrent(2).Complete += OnAttackAnimationComplete;
                break;
            case 3:
            if (currentAnimationName != attack_aAnimationName)
                {Stop();
                    _spineAnimationState.SetAnimation(2, attack_aAnimationName, false);
                    currentAnimationName = attack_aAnimationName;
                    _spineAnimationState.Event += HandleEvent;

                    //Debug.Log("Combo Count: " + comboCount + ", Playing Animation: combo_1");
                }
                // Add event listener for when the animation completes
                _spineAnimationState.GetCurrent(2).Complete += OnAttackAnimationComplete;
                
                break;
            case 4:
                if (currentAnimationName != attack_lAnimationName)
                {Stop();
                    _spineAnimationState.SetAnimation(2, attack_lAnimationName, false);
                    currentAnimationName = attack_lAnimationName;
                    _spineAnimationState.Event += HandleEvent;

                    //Debug.Log("Combo Count: " + comboCount + ", Playing Animation: combo_1");
                }
                // Add event listener for when the animation completes
                _spineAnimationState.GetCurrent(2).Complete += OnAttackAnimationComplete;
                break;
            default:
                break;
        }
    }
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
    isAttacking = false;
    isAttackingAir = false;

}
public void Stooping()
{
             if (currentAnimationName != talkAnimationName)
                {
                    //_spineAnimationState.ClearTrack(2);
                    //_spineAnimationState.ClearTrack(1);
                    _spineAnimationState.SetAnimation(1, talkAnimationName, true);
                    currentAnimationName = talkAnimationName;
                    _spineAnimationState.Event += HandleEvent;

                    //Debug.Log("Combo Count: " + comboCount + ", Playing Animation: combo_1");
                }
                // Add event listener for when the animation completes
//                _spineAnimationState.GetCurrent(2).Complete += OnAttackAnimationComplete;
}
public void AnmHurt()
{
             if (currentAnimationName != hurtAnimationName)
                {
                    SHurt.Play();
                    _spineAnimationState.ClearTrack(2);
                    _spineAnimationState.SetAnimation(2, hurtAnimationName, false);
                    currentAnimationName = hurtAnimationName;
                    _spineAnimationState.Event += HandleEvent;

                    //Debug.Log("Combo Count: " + comboCount + ", Playing Animation: combo_1");
                }
                // Add event listener for when the animation completes
                _spineAnimationState.GetCurrent(2).Complete += OnAttackAnimationComplete;
}
public void death()
{
             if (currentAnimationName != deathAnimationName)
                {
                    _spineAnimationState.ClearTrack(1);
                    _spineAnimationState.SetAnimation(2, deathAnimationName, true);
                    currentAnimationName = deathAnimationName;
                    _spineAnimationState.Event += HandleEvent;
                    //Debug.Log("Combo Count: " + comboCount + ", Playing Animation: combo_1");
                }
                // Add event listener for when the animation completes
            //_spineAnimationState.GetCurrent(2).Complete += OnAttackAnimationComplete;

}
public void respawn()
{
             if (currentAnimationName != respawnAnimationName)
                {
                    _spineAnimationState.SetAnimation(2, respawnAnimationName, false);
                    currentAnimationName = respawnAnimationName;
                    _spineAnimationState.Event += HandleEvent;

                    //Debug.Log("Combo Count: " + comboCount + ", Playing Animation: combo_1");
                }
                // Add event listener for when the animation completes
            _spineAnimationState.GetCurrent(2).Complete += OnAttackAnimationComplete;

}
public void respawnRest()
{
             if (currentAnimationName != respawnRestAnimationName)
                {
                    _spineAnimationState.SetAnimation(2, respawnRestAnimationName, true);
                    currentAnimationName = respawnRestAnimationName;
                    _spineAnimationState.Event += HandleEvent;

                    //Debug.Log("Combo Count: " + comboCount + ", Playing Animation: combo_1");
                }
                // Add event listener for when the animation completes
            //_spineAnimationState.GetCurrent(2).Complete += OnAttackAnimationComplete;

}

public void AnimationRest()
{
    if (currentAnimationName != RestAnimationName)
                {
                    Stop();
                    _spineAnimationState.SetAnimation(2, RestAnimationName, false);
                    currentAnimationName = RestAnimationName;
                    _spineAnimationState.Event += HandleEvent;

                    //Debug.Log("Combo Count: " + comboCount + ", Playing Animation: combo_1");
                }
                // Add event listener for when the animation completes
                //_spineAnimationState.GetCurrent(2).Complete += OnAttackAnimationComplete;
}
public void animationWakeup()
{
    if (currentAnimationName != UpAnimationName)
                {
                    Stop();
                    _spineAnimationState.SetAnimation(2, UpAnimationName, false);
                    currentAnimationName = UpAnimationName;
                    _spineAnimationState.Event += HandleEvent;

                    //Debug.Log("Combo Count: " + comboCount + ", Playing Animation: combo_1");
                }
                // Add event listener for when the animation completes
                _spineAnimationState.GetCurrent(2).Complete += OnAttackAnimationComplete;
}

public void respawnWakeup()
{
    if (currentAnimationName != respawnAnimationName)
                {
                    _spineAnimationState.SetAnimation(2, respawnAnimationName, false);
                    currentAnimationName = respawnAnimationName;
                    _spineAnimationState.Event += HandleEvent;

                    //Debug.Log("Combo Count: " + comboCount + ", Playing Animation: combo_1");
                }
                // Add event listener for when the animation completes
                _spineAnimationState.GetCurrent(2).Complete += OnAttackAnimationComplete;
}

private void moving() {
    if(!isTouchingWall)
    {
        if(!stopInput)
        {
             if(!isHeal)
        {
            if(!isDeath)
        {
    switch (rb.velocity.y) {
        case 0:
            float speed = Mathf.Abs(rb.velocity.x);
            Test = speed;
            if (Mathf.Abs(Input.GetAxis("Horizontal")) < 0.01f) {
                // Player is not moving
                if (currentAnimationName != idleAnimationName) {
                    _spineAnimationState.SetAnimation(1, idleAnimationName, true);
                    currentAnimationName = idleAnimationName;
                }
            } else if (speed > runSpeedThreshold) {
                // Player is running
                if (currentAnimationName != runAnimationName) {
                    _spineAnimationState.SetAnimation(1, runAnimationName, true);
                    currentAnimationName = runAnimationName;
                }
            } else {
                // Player is walking
                if (currentAnimationName != walkAnimationName) {
                    _spineAnimationState.SetAnimation(1, walkAnimationName, true);
                    currentAnimationName = walkAnimationName;
                }
            }
            break;

        case > 0:
            // Player is jumping
            
            if (currentAnimationName != jumpAnimationName) {
                _spineAnimationState.SetAnimation(1, jumpAnimationName, true);
                currentAnimationName = jumpAnimationName;
            }
            
            break;

        case < 0:
            // Player is falling
            
            if (currentAnimationName != jumpDownAnimationName) {
                _spineAnimationState.SetAnimation(1, jumpDownAnimationName, true);
                currentAnimationName = jumpDownAnimationName;
            }
            
            break;
    }
    }else{    _spineAnimationState.ClearTrack(1);}
    }else{    _spineAnimationState.ClearTrack(1);}
    }
    }
}


//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

//Non puoi giocare di local scale sui vfx perché sono vincolati dal localscale del player PERò puoi giocare sulla rotazione E ottenere gli
//stessi effetti
void HandleEvent (TrackEntry trackEntry, Spine.Event e) {

if (e.Data.Name == "VFXpesante") {
        // Inserisci qui il codice per gestire l'evento.
        Instantiate(pesante, slashpoint.position, transform.rotation);
    }


    if (e.Data.Name == "SoundSlash") {     
    // Controlla se la variabile "SwSl" è stata inizializzata correttamente.
    
        if (SwSl == null) {
            Debug.LogError("AudioSource non trovato");
            return;
        }
        // Assicurati che l'oggetto contenente l'AudioSource sia attivo.
        if (!SwSl.gameObject.activeInHierarchy) {
            SwSl.gameObject.SetActive(true);
        }
        // Imposta la pitch dell'AudioSource in base ai valori specificati.
        SwSl.pitch = basePitch + Random.Range(-randomPitchOffset, randomPitchOffset); 
        // Assegna la clip audio all'AudioSource e avviala.
        SwSl.Play();
    }
    
    if (e.Data.Name == "VFXSlash") {
        // Inserisci qui il codice per gestire l'evento.
        Instantiate(attack, slashpoint.position, transform.rotation);
    }

if (e.Data.Name == "VFXSlash_h") {
        // Inserisci qui il codice per gestire l'evento.
        Instantiate(attack_h, slashpoint.position, attack_h.transform.rotation);
        // Controlla se la variabile "SwSl" è stata inizializzata correttamente.
    }
if (e.Data.Name == "VFXSlash_l") {
        // Inserisci qui il codice per gestire l'evento.
        Instantiate(attack_l, slashpoint.position, attack_l.transform.rotation);
    }
    if (e.Data.Name == "VFXSlash_a") {
        // Inserisci qui il codice per gestire l'evento.
        Instantiate(attack_a, slashpoint.position, attack_a.transform.rotation);
    }

if (e.Data.Name == "soundWalk") {
        // Imposta la pitch dell'AudioSource in base ai valori specificati.
        Swalk.pitch = basePitch + Random.Range(-randomPitchOffset, randomPitchOffset); 
        // Assegna la clip audio all'AudioSource e avviala.
        Swalk.Play();
    }
if (e.Data.Name == "soundRun") {
        // Imposta la pitch dell'AudioSource in base ai valori specificati.
        Swalk.pitch = basePitch + Random.Range(-randomPitchOffset, randomPitchOffset); 
        // Assegna la clip audio all'AudioSource e avviala.
        Swalk.Play();
    }
if (e.Data.Name == "SoundCharge") {
            
        Instantiate(charge, transform.position, transform.rotation);

        // Imposta la pitch dell'AudioSource in base ai valori specificati.

        Scharge.pitch = basePitch + Random.Range(-randomPitchOffset, randomPitchOffset); 
        // Assegna la clip audio all'AudioSource e avviala.
        Scharge.Play();
    }
if (e.Data.Name == "dash") {
            
        Sdash.pitch = basePitch + Random.Range(-randomPitchOffset, randomPitchOffset); 
        // Assegna la clip audio all'AudioSource e avviala.
        Sdash.Play();
    }
    if (e.Data.Name == "downslash") {

        Instantiate(attack_air_bottom, slashpoint.position, attack_air_bottom.transform.rotation);
       // Controlla se la variabile "SwSl" è stata inizializzata correttamente.
        if (SwSl == null) {
            Debug.LogError("AudioSource non trovato");
            return;
        }
        // Assicurati che l'oggetto contenente l'AudioSource sia attivo.
        if (!SwSl.gameObject.activeInHierarchy) {
            SwSl.gameObject.SetActive(true);
        }
        // Imposta la pitch dell'AudioSource in base ai valori specificati.
        SwSl.pitch = basePitch + Random.Range(-randomPitchOffset, randomPitchOffset); 
        // Assegna la clip audio all'AudioSource e avviala.
        SwSl.Play();
    }
    if (e.Data.Name == "upSlash") {

        Instantiate(attack_air_up, slashpoint.position, attack_air_up.transform.rotation);
       // Controlla se la variabile "SwSl" è stata inizializzata correttamente.
        if (SwSl == null) {
            Debug.LogError("AudioSource non trovato");
            return;
        }
        // Assicurati che l'oggetto contenente l'AudioSource sia attivo.
        if (!SwSl.gameObject.activeInHierarchy) {
            SwSl.gameObject.SetActive(true);
        }
        // Imposta la pitch dell'AudioSource in base ai valori specificati.
        SwSl.pitch = basePitch + Random.Range(-randomPitchOffset, randomPitchOffset); 
        // Assegna la clip audio all'AudioSource e avviala.
        SwSl.Play();
    }
    if (e.Data.Name == "evocationSwordRain") {

        Instantiate(swordRain, transform.position, transform.rotation);
       // Controlla se la variabile "SwSl" è stata inizializzata correttamente.
        if (SwSl == null) {
            Debug.LogError("AudioSource non trovato");
            return;
        }
        // Assicurati che l'oggetto contenente l'AudioSource sia attivo.
        if (!SwSl.gameObject.activeInHierarchy) {
            SwSl.gameObject.SetActive(true);
        }
        // Imposta la pitch dell'AudioSource in base ai valori specificati.
        SwSl.pitch = basePitch + Random.Range(-randomPitchOffset, randomPitchOffset); 
        // Assegna la clip audio all'AudioSource e avviala.
        SwSl.Play();
    }
if (e.Data.Name == "VFXHeal") {

        Instantiate(VFXHeal, transform.position, transform.rotation);
       // Controlla se la variabile "SwSl" è stata inizializzata correttamente.
        if (SHeal == null) {
            Debug.LogError("AudioSource non trovato");
            return;
        }
        // Assicurati che l'oggetto contenente l'AudioSource sia attivo.
        if (!SHeal.gameObject.activeInHierarchy) {
            SHeal.gameObject.SetActive(true);
        }
        // Imposta la pitch dell'AudioSource in base ai valori specificati.
        SHeal.pitch = basePitch + Random.Range(-randomPitchOffset, randomPitchOffset); 
        // Assegna la clip audio all'AudioSource e avviala.
        SHeal.Play();
    }

    if (e.Data.Name == "VFXStopHeal") {

        Instantiate(StopHeal, transform.position, transform.rotation);
        SHeal.Stop();
    }
}

    #region Gizmos
private void OnDrawGizmos()
    {
    Gizmos.color = Color.red;
    // disegna un Gizmo che rappresenta il Raycast
    Gizmos.DrawLine(transform.position, transform.position + new Vector3(transform.localScale.x, 0, 0) * wallDistance);
    }
#endregion

#if(UNITY_EDITOR)
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position + raycastColliderOffset, transform.position + raycastColliderOffset + Vector3.down * distanceFromGroundRaycast);
        Gizmos.DrawLine(transform.position - raycastColliderOffset, transform.position - raycastColliderOffset + Vector3.down * distanceFromGroundRaycast);
    }
#endif
}


