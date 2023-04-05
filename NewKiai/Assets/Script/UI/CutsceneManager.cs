using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CutsceneManager : MonoBehaviour
{
public bool[] filmati;
private GameObject[] timelineObjects;
// Singleton pattern per accedere a CutsceneManager da altre classi
public static CutsceneManager Instance;

private void Awake()
{
    Instance = this;
}  

private void Start()
{
    // Cerca tutti i GameObjects con il tag "Timeline" all'inizio dello script
    timelineObjects = GameObject.FindGameObjectsWithTag("Timeline");
}

public void TimelineStart(int id)
{
    filmati[id] = true;  
    GameplayManager.instance.DeactivationGame();
 
}

public void TimelineEnd(int id)
{
    filmati[id] = false;   
    GameplayManager.instance.ActivationGame();
}

private void OnEnable()
{
    SceneManager.sceneLoaded += OnSceneLoaded;
}

private void OnDisable()
{
    SceneManager.sceneLoaded -= OnSceneLoaded;
}

private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
{
    // Cerca tutti i GameObjects con il tag "Timeline" nella nuova scena
    timelineObjects = GameObject.FindGameObjectsWithTag("Timeline");

    // Disattiva i GameObjects in base allo stato dei bool
    foreach (GameObject timelineObject in timelineObjects)
    {
        int id = timelineObject.GetComponent<TimelineController>().ID;
        timelineObject.SetActive(filmati[id]);
    }
}
}


