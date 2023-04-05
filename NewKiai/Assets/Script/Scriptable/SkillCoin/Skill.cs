using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName ="New Skill", menuName = "Skill/Create New Skill")]

public class Skill : ScriptableObject
{
public int id;
public string SkillName;
public string Description;
public int value;
public Sprite icon;
//public gameObject bullet;


   public Skill(string name, int value, Sprite icon)
    {
        this.name = name;
        this.value = value;
        this.icon = icon;
    }
}

