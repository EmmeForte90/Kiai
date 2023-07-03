using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Audio;
using Cinemachine;

public class LevelChanger : MonoBehaviour
{
   // Variabili per memorizzare la scena attuale e la posizione del player
public string spawnPointTag = "SpawnPoint";
public GameObject button;
private CinemachineVirtualCamera vCam;
public bool camFollowPlayer = true;
public int Timelife = 5;
public bool interactWithKey = true;
//public KeyCode changeSceneKey = "Talk";
public string sceneName;
public bool needButton;
public bool isDoor = false;

public bool isTimer = false;

public Animator anim; // componente Animator del personaggio
// Riferimento all'evento di cambio scena
private SceneEvent sceneEvent;
// Riferimento al game object del player
private GameObject player;
 [Header("Audio")]
    [HideInInspector] public float basePitch = 1f;
    [HideInInspector] public float randomPitchOffset = 0.1f;
    [SerializeField] public AudioClip[] listmusic; // array di AudioClip contenente tutti i suoni che si vogliono riprodurre
    private AudioSource[] bgm; // array di AudioSource che conterrà gli oggetti AudioSource creati
    public AudioMixer SFX;
    private bool bgmActive = false;

private void Start()
{
    // Inizialmente nascondiamo il testo del dialogo
    button.gameObject.SetActive(false); 
    // Recuperiamo il riferimento allo script dell'evento di cambio scena
    sceneEvent = GetComponent<SceneEvent>();
    // Aggiungiamo un listener all'evento di cambio scena
    sceneEvent.onSceneChange.AddListener(ChangeScene);
   bgm = new AudioSource[listmusic.Length]; // inizializza l'array di AudioSource con la stessa lunghezza dell'array di AudioClip
        for (int i = 0; i < listmusic.Length; i++) // scorre la lista di AudioClip
        {
        bgm[i] = gameObject.AddComponent<AudioSource>(); // crea un nuovo AudioSource come componente del game object attuale (quello a cui è attaccato lo script)
        bgm[i].clip = listmusic[i]; // assegna l'AudioClip corrispondente all'AudioSource creato
        bgm[i].playOnAwake = false; // imposto il flag playOnAwake a false per evitare che il suono venga riprodotto automaticamente all'avvio del gioco
        bgm[i].loop = false; // imposto il flag playOnAwake a false per evitare che il suono venga riprodotto automaticamente all'avvio del gioco

        }

        // Aggiunge i canali audio degli AudioSource all'output del mixer
        foreach (AudioSource audioSource in bgm)
        {
        audioSource.outputAudioMixerGroup = SFX.FindMatchingGroups("Master")[0];
        }
        
        if(isTimer){
         // Troviamo il game object del player
        GameplayManager.instance.startGame = false;
        GameplayManager.instance.isStartGame = false;
        GameplayManager.instance.spawnPointTag = spawnPointTag;
        StartCoroutine(loading());
        }  

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
        player = GameObject.FindGameObjectWithTag("Player");
        player.gameObject.SetActive(true);
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
IEnumerator loading()
{   
    GameplayManager.instance.FadeOut();
    yield return new WaitForSeconds(Timelife);
    // Invochiamo l'evento di cambio scena
    sceneEvent.InvokeOnSceneChange();
    
}
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
        GameplayManager.instance.startGame = false;
        GameplayManager.instance.isStartGame = false;
        GameplayManager.instance.spawnPointTag = spawnPointTag;
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
                PlayMFX(0);
                anim.SetBool("talk", true);
            }
            // Avviamo la coroutine per attendere il caricamento della scena
            StartCoroutine(WaitForSceneLoad());
        }  
    }
}
 public void StopMFX(int soundToPlay)
    {
        if (bgmActive)
        {
            bgm[soundToPlay].Stop();
            bgmActive = false;
        }
    }

public void PlayMFX(int soundToPlay)
    {
        bgm[soundToPlay].Stop();
        // Imposta la pitch dell'AudioSource in base ai valori specificati.
        bgm[soundToPlay].pitch = basePitch + Random.Range(-randomPitchOffset, randomPitchOffset); 
        bgm[soundToPlay].Play();
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