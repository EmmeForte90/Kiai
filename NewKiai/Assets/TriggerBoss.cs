using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.SceneManagement;

public class TriggerBoss : MonoBehaviour
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
    public BoxCollider2D trigger;
    public GameObject[] Arena;

    [Tooltip("Musica di base")]
    public int MusicBefore;
    [Tooltip("Musica da attivare se necessario quando la telecamera inquadra l'evento")]
    public int MusicAfter;

public void OrdaliaDosentExist()
    {
 Destroy(gameObject);
 }

 void Start()
    {
    virtualCamera = GameObject.FindWithTag("MainCamera").GetComponent<CinemachineVirtualCamera>();
    //ottieni il riferimento alla virtual camera di Cinemachine
    player = GameObject.FindWithTag("Player");
    //EnemyCount = MinEnemy;            

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



