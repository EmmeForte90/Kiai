using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillInventoryHUD : MonoBehaviour
{

public static SkillInventoryHUD Instance;
  
public bool IsUpper = false;
[SerializeField] GameObject  UpperSkill;
[SerializeField] GameObject  UpperSkillM;

public bool IsSwordRain = false;
[SerializeField] GameObject  SwordRainSkill;
[SerializeField] GameObject  SwordRainSkillM;

public bool IsTornado = false;
[SerializeField] GameObject  TornadoSkill;
[SerializeField] GameObject  TornadoSkillM;

public bool IsSpinner = false;
[SerializeField] GameObject  SpinnerSkill;
[SerializeField] GameObject  SpinnerSkillM;

public bool IsDash = false;
[SerializeField] GameObject  DashSkill;
[SerializeField] GameObject  DashSkillM;

public bool IsMultilunge = false;
[SerializeField] GameObject  MultilungeSkill;
[SerializeField] GameObject  MultilungeSkillM;

public bool IsSlash = false;
[SerializeField] GameObject  SlashSkill;
[SerializeField] GameObject  SlashSkillM;


////////////////////////////////////////
public bool IsPenetratig = false;
[SerializeField] GameObject  PenetratigSkill;
[SerializeField] GameObject  PenetratigSkillM;

public bool IsGlobe = false;
[SerializeField] GameObject  GlobeSkill;
[SerializeField] GameObject  GlobeSkillM;

public bool IsShotgun = false;
[SerializeField] GameObject  ShotgunSkill;
[SerializeField] GameObject  ShotgunSkillM;

public bool IsSawDash = false;
[SerializeField] GameObject  SawDashSkill;
[SerializeField] GameObject  SawDashSkillM;

public bool IsWall = false;
[SerializeField] GameObject  WallSkill;
[SerializeField] GameObject  WallSkillM;

public bool IsBomb = false;
[SerializeField] GameObject  BombSkill;
[SerializeField] GameObject  BombSkillM;

public bool IsBoomerang = false;
[SerializeField] GameObject  BoomerangSkill;
[SerializeField] GameObject  BoomerangSkillM;


////////////////////////////////////////
public bool IsGladio = false;
[SerializeField] GameObject  GladioSkill;
[SerializeField] GameObject  GladioSkillM;

public bool IsLumen = false;
[SerializeField] GameObject  LumenSkill;
[SerializeField] GameObject  LumenSkillM;

public bool IsTurris = false;
[SerializeField] GameObject  TurrisSkill;
[SerializeField] GameObject  TurrisSkillM;

public bool IsShield = false;
[SerializeField] GameObject  ShieldSkill;
[SerializeField] GameObject  ShieldSkillM;

public bool IsFlame = false;
[SerializeField] GameObject  FlameSkill;
[SerializeField] GameObject  FlameSkillM;

public bool IsAura = false;
[SerializeField] GameObject  AuraSkill;
[SerializeField] GameObject  AuraSkillM;

public bool IsHeal = false;
[SerializeField] GameObject  HealSkill;
[SerializeField] GameObject  HealSkillM;


private void Awake()
   {
    Instance = this;   
    
   }

private void Update()
{
   if(IsUpper)
   {
      UpperSkill.gameObject.SetActive(true);
            UpperSkillM.gameObject.SetActive(true);

   }
if(IsSwordRain)
   {
      SwordRainSkill.gameObject.SetActive(true);
            SwordRainSkillM.gameObject.SetActive(true);

   }

if(IsTornado)
   {
      TornadoSkill.gameObject.SetActive(true);
            TornadoSkillM.gameObject.SetActive(true);

   }
if(IsSpinner)
   {
      SpinnerSkill.gameObject.SetActive(true);
            SpinnerSkillM.gameObject.SetActive(true);

   }
if(IsDash)
   {
      DashSkill.gameObject.SetActive(true);
            DashSkillM.gameObject.SetActive(true);

   }

if(IsMultilunge)
   {
      MultilungeSkill.gameObject.SetActive(true);
            MultilungeSkillM.gameObject.SetActive(true);

   }

if(IsSlash)
   {
      SlashSkill.gameObject.SetActive(true);
            SlashSkillM.gameObject.SetActive(true);

   }

///////////////////////////////////////////

if(IsPenetratig)
   {
      PenetratigSkill.gameObject.SetActive(true);
            PenetratigSkillM.gameObject.SetActive(true);

   }
if(IsGlobe)
   {
      GlobeSkill.gameObject.SetActive(true);
            GlobeSkillM.gameObject.SetActive(true);

   }

if(IsShotgun)
   {
      ShotgunSkill.gameObject.SetActive(true);
            ShotgunSkillM.gameObject.SetActive(true);

   }
if(IsSawDash)
   {
      SawDashSkill.gameObject.SetActive(true);
            SawDashSkillM.gameObject.SetActive(true);

   }
if(IsDash)
   {
      DashSkill.gameObject.SetActive(true);
            DashSkillM.gameObject.SetActive(true);

   }

if(IsWall)
   {
      WallSkill.gameObject.SetActive(true);
            WallSkillM.gameObject.SetActive(true);

   }

if(IsBomb)
   {
      BombSkill.gameObject.SetActive(true);
            BombSkillM.gameObject.SetActive(true);

   }

if(IsBoomerang)
   {
      BoomerangSkill.gameObject.SetActive(true);
            BoomerangSkillM.gameObject.SetActive(true);

   }

/////////////////////////////////////////////////

if(IsGladio)
   {
      GladioSkill.gameObject.SetActive(true);
            GladioSkillM.gameObject.SetActive(true);

   }
if(IsLumen)
   {
      LumenSkill.gameObject.SetActive(true);
            LumenSkillM.gameObject.SetActive(true);

   }

if(IsTurris)
   {
      TurrisSkill.gameObject.SetActive(true);
            TurrisSkillM.gameObject.SetActive(true);

   }
if(IsAura)
   {
      AuraSkill.gameObject.SetActive(true);
            AuraSkillM.gameObject.SetActive(true);

   }
if(IsFlame)
   {
      FlameSkill.gameObject.SetActive(true);
            FlameSkillM.gameObject.SetActive(true);

   }

if(IsHeal)
   {
      HealSkill.gameObject.SetActive(true);
            HealSkillM.gameObject.SetActive(true);

   }

if(IsShield)
   {
      ShieldSkill.gameObject.SetActive(true);
            ShieldSkillM.gameObject.SetActive(true);

   }





}

}



