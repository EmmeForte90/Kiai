using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CreateAssetMenu(fileName ="New Dialogue", menuName = "Dialogue/Create New Dialogue")]
public class Dialogues : ScriptableObject
{
public int id;
public string CharacterName;
[SerializeField][TextArea(3, 10)]public string[] dialogue;

//////////////////////////////////
[SerializeField][TextArea(3, 10)]
public string[] Startdialogue; // array of string to store the dialogues
[SerializeField][TextArea(3, 10)]
public string[] Middledialogue; // array of string to store the dialogues
[SerializeField][TextArea(3, 10)]
public string[] Endingdialogue; // array of string to store the dialogues

////////////////////////////////////////////

//public int value;

public bool First;

public bool Middle;

public bool End;

#if UNITY_EDITOR
private void OnDisable()
{
// reset dei bool quando la modalità Play di Unity viene terminata
if (!EditorApplication.isPlaying)
{
First = false;
Middle = false;
End = false;
}
}
#endif
}
