using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class MainMenuCam : MonoBehaviour
{
    public GameObject mainmenu;
    public GameObject menu;
    public GameObject SecondMenu; // Variabile per il SecondMenu
    private CinemachineVirtualCamera virtualCamera;

    void Start()
    {        
        virtualCamera = GameObject.FindWithTag("MainCamera").GetComponent<CinemachineVirtualCamera>(); //ottieni il riferimento alla virtual camera di Cinemachine
        AudioManager.instance.CrossFadeINAudio(0);
    }

public void ChooseCharacter()
    {
        virtualCamera.Follow = SecondMenu.transform;
        //virtualCamera.LookAt = SecondMenu.transform;
        mainmenu.gameObject.SetActive(false);

    }
public void notchoose()
    {
        virtualCamera.Follow = menu.transform;
        //virtualCamera.LookAt = SecondMenu.transform;
        mainmenu.gameObject.SetActive(true);

    }
    
}
