using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class HealItem : MonoBehaviour
{
    [SerializeField] GameObject Explode;
   // [SerializeField] Transform prefabExp;
    [SerializeField] int heal = 50;

    [SerializeField] float lifeTime = 0.2f;
    [SerializeField] Rigidbody2D rb;



     void OnTriggerEnter2D(Collider2D other) 
{
        if(other.CompareTag("Player"))
        { Move.instance.AnimationHeal();
        Move.instance.Stop();
        AudioManager.instance.PlaySFX(4);
        Instantiate(Explode, transform.position, transform.rotation);
        PlayerHealth.Instance.IncreaseHP(heal);
        Invoke("Destroy", lifeTime); 
        }
}

   void OnTriggerStay2D(Collider2D other) 
{
        if(other.CompareTag("Player"))
        { Move.instance.AnimationHeal();
        Move.instance.Stop();
        AudioManager.instance.PlaySFX(4);
        Instantiate(Explode, transform.position, transform.rotation);
        PlayerHealth.Instance.IncreaseHP(heal);
        Invoke("Destroy", lifeTime); 
        }
}
    
    private void Destroy()
    {
        Destroy(gameObject);
    }
}


