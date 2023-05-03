using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class AssignItem : MonoBehaviour
{
       public Item Item;
 
 // Riferimento al contenitore dei pulsanti delle item
    [Header("Menu Consumabili")]
   //public Transform ItemContent;
 // Riferimenti ai componenti delle immagini di preview e delle descrizioni
    public Image previewImages;
    public Image iconImages;
    public TextMeshProUGUI descriptions;
    public TextMeshProUGUI Num;
    public TextMeshProUGUI NameItems;
    private int itemValue;

    public GameObject Lock;
    [HideInInspector] public Color imageColor;
    public int selectedId = -1; // Id dell'abilità selezionata  
    
public static AssignItem Instance;

public void Awake()
    {
    if (Instance == null)
        {
            Instance = this;
        }
        Num.text = Item.value.ToString();
        iconImages.sprite = Item.icon;
    }

public void Update()
    {
        Num.text = Item.value.ToString();
        if(Item.value <= 0)
        {
        Lock.gameObject.SetActive(true);
        Item.value = 0;
        }
        if(Item.value > 0)
        {
        Lock.gameObject.SetActive(false);
        }
    }

 public void AssignId(Item Item)
    {
        previewImages.sprite = Item.icon;
        descriptions.text = Item.Description;
        NameItems.text = Item.itemName;
        selectedId = Item.id;
        itemValue = Item.value;

        if(Item.value > 0 )
        {
        if(ItemRapidMenu.Instance.isSlot1)
        {
            ItemRapidMenu.Instance.Item_1 = Item;
            ItemRapidMenu.Instance.Slot1 = Item.id;
            UpdateMenuRapido.Instance.Slot1 = Item.id;
            print(selectedId);
           if(ItemRapidMenu.Instance.isSlot1 != ItemRapidMenu.Instance.isSlot2 ||
            ItemRapidMenu.Instance.isSlot1 != ItemRapidMenu.Instance.isSlot3 ||
           ItemRapidMenu.Instance.isSlot1 != ItemRapidMenu.Instance.isSlot4)
        { 
            ItemRapidMenu.Instance.MXV1 = Item.value;
            UpdateMenuRapido.Instance.MXV1 = Item.value;
            ItemRapidMenu.Instance.Slot1_T.text = Item.value.ToString();
            ItemRapidMenu.Instance.Slot1_I.sprite = Item.icon;
            Num.text = Item.value.ToString();
            UpdateMenuRapido.Instance.Slot1_T.text = Item.value.ToString();
            UpdateMenuRapido.Instance.Slot1_I.sprite = Item.icon;
            //
            imageColor = UpdateMenuRapido.Instance.Slot1_I.color;
            imageColor.a = 1f;
            UpdateMenuRapido.Instance.Slot1_I.color = imageColor;
            //
            imageColor = ItemRapidMenu.Instance.Slot1_I.color;
            imageColor.a = 1f;
            ItemRapidMenu.Instance.Slot1_I.color = imageColor;
        }
        }else if(ItemRapidMenu.Instance.isSlot2)
        {
            ItemRapidMenu.Instance.Item_2 = Item;
            ItemRapidMenu.Instance.Slot2 = Item.id;
            UpdateMenuRapido.Instance.Slot2 = Item.id;
                    print(selectedId);

            if(ItemRapidMenu.Instance.isSlot2 != ItemRapidMenu.Instance.isSlot1 ||
            ItemRapidMenu.Instance.isSlot2 != ItemRapidMenu.Instance.isSlot3 ||
           ItemRapidMenu.Instance.isSlot2 != ItemRapidMenu.Instance.isSlot4)
        { 
            ItemRapidMenu.Instance.MXV2 = Item.value;
            UpdateMenuRapido.Instance.MXV2 = Item.value;
            ItemRapidMenu.Instance.Slot2_T.text = Item.value.ToString();
            ItemRapidMenu.Instance.Slot2_I.sprite = Item.icon;
            Num.text = Item.value.ToString();
            UpdateMenuRapido.Instance.Slot2_T.text = Item.value.ToString();
            UpdateMenuRapido.Instance.Slot2_I.sprite = Item.icon;
           
            //
            imageColor = UpdateMenuRapido.Instance.Slot2_I.color;
            imageColor.a = 1f;
            UpdateMenuRapido.Instance.Slot2_I.color = imageColor;
            //
            imageColor = ItemRapidMenu.Instance.Slot2_I.color;
            imageColor.a = 1f;
            ItemRapidMenu.Instance.Slot2_I.color = imageColor;
        }
        }else if(ItemRapidMenu.Instance.isSlot3)
        {            
            ItemRapidMenu.Instance.Item_3 = Item;
            ItemRapidMenu.Instance.Slot3 = Item.id;
            UpdateMenuRapido.Instance.Slot3 = Item.id;
                    print(selectedId);

            if(ItemRapidMenu.Instance.isSlot3 != ItemRapidMenu.Instance.isSlot2 ||
            ItemRapidMenu.Instance.isSlot3 != ItemRapidMenu.Instance.isSlot1 ||
           ItemRapidMenu.Instance.isSlot3 != ItemRapidMenu.Instance.isSlot4)
        { 
            ItemRapidMenu.Instance.MXV3 = Item.value;
            UpdateMenuRapido.Instance.MXV3 = Item.value;
            ItemRapidMenu.Instance.Slot3_T.text = Item.value.ToString();
            ItemRapidMenu.Instance.Slot3_I.sprite = Item.icon;
            Num.text = Item.value.ToString();
            UpdateMenuRapido.Instance.Slot3_T.text = Item.value.ToString();
            UpdateMenuRapido.Instance.Slot3_I.sprite = Item.icon;
            //
            imageColor = UpdateMenuRapido.Instance.Slot3_I.color;
            imageColor.a = 1f;
            UpdateMenuRapido.Instance.Slot3_I.color = imageColor;
            //
            imageColor = ItemRapidMenu.Instance.Slot3_I.color;
            imageColor.a = 1f;
            ItemRapidMenu.Instance.Slot3_I.color = imageColor;
        }
        }else if(ItemRapidMenu.Instance.isSlot4)
        {
            ItemRapidMenu.Instance.Item_4 = Item;
            ItemRapidMenu.Instance.Slot4 = Item.id;
            UpdateMenuRapido.Instance.Slot4 = Item.id;
                    print(selectedId);

            if(ItemRapidMenu.Instance.isSlot4 != ItemRapidMenu.Instance.isSlot2 ||
            ItemRapidMenu.Instance.isSlot4 != ItemRapidMenu.Instance.isSlot3 ||
           ItemRapidMenu.Instance.isSlot4 != ItemRapidMenu.Instance.isSlot1)
        { 
            ItemRapidMenu.Instance.MXV4 = Item.value;
            UpdateMenuRapido.Instance.MXV4 = Item.value;
            ItemRapidMenu.Instance.Slot4_T.text = Item.value.ToString();
            ItemRapidMenu.Instance.Slot4_I.sprite = Item.icon;
            Num.text = Item.value.ToString();
            UpdateMenuRapido.Instance.Slot4_T.text = Item.value.ToString();
            UpdateMenuRapido.Instance.Slot4_I.sprite = Item.icon;
            
            //
            imageColor = UpdateMenuRapido.Instance.Slot4_I.color;
            imageColor.a = 1f;
            UpdateMenuRapido.Instance.Slot4_I.color = imageColor;
            //
            imageColor = ItemRapidMenu.Instance.Slot4_I.color;
            imageColor.a = 1f;
            ItemRapidMenu.Instance.Slot4_I.color = imageColor;
        }
        }
    }
    }

}
