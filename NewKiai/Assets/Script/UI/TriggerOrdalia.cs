using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class TriggerOrdalia : MonoBehaviour
{    
    [Header("Il tipo di Quest che completa")]
    public Quests Quest;
    public bool isQuest = false;
    public int id;
private int lastSpawnIndex = -1;

    [Header("Il tipo di nemici e la cutscene")]
    [Tooltip("Il tempo per far partire l'ordalia consigliabile 2 secondi")]
    public float TimeStart = 2f;
    public GameObject Enemy;
    public GameObject Camera;
    private CinemachineVirtualCamera virtualCamera;
    private GameObject player;
    public GameObject Actor;
    public GameObject VFX;
    public BoxCollider2D trigger;
    public GameObject[] Arena;

    [Header("Ondate")]
    public GameObject[] EnemyPrefab;
    [Tooltip("array di punti di spawn")]
    public Transform[] SpawnPoints;
    private bool generateWaves = true;
    private bool StartOndata = false;
    [Tooltip("contatore delle ondate")]
    public int waveCount;
    public int MaxOndate;
    public int EnemiesPerWave;
[Header("Numero di nemici per ogni ondata")]
public int[] waves;
    private bool canStartNextWave = true;
    private bool ContatoreAum = true;
    [Header("Il valore deve sempre esse dato in negativo")]
    public int EnemyDefeated;

    [Header("Tempo tra uno spawn e un intervallo tra ondate")]
    [Tooltip("Tempo tra lo spawn di un nemico e l'altro")]
    public float timerMax;
    private float NextTimer = 0.5f;

    [Tooltip("Musica di base")]
    public int MusicBefore;
    [Tooltip("Musica da attivare se necessario quando la telecamera inquadra l'evento")]
    public int MusicAfter;

    private void Start()
    {
        virtualCamera = GameObject.FindWithTag("MainCamera").GetComponent<CinemachineVirtualCamera>();
        player = GameObject.FindWithTag("Player");
    }

    private void Update()
    {
        if (GameplayManager.instance.EnemyDefeated == 1)
        {
            if (!StartOndata)
            {
                if (GameplayManager.instance == null || !GameplayManager.instance.PauseStop)
                {
                    GameplayManager.instance.EnemyDWave = 0;
                    GameplayManager.instance.EnemyDefeated = 0;
                    GenerateEnemy();
                    StartOndata = true;
                }
            }
        }

        if (GameplayManager.instance.EnemyDefeated == EnemyDefeated && StartOndata)
        {
            StartCoroutine(EndOrdalia());
        }

        if (GameplayManager.instance.EnemyDWave == EnemiesPerWave && StartOndata)
        {
            NextTimer -= Time.deltaTime;
            if (NextTimer <= 0f)
            {
                Cont();
                GenerateEnemy();
            }
        }
    }

    private void Cont()
    {
        if (ContatoreAum)
        {
            waveCount++;
            NextTimer = timerMax;
            GameplayManager.instance.EnemyDWave = 0;
            canStartNextWave = true;
            ContatoreAum = false;
        }
    }

    public void OrdaliaDoesntExist()
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

    private void GenerateEnemy()
{
    MaxOndate = waves.Length;
    if (EnemyPrefab.Length > 0 && waveCount < MaxOndate)
    {
        EnemiesPerWave = waves[waveCount];
        if (canStartNextWave)
        {
            for (int i = 0; i < EnemiesPerWave; i++)
            {
                int spawnIndex = GetNextSpawnIndex();
                if (GameplayManager.instance.EnemyDefeated < EnemyDefeated)
                {
                    if (waveCount <= MaxOndate)
                    {
                        GameObject enemyToSpawn = EnemyPrefab[0];
                        Instantiate(enemyToSpawn, SpawnPoints[spawnIndex].position, transform.rotation);
                        Instantiate(VFX, SpawnPoints[spawnIndex].position, transform.rotation);

                        // Remove the spawned enemy from the array
                        GameObject[] updatedEnemyPrefab = new GameObject[EnemyPrefab.Length - 1];
                        Array.Copy(EnemyPrefab, 1, updatedEnemyPrefab, 0, EnemyPrefab.Length - 1);
                        EnemyPrefab = updatedEnemyPrefab;

                        canStartNextWave = false;
                    }
                }
            }
        }
    }
}


private int GetNextSpawnIndex()
{
    int nextSpawnIndex = UnityEngine.Random.Range(0, SpawnPoints.Length);
    if (nextSpawnIndex == lastSpawnIndex)
    {
        nextSpawnIndex = (nextSpawnIndex + 1) % SpawnPoints.Length;
    }
    lastSpawnIndex = nextSpawnIndex;
    return nextSpawnIndex;
}


    private IEnumerator EndOrdalia()
    {
        yield return new WaitForSeconds(2f);
        virtualCamera.Follow = player.transform;
        foreach (GameObject arenaObject in Arena)
        {
            arenaObject.SetActive(false);
            AudioManager.instance.CrossFadeOUTAudio(MusicAfter);
            AudioManager.instance.CrossFadeINAudio(MusicBefore);
            if (isQuest)
            {
                Quest.isActive = false;
                Quest.isComplete = true;
            }
            GameplayManager.instance.ordalia = false;
            GameplayManager.instance.battle = false;
            GameplayManager.instance.BossEnd(id);
        }
    }
}
