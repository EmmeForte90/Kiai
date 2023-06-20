using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pietre_terreno_boss : MonoBehaviour
{
    public float attackDamage = 10; // danno d'attacco
    public float damagestamina = 50; // danno d'attacco
    public GameObject impactEffect;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(distruggi_oggetto());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D col){
        if(col.name == "Nekotaro"){
            //Move.instance.DamagePlayer(damageAmount);
           if(!Move.instance.isGuard)
            {
            if (!Move.instance.isDeath)
            {
                if (!Move.instance.isHurt)
            {
            PlayerHealth.Instance.Damage(attackDamage);
            Move.instance.KnockbackS();            

            }}}if(Move.instance.isGuard)
            {
                Move.instance.KnockbackS(); 
                Move.instance.GuardHit(); 
                PlayerHealth.Instance.currentStamina -= damagestamina;           
            }
            esplosione();
        }

        //AudioManager.instance.PlaySFXAdjusted(3);
    }
    private void esplosione(){
        if(impactEffect != null)
        {
            Instantiate(impactEffect, transform.position, transform.rotation);

            Destroy(gameObject);
        }
    }

    private IEnumerator distruggi_oggetto(){
        yield return new WaitForSeconds(1.5f);
        Destroy(gameObject);
    }
}
