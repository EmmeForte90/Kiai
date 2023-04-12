using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;


public class ArrowPlayer : MonoBehaviour
{
   [Header("Bullet")]
    [SerializeField] float bombSpeed = 10f;
    [SerializeField] GameObject Explode;
    [SerializeField] Transform prefabExp;
    float xSpeed;
    [SerializeField] int damage = 10;
    public Vector3 LaunchOffset;
    public string targetTag = "Enemy";
    [SerializeField] float lifeTime = 1f;
    private GameObject target;
    public Rigidbody2D rb;
    public SpriteRenderer sprite;


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
            target = GameObject.FindWithTag(targetTag);
            Move.instance.Bow();
            Move.instance.Stop();
            if( Move.instance.transform.localScale.x > 0)
            {
            var direction = transform.right + Vector3.up;
            rb.AddForce(direction * bombSpeed, ForceMode2D.Impulse);
            }
            else if( Move.instance.transform.localScale.x < 0)
            {
            var direction = -transform.right + Vector3.up;
            rb.AddForce(direction * bombSpeed, ForceMode2D.Impulse);
            }
            transform.Translate(LaunchOffset);

    }

    // Update is called once per frame
    void Update()
    {
        FlipSprite();
    }

    void FlipSprite()
    {
       bool bulletHorSpeed = Mathf.Abs(rb.velocity.x) > Mathf.Epsilon;
//se il player si sta muovendo le sue coordinate x sono maggiori di quelle e
//di un valore inferiore a 0
        if (bulletHorSpeed) //Se il player si sta muovendo
{
    transform.localScale = new Vector2 (Mathf.Sign(rb.velocity.x), 1f);
    //La scala assume un nuovo vettore e il rigidbody sull'asse x 
    //viene modificato mentre quello sull'asse y no.

    float angle = Mathf.Atan2(rb.velocity.y, rb.velocity.x) * Mathf.Rad2Deg;
    sprite.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
}    
        
    }

    void OnTriggerEnter2D(Collider2D other) 
    {
        if(other.gameObject.tag == "Enemy")
        //Se il proiettile tocca il nemico
        {            
            //SExp.Play();

            Instantiate(Explode, transform.position, transform.rotation);
            IDamegable hit = other.GetComponent<IDamegable>();
            hit.Damage(damage);
            Invoke("Destroy", lifeTime);

        }
        if(other.gameObject.tag == "Ground")
        //Se il proiettile tocca il nemico
        {     
            //SExp.Play();       
            Instantiate(Explode, transform.position, transform.rotation);
            Invoke("Destroy", lifeTime);
            //Viene distrutto
        }
        
    }

     private void Destroy()
    {
        Destroy(gameObject);
    }
}
