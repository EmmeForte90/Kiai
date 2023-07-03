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
    [SerializeField] GameObject Enemy;


    [Header("HP")]

    private int vitalita;
    public int vitalita_max = 50;
    
    public bool isDead = false;

    [Header("Stamina")]
    [Tooltip("Il nemico possiede una stamina?")]
    public bool IsStamina = false;
    private float stamina;
    public float stamina_max = 50;
    public float DamageStamina;
    [SerializeField] GameObject Staminaobj;
    [SerializeField] GameObject StaminaVFX;

    public Scrollbar staminaBar;
    private float horizontal;

    private bool facingR = true;

    [Header("Knockback")]
    [Tooltip("Il nemico fa knockback?")]
    public bool isKnock = false;

    private bool KnockbackAt = false;
    private bool KnockbackAtL = false;
    public float knockForceShort = 3f;
    public float knockForceLong = 5f;
    private float dashTime;
    [SerializeField] private Rigidbody2D rb;
    
    [Header("Timer")]

    [Header("VFX")]

    [SerializeField] public Transform slashpoint;
    [SerializeField] public Transform hitpoint;
    [SerializeField] GameObject attack;
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
    public float coinSpawnDelay = 5f; // ritardo tra la spawn di ogni moneta
    private int randomChance;
    private float coinForce = 5f; // forza con cui le monete saltano
    private Vector2 coinForceVariance = new Vector2(1, 0); // varianza della forza con cui le monete saltano
    private int coinCount; // conteggio delle monete
    
    [Header("Animations")]

    private Animator Anm;
    private SkeletonMecanim skeletonAnimation;
    private string stato;
    //private bool  anmDie = false;
    //[SpineAnimation][SerializeField] string idle;
    //[SpineAnimation][SerializeField] string walk;
//    [SpineAnimation][SerializeField] string tired;
    //[SpineAnimation][SerializeField] string guard;
   // [SpineAnimation][SerializeField] string idle_battle;
   // [SpineAnimation][SerializeField] string die;

    // Start is called before the first frame update
    void Start(){
        vitalita = vitalita_max;
        if(IsStamina)
        {stamina = stamina_max;}
        toy = GameObject.FindWithTag("Player");
        skeletonAnimation = GetComponent<SkeletonMecanim>();
        Anm =  GetComponent<Animator>();
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
        Flip();
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
        stato = "tired";
        StaminaVFX.gameObject.SetActive(false);
        if(stato == "tired"){
        Anm.SetBool("tired", true);        
        StartCoroutine(ripristina_Stamina());}
        }
        }
        }
        ////////////////////////////////////////////////
        
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
                    vitalita -= 5; 
                    PlayMFX(2);
                    stamina -= 10;
                    stato = "guard";
                    if(stato == "guard")
                    {Anm.SetBool("def", true);}
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

#region Die
private void Die()
    {
        SpawnCoins();
        KiaiGive();
        stato = "die";
        Anm.SetBool("die1", true);
          /*if (horizontal < 0)
        {Anm.SetBool("die1", true);}
        else if (horizontal > 0)
        {Anm.SetBool("die2", true);}
         else if (horizontal == 0)
        {if (rb.transform.localScale.x == -1)
        {Anm.SetBool("die3", true);}*/
        StartCoroutine(DestroyEnm());
    }

     private IEnumerator DestroyEnm(){
        yield return new WaitForSeconds(1f);
        Destroy(gameObject);
    }
#endregion

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
 private void Damage()
    {
        vitalita -= HitboxPlayer.Instance.Damage; 
                    PlayMFX(1);
                    skeletonAnimation.Skeleton.SetColor(Color.red);
                    Instantiate(VFXHurt, hitpoint.position, transform.rotation);
                    StartCoroutine(ripristina_colore());
                    if(isKnock){ KnockbackAtL = true;}
    }
private IEnumerator ripristina_Stamina(){
        yield return new WaitForSeconds(2f);
        stamina = stamina_max;
        stato = "idle_battle";
        if(stato == "idle_battle")
        {
        StaminaVFX.gameObject.SetActive(true);
        Anm.SetBool("def", false);        
        Anm.SetBool("tired", false);        
        }
    }

    private IEnumerator ripristina_Posa(){
        yield return new WaitForSeconds(1f);
        stato = "idle_battle";
        if(stato == "idle_battle")
        {
            Anm.SetBool("def", false);        
            Anm.SetBool("battle", true);        
        }
    }


private IEnumerator ripristina_colore(){
        yield return new WaitForSeconds(0.1f);
        skeletonAnimation.Skeleton.SetColor(Color.white);
    }


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

public void PlayMFX(int soundToPlay)
    {
        bgm[soundToPlay].Stop();
        // Imposta la pitch dell'AudioSource in base ai valori specificati.
        bgm[soundToPlay].pitch = basePitch + Random.Range(-randomPitchOffset, randomPitchOffset); 
        bgm[soundToPlay].Play();
    }

}
