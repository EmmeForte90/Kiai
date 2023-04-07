using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class AssignItem : MonoBehaviour
{
    //private int IDItem;
    [HideInInspector]
    public int selectedId = -1; // Id dell'abilità selezionata  
    
public static AssignItem Instance;

public void Awake()
    {
   // IDItem = Item.id;
    if (Instance == null)
        {
            Instance = this;
        }
    }

 public void AssignId(int id)
    {
        selectedId = id; // Assegna l'id dell'abilità selezionata
    }

}
