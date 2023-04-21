using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitboxPlayer : MonoBehaviour
{
    [SerializeField] public Transform Pos;
    private bool CanTake = false;
    public float TimeHitbox; 
    public int Damage = 10;

    public static HitboxPlayer Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

private void Update()
    {
        if(CanTake){
        TimeHitbox -= Time.deltaTime; //decrementa il timer ad ogni frame
        if (TimeHitbox <= 0f) {
        CanTake = false; 
        }}
        
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
 if(!CanTake)
    {
            Debug.Log("Normal"+ Damage);

        TimeHitbox = 0.5f;
        GameplayManager.instance.ComboCount();
        CanTake = true;
        IDamegable hit = other.GetComponent<IDamegable>();
          hit.Damage(Damage);
            //Debug.Log("Damage:" + Player.Damage);
            if(Move.instance.rb.velocity.y > 0)
            {               
                Move.instance.isBump = true;
                Move.instance.Bump();
            }
    }
}}
///////////////////////
if(GameplayManager.instance.styleIcon[1] == true)
{if (Move.instance.style == 1) //Rock
{
 if(!CanTake)
    {
            Debug.Log("Rock"+ Damage);

        TimeHitbox = 2f;
        GameplayManager.instance.ComboCount();
        CanTake = true;
        IDamegable hit = other.GetComponent<IDamegable>();
           hit.Damage(Damage);
            //Debug.Log("Damage:" + Player.Damage);
            if(Move.instance.rb.velocity.y > 0)
            {               
                Move.instance.isBump = true;
                Move.instance.Bump();
            }
    }
}}
///////////////////////////////////
if(GameplayManager.instance.styleIcon[2] == true)
{if (Move.instance.style == 2) //Fire
{
 if(!CanTake)
    {
            Debug.Log("Fire"+ Damage);

        TimeHitbox = 0.7f;
        GameplayManager.instance.ComboCount();
        CanTake = true;
        IDamegable hit = other.GetComponent<IDamegable>();
          hit.Damage(Damage);
            //Debug.Log("Damage:" + Player.Damage);
            if(Move.instance.rb.velocity.y > 0)
            {               
                Move.instance.isBump = true;
                Move.instance.Bump();
            }
    }
}}
//////////////////////////////////
if(GameplayManager.instance.styleIcon[3] == true)
{if (Move.instance.style == 3) //Wind
{
 if(!CanTake)
    {
            Debug.Log("Wind"+ Damage);

        TimeHitbox = 0.5f;
        GameplayManager.instance.ComboCount();
        CanTake = true;
        IDamegable hit = other.GetComponent<IDamegable>();
         hit.Damage(Damage);
            //Debug.Log("Damage:" + Player.Damage);
            if(Move.instance.rb.velocity.y > 0)
            {               
                Move.instance.isBump = true;
                Move.instance.Bump();
            }
    }
}}
///////////////////////////////////
if(GameplayManager.instance.styleIcon[4] == true)
{if (Move.instance.style == 4) //Water
{
            Debug.Log("Water"+ Damage);

        TimeHitbox = 0.5f;
        GameplayManager.instance.ComboCount();
        IDamegable hit = other.GetComponent<IDamegable>();
         hit.Damage(Damage);
            //Debug.Log("Damage:" + Player.Damage);
            if(Move.instance.rb.velocity.y > 0)
            {               
                Move.instance.isBump = true;
                Move.instance.Bump();
            }
}}
////////////////////////////
if(GameplayManager.instance.styleIcon[5] == true)
{if (Move.instance.style == 5) //Void
{
    Debug.Log("Void"+ Damage);
    TimeHitbox = 0.3f;
    GameplayManager.instance.ComboCount();
     IDamegable hit = other.GetComponent<IDamegable>();
        hit.Damage(Damage);
            //Debug.Log("Damage:" + Player.Damage);
            if(Move.instance.rb.velocity.y > 0)
            {               
                Move.instance.isBump = true;
                Move.instance.Bump();
            }
}}
}
////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

         if(other.gameObject.tag == "Hitbox_E")
        //Se il proiettile tocca il nemico
        {       
            CanTake = true;
            //SClang.Play();                   
            Move.instance.Knockback();
            if(Move.instance.rb.velocity.y > 0)
            {
                GameplayManager.instance.sbam();
                Move.instance.Knockback(); 
                Move.instance.isBump = true;
                Move.instance.Bump();
            }
        }
}
////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
}    
