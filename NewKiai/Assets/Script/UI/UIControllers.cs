using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UIControllers : MonoBehaviour
{

    public GameObject Button;
   
    public static UIControllers instance;

private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }
   public void SetSelectedGameObjectToSettings()
    {
        //Clear
        EventSystem.current.SetSelectedGameObject(null);
        //Reassign
        EventSystem.current.SetSelectedGameObject(Button);
    }
   
}
