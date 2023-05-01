using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class InventoryManager : MonoBehaviour
{
   public static InventoryManager Instance;
   //public List<Item> Items = new List<Item>();

   // Riferimento al contenitore dei pulsanti delle item
    [Header("Menu Consumabili")]
   public Transform ItemContent;
 // Riferimenti ai componenti delle immagini di preview e delle descrizioni
    public Image previewImages;
    public TextMeshProUGUI descriptions;
    public TextMeshProUGUI Num;
    public TextMeshProUGUI NameItems;
    public int selectedId = -1; // Id dell'abilità selezionata  

[Header("Menu Equip")]
   //public Transform ItemContent_E;
 // Riferimenti ai componenti delle immagini di preview e delle descrizioni
    public Image previewImages_E;
    public TextMeshProUGUI descriptions_E;
    public TextMeshProUGUI Num_E;
    public TextMeshProUGUI NameItems_E;
    public Transform ItemContentKat;
   public Transform ItemContentDres;

    [Header("Menu Non consumabili")]
   public Transform ItemContentIMP;
 // Riferimenti ai componenti delle immagini di preview e delle descrizioni
    public Image previewImages_C;
    public TextMeshProUGUI descriptions_C;
    public TextMeshProUGUI Num_C;
    public TextMeshProUGUI NameItems_C;
   
   public  GameObject ItemPrefab;
   public  GameObject InventoryItem;
       [HideInInspector] public Color imageColor;
   



    [HideInInspector] public int qID;
    // Scriptable Object delle item
    public List<Item> itemDatabase;
    private int val = 1;
    public bool[] itemActive;
    private bool buy = false; 
   

 [Header("MenuSlot")]
    [SerializeField]public TextMeshProUGUI Slot1_T;
    [SerializeField]public TextMeshProUGUI Slot2_T;
    [SerializeField]public TextMeshProUGUI Slot3_T;
    [SerializeField]public TextMeshProUGUI Slot4_T;
    [SerializeField]public TextMeshProUGUI Slot5_T;
    [SerializeField]public TextMeshProUGUI Slot6_T;
    [SerializeField]public TextMeshProUGUI Slot7_T;
    [SerializeField]public TextMeshProUGUI Slot8_T;

    [SerializeField]public Image Slot1_I;
    [SerializeField]public Image Slot2_I;
    [SerializeField]public Image Slot3_I;
    [SerializeField]public Image Slot4_I;
    [SerializeField]public Image Slot5_I;
    [SerializeField]public Image Slot6_I;
    [SerializeField]public Image Slot7_I;
    [SerializeField]public Image Slot8_I;

    [SerializeField] public GameObject[] SlotTot;
    //[SerializeField] public GameObject[] SlotTotM;
    [SerializeField] public bool[] SlotIcon;

    private static int uniqueId;


private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }




private void Update()
    {
        /*if(InventoryItem == null)
        {
            GameObject prefab = Resources.Load<GameObject>("Item");
            InventoryItem = prefab;
        }*/
    //previewImages;
    //descriptions;
    //Num;
    //NameItems;
    }

   public void SlotActivated(int id)
{
    SlotTot[id].SetActive(true);
    //SlotTotM[id].SetActive(true);
    SlotIcon[id] = true;   
}

     // Metodo per aggiungere una nuova item al database
  public void AddItem(Item newItem)
{
      // Trova un oggetto esistente con lo stesso ID del nuovo oggetto.
      Item existingItem = itemDatabase.Find(q => q.id == newItem.id);
      
      // Se viene trovato un oggetto esistente, incrementa il suo valore di uno.
      if (existingItem != null)
      {
          existingItem.value++;
      }
      
      // Se non viene trovato un oggetto esistente, aggiungi il nuovo oggetto al database.
      else if (existingItem == null)
      {
          itemDatabase.Add(newItem);
      } 
}



public bool RemoveItem(Item itemToRemove)
{
    Item existingItem = itemDatabase.Find(item => item.id == itemToRemove.id);
    if (existingItem == null)
    {
        return false; // l'oggetto non è stato trovato nella lista, restituisci false
    }

    if (val < 1)
    {
        itemDatabase.Remove(existingItem); // rimuovi completamente l'oggetto dalla lista
    }
    else
    {
        val--;
        Num.text = val.ToString(); // decrementa solo la quantità dell'oggetto
    }

    return true; // operazione completata con successo, restituisci true
}


