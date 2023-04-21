using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class Puntine : MonoBehaviour
{
    public float speed = 10f; // velocità del proiettile
    [SerializeField] GameObject Explode;
   // [SerializeField] Transform prefabExp;
    [SerializeField] int damage = 10;
    public float rotationSpeed = 2500f;

    [SerializeField] float lifeTime = 5f;
    Rigidbody2D rb;

    [Header("Audio")]
    [SerializeField] public AudioClip[] list; // array di AudioClip contenente tutti i suoni che si vogliono riprodurre
    private AudioSource[] sgm; // array di AudioSource che conterrà gli oggetti AudioSource creati
    private bool sgmActive = false;
    public AudioMixer SFX;

    // Start is called before the first frame update
    void Start()
    {
         sgm = new AudioSource[list.Length]; // inizializza l'array di AudioSource con la stessa lunghezza dell'array di AudioClip
        for (int i = 0; i < list.Length; i++) // scorre la lista di AudioClip
        {
            sgm[i] = gameObject.AddComponent<AudioSource>(); // crea un nuovo AudioSource come componente del game object attuale (quello a cui è attaccato lo script)
            sgm[i].clip = list[i]; // assegna l'AudioClip corrispondente all'AudioSource creato
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
        if(Move.instance.transform.localScale.x > 0)
        {
            rb.velocity = transform.right * speed;
                transform.localScale = new Vector3(1, 1, 1);
        } 
        else if(Move.instance.transform.localScale.x < 0)
        {
            rb.velocity = -transform.right * speed;
                transform.localScale = new Vector3(-1, 1, 1);

        }
        Move.instance.Throw();
        Move.instance.Stop();

        
    }

    public void PlaySFX(int soundToPlay)
    {
        if (!sgmActive)
        {
            sgm[soundToPlay].Play();
            sgmActive = true;
        }
    }
    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Vector3.forward, rotationSpeed * Time.deltaTime);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Enemy")
        {  
            Instantiate(Explode, transform.position, transform.rotation);
            IDamegable hit = other.GetComponent<IDamegable>();
            hit.Damage(damage);

            Invoke("Destroy", lifeTime);
            //Destroy(gameObject);
        }

        if (other.gameObject.tag == "Ground")
        { 
            Invoke("Destroy", lifeTime);
        }
       
    }
    private void Destroy()
    {
        Destroy(gameObject);
    }
}

