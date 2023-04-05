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
    public float TimeStart;
    public GameObject Enemy;
    public GameObject Camera;
    private GameObject player; // Variabile per il player
    public GameObject Actor;
    public GameObject VFX;
    public BoxCollider2D trigger;
    private CinemachineVirtualCamera virtualCamera; //riferimento alla virtual camera di Cinemachine
    public GameObject[] Arena;

    [Header("Ondate")]
     public GameObject[] EnemyPrefab;
    public Transform[] SpawnPoints; // array di punti di spawn
    private bool generateWaves = true;
    private bool StartOndata = false;
    private int waveCount = 0; // contatore delle ondate
    private int COnde;

    [Header("Il valore deve sempre esse dato in negativo")]
    public int EnemyDefeated = 0;
    
    [Header("Tempo tra uno spawn e un intervallo tra ondate")]
    public float SpawnInterval = 2f;
    public float WaveInterval = 2f;

    public int[] waves = {3, 5, 7, 9};//Numerp di nemici generati per ondate
    private int lastSpawnIndex = -1;
           

    void Start()
    {
    virtualCamera = GameObject.FindWithTag("MainCamera").GetComponent<CinemachineVirtualCamera>(); //ottieni il riferimento alla virtual camera di Cinemachine
    player = GameObject.FindWithTag("Player");
    //EnemyCount = MinEnemy;
    }

  void Update()
    {
  
        if (Enemy == null)
        {
            if(!StartOndata)
            {
            if(GameplayManager.instance == null || !GameplayManager.instance.PauseStop)
            {
            StartCoroutine(GeneratEnemy());
            StartOndata = true;
            }
            }
        }   

        if (GameplayManager.instance.EnemyDefeated >= EnemyDefeated) // se tutti i nemici sono stati sconfitti e abbiamo raggiunto l'ultimo livello di ondate
        {
            print("Ordalia finita");
            StartCoroutine(EndOrdalia()); // chiama la funzione EndOrdalia
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


 IEnumerator StartOrdalia()
    {

    trigger.enabled = false;
    ActorOrdalia.Instance.Standup();
    AudioManager.instance.CrossFadeOUTAudio(1);
    AudioManager.instance.CrossFadeINAudio(2);
    yield return new WaitForSeconds(2);
    ActorOrdalia.Instance.idle();
    yield return new WaitForSeconds(TimeStart);
    Actor.gameObject.SetActive(false);
    Enemy.gameObject.SetActive(true);
    GameplayManager.instance.ordalia = true;
    }



IEnumerator GeneratEnemy()
{     
   
    COnde = waves.Length;

while (EnemyPrefab.Length > 0 && waveCount < COnde) // finché ci sono ancora nemici nell'array e non abbiamo raggiunto il numero di ondate
{
    int EnemiesPerWave = waves[waveCount];
    waveCount++; // aumenta il contatore delle ondate
    
    for (int i = 0; i < EnemiesPerWave; i++) // per ogni nemico per ondata
    {
        lastSpawnIndex++;
        if (GameplayManager.instance.EnemyDefeated < EnemyDefeated)
        {
        if (lastSpawnIndex >= SpawnPoints.Length)
        {
            lastSpawnIndex = 0;
        }
        if (waveCount <= COnde) // se tutti i nemici non sono stati sconfitti e non abbiamo raggiunto l'ultimo livello di ondate continua a generarli
        {
        GameObject enemyToSpawn = EnemyPrefab[0]; // prendi il primo nemico dell'array
        Instantiate(enemyToSpawn, SpawnPoints[lastSpawnIndex].position, transform.rotation); // spawn il nemico nella prossima posizione di spawn
        Instantiate(VFX, SpawnPoints[lastSpawnIndex].position, transform.rotation); // spawn il nemico nella prossima posizione di spawn
        AiEnemysword.instance.chaseThreshold = 10f; // soglia di distanza per iniziare l'inseguimento
        EnemyPrefab = EnemyPrefab.Where((enemy, index) => index != 0).ToArray();
        yield return new WaitForSeconds(SpawnInterval);
        }
        }
        
    }
    
    if (waveCount < COnde) // se non è l'ultima ondata
    {
        yield return new WaitForSeconds(WaveInterval); // aspetta l'intervallo tra le ondate
    }
    
}
}



public IEnumerator EndOrdalia()
    {
            yield return new WaitForSeconds(2f);   
            virtualCamera.Follow = player.transform;
            foreach (GameObject arenaObject in Arena)
        {
            print("L'ordina sta contando il tempo per la fine");
            arenaObject.SetActive(false);
            AudioManager.instance.CrossFadeOUTAudio(2);
            AudioManager.instance.CrossFadeINAudio(1);
            if(isQuest)
            {
            Quest.isActive = false;
            Quest.isComplete = true;
            }
            GameplayManager.instance.ordalia = false;
            GameplayManager.instance.OrdaliaEnd(id);
            Destroy(gameObject);
        }    
    }
}




