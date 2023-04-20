using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;
using Spine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;

public class TreasureScript : MonoBehaviour
{
    [Header("Key")]
    public Item TypesKey;
     public bool NeedKey = false; // indica se il tesoro è stato aperto

    public GameObject coinPrefab; // prefab per la moneta
    public GameObject VFX; // prefab per la moneta

    public int maxCoins = 10; // numero massimo di monete che possono essere rilasciate
    public float coinSpawnDelay = 0.1f; // ritardo tra la spawn di ogni moneta
    public float treasureLifetime = 10f; // tempo in secondi dopo il quale il tesoro sparirà
    public float coinForce = 5f; // forza con cui le monete saltano
    public Vector2 coinForceVariance = new Vector2(1, 1); // varianza della forza con cui le monete saltano
    public GameObject spawnPoint;
    private int coinCount; // conteggio delle monete
    private bool treasureOpened = false; // indica se il tesoro è stato aperto
 
 [Header("Audio")]
    [HideInInspector] public float basePitch = 1f;
    [HideInInspector] public float randomPitchOffset = 0.1f;
    [SerializeField] public AudioClip[] listSound; // array di AudioClip contenente tutti i suoni che si vogliono riprodurre
    private AudioSource[] bgm; // array di AudioSource che conterrà gli oggetti AudioSource creati
    public AudioMixer SFX;
    private bool bgmActive = false;

[Header("Animations")]
    [SpineAnimation][SerializeField] private string idleAnimationName;
    [SpineAnimation][SerializeField] private string openAnimationName;
    
    private string currentAnimationName;
    public SkeletonAnimation _skeletonAnimation;
    private Spine.AnimationState _spineAnimationState;
    private Spine.Skeleton _skeleton;
    Spine.EventData eventData;

public static TreasureScript instance;


 private void Awake()
    {
    if (_skeletonAnimation == null) {
        Debug.LogError("Componente SkeletonAnimation non trovato!");
    }
    _spineAnimationState = _skeletonAnimation.AnimationState;
    _skeleton = _skeletonAnimation.skeleton;
    if (instance == null)
    {
        instance = this;
    }
    bgm = new AudioSource[listSound.Length]; // inizializza l'array di AudioSource con la stessa lunghezza dell'array di AudioClip
        for (int i = 0; i < listSound.Length; i++) // scorre la lista di AudioClip
        {
            bgm[i] = gameObject.AddComponent<AudioSource>(); // crea un nuovo AudioSource come componente del game object attuale (quello a cui è attaccato lo script)
            bgm[i].clip = listSound[i]; // assegna l'AudioClip corrispondente all'AudioSource creato
            bgm[i].playOnAwake = false; // imposto il flag playOnAwake a false per evitare che il suono venga riprodotto automaticamente all'avvio del gioco
            bgm[i].loop = false; // imposto il flag playOnAwake a false per evitare che il suono venga riprodotto automaticamente all'avvio del gioco

        }
 // Aggiunge i canali audio degli AudioSource all'output del mixer
        foreach (AudioSource audioSource in bgm)
        {
        audioSource.outputAudioMixerGroup = SFX.FindMatchingGroups("Master")[0];
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(!NeedKey)
        {
        if (!treasureOpened && other.CompareTag("Player"))
        {
            // genera un numero casuale di monete da rilasciare
            coinCount = Random.Range(1, maxCoins + 1);

            // apri il tesoro
            PlayMFX(0);
            OpenAnm();
            Instantiate(VFX, spawnPoint.transform.position, Quaternion.identity);

            // inizia a spawnare le monete
            StartCoroutine(SpawnCoins());
            treasureOpened = true;
                    // distruggi il tesoro dopo un periodo di tempo specifico
        //Destroy(gameObject, treasureLifetime);
    }
    }else if(NeedKey)
        {
        if (!treasureOpened && other.CompareTag("Player"))
        {
             Accepted();
        }
}
}

public void Accepted()
    {
            Move.instance.stopInput = false;
            if(InventoryManager.Instance.itemDatabase.Find(q => q.id == TypesKey.id))
            {
                print("Hai aperto");
            InventoryManager.Instance.RemoveItem(TypesKey);
            }else
            {
                print("niente da fare");
            }
            
    }
IEnumerator SpawnCoins()
{

    for (int i = 0; i < coinCount; i++)
    {
        // crea una nuova moneta
GameObject newCoin = Instantiate(coinPrefab, spawnPoint.transform.position, Quaternion.identity);

        // applica una forza casuale alla moneta per farla saltare
        Vector2 randomForce = new Vector2(
            Random.Range(-coinForceVariance.x, coinForceVariance.x),
            Random.Range(-coinForceVariance.y, coinForceVariance.y)
        );
        newCoin.GetComponent<Rigidbody2D>().AddForce(randomForce * coinForce, ForceMode2D.Impulse);

        // aspetta prima di spawnare la prossima moneta
        yield return new WaitForSeconds(coinSpawnDelay);
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

public void OpenAnm()
{
    
    if (currentAnimationName != openAnimationName)
                {
                    _spineAnimationState.SetAnimation(2, openAnimationName, false);
                    currentAnimationName = openAnimationName;
                   // _spineAnimationState.Event += HandleEvent;
                }
                // Add event listener for when the animation completes
                //_spineAnimationState.GetCurrent(2).Complete += OnAttackAnimationComplete;
}

public void IdleAnm()
{
    if (currentAnimationName != idleAnimationName)
                {
                    _spineAnimationState.SetAnimation(1, idleAnimationName, true);
                    currentAnimationName = idleAnimationName;
                   // _spineAnimationState.Event += HandleEvent;
                }
                // Add event listener for when the animation completes
                //_spineAnimationState.GetCurrent(2).Complete += OnAttackAnimationComplete;
}

}