public bool RemoveItemID(int itemToRemove)
{
    Item existingItem = itemDatabase.Find(item => item.id == itemToRemove);
    if (existingItem == null)
    {
        return false; // l'oggetto non è stato trovato nella lista, restituisci false
    }

    if (val < 1)
    {
                            

        //itemDatabase.Remove(existingItem); // rimuovi completamente l'oggetto dalla lista
            // Se l'item è già presente nella lista, incrementa il suo valore
            foreach (Transform child in ItemContent.transform)
            {
                if (child.name == "ItemButton_" + existingItem.id)
                {   
                    
                    Destroy(child.gameObject);
                    
                }
                    break;
            }
    }
    else
    {
        val--;
        Num.text = val.ToString(); // decrementa solo la quantità dell'oggetto
    }

    return true; // operazione completata con successo, restituisci true
}


public void ListItem(int itemId)
    {
        // Cerca la item con l'id specificato
        Item item = itemDatabase.Find(q => q.id == itemId);
        if(InventoryItem == null)
        {
        InventoryItem = ItemPrefab;
        }

       if (item != null)
    {
        if (ItemAlreadyInList(item.id))
        {
/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        if(item.isConsumable){
            // Se l'item è già presente nella lista, incrementa il suo valore
            foreach (Transform child in ItemContent.transform)
            {
                if (child.name == "ItemButton_" + item.id)
                {   
                 
                    // Aggiorna il testo del componente TextMeshProUGUI
                    Num.text = item.value.ToString();
                    break;
                }
            }
            // Incrementa il valore dell'item
            val++;
            }
/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            else
            if(item.isKey)
            {
            // Se l'item è già presente nella lista, incrementa il suo valore
            foreach (Transform child in ItemContentIMP.transform)
            {
                if (child.name == "ItemButton_" + item.id)
                {   
                 
                    // Aggiorna il testo del componente TextMeshProUGUI
                    Num_C.text = item.value.ToString();
                    break;
                }
            }
            // Incrementa il valore dell'item
            val++;
            }
/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            else
            if(item.isKatana)
            {
            // Se l'item è già presente nella lista, incrementa il suo valore
            foreach (Transform child in ItemContentKat.transform)
            {
                if (child.name == "ItemButton_" + item.id)
                {   
                 
                    // Aggiorna il testo del componente TextMeshProUGUI
                    //Num_C.text = item.value.ToString();
                    break;
                }
            }
            // Incrementa il valore dell'item
            val++;
            }
/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            else
            if(item.isDress)
            {
            // Se l'item è già presente nella lista, incrementa il suo valore
            foreach (Transform child in ItemContentDres.transform)
            {
                if (child.name == "ItemButton_" + item.id)
                {   
                 
                    // Aggiorna il testo del componente TextMeshProUGUI
                    //Num_C.text = item.value.ToString();
                    break;
                }
            }
            // Incrementa il valore dell'item
            val++;
            }
/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        }
        else
        {
    // il codice qui sotto verrà eseguito solo se l'item esiste e non è già presente nella lista
    // Istanzia il prefab del bottone della item nella lista UI
    if(item.isConsumable){InventoryItem = Instantiate(InventoryItem, ItemContent);}
    else
    if(item.isKey){InventoryItem = Instantiate(InventoryItem, ItemContentIMP);}
    else
    if(item.isDress){InventoryItem = Instantiate(InventoryItem, ItemContentDres);}
    else
    if(item.isKatana){InventoryItem = Instantiate(InventoryItem, ItemContentKat);}
    // Recupera il riferimento al componente del titolo della item e del bottone
    //var questName = obj.transform.Find("Name_Item").GetComponent<TextMeshProUGUI>();
    var Itemimg = InventoryItem.transform.Find("Icon_item").GetComponent<Image>();

    // Assegna l'id univoco al game object istanziato
    InventoryItem.name = "ItemButton_" + item.id;


    if (Itemimg != null && item.icon != null)
    {
        Itemimg.sprite = item.icon;
       
    }

    // Assegna i valori desiderati ai componenti dell'immagine di preview e della descrizione del pulsante della item
    if (previewImages != null)
    {
        if(item.isConsumable){previewImages.sprite = item.icon;}
        else
        if(item.isKey){previewImages_C.sprite = item.icon;}
        else
        if(item.isKatana || item.isDress){previewImages_E.sprite = item.icon;}
    }

    if (descriptions != null)
    {
        if(item.isConsumable){descriptions.text = item.Description;}
        else
        if(item.isKey){descriptions_C.text = item.Description;}
        else 
        if(item.isKatana || item.isDress){descriptions_E.text = item.Description;}
        
    }

    if (Num != null)
    {
        if(item.isConsumable){Num.text = val.ToString();}
        else
        if(item.isKey){Num_C.text = val.ToString();}
        else 
        if(item.isKatana || item.isDress){Num_E.text = val.ToString();}
    }

    if (NameItems != null)
    {
        if(item.isConsumable){NameItems.text = item.itemName;}
        else
        if(item.isKey){NameItems_C.text = item.itemName;}
        else
        if(item.isKatana || item.isDress){NameItems_E.text = item.itemName;}
    }

    // Aggiungi un listener per il click del bottone
    var button = InventoryItem.GetComponent<Button>();
    button.onClick.AddListener(() => OnQuestButtonClicked(item.id, previewImages, descriptions, selectedId, item.isConsumable, item.isKey, item.isKatana, item.isDress, item.NameSkin));
        }
}
    }
    
