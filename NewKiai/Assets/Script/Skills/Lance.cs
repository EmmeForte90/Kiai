using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lance : MonoBehaviour
{
   public float initialSpeed = 10f;
public float gravity = 9.8f;

private Rigidbody2D rb;
private Vector2 startPosition;
private float startTime;
[SerializeField] int MP = 10;
[SerializeField] Transform prefabExp;
[SerializeField] int damage = 305;
PlayerHealth Less;
private float lifeTime = 0.5f;
[SerializeField] GameObject Explode;
Move player;
private Vector2 initialDirection;

private void Start()
{
    rb = GetComponent<Rigidbody2D>();
    startPosition = transform.position;
    startTime = Time.time;
    player = FindObjectOfType<Move>();
    Less = FindObjectOfType<PlayerHealth>();
    if(player.transform.localScale.x > 0)
    {
        initialDirection = Vector2.right;
    } 
    else if(player.transform.localScale.x < 0)
    {
        initialDirection = Vector2.left;
    }
    CostMP();
}

private void FixedUpdate()
{
    float elapsedTime = Time.time - startTime;
    float verticalDisplacement = initialSpeed * elapsedTime * Mathf.Sin(45) - 0.5f * gravity * elapsedTime * elapsedTime;
    float horizontalDisplacement = initialSpeed * elapsedTime * Mathf.Cos(45);

    transform.position = startPosition + initialDirection * horizontalDisplacement + Vector2.up * verticalDisplacement;

    float angle = Mathf.Atan2(verticalDisplacement, horizontalDisplacement * initialDirection.x) * Mathf.Rad2Deg;
    transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
}

#region MP
void CostMP()
{
   // Less.TakeManaDamage(MPCost);
}
#endregion

void OnTriggerEnter2D(Collider2D other)
{
    if (other.gameObject.tag == "Enemy")
    {  
        Instantiate(Explode, transform.position, transform.rotation);
        int damage = 50;
        IDamegable hit = other.GetComponent<IDamegable>();
        hit.Damage(damage);
        Destroy(gameObject);

    }

    if (other.gameObject.tag == "Ground")
    { 
        Invoke("Destroy", lifeTime);
    }
}
}
