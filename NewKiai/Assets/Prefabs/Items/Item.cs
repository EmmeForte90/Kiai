using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CreateAssetMenu(fileName ="New Item", menuName = "Item/Create New Item")]

public class Item : ScriptableObject
{
public int id;
public string itemName;
[SerializeField][TextArea(3, 10)]
public string Description;
public int value;
public int prices;
public Sprite icon;
[SerializeField][TextArea(3, 10)]
public string Dialogue;
public bool isConsumable = false;
public bool isKatana = false;
public bool isDress = false;

  public Item(string name, int value, Sprite icon)
    {
        this.name = name;
        this.value = value;
        this.icon = icon;
    }

  #if UNITY_EDITOR
private void OnDisable()
{
// reset dei bool quando la modalità Play di Unity viene terminata
if (!EditorApplication.isPlaying)
{
value = 1;
}
}
#endif
}


