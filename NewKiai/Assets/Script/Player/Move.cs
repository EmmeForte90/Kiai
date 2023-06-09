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
    [SerializeField] public GameObject Shadow;


    [Header("Movement")]
    [SerializeField] private float speed;
    [SerializeField] private float acceleration;
    [SerializeField] private float deceleration;
    [HideInInspector] public float Test;
    [HideInInspector] public float InvincibleTime = 1f;
    [HideInInspector] public bool isHurt = false;
    [HideInInspector] public bool isBump = false;

    [HideInInspector] public float horDir;
    [HideInInspector] public float vertDir;
    [HideInInspector] public float DpadX;//DPad del joypad per il menu rapido
    [HideInInspector] public float DpadY;//DPad del joypad per il menu rapido
    [HideInInspector] public float L2;
    [HideInInspector] public float R2;
    

    [HideInInspector] public float runSpeedThreshold = 5f; // or whatever value you want
    [Header("Dash")]
    public float dashForce = 50f;
    public float dashDuration = 0.5f;
    private float dashTime;
    private bool dashing;
    private bool Atkdashing;
    public float dashForceAtk = 50f;
    public float upperForceAtk = 5f;
    private bool attackNormal;
    private bool attackUpper;
    private bool StartKiai = false;
    [HideInInspector] public float dashCoolDown = 1f;
    private float coolDownTime;
    [HideInInspector]
    public bool drawsword = false;

    [Header("Jump")]
    [SerializeField] private float jumpForce;
    [SerializeField] private float jumpForceX;
    [SerializeField] private float bumpForce;

    [Header("Knockback")]
    private bool KnockbackAt = false;
    private bool KnockbackAtL = false;
    private float knockForceShort = 10f;
    private float knockForceLong = 60f;

    bool canDoubleJump = false;
   // [HideInInspector] public float groundDelay = 0.1f; // The minimum time before the player can jump again after touching the ground
    bool isTouchingWall = false;
    public LayerMask wallLayer;         // layer del muro
    //public float wallJumpForce = 7f;    // forza del walljump
    public float wallSlideSpeed = 1f;   // velocità di scivolamento lungo il muro
    public float wallDistance = 0.5f;   // distanza dal muro per effettuare il walljump
    public bool canWallJump = false;
    //bool wallJumped = false;

    //float coyoteCounter = 0f;

    [SerializeField] private float coyoteTime;
    private float lastTimeGround;
    
    [SerializeField] private float jumpDelay;
    private float lastTimeJump;

    [SerializeField] private float gravityOnJump;
    [SerializeField] private float gravityOnFall;
    
    private readonly Vector3 raycastColliderOffset = new (0.25f, 0, 0);
    private const float distanceFromGroundRaycast = 0.3f;
    private const float distanceFromGroundJR = 1f;
    [SerializeField] private LayerMask groundLayer;
   
    [HideInInspector] public bool slotR,slotL,slotU,slotB = false;
    [Header("Respawn")]
    //private Transform respawnPoint; // il punto di respawn del giocatore
    public string sceneName; // il nome della scena in cui si trova il punto di respawn

    [Header("VFX")]
    
    [SerializeField] ParticleSystem ParticleMove;
    [SerializeField] GameObject ParticleFall;

    [SerializeField] public Transform gun;
    [SerializeField] public Transform top;
    [SerializeField] public Transform bottom;

    [SerializeField] GameObject Circle;
    [SerializeField] public Transform circlePoint;
    [SerializeField] public Transform slashpoint;
    [SerializeField] public Transform slashpoint1;
    [SerializeField] GameObject VFXHeal;
    [SerializeField] GameObject VFXHitGuard;
    [SerializeField] GameObject VFXWindSlash;
    [SerializeField] GameObject VFXWindSlash_h;
    [SerializeField] GameObject VFXWindSlashTOP;
    [SerializeField] GameObject VFXWindSlashDOWN;
    [SerializeField] GameObject VFXDash;

    private bool vfx = false;
    private float vfxTimer = 0.5f;
    //private Vector2 targetPosition; // La posizione di destinazione del nemico
    //private float speedBack = 3f; // La velocità del movimento

    [Header("TimerKiai")]
    public float timerestoreKiai = 3f; // il massimo valore di essenza disponibile
    public float timeKiai; // il massimo valore di essenza disponibile
    public bool KiaiReady = false;
    //public bool GoKiai = false;
    [Header("Animations")]
#region Animazioni
    [SpineAnimation][SerializeField]  string walkAnimationName;
    [SpineAnimation][SerializeField]  string walkSwordAnimationName;
    [SpineAnimation][SerializeField]  string runAnimationName;
    [SpineAnimation][SerializeField]  string runSwordAnimationName;
    [SpineAnimation][SerializeField]  string talkAnimationName;
    [Header("Jump")]
    [SpineAnimation][SerializeField]  string jumpAnimationName;
    [SpineAnimation][SerializeField]  string jumpSwordAnimationName;
    [SpineAnimation][SerializeField]  string jumpDownAnimationName;
    [SpineAnimation][SerializeField]  string jumpDownSwordAnimationName;
    [SpineAnimation][SerializeField]  string walljumpAnimationName;
    [SpineAnimation][SerializeField]  string walljumpdownAnimationName;
    [SpineAnimation][SerializeField]  string dashAnimationName;
    //////////////////////////////////////////////////////////////////////////
        [Header("HpAnm")]
    [SpineAnimation][SerializeField]  string hurtAnimationName;
    [SpineAnimation][SerializeField]  string BighurtAnimationName;
    [SpineAnimation][SerializeField]  string deathAnimationName;
    [SpineAnimation][SerializeField]  string RestAnimationName;
    [SpineAnimation][SerializeField]  string respawnRestAnimationName;
    [SpineAnimation][SerializeField]  string UpAnimationName;
    [SpineAnimation][SerializeField]  string respawnAnimationName;
    ///////////////////////////////////////////////////////////////////////////
        [Header("NormalStyle")]
    public GameObject S_Normal;
    [Tooltip("Animazione a singolo frame")]
    [SpineAnimation][SerializeField]  string idleSAnimationName;
    [Tooltip("Animazione Completa")]
    [SpineAnimation][SerializeField]  string idleAnimationName;
    [SpineAnimation][SerializeField]  string idleSwordAnimationName;
    [SerializeField] GameObject attack;
    [SerializeField] GameObject attack_h;
    [SerializeField] GameObject attack_h2;
    [SerializeField] GameObject attack_air_bottom;
    [SerializeField] GameObject attack_air_up;
    [SpineAnimation][SerializeField]  string KiaiNormalAnimationName;
    [SpineAnimation][SerializeField]  string attackNormal1AnimationName;
    [SpineAnimation][SerializeField]  string attackNormal2AnimationName;
    [SpineAnimation][SerializeField]  string attackNormal3AnimationName;
    [SpineAnimation][SerializeField]  string upatkjumpAnimationName;
    [SpineAnimation][SerializeField]  string downatkjumpAnimationName;
    private bool NormalSpecial = false;
    [SerializeField] GameObject attack_n_sp;
    [SpineAnimation][SerializeField]  string NHeavyReleaseAnimationName;
    [SpineAnimation][SerializeField]  string NHeavyAnimationName;
    [SerializeField] GameObject S_normalK_hitbox;
        [Header("Fire")]
    public GameObject S_Fire;
    [Tooltip("Animazione a singolo frame")]
    [SpineAnimation][SerializeField]  string fireposSAnimationName;
    [Tooltip("Animazione Completa")]
    [SpineAnimation][SerializeField]  string fireposAnimationName;
    [SpineAnimation][SerializeField]  string KiaiFireAnimationName;
    [SerializeField] GameObject attack_f_v;
    [SerializeField] GameObject attack_f_h;
    [SerializeField] GameObject attack_f_h2;
    [SerializeField] GameObject attack_f_air_bottom;
    [SerializeField] GameObject attack_f_air_up;
    [SpineAnimation][SerializeField]  string attackFire1AnimationName;
    [SpineAnimation][SerializeField]  string attackFire2AnimationName;
    [SpineAnimation][SerializeField]  string attackFire3AnimationName;
    [SpineAnimation][SerializeField]  string upatkFirejumpAnimationName;
    [SpineAnimation][SerializeField]  string downatkFirejumpAnimationName;
    private bool FireSpecial = false;
    [SerializeField] GameObject attack_f_sp;
    [SpineAnimation][SerializeField]  string UpperFireKiaijumpAnimationName;
    [SpineAnimation][SerializeField]  string UpperFireEndjumpAnimationName;
    [SerializeField] GameObject S_FireK_hitbox;

        [Header("Water")]
    private float SpeeRestore = 20f; // il massimo valore di essenza disponibile
    public GameObject S_Water;
    [Tooltip("Animazione a singolo frame")]
    [SpineAnimation][SerializeField]  string waterposSAnimationName;
    [Tooltip("Animazione Completa")]
    [SpineAnimation][SerializeField]  string waterposAnimationName;
    [SpineAnimation][SerializeField]  string KiaiWaterAnimationName;
    [SerializeField] GameObject attack_w_v;
    [SerializeField] GameObject attack_w_h;
    [SerializeField] GameObject attack_w_h2;
    [SpineAnimation][SerializeField]  string attackWater1AnimationName;
    [SpineAnimation][SerializeField]  string attackWater2AnimationName;
    [SpineAnimation][SerializeField]  string attackWater3AnimationName;
    [SpineAnimation][SerializeField]  string WaterjumpAnimationName;
    private bool WaterSpecial = false;
    private bool attackWater = false;
    [SpineAnimation][SerializeField]  string WaterLoopAnimationName;
    [SpineAnimation][SerializeField]  string WaterEndAnimationName;
    [SerializeField] GameObject attack_w_sp;
    [SerializeField] GameObject S_waterK_hitbox;

        [Header("Rock")]
    public GameObject S_Rock;
    [Tooltip("Animazione a singolo frame")]
    [SpineAnimation][SerializeField]  string rockposSAnimationName;
    [Tooltip("Animazione Completa")]
    [SpineAnimation][SerializeField]  string rockposAnimationName;
    [SpineAnimation][SerializeField]  string KiaiRockAnimationName;
    [SerializeField] GameObject attack_r_v;
    [SerializeField] GameObject attack_r_h;
    [SerializeField] GameObject attack_r_h2;
    [SerializeField] GameObject pesante;
    [SerializeField] GameObject charge;
    [SerializeField] GameObject VfxRock;
    [SpineAnimation][SerializeField]  string RockjumpAnimationName;
    [SpineAnimation][SerializeField]  string RockjumpLandingAnimationName;
    [SpineAnimation][SerializeField]  string chargeAnimationName;
    [SpineAnimation][SerializeField]  string releaseAnimationName;
    [SpineAnimation][SerializeField]  string attackRock1AnimationName;
    [SpineAnimation][SerializeField]  string attackRock2AnimationName;
    [SpineAnimation][SerializeField]  string SbamreleaseAnimationName;
    public bool RockSpecial = false;
    private bool attackRock = false;
    public bool isCrushRock = false;
    public bool JumpRock = false;
    public bool stomp = false;
    public float JumpRockTimer;
    public float JumpRockTimerMax = 1f;
    [SerializeField] GameObject S_rockK_hitbox;

        [Header("Wind")]
    public GameObject S_Wind;
    [Tooltip("Animazione a singolo frame")]
    [SpineAnimation][SerializeField]  string windposSAnimationName;
    [Tooltip("Animazione Completa")]
    [SpineAnimation][SerializeField]  string windposAnimationName;
    [SpineAnimation][SerializeField]  string KiaiWindAnimationName;
    [SerializeField] GameObject attack_v_v;
    [SerializeField] GameObject attack_v_h;
    [SerializeField] GameObject attack_v_h2;
    [SerializeField] GameObject attack_v_air_bottom;
    [SerializeField] GameObject attack_v_air_up;
    [SpineAnimation][SerializeField]  string attackWind1AnimationName;
    [SpineAnimation][SerializeField]  string attackWind2AnimationName;
    [SpineAnimation][SerializeField]  string attackWind3AnimationName;
    [SpineAnimation][SerializeField]  string attackWind4AnimationName;
    [SpineAnimation][SerializeField]  string upatkWindjumpAnimationName;
    [SpineAnimation][SerializeField]  string downatkWindjumpAnimationName;
    private bool WindSpecial = false;
    [SerializeField] GameObject attack_v_sp;
    [SpineAnimation][SerializeField]  string TornadoWindjumpAnimationName;
    [SerializeField] GameObject S_windK_hitbox;

        [Header("Void")]
    public GameObject S_Void;
    [Tooltip("Animazione a singolo frame")]
    [SpineAnimation][SerializeField]  string voidposSAnimationName;
    [Tooltip("Animazione Completa")]
    [SpineAnimation][SerializeField]  string voidposAnimationName;
    [SpineAnimation][SerializeField]  string KiaiVoidAnimationName;
    [SerializeField] GameObject attack_m_v;
    [SerializeField] GameObject attack_m_h;
    [SerializeField] GameObject attack_m_h2;
    
    [SpineAnimation][SerializeField]  string attackVoid1AnimationName;
    [SpineAnimation][SerializeField]  string attackVoid2AnimationName;
    [SpineAnimation][SerializeField]  string attackVoid3AnimationName;
    [SpineAnimation][SerializeField]  string upatkVoidjumpAnimationName;
    [SpineAnimation][SerializeField]  string downatkVoidjumpAnimationName;
    [SpineAnimation][SerializeField]  string guardNoSAnimationName;
    [SpineAnimation][SerializeField]  string guardNoEndSAnimationName;
    [SpineAnimation][SerializeField]  string guardNoHitSAnimationName;
    private bool VoidSpecial = false;
    [SerializeField] GameObject attack_S_sp;
    [SerializeField] GameObject S_voidK_hitbox;
    /////////////////////////////////////////////////////////////////////
    [Header("Stalmate")]
    [SpineAnimation][SerializeField]  string LoopStAnimationName;
    [SpineAnimation][SerializeField]  string StartStAnimationName;
    /////////////////////////////////////////////////////////////////////
     [Header("Fatalitis")]
    [SpineAnimation][SerializeField]  string F_JumpAnimationName;
    [SpineAnimation][SerializeField]  string F_BackAnimationName;
    [SpineAnimation][SerializeField]  string F_DanceAnimationName;
    [SpineAnimation][SerializeField]  string F_BlockKAnimationName;
    [SpineAnimation][SerializeField]  string F_LungeAnimationName;
    [SpineAnimation][SerializeField]  string F_SbemAnimationName;
    [SpineAnimation][SerializeField]  string F_SickleAnimationName;
    [SpineAnimation][SerializeField]  string F_SlashAnimationName;
    [SpineAnimation][SerializeField]  string BossDSpAnimationName;
    [SpineAnimation][SerializeField]  string LanceFAnimationName;
    [SpineAnimation][SerializeField]  string VeloceFAnimationName;
    /////////////////////////////////////////////////////////////////////
     [Header("Dodge and defend")]
    [SpineAnimation][SerializeField]  string TiredAnimationName;
    [SpineAnimation][SerializeField]  string DashAttackAnimationName;
    [SpineAnimation][SerializeField]  string swordDownAnimationName;
    [SpineAnimation][SerializeField]  string swordupAnimationName;
    [SpineAnimation][SerializeField]  string guardDownAnimationName;
    [SpineAnimation][SerializeField]  string guardEndDownAnimationName;
    [SpineAnimation][SerializeField]  string guardHitDownAnimationName;
    /////////////////////////////////////////////////////////////////////
    [Header("Items")]
    [SpineAnimation][SerializeField]  string HealAnimationName;
    [SpineAnimation][SerializeField]  string throwAnimationName;
    [SpineAnimation][SerializeField]  string BowAnimationName;
    [SpineAnimation][SerializeField]  string RifleAnimationName;

