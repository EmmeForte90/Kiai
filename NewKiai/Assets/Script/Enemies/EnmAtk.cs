using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnmAtk : MonoBehaviour
{        
    public float attackDamage = 10; // danno d'attacco
    public float damagestamina = 50; // danno d'attacco
    private bool take = false;
    [Header("VFX")]
    private bool vfx = false;
    private float vfxTimer = 0.5f;

    // Start is called before the first frame update
    void Start()
    {
 
 if (GameplayManager.instance == null) return;
    
    if (GameplayManager.instance.Easy)
    {
        attackDamage /= 2;
    }
    else if (GameplayManager.instance.Hard)
    {
        attackDamage *= 2;
    }
    
    
}
void Update()
    {
        if(vfx)
        {vfxTimer -= Time.deltaTime; //decrementa il timer ad ogni frame
        if (vfxTimer <= 0f) {
        take = false;
        vfx = false;
        }} 
    }



    private void OnTriggerEnter2D(Collider2D collision)
    {
    if(!take)
        {
        if (collision.CompareTag("Player"))
        {
        
        take = true;
        if(!vfx)
        {
            vfxTimer = 0.3f;
            if(!Move.instance.isGuard)
            {
            if (!Move.instance.isDeath)
            {
                if (!Move.instance.isHurt)
            {
            PlayerHealth.Instance.Damage(attackDamage);
            Move.instance.KnockbackS(); 
            GameplayManager.instance.ResetComboCount();           
            }}}if(Move.instance.isGuard)
            {
                Move.instance.KnockbackS(); 
                Move.instance.GuardHit(); 
                PlayerHealth.Instance.currentStamina -= damagestamina;           
            }
            vfx = true;
        }

    }else if (collision.gameObject.tag == "Hitbox")
    {
        if(!vfx)
        {
        vfxTimer = 0.3f;
        take = true;
        //GameplayManager.instance.sbam();
        Move.instance.KnockbackS();            
        }
        vfx = true;
    }}}
}
