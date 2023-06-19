using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ItemRapidMenu : MonoBehaviour
{
    // Mappa che mappa gli id delle skill ai loro valori
    Dictionary<int, Item> itemMap = new Dictionary<int, Item>();
    public float timeSelection = 0.1f; // ritardo tra la spawn di ogni moneta

    [HideInInspector]
    public int selectedId = -1; // Id dell'abilità selezionata  
    public int Slot1= -1; // Id dell'abilità selezionata
    public int Slot2= -1; // Id dell'abilità selezionata 
    public int Slot3= -1; // Id dell'abilità selezionata
    public int Slot4= -1; // Id dell'abilità selezionata 
    public int Slot5= -1; // Id dell'abilità selezionata
    public int Slot6= -1; // Id dell'abilità selezionata
    public int Slot7= -1; // Id dell'abilità selezionata
    public int Slot8= -1; // Id dell'abilità selezionata
    public int SlotKat= -1; // Id dell'abilità selezionata
    public int SlotDres= -1; // Id dell'abilità selezionata


    public int MXV1; // Id dell'abilità selezionata
    public int MXV2; // Id dell'abilità selezionata
    public int MXV3; // Id dell'abilità selezionata
    public int MXV4; // Id dell'abilità selezionata
    public int MXV5; // Id dell'abilità selezionata
    public int MXV6; // Id dell'abilità selezionata
    public int MXV7; // Id dell'abilità selezionata
    public int MXV8; // Id dell'abilità selezionata

    private float horDir;
    private float vertDir;
        
    [HideInInspector]
    public Item Item_1;
    [HideInInspector]
    public Item Item_2;
    [HideInInspector]
    public Item Item_3;
    [HideInInspector]
    public Item Item_4;

    [HideInInspector]
    public bool isSlot0;
    public bool isSlot1;
    public bool isSlot2;
    public bool isSlot3;
    public bool isSlot4;
    public bool isSlot5;
    public bool isSlot6;
    public bool isSlot7;
    public bool isSlot8;
    public bool isSlotKat;
    public bool isSlotDres;

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
    [SerializeField]public Image SlotKat_I;
    [SerializeField]public Image SlotDres_I;
        
    public bool[] itemActive;

public static ItemRapidMenu Instance;


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    void Update()
    {
        horDir = Input.GetAxisRaw("Horizontal");
        vertDir = Input.GetAxisRaw("Vertical");

//Se il valore è zero cancella il numero dallo slot
if(GameplayManager.instance.startGame){
    if(UpdateMenuRapido.Instance.MXV1 <= 0)
    {
        Slot1_T.text = null;
    }
    if(UpdateMenuRapido.Instance.MXV2 <= 0)
    {
        Slot2_T.text = null;
    }
    if(UpdateMenuRapido.Instance.MXV3 <= 0)
    {
       Slot3_T.text = null;   
    }
    if(UpdateMenuRapido.Instance.MXV4 <= 0)
    {
        Slot4_T.text = null; 
    }
    if(UpdateMenuRapido.Instance.MXV5 <= 0)
    {
        Slot5_T.text = null;
    }
    if(UpdateMenuRapido.Instance.MXV6 <= 0)
    {
       Slot6_T.text = null;
    }
    if(UpdateMenuRapido.Instance.MXV7 <= 0)
    {
        Slot7_T.text = null;
    }if(UpdateMenuRapido.Instance.MXV8 <= 0)
    {
        Slot8_T.text = null;
    }
    }}

    public void AssignId(int id)
    {
        selectedId = id; // Assegna l'id dell'abilità selezionata
    }

 public void AssignKat()
{    
    isSlotKat = true;
}

 public void AssignDres()
{    
    isSlotDres = true;
}
   public void Assign0()
{    
    isSlot0 = true;
    isSlot1 = false;
    isSlot2 = false;
    isSlot3 = false;
    isSlot4 = false;
    isSlot5 = false;
    isSlot6 = false;
    isSlot7 = false;
    isSlot8 = false;

}
   public void Assign1()
{    
 
    isSlot0 = false;
    isSlot1 = true;
    isSlot2 = false;
    isSlot3 = false;
    isSlot4 = false;
    isSlot5 = false;
    isSlot6 = false;
    isSlot7 = false;
    isSlot8 = false;
}
  
public void Assign2()
{    
  
    isSlot0 = false;
    isSlot1 = false;
    isSlot2 = true;
    isSlot3 = false;
    isSlot4 = false;
    isSlot5 = false;
    isSlot6 = false;
    isSlot7 = false;
    isSlot8 = false;
}
  
public void Assign3()
{
   
    isSlot0 = false;
    isSlot1 = false;
    isSlot2 = false;
    isSlot3 = true;
    isSlot4 = false;
    isSlot5 = false;
    isSlot6 = false;
    isSlot7 = false;
    isSlot8 = false;
}

 public void Assign4()
{
    
    isSlot0 = false;
    isSlot1 = false;
    isSlot2 = false;
    isSlot3 = false;
    isSlot4 = true;
    isSlot5 = false;
    isSlot6 = false;
    isSlot7 = false;
    isSlot8 = false;
}
public void Assign5()
{
    isSlot0 = false;
    isSlot1 = false;
    isSlot2 = false;
    isSlot3 = false;
    isSlot4 = false;
    isSlot5 = true;
    isSlot6 = false;
    isSlot7 = false;
    isSlot8 = false;
}
public void Assign6()
{
    isSlot0 = false;
    isSlot1 = false;
    isSlot2 = false;
    isSlot3 = false;
    isSlot4 = false;
    isSlot5 = false;
    isSlot6 = true;
    isSlot7 = false;
    isSlot8 = false;
}
public void Assign7()
{
    isSlot0 = false;
    isSlot1 = false;
    isSlot2 = false;
    isSlot3 = false;
    isSlot4 = false;
    isSlot5 = false;
    isSlot6 = false;
    isSlot7 = true;
    isSlot8 = false;
}
public void Assign8()
{
    isSlot0 = false;
    isSlot1 = false;
    isSlot2 = false;
    isSlot3 = false;
    isSlot4 = false;
    isSlot5 = false;
    isSlot6 = false;
    isSlot7 = false;
    isSlot8 = true;
}


}