private string currentAnimationName;
#endregion
private int comboCount = 0;
     

    [Header("Attacks")]
    //[HideInInspector] public int Damage;
    private int timeScale = 1;
    private int FastCombo = 2;
    private bool canAttack = true;

    private float ShotTimer = 0f;
    private float attackRate = 0.5f;
    [SerializeField] private GameObject bullet;
    [HideInInspector] public int item = 0;
    // Dichiarazione delle variabili
    [HideInInspector] public int style = 0;
    private int MaxStyle;
    private int MaxItem;
    private int currentTime;
    private int timeLimit = 3; // Tempo massimo per caricare l'attacco
    private int maxDamage = 100; // Danno massimo dell'attacco caricato
    private int minDamage = 50; // Danno minimo dell'attacco non caricato
    private float timeSinceLastAttack = 0f;
    [HideInInspector]public bool isCharging;
    private bool touchGround;
    private bool isDashing;
    [HideInInspector]public bool isGuard;
    [HideInInspector]public bool isPray;//DPad del joypad per il menu rapido
    [HideInInspector]public bool isHeal;
    [HideInInspector]public bool isDeath;
    [HideInInspector]public bool isAttacking = false; // vero se il personaggio sta attaccando
    [HideInInspector]public bool isTired = false;
    public bool isAttackingAir = false; // vero se il personaggio sta attaccando
    public float isAttackingAirTimer = 2f;
    private bool isBlast = false; // vero se il personaggio sta attaccando
    public bool stopInput = false;
    [HideInInspector] public bool NotStrangeAnimationTalk = false;

    
    [Header("Audio")]
    [SerializeField] public AudioClip[] listSound; // array di AudioClip contenente tutti i suoni che si vogliono riprodurre
    private AudioSource[] sgm; // array di AudioSource che conterrà gli oggetti AudioSource creati
    public AudioMixer SFX;
    private bool sgmActive = false;

    private SkeletonAnimation _skeletonAnimation;
    private Spine.AnimationState _spineAnimationState;
    private Spine.Skeleton _skeleton;
    Spine.EventData eventData;

    public Rigidbody2D rb;
    private bool vfxFall = false;
public static Move instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        ParticleMove.Stop();
        //ParticleFall.Stop();
        _skeletonAnimation = GetComponent<SkeletonAnimation>();
        if (_skeletonAnimation == null) {
            Debug.LogError("Componente SkeletonAnimation non trovato!");
        }   
        _spineAnimationState = GetComponent<Spine.Unity.SkeletonAnimation>().AnimationState;
        _spineAnimationState = _skeletonAnimation.AnimationState;
        _skeleton = _skeletonAnimation.skeleton;
        sgm = new AudioSource[listSound.Length]; // inizializza l'array di AudioSource con la stessa lunghezza dell'array di AudioClip
        for (int i = 0; i < listSound.Length; i++) // scorre la lista di AudioClip
        {
            sgm[i] = gameObject.AddComponent<AudioSource>(); // crea un nuovo AudioSource come componente del game object attuale (quello a cui è attaccato lo script)
            sgm[i].clip = listSound[i]; // assegna l'AudioClip corrispondente all'AudioSource creato
            sgm[i].playOnAwake = false; // imposto il flag playOnAwake a false per evitare che il suono venga riprodotto automaticamente all'avvio del gioco
            sgm[i].loop = false; // imposto il flag playOnAwake a false per evitare che il suono venga riprodotto automaticamente all'avvio del gioco

        }
 // Aggiunge i canali audio degli AudioSource all'output del mixer
        foreach (AudioSource audioSource in sgm)
        {
        audioSource.outputAudioMixerGroup = SFX.FindMatchingGroups("Master")[0];
        }
// Inizializza la posizione di destinazione del nemico a pointB
        JumpRockTimer = JumpRockTimerMax;   
        VFXDash.gameObject.SetActive(false);
        }

