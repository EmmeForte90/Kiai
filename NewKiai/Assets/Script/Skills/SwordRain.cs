using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordRain : MonoBehaviour
{
       public float scaleFactor = 1.5f; // fattore di scala
    public float duration = 1f; // durata dell'animazione di scaling
    public float destroyDelay = 1.0f; // intervallo di tempo prima della distruzione del gameobject

    private float scaleTimer = 0.0f; // timer per il conteggio della durata dell'animazione di scaling
    private bool hasStarted = false; // flag per verificare se l'animazione è già stata avviata
    [SerializeField] GameObject Explode;
    [SerializeField] Transform prefabExp;
    [SerializeField] int damage = 50;

    [Header("Audio")]
    [SerializeField] AudioSource SExp;
    [SerializeField] AudioSource SBomb;

    public delegate void AnimationEndDelegate();
    public event AnimationEndDelegate OnAnimationEnd;

    void Start()
    {
        if (!hasStarted) {
            Move.instance.SwordRain();
            hasStarted = true;
        }
    }



    // Update is called once per frame
    void Update()
    {
        Move.instance.Stop();
        if (hasStarted) {
            // Incrementa il timer dell'animazione di scaling
            scaleTimer += Time.deltaTime;

            // Calcola il fattore di scala corrente in base al tempo trascorso
            float currentScaleFactor = Mathf.Lerp(1.0f, scaleFactor, scaleTimer / duration);

            // Aggiorna il local scale del GameObject
            transform.localScale = new Vector3(currentScaleFactor, currentScaleFactor, currentScaleFactor);

            // Se la durata dell'animazione di scaling è scaduta, distruggi il GameObject dopo l'intervallo di tempo specificato
            if (scaleTimer >= duration + destroyDelay)
            {
                Destroy(gameObject);
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other) 
    {
        if(hasStarted && other.gameObject.tag == "Enemy")
        //Se il proiettile tocca il nemico
        {            
            SExp.Play();
            Instantiate(Explode, transform.position, transform.rotation);
            IDamegable hit = other.GetComponent<IDamegable>();
            hit.Damage(damage);
        }
    }  
    
    private void OnDestroy() {
        if (OnAnimationEnd != null) {
            OnAnimationEnd();
        }
    }
}