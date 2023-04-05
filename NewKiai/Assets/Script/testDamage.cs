using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testDamage : Health, IDamegable
{
private Health health;
    private Rigidbody2D rb;
    private GameplayManager gM;
    private Transform player;

    public float invincibleTime = 2.0f; // Durata dell'invincibilità in secondi
    private bool isInvincible = false; // Indica se il personaggio è invincibile o meno
    private float invincibleTimer = 0.0f; // Contatore per il tempo di invincibilità rimanente

    void Awake()
    {
        player = GameObject.FindWithTag("Player").transform;
        health = GetComponent<Health>();
        rb = GetComponent<Rigidbody2D>();
    if (gM == null)
    {
        gM = GameObject.FindObjectOfType<GameplayManager>();
    }
    }

void Update()
    {
        if (isInvincible)
        {
            invincibleTimer -= Time.deltaTime; // Riduce il tempo di invincibilità rimanente
            if (invincibleTimer <= 0)
            {
                isInvincible = false; // Termina l'invincibilità quando il timer raggiunge 0
            }
        }
    }

 
    public void Damage(int damage)
    {
        if (!isInvincible)
        {
            // Gestisci il danno ricevuto qui
            isInvincible = true; // Il personaggio diventa invincibile
            invincibleTimer = invincibleTime; // Imposta il tempo di invincibilità rimanente
            health.currentHealth -= damage;

        }
        
    }


}
