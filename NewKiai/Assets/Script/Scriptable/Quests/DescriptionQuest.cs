using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;


public class DescriptionQuest : MonoBehaviour
{    
private Image previewImages;
private TextMeshProUGUI descriptions;
public static DescriptionQuest Instance;
public int id;
private List<Quests> questDatabase;

  // Metodo per aggiungere una nuova quest al database
    public void AddQuest(Quests newQuest)
{
    questDatabase.Add(newQuest);
    id = newQuest.id;
}
void Awake()
{    
    Instance = this;
    previewImages = QuestManager.Instance.previewImages;
    descriptions = QuestManager.Instance.descriptions;
}


    public void UpdateQuestInfo()
    {
        if (id >= 0 && id < questDatabase.Count)
        {
            // Assegna i valori desiderati ai componenti dell'immagine di preview e della descrizione
            previewImages.sprite = questDatabase[id].Bigicon;
            descriptions.text = questDatabase[id].Description;
        }
    }
 
}
