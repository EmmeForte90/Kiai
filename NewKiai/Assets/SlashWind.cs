using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SlashWind : MonoBehaviour
{
    public float speed = 10f; // velocità del proiettile
    [SerializeField] GameObject Explode;
    [SerializeField] Transform prefabExp;
    //[SerializeField] int damage = 10;

    [SerializeField] float lifeTime = 0.5f;
    Rigidbody2D rb;

    [Header("Audio")]
    [HideInInspector] public float basePitch = 1f;
    [HideInInspector] public float randomPitchOffset = 0.1f;
    [SerializeField] public AudioClip[] listSound; // array di AudioClip contenente tutti i suoni che si vogliono riprodurre
    private AudioSource[] sgm; // array di AudioSource che conterrà gli oggetti AudioSource creati
    public AudioMixer SFX;
    private bool sgmActive = false;
    public bool isTop = false;
    public bool isBottom = false;

    // Start is called before the first frame update
    void Start()
    { 
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

        //Recupera i componenti del rigidbody
        rb = GetComponent<Rigidbody2D>();
        //Recupera i componenti dello script
        //La variabile è uguale alla scala moltiplicata la velocità del proiettile
        //Se il player si gira  anche lo spawn del proiettile farà lo stesso
        if(Move.instance.transform.localScale.x > 0 && !isTop && !isBottom)
        {
            rb.velocity = transform.right * speed;
            transform.localScale = new Vector3(1, 1, 1);
        } 
        else if(Move.instance.transform.localScale.x < 0 && !isTop && !isBottom)
        {
            rb.velocity = -transform.right * speed;
            transform.localScale = new Vector3(-1, 1, 1);
        }else if(isTop && !isBottom)
        {
            rb.velocity = transform.up * speed;
            transform.localScale = new Vector3(1, 1, 1);
        }else if(!isTop && isBottom)
        {
            rb.velocity = -transform.up * speed;
            transform.localScale = new Vector3(1, 1, 1);
        }
        Move.instance.Stop();
        PlaySFX(1);
    }
    
  public void PlaySFX(int soundToPlay)
    {
        sgm[soundToPlay].Play(); 
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Enemy")
        {  
            GameplayManager.instance.ComboCount();
            Instantiate(Explode, transform.position, transform.rotation);
            IDamegable hit = other.GetComponent<IDamegable>();
            hit.Damage(HitboxPlayer.Instance.Damage);
            PlaySFX(0);

        }

        if (other.gameObject.tag == "Ground")
        { 
            Invoke("Destroy", lifeTime);
            PlaySFX(2);
        }
       
    }

    private void Destroy()
    {
        Destroy(gameObject);
    }
}