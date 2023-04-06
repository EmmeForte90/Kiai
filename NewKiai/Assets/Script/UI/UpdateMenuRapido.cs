using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UpdateMenuRapido : MonoBehaviour
{
    // Mappa che mappa gli id delle skill ai loro valori
    Dictionary<int, Skill> skillMap = new Dictionary<int, Skill>();
[SerializeField] public Sprite icon0; // Define icon1 as an Image variable
[SerializeField] public Sprite icon1; // Define icon1 as an Image variable
[SerializeField] public Sprite icon2; // Define icon1 as an Image variable
[SerializeField] public Sprite icon3; // Define icon1 as an Image variable

 [HideInInspector]
    public int idleft = -1; // Id dell'abilità selezionata
    [HideInInspector]
    public int idright = -1; // Id dell'abilità selezionata
    [HideInInspector]
    public int idup= -1; // Id dell'abilità selezionata
    [HideInInspector]
    public int idbottom= -1; // Id dell'abilità selezionata

    
    public int Vleft; // Id dell'abilità selezionata
  
    public int Vright; // Id dell'abilità selezionata
    
    public int Vup; // Id dell'abilità selezionata
    
    public int Vbottom; // Id dell'abilità selezionata

    [SerializeField] public TextMeshProUGUI SkillLeft_T;
    [SerializeField] public TextMeshProUGUI SkillRight_T;
    [SerializeField] public TextMeshProUGUI SkillUp_T;
    [SerializeField] public TextMeshProUGUI SkillBottom_T;

    [SerializeField] public Image SkillLeft;
    [SerializeField] public Image SkillRight;
    [SerializeField] public Image SkillUp;
    [SerializeField] public Image SkillBottom;

    [SerializeField] public Image SkillLeftsel;
    [SerializeField] public Image SkillRightsel;
    [SerializeField] public Image SkillUpsel;
    [SerializeField] public Image SkillBottomsel;
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
        
        PlayerWeaponManager.instance.SetWeapon(SkillMenu.Instance.selectedId);
        SkillUpsel.gameObject.SetActive(true);
        SkillLeftsel.gameObject.SetActive(false);        
        SkillRightsel.gameObject.SetActive(false);
        SkillBottomsel.gameObject.SetActive(false);
        //StartCoroutine(closeSel());

    }

public void Selbottom()
    {
        PlayerWeaponManager.instance.SetWeapon(SkillMenu.Instance.selectedId);
        SkillBottomsel.gameObject.SetActive(true);
        SkillLeftsel.gameObject.SetActive(false);
        SkillUpsel.gameObject.SetActive(false);
        SkillRightsel.gameObject.SetActive(false);
        //StartCoroutine(closeSel());

    }

public void Selleft()
    {
        PlayerWeaponManager.instance.SetWeapon(SkillMenu.Instance.selectedId);
        SkillLeftsel.gameObject.SetActive(true);
        SkillUpsel.gameObject.SetActive(false);
        SkillRightsel.gameObject.SetActive(false);
        SkillBottomsel.gameObject.SetActive(false);
       // StartCoroutine(closeSel());

    }

    
public void Selright()
    {
        PlayerWeaponManager.instance.SetWeapon(SkillMenu.Instance.selectedId);
        SkillRightsel.gameObject.SetActive(true);
        SkillLeftsel.gameObject.SetActive(false);
        SkillUpsel.gameObject.SetActive(false);
        SkillBottomsel.gameObject.SetActive(false);
        //StartCoroutine(closeSel());

    }
IEnumerator closeSel()
{

    
        yield return new WaitForSeconds(timeSelection);
                SkillLeftsel.gameObject.SetActive(false);
                SkillUpsel.gameObject.SetActive(false);
                SkillRightsel.gameObject.SetActive(false);
                SkillBottomsel.gameObject.SetActive(false);


}
}