private void Update()
{
//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
if(!stopInput && !isDeath)
        { if(!isTired){
        if(!isGuard || !isCharging || !isHeal)
        {
        horDir = Input.GetAxisRaw("Horizontal");
        vertDir = Input.GetAxisRaw("Vertical");
        DpadX = Input.GetAxis("DPad X");
        DpadY = Input.GetAxis("DPad Y");
        L2 = Input.GetAxis("L2");
        R2 = Input.GetAxis("R2");
        style = MaxStyle;
        item = MaxItem;
        }
        
//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        if (isGrounded())
        {
            lastTimeGround = coyoteTime; 
            isAttackingAir = false;
            canDoubleJump = true;
            Shadow.gameObject.SetActive(true);
            rb.gravityScale = 1;
            if(vfxFall)
            {Instantiate(ParticleFall, transform.position, transform.rotation); vfx = true;}
        }
        else
        {
            lastTimeGround -= Time.deltaTime;
            modifyPhysics();
            Shadow.gameObject.SetActive(false);  
        }
//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
if (JumpRock)
        {   
             RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down);
             if (isGrounded())
        {
            if (hit.collider != null && hit.distance <= distanceFromGroundJR)
            {
                stomp = true;
                //print("fatto");
                Stop();
                SbamRelease();           
            JumpRockTimer -= Time.deltaTime; //decrementa il timer ad ogni frame
            if (JumpRockTimer <= 0f && isGrounded()){
            JumpRockTimer = JumpRockTimerMax;
            JumpRock = false;
            stopInput = false;    
            stomp = false;        
            PlayMFX(1);
            }}}}
        if (stomp){Stop();}
        if (rb.velocity.y < 0 && stomp)
        {SbamRelease();  JumpRock = false; stopInput = false;  stomp = false;}
//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        if(vfx)
        {vfxTimer -= Time.deltaTime; //decrementa il timer ad ogni frame
        if (vfxTimer <= 0f) {
        vfx = false; vfxFall = false;}}
//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
if (Input.GetButtonDown("Jump") && Input.GetButtonDown("Fire1"))
{
    //Non succede nulla
    Stop();
}
if (Input.GetButtonDown("Jump") && Input.GetButtonDown("Fire2"))
{
    //Non succede nulla
    Stop();
}
if (Input.GetButtonDown("Jump") && Input.GetButtonDown("Fire2") && Input.GetButtonDown("Fire1")  && Input.GetButtonDown("Fire3")
 && Input.GetButtonDown("Dash")  && Input.GetButtonDown("Hsword"))
{
    //Non succede nulla
    Stop();
}
//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
#region Salto
 // Controllo se il personaggio è a contatto con un muro
 if( GameplayManager.instance.unlockWalljump)
 {
        Vector2 direction = transform.localScale.x > 0 ? Vector2.right : Vector2.left;
isTouchingWall = Physics2D.Raycast(transform.position, direction, wallDistance, wallLayer);
 }

if (Input.GetButtonDown("Jump") && !isGuard && !NotStrangeAnimationTalk  
        && !FireSpecial && !WaterSpecial && !WindSpecial && !RockSpecial && !NormalSpecial && !VoidSpecial
        && !StartKiai)
{            
    //wallJumped = false;
    if(!isTouchingWall)
        {
            //print("Walljump not");
            lastTimeJump = Time.time + jumpDelay;
        }
        //Pre-interrupt jump if button released
        if (Input.GetButtonUp("Jump") && rb.velocity.y > 0 
        && !isGuard && !NotStrangeAnimationTalk  
        && !FireSpecial && !WaterSpecial && !WindSpecial && !RockSpecial && !NormalSpecial && !VoidSpecial
        && !StartKiai)
        {
        if(isTouchingWall)
        {
        rb.velocity = new Vector2(-transform.localScale.x * 14, 18);
        }
        else
        {
            lastTimeGround = 0; //Avoid spam button
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.5f);   
        }
        }
    
    if (canDoubleJump && GameplayManager.instance.unlockDoubleJump && !isTouchingWall)
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
if (Input.GetButtonDown("Jump") && !isGuard && !NotStrangeAnimationTalk && isTouchingWall 
        && !FireSpecial && !WaterSpecial && !WindSpecial && !RockSpecial && !NormalSpecial && !VoidSpecial
        && !StartKiai)
{

    // Walljump
        //print("Walljump");
        if(transform.localScale.x > 0)
        {
            horDir = -1;
        rb.AddForce(new Vector2(jumpForceX, jumpForce), ForceMode2D.Impulse);
        //rb.velocity = new Vector2(jumpForceX, jumpForce);}
        }
        else if(transform.localScale.x < 0)
        {
            horDir = 1;
        rb.AddForce(new Vector2(-jumpForceX, jumpForce), ForceMode2D.Impulse);
        //rb.velocity = new Vector2(-jumpForceX, jumpForce);}
        //wallJumped = true;
        }
}

// Wallslide
        if (isTouchingWall && !isGrounded() && rb.velocity.y < 0 &&  GameplayManager.instance.unlockWalljump)
        {
            rb.velocity = new Vector2(rb.velocity.x, -wallSlideSpeed);
            wallSlidedown();
        }
#endregion
////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////             
// gestione dell'input dello sparo
    #region  Usare Items

    if (L2 == 1 && isBlast && Time.time >= ShotTimer)
    {
    //print("Spara");
    //Se non hai finito gli utilizzi
   if(
    UpdateMenuRapido.Instance.MXV1 > 0 ||
    UpdateMenuRapido.Instance.MXV2 > 0 ||
    UpdateMenuRapido.Instance.MXV3 > 0 ||
    UpdateMenuRapido.Instance.MXV4 > 0 ||
    UpdateMenuRapido.Instance.MXV5 > 0)
    {
        //Se lo slot non è vuoto
    if(
    UpdateMenuRapido.Instance.Slot1 > 0 || 
    UpdateMenuRapido.Instance.Slot2 > 0 || 
    UpdateMenuRapido.Instance.Slot3 > 0 || 
    UpdateMenuRapido.Instance.Slot4 > 0 ||
    UpdateMenuRapido.Instance.Slot5 > 0  
     )
     {  
    //L Animazione è gestita dagli script dei bullets visto che cambia a seconda del bullet
    isBlast = false;
    if(ItemRapidMenu.Instance.MXV1 > 0)
    {
        Blast(ItemRapidMenu.Instance.Item_1);
    }  
    if(ItemRapidMenu.Instance.MXV2 > 0)
    {
        Blast(ItemRapidMenu.Instance.Item_2);
    }  
    if(ItemRapidMenu.Instance.MXV3 > 0)
    {
        Blast(ItemRapidMenu.Instance.Item_3);
    }  
    if(ItemRapidMenu.Instance.MXV4 > 0)
    {
        Blast(ItemRapidMenu.Instance.Item_4);
    }  

    ShotTimer = Time.time + 1f / attackRate;
     }
    } 
}
    if(ItemRapidMenu.Instance.MXV1 <= 0)
    {
        UpdateMenuRapido.Instance.Slot1_T.text = null;
        UpdateMenuRapido.Instance.imageColor = UpdateMenuRapido.Instance.Slot1_I.color;
        UpdateMenuRapido.Instance.imageColor.a = 0f;
        UpdateMenuRapido.Instance.Slot1_I.color = UpdateMenuRapido.Instance.imageColor;
        //
        InventoryManager.Instance.imageColor = InventoryManager.Instance.Slot1_I.color;
        InventoryManager.Instance.imageColor.a = 0f;
        InventoryManager.Instance.Slot1_I.color = InventoryManager.Instance.imageColor;
    }
    if(ItemRapidMenu.Instance.MXV2 <= 0)
    {
        UpdateMenuRapido.Instance.Slot2_T.text = null;
        UpdateMenuRapido.Instance.imageColor = UpdateMenuRapido.Instance.Slot2_I.color;
        UpdateMenuRapido.Instance.imageColor.a = 0f;
        UpdateMenuRapido.Instance.Slot2_I.color = UpdateMenuRapido.Instance.imageColor;
        //
        InventoryManager.Instance.imageColor = InventoryManager.Instance.Slot2_I.color;
        InventoryManager.Instance.imageColor.a = 0f;
        InventoryManager.Instance.Slot2_I.color = InventoryManager.Instance.imageColor;
    }
    if(ItemRapidMenu.Instance.MXV3 <= 0)
    {
        UpdateMenuRapido.Instance.Slot3_T.text = null;
        UpdateMenuRapido.Instance.imageColor = UpdateMenuRapido.Instance.Slot3_I.color;
        UpdateMenuRapido.Instance.imageColor.a = 0f;
        UpdateMenuRapido.Instance.Slot3_I.color = UpdateMenuRapido.Instance.imageColor;
        //
        InventoryManager.Instance.imageColor = InventoryManager.Instance.Slot3_I.color;
        InventoryManager.Instance.imageColor.a = 0f;
        InventoryManager.Instance.Slot3_I.color = InventoryManager.Instance.imageColor;
    }
    if(ItemRapidMenu.Instance.MXV4 <= 0)
    {
        UpdateMenuRapido.Instance.Slot4_T.text = null;
        UpdateMenuRapido.Instance.imageColor = UpdateMenuRapido.Instance.Slot4_I.color;
        UpdateMenuRapido.Instance.imageColor.a = 0f;
        UpdateMenuRapido.Instance.Slot4_I.color = UpdateMenuRapido.Instance.imageColor;
        //
        InventoryManager.Instance.imageColor = InventoryManager.Instance.Slot4_I.color;
        InventoryManager.Instance.imageColor.a = 0f;
        InventoryManager.Instance.Slot4_I.color = InventoryManager.Instance.imageColor;
    }



// ripristina la possibilità di attaccare dopo il tempo di attacco
if (!isBlast && Time.time >= ShotTimer)
{
    isBlast = true;
}
        
        #endregion
////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
//Activate kiai
#region  Kiai
if(KiaiReady)
{
if (L2 == 1 && R2 == 1)
{
    CameraZoom.instance.ZoomIn();
    StopinputTrue();
    Stooping();
    Stop();
    StartKiai = true;
            if(GameplayManager.instance.styleIcon[4] == true)
            {if (style == 4) //Water
            {KiaiWater();}}
            if(GameplayManager.instance.styleIcon[1] == true)
            {if (style == 1) //Rock
            {KiaiRock();}}
            if(GameplayManager.instance.styleIcon[2] == true)
            {if (style == 2) //Fire
            {KiaiFire();}}
            if(GameplayManager.instance.styleIcon[3] == true)
            {if (style == 3) //Wind
            {KiaiWind();}}
            if(GameplayManager.instance.styleIcon[5] == true)
            {if (style == 5) //Void
            {KiaiVoid();}}
            if(GameplayManager.instance.styleIcon[0] == true)
            {if (style == 0)//Normal
            {KiaiNormal();}}
            StartCoroutine(FinishKiai());
        }
}
#endregion
////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
//Scelta della skill dal menu rapido
#region Scelta MenuRapido
if (Input.GetButtonDown("SlotUp") || DpadY == 1)
{
    if (UpdateMenuRapido.Instance.Slot1 > 0)
{
    UpdateMenuRapido.Instance.Selup();
    slotU = true;
    slotB = false;
    slotL = false;
    slotR = false;
}
}
else if (Input.GetButtonDown("SlotRight") || DpadX == 1)
{
    if (UpdateMenuRapido.Instance.Slot3 > 0)
{
    UpdateMenuRapido.Instance.Selright();
    slotU = false;
    slotB = false;
    slotL = false;
    slotR = true;
}
}
else if (Input.GetButtonDown("SlotLeft")|| DpadX == -1)
{
    if (UpdateMenuRapido.Instance.Slot2 > 0)
{
    UpdateMenuRapido.Instance.Selleft();
    slotU = false;
    slotB = false;
    slotL = true;
    slotR = false;
}
}
else if (Input.GetButtonDown("SlotBottom")|| DpadY == -1)
{
    if (UpdateMenuRapido.Instance.Slot4 > 0)
    {
    UpdateMenuRapido.Instance.Selbottom();
    slotU = false;
    slotB = true;
    slotL = false;
    slotR = false;
    }
}
#endregion
////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////  
//Attacco
#region Attacco
if (Input.GetButtonDown("Fire1") 
&& !isAttacking && !isAttackingAir && !isGuard && !NotStrangeAnimationTalk && !isCharging 
&& !FireSpecial && !WaterSpecial && !WindSpecial && !RockSpecial && !NormalSpecial && !VoidSpecial
&& !StartKiai) 
{
isAttacking = true;
drawsword = true;
ComboContatore();
AddCombo();
}
#endregion
//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
//Attacco in salto
#region  Attacchi in salto
        // controlla se il player è in aria e preme il tasto di attacco e il tasto direzionale basso
        if(!isAttackingAir){
         if (!isGrounded() && Input.GetButtonDown("Fire1") && vertDir < 0
        && !NotStrangeAnimationTalk && !isGuard && !isCharging 
        && !FireSpecial && !WaterSpecial && !WindSpecial && !RockSpecial && !NormalSpecial && !VoidSpecial
        && !StartKiai)
        {
            if(GameplayManager.instance.styleIcon[4] == true)
            {if (style == 4) //Water
            {WaterJumpAtk();}}
            if(GameplayManager.instance.styleIcon[1] == true)
            {if (style == 1) //Rock
            {RockJumpAtk();}} 
            if(GameplayManager.instance.styleIcon[2] == true)
            {if (style == 2) //Fire
            {DownAtkFire();}}  
            if(GameplayManager.instance.styleIcon[3] == true)
            {if (style == 3) //Wind
            {DownAtkWind();}}  
            if(GameplayManager.instance.styleIcon[5] == true)
            {if (style == 5) //Void
            {DownAtkVoid();  drawsword = false;}}  
            if(GameplayManager.instance.styleIcon[0] == true)
            {if (style == 0)//Normal
            {DownAtk();}}
            isAttackingAir = true;
            drawsword = true;
        } 
        else if (!isGrounded() && Input.GetButtonDown("Fire1") && vertDir > 0
        && !NotStrangeAnimationTalk && !isGuard && !isCharging 
        && !FireSpecial && !WaterSpecial && !WindSpecial && !RockSpecial && !NormalSpecial && !VoidSpecial
        && !StartKiai)
        {
            if(GameplayManager.instance.styleIcon[4] == true)
            {if (style == 4) //Water
            {WaterJumpAtk();}}
            if(GameplayManager.instance.styleIcon[1] == true)
            {if (style == 1) //Rock
            {RockJumpAtk();}} 
            if(GameplayManager.instance.styleIcon[2] == true)
            {if (style == 2) //Fire
            {UpAtkFire();}}  
            if(GameplayManager.instance.styleIcon[3] == true)
            {if (style == 3) //Wind
            {UpAtkWind();}}  
            if(GameplayManager.instance.styleIcon[5] == true)
            {if (style == 5) //Void
            {UpAtkVoid(); drawsword = false;}}  
            if(GameplayManager.instance.styleIcon[0] == true)
            {if (style == 0)//Normal
            {UpAtk();}}
            isAttackingAir = true;
            drawsword = true;
        }
        else if (!isGrounded() && Input.GetButtonDown("Fire1")
        && !NotStrangeAnimationTalk && !isGuard && !isCharging 
        && !FireSpecial && !WaterSpecial && !WindSpecial && !RockSpecial && !NormalSpecial && !VoidSpecial
        && !StartKiai)
        {
            if(GameplayManager.instance.styleIcon[4] == true)
            {if (style == 4) //Water
            {WaterJumpAtk();}}
            if(GameplayManager.instance.styleIcon[1] == true)
            {if (style == 1) //Rock
            {RockJumpAtk(); JumpRock = true;}}  
            drawsword = true;
            isAttackingAir = true;
        }
        }
        else if(isAttackingAir)
        {isAttackingAirTimer -= Time.deltaTime; //decrementa il timer ad ogni frame
        if (isAttackingAirTimer <= 0f) {
        isAttackingAir = false;
        isAttackingAirTimer = 2f;
        }}   
#endregion      
///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////   
//Riporre spada
#region Riporre Spada
if(drawsword)
{if(Input.GetButtonDown("Hsword") 
&& !isAttacking && !isAttackingAir && !isGuard && !NotStrangeAnimationTalk && !isCharging 
&& !FireSpecial && !WaterSpecial && !WindSpecial && !RockSpecial && !NormalSpecial && !VoidSpecial
&& !StartKiai && isGrounded())
            {drawsword = false;
            PlayMFX(5);
            repostsword();
            if (_skeletonAnimation != null)
            {_skeletonAnimation.timeScale = timeScale;}}
}
#endregion
////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////             
//Cambio stile
#region Tasti CambioStile
if (Input.GetButtonDown("R1"))
{
//print("Hai premuto R1");
if(
GameplayManager.instance.styleIcon[1] ||
GameplayManager.instance.styleIcon[2] ||
GameplayManager.instance.styleIcon[3] ||
GameplayManager.instance.styleIcon[4] ||
GameplayManager.instance.styleIcon[5])
{
MaxStyle++;
comboCount = 0;
changeStyle();
}
}

if (Input.GetButtonDown("L1"))
{
//print("Hai premuto L1");
if(
GameplayManager.instance.styleIcon[1] ||
GameplayManager.instance.styleIcon[2] ||
GameplayManager.instance.styleIcon[3] ||
GameplayManager.instance.styleIcon[4] ||
GameplayManager.instance.styleIcon[5])
{
MaxStyle--;
comboCount = 0;
changeStyle();
}}
#endregion
///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    #region testForanysituation
          /*  if(Input.GetKeyDown(KeyCode.O))
            {
                Debug.Log("L");
                GameplayManager.instance.TakeCamera();
                //KnockbackLong();
            }
if(Input.GetKeyDown(KeyCode.P))
            {
                //KnockbackS();
                Debug.Log("S");
            }  */ 
            #endregion
///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////   
//Guardia
#region Guardia
if (Input.GetButton("Fire3") && !isAttacking && !isAttackingAir && !isGuard && !NotStrangeAnimationTalk && !isCharging 
&& !FireSpecial && !WaterSpecial && !WindSpecial && !RockSpecial && !NormalSpecial && !VoidSpecial
&& !StartKiai && isGrounded())
{
if(GameplayManager.instance.styleIcon[5] == true ||
GameplayManager.instance.styleIcon[0] == true ||
GameplayManager.instance.styleIcon[1] == true ||
GameplayManager.instance.styleIcon[2] == true ||
GameplayManager.instance.styleIcon[3] == true ||
GameplayManager.instance.styleIcon[4] == true)
{if (style == 5) //Void
{ 
isGuard = true;
GuardArmAnm();
Stop();
}
else if(style == 0 || 
style == 1 ||
style == 2 ||
style == 3 ||
style == 4)
{
isGuard = true;
drawsword = true;
GuardAnm();
Stop();
}}}

if (Input.GetButtonUp("Fire3") && !isAttacking && !isAttackingAir && isGuard && !NotStrangeAnimationTalk && !isCharging 
&& !FireSpecial && !WaterSpecial && !WindSpecial && !RockSpecial && !NormalSpecial && !VoidSpecial
&& !StartKiai && isGrounded())
{
if(GameplayManager.instance.styleIcon[5] == true ||
GameplayManager.instance.styleIcon[0] == true ||
GameplayManager.instance.styleIcon[1] == true ||
GameplayManager.instance.styleIcon[2] == true ||
GameplayManager.instance.styleIcon[3] == true ||
GameplayManager.instance.styleIcon[4] == true)
{    
if (isGuard)
{
if (style == 5) //Void
{isGuard = false;
GuardArmAnmEnd();
}
else if(style == 0 || 
style == 1 ||
style == 2 ||
style == 3 ||
style == 4)
{endGuard();
isGuard = false;}
}
}
}

if (isGuard)
{Stop();}
#endregion
///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////   
//Special
#region Normal
if (Input.GetButton("Fire2") && !isAttacking && !isAttackingAir && !isGuard && !NotStrangeAnimationTalk && !isCharging 
&& !FireSpecial && !WaterSpecial && !WindSpecial && !RockSpecial && !NormalSpecial && !VoidSpecial
&& !StartKiai && isGrounded())
{
if(GameplayManager.instance.styleIcon[5] == true ||
GameplayManager.instance.styleIcon[0] == true ||
GameplayManager.instance.styleIcon[1] == true ||
GameplayManager.instance.styleIcon[2] == true ||
GameplayManager.instance.styleIcon[3] == true ||
GameplayManager.instance.styleIcon[4] == true)
{if (style == 0) //Normal
{if (PlayerHealth.Instance.currentStamina > 50) //Water
{ 
NormalSpecial = true;
drawsword = true;
HeavyHitS();
Stop();
}}}}

if (Input.GetButtonUp("Fire2") && !isAttacking && !isAttackingAir && !isGuard && !NotStrangeAnimationTalk && !isCharging 
&& !FireSpecial && !WaterSpecial && !WindSpecial && !RockSpecial && NormalSpecial && !VoidSpecial
&& !StartKiai && isGrounded())
{
if(GameplayManager.instance.styleIcon[5] == true ||
GameplayManager.instance.styleIcon[0] == true ||
GameplayManager.instance.styleIcon[1] == true ||
GameplayManager.instance.styleIcon[2] == true ||
GameplayManager.instance.styleIcon[3] == true ||
GameplayManager.instance.styleIcon[4] == true)
{    
if (NormalSpecial)
{
if (style == 0) //normal
{
PlayerHealth.Instance.currentStamina -= 50;
NormalSpecial = false;
HeavyHitRelease();        
}}}}

if (NormalSpecial)
{Stop();}
#endregion

#region Rock
if(GameplayManager.instance.styleIcon[5] == true ||
GameplayManager.instance.styleIcon[0] == true ||
GameplayManager.instance.styleIcon[1] == true ||
GameplayManager.instance.styleIcon[2] == true ||
GameplayManager.instance.styleIcon[3] == true ||
GameplayManager.instance.styleIcon[4] == true)
{if (style == 1) //Rock
{
 if (Input.GetButtonDown("Fire2") && !isCharging && Time.time - timeSinceLastAttack > attackRate
&& !isAttacking && !isAttackingAir && !isGuard && !NotStrangeAnimationTalk 
&& !FireSpecial && !WaterSpecial && !WindSpecial && !RockSpecial && !NormalSpecial && !VoidSpecial
&& !StartKiai)
    {if (PlayerHealth.Instance.currentStamina > 50) //Water
    { 
        isCharging = true;
        drawsword = true;
        isCrushRock = true;
        AnimationCharge();
        CameraZoom.instance.ZoomIn();
        Stop();
        // Inizializza il timer al tempo massimo
        currentTime = timeLimit;
        InvokeRepeating("CountDown", 1f, 1f);
    }}

    if (Input.GetButtonDown("Fire2") && isCharging
    && !isAttacking && !isAttackingAir && !isGuard && !NotStrangeAnimationTalk  
&& !FireSpecial && !WaterSpecial && !WindSpecial && !RockSpecial && !NormalSpecial && !VoidSpecial
&& !StartKiai)
    {
        Stop();
        // Decrementa il timer di un secondo
        currentTime--;
        // Aggiorna il danno dell'attacco in base al tempo rimanente
         GameplayManager.instance.Damage = minDamage + (maxDamage - minDamage) * currentTime / timeLimit;
    }

    if (Input.GetButtonUp("Fire2") && isCharging
    && !isAttacking && !isAttackingAir && !isGuard && !NotStrangeAnimationTalk  
&& !FireSpecial && !WaterSpecial && !WindSpecial && !RockSpecial && !NormalSpecial && !VoidSpecial
&& !StartKiai)
    {
        CameraZoom.instance.ZoomOut();
        if (currentTime == 0)
        {
             GameplayManager.instance.Damage  = maxDamage;
        }
        else
        {
             GameplayManager.instance.Damage = minDamage + (maxDamage - minDamage) * currentTime / timeLimit;
        }
        PlayerHealth.Instance.currentStamina -= 50;
        AnimationChargeRelease();
        isCharging = false;
        //Debug.Log("Charge ratio: " + (float)currentTime / timeLimit + ", Damage: " + HitboxPlayer.Instance.Damage);
        timeSinceLastAttack = Time.time;
        CancelInvoke("CountDown");
    }

    if (isCharging)
    {
    Stop();
    }
}
}
#endregion

#region Fire
if (Input.GetButton("Fire2") && !FireSpecial
&& !isAttacking && !isAttackingAir && !isGuard && !NotStrangeAnimationTalk && !isCharging 
&& !WaterSpecial && !WindSpecial && !RockSpecial && !NormalSpecial && !VoidSpecial
&& !StartKiai && isGrounded())
{
if(GameplayManager.instance.styleIcon[5] == true ||
GameplayManager.instance.styleIcon[0] == true ||
GameplayManager.instance.styleIcon[1] == true ||
GameplayManager.instance.styleIcon[2] == true ||
GameplayManager.instance.styleIcon[3] == true ||
GameplayManager.instance.styleIcon[4] == true)
{if (style == 2) //Fire
{ if (PlayerHealth.Instance.currentStamina > 50) //Water
{ 
FireSpecial = true;
drawsword = true;
FireUpper();
Stop();
}}}}

if (Input.GetButtonUp("Fire2") && !isAttacking && !isAttackingAir && !isGuard && !NotStrangeAnimationTalk && !isCharging 
&& FireSpecial && !WaterSpecial && !WindSpecial && !RockSpecial && !NormalSpecial && !VoidSpecial
&& !StartKiai)
{
if(GameplayManager.instance.styleIcon[5] == true ||
GameplayManager.instance.styleIcon[0] == true ||
GameplayManager.instance.styleIcon[1] == true ||
GameplayManager.instance.styleIcon[2] == true ||
GameplayManager.instance.styleIcon[3] == true ||
GameplayManager.instance.styleIcon[4] == true)
{    
if (FireSpecial)
{
if (style == 2) //Fire
{FireSpecial = false;
PlayerHealth.Instance.currentStamina -= 30;
attackupper();
}}}}
if (FireSpecial)
{Stop();}


#endregion

#region Wind
if (Input.GetButton("Fire2") && !WindSpecial
&& !isAttacking && !isAttackingAir && !isGuard && !NotStrangeAnimationTalk && !isCharging 
&& !FireSpecial && !WaterSpecial && !RockSpecial && !NormalSpecial && !VoidSpecial
&& !StartKiai)
{
if(GameplayManager.instance.styleIcon[5] == true ||
GameplayManager.instance.styleIcon[0] == true ||
GameplayManager.instance.styleIcon[1] == true ||
GameplayManager.instance.styleIcon[2] == true ||
GameplayManager.instance.styleIcon[3] == true ||
GameplayManager.instance.styleIcon[4] == true)
{if (style == 3) //Wind
{ if (PlayerHealth.Instance.currentStamina > 50) 
{ 
WindSpecial = true;
WindLoop();
}}}}
#endregion

#region Water
if (PlayerHealth.Instance.currentStamina > 10) //Water
{ 
if (Input.GetButton("Fire2") && !WaterSpecial
&& !isAttacking && !isAttackingAir && !isGuard && !NotStrangeAnimationTalk && !isCharging 
&& !FireSpecial && !WindSpecial && !RockSpecial && !NormalSpecial && !VoidSpecial
&& !StartKiai && isGrounded())
{
if(GameplayManager.instance.styleIcon[5] == true ||
GameplayManager.instance.styleIcon[0] == true ||
GameplayManager.instance.styleIcon[1] == true ||
GameplayManager.instance.styleIcon[2] == true ||
GameplayManager.instance.styleIcon[3] == true ||
GameplayManager.instance.styleIcon[4] == true)
{if (style == 4) //Water
{ if (PlayerHealth.Instance.currentStamina > 10) //Water
{ 
WaterSpecial = true;
drawsword = true;
WaterLoop();
Stop();
}}}}


if (Input.GetButtonUp("Fire2")&& !isAttacking && !isAttackingAir && !isGuard && !NotStrangeAnimationTalk && !isCharging 
&& !FireSpecial && WaterSpecial && !WindSpecial && !RockSpecial && !NormalSpecial && !VoidSpecial
&& !StartKiai)
{
if(GameplayManager.instance.styleIcon[5] == true ||
GameplayManager.instance.styleIcon[0] == true ||
GameplayManager.instance.styleIcon[1] == true ||
GameplayManager.instance.styleIcon[2] == true ||
GameplayManager.instance.styleIcon[3] == true ||
GameplayManager.instance.styleIcon[4] == true)
{    
if (WaterSpecial)
{
if (style == 4) //Water
{WaterSpecial = false;
attackWater = false;
EndWater();

if (WaterSpecial)
{Stop();}

}}}}
}else if (Input.GetButtonUp("Fire2") || Input.GetButton("Fire2") && PlayerHealth.Instance.currentStamina < 1) //Water
{ 
EndWater();
WaterSpecial = false;
attackWater = false;
}
#endregion

#region Void
if (Input.GetButton("Fire2") && !VoidSpecial
&& !isAttacking && !isAttackingAir && !isGuard && !NotStrangeAnimationTalk && !isCharging 
&& !FireSpecial && !WaterSpecial && !WindSpecial && !RockSpecial && !NormalSpecial 
&& !StartKiai)
{
if(GameplayManager.instance.styleIcon[5] == true ||
GameplayManager.instance.styleIcon[0] == true ||
GameplayManager.instance.styleIcon[1] == true ||
GameplayManager.instance.styleIcon[2] == true ||
GameplayManager.instance.styleIcon[3] == true ||
GameplayManager.instance.styleIcon[4] == true)
{if (style == 5) //Void
{ 
VoidSpecial = true;
HeavyHitS();
}}}

if (Input.GetButtonUp("Fire2")&& !isAttacking && !isAttackingAir && !isGuard && !NotStrangeAnimationTalk && !isCharging 
&& !FireSpecial && !WaterSpecial && !WindSpecial && !RockSpecial && !NormalSpecial && VoidSpecial
&& !StartKiai)
{
if(GameplayManager.instance.styleIcon[5] == true ||
GameplayManager.instance.styleIcon[0] == true ||
GameplayManager.instance.styleIcon[1] == true ||
GameplayManager.instance.styleIcon[2] == true ||
GameplayManager.instance.styleIcon[3] == true ||
GameplayManager.instance.styleIcon[4] == true)
{    
if (VoidSpecial)
{
if (style == 5) //Void
{VoidSpecial = false;
HeavyHitRelease();
}}}}
#endregion
///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
//Dash
#region Dash
  if ( GameplayManager.instance.unlockDash)
        {
 if (Input.GetButtonUp("Dash") ||  R2 == 1 && !dashing && coolDownTime <= 0 && !FireSpecial
&& !isAttacking && !isAttackingAir && !isGuard && !NotStrangeAnimationTalk && !isCharging 
&& !WaterSpecial && !WindSpecial && !RockSpecial && !NormalSpecial && !VoidSpecial
&& !StartKiai)
        {
            dashing = true;
            coolDownTime = dashCoolDown;
            dashTime = dashDuration;
            dashAnm();
            PlayMFX(4);
        }

        if (coolDownTime > 0)
        {
            coolDownTime -= Time.deltaTime;
        }    
        }
#endregion
///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////   
//Pause
}}
#region Pause

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
#endregion
///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////   
//Bloccatori per animazioni
if(attackRock)
  {Stop();}  

