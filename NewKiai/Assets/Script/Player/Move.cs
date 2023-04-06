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
   // [SerializeField] GameObject blam;
    [SerializeField] public Transform gun;
    [SerializeField] public Transform top;
    [SerializeField] GameObject Circle;
    [SerializeField] public Transform circlePoint;
    [SerializeField] public Transform slashpoint;
    [SerializeField] GameObject VFXHeal;

    

   
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
        [Header("HpAnm")]

    [SpineAnimation][SerializeField] private string hurtAnimationName;
    [SpineAnimation][SerializeField] private string HealAnimationName;
    [SpineAnimation][SerializeField] private string deathAnimationName;
    [SpineAnimation][SerializeField] private string RestAnimationName;
    [SpineAnimation][SerializeField] private string respawnRestAnimationName;
    [SpineAnimation][SerializeField] private string UpAnimationName;
    [SpineAnimation][SerializeField] private string respawnAnimationName;
    ///////////////////////////////////////////////////////////////////////////
        [Header("NormalStyle")]
    [SerializeField] GameObject attack;
    [SerializeField] GameObject attack_h;
    [SerializeField] GameObject attack_h2;
    [SerializeField] GameObject attack_air_bottom;
    [SerializeField] GameObject attack_air_up;
    [SpineAnimation][SerializeField] private string attackNormal1AnimationName;
    [SpineAnimation][SerializeField] private string attackNormal2AnimationName;
    [SpineAnimation][SerializeField] private string attackNormal3AnimationName;
        [Header("Fire")]
    [SpineAnimation][SerializeField] private string fireposAnimationName;
    [SerializeField] GameObject attack_f_v;
    [SerializeField] GameObject attack_f_h;
    [SerializeField] GameObject attack_f_h2;
   // [SerializeField] GameObject attack_f_l;
   // [SerializeField] GameObject attack_f_b;
   // [SerializeField] GameObject attack_f_a;
    [SerializeField] GameObject attack_f_air_bottom;
    [SerializeField] GameObject attack_f_air_up;
    [SpineAnimation][SerializeField] private string attackFire1AnimationName;
    [SpineAnimation][SerializeField] private string attackFire2AnimationName;
    [SpineAnimation][SerializeField] private string attackFire3AnimationName;
        [Header("Water")]
    [SpineAnimation][SerializeField] private string waterposAnimationName;
    [SerializeField] GameObject attack_w_v;
    [SerializeField] GameObject attack_w_h;
    [SerializeField] GameObject attack_w_h2;
   // [SerializeField] GameObject attack_w_l;
   // [SerializeField] GameObject attack_w_b;
  //  [SerializeField] GameObject attack_w_a;
    [SerializeField] GameObject attack_w_air_bottom;
    [SerializeField] GameObject attack_w_air_up;
    [SpineAnimation][SerializeField] private string attackWater1AnimationName;
    [SpineAnimation][SerializeField] private string attackWater2AnimationName;
    [SpineAnimation][SerializeField] private string attackWater3AnimationName;
        [Header("Rock")]
    [SpineAnimation][SerializeField] private string rockposAnimationName;
    [SerializeField] GameObject attack_r_v;
    [SerializeField] GameObject attack_r_h;
    [SerializeField] GameObject attack_r_h2;
  //  [SerializeField] GameObject attack_r_l;
   // [SerializeField] GameObject attack_r_b;
   // [SerializeField] GameObject attack_r_a;
    [SerializeField] GameObject attack_r_air_bottom;
    [SerializeField] GameObject attack_r_air_up;
    [SerializeField] GameObject pesante;
    [SerializeField] GameObject charge;
    [SpineAnimation][SerializeField] private string attackRock1AnimationName;
    [SpineAnimation][SerializeField] private string attackRock2AnimationName;
    [SpineAnimation][SerializeField] private string attackRock3AnimationName;
        [Header("Wind")]
    [SpineAnimation][SerializeField] private string windposAnimationName;
    [SerializeField] GameObject attack_v_v;
    [SerializeField] GameObject attack_v_h;
    [SerializeField] GameObject attack_v_h2;
    //[SerializeField] GameObject attack_v_l;
   // [SerializeField] GameObject attack_v_b;
    //[SerializeField] GameObject attack_v_a;
    [SerializeField] GameObject attack_v_air_bottom;
    [SerializeField] GameObject attack_v_air_up;
    [SpineAnimation][SerializeField] private string attackWind1AnimationName;
    [SpineAnimation][SerializeField] private string attackWind2AnimationName;
    [SpineAnimation][SerializeField] private string attackWind3AnimationName;
    [SpineAnimation][SerializeField] private string attackWind4AnimationName;

        [Header("Void")]
    [SpineAnimation][SerializeField] private string voidposAnimationName;
    [SerializeField] GameObject attack_m_v;
    [SerializeField] GameObject attack_m_h;
    [SerializeField] GameObject attack_m_h2;
   // [SerializeField] GameObject attack_m_l;
    //[SerializeField] GameObject attack_m_b;
   // [SerializeField] GameObject attack_m_a;
    [SerializeField] GameObject attack_m_air_bottom;
    [SerializeField] GameObject attack_m_air_up;
    [SpineAnimation][SerializeField] private string attackVoid1AnimationName;
    [SpineAnimation][SerializeField] private string attackVoid2AnimationName;
    [SpineAnimation][SerializeField] private string attackVoid3AnimationName;
        [Header("JumpAtk")]


    [SpineAnimation][SerializeField] private string upatkjumpAnimationName;
    [SpineAnimation][SerializeField] private string downatkjumpAnimationName;
    /////////////////////////////////////////////////////////////////////
    [SpineAnimation][SerializeField] private string throwAnimationName;
    [SpineAnimation][SerializeField] private string bigthrowAnimationName;
    [SpineAnimation][SerializeField] private string SpecialAnimationName;
    [SpineAnimation][SerializeField] private string chargeAnimationName;
    [SpineAnimation][SerializeField] private string DashAttackAnimationName;
    [SpineAnimation][SerializeField] private string pesanteAnimationName;
    /////////////////////////////////////////////////////////////////////


