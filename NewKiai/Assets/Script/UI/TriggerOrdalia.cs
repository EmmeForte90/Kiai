using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.SceneManagement;
using System.Linq;

public class TriggerOrdalia : MonoBehaviour
{    
    [Header("Il tipo di Quest che completa")]
    public Quests Quest;
    public bool isQuest = false;
    public int id;

    [Header("Il tipo di nemici e la cutscene")]
    [Tooltip("Il tempo per far partire l'ordalia consigliabile 2 secondi")]
    public float TimeStart = 2f;
    public GameObject Enemy;
    public GameObject Camera;
    private CinemachineVirtualCamera virtualCamera; //riferimento alla virtual camera di Cinemachine
    private GameObject player; // Variabile per il player
    public GameObject Actor;
    public GameObject VFX;
    public BoxCollider2D trigger;
    public GameObject[] Arena;

    [Header("Ondate")]
     public GameObject[] EnemyPrefab;
     [Tooltip("array di punti di spawn")]
    public Transform[] SpawnPoints; // array di punti di spawn
    private bool generateWaves = true;
    private bool StartOndata = false;
    [Tooltip("contatore delle ondate")]
    public int waveCount; // contatore delle ondate
    public int MaxOndate;
    public int EnemiesPerWave;

    private bool canStartNextWave = true;
    private bool ContatoreAum = true;
    [Header("Il valore deve sempre esse dato in negativo")]
    public int EnemyDefeated;
    
    [Header("Tempo tra uno spawn e un intervallo tra ondate")]
    [Tooltip("Tempo tra lo spaqwn di un nemico e l'altro")]
    public float timerMax;
    private float NextTimer = 0.5f;
   // [Tooltip("Tempo tra un ondata e l'altra")]
    //public float WaveInterval = 2f;

    [Tooltip("Numero di nemici generati per ondate, ad esempio ondata 1 spawn 3 nemici, ondata 2 4 ecc ecc")]
    public int[] waves = {3, 5, 7, 9};//Numerp di nemici generati per ondate
    private int lastSpawnIndex = -1;
               
    [Tooltip("Musica di base")]
    public int MusicBefore;
    [Tooltip("Musica da attivare se necessario quando la telecamera inquadra l'evento")]
    public int MusicAfter;

    void Start()
    {
    virtualCamera = GameObject.FindWithTag("MainCamera").GetComponent<CinemachineVirtualCamera>();
    //ottieni il riferimento alla virtual camera di Cinemachine
    player = GameObject.FindWithTag("Player");
    //EnemyCount = MinEnemy;
    }

  void Update()
    {
        if (GameplayManager.instance.EnemyDefeated == 1)
        {
            if(!StartOndata)
            {
                //print("Parte l'ordalia");
            if(GameplayManager.instance == null || !GameplayManager.instance.PauseStop)
            {
            GameplayManager.instance.EnemyDWave = 0;
            GameplayManager.instance.EnemyDefeated = 0;
            GeneratEnemy();
            StartOndata = true;
            }
            }
        }   

        if (GameplayManager.instance.EnemyDefeated == EnemyDefeated && StartOndata) 
        // se tutti i nemici sono stati sconfitti e abbiamo raggiunto l'ultimo livello di ondate
        {
            print("Ordalia finita");
            StartCoroutine(EndOrdalia()); // chiama la funzione EndOrdalia
        }

        


        
    //questa parte va sistemata perché non funziona
        if (GameplayManager.instance.EnemyDWave == EnemiesPerWave && StartOndata) 
        {
        NextTimer -= Time.deltaTime; //decrementa il timer ad ogni frame
        if (NextTimer <= 0f) 
        {
        ContatoreAum = true;
        Cont();
        GeneratEnemy();
        }
        
        }
}


private void Cont()
    {
       // 

        if(ContatoreAum)
        {
        waveCount++;
        NextTimer = timerMax;
        GameplayManager.instance.EnemyDWave = 0;
        canStartNextWave = true; // abilita il passaggio alla prossima ondata
        // Aumentiamo il contatore delle ondate
        ContatoreAum = false;
        }
    }




public void OrdaliaDosentExist()
    {
 Destroy(gameObject);
 }


