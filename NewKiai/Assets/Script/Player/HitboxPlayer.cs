using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitboxPlayer : MonoBehaviour
{
    private bool CanTake = false;
    private float TimeHitbox; 
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
        if(other.CompareTag("Enemy") || other.CompareTag("Boss"))
        //Se il proiettile tocca il nemico
        {       
            //print("colpito torcia(ENM)");

if(GameplayManager.instance.styleIcon[0] == true)
{if (Move.instance.style == 0) //Normal
{
 if(!CanTake)
    {
        TimeHitbox = 0.2f;
        GameplayManager.instance.ComboCount();
        CanTake = true;
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
        TimeHitbox = 2f;
        GameplayManager.instance.ComboCount();
        CanTake = true;
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
        TimeHitbox = 1f;
        GameplayManager.instance.ComboCount();
        CanTake = true;
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
        TimeHitbox = 0.5f;
        GameplayManager.instance.ComboCount();
        CanTake = true;
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
        TimeHitbox = 0.3f;
        GameplayManager.instance.ComboCount();
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
    TimeHitbox = 0.3f;
    GameplayManager.instance.ComboCount();
            if(Move.instance.rb.velocity.y > 0)
            {               
                Move.instance.isBump = true;
                Move.instance.Bump();
            }
}}
}
////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

         if(other.CompareTag("Hitbox_E"))
        {       
            CanTake = true;
            Move.instance.KnockbackS();
            if(Move.instance.rb.velocity.y > 0)
            {
              //  GameplayManager.instance.sbam();
                Move.instance.KnockbackS(); 
            }
        }
////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
       
}
////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
}    