private string currentAnimationName;

//private CharacterState currentState = CharacterState.Idle;
private int comboCount = 0;
     

    [Header("Attacks")]
    public int Damage;
    private float comboTimer; //imposta la durata del timer a 1 secondi
    public float comboDurata = 0.5f; //imposta la durata del timer a 1 secondi
    [SerializeField] public int comboCounter = 0; // contatore delle combo
    [SerializeField] float nextAttackTime = 0f;
    [SerializeField] float attackRate = 0.5f;
    [SerializeField] public float shootTimer = 2f; // tempo per completare una combo
    [SerializeField] private GameObject bullet;
    [SerializeField] private int style = 0;
    // Dichiarazione delle variabili
    private int MaxStyle;
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
    [HideInInspector] public float basePitch = 1f;
    [HideInInspector] public float randomPitchOffset = 0.1f;
    [SerializeField] public AudioClip[] listSound; // array di AudioClip contenente tutti i suoni che si vogliono riprodurre
    private AudioSource[] sgm; // array di AudioSource che conterrà gli oggetti AudioSource creati
    public AudioMixer SFX;
    private bool sgmActive = false;



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
        }        
        _spineAnimationState = GetComponent<Spine.Unity.SkeletonAnimation>().AnimationState;
        _spineAnimationState = _skeletonAnimation.AnimationState;
        _skeleton = _skeletonAnimation.skeleton;
        rb = GetComponent<Rigidbody2D>();
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

Debug.Log("AudioMixer aggiunto correttamente agli AudioSource.");


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
        style = MaxStyle;

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
/*    if(UpdateMenuRapido.Instance.Vbottom > 0 ||
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
        
    }*/
    
}
// ripristina la possibilità di attaccare dopo il tempo di attacco
if (!isBlast && Time.time >= nextAttackTime)
{
    isBlast = true;
}
////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////             

