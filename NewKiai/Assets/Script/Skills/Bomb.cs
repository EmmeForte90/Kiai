using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    [Header("Bullet")]
    [SerializeField] float bombSpeed = 10f;
    [SerializeField] GameObject Explode;
    [SerializeField] Transform prefabExp;
    private float lifeTime = 0.5f;
    float xSpeed;
    [SerializeField] int damage = 50;
    public Vector3 LaunchOffset;
    public string targetTag = "Enemy";
    private GameObject target;
    public Rigidbody2D rb;

      [Header("Audio")]
[SerializeField] AudioSource SExp;
[SerializeField] AudioSource SBomb;
    // Start is called before the first frame update
    void Start()
    {
            target = GameObject.FindWithTag(targetTag);
            Move.instance.Throw();
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
        }
        
        
    }

    void OnTriggerEnter2D(Collider2D other) 
    {
        if(other.gameObject.tag == "Enemy")
        //Se il proiettile tocca il nemico
        {            
            SExp.Play();

            Instantiate(Explode, transform.position, transform.rotation);
            IDamegable hit = other.GetComponent<IDamegable>();
            hit.Damage(damage);
            Destroy(gameObject);

        }
        if(other.gameObject.tag == "Ground")
        //Se il proiettile tocca il nemico
        {     
            SExp.Play();       
            Instantiate(Explode, transform.position, transform.rotation);
            Destroy(gameObject);
            //Viene distrutto
        }
        
    }
}
