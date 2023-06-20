using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaseBulletEn : MonoBehaviour
{
   public float moveSpeed;

    public Rigidbody2D theRB;
    public float attackDamage = 10; // danno d'attacco
    public float damagestamina = 30; // danno d'attacco

    //public int damageAmount;
    public GameObject impactEffect;

    // Start is called before the first frame update
    void Start()
    {
        Vector3 direction = transform.position - Move.instance.transform.position;
        direction.y-=1.5f; //eros: per prendere il centro del personaggio
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

        //AudioManager.instance.PlaySFXAdjusted(2);
    }

    // Update is called once per frame
    void Update()
    {
        theRB.velocity = -transform.right * moveSpeed;
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
}
