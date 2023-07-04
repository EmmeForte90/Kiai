using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class Pickup : MonoBehaviour
{
   //[SerializeField] AudioClip coinPickupSFX;
    //Variabile per il suono
    [SerializeField] public int pointsForCoinPickup;
    //Valore della moneta quando raccolta
    [SerializeField] public float loadDelay = 0.1f;
    //Temo di recupero
    [SerializeField] public Animator myAnimator;
    //Animatore
    bool wasCollected = false;
    private Rigidbody2D rb; // componente Rigidbody2D del personaggio
    [SerializeField] float lifeTime = 20f;
    public float takeradious = 2f;
    private Transform player;

    [SerializeField]  GameObject Lighter;
    //Bool per evitare che la moneta sia raccolta più volte

void Start()
{    
    player = GameObject.FindWithTag("Player").transform;

    myAnimator = GetComponent<Animator>();
    //Recupera i componenti dell'animator
 Invoke("Destroy", lifeTime);


}
private void Update()
    {
        if (Vector2.Distance(transform.position, player.position) < takeradious) 
        {
            if (!wasCollected)
        //Se il player tocca la moneta e non è stato collezionata
        {
            wasCollected = true;
            Collider2D collider = GetComponent<Collider2D>();
        if (collider != null)
        {
            collider.isTrigger = true;
        }
    
            //La moneta è collezionata
            GameplayManager.instance.AddTomoney(pointsForCoinPickup);
            //Richiama la funzione dello script GameSessione e aumenta lo score
            AudioManager.instance.PlaySFX(0);
            //AudioSource.PlayClipAtPoint(coinPickupSFX, Camera.main.transform.position);
            //Avvia l'audio
            myAnimator.SetTrigger("take");
            //Attiva il suono
            Invoke("takeCoin", loadDelay);
            
        }
        }
    }


#region Gizmos
private void OnDrawGizmos()
    {
    Gizmos.color = Color.blue;
    Gizmos.DrawWireSphere(transform.position, takeradious);
        //Debug.DrawRay(transform.position, new Vector3(chaseThreshold, 0), Color.red);
    }
#endregion
    

       
    
 private void Destroy()
    {
        Destroy(gameObject);
    }
#region Funzione per cancellare l'item
    void takeCoin()
    {

        gameObject.SetActive(false);
        //Disattiva l'oggetto
        Destroy(gameObject);
        //Lo distrugge
    }
    #endregion
}
