using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


[CreateAssetMenu(fileName ="New Quest", menuName = "Quest/Create New Quest")]
public class Quests : ScriptableObject
{
public int id;
public string CharacterName;
public string questName;
public string Description;
public int value;
//public Sprite icon;
public Sprite Bigicon;
[SerializeField][TextArea(3, 10)]
public string[] Startdialogue; // array of string to store the dialogues
[SerializeField][TextArea(3, 10)]
public string[] Middledialogue; // array of string to store the dialogues
[SerializeField][TextArea(3, 10)]
public string[] Endingdialogue; // array of string to store the dialogues
[SerializeField][TextArea(3, 10)]
public string[] Afterdialogue; // array of string to store the dialogues

public bool isActive;

public bool isComplete;

public bool AfterQuest;

#if UNITY_EDITOR
private void OnDisable()
{
// reset dei bool quando la modalità Play di Unity viene terminata
if (!EditorApplication.isPlaying)
{
isActive = false;
isComplete = false;
AfterQuest = false;
}
}
#endif
}

