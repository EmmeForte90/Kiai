using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ItemPickup : MonoBehaviour
{
    public Item Item;
    [SerializeField] GameObject VFX;
    private int IDItem;
    public bool isGadget = false;
    [HideInInspector]
    public bool isCollected = false;

public void Awake()
    {
    IDItem = Item.id;
    }

public void Update()
    {
    if(isCollected)
    {Destroy(gameObject);}
    }

     
    public void Pickup()
    {   
        if(isGadget)
        {
        InventoryManager.Instance.GadgetAc(IDItem);
        }else if(!isGadget){
        InventoryManager.Instance.AddItem(Item);
        InventoryManager.Instance.ListItem(IDItem);
        InventoryManager.Instance.ItemActive(IDItem);
        if( AssignItem.Instance == null)
        {
        //AssignItem.Instance.AssignId(IDItem);
        }}
        isCollected = true; // Imposta la variabile booleana a "true" quando l'oggetto viene raccolto
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
        Pickup();
        Instantiate(VFX, transform.position, transform.rotation);
        }
    }



}