// gestione dell'input dello sparo
if (Input.GetButtonDown("R1"))
{
    //print("Hai premuto R1");
    MaxStyle++;
    changeStyle();
}

if (Input.GetButtonDown("L1"))
{
    //print("Hai premuto L1");
    MaxStyle--;
    changeStyle();
}
////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
/*if (isHeal && PlayerHealth.Instance.currentEssence == 0 || isDeath) 
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
}*/


////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
//Scelta della skill dal menu rapido
if (Input.GetButtonDown("SlotUp") || DpadY == 1)
{
    if (UpdateMenuRapido.Instance.idup > 0)
{
   UpdateMenuRapido.Instance.Selup();
    PlayerWeaponManager.instance.SetWeapon(SkillMenu.Instance.idup);

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

if (Input.GetButtonDown("Fire1") && !isAttacking && !isAttackingAir && !NotStrangeAnimationTalk && !isCharging) 
{
isAttacking = true;
AddCombo();

if(GameplayManager.instance.styleIcon[0] == true)
{if (style == 0 && comboCount == 3) //Normal
{comboCount = 0;}}

if(GameplayManager.instance.styleIcon[1] == true)
{if (style == 1 && comboCount == 2) //Rock
{comboCount = 0;}}

if(GameplayManager.instance.styleIcon[2] == true)
{if (style == 2 && comboCount == 3) //Fire
{comboCount = 0;}}

if(GameplayManager.instance.styleIcon[3] == true)
{if (style == 3 && comboCount == 4) //Wind
{comboCount = 0;}}

if(GameplayManager.instance.styleIcon[4] == true)
{if (style == 4 && comboCount == 1) //Water
{comboCount = 0;}}

if(GameplayManager.instance.styleIcon[5] == true)
{if (style == 5 && comboCount == 3) //Void
{comboCount = 0;}}
}

if (comboCount > 0) {
comboTimer -= Time.deltaTime; //decrementa il timer ad ogni frame
if (comboTimer <= 0f) {
comboCount = 0; //ripristina il comboCount a 0 quando il timer raggiunge 0
comboTimer = comboDurata; //riavvia il timer alla sua durata originale
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
              //  GameplayManager.instance.StyleActivated(TESTID);
              //  PlayerHealth.Instance.IncreaseEssence(10);
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

public void changeStyle()
{
     if(MaxStyle >= 5)
    {
        MaxStyle = 5;
        if(GameplayManager.instance.styleIcon[5] == true)
        {
        PlayerWeaponManager.instance.SetStyle(5);
        GameplayManager.instance.Selector.transform.position = GameplayManager.instance.StyleS[5].transform.position;
        VoidPose();
        }
    }
    else if(MaxStyle <= 0)
    {
        MaxStyle = 0;
    if(GameplayManager.instance.styleIcon[0] == true)
        {
        PlayerWeaponManager.instance.SetStyle(0);
        GameplayManager.instance.Selector.transform.position = GameplayManager.instance.StyleS[0].transform.position;
        NormalPose();
        }        
    }else if(MaxStyle == 1)
    {
    if(GameplayManager.instance.styleIcon[1] == true)
        {
        MaxStyle = 1;
        PlayerWeaponManager.instance.SetStyle(1);
        GameplayManager.instance.Selector.transform.position = GameplayManager.instance.StyleS[1].transform.position;
        RockPose();
        }        
    }else if(MaxStyle == 2)
    { 
    if(GameplayManager.instance.styleIcon[2] == true)
        {
        MaxStyle = 2;
        PlayerWeaponManager.instance.SetStyle(2);
        GameplayManager.instance.Selector.transform.position = GameplayManager.instance.StyleS[2].transform.position;
        FirePose();
        }        
    }else if(MaxStyle == 3)
    {
    if(GameplayManager.instance.styleIcon[3] == true)
        {
            MaxStyle = 3;
        PlayerWeaponManager.instance.SetStyle(3);
        GameplayManager.instance.Selector.transform.position = GameplayManager.instance.StyleS[3].transform.position;
        WindPose();
        }       
    }else if(MaxStyle == 4)
    {            
    if(GameplayManager.instance.styleIcon[4] == true)
        {
        MaxStyle = 4;
        PlayerWeaponManager.instance.SetStyle(4);
        GameplayManager.instance.Selector.transform.position = GameplayManager.instance.StyleS[4].transform.position;
        WaterPose();
        }       
    }else if(MaxStyle == 5)
    {    
    if(GameplayManager.instance.styleIcon[5] == true)
        {
        MaxStyle = 5;
        PlayerWeaponManager.instance.SetStyle(5);
        GameplayManager.instance.Selector.transform.position = GameplayManager.instance.StyleS[5].transform.position;
        VoidPose();
        }        
    }else if(MaxStyle == -1)
    {   
    if(GameplayManager.instance.styleIcon[0] == true)
        {
            MaxStyle = 0;
        PlayerWeaponManager.instance.SetStyle(0);
        GameplayManager.instance.Selector.transform.position = GameplayManager.instance.StyleS[0].transform.position;
        NormalPose();
        }        
    }
}


public void attackDash()
{
            attackNormal = true;
            coolDownTime = dashCoolDown;
            dashTime = dashDuration;
            DashAttack();        
}

/*public void attackupper()
{
            attackUpper = true;
            coolDownTime = dashCoolDown;
            dashTime = dashDuration;
            Upper();        
}*/


    private void FixedUpdate()
    {
        if(!GameplayManager.instance.PauseStop || !isAttacking || !isCharging || !touchGround || !isDashing || !isDeath)
        {


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
        //Swalk.Stop();
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
       // Instantiate(blam, gun.position, transform.rotation);
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
      //  Instantiate(blam, gun.position, transform.rotation);
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
       // Instantiate(blam, gun.position, transform.rotation);
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
     //   Instantiate(blam, gun.position, transform.rotation);
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

public void Blasting()
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
/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
public void NormalPose()
{
    if (currentAnimationName != idleAnimationName)
                {
                    _spineAnimationState.SetAnimation(2, idleAnimationName, true);
                    currentAnimationName = idleAnimationName;
                   // Debug.Log("Combo Count: " + comboCount + ", Playing Animation: combo_1");
                }
                // Add event listener for when the animation completes
               // _spineAnimationState.GetCurrent(2).Complete += OnAttackAnimationComplete;
}
public void FirePose()
{
    if (currentAnimationName != fireposAnimationName)
                {
                    _spineAnimationState.SetAnimation(2, fireposAnimationName, true);
                    currentAnimationName = fireposAnimationName;
                   // Debug.Log("Combo Count: " + comboCount + ", Playing Animation: combo_1");
                }
                // Add event listener for when the animation completes
                //_spineAnimationState.GetCurrent(2).Complete += OnAttackAnimationComplete;
}
public void RockPose()
{
    if (currentAnimationName != rockposAnimationName)
                {
                    _spineAnimationState.SetAnimation(2, rockposAnimationName, true);
                    currentAnimationName = rockposAnimationName;
                   // Debug.Log("Combo Count: " + comboCount + ", Playing Animation: combo_1");
                }
                // Add event listener for when the animation completes
                //_spineAnimationState.GetCurrent(2).Complete += OnAttackAnimationComplete;
}public void WaterPose()
{
    if (currentAnimationName != waterposAnimationName)
                {
                    _spineAnimationState.SetAnimation(2, waterposAnimationName, true);
                    currentAnimationName = waterposAnimationName;
                   // Debug.Log("Combo Count: " + comboCount + ", Playing Animation: combo_1");
                }
                // Add event listener for when the animation completes
               // _spineAnimationState.GetCurrent(2).Complete += OnAttackAnimationComplete;
}
public void VoidPose()
{
    if (currentAnimationName != voidposAnimationName)
                {
                    _spineAnimationState.SetAnimation(2, voidposAnimationName, true);
                    currentAnimationName = voidposAnimationName;
                   // Debug.Log("Combo Count: " + comboCount + ", Playing Animation: combo_1");
                }
                // Add event listener for when the animation completes
                //_spineAnimationState.GetCurrent(2).Complete += OnAttackAnimationComplete;
}
public void WindPose()
{
    if (currentAnimationName != windposAnimationName)
                {
                    _spineAnimationState.SetAnimation(2, windposAnimationName, true);
                    currentAnimationName = windposAnimationName;
                   // Debug.Log("Combo Count: " + comboCount + ", Playing Animation: combo_1");
                }
                // Add event listener for when the animation completes
                //_spineAnimationState.GetCurrent(2).Complete += OnAttackAnimationComplete;
}
/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
public void AddCombo()
{
    //Se sta attaccando
    if (isAttacking)
    {
        //Il contatore aumenta ogni volta che si preme il tasto
        comboCount++;

//Normal style
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
//Rock style
else if(style == 1)
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
            case 3:
            if (currentAnimationName != attackRock3AnimationName)
                {Stop();
                    _spineAnimationState.SetAnimation(2, attackRock3AnimationName, false);
                    currentAnimationName = attackRock3AnimationName;
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
    



//Fire style
else if(style == 2)
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






//Wind style
else if(style == 3)
{
        switch (comboCount)
        {
            //Setta lo stato d'animazione ed esegue l'animazione in base al conto della combo
            case 1:
                if (currentAnimationName != attackWind1AnimationName)
                {Stop();
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
                {Stop();
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
                {Stop();
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
                {Stop();
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


//Water style
else if(style == 4)
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



//Void style
else if(style == 5)
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
                    PlayMFX(2);
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

///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
//EVENTS
//Non puoi giocare di local scale sui vfx perché sono vincolati dal localscale del player PERò puoi giocare sulla rotazione E ottenere gli
//stessi effetti
 public void PlayMFX(int soundToPlay)
    {
        if (!sgmActive)
        {
            sgm[soundToPlay].Play();
            sgmActive = true;
        }
    }

void HandleEvent (TrackEntry trackEntry, Spine.Event e) {

if (e.Data.Name == "VFXpesante") {
        // Inserisci qui il codice per gestire l'evento.
        Instantiate(pesante, slashpoint.position, transform.rotation);
    }

//Normal VFX
    if (e.Data.Name == "slash_h2_normal") {     
    // Controlla se la variabile "SwSl" è stata inizializzata correttamente.
    
        Instantiate(attack_h2, slashpoint.position, attack_h2.transform.rotation);
        PlayMFX(1);
    }
    
    if (e.Data.Name == "slash_h_normal") {     
    // Controlla se la variabile "SwSl" è stata inizializzata correttamente.
    
        Instantiate(attack_h, slashpoint.position, attack_h.transform.rotation);
        PlayMFX(1);
    }

    if (e.Data.Name == "slash_v_normal") {     
    // Controlla se la variabile "SwSl" è stata inizializzata correttamente.
    
        Instantiate(attack, slashpoint.position, attack.transform.rotation);
        PlayMFX(1);
    }

//Fire VFX
    if (e.Data.Name == "slash_h2_fire") {     
    // Controlla se la variabile "SwSl" è stata inizializzata correttamente.
    
        Instantiate(attack_f_h2, slashpoint.position, attack_f_h2.transform.rotation);
        PlayMFX(1);
    }
    
    if (e.Data.Name == "slash_h_fire") {     
    // Controlla se la variabile "SwSl" è stata inizializzata correttamente.
    
        Instantiate(attack_f_h, slashpoint.position, attack_f_h.transform.rotation);
        PlayMFX(1);
    }

    if (e.Data.Name == "slash_v_fire") {     
    // Controlla se la variabile "SwSl" è stata inizializzata correttamente.
    
        Instantiate(attack_f_v, slashpoint.position, attack_f_v.transform.rotation);
        PlayMFX(1);
    }
//Water VFX
    if (e.Data.Name == "slash_h2_water") {     
    // Controlla se la variabile "SwSl" è stata inizializzata correttamente.
    
        Instantiate(attack_w_h2, slashpoint.position, attack_w_h2.transform.rotation);
        PlayMFX(1);
    }
    
    if (e.Data.Name == "slash_h_water") {     
    // Controlla se la variabile "SwSl" è stata inizializzata correttamente.
    
        Instantiate(attack_w_h, slashpoint.position, attack_w_h.transform.rotation);
        PlayMFX(1);
    }

    if (e.Data.Name == "slash_v_water") {     
    // Controlla se la variabile "SwSl" è stata inizializzata correttamente.
    
        Instantiate(attack_w_v, slashpoint.position, attack_w_v.transform.rotation);
        PlayMFX(1);
    }

//Rock VFX
    if (e.Data.Name == "slash_h2_rock") {     
    // Controlla se la variabile "SwSl" è stata inizializzata correttamente.
    
        Instantiate(attack_r_h2, slashpoint.position, attack_r_h2.transform.rotation);
        PlayMFX(1);
    }
    
    if (e.Data.Name == "slash_h_rock") {     
    // Controlla se la variabile "SwSl" è stata inizializzata correttamente.
    
        Instantiate(attack_r_h, slashpoint.position, attack_r_h.transform.rotation);
        PlayMFX(1);
    }

    if (e.Data.Name == "slash_v_rock") {     
    // Controlla se la variabile "SwSl" è stata inizializzata correttamente.
    
        Instantiate(attack_r_v, slashpoint.position, attack_r_v.transform.rotation);
        PlayMFX(1);
    }
//Wind VFX
    if (e.Data.Name == "slash_h2_wind") {     
    // Controlla se la variabile "SwSl" è stata inizializzata correttamente.
    
        Instantiate(attack_v_h2, slashpoint.position, attack_v_h.transform.rotation);
        PlayMFX(1);
    }
    
    if (e.Data.Name == "slash_h_wind") {     
    // Controlla se la variabile "SwSl" è stata inizializzata correttamente.
    
        Instantiate(attack_v_h, slashpoint.position, attack_v_h.transform.rotation);
        PlayMFX(1);
    }

    if (e.Data.Name == "slash_v_wind") {     
    // Controlla se la variabile "SwSl" è stata inizializzata correttamente.
    
        Instantiate(attack_v_v, slashpoint.position, attack_v_v.transform.rotation);
        PlayMFX(1);
    }
//Void VFX
    if (e.Data.Name == "slash_l_void") {     
    // Controlla se la variabile "SwSl" è stata inizializzata correttamente.
    
        Instantiate(attack_m_h2, slashpoint.position, attack_m_h2.transform.rotation);
        PlayMFX(1);
    }
    
    if (e.Data.Name == "slash_h_void") {     
    // Controlla se la variabile "SwSl" è stata inizializzata correttamente.
    
        Instantiate(attack_m_h, slashpoint.position, attack_m_h.transform.rotation);
        PlayMFX(1);
    }

    if (e.Data.Name == "slash_v_void") {     
    // Controlla se la variabile "SwSl" è stata inizializzata correttamente.
    
        Instantiate(attack_m_v, slashpoint.position, attack_m_v.transform.rotation);
        PlayMFX(1);
    }



if (e.Data.Name == "soundWalk") {
       PlayMFX(0);
    }
if (e.Data.Name == "soundRun") {
       PlayMFX(0);
    }
if (e.Data.Name == "SoundCharge") {
            
        Instantiate(charge, transform.position, transform.rotation);
        PlayMFX(4);
    }
if (e.Data.Name == "dash") {
            
    PlayMFX(5);

    }
    if (e.Data.Name == "downslash") {

        Instantiate(attack_air_bottom, slashpoint.position, attack_air_bottom.transform.rotation);
       PlayMFX(1);
    }
    if (e.Data.Name == "upSlash") {

        Instantiate(attack_air_up, slashpoint.position, attack_air_up.transform.rotation);
       PlayMFX(1);
    }
   
if (e.Data.Name == "VFXHeal") {

        Instantiate(VFXHeal, transform.position, transform.rotation);
       PlayMFX(3);
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