     private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
         foreach (GameObject arenaObject in Arena)
        {
            arenaObject.SetActive(true);
            virtualCamera.Follow = Camera.transform;
            StartCoroutine(StartOrdalia());
        }
        }
    }


 private IEnumerator StartOrdalia()
    {

    trigger.enabled = false;
    ActorOrdalia.Instance.FacePlayer();
    ActorOrdalia.Instance.Standup();
    AudioManager.instance.CrossFadeOUTAudio(MusicBefore);
    AudioManager.instance.CrossFadeINAudio(MusicAfter);
    yield return new WaitForSeconds(2);
    ActorOrdalia.Instance.idle();
    yield return new WaitForSeconds(TimeStart);
    Actor.gameObject.SetActive(false);
    Enemy.gameObject.SetActive(true);
    GameplayManager.instance.ordalia = true;
    GameplayManager.instance.battle = true;

    }



// Questo è un metodo che genera nemici in base alle specifiche impostate nel gioco.
// Viene utilizzato un array di nemici per ondata e viene generato un numero specifico di ondate.
// Se tutti i nemici non sono stati sconfitti e non abbiamo raggiunto l'ultimo livello di ondate, continuiamo a generarli.

private void GeneratEnemy()
{
MaxOndate = waves.Length; 
// Impostiamo il numero totale di ondate dal numero di elementi nell'array 'waves'
if(EnemyPrefab.Length > 0 && waveCount < MaxOndate) 
// Verifichiamo se ci sono ancora nemici nell'array e se abbiamo raggiunto il numero di ondate massimo
{
EnemiesPerWave = waves[waveCount]; 
// Impostiamo il numero di nemici per ondata

if(canStartNextWave)
{

for (int i = 0; i < EnemiesPerWave; i++) 
// Per ogni nemico per ondata
{
lastSpawnIndex++; 
// Incrementiamo l'indice dell'ultima posizione di spawn utilizzata
if (GameplayManager.instance.EnemyDefeated < EnemyDefeated ) 
// Verifichiamo se il numero di nemici sconfitti è inferiore al numero massimo di nemici
{
if (lastSpawnIndex >= SpawnPoints.Length) 
// Se abbiamo utilizzato tutte le posizioni di spawn disponibili, ricominciamo dall'inizio
{
lastSpawnIndex = 0;
}
if (waveCount <= MaxOndate) 
// Se tutti i nemici non sono stati sconfitti e non abbiamo raggiunto l'ultimo livello di ondate continua a generarli
{
GameObject enemyToSpawn = EnemyPrefab[0]; 
// Prendiamo il primo nemico dell'array
Instantiate(enemyToSpawn, SpawnPoints[lastSpawnIndex].position, transform.rotation); 
// Spawniamo il nemico nella prossima posizione di spawn
Instantiate(VFX, SpawnPoints[lastSpawnIndex].position, transform.rotation); 
// Spawniamo il VFX nella prossima posizione di spawn
EnemyPrefab = EnemyPrefab.Where((enemy, index) => index != 0).ToArray();
// Disabilitiamo l'inizio della prossima ondata finché non saranno stati sconfitti tutti i nemici dell'attuale ondata
//yield return new WaitForSeconds(SpawnInterval); 
canStartNextWave = false;
// Attendiamo il tempo di intervallo tra uno spawn e l'altro
}
}
}
}
}
}



private IEnumerator EndOrdalia()
    {
            yield return new WaitForSeconds(2f);   
            virtualCamera.Follow = player.transform;
            foreach (GameObject arenaObject in Arena)
        {
            //print("L'ordina sta contando il tempo per la fine");
            arenaObject.SetActive(false);
            AudioManager.instance.CrossFadeOUTAudio(MusicAfter);
            AudioManager.instance.CrossFadeINAudio(MusicBefore);
            if(isQuest)
            {
            Quest.isActive = false;
            Quest.isComplete = true;
            }
            GameplayManager.instance.ordalia = false;
            GameplayManager.instance.battle = false;
            GameplayManager.instance.BossEnd(id);
            //Destroy(gameObject);
        }    
    }
}




