using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tornado : MonoBehaviour
{
     [SerializeField] GameObject Explode;
    [SerializeField] Transform prefabExp;
        [SerializeField] int damage = 50;
    [SerializeField] float lifeTime = 0.5f;

[Header("Audio")]
    [SerializeField] AudioSource SExp;
    [SerializeField] AudioSource SBomb;
    // Start is called before the first frame update
    void Start()
    {
        //Move.instance.Tornado();
         Invoke("Destroy", lifeTime);
          
          // Imposta l'oggetto Tornado come figlio del player
    transform.parent = GameObject.FindGameObjectWithTag("Player").transform;
    }

     void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Enemy")
        {  
            Instantiate(Explode, transform.position, transform.rotation);
            IDamegable hit = other.GetComponent<IDamegable>();
            hit.Damage(damage);
            
        }
    }
     private void Destroy()
    {
        Destroy(gameObject);
    }
}
