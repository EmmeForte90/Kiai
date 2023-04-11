using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class testit : MonoBehaviour
{
    public int IDItem;
    public bool isCollected = false;

public void Awake()
    {
    }

public void Update()
    {
    if(isCollected)
    {Destroy(gameObject);}
    }

     
    public void Pickup()
    {
        InventoryManager.Instance.RemoveItemID(IDItem);
        if( AssignItem.Instance == null)
        {
        //AssignItem.Instance.AssignId(IDItem);
        }
        isCollected = true; // Imposta la variabile booleana a "true" quando l'oggetto viene raccolto
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
        Pickup();
       // Instantiate(VFX, transform.position, transform.rotation);
        }
    }



}
