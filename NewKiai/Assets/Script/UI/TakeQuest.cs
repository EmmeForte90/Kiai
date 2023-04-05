using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TakeQuest : MonoBehaviour
{
   [HideInInspector] public Quests quests;
  [HideInInspector] public int id;
   public TextMeshProUGUI QuestDescriptionText;
    public Image icon;


public static TakeQuest Instance;

    private void Awake()
   {
    Instance = this;   
   }
}
