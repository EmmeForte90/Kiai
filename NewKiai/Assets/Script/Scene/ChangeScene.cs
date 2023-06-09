using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Audio;

public class ChangeScene : MonoBehaviour
{
    public string startScene;
    public string levelToLoad;
    public float Timelife;
    public string spawnPointTag = "SpawnPoint";
    private GameObject player;



void Awake()
{        
    GameplayManager.instance.FadeIn();
    GameplayManager.instance.FirstoOfPlay();
    StartCoroutine(FinishVideo());
}


// Metodo per cambiare scena
private void changeScene()
{
    SceneManager.LoadScene(startScene, LoadSceneMode.Single);
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
        Move.instance.Stop();
        // Troviamo il game object del punto di spawn
        GameObject spawnPoint = GameObject.FindWithTag(spawnPointTag);
        if (spawnPoint != null)
        {
            GameplayManager.instance.FirstoOfPlay();
            // Muoviamo il player al punto di spawn
            player.transform.position = spawnPoint.transform.position;
            //yield return new WaitForSeconds(3f);
        }
    }
    GameplayManager.instance.StopFade();    
}

    IEnumerator FinishVideo()
    {
        player = GameObject.FindWithTag("Player");
        yield return new WaitForSeconds(Timelife);
        GameplayManager.instance.spawnPointTag = spawnPointTag;
        SceneManager.LoadScene(startScene, LoadSceneMode.Single);
        SceneManager.sceneLoaded += OnSceneLoaded;
        //SceneManager.LoadScene(startScene);
    }
}