if(attackWater)
  {Stop(); PlayerHealth.Instance.currentStamina -= SpeeRestore * Time.deltaTime;}  
    
/*if(PlayerHealth.Instance.currentStamina <= 0 && isGrounded())
  {Stop(); TiredAnm(); isTired = true;} 
else if(PlayerHealth.Instance.currentStamina > 20)
  {isTired = false;}*/ 

if(dashing)
  {VFXDash.gameObject.SetActive(true);} 
else if(!dashing)
  {VFXDash.gameObject.SetActive(false);} 

}
 ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////   
public void TiredFunc()
{
    Stop(); TiredAnm(); isTired = true; stopInput = true; drawsword = true; isGuard = false;  WaterSpecial = false; FireSpecial = false;
    attackWater = false;  isCharging = false; NormalSpecial = false;
}
public void RestoreTiredFunc()
{
    isTired = false; stopInput = false; TiredAnmWithEnd(); drawsword = true; isGuard = false;   WaterSpecial = false;
    attackWater = false;  FireSpecial = false;  isCharging = false;  NormalSpecial = false; 
}
 ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////   
#region ContatoreCombo
public void ComboContatore()
{
//QUI PUOI DECIDERE QUANTE COMBO FARE PER STILE//
if(GameplayManager.instance.styleIcon[0] == true)
{if (style == 0) //Normal
{
if (_skeletonAnimation != null)
{
_skeletonAnimation.timeScale = timeScale; // Impostare il valore di time scale
}
if(comboCount >= 3)
{
comboCount = 0;}}}
///////////////////////
if(GameplayManager.instance.styleIcon[1] == true)
{if (style == 1) //Rock
{
if (_skeletonAnimation != null)
{
_skeletonAnimation.timeScale = timeScale; // Impostare il valore di time scale
}  
if(comboCount >= 2)
{
comboCount = 0;}}}
///////////////////////////////////
if(GameplayManager.instance.styleIcon[2] == true)
{if (style == 2 && canAttack) //Fire
{
if (_skeletonAnimation != null)
{
_skeletonAnimation.timeScale = timeScale; // Impostare il valore di time scale
}  
if(comboCount >= 2)
{
comboCount = 0;}}}
//////////////////////////////////
if(GameplayManager.instance.styleIcon[3] == true)
{if (style == 3) //Wind
{
if (_skeletonAnimation != null)
{
_skeletonAnimation.timeScale = FastCombo;// Impostare il valore di time scale
} 
if(comboCount >= 4)
{
comboCount = 0;}}}
///////////////////////////////////
if(GameplayManager.instance.styleIcon[4] == true)
{if (style == 4) //Water
{
if (_skeletonAnimation != null)
{
_skeletonAnimation.timeScale = timeScale; // Impostare il valore di time scale
}
if(comboCount >= 3)
{
comboCount = 0;}}}
////////////////////////////
if(GameplayManager.instance.styleIcon[5] == true)
{if (style == 5) //Void
{
drawsword = false;
if (_skeletonAnimation != null)
{
_skeletonAnimation.timeScale = FastCombo; // Impostare il valore di time scale
}
if(comboCount == 3)
{
comboCount = 0;}}}
}
#endregion

#region Cambia stile di combattimento
public void changeStyle()
{
     if(MaxStyle >= 5)
    {
        MaxStyle = 5;
        if(GameplayManager.instance.styleIcon[5] == true)//Void
        {
        PlayerWeaponManager.instance.SetStyle(5);
        GameplayManager.instance.Selector.transform.position = GameplayManager.instance.StyleS[5].transform.position;
        GameplayManager.instance.Damage = 2;
        VoidPose();
        drawsword = false;
        }
    }
    else if(MaxStyle <= 0)
    {
        MaxStyle = 0;
    if(GameplayManager.instance.styleIcon[0] == true)//Normal
        {
        PlayerWeaponManager.instance.SetStyle(0);
        GameplayManager.instance.Selector.transform.position = GameplayManager.instance.StyleS[0].transform.position;
        GameplayManager.instance.Damage = 10;
        NormalPose();
        drawsword = true;
        }        
    }else if(MaxStyle == 1)
    {
    if(GameplayManager.instance.styleIcon[1] == true)//Rock
        {
        MaxStyle = 1;
        PlayerWeaponManager.instance.SetStyle(1);
        GameplayManager.instance.Selector.transform.position = GameplayManager.instance.StyleS[1].transform.position;
        GameplayManager.instance.Damage = 50;
        RockPose();
        drawsword = true;
        }        
    }else if(MaxStyle == 2)
    { 
    if(GameplayManager.instance.styleIcon[2] == true)//Fire
        {
        MaxStyle = 2;
        PlayerWeaponManager.instance.SetStyle(2);
        GameplayManager.instance.Selector.transform.position = GameplayManager.instance.StyleS[2].transform.position;
        GameplayManager.instance.Damage =  30;
        FirePose();
        drawsword = true;
        }        
    }else if(MaxStyle == 3)
    {
    if(GameplayManager.instance.styleIcon[3] == true)//Wind
        {
            MaxStyle = 3;
        PlayerWeaponManager.instance.SetStyle(3);
        GameplayManager.instance.Selector.transform.position = GameplayManager.instance.StyleS[3].transform.position;
        GameplayManager.instance.Damage = 5;
        WindPose();
        drawsword = true;
        }       
    }else if(MaxStyle == 4)
    {            
    if(GameplayManager.instance.styleIcon[4] == true)//Water
        {
        MaxStyle = 4;
        PlayerWeaponManager.instance.SetStyle(4);
        GameplayManager.instance.Selector.transform.position = GameplayManager.instance.StyleS[4].transform.position;
        GameplayManager.instance.Damage =  5;
        WaterPose();
        drawsword = true;
        }       
    }else if(MaxStyle == 5)
    {    
    if(GameplayManager.instance.styleIcon[5] == true)//Void
        {
        MaxStyle = 5;
        PlayerWeaponManager.instance.SetStyle(5);
        GameplayManager.instance.Selector.transform.position = GameplayManager.instance.StyleS[5].transform.position;
        GameplayManager.instance.Damage =  2;
        VoidPose();
        drawsword = false;
        }        
    }else if(MaxStyle == -1)
    {   
    if(GameplayManager.instance.styleIcon[0] == true)//Normal
        {
            MaxStyle = 0;
        PlayerWeaponManager.instance.SetStyle(0);
        GameplayManager.instance.Selector.transform.position = GameplayManager.instance.StyleS[0].transform.position;
        GameplayManager.instance.Damage =  10;
        NormalPose();
        drawsword = true;
        }        
    }
}
#endregion
///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
#region Fisica attacchi (FixedUpdate)
public void attackDash()
{
    //Sistemare il cooldown
            Atkdashing = true;
            coolDownTime = dashCoolDown;
            dashTime = dashDuration;
            DashAttack();        
}

public void attackupper()
{
            attackUpper = true;
            coolDownTime = dashCoolDown;
            dashTime = dashDuration;
            Instantiate(attack_f_sp, bottom.position, attack_f_sp.transform.rotation);
            PlayMFX(1);
            FireUpperEnd();
}


    private void FixedUpdate()
    {
        if(!GameplayManager.instance.PauseStop || !isAttacking || !isCharging || !touchGround || !isDashing || !isDeath)
        {
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        if (isHeal)
        {
            PlayerHealth.Instance.currentKiai -= PlayerHealth.Instance.KiaiPerSecond * Time.fixedDeltaTime;
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
            rb.AddForce(transform.right * dashForce, ForceMode2D.Impulse);
            dashTime -= Time.deltaTime;
        }
        }
            if (dashTime <= 0)
            {
                dashing = false;
                Atkdashing = false;
            }
        }
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        if (KnockbackAt)
        {
            if (horDir < 0)
        {

           rb.AddForce(transform.right * knockForceShort, ForceMode2D.Impulse);
            dashTime -= Time.deltaTime;
        }
        else if (horDir > 0)
        {
            rb.AddForce(-transform.right * knockForceShort, ForceMode2D.Impulse);
            dashTime -= Time.deltaTime;
        }
         else if (horDir == 0)
        {
            if (rb.transform.localScale.x == -1)
        {

           rb.AddForce(transform.right * knockForceShort, ForceMode2D.Impulse);
            dashTime -= Time.deltaTime;
        }
        else if (rb.transform.localScale.x == 1)
        {
            rb.AddForce(-transform.right * knockForceShort, ForceMode2D.Impulse);
            dashTime -= Time.deltaTime;
        }
        }
            if (dashTime <= 0)
            {
                dashing = false;
                attackNormal = false;
                KnockbackAt = false;
            }
        }
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        if (KnockbackAtL)
        {
            if (horDir < 0)
        {

           rb.AddForce(transform.right * knockForceLong, ForceMode2D.Impulse);
            dashTime -= Time.deltaTime;
        }
        else if (horDir > 0)
        {
            rb.AddForce(-transform.right * knockForceLong, ForceMode2D.Impulse);
            dashTime -= Time.deltaTime;
        }
         else if (horDir == 0)
        {
            if (rb.transform.localScale.x == -1)
        {

           rb.AddForce(transform.right * knockForceLong, ForceMode2D.Impulse);
            dashTime -= Time.deltaTime;
        }
        else if (rb.transform.localScale.x == 1)
        {
            rb.AddForce(-transform.right * knockForceLong, ForceMode2D.Impulse);
            dashTime -= Time.deltaTime;
        }
        }
            if (dashTime <= 0)
            {
                dashing = false;
                attackNormal = false;
                KnockbackAtL = false;
            }
        }
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        if (attackNormal)
        {
            if (horDir < 0)
        {

           rb.AddForce(-transform.right * dashForceAtk, ForceMode2D.Impulse);
            dashTime -= Time.deltaTime;
        }
        else if (horDir > 0)
        {
            rb.AddForce(transform.right * dashForceAtk, ForceMode2D.Impulse);
            dashTime -= Time.deltaTime;
        }
         else if (horDir == 0)
        {
            if (rb.transform.localScale.x == -1)
        {

           rb.AddForce(-transform.right * dashForceAtk, ForceMode2D.Impulse);
            dashTime -= Time.deltaTime;
        }
        else if (rb.transform.localScale.x == 1)
        {
            rb.AddForce(transform.right * dashForceAtk, ForceMode2D.Impulse);
            dashTime -= Time.deltaTime;
        }
        }
            if (dashTime <= 0)
            {
                dashing = false;
                attackNormal = false;

            }
        } 
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        else if (attackUpper)
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
#endregion
public void KnockbackS()
{
    Stop();
    KnockbackAt = true;
}

public void KnockbackLong()
{
    Stop();
    KnockbackAtL = true;
    BigHurt();
}

IEnumerator FinishKiai()
{   
    yield return new WaitForSeconds(timeKiai);
    StopinputFalse();
    CameraZoom.instance.ZoomOut();
    PlayerHealth.Instance.currentKiai = 0;
    KiaiReady = false;
}

#region Fisica
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
    {rb.gravityScale = gravityOnJump;}
    else if (rb.velocity.y < 0)
    {rb.gravityScale = gravityOnFall;}

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
#endregion

public void StopinputTrue()
{
   stopInput = true;   
}
public void StopinputFalse()
{
    stopInput = false;   
}
  
    private void checkFlip()
    {
        
        if (horDir > 0)
            transform.localScale = new Vector2(1, 1);
        else if (horDir < 0)
            transform.localScale = new Vector2(-1, 1);
    }


void CountDown()
{
    currentTime--;
    if (currentTime == 0)
    {
         GameplayManager.instance.Damage = maxDamage;
        AnimationChargeRelease();
        isCharging = false;
        CameraZoom.instance.ZoomOut();
        Debug.Log("Charge ratio: 1.0, Damage: " +  GameplayManager.instance.Damage);
        timeSinceLastAttack = Time.time;
        CancelInvoke("CountDown");
    }
}

 public void Stop()
    {
        rb.velocity = new Vector2(0f, 0f);
        horDir = 0;
        //Swalk.Stop();
    }

