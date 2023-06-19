using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//Usa l'input system di Unity scaricato dal packet manager


public class PlayerWeaponManager : MonoBehaviour
{
    [Header("Skill")]
    private int Normal = 0;
    private int Rock = 1;
    private int Fire = 2;
    private int Wind = 3;
    private int Water = 4;
    private int Void = 5;

    [Header("Items")]
    [SerializeField] private GameObject kunai;
    [SerializeField] private GameObject Shuriken;
    [SerializeField] private GameObject Bomb;
    [SerializeField] private GameObject Spille;
    [SerializeField] private GameObject freccia;
    [SerializeField] private GameObject Taneghasima;
    [SerializeField] private GameObject multishuriken;
    [SerializeField] private GameObject Gigashuriken;
    [SerializeField] private GameObject Sashimi;
    [SerializeField] private GameObject Onigiri;

    public static PlayerWeaponManager instance;

    private void Awake()
    { 
        instance = this;
    }

    public void SetStyle(int StyleID)
{
    switch (StyleID)
    {
    case 0:
    Move.instance.SetStylePrefab(Normal);
    break;
    
    case 1:
    Move.instance.SetStylePrefab(Rock);
    break;
    
    case 2:
    Move.instance.SetStylePrefab(Fire);
    break;

    case 3:
    Move.instance.SetStylePrefab(Wind);
    break;

    case 4:
    Move.instance.SetStylePrefab(Water);
    break;

    case 5:
    Move.instance.SetStylePrefab(Void);
    break; 
      
    }
    

 }
   
public void SetWeapon(int WeaponID)
{
    switch (WeaponID)
    {
    case 1:
    Move.instance.SetBulletPrefab(kunai);
    break;
    
    case 2:
    Move.instance.SetBulletPrefab(Shuriken);
    break;
    
    case 3:
    Move.instance.SetBulletPrefab(Bomb);
    break;

    case 4:
    Move.instance.SetBulletPrefab(Spille);
    break;

    case 5:
    Move.instance.SetBulletPrefab(freccia);
    break;

    case 6:
    Move.instance.SetBulletPrefab(Taneghasima);
    break; 

    case 7:
    Move.instance.SetBulletPrefab(Taneghasima);
    break;

    case 8:
    Move.instance.SetBulletPrefab(Taneghasima);
    break;

    case 9:
    Move.instance.SetBulletPrefab(Taneghasima);
    break;

    case 10:
    Move.instance.SetBulletPrefab(Taneghasima);
    break; 

    case 11:
    Move.instance.SetBulletPrefab(Taneghasima);
    break;
    
    case 12:
    Move.instance.SetBulletPrefab(Taneghasima);
    break;

    case 13:
    Move.instance.SetBulletPrefab(Taneghasima);
    break;

    case 14:
    Move.instance.SetBulletPrefab(Onigiri);
    break;

    case 15:
    Move.instance.SetBulletPrefab(Sashimi);
    break; 
    }
    

 }

}

