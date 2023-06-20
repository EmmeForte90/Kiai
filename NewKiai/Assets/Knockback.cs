using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knockback : MonoBehaviour
{
       public float knockbackForce = 10f;
    public float knockbackDuration = 0.5f;

    private float knockbackTimer;
    private Rigidbody2D rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (knockbackTimer > 0)
        {
            knockbackTimer -= Time.deltaTime;
            if (knockbackTimer <= 0)
            {
                rb.velocity = Vector2.zero;
            }
        }
    }

    public void ApplyKnockback(Vector2 direction)
    {
        rb.velocity = direction * knockbackForce;
        knockbackTimer = knockbackDuration;
    }
}

public static class KnockbackHelper
{
    public static void ApplyKnockbackToObject(GameObject obj, Vector2 direction, float force, float duration)
    {
        var knockbackComponent = obj.GetComponent<Knockback>();
        if (knockbackComponent != null)
        {
            knockbackComponent.knockbackForce = force;
            knockbackComponent.knockbackDuration = duration;
            knockbackComponent.ApplyKnockback(direction);
        }
        else
        {
            Debug.LogError("No Knockback component found on the object: " + obj.name);
        }
    }
}