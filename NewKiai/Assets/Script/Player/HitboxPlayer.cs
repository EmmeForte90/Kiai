using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitboxPlayer : MonoBehaviour
{
    [SerializeField] public Transform Pos;
    private bool take = false;
    //[SerializeField] AudioSource SClang;

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

////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
void OnTriggerEnter2D(Collider2D other) 
{
        if(other.gameObject.tag == "Enemy" || other.gameObject.tag == "Boss")
        //Se il proiettile tocca il nemico
        {       

if(GameplayManager.instance.styleIcon[0] == true)
{if (Move.instance.style == 0) //Normal
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
}}
///////////////////////
if(GameplayManager.instance.styleIcon[1] == true)
{if (Move.instance.style == 1) //Rock
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
}}
///////////////////////////////////
if(GameplayManager.instance.styleIcon[2] == true)
{if (Move.instance.style == 2) //Fire
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
}}
//////////////////////////////////
if(GameplayManager.instance.styleIcon[3] == true)
{if (Move.instance.style == 3) //Wind
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
}}
///////////////////////////////////
if(GameplayManager.instance.styleIcon[4] == true)
{if (Move.instance.style == 4) //Water
{
        IDamegable hit = other.GetComponent<IDamegable>();
            hit.Damage(Move.instance.Damage);
            //Debug.Log("Damage:" + Player.Damage);
            if(Move.instance.rb.velocity.y > 0)
            {               
                Move.instance.isBump = true;
                Move.instance.Bump();
            }
            StartCoroutine(StopD());
}}
////////////////////////////
if(GameplayManager.instance.styleIcon[5] == true)
{if (Move.instance.style == 5) //Void
{
     IDamegable hit = other.GetComponent<IDamegable>();
            hit.Damage(Move.instance.Damage);
            //Debug.Log("Damage:" + Player.Damage);
            if(Move.instance.rb.velocity.y > 0)
            {               
                Move.instance.isBump = true;
                Move.instance.Bump();
            }
            StartCoroutine(StopD()); 
}}
}
////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

         if(other.gameObject.tag == "Hitbox_E")
        //Se il proiettile tocca il nemico
        {       
            take = true;
            //SClang.Play();                   
            Move.instance.Knockback();
            if(Move.instance.rb.velocity.y > 0)
            {
                Move.instance.isBump = true;
                Move.instance.Bump();
            }
        }
}
////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
}    
