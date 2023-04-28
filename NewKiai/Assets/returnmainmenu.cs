using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;
using UnityEngine.UI;
using Cinemachine;
using TMPro;
using UnityEngine.EventSystems;

public class returnmainmenu : MonoBehaviour
{
    public string sceneName;
    public float Timelife;
    // Riferimento all'evento di cambio scena
    public SceneEvent sceneEvent;
    private GameObject player;
    private CinemachineVirtualCamera vCam;

    public void Start()
    {
        GameplayManager.instance.StopPlay();
    }

     public void mainmenu()
    {
        StartCoroutine(fade());
    }
    
    IEnumerator fade()
    {
        GameplayManager.instance.FadeOut();
        Move.instance.stopInput = true;
        Move.instance.Stop();
        yield return new WaitForSeconds(Timelife);
        // Invochiamo l'evento di cambio scena
        ChangeScene();
    }
// Metodo per cambiare scena
private void ChangeScene()
{
    SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
    SceneManager.sceneLoaded += OnSceneLoaded;
}

// Metodo eseguito quando la scena è stata caricata
private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
{
    GameplayManager.instance.FadeIn();
    SceneManager.sceneLoaded -= OnSceneLoaded;
    if (player != null)
    {
        Move.instance.stopInput = false;
        vCam = GameObject.FindWithTag("MainCamera").GetComponent<CinemachineVirtualCamera>(); //ottieni il riferimento alla virtual camera di Cinemachine
        vCam.Follow = player.transform;
       
    }
    GameplayManager.instance.StopFade();    
}
}
