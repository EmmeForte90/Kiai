using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testwalljump : MonoBehaviour
{
   public float speed = 5f;            // velocità del personaggio
    public float jumpForce = 10f;       // forza del salto
    public float wallJumpForce = 7f;    // forza del walljump
    public float wallSlideSpeed = 1f;   // velocità di scivolamento lungo il muro
    public float wallDistance = 0.5f;   // distanza dal muro per effettuare il walljump
    public float checkRadius = 0.2f;    // raggio di controllo per il ground check

    public Transform groundCheck;       // oggetto per controllare se il personaggio è a contatto col suolo
    public LayerMask groundLayer;       // layer del suolo
    public LayerMask wallLayer;         // layer del muro

    Rigidbody2D rb;
    bool facingRight = true;
    bool isGrounded = false;
    bool isTouchingWall = false;
    bool wallJumped = false;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {// Salto
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }
        
        // Controllo se il personaggio è a contatto col suolo
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, checkRadius, groundLayer);

        // Controllo se il personaggio è a contatto con un muro
        isTouchingWall = Physics2D.Raycast(transform.position, transform.right, wallDistance, wallLayer);

        // Movimento orizzontale
        float moveInput = Input.GetAxisRaw("Horizontal");
        rb.velocity = new Vector2(moveInput * speed, rb.velocity.y);

        // Flip del personaggio in base alla direzione dello spostamento
        if (moveInput > 0 && !facingRight)
        {
            Flip();
        }
        else if (moveInput < 0 && facingRight)
        {
            Flip();
        }

        

        // Wallslide
        if (isTouchingWall && !isGrounded && rb.velocity.y < 0)
        {
            rb.velocity = new Vector2(rb.velocity.x, -wallSlideSpeed);
        }

        // Walljump
        if (Input.GetButtonDown("Jump") && isTouchingWall && !isGrounded)
        {
            if (facingRight)
            {
                rb.velocity = new Vector2(-wallJumpForce, jumpForce);
            }
            else
            {
                rb.velocity = new Vector2(wallJumpForce, jumpForce);
            }
            wallJumped = true;
            Invoke("SetWallJumpedToFalse", 0.5f);
        }
        
        }

   

    // Metodo per il flip del personaggio
    void Flip()
    {
        facingRight = !facingRight;
        transform.Rotate(0f, 180f, 0f);
    }

    // Metodo per ripristinare il valore di wallJumped dopo 0.5 secondi
    void SetWallJumpedToFalse()
    {
        wallJumped = false;
    }
}