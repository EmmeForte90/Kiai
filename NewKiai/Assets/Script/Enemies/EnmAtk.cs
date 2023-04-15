using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnmAtk : MonoBehaviour
{        
    public GameObject Hitbox;
    private Transform player;
    public float attackDamage = 10; // danno d'attacco
    public float damagestamina = 50; // danno d'attacco
    private bool take = false;

    // Start is called before the first frame update
    void Start()
    {
                player = GameObject.FindWithTag("Player").transform;
 
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

IEnumerator StopD()
    {
        yield return new WaitForSeconds(0.5f);
        take = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
    if(!take)
        {
        if (collision.CompareTag("Player"))
        {
             take = true;
            StartCoroutine(StopD());
            if(!Move.instance.isGuard)
            {
            if (!Move.instance.isDeath)
            {
                if (!Move.instance.isHurt)
            {
            player.GetComponent<PlayerHealth>().Damage(attackDamage);
            Move.instance.Knockback();            

            }}}if(Move.instance.isGuard)
            {
                Move.instance.Knockback(); 
                Move.instance.GuardHit(); 
                PlayerHealth.Instance.currentStamina -= damagestamina;           
            }

    }else if (collision.gameObject.tag == "Hitbox")
    {
        take = true;
            Move.instance.sbam();
           Move.instance.Knockback();            
           StartCoroutine(StopD());

    }
    }
    }
}
