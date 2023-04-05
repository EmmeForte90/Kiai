using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//Usa l'input system di Unity scaricato dal packet manager


public class PlayerWeaponManager : MonoBehaviour
{
   [Header("Skill")]
    [SerializeField] private GameObject Upper;
    [SerializeField] private GameObject Swordrain;
    [SerializeField] private GameObject Tornado;
    [SerializeField] private GameObject Spinner;
    [SerializeField] private GameObject DashLunge;
    [SerializeField] private GameObject Multilunge;
    [SerializeField] private GameObject Slash;
    [SerializeField] private GameObject PenetratingSlash;
    [SerializeField] private GameObject Globo;
    [SerializeField] private GameObject Shotgun;
    [SerializeField] private GameObject Dashsaw;
    [SerializeField] private GameObject Wall;
    [SerializeField] private GameObject Bomb;
    [SerializeField] private GameObject Boomerang;
    [SerializeField] private GameObject Gladio;
    [SerializeField] private GameObject Lumen;
    [SerializeField] private GameObject Turris;
    [SerializeField] private GameObject Shield;
    [SerializeField] private GameObject Flame;
    [SerializeField] private GameObject Aura;
    [SerializeField] private GameObject Heal;

    public static PlayerWeaponManager instance;

    private void Awake()
    { 
        instance = this;
    }

    public void SetWeapon(int WeaponID)
{
    switch (WeaponID)
    {
    case 1:
    Move.instance.SetBulletPrefab(Upper);
    break;
    
    case 2:
    Move.instance.SetBulletPrefab(Swordrain);
    break;

    case 3:
    Move.instance.SetBulletPrefab(Tornado);
    break;

    case 4:
    Move.instance.SetBulletPrefab(Spinner);
    break;

    case 5:
    Move.instance.SetBulletPrefab(DashLunge);
    break;
        
    case 6:
    Move.instance.SetBulletPrefab(Multilunge);
    break;

    case 7:
    Move.instance.SetBulletPrefab(Slash);
    break;

    case 8:
    Move.instance.SetBulletPrefab(PenetratingSlash);
    break;

    case 9:
    Move.instance.SetBulletPrefab(Globo);
    break;

    case 10:
    Move.instance.SetBulletPrefab(Shotgun);
    break;

    case 11:
    Move.instance.SetBulletPrefab(Dashsaw);
    break;

    case 12:
    Move.instance.SetBulletPrefab(Wall);
    break;

    case 13:
    Move.instance.SetBulletPrefab(Bomb);
    break;

    case 14:
    Move.instance.SetBulletPrefab(Boomerang);
    break;

    case 15:
    Move.instance.SetBulletPrefab(Gladio);
    break;

    case 16:
    Move.instance.SetBulletPrefab(Lumen);
    break;

    case 17:
    Move.instance.SetBulletPrefab(Turris);
    break;

    case 18:
    Move.instance.SetBulletPrefab(Shield);
    break;

    case 19:
    Move.instance.SetBulletPrefab(Flame);
    break;

    case 20:
    Move.instance.SetBulletPrefab(Aura);
    break;

    case 21:
    Move.instance.SetBulletPrefab(Heal);
    break;
     
    }
    

 }
   


}

