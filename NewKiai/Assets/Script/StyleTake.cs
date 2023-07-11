using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class StyleTake : MonoBehaviour
{     
    [SerializeField] public int StyleID = 0;
    [SerializeField] GameObject Oggetto; 
    [SerializeField] GameObject VFX; 
    public bool isCollected = false;
    //[SerializeField] AudioClip PickupSFX;
    public static StyleTake instance;


  private void Awake() 
  {
        if (instance == null)
        {
            instance = this;
        }
        /////////////////////////////////////////////
        if(GameplayManager.instance == null || GameplayManager.instance != null)
        {
        if(GameplayManager.instance.styleIcon[1] == true)
        {Destroy(Oggetto.gameObject);}
        if(GameplayManager.instance.styleIcon[2] == true)
        {Destroy(Oggetto.gameObject);}
        if(GameplayManager.instance.styleIcon[3] == true)
        {Destroy(Oggetto.gameObject);}
        if(GameplayManager.instance.styleIcon[4] == true)
        {Destroy(Oggetto.gameObject);}
        if(GameplayManager.instance.styleIcon[5] == true)
        {Destroy(Oggetto.gameObject);}}
}

public void Update()
    {

        
        if(isCollected)
        {Destroy(Oggetto.gameObject);}
    }

     public void Take()
    {
        Destroy(Oggetto.gameObject);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
        if(!isCollected)
        {
        GameplayManager.instance.StyleActivated(StyleID);
        AudioManager.instance.PlaySFX(1);
        //AudioSource.PlayClipAtPoint(PickupSFX, Camera.main.transform.position);
        Instantiate(VFX, transform.position, transform.rotation);
        isCollected = true;
        }
        }
    }

      
}
