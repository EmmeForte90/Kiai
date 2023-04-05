using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


[CreateAssetMenu(fileName ="New Filmato", menuName = "Filmato/Create New Filmato")]
public class Filmato : ScriptableObject
{

public int id;
public bool isActive;
public bool isComplete;

}
