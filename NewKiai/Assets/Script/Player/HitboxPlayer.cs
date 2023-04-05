using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitboxPlayer : MonoBehaviour
{
    //[SerializeField] GameObject Clang;
    [SerializeField] public Transform Pos;
    public bool pesante;
    public bool normal;
    private bool take = false;
    [SerializeField] AudioSource SClang;

public static HitboxPlayer Instance;


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

IEnumerator StopD()
    {
        yield return new WaitForSeconds(0.5f);
        take = false;
    }

void OnTriggerEnter2D(Collider2D other) 
    {
        if(other.gameObject.tag == "Enemy")
        //Se il proiettile tocca il nemico
        {       
        if(!take)
        {
            take = true;
            IDamegable hit = other.GetComponent<IDamegable>();
            hit.Damage(Move.instance.Damage);
            //Debug.Log("Damage:" + Player.Damage);
            if(Move.instance.rb.velocity.y > 0)
            {               
                Move.instance.isBump = true;
                Move.instance.Bump();
            }
            StartCoroutine(StopD());
        
        }

         if(other.gameObject.tag == "Boss")
        //Se il proiettile tocca il nemico
        {       
        if(!take)
        {
            take = true;
            IDamegable hit = other.GetComponent<IDamegable>();
            hit.Damage(Move.instance.Damage);
            //Debug.Log("Damage:" + Player.Damage);
            if(Move.instance.rb.velocity.y > 0)
            {               
                Move.instance.isBump = true;
                Move.instance.Bump();
            }
            StartCoroutine(StopD());
        
        }
         if(other.gameObject.tag == "Hitbox_E")
        //Se il proiettile tocca il nemico
        {       
            take = true;
            SClang.Play();
                            
            Move.instance.Knockback();

            if(Move.instance.rb.velocity.y > 0)
            {
                Move.instance.isBump = true;
                Move.instance.Bump();
            }

        }
        }
       
    }    
}
}