#region Respawn Meccanica
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
IEnumerator WaitForSceneLoad()
{   
    yield return new WaitForSeconds(2f);
    GameplayManager.instance.FadeOut();
    AudioManager.instance.CrossFadeOUTAudio(AudioManager.instance.MusicAfter);
    yield return new WaitForSeconds(5f);
    AudioManager.instance.CrossFadeINAudio(AudioManager.instance.MusicBefore);
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
            GameplayManager.instance.TakeCamera();
            // Muoviamo il player al punto di spawn
            Player.transform.position = respawnPoint.transform.position;
            Player.transform.localScale = new Vector2(1, 1);
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
#endregion

//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    
#region Set Oggetti e stili
    public void SetStylePrefab(int newstyle)
    //Funzione per cambiare arma
    {
       style = newstyle;
    }    

    public void SetBulletPrefab(GameObject newBullet)
    //Funzione per cambiare arma
    {
       bullet = newBullet;
    }    
    
void Blast(Item newItem)
{
        isBlast = true;
        //print("il blast è partito");
        if(slotB)
        {
            if(UpdateMenuRapido.Instance.Slot4 > 0 && UpdateMenuRapido.Instance.MXV4 > 0)//Bottom
            {
        UpdateMenuRapido.Instance.MXV4--;
        ItemRapidMenu.Instance.MXV4--;
        newItem.value--;
        UpdateMenuRapido.Instance.Slot4_T.text = UpdateMenuRapido.Instance.MXV4.ToString();
        ItemRapidMenu.Instance.Slot4_T.text = UpdateMenuRapido.Instance.MXV4.ToString();
        Instantiate(bullet, gun.position, transform.rotation);
        }
        }else if(slotU)
        {
            if(UpdateMenuRapido.Instance.Slot1 > 0 && UpdateMenuRapido.Instance.MXV1 > 0)//up
            {
        UpdateMenuRapido.Instance.MXV1--;
        ItemRapidMenu.Instance.MXV1--;
        newItem.value--;
        UpdateMenuRapido.Instance.Slot1_T.text = UpdateMenuRapido.Instance.MXV1.ToString();
        ItemRapidMenu.Instance.Slot1_T.text = UpdateMenuRapido.Instance.MXV1.ToString();
        Instantiate(bullet, gun.position, transform.rotation);
        }
        }else if(slotL)
        {
            if(UpdateMenuRapido.Instance.Slot2 > 0 && UpdateMenuRapido.Instance.MXV2 > 0)//Left
            {
        UpdateMenuRapido.Instance.MXV2--;
        ItemRapidMenu.Instance.MXV2--;
        newItem.value--;
        UpdateMenuRapido.Instance.Slot2_T.text = UpdateMenuRapido.Instance.MXV2.ToString();
        ItemRapidMenu.Instance.Slot2_T.text = UpdateMenuRapido.Instance.MXV2.ToString();
        Instantiate(bullet, gun.position, transform.rotation);        
        }
        }else if(slotR)
        {
            if(UpdateMenuRapido.Instance.Slot3 > 0 && UpdateMenuRapido.Instance.MXV3 > 0)//Right
            {
        UpdateMenuRapido.Instance.MXV3--;
        ItemRapidMenu.Instance.MXV3--;
        newItem.value--;
        UpdateMenuRapido.Instance.Slot3_T.text = UpdateMenuRapido.Instance.MXV3.ToString();
        ItemRapidMenu.Instance.Slot3_T.text = UpdateMenuRapido.Instance.MXV3.ToString();
        Instantiate(bullet, gun.position, transform.rotation);          
        }
        }
        
}
#endregion

//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

#region Animazione salti
public void UpAtk()
{
    if (currentAnimationName != upatkjumpAnimationName)
                {
                    _spineAnimationState.ClearTrack(2);
                    _spineAnimationState.ClearTrack(1);
                    _spineAnimationState.SetAnimation(2, upatkjumpAnimationName, false);
                    currentAnimationName = upatkjumpAnimationName;
                                        _spineAnimationState.Event += HandleEvent;
                }
            _spineAnimationState.GetCurrent(2).Complete += OnAttackAnimationComplete;
    
}

public void DownAtk()
{
    if (currentAnimationName != downatkjumpAnimationName)
                {
                    _spineAnimationState.ClearTrack(2);
                    _spineAnimationState.ClearTrack(1);
                    _spineAnimationState.SetAnimation(2, downatkjumpAnimationName, false);
                    currentAnimationName = downatkjumpAnimationName;
                                        _spineAnimationState.Event += HandleEvent;
                }
            _spineAnimationState.GetCurrent(2).Complete += OnAttackAnimationComplete;
    
}
public void UpAtkFire()
{
    if (currentAnimationName != upatkFirejumpAnimationName)
                {
                    _spineAnimationState.SetAnimation(2, upatkFirejumpAnimationName, false);
                    currentAnimationName = upatkFirejumpAnimationName;
                                        _spineAnimationState.Event += HandleEvent;
                }
            _spineAnimationState.GetCurrent(2).Complete += OnAttackAnimationComplete;
    
}

public void DownAtkFire()
{
    if (currentAnimationName != downatkFirejumpAnimationName)
                {
                    _spineAnimationState.SetAnimation(2, downatkFirejumpAnimationName, false);
                    currentAnimationName = downatkFirejumpAnimationName;
                                        _spineAnimationState.Event += HandleEvent;
                }
            _spineAnimationState.GetCurrent(2).Complete += OnAttackAnimationComplete;
    
}
public void UpAtkWind()
{
    if (currentAnimationName != upatkWindjumpAnimationName)
                {
                    _spineAnimationState.SetAnimation(2, upatkWindjumpAnimationName, false);
                    currentAnimationName = upatkWindjumpAnimationName;
                                        _spineAnimationState.Event += HandleEvent;
                }
            _spineAnimationState.GetCurrent(2).Complete += OnAttackAnimationComplete;
    
}

public void DownAtkWind()
{
    if (currentAnimationName != downatkWindjumpAnimationName)
                {
                    _spineAnimationState.ClearTrack(2);
                    _spineAnimationState.SetAnimation(2, downatkWindjumpAnimationName, false);
                    currentAnimationName = downatkWindjumpAnimationName;
                                        _spineAnimationState.Event += HandleEvent;
                }
            _spineAnimationState.GetCurrent(2).Complete += OnAttackAnimationComplete;
    
}

public void UpAtkVoid()
{
    if (currentAnimationName != upatkVoidjumpAnimationName)
                {
                    _spineAnimationState.ClearTrack(2);
                    _spineAnimationState.SetAnimation(2, upatkVoidjumpAnimationName, false);
                    currentAnimationName = upatkVoidjumpAnimationName;
                                        _spineAnimationState.Event += HandleEvent;
                }
            _spineAnimationState.GetCurrent(2).Complete += OnAttackAnimationComplete;
    
}

public void DownAtkVoid()
{
    if (currentAnimationName != downatkVoidjumpAnimationName)
                {
                    _spineAnimationState.SetAnimation(2, downatkVoidjumpAnimationName, false);
                    currentAnimationName = downatkVoidjumpAnimationName;
                                        _spineAnimationState.Event += HandleEvent;
                }
            _spineAnimationState.GetCurrent(2).Complete += OnAttackAnimationComplete;
    
}


public void WaterJumpAtk()
{
    if (currentAnimationName != WaterjumpAnimationName)
                {    
                    _spineAnimationState.ClearTrack(2);
                    _spineAnimationState.ClearTrack(1);
                    _spineAnimationState.SetAnimation(2, WaterjumpAnimationName, true);
                    currentAnimationName = WaterjumpAnimationName;
                                        _spineAnimationState.Event += HandleEvent;
                }
            _spineAnimationState.GetCurrent(2).Complete += OnAttackAnimationComplete;
}

public void RockJumpAtk()
{
    if (currentAnimationName != RockjumpAnimationName)
                {    
                    _spineAnimationState.ClearTrack(2);
                    _spineAnimationState.ClearTrack(1);
                    _spineAnimationState.SetAnimation(2, RockjumpAnimationName, true);
                    currentAnimationName = RockjumpAnimationName;
                                        _spineAnimationState.Event += HandleEvent;
                }
            _spineAnimationState.GetCurrent(2).Complete += OnAttackAnimationComplete;
}
public void RockJumpLanding()
{
    if (currentAnimationName != RockjumpLandingAnimationName)
                {    
                    _spineAnimationState.ClearTrack(2);
                    _spineAnimationState.ClearTrack(1);
                    _spineAnimationState.SetAnimation(2, RockjumpLandingAnimationName, false);
                    currentAnimationName = RockjumpLandingAnimationName;
                                        _spineAnimationState.Event += HandleEvent;
                }
            _spineAnimationState.GetCurrent(2).Complete += OnAttackAnimationComplete;
}



public void wallJump()
{
    if (currentAnimationName != walljumpAnimationName)
                {
                    _spineAnimationState.SetAnimation(1, walljumpAnimationName, true);
                    currentAnimationName = walljumpAnimationName;
                                        _spineAnimationState.Event += HandleEvent;
                }
            _spineAnimationState.GetCurrent(1).Complete += OnJumpAnimationComplete;
    
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
    if(!drawsword)
            {
                if (currentAnimationName != idleAnimationName) {
                    _spineAnimationState.SetAnimation(1, idleAnimationName, true);
                    currentAnimationName = idleAnimationName;
                }
            } else if(drawsword)
            {
                if (currentAnimationName != idleSwordAnimationName) {
                    _spineAnimationState.SetAnimation(1, idleSwordAnimationName, true);
                    currentAnimationName = idleSwordAnimationName;
                }
            }
     // Reset the attack state
    isAttacking = false;
    if (_skeletonAnimation != null)
    {
    _skeletonAnimation.timeScale = timeScale; // Impostare il valore di time scale
    }
}

#endregion

#region Heal e tired animation
public void AnimationHeal()
{
    if (currentAnimationName != HealAnimationName)
                {
                    _spineAnimationState.SetAnimation(2, HealAnimationName, false);
                    currentAnimationName = HealAnimationName;
                         _spineAnimationState.Event += HandleEvent;
                }
                _spineAnimationState.GetCurrent(2).Complete += OnAttackAnimationComplete;
}

public void TiredAnm()
{
    if (currentAnimationName != TiredAnimationName)
                {
                    _spineAnimationState.ClearTrack(2);
                    _spineAnimationState.ClearTrack(2);
                    _spineAnimationState.SetAnimation(2, TiredAnimationName, true);
                    currentAnimationName = TiredAnimationName;
                    _spineAnimationState.Event += HandleEvent;
                }
}

public void TiredAnmWithEnd()
{
    if (currentAnimationName != TiredAnimationName)
                {
                    _spineAnimationState.ClearTrack(2);
                    _spineAnimationState.ClearTrack(2);
                    _spineAnimationState.SetAnimation(2, TiredAnimationName, true);
                    currentAnimationName = TiredAnimationName;
                    _spineAnimationState.Event += HandleEvent;
                }
            _spineAnimationState.GetCurrent(2).Complete += OnAttackAnimationComplete;

}
#endregion

#region Guardia
public void GuardAnm()
{
    if (currentAnimationName != guardDownAnimationName)
                {
                    _spineAnimationState.SetAnimation(2, guardDownAnimationName, true);
                    currentAnimationName = guardDownAnimationName;
                         _spineAnimationState.Event += HandleEvent;
                }
}
public void endGuard()
{
    if (currentAnimationName != guardEndDownAnimationName)
                {
                    _spineAnimationState.SetAnimation(2, guardEndDownAnimationName, false);
                    currentAnimationName = guardEndDownAnimationName;
                         _spineAnimationState.Event += HandleEvent;
                }
               _spineAnimationState.GetCurrent(2).Complete += OnAttackAnimationComplete;
}


public void GuardHit()
{
    if (currentAnimationName != guardHitDownAnimationName)
                {
                    PlayMFX(6);
                    Instantiate(VFXHitGuard, slashpoint.position, VFXHitGuard.transform.rotation);
                    _spineAnimationState.SetAnimation(2, guardHitDownAnimationName, false);
                    currentAnimationName = guardHitDownAnimationName;
                         _spineAnimationState.Event += HandleEvent;
                }
}

public void GuardArmAnm()
{
    if (currentAnimationName != guardNoSAnimationName)
                {
                    _spineAnimationState.SetAnimation(2, guardNoSAnimationName, true);
                    currentAnimationName = guardNoSAnimationName;
                         _spineAnimationState.Event += HandleEvent;
                }
}
public void GuardArmAnmEnd()
{
    if (currentAnimationName != guardNoEndSAnimationName)
                {
                    _spineAnimationState.SetAnimation(2, guardNoEndSAnimationName, false);
                    currentAnimationName = guardNoEndSAnimationName;
                         _spineAnimationState.Event += HandleEvent;
                }
               _spineAnimationState.GetCurrent(2).Complete += OnAttackAnimationComplete;
}

public void GuardArmHit()
{
    if (currentAnimationName != guardNoHitSAnimationName)
                {
                    PlayMFX(6);
                    Instantiate(VFXHitGuard, slashpoint.position, VFXHitGuard.transform.rotation);
                    _spineAnimationState.SetAnimation(2, guardNoHitSAnimationName, false);
                    currentAnimationName = guardNoHitSAnimationName;
                         _spineAnimationState.Event += HandleEvent;
                }
}
#endregion

#region Charge and Rock


public void AnimationCharge()
{
    if (currentAnimationName != chargeAnimationName)
                {
                    _spineAnimationState.SetAnimation(2, chargeAnimationName, true);
                    currentAnimationName = chargeAnimationName;
                         _spineAnimationState.Event += HandleEvent;
                }
}

public void AnimationChargeRelease()
{
    if (currentAnimationName != releaseAnimationName)
                {
                    PlayerHealth.Instance.currentStamina -= 50f;
                    _spineAnimationState.SetAnimation(2, releaseAnimationName, false);
                    currentAnimationName = releaseAnimationName;
                }
                _spineAnimationState.GetCurrent(2).Complete += OnAttackAnimationComplete;
}
public void SbamRelease()
{
    if (currentAnimationName != SbamreleaseAnimationName)
                {
                    _spineAnimationState.SetAnimation(2, SbamreleaseAnimationName, false);
                    currentAnimationName = SbamreleaseAnimationName;
                }
                _spineAnimationState.GetCurrent(2).Complete += OnAttackAnimationComplete;
}

public void HeavyHitS()
{
    if (currentAnimationName != NHeavyAnimationName)
                {
                    _spineAnimationState.SetAnimation(2, NHeavyAnimationName, false);
                    currentAnimationName = NHeavyAnimationName;
                         _spineAnimationState.Event += HandleEvent;
                }
}

public void HeavyHitRelease()
{
    if (currentAnimationName != NHeavyReleaseAnimationName)
                {
                    attackDash();
                    PlayerHealth.Instance.currentStamina -= 30f;
                    _spineAnimationState.SetAnimation(2, NHeavyReleaseAnimationName, false);
                    currentAnimationName = NHeavyReleaseAnimationName;
                }
                _spineAnimationState.GetCurrent(2).Complete += OnAttackAnimationComplete;
}
#endregion

#region Qualche special e lancio oggetti
public void WaterLoop()
{
    if (currentAnimationName != WaterLoopAnimationName)
                {
                    PlayerHealth.Instance.currentStamina -= 20f * Time.deltaTime;
                    _spineAnimationState.SetAnimation(2, WaterLoopAnimationName, true);
                    currentAnimationName = WaterLoopAnimationName;
                         _spineAnimationState.Event += HandleEvent;
                }
}

public void EndWater()
{
    if (currentAnimationName != WaterEndAnimationName)
                {
                    _spineAnimationState.SetAnimation(2, WaterEndAnimationName, false);
                    currentAnimationName = WaterEndAnimationName;
                }
                _spineAnimationState.GetCurrent(2).Complete += OnAttackAnimationComplete;
}

public void FireUpper()
{
    if (currentAnimationName != UpperFireKiaijumpAnimationName)
                {
                    _spineAnimationState.SetAnimation(2, UpperFireKiaijumpAnimationName, false);
                    currentAnimationName = UpperFireKiaijumpAnimationName;
                }
}
public void FireUpperEnd()
{
    if (currentAnimationName != UpperFireEndjumpAnimationName)
                {
                    PlayerHealth.Instance.currentStamina -= 30f;
                    _spineAnimationState.SetAnimation(2, UpperFireEndjumpAnimationName, false);
                    currentAnimationName = UpperFireEndjumpAnimationName;
                }
                _spineAnimationState.GetCurrent(2).Complete += OnAttackAnimationComplete;
}

public void WindLoop()
{
    if (currentAnimationName != TornadoWindjumpAnimationName)
                {
                    WindSpecial = false;
                    PlayerHealth.Instance.currentStamina -= 40f;
                    _spineAnimationState.SetAnimation(2, TornadoWindjumpAnimationName, false);
                    currentAnimationName = TornadoWindjumpAnimationName;
                         _spineAnimationState.Event += HandleEvent;
                }
                _spineAnimationState.GetCurrent(2).Complete += OnAttackAnimationComplete;
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
                }
            _spineAnimationState.GetCurrent(2).Complete += OnAttackAnimationComplete;
}
public void DashAttack()
{
    if (currentAnimationName != DashAttackAnimationName)
                {
                    _spineAnimationState.SetAnimation(2, DashAttackAnimationName, false);
                    currentAnimationName = DashAttackAnimationName;
                }
                _spineAnimationState.GetCurrent(2).Complete += OnAttackAnimationComplete;
}

