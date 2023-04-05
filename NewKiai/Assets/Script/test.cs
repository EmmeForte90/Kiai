using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Audio;

public class test : MonoBehaviour
{     
    [Header("Skill")]
    public Skill Skill;

     [Header("Audio")]
    public float basePitch = 1f;
    public float randomPitchOffset = 0.1f;
    [SerializeField] AudioSource take;
    
    [Header("VFX")]
    [SerializeField] GameObject VFX;
    //public TextMeshProUGUI itemDescriptionText;
public bool IsUpper = false;
public bool IsSwordRain = false;
public bool IsTornado = false;
public bool IsSpinner = false;
public bool IsDash = false;
public bool IsMultilunge = false;
public bool IsSlash = false;

////////////////////////////////////////
public bool IsPenetratig = false;
public bool IsGlobe = false;
public bool IsShotgun = false;
public bool IsSawDash = false;
public bool IsWall = false;
public bool IsBomb = false;
public bool IsBoomerang = false;

////////////////////////////////////////
public bool IsGladio = false;
public bool IsLumen = false;
public bool IsTurris = false;
public bool IsShield = false;
public bool IsFlame = false;
public bool IsAura = false;
public bool IsHeal = false;


    public void Pickup()
    {
        if(IsUpper)
        {
        SkillInventoryHUD.Instance.IsUpper = true;}
        else if(IsSwordRain)
        {
        SkillInventoryHUD.Instance.IsSwordRain = true;}
        else if(IsTornado)
        {
        SkillInventoryHUD.Instance.IsTornado = true;}
        else if(IsSpinner)
        {
        SkillInventoryHUD.Instance.IsSpinner = true;}
        else if(IsDash)
        {
        SkillInventoryHUD.Instance.IsDash = true;}
        else if(IsMultilunge)
        {
        SkillInventoryHUD.Instance.IsMultilunge = true;}
        else if(IsSlash)
        {
        SkillInventoryHUD.Instance.IsSlash = true;}

        else if(IsPenetratig)
        {
        SkillInventoryHUD.Instance.IsPenetratig = true;}
        else if(IsGlobe)
        {//SkillsInventory.Instance.IsGlobe = true;
        SkillInventoryHUD.Instance.IsGlobe = true;}
        else if(IsShotgun)
        {
        SkillInventoryHUD.Instance.IsShotgun = true;}
        else if(IsSawDash)
        {
        SkillInventoryHUD.Instance.IsSawDash = true;}
        else if(IsWall)
        {
        SkillInventoryHUD.Instance.IsWall = true;}
        else if(IsBomb)
        {
        SkillInventoryHUD.Instance.IsBomb = true;}
        else if(IsBoomerang)
        {
        SkillInventoryHUD.Instance.IsBoomerang = true;}

         else if(IsGladio)
        {
        SkillInventoryHUD.Instance.IsGladio = true;}
        else if(IsLumen)
        {
        SkillInventoryHUD.Instance.IsLumen = true;}
        else if(IsTurris)
        {
        SkillInventoryHUD.Instance.IsTurris = true;}
        else if(IsShield)
        {
        SkillInventoryHUD.Instance.IsShield = true;}
        else if(IsAura)
        {
        SkillInventoryHUD.Instance.IsAura = true;}
        else if(IsFlame)
        {
        SkillInventoryHUD.Instance.IsFlame = true;}
        else if(IsHeal)
        {
        SkillInventoryHUD.Instance.IsHeal = true;}
        
 
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            
        Pickup();
        Instantiate(VFX, transform.position, transform.rotation);
       // take.Play();

        }
    }
}

