using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gladio : MonoBehaviour
{


    
    [SerializeField] float lifeTime = 10;
    public float detectionRange = 10f;
     [SerializeField] GameObject Explode;
    [SerializeField] Transform prefabExp;
    public LayerMask enemyLayer;
    [SerializeField] int damage = 50;

   // private Vector3 startingPosition;

    
    private float horizontal;
    private bool DirX = true;
    private bool FindEnemy = false;
	private GameObject target;
	public float speed;
	public int distance;
	
	public float speed_2;
	public int distance_2;
	
	public float speed_3;
	public int distance_3;
	
private void Start()
    {
        //startingPosition = transform.position;
        target = GameObject.FindWithTag("Player");
        Invoke("Destroy", lifeTime);
        Move.instance.Evocation();
        Move.instance.Stop();

    }

    // Update is called once per frame
    void Update()
    {         
        
    RaycastHit2D hit = Physics2D.Raycast(transform.position, target.transform.position - transform.position, detectionRange, enemyLayer);
    if(hit.collider != null && hit.collider.gameObject.CompareTag("Enemy"))
    {
    target = hit.collider.gameObject;
    FindEnemy = true;
    transform.position = Vector2.MoveTowards(transform.position, target.transform.position, speed * Time.deltaTime);
    }

        if(!FindEnemy)
        {
		if (transform.position.x<target.transform.position.x)
        {
            horizontal=1;
        }
		else 
        {
            horizontal=-1;
        }
        if (Vector2.Distance(transform.position, target.transform.position)>distance)
        {
			if (Vector2.Distance(transform.position, target.transform.position)>distance_2)
            {
				if (Vector2.Distance(transform.position, target.transform.position)>distance_3)
                {
					transform.position = Vector2.MoveTowards(transform.position, target.transform.position, speed_3*Time.deltaTime);
				} else 
                {
					transform.position = Vector2.MoveTowards(transform.position, target.transform.position, speed_2*Time.deltaTime);
				}
			} else 
            {
				transform.position = Vector2.MoveTowards(transform.position, target.transform.position, speed*Time.deltaTime);
			}
		}
        }
        else if(FindEnemy)
        {
            RaycastHit2D hit2 = Physics2D.Raycast(transform.position, target.transform.position - transform.position, detectionRange, enemyLayer);
        if (hit2.collider != null)
        {
            target = hit2.collider.gameObject;
            transform.position = Vector2.MoveTowards(transform.position, target.transform.position, speed * Time.deltaTime);
        }
        }

		Flip();

    }
	
	private void Flip()
    {
        if (DirX && horizontal < 0f || !DirX && horizontal > 0f)
        {
            DirX = !DirX;
            Vector3 localScale = transform.localScale;
            localScale.x *= -1f;
            transform.localScale = localScale;
        }
    }


 void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Enemy")
        {  
            //Debug.Log("Colpito");
            Instantiate(Explode, transform.position, transform.rotation);
            IDamegable hit = other.GetComponent<IDamegable>();
            hit.Damage(damage);
            Destroy(gameObject);
            
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
    
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
    }
}