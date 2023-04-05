using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    public Item Item;
    [SerializeField] GameObject VFX;
    private int IDItem;
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
        InventoryManager.Instance.AddItem(Item);
        InventoryManager.Instance.ListItem(IDItem);
        InventoryManager.Instance.ItemActive(IDItem);
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
