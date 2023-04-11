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
   public Transform ItemContent;
   public  GameObject InventoryItem;
   [HideInInspector] public int qID;
    // Scriptable Object delle item
   public List<Item> itemDatabase;
    private int val = 1;
    public bool[] itemActive;

    // Riferimenti ai componenti delle immagini di preview e delle descrizioni
    public Image previewImages;
    public TextMeshProUGUI descriptions;
    public TextMeshProUGUI Num;
    public TextMeshProUGUI NameItems;
    public int selectedId = -1; // Id dell'abilità selezionata  

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
    [SerializeField] public GameObject[] SlotTotM;
    [SerializeField] public GameObject Selector;
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

   public void SlotActivated(int id)
{
    SlotTot[id].SetActive(true);
    SlotTotM[id].SetActive(true);
    SlotIcon[id] = true;   
}

     // Metodo per aggiungere una nuova item al database
    public void AddItem(Item newItem)
{
      Item existingItem = itemDatabase.Find(q => q.id == newItem.id);
    if (existingItem != null)
    {
        existingItem.value++;
    }
    else
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

    if (val <= 1)
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

    if (val <= 1)
    {
        itemDatabase.Remove(existingItem); // rimuovi completamente l'oggetto dalla lista
            // Se l'item è già presente nella lista, incrementa il suo valore
            foreach (Transform child in ItemContent.transform)
            {
                if (child.name == "ItemButton_" + existingItem.id)
                {   
                    Destroy(child.gameObject);
                    break;
                }
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
       

       if (item != null)
    {
        if (ItemAlreadyInList(item.id))
        {
            

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
        else
        {
    // il codice qui sotto verrà eseguito solo se l'item esiste e non è già presente nella lista
    // Istanzia il prefab del bottone della item nella lista UI
    InventoryItem = Instantiate(InventoryItem, ItemContent);
    // Recupera il riferimento al componente del titolo della item e del bottone
    //var questName = obj.transform.Find("Name_Item").GetComponent<TextMeshProUGUI>();
    var Itemimg = InventoryItem.transform.Find("Icon_item").GetComponent<Image>();

    // Assegna l'id univoco al game object istanziato
    InventoryItem.name = "ItemButton_" + item.id;

    // Assegna il nome della item al componente del titolo
    //questName.text = item.itemName;

    if (Itemimg != null && item.icon != null)
    {
        Itemimg.sprite = item.icon;
    }

    // Assegna i valori desiderati ai componenti dell'immagine di preview e della descrizione del pulsante della item
    if (previewImages != null)
    {
        previewImages.sprite = item.icon;
    }

    if (descriptions != null)
    {
        descriptions.text = item.Description;
    }

    if (Num != null)
    {
        Num.text = val.ToString();
    }

    if (NameItems != null)
    {
        NameItems.text = item.itemName;
    }

    // Aggiungi un listener per il click del bottone
    var button = InventoryItem.GetComponent<Button>();
    button.onClick.AddListener(() => OnQuestButtonClicked(item.id, previewImages, descriptions, selectedId));
        }
}
    }
    
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

    

public void OnQuestButtonClicked(int itemId, Image previewImages, TextMeshProUGUI descriptions, int selectedId)
{
    if (itemId >= 0)
    {
        // Qui puoi fare qualcosa quando il pulsante della item viene cliccato, ad esempio aprire una finestra con i dettagli della item
        // Assegna i valori desiderati ai componenti dell'immagine di preview e della descrizione
        previewImages.sprite = itemDatabase.Find(q => q.id == itemId).icon;
        descriptions.text = itemDatabase.Find(q => q.id == itemId).Description;
        NameItems.text = itemDatabase.Find(q => q.id == itemId).itemName;
        selectedId = itemDatabase.Find(q => q.id == itemId).id;
        if(ItemRapidMenu.Instance.isSlot1)
        {
            ItemRapidMenu.Instance.Slot1_T.text = itemDatabase.Find(q => q.id == itemId).value.ToString();
            ItemRapidMenu.Instance.Slot1_I.sprite = itemDatabase.Find(q => q.id == itemId).icon;
            ItemRapidMenu.Instance.Slot1 = selectedId;
            UpdateMenuRapido.Instance.Slot1 = selectedId;
            UpdateMenuRapido.Instance.Slot1_T.text = itemDatabase.Find(q => q.id == itemId).value.ToString();
            //UpdateMenuRapido.Instance.Slot1 = itemDatabase.Find(q => q.id == itemId).value;
            UpdateMenuRapido.Instance.Slot1_I.sprite = itemDatabase.Find(q => q.id == itemId).icon;
            ItemRapidMenu.Instance.MXV1 = itemDatabase.Find(q => q.id == itemId).value;
            UpdateMenuRapido.Instance.MXV1 = itemDatabase.Find(q => q.id == itemId).value;
        }else if(ItemRapidMenu.Instance.isSlot2)
        {
            ItemRapidMenu.Instance.Slot2_T.text = itemDatabase.Find(q => q.id == itemId).value.ToString();
            ItemRapidMenu.Instance.Slot2_I.sprite = itemDatabase.Find(q => q.id == itemId).icon;
            ItemRapidMenu.Instance.Slot2 = selectedId;
            UpdateMenuRapido.Instance.Slot2 = selectedId;
            UpdateMenuRapido.Instance.Slot2_T.text = itemDatabase.Find(q => q.id == itemId).value.ToString();
            //UpdateMenuRapido.Instance.Slot2 = itemDatabase.Find(q => q.id == itemId).value;
            UpdateMenuRapido.Instance.Slot2_I.sprite = itemDatabase.Find(q => q.id == itemId).icon;
            ItemRapidMenu.Instance.MXV2 = itemDatabase.Find(q => q.id == itemId).value;
            UpdateMenuRapido.Instance.MXV2 = itemDatabase.Find(q => q.id == itemId).value;
        }else if(ItemRapidMenu.Instance.isSlot3)
        {
            ItemRapidMenu.Instance.Slot3_T.text = itemDatabase.Find(q => q.id == itemId).value.ToString();
            ItemRapidMenu.Instance.Slot3_I.sprite = itemDatabase.Find(q => q.id == itemId).icon;
            ItemRapidMenu.Instance.Slot3 = selectedId;
            UpdateMenuRapido.Instance.Slot3 = selectedId;
            UpdateMenuRapido.Instance.Slot3_T.text = itemDatabase.Find(q => q.id == itemId).value.ToString();
            //UpdateMenuRapido.Instance.Slot3 = itemDatabase.Find(q => q.id == itemId).value;
            UpdateMenuRapido.Instance.Slot3_I.sprite = itemDatabase.Find(q => q.id == itemId).icon;
            ItemRapidMenu.Instance.MXV3 = itemDatabase.Find(q => q.id == itemId).value;
            UpdateMenuRapido.Instance.MXV3 = itemDatabase.Find(q => q.id == itemId).value;
        }else if(ItemRapidMenu.Instance.isSlot4)
        {
            ItemRapidMenu.Instance.Slot4_T.text = itemDatabase.Find(q => q.id == itemId).value.ToString();
            ItemRapidMenu.Instance.Slot4_I.sprite = itemDatabase.Find(q => q.id == itemId).icon;
            ItemRapidMenu.Instance.Slot4 = selectedId;
            UpdateMenuRapido.Instance.Slot4 = selectedId;
            UpdateMenuRapido.Instance.Slot4_T.text = itemDatabase.Find(q => q.id == itemId).value.ToString();
           // UpdateMenuRapido.Instance.Slot4 = itemDatabase.Find(q => q.id == itemId).value;
            UpdateMenuRapido.Instance.Slot4_I.sprite = itemDatabase.Find(q => q.id == itemId).icon;
            ItemRapidMenu.Instance.MXV4 = itemDatabase.Find(q => q.id == itemId).value;
            UpdateMenuRapido.Instance.MXV4 = itemDatabase.Find(q => q.id == itemId).value;

        }else if(ItemRapidMenu.Instance.isSlot5)
        {
            ItemRapidMenu.Instance.Slot5_T.text = itemDatabase.Find(q => q.id == itemId).value.ToString();
            ItemRapidMenu.Instance.Slot5_I.sprite = itemDatabase.Find(q => q.id == itemId).icon;
            ItemRapidMenu.Instance.Slot5 = selectedId;
            UpdateMenuRapido.Instance.Slot5 = selectedId;
            UpdateMenuRapido.Instance.Slot5_T.text = itemDatabase.Find(q => q.id == itemId).value.ToString();
           // UpdateMenuRapido.Instance.Slot5 = itemDatabase.Find(q => q.id == itemId).value;
            UpdateMenuRapido.Instance.Slot5_I.sprite = itemDatabase.Find(q => q.id == itemId).icon;
            ItemRapidMenu.Instance.MXV5 = itemDatabase.Find(q => q.id == itemId).value;
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
        }

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