//Serve per non creare cloni nel menu
 private bool ItemAlreadyInList(int itemId)
{
    foreach (Transform child in ItemContent.transform)
    {
        if (child.name == "ItemButton_" + itemId)
        {
            // L'item è già presente nella lista
            return true;
        }
    }

    // L'item non è presente nella lista
    return false;
}     

    

public void OnQuestButtonClicked(int itemId, Image previewImages, TextMeshProUGUI descriptions, int selectedId, bool isConsumable, bool isKey, bool isKatana, bool isDress, string NameSkin)
{
    if (itemId >= 0 && isConsumable)
    {
        // Qui puoi fare qualcosa quando il pulsante della item viene cliccato, ad esempio aprire una finestra con i dettagli della item
        // Assegna i valori desiderati ai componenti dell'immagine di preview e della descrizione
        
        previewImages.sprite = itemDatabase.Find(q => q.id == itemId).icon;
        descriptions.text = itemDatabase.Find(q => q.id == itemId).Description;
        NameItems.text = itemDatabase.Find(q => q.id == itemId).itemName;
        selectedId = itemDatabase.Find(q => q.id == itemId).id;
        if(ItemRapidMenu.Instance.isSlot1)
        {
            ItemRapidMenu.Instance.Slot1 = itemDatabase.Find(q => q.id == itemId).id;
            UpdateMenuRapido.Instance.Slot1 = itemDatabase.Find(q => q.id == itemId).id;
           if(ItemRapidMenu.Instance.isSlot1 != ItemRapidMenu.Instance.isSlot2 ||
            ItemRapidMenu.Instance.isSlot1 != ItemRapidMenu.Instance.isSlot3 ||
           ItemRapidMenu.Instance.isSlot1 != ItemRapidMenu.Instance.isSlot4)
        { 
            ItemRapidMenu.Instance.MXV1 = itemDatabase.Find(q => q.id == itemId).value;
            UpdateMenuRapido.Instance.MXV1 = itemDatabase.Find(q => q.id == itemId).value;
            ItemRapidMenu.Instance.Slot1_T.text = itemDatabase.Find(q => q.id == itemId).value.ToString();
            ItemRapidMenu.Instance.Slot1_I.sprite = itemDatabase.Find(q => q.id == itemId).icon;
            Num.text = itemDatabase.Find(q => q.id == itemId).value.ToString();
            UpdateMenuRapido.Instance.Slot1_T.text = itemDatabase.Find(q => q.id == itemId).value.ToString();
            UpdateMenuRapido.Instance.Slot1_I.sprite = itemDatabase.Find(q => q.id == itemId).icon;
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
            ItemRapidMenu.Instance.Slot2 = itemDatabase.Find(q => q.id == itemId).id;
            UpdateMenuRapido.Instance.Slot2 = itemDatabase.Find(q => q.id == itemId).id;
            if(ItemRapidMenu.Instance.isSlot2 != ItemRapidMenu.Instance.isSlot1 ||
            ItemRapidMenu.Instance.isSlot2 != ItemRapidMenu.Instance.isSlot3 ||
           ItemRapidMenu.Instance.isSlot2 != ItemRapidMenu.Instance.isSlot4)
        { 
            ItemRapidMenu.Instance.MXV2 = itemDatabase.Find(q => q.id == itemId).value;
            UpdateMenuRapido.Instance.MXV2 = itemDatabase.Find(q => q.id == itemId).value;
            ItemRapidMenu.Instance.Slot2_T.text = itemDatabase.Find(q => q.id == itemId).value.ToString();
            ItemRapidMenu.Instance.Slot2_I.sprite = itemDatabase.Find(q => q.id == itemId).icon;
            Num.text = itemDatabase.Find(q => q.id == itemId).value.ToString();
            UpdateMenuRapido.Instance.Slot2_T.text = itemDatabase.Find(q => q.id == itemId).value.ToString();
            UpdateMenuRapido.Instance.Slot2_I.sprite = itemDatabase.Find(q => q.id == itemId).icon;
           
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
            ItemRapidMenu.Instance.Slot3 = itemDatabase.Find(q => q.id == itemId).id;
            UpdateMenuRapido.Instance.Slot3 = itemDatabase.Find(q => q.id == itemId).id;
            if(ItemRapidMenu.Instance.isSlot3 != ItemRapidMenu.Instance.isSlot2 ||
            ItemRapidMenu.Instance.isSlot3 != ItemRapidMenu.Instance.isSlot1 ||
           ItemRapidMenu.Instance.isSlot3 != ItemRapidMenu.Instance.isSlot4)
        { 
            ItemRapidMenu.Instance.MXV3 = itemDatabase.Find(q => q.id == itemId).value;
            UpdateMenuRapido.Instance.MXV3 = itemDatabase.Find(q => q.id == itemId).value;
            ItemRapidMenu.Instance.Slot3_T.text = itemDatabase.Find(q => q.id == itemId).value.ToString();
            ItemRapidMenu.Instance.Slot3_I.sprite = itemDatabase.Find(q => q.id == itemId).icon;
            Num.text = itemDatabase.Find(q => q.id == itemId).value.ToString();
            UpdateMenuRapido.Instance.Slot3_T.text = itemDatabase.Find(q => q.id == itemId).value.ToString();
            UpdateMenuRapido.Instance.Slot3_I.sprite = itemDatabase.Find(q => q.id == itemId).icon;
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
            ItemRapidMenu.Instance.Slot4 = itemDatabase.Find(q => q.id == itemId).id;
            UpdateMenuRapido.Instance.Slot4 = itemDatabase.Find(q => q.id == itemId).id;
            if(ItemRapidMenu.Instance.isSlot4 != ItemRapidMenu.Instance.isSlot2 ||
            ItemRapidMenu.Instance.isSlot4 != ItemRapidMenu.Instance.isSlot3 ||
           ItemRapidMenu.Instance.isSlot4 != ItemRapidMenu.Instance.isSlot1)
        { 
            ItemRapidMenu.Instance.MXV4 = itemDatabase.Find(q => q.id == itemId).value;
            UpdateMenuRapido.Instance.MXV4 = itemDatabase.Find(q => q.id == itemId).value;
            ItemRapidMenu.Instance.Slot4_T.text = itemDatabase.Find(q => q.id == itemId).value.ToString();
            ItemRapidMenu.Instance.Slot4_I.sprite = itemDatabase.Find(q => q.id == itemId).icon;
            Num.text = itemDatabase.Find(q => q.id == itemId).value.ToString();
            UpdateMenuRapido.Instance.Slot4_T.text = itemDatabase.Find(q => q.id == itemId).value.ToString();
            UpdateMenuRapido.Instance.Slot4_I.sprite = itemDatabase.Find(q => q.id == itemId).icon;
            
            //
            imageColor = UpdateMenuRapido.Instance.Slot4_I.color;
            imageColor.a = 1f;
            UpdateMenuRapido.Instance.Slot4_I.color = imageColor;
            //
            imageColor = ItemRapidMenu.Instance.Slot4_I.color;
            imageColor.a = 1f;
            ItemRapidMenu.Instance.Slot4_I.color = imageColor;
        }
        }else if(ItemRapidMenu.Instance.isSlot5)
        {
            ItemRapidMenu.Instance.Slot5_T.text = itemDatabase.Find(q => q.id == itemId).value.ToString();
            ItemRapidMenu.Instance.Slot5_I.sprite = itemDatabase.Find(q => q.id == itemId).icon;
            ItemRapidMenu.Instance.Slot5 = selectedId;
            UpdateMenuRapido.Instance.Slot5 = selectedId;
            UpdateMenuRapido.Instance.Slot5_T.text = itemDatabase.Find(q => q.id == itemId).value.ToString();
            UpdateMenuRapido.Instance.Slot5_I.sprite = itemDatabase.Find(q => q.id == itemId).icon;
            ItemRapidMenu.Instance.MXV5 = itemDatabase.Find(q => q.id == itemId).value;
            //
            imageColor = UpdateMenuRapido.Instance.Slot5_I.color;
            imageColor.a = 1f;
            UpdateMenuRapido.Instance.Slot5_I.color = imageColor;
            //
            imageColor = ItemRapidMenu.Instance.Slot5_I.color;
            imageColor.a = 1f;
            ItemRapidMenu.Instance.Slot5_I.color = imageColor;
        }else if(ItemRapidMenu.Instance.isSlot6)
        {
            ItemRapidMenu.Instance.Slot6_T.text = itemDatabase.Find(q => q.id == itemId).value.ToString();
            ItemRapidMenu.Instance.Slot6_I.sprite = itemDatabase.Find(q => q.id == itemId).icon;
            ItemRapidMenu.Instance.Slot6 = selectedId;
            UpdateMenuRapido.Instance.Slot6 = selectedId;
            UpdateMenuRapido.Instance.Slot6_T.text = itemDatabase.Find(q => q.id == itemId).value.ToString();
            UpdateMenuRapido.Instance.Slot6 = itemDatabase.Find(q => q.id == itemId).value;
            UpdateMenuRapido.Instance.Slot6_I.sprite = itemDatabase.Find(q => q.id == itemId).icon;
            ItemRapidMenu.Instance.MXV6 = itemDatabase.Find(q => q.id == itemId).value;
            //        
            imageColor = UpdateMenuRapido.Instance.Slot6_I.color;
            imageColor.a = 1f;
            UpdateMenuRapido.Instance.Slot6_I.color = imageColor;
            //
            imageColor = ItemRapidMenu.Instance.Slot6_I.color;
            imageColor.a = 1f;
            ItemRapidMenu.Instance.Slot6_I.color = imageColor;

        }else if(ItemRapidMenu.Instance.isSlot7)
        {
            ItemRapidMenu.Instance.Slot7_T.text = itemDatabase.Find(q => q.id == itemId).value.ToString();
            ItemRapidMenu.Instance.Slot7_I.sprite = itemDatabase.Find(q => q.id == itemId).icon;
            ItemRapidMenu.Instance.Slot7 = selectedId;
            UpdateMenuRapido.Instance.Slot7 = selectedId;
            UpdateMenuRapido.Instance.Slot7_T.text = itemDatabase.Find(q => q.id == itemId).value.ToString();
            UpdateMenuRapido.Instance.Slot7 = itemDatabase.Find(q => q.id == itemId).value;
            UpdateMenuRapido.Instance.Slot7_I.sprite = itemDatabase.Find(q => q.id == itemId).icon;
            ItemRapidMenu.Instance.MXV7 = itemDatabase.Find(q => q.id == itemId).value;
            //
            imageColor = UpdateMenuRapido.Instance.Slot7_I.color;
            imageColor.a = 1f;
            UpdateMenuRapido.Instance.Slot7_I.color = imageColor;
            //
            imageColor = ItemRapidMenu.Instance.Slot7_I.color;
            imageColor.a = 1f;
            ItemRapidMenu.Instance.Slot7_I.color = imageColor;

        }else if(ItemRapidMenu.Instance.isSlot8)
        {
            ItemRapidMenu.Instance.Slot8_T.text = itemDatabase.Find(q => q.id == itemId).value.ToString();
            ItemRapidMenu.Instance.Slot8_I.sprite = itemDatabase.Find(q => q.id == itemId).icon;
            ItemRapidMenu.Instance.Slot8 = selectedId;
            UpdateMenuRapido.Instance.Slot8 = selectedId;
            UpdateMenuRapido.Instance.Slot8_T.text = itemDatabase.Find(q => q.id == itemId).value.ToString();
            UpdateMenuRapido.Instance.Slot8 = itemDatabase.Find(q => q.id == itemId).value;
            UpdateMenuRapido.Instance.Slot8_I.sprite = itemDatabase.Find(q => q.id == itemId).icon;
            ItemRapidMenu.Instance.MXV8 = itemDatabase.Find(q => q.id == itemId).value;
            //
            imageColor = UpdateMenuRapido.Instance.Slot8_I.color;
            imageColor.a = 1f;
            UpdateMenuRapido.Instance.Slot8_I.color = imageColor;
            //
            imageColor = ItemRapidMenu.Instance.Slot8_I.color;
            imageColor.a = 1f;
            ItemRapidMenu.Instance.Slot8_I.color = imageColor;

        }

    }else if(itemId >= 0 && isKey)
    {
        previewImages_C.sprite = itemDatabase.Find(q => q.id == itemId).icon;
        descriptions_C.text = itemDatabase.Find(q => q.id == itemId).Description;
        NameItems_C.text = itemDatabase.Find(q => q.id == itemId).itemName;
        selectedId = itemDatabase.Find(q => q.id == itemId).id;
    }else if(itemId >= 0 && isKatana)
    {
            //ItemRapidMenu.Instance.SlotKat_T.text = itemDatabase.Find(q => q.id == itemId).value.ToString();
            ItemRapidMenu.Instance.SlotKat_I.sprite = itemDatabase.Find(q => q.id == itemId).icon;
            ItemRapidMenu.Instance.SlotKat = selectedId;
            ChangeHeroSkin.Instance.katana = NameSkin;
            previewImages_E.sprite = itemDatabase.Find(q => q.id == itemId).icon;
            descriptions_E.text = itemDatabase.Find(q => q.id == itemId).Description;
            NameItems_E.text = itemDatabase.Find(q => q.id == itemId).itemName;
            ChangeHeroSkin.Instance.UpdateCharacterSkin();
		    ChangeHeroSkin.Instance.UpdateCombinedSkin(); 
            PuppetSkin.Instance.katana = NameSkin;
            PuppetSkin.Instance.UpdateCharacterSkinUI(NameSkin);
            PuppetSkin.Instance.UpdateCombinedSkinUI(); 

		   // PuppetSkin.Instance.UpdateCombinedSkinUI();
    }else if(itemId >= 0 && isDress)
    {
            //ItemRapidMenu.Instance.SlotKat_T.text = itemDatabase.Find(q => q.id == itemId).value.ToString();
            ItemRapidMenu.Instance.SlotDres_I.sprite = itemDatabase.Find(q => q.id == itemId).icon;
            ItemRapidMenu.Instance.SlotDres = selectedId;
            ChangeHeroSkin.Instance.DressSkin = NameSkin;
            previewImages_E.sprite = itemDatabase.Find(q => q.id == itemId).icon;
            descriptions_E.text = itemDatabase.Find(q => q.id == itemId).Description;
            NameItems_E.text = itemDatabase.Find(q => q.id == itemId).itemName;
            ChangeHeroSkin.Instance.UpdateCharacterSkin();
	    	ChangeHeroSkin.Instance.UpdateCombinedSkin();
            PuppetSkin.Instance.DressSkin = NameSkin;
            PuppetSkin.Instance.UpdateCharacterSkinUI(NameSkin);
            PuppetSkin.Instance.UpdateCombinedSkinUI(); 
		   // PuppetSkin.Instance.UpdateCombinedSkinUI();
    }
}

private void OnEnable()
{
    SceneManager.sceneLoaded += OnSceneLoaded;
}

private void OnDisable()
{
    SceneManager.sceneLoaded -= OnSceneLoaded;
}
public void ItemActive(int id)
{
    // Imposta lo stato di completamento della quest a true
    itemActive[id] = true;  
}
// Questo metodo viene chiamato quando una nuova scena viene caricata
private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
{
    // Cerca tutti gli oggetti con il tag "Item" nella scena appena caricata
    GameObject[] collectibleItem = GameObject.FindGameObjectsWithTag("Item");

    // Cicla su tutti gli oggetti trovati
    foreach (GameObject Item in collectibleItem)
    {
        // Cerca il componente "ItemPickup" collegato all'oggetto
        ItemPickup ItemTake = Item.GetComponent<ItemPickup>();

        // Se l'oggetto ha il componente "ItemPickup"
        if (ItemTake != null)
        {
            // Recupera l'identificatore dell'oggetto
            int ItemId = ItemTake.Item.id;

            // Verifica se l'oggetto è già stato raccolto
            for (int i = 0; i < itemActive.Length; i++)
            {
                if (itemActive[i] && i == ItemId)
                {
                    // Se l'oggetto è stato raccolto, imposta il suo stato "isCollected" su true
                    ItemTake.isCollected = true;
                    break;
                }
            }
        }
    }
}




}

