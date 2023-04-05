using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour
{
   [SerializeField] AudioClip coinPickupSFX;
    //Variabile per il suono
    [SerializeField] public int pointsForCoinPickup;
    //Valore della moneta quando raccolta
    [SerializeField] public float loadDelay = 0.1f;
    //Temo di recupero
    [SerializeField] public Animator myAnimator;
    //Animatore
    bool wasCollected = false;
    private Rigidbody2D rb; // componente Rigidbody2D del personaggio

    [SerializeField]  GameObject light;
    //Bool per evitare che la moneta sia raccolta più volte
    [SerializeField] public bool isHeal;


void Start()
{
    myAnimator = GetComponent<Animator>();
    //Recupera i componenti dell'animator
    

}


    void OnCollisionEnter2D(Collision2D other) 
    {

        #region CollCoin
        if (other.gameObject.tag == "Player" && !wasCollected && !isHeal)
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
            AudioSource.PlayClipAtPoint(coinPickupSFX, Camera.main.transform.position);
            //Avvia l'audio
            myAnimator.SetTrigger("take");
            //Attiva il suono
            Invoke("takeCoin", loadDelay);
            
        }

        #endregion
        
       
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
