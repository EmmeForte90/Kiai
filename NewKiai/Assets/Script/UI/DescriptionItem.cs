using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class DescriptionItem : MonoBehaviour
{
    public Item item;
   public int DataId;
   private int id;
    private int Value;

   public static DescriptionItem Instance;
    public TextMeshProUGUI Title;
   public TextMeshProUGUI ItemDescriptionText;
    [SerializeField] TextMeshProUGUI N_Item;
    public GameObject Light;

   public Image icon;

    private void Awake()
   {
    Instance = this;   
    id = item.id;
   }

public void DescriptionItemS()
    { 
        // Inserisci qui il codice che vuoi eseguire quando l'immagine viene cliccata
        Light.gameObject.SetActive(true);
        ItemDescriptionText.text = item.Description;
        Title.text = item.itemName;
        id = item.id;
        Value = item.value;
        N_Item.text = Value.ToString();
         // Imposta l'icona dell'abilità
        icon.sprite = item.icon;
         
    }
}
