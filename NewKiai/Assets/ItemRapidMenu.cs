using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ItemRapidMenu : MonoBehaviour
{
    // Mappa che mappa gli id delle skill ai loro valori
    Dictionary<int, Item> itemMap = new Dictionary<int, Item>();
[SerializeField] private Sprite icon0; // Define icon1 as an Image variable
[SerializeField] private Sprite icon1; // Define icon1 as an Image variable
[SerializeField] private Sprite icon2; // Define icon1 as an Image variable
[SerializeField] private Sprite icon3; // Define icon1 as an Image variable
[SerializeField] private Sprite icon4; // Define icon1 as an Image variable
[SerializeField] private Sprite icon5; // Define icon1 as an Image variable
[SerializeField] private Sprite icon6; // Define icon1 as an Image variable
[SerializeField] private Sprite icon7; // Define icon1 as an Image variable
[SerializeField] private Sprite icon8; // Define icon1 as an Image variable
[SerializeField] private Sprite icon9; // Define icon1 as an Image variable
[SerializeField] private Sprite icon10; // Define icon1 as an Image variable
[SerializeField] private Sprite icon11; // Define icon1 as an Image variable
[SerializeField] private Sprite icon12; // Define icon1 as an Image variable
[SerializeField] private Sprite icon13; // Define icon1 as an Image variable
[SerializeField] private Sprite icon14; // Define icon1 as an Image variable
[SerializeField] private Sprite icon15; // Define icon1 as an Image variable
[SerializeField] private Sprite icon16; // Define icon1 as an Image variable
[SerializeField] private Sprite icon17; // Define icon1 as an Image variable
[SerializeField] private Sprite icon18; // Define icon1 as an Image variable
[SerializeField] private Sprite icon19; // Define icon1 as an Image variable
[SerializeField] private Sprite icon20; // Define icon1 as an Image variable
[SerializeField] private Sprite icon21; // Define icon1 as an Image variable

    [HideInInspector]
    public int selectedId = -1; // Id dell'abilità selezionata  
    public int Slot1= -1; // Id dell'abilità selezionata
    public int Slot2 = -1; // Id dell'abilità selezionata 
    public int Slot3 = -1; // Id dell'abilità selezionata
    public int Slot4= -1; // Id dell'abilità selezionata 
    public int Slot5= -1; // Id dell'abilità selezionata
    public int Slot6= -1; // Id dell'abilità selezionata


    public int MXV1; // Id dell'abilità selezionata
    public int MXV2; // Id dell'abilità selezionata
    public int MXV3; // Id dell'abilità selezionata
    public int MXV4; // Id dell'abilità selezionata
    public int MXV5; // Id dell'abilità selezionata
    public int MXV6; // Id dell'abilità selezionata

    private float horDir;
    private float vertDir;


    [SerializeField]public TextMeshProUGUI Slot1_T;
    [SerializeField]public TextMeshProUGUI Slot2_T;
    [SerializeField]public TextMeshProUGUI Slot3_T;
    [SerializeField]public TextMeshProUGUI Slot4_T;
    [SerializeField]public TextMeshProUGUI Slot5_T;
    [SerializeField]public TextMeshProUGUI Slot6_T;


    [SerializeField]public Image Slot1_I;
    [SerializeField]public Image Slot2_I;
    [SerializeField]public Image Slot3_I;
    [SerializeField]public Image Slot4_I;
    [SerializeField]public Image Slot5_I;
    [SerializeField]public Image Slot6_I;

public static ItemRapidMenu Instance;


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    void Start()
    {
        // Aggiungi le tue skill alla mappa
        itemMap.Add(-1, new Item("noSkill", 0, icon0));//Noitem
        itemMap.Add(1, new Item("Item 1", 10, icon1));//Kunai
        itemMap.Add(2, new Item("Item 2", 5, icon2));//Shuriken
        itemMap.Add(3, new Item("Item 3", 5, icon3));//Bomb
        itemMap.Add(4, new Item("Item 4", 10, icon4));//Spille
        itemMap.Add(5, new Item("Item 5", 5, icon5));//freccia
        itemMap.Add(6, new Item("Item 6", 7, icon6));//Taneghasima
        itemMap.Add(7, new Item("Item 7", 10, icon7));//slash
        itemMap.Add(8, new Item("Item 8", 10, icon8));//Penetrating
        itemMap.Add(9, new Item("Item 9", 5, icon9));//Globo
        itemMap.Add(10, new Item("Item 10", 5, icon10));//shotgun
        itemMap.Add(11, new Item("Item 11", 5, icon11));//dashsaw
        itemMap.Add(12, new Item("Item 12", 3, icon12));//wall
        itemMap.Add(13, new Item("Item 13", 5, icon13));//bomb
        itemMap.Add(14, new Item("Item 14", 10, icon14));//boomerang
        itemMap.Add(15, new Item("Item 15", 5, icon15));//gladio
        itemMap.Add(16, new Item("Item 16", 2, icon16));//lumen
        itemMap.Add(17, new Item("Item 17", 3, icon17));//turris
        itemMap.Add(18, new Item("Item 18", 5, icon18));//shield
        itemMap.Add(19, new Item("Item 19", 5, icon19));//flame
        itemMap.Add(20, new Item("Item 20", 3, icon20));//aura
        itemMap.Add(21, new Item("Item 21", 3, icon21));//heal

    }

    void Update()
    {
        horDir = Input.GetAxisRaw("Horizontal");
        vertDir = Input.GetAxisRaw("Vertical");
    }

    public void AssignId(int id)
    {
        selectedId = id; // Assegna l'id dell'abilità selezionata
    }

   public void Assign1()
{
    // Recupera la skill corrispondente all'id selezionato
    Item selcetedItem = itemMap[selectedId];
    //PlayerWeaponManager.instance.SetWeapon(selectedId);

    if (selectedId > 0)
    {
        Slot1_T.text = selcetedItem.value.ToString();
        Slot1_I.sprite = selcetedItem.icon;
        Slot1 = selectedId;
        UpdateMenuRapido.Instance.Slot1 = selectedId;
         UpdateMenuRapido.Instance.Slot1_T.text = selcetedItem.value.ToString();
         UpdateMenuRapido.Instance.Slot1 = selcetedItem.value;
         UpdateMenuRapido.Instance.Slot1_I.sprite = selcetedItem.icon;
         MXV1 = selcetedItem.value;
    }
}
  public void Assign2()
{
    // Recupera la skill corrispondente all'id selezionato
    Item selcetedItem = itemMap[selectedId];
    //PlayerWeaponManager.instance.SetWeapon(selectedId);

    if (selectedId > 0)
    {
        Slot2_T.text = selcetedItem.value.ToString();
        Slot2_I.sprite = selcetedItem.icon;
        Slot2 = selectedId;
        UpdateMenuRapido.Instance.Slot2 = selectedId;
        UpdateMenuRapido.Instance.Slot2_T.text = selcetedItem.value.ToString();
        UpdateMenuRapido.Instance.Slot2 = selcetedItem.value;
        UpdateMenuRapido.Instance.Slot2_I.sprite = selcetedItem.icon;
        MXV2 = selcetedItem.value;
        
    }
}  
public void Assign3()
{
    // Recupera la skill corrispondente all'id selezionato
    Item selcetedItem = itemMap[selectedId];
    //PlayerWeaponManager.instance.SetWeapon(selectedId);

    if (selectedId > 0)
    {
        Slot3_T.text = selcetedItem.value.ToString();
        Slot3_I.sprite = selcetedItem.icon;
        Slot3 = selectedId;
        UpdateMenuRapido.Instance.Slot3 = selectedId;
        UpdateMenuRapido.Instance.Slot3_T.text = selcetedItem.value.ToString();
        UpdateMenuRapido.Instance.Slot3 = selcetedItem.value;
        UpdateMenuRapido.Instance.Slot3_I.sprite = selcetedItem.icon;
        MXV3 = selcetedItem.value;
    }
}  public void Assign4()
{
    // Recupera la skill corrispondente all'id selezionato
    Item selcetedItem = itemMap[selectedId];
   // PlayerWeaponManager.instance.SetWeapon(selectedId);

    if (selectedId > 0)
    {
        Slot4_T.text = selcetedItem.value.ToString();
        Slot4_I.sprite = selcetedItem.icon;
        Slot4 = selectedId;
        UpdateMenuRapido.Instance.Slot4 = selectedId;
        UpdateMenuRapido.Instance.Slot4_T.text = selcetedItem.value.ToString();
        UpdateMenuRapido.Instance.Slot4 = selcetedItem.value;
        UpdateMenuRapido.Instance.Slot4_I.sprite = selcetedItem.icon;
        MXV4 = selcetedItem.value;
    }
}
public void Assign5()
{
    // Recupera la skill corrispondente all'id selezionato
    Item selcetedItem = itemMap[selectedId];
   // PlayerWeaponManager.instance.SetWeapon(selectedId);

    if (selectedId > 0)
    {
        Slot5_T.text = selcetedItem.value.ToString();
        Slot5_I.sprite = selcetedItem.icon;
        Slot5 = selectedId;
        UpdateMenuRapido.Instance.Slot5 = selectedId;
        UpdateMenuRapido.Instance.Slot5_T.text = selcetedItem.value.ToString();
        UpdateMenuRapido.Instance.Slot5 = selcetedItem.value;
        UpdateMenuRapido.Instance.Slot5_I.sprite = selcetedItem.icon;
        MXV5 = selcetedItem.value;
    }
}
public void Assign6()
{
    // Recupera la skill corrispondente all'id selezionato
    Item selcetedItem = itemMap[selectedId];
   // PlayerWeaponManager.instance.SetWeapon(selectedId);

    if (selectedId > 0)
    {
        Slot6_T.text = selcetedItem.value.ToString();
        Slot6_I.sprite = selcetedItem.icon;
        Slot6 = selectedId;
        UpdateMenuRapido.Instance.Slot6 = selectedId;
        UpdateMenuRapido.Instance.Slot6_T.text = selcetedItem.value.ToString();
        UpdateMenuRapido.Instance.Slot6 = selcetedItem.value;
        UpdateMenuRapido.Instance.Slot6_I.sprite = selcetedItem.icon;
        MXV6 = selcetedItem.value;
    }
}
}





