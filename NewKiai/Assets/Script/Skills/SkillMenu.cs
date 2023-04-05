using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
/*
La classe SkillMenu contiene una mappa di abilità (skillMap), 
in cui gli id delle skill sono mappati ai loro valori, e tre icone di abilità 
(icon1, icon2, icon3). Viene anche dichiarato un valore intero "selectedId" 
per tenere traccia dell'abilità selezionata, e due variabili float "horDir" 
e "vertDir" per tenere traccia dell'input dell'utente.

Nel metodo Start(), le tre abilità definite precedentemente vengono aggiunte 
alla mappa skillMap.

Nel metodo Update(), l'input dell'utente viene registrato e memorizzato nelle 
variabili horDir e vertDir.

Ci sono quattro metodi pubblici AssignButtonUp(), AssignButtonLeft(), AssignButtonRight(), 
e AssignButtonBottom(), ognuno dei quali viene chiamato quando l'utente preme il pulsante 
corrispondente sulla tastiera o sul controller. Ognuno di questi metodi recupera l'abilità 
corrispondente all'id selezionato, se l'id è maggiore di zero (cioè se un'abilità è stata selezionata), 
quindi aggiorna l'etichetta e l'icona dell'abilità corrispondente. L'etichetta e l'icona dell'abilità 
vengono visualizzate in quattro posizioni differenti, corrispondenti ai pulsanti su, sinistra, 
destra e giù dell'interfaccia grafica del menu.

Infine, c'è un metodo pubblico AssignId(), che assegna l'id dell'abilità selezionata alla 
variabile selectedId. Questo metodo viene chiamato quando l'utente seleziona un'abilità dal menu.*/

public class SkillMenu : MonoBehaviour
{

// Mappa che mappa gli id delle skill ai loro valori
    Dictionary<int, Skill> skillMap = new Dictionary<int, Skill>();
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
    
    public int idleft = -1; // Id dell'abilità selezionata
    
    public int idright = -1; // Id dell'abilità selezionata
   
    public int idup= -1; // Id dell'abilità selezionata
   
    public int idbottom= -1; // Id dell'abilità selezionata


    
    public int MXVleft; // Id dell'abilità selezionata
   
    public int MXVright; // Id dell'abilità selezionata
    
    public int MXVup; // Id dell'abilità selezionata
   
    public int MXVbottom; // Id dell'abilità selezionata

    private float horDir;
    private float vertDir;


    [SerializeField] TextMeshProUGUI SkillLeft_T;
    [SerializeField] TextMeshProUGUI SkillRight_T;
    [SerializeField] TextMeshProUGUI SkillUp_T;
    [SerializeField] TextMeshProUGUI SkillBottom_T;

    [SerializeField] Image SkillLeft;
    [SerializeField] Image SkillRight;
    [SerializeField] Image SkillUp;
    [SerializeField] Image SkillBottom;

public static SkillMenu Instance;


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
        skillMap.Add(-1, new Skill("noSkill", 0, icon0));//NoSkill
        skillMap.Add(1, new Skill("Skill 1", 10, icon1));//upper
        skillMap.Add(2, new Skill("Skill 2", 5, icon2));//Shockwave
        skillMap.Add(3, new Skill("Skill 3", 5, icon3));//tornado
        skillMap.Add(4, new Skill("Skill 4", 10, icon4));//spinner
        skillMap.Add(5, new Skill("Skill 5", 5, icon5));//dashlunge
        skillMap.Add(6, new Skill("Skill 6", 7, icon6));//multilunge
        skillMap.Add(7, new Skill("Skill 7", 10, icon7));//slash
        skillMap.Add(8, new Skill("Skill 8", 10, icon8));//Penetrating
        skillMap.Add(9, new Skill("Skill 9", 5, icon9));//Globo
        skillMap.Add(10, new Skill("Skill 10", 5, icon10));//shotgun
        skillMap.Add(11, new Skill("Skill 11", 5, icon11));//dashsaw
        skillMap.Add(12, new Skill("Skill 12", 3, icon12));//wall
        skillMap.Add(13, new Skill("Skill 13", 5, icon13));//bomb
        skillMap.Add(14, new Skill("Skill 14", 10, icon14));//boomerang
        skillMap.Add(15, new Skill("Skill 15", 5, icon15));//gladio
        skillMap.Add(16, new Skill("Skill 16", 2, icon16));//lumen
        skillMap.Add(17, new Skill("Skill 17", 3, icon17));//turris
        skillMap.Add(18, new Skill("Skill 18", 5, icon18));//shield
        skillMap.Add(19, new Skill("Skill 19", 5, icon19));//flame
        skillMap.Add(20, new Skill("Skill 20", 3, icon20));//aura
        skillMap.Add(21, new Skill("Skill 21", 3, icon21));//heal

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

