using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boomerang : MonoBehaviour
{
    public float speed = 10f;  // velocità del boomerang
    public float range = 5f;  // distanza massima percorsa dal boomerang
    public float returnSpeed = 20f;  // velocità del ritorno del boomerang
    public float returnDelay = 1f;  // ritardo in secondi prima che il boomerang inizi a tornare indietro
    public float rotationSpeed = 2500f;
    [SerializeField] int damage = 50;
    [SerializeField] GameObject Explode;
    [SerializeField] Transform prefabExp;
    private Vector2 startPos;  // posizione di partenza del boomerang
    private Vector2 targetPos;  // posizione del bersaglio del boomerang
    private bool returning = false;  // indica se il boomerang sta tornando indietro
    private bool returned = false;  // indica se il boomerang è tornato nelle mani del giocatore

    [Header("Audio")]
    [SerializeField] AudioSource SExp;
    [SerializeField] AudioSource SBomb;
    
    void Start()
{
    startPos = transform.position;  // memorizza la posizione di partenza
    Vector2 targetPos2D = new Vector2(transform.right.x, transform.right.y);  // converte il vettore in Vector2
    targetPos = startPos + targetPos2D * range;  // calcola la posizione del bersaglio
    Move.instance.Throw();
    Move.instance.Stop();
}

    void Update()
    {
                transform.Rotate(Vector3.forward, rotationSpeed * Time.deltaTime);

        if (!returning)
        {
            if(Move.instance.transform.localScale.x > 0)
            {
            // se il boomerang non sta tornando indietro, si muove verso il bersaglio
            transform.position = Vector2.MoveTowards(transform.position, targetPos, speed * Time.deltaTime);
            }else if(Move.instance.transform.localScale.x < 0)
            {
            // se il boomerang non sta tornando indietro, si muove verso il bersaglio
            transform.position = Vector2.MoveTowards(transform.position, -targetPos, -speed * Time.deltaTime);
            }

            if ((Vector2)transform.position == targetPos)
            {
                // se il boomerang ha raggiunto il bersaglio, inizia il ritorno
                returning = true;
                StartCoroutine(ReturnBoomerang());
            }
        }
        else
        {
            // se il boomerang sta tornando indietro, si muove verso la posizione di partenza
            transform.position = Vector2.MoveTowards(transform.position, startPos, returnSpeed * Time.deltaTime);
            if ((Vector2)transform.position == startPos)
            {
                // se il boomerang è tornato nelle mani del giocatore, lo distrugge
                Instantiate(Explode, transform.position, transform.rotation);
                Destroy(gameObject);
            }
        }
    }

    IEnumerator ReturnBoomerang()
    {
        // aspetta il ritardo prima di far tornare indietro il boomerang
        yield return new WaitForSeconds(returnDelay);
        targetPos = startPos;  // cambia il bersaglio in modo che il boomerang torni indietro
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Enemy")
        {  
            //Debug.Log("Colpito");
            Instantiate(Explode, transform.position, transform.rotation);
            IDamegable hit = other.GetComponent<IDamegable>();
            hit.Damage(damage);
            
        }

        if (other.gameObject.tag == "Ground")
        { 
            Destroy(gameObject);
        }
    }

    private void Destroy()
    {
        Destroy(gameObject);
    }
        
    }

