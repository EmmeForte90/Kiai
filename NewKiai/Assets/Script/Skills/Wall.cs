using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : MonoBehaviour
{
   public float speed = 5f; // velocità del proiettile
    public float growSpeed = 0.2f; // velocità di crescita del proiettile
    public float maxScale = 5f; // dimensione massima del proiettile
    public float lifeTime = 3f; // durata del proiettile

    private float scale = 0f; // dimensione corrente del proiettile
    private float timer = 0f; // timer per la durata del proiettile

[SerializeField] GameObject Explode;
    [SerializeField] Transform prefabExp;
        [SerializeField] int damage = 50;
        
[Header("Audio")]
    [SerializeField] AudioSource SExp;
    [SerializeField] AudioSource SBomb;

    private void Start()
    {
       // Move.instance.Bigblast();
        Move.instance.Stop();
        // resetta la scala e il timer
        transform.localScale = Vector3.zero;
        scale = 0f;
        timer = 0f;
    }

    private void Update()
    {
        // incrementa il timer
        timer += Time.deltaTime;

        // se il proiettile è ancora vivo
        if (timer < lifeTime)
        {
            // incrementa la scala del proiettile
            scale += growSpeed * Time.deltaTime;
            scale = Mathf.Clamp(scale, 0f, maxScale); // limita la scala alla dimensione massima
            transform.localScale = new Vector3(scale, scale, 1f);

            // muovi il proiettile in avanti
            transform.position += transform.right * speed * Time.deltaTime;
        }
        else // altrimenti, distruggi il proiettile
        {
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D other) 
    {
        if(other.gameObject.tag == "Enemy")
        //Se il proiettile tocca il nemico
        {            
            SExp.Play();
            Instantiate(Explode, transform.position, transform.rotation);
            IDamegable hit = other.GetComponent<IDamegable>();
            hit.Damage(damage);
        }
        
        
    }  
}
