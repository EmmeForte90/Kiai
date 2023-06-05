using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;


public class bullet_rule : MonoBehaviour
{
   [Header("Bullet")]
    public float velocita_proiettile_partenza = 15f;
    private float velocita_proiettile;
    public float random_range_velocita_proiettile = 3f;
    public GameObject Explode;
    public Transform prefabExp;
    public int damage = 10;
    public float potenza_alta_partenza=0.5f;
    private float potenza_alta;
    public float range_random_potenza_alta=0.1f;
    private GameObject target;
    public Rigidbody2D rb;
    public SpriteRenderer sprite;
    public bool bool_distruttibile=false;

    // Start is called before the first frame update
    void Start()
    {
        potenza_alta=potenza_alta_partenza;
        potenza_alta-=range_random_potenza_alta;
        potenza_alta+=Random.Range(0,(range_random_potenza_alta*2));

        velocita_proiettile=velocita_proiettile_partenza;  
        velocita_proiettile-=random_range_velocita_proiettile;
        velocita_proiettile+=Random.Range(0,(random_range_velocita_proiettile*2));

        target = GameObject.Find("Nekotaro");
        if(target.transform.position.x > transform.position.x){
            var direction = transform.right + new Vector3(0,potenza_alta,0);
            rb.AddForce(direction * velocita_proiettile, ForceMode2D.Impulse);
        }
        else{
            var direction = -transform.right + new Vector3(0,potenza_alta,0);
            rb.AddForce(direction * velocita_proiettile, ForceMode2D.Impulse);
        }
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

    void OnTriggerEnter2D(Collider2D other) {
        if ((other.name=="Hitbox")&&(bool_distruttibile)){distruggi_freccia();return;}
        if(other.gameObject.name == "Nekotaro"){
            //IDamegable hit = other.GetComponent<IDamegable>();
            //hit.Damage(damage);
            distruggi_freccia();
        }
        else {
            if(other.gameObject.tag == "Ground"){    
                distruggi_freccia();
            }
        }
    }

    private void distruggi_freccia(){
        Instantiate(Explode, transform.position, transform.rotation);
        Destroy(gameObject);
    }
}
