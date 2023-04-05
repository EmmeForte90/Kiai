using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowEnemy : MonoBehaviour
{
    public float speed = 10f; // velocità del proiettile
    [SerializeField] GameObject Explode;
    public float attackDamage = 10; // danno d'attacco
    [SerializeField] float lifeTime = 0.5f;
    Rigidbody2D rb;
    private Transform player;

 //   [Header("Audio")]
    //[SerializeField] AudioSource SExp;
    //[SerializeField] AudioSource SBomb;

    // Start is called before the first frame update
    void Start()
    {
        //Recupera i componenti del rigidbody
        rb = GetComponent<Rigidbody2D>();                
        player = GameObject.FindWithTag("Player").transform;
        //Recupera i componenti dello script
        //La variabile è uguale alla scala moltiplicata la velocità del proiettile
        //Se il player si gira  anche lo spawn del proiettile farà lo stesso
        if(AiEnemycrossbow.instance.transform.localScale.x > 0)
        {
            rb.velocity = transform.right * speed;
        } 
        else if(AiEnemycrossbow.instance.transform.localScale.x < 0)
        {
            rb.velocity = -transform.right * speed;
        }
       
    }
void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {  
            Instantiate(Explode, transform.position, transform.rotation);
            if (!Move.instance.isDeath)
            {
                if (!Move.instance.isHurt)
            {
            player.GetComponent<PlayerHealth>().Damage(attackDamage);
            }
            }
            Destroy(gameObject);

        }

        if (other.gameObject.tag == "Ground")
        { 
            Instantiate(Explode, transform.position, transform.rotation);
        Destroy(gameObject);
        }
       
    }

    
}