public void Blasting()
{
    if (currentAnimationName != throwAnimationName)
                {
                    _spineAnimationState.SetAnimation(2, throwAnimationName, false);
                    currentAnimationName = throwAnimationName;
                }
                _spineAnimationState.GetCurrent(2).Complete += OnAttackAnimationComplete;
}

public void Bow()
{
    if (currentAnimationName != BowAnimationName)
                {
                    _spineAnimationState.SetAnimation(2, BowAnimationName, false);
                    currentAnimationName = BowAnimationName;
                }
                _spineAnimationState.GetCurrent(2).Complete += OnAttackAnimationComplete;
}

public void Rifle()
{
    if (currentAnimationName != RifleAnimationName)
                {
                    _spineAnimationState.SetAnimation(2, RifleAnimationName, false);
                    currentAnimationName = RifleAnimationName;
                }
                _spineAnimationState.GetCurrent(2).Complete += OnAttackAnimationComplete;
}


public void Throw()
{
    if (currentAnimationName != throwAnimationName)
                {
                    _spineAnimationState.SetAnimation(2, throwAnimationName, false);
                    currentAnimationName = throwAnimationName;
                }
                _spineAnimationState.GetCurrent(2).Complete += OnAttackAnimationComplete;
}
#endregion

/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
//SINGLE FRAME

#region Pose a singolo frame(Per quando si sta fermi)
public void NormalPose()
{
    if (currentAnimationName != idleSAnimationName)
                {
                Stop();
                Instantiate(S_Normal, top.transform.position, S_Normal.transform.rotation);
                    _spineAnimationState.SetAnimation(2, idleSAnimationName, false);
                    currentAnimationName = idleSAnimationName;
                   // Debug.Log("Combo Count: " + comboCount + ", Playing Animation: combo_1");
                }
                // Add event listener for when the animation completes
               _spineAnimationState.GetCurrent(2).Complete += OnAttackAnimationComplete;
}
public void FirePose()
{
    if (currentAnimationName != fireposSAnimationName)
                {
                    Stop();
                    Instantiate(S_Fire, top.transform.position, S_Fire.transform.rotation);
                    _spineAnimationState.SetAnimation(2, fireposSAnimationName, false);
                    currentAnimationName = fireposSAnimationName;
                   // Debug.Log("Combo Count: " + comboCount + ", Playing Animation: combo_1");
                }
                // Add event listener for when the animation completes
                _spineAnimationState.GetCurrent(2).Complete += OnAttackAnimationComplete;
}
public void RockPose()
{
    if (currentAnimationName != rockposSAnimationName)
                {
                    Stop();
                    Instantiate(S_Rock, top.transform.position, S_Rock.transform.rotation);
                    _spineAnimationState.SetAnimation(2, rockposSAnimationName, false);
                    currentAnimationName = rockposSAnimationName;
                   // Debug.Log("Combo Count: " + comboCount + ", Playing Animation: combo_1");
                }
                // Add event listener for when the animation completes
                _spineAnimationState.GetCurrent(2).Complete += OnAttackAnimationComplete;
}public void WaterPose()
{
    if (currentAnimationName != waterposSAnimationName)
                {
                    Stop();
                    Instantiate(S_Water, top.transform.position, S_Water.transform.rotation);
                    _spineAnimationState.SetAnimation(2, waterposSAnimationName, false);
                    currentAnimationName = waterposSAnimationName;
                   // Debug.Log("Combo Count: " + comboCount + ", Playing Animation: combo_1");
                }
                // Add event listener for when the animation completes
                _spineAnimationState.GetCurrent(2).Complete += OnAttackAnimationComplete;
}
public void VoidPose()
{
    if (currentAnimationName != voidposSAnimationName)
                {
                    Stop();
                    Instantiate(S_Void, top.transform.position, S_Void.transform.rotation);
                    _spineAnimationState.SetAnimation(2, voidposSAnimationName, false);
                    currentAnimationName = voidposSAnimationName;
                   // Debug.Log("Combo Count: " + comboCount + ", Playing Animation: combo_1");
                }
                // Add event listener for when the animation completes
                _spineAnimationState.GetCurrent(2).Complete += OnAttackAnimationComplete;
}
public void WindPose()
{
    if (currentAnimationName != windposSAnimationName)
                {
                    Stop();
                    Instantiate(S_Wind, top.transform.position, S_Wind.transform.rotation);
                    _spineAnimationState.SetAnimation(2, windposSAnimationName, false);
                    currentAnimationName = windposSAnimationName;
                   // Debug.Log("Combo Count: " + comboCount + ", Playing Animation: combo_1");
                }
                // Add event listener for when the animation completes
                _spineAnimationState.GetCurrent(2).Complete += OnAttackAnimationComplete;
}
#endregion

/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
//Complete Animations

#region Pose 
public void NormalPoseC()
{
    if (currentAnimationName != idleSwordAnimationName)
                {
                    _spineAnimationState.SetAnimation(1, idleSwordAnimationName, true);
                    currentAnimationName = idleSwordAnimationName;
                }
}
public void FirePoseC()
{
    if (currentAnimationName != fireposAnimationName)
                {
                    _spineAnimationState.SetAnimation(1, fireposAnimationName, true);
                    currentAnimationName = fireposAnimationName;
                }
}
public void RockPoseC()
{
    if (currentAnimationName != rockposAnimationName)
                {
                    _spineAnimationState.SetAnimation(1, rockposAnimationName, true);
                    currentAnimationName = rockposAnimationName;
                }
}
public void WaterPoseC()
{
    if (currentAnimationName != waterposAnimationName)
                {
                    _spineAnimationState.SetAnimation(1, waterposAnimationName, true);
                    currentAnimationName = waterposAnimationName;
                }
}
public void VoidPoseC()
{
    if (currentAnimationName != voidposAnimationName)
                {
                    _spineAnimationState.SetAnimation(1, voidposAnimationName, true);
                    currentAnimationName = voidposAnimationName;
                }
}
public void WindPoseC()
{
    if (currentAnimationName != windposAnimationName)
                {
                    _spineAnimationState.SetAnimation(1, windposAnimationName, true);
                    currentAnimationName = windposAnimationName;
                }
}
#endregion

/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

#region Kiai Animazioni

public void KiaiNormal()
{
    if (currentAnimationName != KiaiNormalAnimationName)
                {
                    _spineAnimationState.SetAnimation(2, KiaiNormalAnimationName, false);
                    currentAnimationName = KiaiNormalAnimationName;
                }
                _spineAnimationState.GetCurrent(2).Complete += OnAttackAnimationComplete;
}
public void KiaiRock()
{
    if (currentAnimationName != KiaiRockAnimationName)
                {
                    _spineAnimationState.SetAnimation(2, KiaiRockAnimationName, false);
                    currentAnimationName = KiaiRockAnimationName;
                }
                _spineAnimationState.GetCurrent(2).Complete += OnAttackAnimationComplete;
}
public void KiaiWater()
{
    if (currentAnimationName != KiaiWaterAnimationName)
                {
                    _spineAnimationState.SetAnimation(2, KiaiWaterAnimationName, false);
                    currentAnimationName = KiaiWaterAnimationName;
                }
                _spineAnimationState.GetCurrent(2).Complete += OnAttackAnimationComplete;
}
public void KiaiWind()
{
    if (currentAnimationName != KiaiWindAnimationName)
                {
                    _spineAnimationState.SetAnimation(2, KiaiWindAnimationName, false);
                    currentAnimationName = KiaiWindAnimationName;
                }
                _spineAnimationState.GetCurrent(2).Complete += OnAttackAnimationComplete;
}
public void KiaiVoid()
{
    if (currentAnimationName != KiaiVoidAnimationName)
                {
                    _spineAnimationState.SetAnimation(2, KiaiVoidAnimationName, false);
                    currentAnimationName = KiaiVoidAnimationName;
                }
                _spineAnimationState.GetCurrent(2).Complete += OnAttackAnimationComplete;
}
public void KiaiFire()
{
    if (currentAnimationName != KiaiFireAnimationName)
                {
                    _spineAnimationState.SetAnimation(2, KiaiFireAnimationName, false);
                    currentAnimationName = KiaiFireAnimationName;
                }
                _spineAnimationState.GetCurrent(2).Complete += OnAttackAnimationComplete;
}
#endregion

/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
//Stalmate

#region Animazioni minigioco stallo
public void LoopStalmate()
{
    if (currentAnimationName != LoopStAnimationName)
                {
                    _spineAnimationState.SetAnimation(2, LoopStAnimationName, true);
                    currentAnimationName = LoopStAnimationName;
                   // Debug.Log("Combo Count: " + comboCount + ", Playing Animation: combo_1");
                }
                // Add event listener for when the animation completes
                _spineAnimationState.GetCurrent(2).Complete += OnAttackAnimationComplete;
}
public void StartStalmate()
{
    if (currentAnimationName != StartStAnimationName)
                {
                    _spineAnimationState.SetAnimation(2, StartStAnimationName, false);
                    currentAnimationName = StartStAnimationName;
                   // Debug.Log("Combo Count: " + comboCount + ", Playing Animation: combo_1");
                }
                // Add event listener for when the animation completes
                _spineAnimationState.GetCurrent(2).Complete += OnAttackAnimationComplete;
}
public void poseStalmate()
{
    if (currentAnimationName != rockposAnimationName)
                {
                    _spineAnimationState.SetAnimation(2, rockposAnimationName, true);
                    currentAnimationName = rockposAnimationName;
                }
}
#endregion

////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
//Fatality Anim

#region Fatality Animation Function
public void Fatality(int watF)
{
switch (watF) {
        case 0:
         if (currentAnimationName != BossDSpAnimationName)
                {
                    _spineAnimationState.SetAnimation(2, BossDSpAnimationName, false);
                    currentAnimationName = BossDSpAnimationName;
                   // Debug.Log("Combo Count: " + comboCount + ", Playing Animation: combo_1");
                }
                // Add event listener for when the animation completes
                _spineAnimationState.GetCurrent(2).Complete += OnAttackAnimationComplete;
                break;
        case 1:
         if (currentAnimationName != F_JumpAnimationName)
                {
                    _spineAnimationState.SetAnimation(2, F_JumpAnimationName, false);
                    currentAnimationName = F_JumpAnimationName;
                   // Debug.Log("Combo Count: " + comboCount + ", Playing Animation: combo_1");
                }
                // Add event listener for when the animation completes
                _spineAnimationState.GetCurrent(2).Complete += OnAttackAnimationComplete;
                break;
        case 2:
         if (currentAnimationName != F_DanceAnimationName)
                {
                    _spineAnimationState.SetAnimation(2, F_DanceAnimationName, false);
                    currentAnimationName = F_DanceAnimationName;
                   // Debug.Log("Combo Count: " + comboCount + ", Playing Animation: combo_1");
                }
                // Add event listener for when the animation completes
                _spineAnimationState.GetCurrent(2).Complete += OnAttackAnimationComplete;
                break;
        case 3:
         if (currentAnimationName != F_BackAnimationName)
                {
                    _spineAnimationState.SetAnimation(2, F_BackAnimationName, false);
                    currentAnimationName = F_BackAnimationName;
                   // Debug.Log("Combo Count: " + comboCount + ", Playing Animation: combo_1");
                }
                // Add event listener for when the animation completes
                _spineAnimationState.GetCurrent(2).Complete += OnAttackAnimationComplete;
                break;
        case 4:
         if (currentAnimationName != LanceFAnimationName)
                {
                    _spineAnimationState.SetAnimation(2, LanceFAnimationName, false);
                    currentAnimationName = LanceFAnimationName;
                   // Debug.Log("Combo Count: " + comboCount + ", Playing Animation: combo_1");
                }
                // Add event listener for when the animation completes
                _spineAnimationState.GetCurrent(2).Complete += OnAttackAnimationComplete;
                break;
        case 5:
         if (currentAnimationName != VeloceFAnimationName)
                {
                    _spineAnimationState.SetAnimation(2, VeloceFAnimationName, false);
                    currentAnimationName = VeloceFAnimationName;
                   // Debug.Log("Combo Count: " + comboCount + ", Playing Animation: combo_1");
                }
                // Add event listener for when the animation completes
                _spineAnimationState.GetCurrent(2).Complete += OnAttackAnimationComplete;
                break;

}
}
#endregion

/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

