using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Enemy :  Health, IDamegable
{
    private static int nextEnemyID = 1;
    public int enemyID;
public Transform pointA, pointB;
public float moveSpeed = 2f; // velocità di movimento
public float chaseSpeed = 4f; // velocità di inseguimento
public float attackDamage = 10; // danno d'attacco
public float sightRadius = 5f; // raggio di vista del nemico
public float chaseThreshold = 2f; // soglia di distanza per iniziare l'inseguimento
public float attackrange = 2f;
public float attackCooldown = 2f; // durata del cooldown dell'attacco
public GameObject DeathBack;
public GameObject Brain;
private bool movingToA = false;
private GameplayManager gM;
private Transform player;
private Rigidbody2D rb;
private Health health;
private Animator animator;
private float attackTimer;
  private bool isChasing = false; // indica se il nemico sta inseguendo il player
  private bool isMove = false;
  private float pauseDuration = 1f; // durata della pausa
private float pauseTimer; // timer per la pausa
[Header("Knockback")]
    private bool kb = false;
    public float knockbackForce; // la forza del knockback
    public float knockbackTime; // il tempo di knockback
    public float jumpHeight; // l'altezza del salto
    public float fallTime; // il tempo di caduta
[SerializeField] GameObject Death;
private enum State { Move, Chase, Attack, Knockback, Dead }
private State currentState;

private void Awake()
{                
    enemyID = nextEnemyID++;

    player = GameObject.FindWithTag("Player").transform;
    animator = GetComponent<Animator>();
    health = GetComponent<Health>();
    rb = GetComponent<Rigidbody2D>();
    if (gM == null)
    {
        gM = GameObject.FindObjectOfType<GameplayManager>();
    }
}


private void Update()
    {
        if (!GameplayManager.instance.PauseStop)
        {
            CheckState();
            switch (currentState)
            {
                case State.Move:
                    Move();
                    break;
                case State.Chase:
                    Chase();
                    break;
                case State.Attack:
                    Attack();
                    break;
                case State.Knockback:
                    Knockback();
                    break;
                case State.Dead:
                    break;
            }
        }
    }




public void anmHurt()
    {
        animator.SetTrigger("TakeDamage"); // attiva il trigger "TakeDamage" dell'animatore
        if (rb != null)
            {
                StartCoroutine(JumpBackCo(rb)); // avvia la routine per il salto
            }
        if (health.currentHealth <= 0f) // se la salute è uguale o inferiore a 0
        {
            DeathAnim();
        }
    }
private void CheckState()
{
    if (health.currentHealth == 0)
    {
        currentState = State.Dead;
        return;
    }

    if (IsKnockback())
    {
        currentState = State.Knockback;
        return;
    }

    if (Vector2.Distance(transform.position, player.position) < attackrange)
    {
        currentState = State.Attack;
        return;
    }

    if (Vector2.Distance(transform.position, player.position) < chaseThreshold)
    {
        currentState = State.Chase;
        return;
    }

    currentState = State.Move;
}



    private void Move()
    {
    animator.SetBool("isMove", true);
    animator.SetBool("isChasing", false); // imposta la variabile booleana "IsChasing" dell'animatore a true
        if (movingToA)
        {
            transform.localScale = new Vector2(-1f, 1f);
            if (pauseTimer > 0)
            {
                animator.SetBool("isMove", false);
                animator.SetBool("isChasing", false);
                pauseTimer -= Time.deltaTime;
                return;
            }
            transform.position = Vector2.MoveTowards(transform.position, pointA.position, moveSpeed * Time.deltaTime);
            if (Vector2.Distance(transform.position, pointA.position) < 0.1f)
            {
                pauseTimer = pauseDuration;
                movingToA = false;
            }
        }
        else
        {
            transform.localScale = new Vector2(1f, 1f);
            if (pauseTimer > 0)
            {
                animator.SetBool("isMove", false);
                animator.SetBool("isChasing", false);
                pauseTimer -= Time.deltaTime;
                return;
            }
            transform.position = Vector2.MoveTowards(transform.position, pointB.position, moveSpeed * Time.deltaTime);
            if (Vector2.Distance(transform.position, pointB.position) < 0.1f)
            {
                pauseTimer = pauseDuration;
                movingToA = true;
            }
        }
    }
    



private void Chase()
{
    isMove = false;
    isChasing = true;
    animator.SetBool("isChasing", true); // imposta la variabile booleana "IsChasing" dell'animatore a true
    animator.SetBool("isMove", false);
    // inseguimento del giocatore
    if (player.transform.position.x > transform.position.x)
    {
        transform.localScale = new Vector2(1f, 1f);
    }
    else if (player.transform.position.x < transform.position.x)
    {
        transform.localScale = new Vector2(-1f, 1f);
    }

    transform.position = Vector2.MoveTowards(transform.position, player.position, chaseSpeed * Time.deltaTime);
}
private IEnumerator JumpBackCo(Rigidbody2D rb)
    {
        if (rb != null)
        {
            isChasing = false;
            kb = true;
            Vector2 knockbackDirection = new Vector2(0f, jumpHeight); // direzione del knockback verso l'alto
            if (rb.transform.position.x < player.transform.position.x) // se la posizione x del nemico è inferiore a quella del player
                knockbackDirection = new Vector2(-1, jumpHeight); // la direzione del knockback è verso sinistra
            else if (rb.transform.position.x > player.transform.position.x) // se la posizione x del nemico è maggiore a quella del player
                knockbackDirection = new Vector2(1, jumpHeight); // la direzione del knockback è verso destra
            rb.AddForce(knockbackDirection * knockbackForce, ForceMode2D.Impulse); // applica il knockback
            yield return new WaitForSeconds(knockbackTime); // aspetta il tempo di knockback
            kb = false;
            isChasing = true;

        }
    }
private void Attack()
{
    // gestione dell'attacco del nemico
    if (attackTimer > 0)
    {
        attackTimer -= Time.deltaTime;
        return;
    }

    animator.SetTrigger("attack");
    player.GetComponent<PlayerHealth>().Damage(attackDamage);
    attackTimer = attackCooldown;
}

private void Knockback()
{
    // gestione del knockback
    // utilizzare la fisica di Unity per gestire la forza e il tempo di knockback
}

private bool IsKnockback()
{
    // controllo se il nemico è in knockback
    return kb;
}
#region Gizmos
private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
    Gizmos.DrawWireSphere(transform.position, chaseThreshold);
    Gizmos.color = Color.blue;
    Gizmos.DrawWireSphere(transform.position, attackrange);
        //Debug.DrawRay(transform.position, new Vector3(chaseThreshold, 0), Color.red);
    }
#endregion
public void DeathAnim()
{
    // animazione di morte
    // determina la direzione della morte in base alla posizione del player rispetto al nemico
        if (movingToA)//transform.position.x < player.position.x)
        {
            Instantiate(Death, transform.position, transform.rotation);
            //EnemyManager.UnregisterEnemy(this);
            Destroy(Brain);

            //animator.SetTrigger("Die"); // attiva il trigger "DieFront" dell'animatore per la morte davanti al player
        }
        else if (!movingToA)
        {
            Instantiate(DeathBack, transform.position, transform.rotation);
            //EnemyManager.UnregisterEnemy(this);
            Destroy(Brain);

            //animator.SetTrigger("Die_1"); // attiva il trigger "DieBack" dell'animatore per la morte dietro al player
        }
}
public void Damage(int damage)
    {
        health.currentHealth -= damage;
        anmHurt();
        
    }

}
