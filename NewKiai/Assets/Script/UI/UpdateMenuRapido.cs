using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UpdateMenuRapido : MonoBehaviour
{
    // Mappa che mappa gli id delle skill ai loro valori
    Dictionary<int, Item> itemMap = new Dictionary<int, Item>();
[SerializeField] public Sprite icon0; // Define icon1 as an Image variable
[SerializeField] public Sprite icon1; // Define icon1 as an Image variable
[SerializeField] public Sprite icon2; // Define icon1 as an Image variable
[SerializeField] public Sprite icon3; // Define icon1 as an Image variable

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


    public float timeSelection = 0.1f; // ritardo tra la spawn di ogni moneta


public static UpdateMenuRapido Instance;


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }


public void Selup()
    {
        
        PlayerWeaponManager.instance.SetWeapon(ItemRapidMenu.Instance.selectedId);
    
        //StartCoroutine(closeSel());

    }

public void Selbottom()
    {
        PlayerWeaponManager.instance.SetWeapon(ItemRapidMenu.Instance.selectedId);
       
        //StartCoroutine(closeSel());

    }

public void Selleft()
    {
        PlayerWeaponManager.instance.SetWeapon(ItemRapidMenu.Instance.selectedId);
      
       // StartCoroutine(closeSel());

    }

    
public void Selright()
    {
        PlayerWeaponManager.instance.SetWeapon(ItemRapidMenu.Instance.selectedId);
      
        //StartCoroutine(closeSel());

    }
IEnumerator closeSel()
{

    
        yield return new WaitForSeconds(timeSelection);
    


}
}

