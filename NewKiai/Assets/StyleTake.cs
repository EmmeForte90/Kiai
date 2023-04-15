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

     
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
        if(!isCollected)
        {
        GameplayManager.instance.StyleActivated(StyleID);
        AudioSource.PlayClipAtPoint(PickupSFX, Camera.main.transform.position);
        Instantiate(VFX, transform.position, transform.rotation);
        isCollected = true;
        }
        }
    }
}
