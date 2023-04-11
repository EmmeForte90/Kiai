using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UpdateMenuRapido : MonoBehaviour
{
    // Mappa che mappa gli id delle skill ai loro valori
    Dictionary<int, Item> itemMap = new Dictionary<int, Item>();

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

    public int MXV1; // Id dell'abilità selezionata
    public int MXV2; // Id dell'abilità selezionata
    public int MXV3; // Id dell'abilità selezionata
    public int MXV4; // Id dell'abilità selezionata
    public int MXV5; // Id dell'abilità selezionata
    public int MXV6; // Id dell'abilità selezionata
    public int MXV7; // Id dell'abilità selezionata
    public int MXV8; // Id dell'abilità selezionata
    
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

    [SerializeField]public GameObject selSlot1;
    [SerializeField]public GameObject selSlot2;
    [SerializeField]public GameObject selSlot3;
    [SerializeField]public GameObject selSlot4;
    [SerializeField]public GameObject selSlot5;
    [SerializeField]public GameObject selSlot6;
    [SerializeField]public GameObject selSlot7;
    [SerializeField]public GameObject selSlot8;

    [SerializeField]public GameObject DesSlot1;
    [SerializeField]public GameObject DesSlot2;
    [SerializeField]public GameObject DesSlot3;
    [SerializeField]public GameObject DesSlot4;
    [SerializeField]public GameObject DesSlot5;
    [SerializeField]public GameObject DesSlot6;
    [SerializeField]public GameObject DesSlot7;
    [SerializeField]public GameObject DesSlot8;


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
        
        PlayerWeaponManager.instance.SetWeapon(ItemRapidMenu.Instance.Slot1);
        selSlot1.SetActive(true);
        selSlot2.SetActive(false);
        selSlot3.SetActive(false);
        selSlot4.SetActive(false);
        selSlot5.SetActive(false);
        selSlot6.SetActive(false);
        selSlot7.SetActive(false);
        selSlot8.SetActive(false);

        //StartCoroutine(closeSel());

    }



public void Selleft()
    {
        PlayerWeaponManager.instance.SetWeapon(ItemRapidMenu.Instance.Slot2);
        selSlot1.SetActive(false);
        selSlot2.SetActive(true);
        selSlot3.SetActive(false);
        selSlot4.SetActive(false);
        selSlot5.SetActive(false);
        selSlot6.SetActive(false);
        selSlot7.SetActive(false);
        selSlot8.SetActive(false);
       // StartCoroutine(closeSel());

    }

    
public void Selright()
    {
        PlayerWeaponManager.instance.SetWeapon(ItemRapidMenu.Instance.Slot3);
        selSlot1.SetActive(false);
        selSlot2.SetActive(false);
        selSlot3.SetActive(true);
        selSlot4.SetActive(false);
        selSlot5.SetActive(false);
        selSlot6.SetActive(false);
        selSlot7.SetActive(false);
        selSlot8.SetActive(false);
        //StartCoroutine(closeSel());

    }

public void Selbottom()
    {
        PlayerWeaponManager.instance.SetWeapon(ItemRapidMenu.Instance.Slot4);
        selSlot1.SetActive(false);
        selSlot2.SetActive(false);
        selSlot3.SetActive(false);
        selSlot4.SetActive(true);
        selSlot5.SetActive(false);
        selSlot6.SetActive(false);
        selSlot7.SetActive(false);
        selSlot8.SetActive(false);
        //StartCoroutine(closeSel());

    }


IEnumerator closeSel()
{

    
        yield return new WaitForSeconds(timeSelection);
    


}
}

