using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class descriptionData : MonoBehaviour
{
   public Skill Skill;
   public int DataId;
   private int id;
   public static descriptionData Instance;
   public TextMeshProUGUI SkillDescriptionText;
   public TextMeshProUGUI SkillNameText;

   public Image icon;

    private void Awake()
   {
    Instance = this;   
    id = Skill.id;
   }

public void DescriptionSkill()
    { 
        // Inserisci qui il codice che vuoi eseguire quando l'immagine viene cliccata
        SkillDescriptionText.text = Skill.Description;
        SkillNameText.text = Skill.SkillName;
        id = Skill.id;
         // Imposta l'icona dell'abilità
        icon.sprite = Skill.icon;
         
    }

}