   public void AssignButtonUp()
{
    // Recupera la skill corrispondente all'id selezionato
    Skill selectedSkill = skillMap[selectedId];
    //PlayerWeaponManager.instance.SetWeapon(selectedId);

    if (selectedId > 0)
    {
        SkillUp_T.text = selectedSkill.value.ToString();
        SkillUp.sprite = selectedSkill.icon;
        idup = selectedId;
        UpdateMenuRapido.Instance.idup = selectedId;
         UpdateMenuRapido.Instance.SkillUp_T.text = selectedSkill.value.ToString();
         UpdateMenuRapido.Instance.Vup = selectedSkill.value;
         UpdateMenuRapido.Instance.SkillUp.sprite = selectedSkill.icon;
         MXVup = selectedSkill.value;
    }
}
  public void AssignButtonleft()
{
    // Recupera la skill corrispondente all'id selezionato
    Skill selectedSkill = skillMap[selectedId];
    //PlayerWeaponManager.instance.SetWeapon(selectedId);

    if (selectedId > 0)
    {
        SkillLeft_T.text = selectedSkill.value.ToString();
        SkillLeft.sprite = selectedSkill.icon;
        idleft = selectedId;
        UpdateMenuRapido.Instance.idleft = selectedId;
        UpdateMenuRapido.Instance.SkillLeft_T.text = selectedSkill.value.ToString();
        UpdateMenuRapido.Instance.Vleft = selectedSkill.value;
        UpdateMenuRapido.Instance.SkillLeft.sprite = selectedSkill.icon;
        MXVleft = selectedSkill.value;
        
    }
}  
public void AssignButtonright()
{
    // Recupera la skill corrispondente all'id selezionato
    Skill selectedSkill = skillMap[selectedId];
    //PlayerWeaponManager.instance.SetWeapon(selectedId);

    if (selectedId > 0)
    {
        SkillRight_T.text = selectedSkill.value.ToString();
        SkillRight.sprite = selectedSkill.icon;
        idright = selectedId;
        UpdateMenuRapido.Instance.idright = selectedId;
        UpdateMenuRapido.Instance.SkillRight_T.text = selectedSkill.value.ToString();
        UpdateMenuRapido.Instance.Vright = selectedSkill.value;
        UpdateMenuRapido.Instance.SkillRight.sprite = selectedSkill.icon;
        MXVright = selectedSkill.value;
    }
}  public void AssignButtonbottom()
{
    // Recupera la skill corrispondente all'id selezionato
    Skill selectedSkill = skillMap[selectedId];
   // PlayerWeaponManager.instance.SetWeapon(selectedId);

    if (selectedId > 0)
    {
        SkillBottom_T.text = selectedSkill.value.ToString();
        SkillBottom.sprite = selectedSkill.icon;
        idbottom = selectedId;
        UpdateMenuRapido.Instance.idbottom = selectedId;
        UpdateMenuRapido.Instance.SkillBottom_T.text = selectedSkill.value.ToString();
        UpdateMenuRapido.Instance.Vbottom = selectedSkill.value;
        UpdateMenuRapido.Instance.SkillBottom.sprite = selectedSkill.icon;
        MXVbottom = selectedSkill.value;
    }
}
}
