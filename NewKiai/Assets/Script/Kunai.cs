using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class Kunai : MonoBehaviour
{
    public float speed = 10f; // velocità del proiettile
    [SerializeField] GameObject Explode;
   // [SerializeField] Transform prefabExp;
    //[SerializeField] int damage = 50;
   

    [SerializeField] float lifeTime = 0.5f;
    Rigidbody2D rb;


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
        Move.instance.Throw();
        Move.instance.Stop();

        
    }

   

    void OnTriggerEnter2D(Collider2D other)
    {
        AudioManager.instance.PlaySFX(6);
        if (other.gameObject.tag == "Enemy")
        {  
            Instantiate(Explode, transform.position, transform.rotation);
            Destroy(gameObject);
            
        }

        if (other.gameObject.tag == "Ground")
        { 
            Instantiate(Explode, transform.position, transform.rotation);
            AudioManager.instance.PlaySFX(5);
            Invoke("Destroy", lifeTime);
        }
        
    }
    private void Destroy()
    {
        Destroy(gameObject);
    }
}

