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
    
    [Tooltip("Si deve cancellare dalla scena?")]
    public bool isdelete = false;
    public int idForDelete;
    public bool isGadget = false;
    public bool isDress = false;

    [HideInInspector]
    public bool isCollected = false;

public void Awake()
    {
    IDItem = Item.id;
    if(isCollected)
    {Destroy(gameObject);}
    }

public void Update()
    {
    if(isCollected)
    {Destroy(gameObject);}
    }

     
    public void Pickup()
    {   
        if(isDress)
        {
        InventoryManager.Instance.DressTake(IDItem);
        }
        else if(isGadget)
        {
        InventoryManager.Instance.GadgetAc(IDItem);
        if(isdelete){InventoryManager.Instance.ItemActive(idForDelete);}
        }else if(!isGadget){
        //Item.value++;
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
