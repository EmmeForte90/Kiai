using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlashGlobe : MonoBehaviour
{
   public float speed = 10f; // velocità del proiettile
    [SerializeField] GameObject Explode;
    [SerializeField] Transform prefabExp;
    [SerializeField] int damage = 50;

    [SerializeField] float lifeTime = 0.5f;
    Rigidbody2D rb;

    [Header("Audio")]
    [SerializeField] AudioSource SExp;
    [SerializeField] AudioSource SBomb;

    // Start is called before the first frame update
    void Start()
    {
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
         Move.instance.Slash();
        Move.instance.Stop();

    }
    

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Enemy")
        {  
            Instantiate(Explode, transform.position, transform.rotation);
            IDamegable hit = other.GetComponent<IDamegable>();
            hit.Damage(damage);
            
        }

        if (other.gameObject.tag == "Ground")
        { 
            Invoke("Destroy", lifeTime);
        }
        if (other.gameObject.tag == "Shield_E")
        { 
            Invoke("Destroy", lifeTime);
        }
    }

    private void Destroy()
    {
        Destroy(gameObject);
    }
}
