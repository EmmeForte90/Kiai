using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StyleTake : MonoBehaviour
{     
    [SerializeField] public int StyleID = 0;
    [SerializeField] GameObject VFX;
    public bool isCollected = false;
    [SerializeField] AudioClip PickupSFX;


public void Update()
    {
            if(isCollected)
            {Destroy(gameObject);}
    }

     
    public void Pickup()
    {
        GameplayManager.instance.StyleActivated(StyleID);
        isCollected = true; // Imposta la variabile booleana a "true" quando l'oggetto viene raccolto
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
        Pickup();
        AudioSource.PlayClipAtPoint(PickupSFX, Camera.main.transform.position);
        Instantiate(VFX, transform.position, transform.rotation);

        }
    }

}
