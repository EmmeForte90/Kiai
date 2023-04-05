using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelChanger : MonoBehaviour
{
   // Variabili per memorizzare la scena attuale e la posizione del player
public string spawnPointTag = "SpawnPoint";
public GameObject button;
public bool interactWithKey = true;
//public KeyCode changeSceneKey = "Talk";
public string sceneName;
public bool needButton;
public bool isDoor = false;

// Riferimento all'evento di cambio scena
private SceneEvent sceneEvent;
// Riferimento al game object del player
private GameObject player;
[Header("Audio")]
[SerializeField] AudioSource Door;

private void Start()
{
    // Inizialmente nascondiamo il testo del dialogo
    button.gameObject.SetActive(false); 
    // Recuperiamo il riferimento allo script dell'evento di cambio scena
    sceneEvent = GetComponent<SceneEvent>();
    // Aggiungiamo un listener all'evento di cambio scena
    sceneEvent.onSceneChange.AddListener(ChangeScene);
   
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

        // Troviamo il game object del punto di spawn
        GameObject spawnPoint = GameObject.FindWithTag(spawnPointTag);
        if (spawnPoint != null)
        {
            // Muoviamo il player al punto di spawn
            player.transform.position = spawnPoint.transform.position;
            //yield return new WaitForSeconds(3f);
        }
    }
    GameplayManager.instance.StopFade();    
}

// Coroutine per attendere il caricamento della scena
IEnumerator WaitForSceneLoad()
{   
    GameplayManager.instance.FadeOut();
    Move.instance.stopInput = true;
    Move.instance.Stop();
    yield return new WaitForSeconds(2f);
    // Invochiamo l'evento di cambio scena
    sceneEvent.InvokeOnSceneChange();
    
}

// Metodo eseguito quando il player entra nel trigger
private void OnTriggerStay2D(Collider2D other)
{
    // Controlliamo se il player ha toccato il collider
    if (other.CompareTag("Player"))
    {
         // Troviamo il game object del player
        player = GameObject.FindGameObjectWithTag("Player");
        GameplayManager.instance.startGame = false;
        // Mostriamo il testo del dialogo se necessario
        if(needButton)
        {
            button.gameObject.SetActive(true); 
        }
        // Verifichiamo se l'interazione avviene tramite il tasto "Talk"
        if (interactWithKey && Input.GetButton("Talk"))
        {    
            Move.instance.Stooping();
            // Riproduciamo l'audio della porta se necessario
            if(isDoor)
            {
                Door.Play();
            }
            // Avviamo la coroutine per attendere il caricamento della scena
            StartCoroutine(WaitForSceneLoad());
        }  
    }
}


private void OnTriggerEnter2D(Collider2D other)
{
    // Controlliamo se il player ha toccato il collider
    if (other.CompareTag("Player"))
    {
        Move.instance.Stooping();
         // Troviamo il game object del player
        player = GameObject.FindGameObjectWithTag("Player");
        if(needButton)
        {
            button.gameObject.SetActive(true); // Initially hide the dialogue text
        }

         if (!interactWithKey)
        {
        StartCoroutine(WaitForSceneLoad());
        }
       
}
}

private void OnTriggerExit2D(Collider2D other)
{
    // Controlliamo se il player ha toccato il collider
    if (other.CompareTag("Player"))
    { 
        Move.instance.Stooping();
        // Troviamo il game object del player
         player = GameObject.FindGameObjectWithTag("Player");
        if(needButton)
        {
            button.gameObject.SetActive(false); // Initially hide the dialogue text
        }

        
       
}
}
}