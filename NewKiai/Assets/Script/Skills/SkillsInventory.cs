using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class SkillsInventory : MonoBehaviour
{        
   //public Skill Skill;

   public static SkillsInventory Instance;
   // public TextMeshProUGUI SkillDescriptionText;
   // public Image imageSkill;
   //public List<Skill> abil = new List<Skill>();

public bool IsUpper = false;
[SerializeField] GameObject  UpperSkill;
public bool IsSwordRain = false;
[SerializeField] GameObject  SwordRainSkill;
public bool IsTornado = false;
[SerializeField] GameObject  TornadoSkill;
public bool IsSpinner = false;
[SerializeField] GameObject  SpinnerSkill;
public bool IsDash = false;
[SerializeField] GameObject  DashSkill;
public bool IsMultilunge = false;
[SerializeField] GameObject  MultilungeSkill;
public bool IsSlash = false;
[SerializeField] GameObject  SlashSkill;

////////////////////////////////////////
public bool IsPenetratig = false;
[SerializeField] GameObject  PenetratigSkill;
public bool IsGlobe = false;
[SerializeField] GameObject  GlobeSkill;
public bool IsShotgun = false;
[SerializeField] GameObject  ShotgunSkill;
public bool IsSawDash = false;
[SerializeField] GameObject  SawDashSkill;
public bool IsWall = false;
[SerializeField] GameObject  WallSkill;
public bool IsBomb = false;
[SerializeField] GameObject  BombSkill;
public bool IsBoomerang = false;
[SerializeField] GameObject  BoomerangSkill;

////////////////////////////////////////
public bool IsGladio = false;
[SerializeField] GameObject  GladioSkill;
public bool IsLumen = false;
[SerializeField] GameObject  LumenSkill;
public bool IsTurris = false;
[SerializeField] GameObject  TurrisSkill;
public bool IsShield = false;
[SerializeField] GameObject  ShieldSkill;
public bool IsFlame = false;
[SerializeField] GameObject  FlameSkill;
public bool IsAura = false;
[SerializeField] GameObject  AuraSkill;
public bool IsHeal = false;
[SerializeField] GameObject  HealSkill;

private void Awake()
   {
    Instance = this;   
    
   }

private void Update()
{
   if(IsUpper)
   {
      UpperSkill.gameObject.SetActive(true);
   }
if(IsSwordRain)
   {
      SwordRainSkill.gameObject.SetActive(true);
   }

if(IsTornado)
   {
      TornadoSkill.gameObject.SetActive(true);
   }
if(IsSpinner)
   {
      SpinnerSkill.gameObject.SetActive(true);
   }
if(IsDash)
   {
      DashSkill.gameObject.SetActive(true);
   }

if(IsMultilunge)
   {
      MultilungeSkill.gameObject.SetActive(true);
   }

if(IsSlash)
   {
      SlashSkill.gameObject.SetActive(true);
   }

///////////////////////////////////////////

if(IsPenetratig)
   {
      PenetratigSkill.gameObject.SetActive(true);
   }
if(IsGlobe)
   {
      GlobeSkill.gameObject.SetActive(true);
   }

if(IsShotgun)
   {
      ShotgunSkill.gameObject.SetActive(true);
   }
if(IsSawDash)
   {
      SawDashSkill.gameObject.SetActive(true);
   }
if(IsDash)
   {
      DashSkill.gameObject.SetActive(true);
   }

if(IsWall)
   {
      WallSkill.gameObject.SetActive(true);
   }

if(IsBomb)
   {
      BombSkill.gameObject.SetActive(true);
   }

if(IsBoomerang)
   {
      BoomerangSkill.gameObject.SetActive(true);
   }

/////////////////////////////////////////////////

if(IsGladio)
   {
      GladioSkill.gameObject.SetActive(true);
   }
if(IsLumen)
   {
      LumenSkill.gameObject.SetActive(true);
   }

if(IsTurris)
   {
      TurrisSkill.gameObject.SetActive(true);
   }
if(IsAura)
   {
      AuraSkill.gameObject.SetActive(true);
   }
if(IsFlame)
   {
      FlameSkill.gameObject.SetActive(true);
   }

if(IsHeal)
   {
      HealSkill.gameObject.SetActive(true);
   }

if(IsShield)
   {
      ShieldSkill.gameObject.SetActive(true);
   }


/*

    public static SkillsInventory Instance;
   public List<Skill> abil = new List<Skill>();

   public Transform SkillContent;
   public GameObject InventorySkill;

   private void Awake()
   {
    Instance = this;
   }

   public void Add(Skill ab)
   {
    abil.Add(ab);
   }

   public void Remove(Skill ab)
   {
     abil.Remove(ab);
   }

   public void Listabil()
   {
      foreach (Transform child in SkillContent)
      {
         Destroy(child.gameObject);
      }

      foreach (var ab in abil)
      {
         GameObject obj = Instantiate(InventorySkill, SkillContent);
         var abIcon = obj.transform.Find("skill_icon").GetComponent<Image>();

         abIcon.sprite = ab.icon;
      }
   }*/
}
}



