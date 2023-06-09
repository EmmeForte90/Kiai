﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;
using UnityEngine.UI;
using Cinemachine;
using TMPro;
using UnityEngine.EventSystems;


public class MainMenu : MonoBehaviour
{

    public GameObject mainmenu;
    public GameObject menu;
    //public GameObject Music;

    public string startScene;
    public float Timelife;
    public GameObject opzioni;
    public AudioMixer MSX;
    public AudioMixer SFX;
    private CinemachineVirtualCamera virtualCamera;
    private GameObject player; // Variabile per il player
    Resolution[] resolutions;
   
    public GameObject firstButton, secondButton, thirdButton,
        newGameMenuYes, options, AreTS;
    
 
    void Start()
    {
        AudioManager.instance.PlayMFX(0);
        resolutions = Screen.resolutions;

        List<string> options = new List<string>();
        int currentResolutionIndex = 0;
        for (int i = 0; i < resolutions.Length; i++)
        {
         string option = resolutions[i].width + "x" + resolutions[i].height;
        options.Add(option);

        if(resolutions[i].width == Screen.currentResolution.width && resolutions[i].height == Screen.currentResolution.height)
        {
            currentResolutionIndex = i;
        }

        }
        player = GameObject.FindWithTag("Player");
        virtualCamera = GameObject.FindWithTag("MainCamera").GetComponent<CinemachineVirtualCamera>(); //ottieni il riferimento alla virtual camera di Cinemachine
        EventSystem.current.SetSelectedGameObject(null); //necessary clear the event system
        EventSystem.current.SetSelectedGameObject(firstButton);
        
        Cursor.visible = true;
    }
/*
   public void openNewGame()
    {
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(newGameMenuYes);
    }

    public void openPause()
    {
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(options);
    }

    public void openAreYouSure()
    {
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(AreTS);
    }

    public void backToNewGame()
    {
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(firstButton);
    }

    public void backToOptions()
    {
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(secondButton);
    }

*/

public void SetResolution(int resolutionIndex)
{
    Resolution resolution = resolutions[resolutionIndex];

    Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
}


    public void StartGame()
    {
        StartCoroutine(fade());
    }
    

    public void Options()
    {
        opzioni.gameObject.SetActive(true);
    }
    public void ExitOptions()
    {
        opzioni.gameObject.SetActive(false);
    }

    public void SetVolume(float volume)
    {
        MSX.SetFloat("Volume", volume);

    }


     public void SetSFX(float volume)
    {
        SFX.SetFloat("Volume", volume);

    }

    public void SetQuality(int qualityIndex)
    {
        QualitySettings.SetQualityLevel(qualityIndex);
    }

    public void SetFullscreen(bool isFullScreen)
    {
        Screen.fullScreen = isFullScreen;
    }

IEnumerator fade()
    {
        GameplayManager.instance.FadeOut();
        yield return new WaitForSeconds(Timelife);
        AudioManager.instance.CrossFadeINAudio(0);
        SceneManager.LoadScene(startScene);       
    }


    IEnumerator fadeCont()
    {
        
        yield return new WaitForSeconds(Timelife);
       // SceneManager.LoadScene(PlayerPrefs.GetString("ContinueLevel"));   
    }
 

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Quitting Game");
    }

}
