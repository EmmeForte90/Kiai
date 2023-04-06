using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Globo : MonoBehaviour
{
   public float speed = 10f; // velocità del proiettile
    [SerializeField] GameObject Explode;
   // [SerializeField] Transform prefabExp;
    [SerializeField] int damage = 50;
    public float rotationSpeed = 2500f;
    public bool isRotating = false;
    public bool Globe = false;
    public bool Needtwohands = false;
    public bool isBig = false;

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
        if(!Needtwohands)
        {
            Move.instance.Blasting();
        }
        else if(Needtwohands)
        {
            //Move.instance.Bigblast();
        }
        Move.instance.Stop();

        if(isBig)
        {
            transform.localScale *= 1.5f;
        }
    }
    
    // Update is called once per frame
    void Update()
    {
        if(isRotating)
        {
            transform.Rotate(Vector3.forward, rotationSpeed * Time.deltaTime);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Enemy")
        {  
            Instantiate(Explode, transform.position, transform.rotation);
            IDamegable hit = other.GetComponent<IDamegable>();
            hit.Damage(damage);
            if(Globe)
            {
                Destroy(gameObject);
            }
        }

        if (other.gameObject.tag == "Ground")
        { 
            Instantiate(Explode, transform.position, transform.rotation);
            Invoke("Destroy", lifeTime);
        }
        if (other.gameObject.tag == "Shield_E")
        { 

            if (!Needtwohands)
        { 
            Invoke("Destroy", lifeTime);
        }
    }
    }
    private void Destroy()
    {
        Destroy(gameObject);
    }
}

