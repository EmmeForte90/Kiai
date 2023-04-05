using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class TitleQuest : MonoBehaviour
{

[HideInInspector]public int id;
public TextMeshProUGUI QuestDescriptionText;
public static TitleQuest Instance;

private void Awake()
{
    Instance = this;
}




}
