using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;
using Spine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;


public class TestHitbox : MonoBehaviour
{
    [Header("Animations")]
    [SpineAnimation][SerializeField] private string idleAnimationName;
    [SpineAnimation][SerializeField] private string HitAnimationName;
   
    private int none;

    private string currentAnimationName;
    private SkeletonAnimation _skeletonAnimation;
    private Spine.AnimationState _spineAnimationState;
    private Spine.Skeleton _skeleton;
    Spine.EventData eventData;
    public Rigidbody2D rb;
    public bool hit = false;
    public GameObject VFX;
    public Transform pos;

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
    private bool bgmActive = false;
    //[SerializeField] float lifeTime = 2f;

private void Awake()
    {

        _skeletonAnimation = GetComponent<SkeletonAnimation>();
        if (_skeletonAnimation == null) {
            Debug.LogError("Componente SkeletonAnimation non trovato!");
        }        
         _spineAnimationState = GetComponent<Spine.Unity.SkeletonAnimation>().AnimationState;
        _spineAnimationState = _skeletonAnimation.AnimationState;
        _skeleton = _skeletonAnimation.skeleton;
        rb = GetComponent<Rigidbody2D>();
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
void Update()
{ 
    /*
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
            */

    if(isKnockback)
{
        KnockTime -= Time.deltaTime; //decrementa il timer ad ogni frame
        if (KnockTime <= 0f) 
        {
        isKnockback = false; 
        rb.isKinematic = true;
        rb.velocity = new Vector2(0f, 0f);
        KnockTime = Knockmax;
        }
}
}

public void Damage(int damage)
{
    damage += none;
}
void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Hitbox" || other.gameObject.tag == "Throw")
        {  
            
            print("Hit the enm");
            Hit();
            //PlayerHealth.Instance.currentStamina -= 40;
            Instantiate(VFX, pos.transform.position, transform.rotation);
            PlayMFX(0);
            //hit = true;
            //Invoke("Resto", lifeTime);
            
        } else{Idle();}   
    }

private void Resto()
    {
       Idle();
        hit = false;
    }
public void StopMFX(int soundToPlay)
    {
        if (bgmActive)
        {
            bgm[soundToPlay].Stop();
            bgmActive = false;
        }
    }

public void PlayMFX(int soundToPlay)
    {
        bgm[soundToPlay].Stop();
        // Imposta la pitch dell'AudioSource in base ai valori specificati.
        bgm[soundToPlay].pitch = basePitch + Random.Range(-randomPitchOffset, randomPitchOffset); 
        bgm[soundToPlay].Play();
    }

public void Knockback()
    {
        if(isKnockback){
         // applica l'impulso del salto se il personaggio è a contatto con il terreno
        print("dovrebbefare il knockback");
         // applica l'impulso del salto se il personaggio è a contatto con il terreno
        if (transform.localScale.x < 0)
        {
        rb.AddForce(new Vector2(-knockForce, 2f), ForceMode2D.Impulse);
        }
        else if (transform.localScale.x > 0)
        {
        rb.AddForce(new Vector2(knockForce, 2f), ForceMode2D.Impulse);
        }
        //isKnockback = false;
        }
        
        //isKnockback = false;
        
    }

public void Idle()
{
             if (currentAnimationName != idleAnimationName)
                {  
                    _spineAnimationState.SetAnimation(1, idleAnimationName, true);
                    currentAnimationName = idleAnimationName;
                }            
}

public void Hit()
{
             if (currentAnimationName != HitAnimationName)
                { 
                    _spineAnimationState.ClearTrack(1);
                    _spineAnimationState.SetAnimation(1, HitAnimationName, false);
                    currentAnimationName = HitAnimationName;
                }
                // Add event listener for when the animation completes
                _spineAnimationState.GetCurrent(1).Complete += OnAttackAnimationComplete;
}

private void OnAttackAnimationComplete(Spine.TrackEntry trackEntry)
{
    // Remove the event listener
    trackEntry.Complete -= OnAttackAnimationComplete;

    // Clear the track 1 and reset to the idle animation
    _spineAnimationState.ClearTrack(1);
    _spineAnimationState.SetAnimation(1, idleAnimationName, true);
    currentAnimationName = idleAnimationName;

}
}