#region Combo attacchi animazioni
public void AddCombo()
{
    //Se sta attaccando
    if (isAttacking)
    {
        //Il contatore aumenta ogni volta che si preme il tasto
        comboCount++;

//Normal style
#region Normal
if(GameplayManager.instance.styleIcon[0] == true){
if(style == 0)
{
        switch (comboCount)
        {
            //Setta lo stato d'animazione ed esegue l'animazione in base al conto della combo
            case 1:
                if (currentAnimationName != attackNormal1AnimationName)
                {Stop();
                        
                    _spineAnimationState.SetAnimation(2, attackNormal1AnimationName, false);
                    currentAnimationName = attackNormal1AnimationName;
                    _spineAnimationState.Event += HandleEvent;

                    //Debug.Log("Combo Count: " + comboCount + ", Playing Animation: combo_1");
                }
                // Add event listener for when the animation completes
                _spineAnimationState.GetCurrent(2).Complete += OnAttackAnimationComplete;
                break;
            case 2:
                if (currentAnimationName != attackNormal2AnimationName)
                {Stop();
                    _spineAnimationState.SetAnimation(2, attackNormal2AnimationName, false);
                    currentAnimationName = attackNormal2AnimationName;
                    _spineAnimationState.Event += HandleEvent;

                    //Debug.Log("Combo Count: " + comboCount + ", Playing Animation: combo_1");
                }
                // Add event listener for when the animation completes
                _spineAnimationState.GetCurrent(2).Complete += OnAttackAnimationComplete;
                break;
            case 3:
            if (currentAnimationName != attackNormal3AnimationName)
                {Stop();
                    _spineAnimationState.SetAnimation(2, attackNormal3AnimationName, false);
                    currentAnimationName = attackFire3AnimationName;
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
#endregion
//Rock style
#region Rock
if(GameplayManager.instance.styleIcon[1] == true){
if(style == 1)
{
        switch (comboCount)
        {
            //Setta lo stato d'animazione ed esegue l'animazione in base al conto della combo
            case 1:
                if (currentAnimationName != attackRock1AnimationName)
                {Stop();
                    _spineAnimationState.SetAnimation(2, attackRock1AnimationName, false);
                    currentAnimationName = attackRock1AnimationName;
                    _spineAnimationState.Event += HandleEvent;

                    //Debug.Log("Combo Count: " + comboCount + ", Playing Animation: combo_1");
                }
                // Add event listener for when the animation completes
                _spineAnimationState.GetCurrent(2).Complete += OnAttackAnimationComplete;
                break;
            case 2:
                if (currentAnimationName != attackRock2AnimationName)
                {Stop();
                    _spineAnimationState.SetAnimation(2, attackRock2AnimationName, false);
                    currentAnimationName = attackRock2AnimationName;
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
#endregion
//Fire style
#region Fire
if(GameplayManager.instance.styleIcon[2] == true){
if(style == 2)
{
        switch (comboCount)
        {
            //Setta lo stato d'animazione ed esegue l'animazione in base al conto della combo
            case 1:
                if (currentAnimationName != attackFire1AnimationName)
                {Stop();
                    _spineAnimationState.SetAnimation(2, attackFire1AnimationName, false);
                    currentAnimationName = attackFire1AnimationName;
                    _spineAnimationState.Event += HandleEvent;

                    //Debug.Log("Combo Count: " + comboCount + ", Playing Animation: combo_1");
                }
                // Add event listener for when the animation completes
                _spineAnimationState.GetCurrent(2).Complete += OnAttackAnimationComplete;
                break;
            case 2:
                if (currentAnimationName != attackFire2AnimationName)
                {Stop();
                    _spineAnimationState.SetAnimation(2, attackFire2AnimationName, false);
                    currentAnimationName = attackFire2AnimationName;
                    _spineAnimationState.Event += HandleEvent;

                    //Debug.Log("Combo Count: " + comboCount + ", Playing Animation: combo_1");
                }
                // Add event listener for when the animation completes
                _spineAnimationState.GetCurrent(2).Complete += OnAttackAnimationComplete;
                break;
            case 3:
            if (currentAnimationName != attackFire3AnimationName)
                {Stop();
                    _spineAnimationState.SetAnimation(2, attackFire3AnimationName, false);
                    currentAnimationName = attackFire3AnimationName;
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
#endregion
//Wind style
#region Wind
if(GameplayManager.instance.styleIcon[3] == true){
if(style == 3)
{
        switch (comboCount)
        {
            //Setta lo stato d'animazione ed esegue l'animazione in base al conto della combo
            case 1:
                if (currentAnimationName != attackWind1AnimationName)
                {
                    Stop();
                    PlayerHealth.Instance.currentStamina -= 5;
                    _spineAnimationState.SetAnimation(2, attackWind1AnimationName, false);
                    currentAnimationName = attackWind1AnimationName;
                    _spineAnimationState.Event += HandleEvent;
                    //Debug.Log("Combo Count: " + comboCount + ", Playing Animation: combo_1");
                }
                // Add event listener for when the animation completes
                _spineAnimationState.GetCurrent(2).Complete += OnAttackAnimationComplete;
                break;
            case 2:
                if (currentAnimationName != attackWind2AnimationName)
                {
                    Stop();
                    PlayerHealth.Instance.currentStamina -= 5;
                    _spineAnimationState.SetAnimation(2, attackWind2AnimationName, false);
                    currentAnimationName = attackWind2AnimationName;
                    _spineAnimationState.Event += HandleEvent;

                    //Debug.Log("Combo Count: " + comboCount + ", Playing Animation: combo_1");
                }
                // Add event listener for when the animation completes
                _spineAnimationState.GetCurrent(2).Complete += OnAttackAnimationComplete;
                break;
            case 3:
            if (currentAnimationName != attackWind3AnimationName)
                {
                    Stop();
                    PlayerHealth.Instance.currentStamina -= 5;
                    _spineAnimationState.SetAnimation(2, attackWind3AnimationName, false);
                    currentAnimationName = attackWind3AnimationName;
                    _spineAnimationState.Event += HandleEvent;

                    //Debug.Log("Combo Count: " + comboCount + ", Playing Animation: combo_1");
                }
                // Add event listener for when the animation completes
                _spineAnimationState.GetCurrent(2).Complete += OnAttackAnimationComplete;
                
                break;
            case 4:
            //Lunge
                if (currentAnimationName != attackWind4AnimationName)
                {
                    Stop();
                    PlayerHealth.Instance.currentStamina -= 5;
                    _spineAnimationState.SetAnimation(2, attackWind4AnimationName, false);
                    currentAnimationName = attackWind4AnimationName;
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
#endregion
//Water style
#region Water
if(GameplayManager.instance.styleIcon[4] == true){
if(style == 4)
{
        switch (comboCount)
        {
            //Setta lo stato d'animazione ed esegue l'animazione in base al conto della combo
            case 1:
                if (currentAnimationName != attackWater1AnimationName)
                {Stop();
                    _spineAnimationState.SetAnimation(2, attackWater1AnimationName, false);
                    currentAnimationName = attackWater1AnimationName;
                    _spineAnimationState.Event += HandleEvent;

                    //Debug.Log("Combo Count: " + comboCount + ", Playing Animation: combo_1");
                }
                // Add event listener for when the animation completes                
                _spineAnimationState.GetCurrent(2).Complete += OnAttackAnimationComplete;
                break;
            case 2:
                if (currentAnimationName != attackWater2AnimationName)
                {Stop();
                    _spineAnimationState.SetAnimation(2, attackWater2AnimationName, false);
                    currentAnimationName = attackWater2AnimationName;
                    _spineAnimationState.Event += HandleEvent;

                    //Debug.Log("Combo Count: " + comboCount + ", Playing Animation: combo_1");
                }
                // Add event listener for when the animation completes                
                _spineAnimationState.GetCurrent(2).Complete += OnAttackAnimationComplete;
                break;
            case 3:
                if (currentAnimationName != attackWater3AnimationName)
                {Stop();
                    _spineAnimationState.SetAnimation(2, attackWater3AnimationName, false);
                    currentAnimationName = attackWater3AnimationName;
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
#endregion
//Void style
#region Void
if(GameplayManager.instance.styleIcon[5] == true){
if(style == 5)
{
        switch (comboCount)
        {
            //Setta lo stato d'animazione ed esegue l'animazione in base al conto della combo
            case 1:
                if (currentAnimationName != attackVoid1AnimationName)
                {Stop();
                    _spineAnimationState.SetAnimation(2, attackVoid1AnimationName, false);
                    currentAnimationName = attackVoid1AnimationName;
                    _spineAnimationState.Event += HandleEvent;

                    //Debug.Log("Combo Count: " + comboCount + ", Playing Animation: combo_1");
                }
                // Add event listener for when the animation completes
                _spineAnimationState.GetCurrent(2).Complete += OnAttackAnimationComplete;
                break;
            case 2:
                if (currentAnimationName != attackVoid2AnimationName)
                {Stop();
                    _spineAnimationState.SetAnimation(2, attackVoid2AnimationName, false);
                    currentAnimationName = attackVoid2AnimationName;
                    _spineAnimationState.Event += HandleEvent;

                    //Debug.Log("Combo Count: " + comboCount + ", Playing Animation: combo_1");
                }
                // Add event listener for when the animation completes
                _spineAnimationState.GetCurrent(2).Complete += OnAttackAnimationComplete;
                break;
            case 3:
            if (currentAnimationName != attackVoid3AnimationName)
                {Stop();
                    _spineAnimationState.SetAnimation(2, attackVoid3AnimationName, false);
                    currentAnimationName = attackVoid3AnimationName;
                    _spineAnimationState.Event += HandleEvent;

                    //Debug.Log("Combo Count: " + comboCount + ", Playing Animation: combo_1");
                }
                // Add event listener for when the animation completes
                _spineAnimationState.GetCurrent(2).Complete += OnAttackAnimationComplete;
                
                break;
            case 4:
            //Lunge
                if (currentAnimationName != attackFire1AnimationName)
                {Stop();
                    _spineAnimationState.SetAnimation(2, attackFire1AnimationName, false);
                    currentAnimationName = attackFire1AnimationName;
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
#endregion
}}
#endregion

/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

#region Pose animazioni
private void OnAttackAnimationComplete(Spine.TrackEntry trackEntry)
{
    // Remove the event listener
    trackEntry.Complete -= OnAttackAnimationComplete;

    // Clear the track 1 and reset to the idle animation
    _spineAnimationState.ClearTrack(2);
    if(!drawsword)
            {
                
                if (currentAnimationName != idleAnimationName) 
                {
                    _spineAnimationState.SetAnimation(1, idleAnimationName, true);
                    currentAnimationName = idleAnimationName;
                }
                
            }
            else if(drawsword)
            {
                if(style == 0)
                {NormalPoseC();}
                else if(style == 1)
                {RockPoseC();}
                else if(style == 2)
                {FirePoseC();}
                else if(style == 3)
                {WindPoseC();}
                else if(style == 4)
                {WaterPoseC();}
                else if(style == 5)
                {VoidPoseC();}
            }
     // Reset the attack state
    isAttacking = false;
    //isAttackingAir = false;
    StartKiai = false;
    if (_skeletonAnimation != null)
    {
    _skeletonAnimation.timeScale = timeScale; // Impostare il valore di time scale
    }

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
#endregion

#region Danni, morte e respawn animazioni
public void AnmHurt()
{
             if (currentAnimationName != hurtAnimationName)
                {
                    drawsword = true;
                    PlayMFX(2);
                    _spineAnimationState.ClearTrack(2);
                    _spineAnimationState.SetAnimation(2, hurtAnimationName, false);
                    currentAnimationName = hurtAnimationName;
                    _spineAnimationState.Event += HandleEvent;
                }
                // Add event listener for when the animation completes
                _spineAnimationState.GetCurrent(2).Complete += OnAttackAnimationComplete;
}

public void BigHurt()
{
             if (currentAnimationName != BighurtAnimationName)
                {
                    drawsword = true;
                    PlayMFX(2);
                    _spineAnimationState.ClearTrack(2);
                    _spineAnimationState.SetAnimation(2, BighurtAnimationName, false);
                    currentAnimationName = BighurtAnimationName;
                    _spineAnimationState.Event += HandleEvent;
                }
                // Add event listener for when the animation completes
                _spineAnimationState.GetCurrent(2).Complete += OnAttackAnimationComplete;
}

public void death()
{
             if (currentAnimationName != deathAnimationName)
                {
                    _spineAnimationState.ClearTrack(1);
                    Stop();
                    _spineAnimationState.SetAnimation(2, deathAnimationName, false);
                    currentAnimationName = deathAnimationName;
                    _spineAnimationState.Event += HandleEvent;
                }
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
                }

}

public void repostsword()
{
    if (currentAnimationName != swordDownAnimationName)
                {
                    Stop();
                    _spineAnimationState.ClearTrack(2);
                    _spineAnimationState.SetAnimation(2, swordDownAnimationName, false);
                    currentAnimationName = swordDownAnimationName;
                    _spineAnimationState.Event += HandleEvent;
                }
                _spineAnimationState.GetCurrent(2).Complete += OnAttackAnimationComplete;
}

public void DrawSword()
{
    if (currentAnimationName != swordupAnimationName)
                {
                    Stop();
                    _spineAnimationState.ClearTrack(2);
                    _spineAnimationState.SetAnimation(2, swordupAnimationName, false);
                    currentAnimationName = swordupAnimationName;
                }
                _spineAnimationState.GetCurrent(2).Complete += OnAttackAnimationComplete;
}

public void AnimationRest()
{
    if (currentAnimationName != RestAnimationName)
                {
                    Stop();
                    _spineAnimationState.SetAnimation(2, RestAnimationName, false);
                    currentAnimationName = RestAnimationName;
                    _spineAnimationState.Event += HandleEvent;
                }
}
public void animationWakeup()
{
    if (currentAnimationName != UpAnimationName)
                {
                    Stop();
                    _spineAnimationState.SetAnimation(2, UpAnimationName, false);
                    currentAnimationName = UpAnimationName;
                    _spineAnimationState.Event += HandleEvent;
                }
                _spineAnimationState.GetCurrent(2).Complete += OnAttackAnimationComplete;
}

public void respawnWakeup()
{
    if (currentAnimationName != respawnAnimationName)
                {
                    _spineAnimationState.SetAnimation(2, respawnAnimationName, false);
                    currentAnimationName = respawnAnimationName;
                    _spineAnimationState.Event += HandleEvent;
                }
                _spineAnimationState.GetCurrent(2).Complete += OnAttackAnimationComplete;
}
#endregion

#region Movimento Animazioni
private void moving() {
   if(!isGuard || !isCharging || !JumpRock)
   {
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
                ParticleMove.Stop();
                if(!drawsword)
            {
                if (currentAnimationName != idleAnimationName) {
                    _spineAnimationState.SetAnimation(1, idleAnimationName, true);
                    currentAnimationName = idleAnimationName;
                }
            } else if(drawsword)
            {
                /*if (currentAnimationName != idleSwordAnimationName) {
                    _spineAnimationState.SetAnimation(1, idleSwordAnimationName, true);
                    currentAnimationName = idleSwordAnimationName;
                }*/
                if(style == 0)
                {NormalPoseC();}
                else if(style == 1)
                {RockPoseC();}
                else if(style == 2)
                {FirePoseC();}
                else if(style == 3)
                {WindPoseC();}
                else if(style == 4)
                {WaterPoseC();}
                else if(style == 5)
                {VoidPoseC();}
            }
            } else if (speed > runSpeedThreshold) {
                // Player is running
                ParticleMove.Play();
                if(!drawsword)
            {
                if (currentAnimationName != runAnimationName) {
                    _spineAnimationState.SetAnimation(1, runAnimationName, true);
                    currentAnimationName = runAnimationName;
                }
            } else if(drawsword)
            {
                if (currentAnimationName != runSwordAnimationName) {
                    _spineAnimationState.SetAnimation(1, runSwordAnimationName, true);
                    currentAnimationName = runSwordAnimationName;
                }
            }
            } else {
                // Player is walking
                if(!drawsword)
            {
                if (currentAnimationName != walkAnimationName) {
                    _spineAnimationState.SetAnimation(1, walkAnimationName, true);
                    currentAnimationName = walkAnimationName;
                }
            } else if(drawsword)
            {
                if (currentAnimationName != walkSwordAnimationName) {
                    _spineAnimationState.SetAnimation(1, walkSwordAnimationName, true);
                    currentAnimationName = walkSwordAnimationName;
                }
            }
            }
            break;

        case > 0:
            // Player is jumping
            vfxFall = true;
            if(!drawsword)
            {
            if (currentAnimationName != jumpAnimationName) {
                _spineAnimationState.SetAnimation(1, jumpAnimationName, true);
                currentAnimationName = jumpAnimationName;
            }} else if(drawsword)
            {
            if (currentAnimationName != jumpSwordAnimationName) {
                _spineAnimationState.SetAnimation(1, jumpSwordAnimationName, true);
                currentAnimationName = jumpSwordAnimationName;
            }}
            
            break;

        case < 0:
            // Player is falling
            vfxFall = true;
            if(!isTouchingWall)
            {
            if(!drawsword)
            {
            if (currentAnimationName != jumpDownAnimationName) {
                _spineAnimationState.SetAnimation(1, jumpDownAnimationName, true);
                currentAnimationName = jumpDownAnimationName;
            }
            } else if(drawsword)
            {
                
            if (currentAnimationName != jumpDownSwordAnimationName) {
                _spineAnimationState.SetAnimation(1, jumpDownSwordAnimationName, true);
                currentAnimationName = jumpDownSwordAnimationName;
            }
            }}else if(isTouchingWall)
            {wallSlide();}
            
            break;
    }
    }else{    _spineAnimationState.ClearTrack(1);}
    }else{    _spineAnimationState.ClearTrack(1);}
    }
    }
}
}
#endregion

//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
//MUSICA

#region  Musica
public void PlayMFX(int soundToPlay)
    {
        if (!sgmActive)
        {
            sgm[soundToPlay].pitch = Random.Range(.9f, 1.1f);
            sgm[soundToPlay].Play();
           // sgmActive = true;
        }
    }
public void StopMFX(int soundToPlay)
    {
        if (!sgmActive)
        {
            //sgm[soundToPlay].pitch = Random.Range(.9f, 1.1f);
            sgm[soundToPlay].Stop();
           // sgmActive = true;
        }
    }
#endregion

///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
//EVENTS
//stessi effetti

#region Eventi
 
void HandleEvent (TrackEntry trackEntry, Spine.Event e) {


//Normal VFX
    if (e.Data.Name == "slash_h2_normal") {     
    // Controlla se la variabile "SwSl" è stata inizializzata correttamente.
    
    if(!vfx)
        {
        Instantiate(attack_h2, slashpoint.position, attack_h2.transform.rotation);
        PlayMFX(1);
        vfx = true;
        }
    }
    
    if (e.Data.Name == "slash_h_normal") {     
    // Controlla se la variabile "SwSl" è stata inizializzata correttamente.
    if(!vfx)
        {
        Instantiate(attack_h, slashpoint.position, attack_h.transform.rotation);
        PlayMFX(1);       
        vfx = true;
        }
        
    }

    if (e.Data.Name == "slash_v_normal") {     
    // Controlla se la variabile "SwSl" è stata inizializzata correttamente.
    if(!vfx)
        {
        Instantiate(attack, slashpoint.position, attack.transform.rotation);
        PlayMFX(1);      
        vfx = true;
        }
       
    }

//Fire VFX
    if (e.Data.Name == "slash_h2_fire") {     
    // Controlla se la variabile "SwSl" è stata inizializzata correttamente.
    if(!vfx)
        {
        Instantiate(attack_f_h2, slashpoint.position, attack_f_h2.transform.rotation);
        PlayMFX(1);      
        vfx = true;
        }
        
    }
    
    if (e.Data.Name == "slash_h_fire") {     
    // Controlla se la variabile "SwSl" è stata inizializzata correttamente.
    if(!vfx)
        {
         Instantiate(attack_f_h, slashpoint.position, attack_f_h.transform.rotation);
        PlayMFX(1);    
        vfx = true;
        }
       
    }

    if (e.Data.Name == "slash_v_fire") {     
    // Controlla se la variabile "SwSl" è stata inizializzata correttamente.
    if(!vfx)
        {
         Instantiate(attack_f_v, slashpoint.position, attack_f_v.transform.rotation);
        PlayMFX(1);
        vfx = true;
        }
       
    }
//Water VFX
    if (e.Data.Name == "slash_h2_water") {     
    // Controlla se la variabile "SwSl" è stata inizializzata correttamente.
    if(!vfx)
        {
        Instantiate(attack_w_h2, slashpoint.position, attack_w_h2.transform.rotation);
        PlayMFX(1);
        vfx = true;
        }
        
    }
    
    if (e.Data.Name == "slash_h_water") {     
    // Controlla se la variabile "SwSl" è stata inizializzata correttamente.
    if(!vfx)
        {
        Instantiate(attack_w_h, slashpoint.position, attack_w_h.transform.rotation);
        PlayMFX(1);
        vfx = true;
        }
        
    }


    if (e.Data.Name == "slash_v_water") {     
    // Controlla se la variabile "SwSl" è stata inizializzata correttamente.
    if(!vfx)
        {
        Instantiate(attack_w_v, slashpoint.position, attack_w_v.transform.rotation);
        PlayMFX(1);
        vfx = true;
        }
        
    }
 if (e.Data.Name == "slash_v_water2") {     
    // Controlla se la variabile "SwSl" è stata inizializzata correttamente.
    if(!vfx)
        {
        Instantiate(attack_w_h, slashpoint.position, attack_w_h.transform.rotation);
        PlayMFX(1);
        vfx = true;
        }
        
    }
    if (e.Data.Name == "slash_v_water3") {     
    // Controlla se la variabile "SwSl" è stata inizializzata correttamente.
    if(!vfx)
        {
        Instantiate(attack_w_h2, slashpoint1.position, attack_w_h2.transform.rotation);
        PlayMFX(1);
        vfx = true;
        }
        
    }
if (e.Data.Name == "waterjump") {     
    if(!vfx)
        {
        Instantiate(attack_w_v, slashpoint.position, attack_w_v.transform.rotation);
        PlayMFX(1);
        vfx = true;
        }
        
    }

//Rock VFX
    if (e.Data.Name == "slash_h2_rock") {     
    // Controlla se la variabile "SwSl" è stata inizializzata correttamente.
    if(!vfx)
        {
        Instantiate(attack_r_h2, slashpoint.position, attack_r_h2.transform.rotation);
        PlayMFX(1);
        vfx = true;
        }
        
    }
    
    if (e.Data.Name == "slash_h_rock") {     
    // Controlla se la variabile "SwSl" è stata inizializzata correttamente.
    if(!vfx)
        {
       Instantiate(attack_r_h, slashpoint.position, attack_r_h.transform.rotation);
        PlayMFX(1);
        vfx = true;
        }
        
    }

    if (e.Data.Name == "slash_v_rock") {     
    // Controlla se la variabile "SwSl" è stata inizializzata correttamente.
    if(!vfx)
        {
       Instantiate(attack_r_v, slashpoint.position, attack_r_v.transform.rotation);
        PlayMFX(1);
        vfx = true;
        }
        
    }
//Wind VFX
    if (e.Data.Name == "slash_h2_wind") {     
    // Controlla se la variabile "SwSl" è stata inizializzata correttamente.
    if(!vfx)
        {
       Instantiate(attack_v_h2, slashpoint.position, attack_v_h.transform.rotation);
       if(PlayerHealth.Instance.currentStamina > 10)
        {
        Instantiate(VFXWindSlash_h, gun.position, VFXWindSlash_h.transform.rotation);
        }
        PlayMFX(1);
        vfx = true;
        }
        
    }
    
    if (e.Data.Name == "slash_h_wind") {     
    // Controlla se la variabile "SwSl" è stata inizializzata correttamente.
    if(!vfx)
        {
        Instantiate(attack_v_h, slashpoint.position, attack_v_h.transform.rotation);
        if(PlayerHealth.Instance.currentStamina > 10)
        {
        Instantiate(VFXWindSlash_h, gun.position, VFXWindSlash_h.transform.rotation);
        }
        PlayMFX(1);
        vfx = true;
        }
       
    }

    if (e.Data.Name == "slash_v_wind") {     
    // Controlla se la variabile "SwSl" è stata inizializzata correttamente.
    if(!vfx)
        {
        Instantiate(attack_v_v, slashpoint.position, attack_v_v.transform.rotation);
        if(PlayerHealth.Instance.currentStamina > 10)
        {
        Instantiate(VFXWindSlash, gun.position, VFXWindSlash.transform.rotation);
        }
        PlayMFX(1);
        vfx = true;
        }
       
    }
//Void VFX
    if (e.Data.Name == "slash_l_void") {     
    // Controlla se la variabile "SwSl" è stata inizializzata correttamente.
    if(!vfx)
        {
         Instantiate(attack_m_h2, slashpoint.position, attack_m_h2.transform.rotation);
        PlayMFX(1);
        vfx = true;
        }
        
    }
    
    if (e.Data.Name == "slash_h_void") {     
    // Controlla se la variabile "SwSl" è stata inizializzata correttamente.
    if(!vfx)
        {
        Instantiate(attack_m_h, slashpoint.position, attack_m_h.transform.rotation);
        PlayMFX(1);
        vfx = true;
        }
        
    }
if (e.Data.Name == "upNorm") {     
    // Controlla se la variabile "SwSl" è stata inizializzata correttamente.
    if(!vfx)
        {
        Instantiate(attack_air_up, top.position, attack_air_up.transform.rotation);
        PlayMFX(1);
        vfx = true;
        }
        
    }
if (e.Data.Name == "bottomNorm") {     
    // Controlla se la variabile "SwSl" è stata inizializzata correttamente.
    if(!vfx)
        {
        Instantiate(attack_air_bottom, bottom.position, attack_air_bottom.transform.rotation);
        PlayMFX(1);
        vfx = true;
        }
        
    }
 if (e.Data.Name == "upFire") {     
    // Controlla se la variabile "SwSl" è stata inizializzata correttamente.
    if(!vfx)
        {
        Instantiate(attack_f_air_up, top.position, attack_f_air_up.transform.rotation);
        PlayMFX(1);
        vfx = true;
        }
        
    }
if (e.Data.Name == "bottomFire") {     
    // Controlla se la variabile "SwSl" è stata inizializzata correttamente.
    if(!vfx)
        {
        Instantiate(attack_f_air_bottom, bottom.position, attack_f_air_bottom.transform.rotation);
        PlayMFX(1);
        vfx = true;
        }
        
    }
if (e.Data.Name == "upWind") {     
    // Controlla se la variabile "SwSl" è stata inizializzata correttamente.
    if(!vfx)
        {
        Instantiate(attack_v_air_up, top.position, attack_v_air_up.transform.rotation);
        if(PlayerHealth.Instance.currentStamina > 10)
        {
        Instantiate(VFXWindSlashTOP, top.position, VFXWindSlashTOP.transform.rotation);
        }
        PlayMFX(1);
        vfx = true;
        }
        
    }
if (e.Data.Name == "bottomWind") {     
    // Controlla se la variabile "SwSl" è stata inizializzata correttamente.
    if(!vfx)
        {
        Instantiate(attack_v_air_bottom, bottom.position, attack_v_air_bottom.transform.rotation);
        if(PlayerHealth.Instance.currentStamina > 10)
        {
        Instantiate(VFXWindSlashDOWN, bottom.position, VFXWindSlashDOWN.transform.rotation);
        }
        PlayMFX(1);
        vfx = true;
        }
        
    }
if (e.Data.Name == "RockLanding") {     
    // Controlla se la variabile "SwSl" è stata inizializzata correttamente.
    if(!vfx)
        {
        stopInput = false;    
        stomp = false;        
        PlayMFX(1);
        vfx = true;
        }
        
    }

if (e.Data.Name == "Rockjumpend") {     
    // Controlla se la variabile "SwSl" è stata inizializzata correttamente.
    if(!vfx)
        {
        Instantiate(VfxRock, slashpoint.position, attack.transform.rotation);
        PlayMFX(1);
        vfx = true;
        }
        
    }


    
    if (e.Data.Name == "slash_v_void") {     
    // Controlla se la variabile "SwSl" è stata inizializzata correttamente.
    if(!vfx)
        {
         Instantiate(attack_m_v, slashpoint.position, attack_m_v.transform.rotation);
        PlayMFX(1);
        vfx = true;
        }
       
    }

if (e.Data.Name == "StoopingRock") {     
    // Controlla se la variabile "SwSl" è stata inizializzata correttamente.
    if(!vfx)
        {
        attackRock = true;
        vfx = true;
        }
    }
if (e.Data.Name == "EndStoopingRock") {     
    // Controlla se la variabile "SwSl" è stata inizializzata correttamente.
    if(!vfx)
        {
        attackRock = false;
        vfx = true;
        }
    }
if (e.Data.Name == "StoopingWater") {     
    // Controlla se la variabile "SwSl" è stata inizializzata correttamente.
    if(!vfx)
        {
        attackWater = true;
        vfx = true;
        }
    }
if (e.Data.Name == "EndStoopingWater") {     
    // Controlla se la variabile "SwSl" è stata inizializzata correttamente.
    if(!vfx)
        {
        attackWater = false;
        vfx = true;
        }
    }



if (e.Data.Name == "ShakeCam") {     
    // Controlla se la variabile "SwSl" è stata inizializzata correttamente.
    if(!vfx)
        {
        GameplayManager.instance.sbam();
        isCrushRock = false;
        vfx = true;
        }
    }
//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
if (e.Data.Name == "Water_Special") {     
    // Controlla se la variabile "SwSl" è stata inizializzata correttamente.
    if(!vfx)
        {
        Instantiate(attack_w_sp, slashpoint.position, attack_w_sp.transform.rotation);
        PlayMFX(1);
        vfx = true;
        }
    }

if (e.Data.Name == "Wind_Special") {     
    // Controlla se la variabile "SwSl" è stata inizializzata correttamente.
    if(!vfx)
        {
        Instantiate(attack_v_sp, slashpoint.position, attack_v_sp.transform.rotation);
        PlayMFX(1);
        vfx = true;
        }
    }

if (e.Data.Name == "Wind_SpecialEnd") {     
    // Controlla se la variabile "SwSl" è stata inizializzata correttamente.
    if(!vfx)
        {
        WindSpecial = false;
        vfx = true;
        }
    }

if (e.Data.Name == "Fire_Special") {     
    // Controlla se la variabile "SwSl" è stata inizializzata correttamente.
    if(!vfx)
        {
        Instantiate(attack_f_sp, bottom.position, attack_f_sp.transform.rotation);
        PlayMFX(1);
        vfx = true;
        }
    }
if (e.Data.Name == "Normal_Special") {     
    // Controlla se la variabile "SwSl" è stata inizializzata correttamente.
    if(!vfx)
        {
        Instantiate(attack_n_sp, slashpoint.position, attack_n_sp.transform.rotation);
        PlayMFX(1);
        vfx = true;
        }
    }
if (e.Data.Name == "Void_Special") {     
    // Controlla se la variabile "SwSl" è stata inizializzata correttamente.
    if(!vfx)
        {
        Instantiate(attack_S_sp, slashpoint.position, attack_S_sp.transform.rotation);
        PlayMFX(1);
        vfx = true;
        }
    }
//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

if (e.Data.Name == "VFXKiaiFire") {     
    // Controlla se la variabile "SwSl" è stata inizializzata correttamente.
    if(!vfx)
        {
        Instantiate(S_FireK_hitbox, transform.position, transform.rotation);
        vfx = true;
        }
        
    }
    if (e.Data.Name == "VFXKiaiWater") {     
    // Controlla se la variabile "SwSl" è stata inizializzata correttamente.
    if(!vfx)
        {
        Instantiate(S_waterK_hitbox, Player.transform.position, transform.rotation);
        vfx = true;
        }
        
    }
    if (e.Data.Name == "VFXKiaiRock") {     
    // Controlla se la variabile "SwSl" è stata inizializzata correttamente.
    if(!vfx)
        {
        Instantiate(S_rockK_hitbox, bottom.transform.position, transform.rotation);
        vfx = true;
        }
        
    }
if (e.Data.Name == "VFXKiaiWind") {     
    // Controlla se la variabile "SwSl" è stata inizializzata correttamente.
    if(!vfx)
        {
        Instantiate(S_windK_hitbox, transform.position, transform.rotation);
        vfx = true;
        }
        
    }

if (e.Data.Name == "VFXKiaiNormal") {     
    // Controlla se la variabile "SwSl" è stata inizializzata correttamente.
    if(!vfx)
        {
        Instantiate(S_normalK_hitbox, slashpoint.position, transform.rotation);
        vfx = true;
        }
        
    }
if (e.Data.Name == "VFXKiaiVoid") {     
    // Controlla se la variabile "SwSl" è stata inizializzata correttamente.
    if(!vfx)
        {
        Instantiate(S_voidK_hitbox, Player.transform.position, transform.rotation);
        vfx = true;
        }
        
    }

if (e.Data.Name == "soundWalk") {
       if(isGuard || isCharging || isAttacking || isAttackingAir)
       {StopMFX(0);}
       else 
       {PlayMFX(0);}
    }
if (e.Data.Name == "soundRun") {
       if(isGuard || isCharging || isAttacking || isAttackingAir)
       {StopMFX(0);}
       else
       {PlayMFX(0);}
    }
if (e.Data.Name == "charge") {
         if(!vfx)
        {  
        Instantiate(charge, transform.position, transform.rotation);
        PlayMFX(3); 
        vfx = true;
        }
    }

    if (e.Data.Name == "pesante") {
        if(!vfx)
        {  
        Instantiate(pesante, slashpoint.position, pesante.transform.rotation);
        PlayMFX(1);
        vfx = true;
        }
    }
    if (e.Data.Name == "downslash") {
if(!vfx)
        {
       Instantiate(attack_air_bottom, slashpoint.position, attack_air_bottom.transform.rotation);
       PlayMFX(1);
        vfx = true;
        }
       
    }
    if (e.Data.Name == "upSlash") {
if(!vfx)
        {
       Instantiate(attack_air_up, slashpoint.position, attack_air_up.transform.rotation);
       PlayMFX(1);
        vfx = true;
        }
        
    }
   
if (e.Data.Name == "VFXHeal") {
if(!vfx)
        {
        Instantiate(VFXHeal, transform.position, transform.rotation);
       PlayMFX(3);
        vfx = true;
        }
        
    }

   #endregion
}

    #region Gizmos
private void OnDrawGizmos()
    {
    Gizmos.color = Color.red;
    // disegna un Gizmo che rappresenta il Raycast
    Gizmos.DrawLine(transform.position, transform.position + new Vector3(transform.localScale.x, 0, 0) * wallDistance);
     Gizmos.color = Color.blue;
    Gizmos.DrawWireSphere(bottom.transform.position, distanceFromGroundJR);
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


