using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;
using Spine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;
using Cinemachine;
using UnityEngine.UI;
using TMPro;

public class Stallo : MonoBehaviour
{
    private GameObject toy; // Variabile per il player
    private GameObject Actor; // Variabile per il player
    private CinemachineVirtualCamera virtualCamera;
    public GameObject PointForCamera;
    private VibrateCinemachine vibrateCinemachine;
    [SerializeField]public TextMeshProUGUI _Time;

    public GameObject PointPlayer;
    public GameObject PointEnemy;
    public Transform Pointlunch;
    //public float velocita = 1f; // velocità del movimento
    public float forzaMassima = 10f; // forza massima da applicare al Rigidbody
//public float raggioSfera = 0.5f; // raggio della sfera per la collisione con il terreno

    public GameObject StartIntermezzo; // Variabile per il player
    public GameObject Tatakai; // Variabile per il player

    [Header("Tasti da premere")]
    [Tooltip("Qui ci sono i tasti che il player deve premere per vincere il duello")]
    public float maxBar = 100f;
    public float currentBar;
    public Scrollbar StalloBar;
    public GameObject UI; // Variabile per il player
     [Tooltip("Quale fatality deve eseguire il player? 1-Jump 2-Dance(DemonMonk) 3-Back 4-For Lance and BigKatana 5-Veloce")]
    public int ChooseFatality;

    [Header("Stallo")]
    public GameObject StalloObj;

    [Header("Boss")]
    public GameObject Boss;

    [Header("Audio")]
   // [HideInInspector] public float basePitch = 1f;
    //[HideInInspector] public float randomPitchOffset = 0.1f;
    [SerializeField] public AudioClip[] listSound; // array di AudioClip contenente tutti i suoni che si vogliono riprodurre
    private AudioSource[] sgm; // array di AudioSource che conterrà gli oggetti AudioSource creati
    public AudioMixer SFX;
  //  private bool sgmActive = false;
    private bool DuelStart = false;
   // private bool canStartTimer = false;
    public float DuelloTime;
    public int Duellomax = 10;
    private bool Win = false;
    public SkeletonAnimation spineAnimation;
    
    [Header("VFX")]
    public GameObject StalVFX;
    public GameObject Smoke;

    [Header("Fatalitis")]
    [SpineAnimation][SerializeField] private string StartAnimationName;
    [SpineAnimation][SerializeField] private string LoopAnimationName;
    [SpineAnimation][SerializeField] private string DefeatAnimationName;
    [SpineAnimation][SerializeField] private string WinAnimationName;

    public static Stallo instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        DuelloTime = Duellomax;
      
        sgm = new AudioSource[listSound.Length]; // inizializza l'array di AudioSource con la stessa lunghezza dell'array di AudioClip
        for (int i = 0; i < listSound.Length; i++) // scorre la lista di AudioClip
        {
            sgm[i] = gameObject.AddComponent<AudioSource>(); // crea un nuovo AudioSource come componente del game object attuale (quello a cui è attaccato lo script)
            sgm[i].clip = listSound[i]; // assegna l'AudioClip corrispondente all'AudioSource creato
            sgm[i].playOnAwake = false; // imposto il flag playOnAwake a false per evitare che il suono venga riprodotto automaticamente all'avvio del gioco
            sgm[i].loop = false; // imposto il flag playOnAwake a false per evitare che il suono venga riprodotto automaticamente all'avvio del gioco
        }
 // Aggiunge i canali audio degli AudioSource all'output del mixer
        foreach (AudioSource audioSource in sgm)
        {
        audioSource.outputAudioMixerGroup = SFX.FindMatchingGroups("Master")[0];
        }
    vibrateCinemachine = GameObject.FindWithTag("MainCamera").GetComponent<VibrateCinemachine>(); //ottieni il riferimento alla virtual camera di Cinemachine
    //virtualCamera = GameObject.FindWithTag("MainCamera").GetComponent<CinemachineVirtualCamera>(); //ottieni il riferimento alla virtual camera di Cinemachine
    toy = GameObject.FindWithTag("Player");
    Actor = GameObject.FindWithTag("Boss");
    currentBar = 0;
    DuelloTime = Duellomax;
    toy.transform.position = PointPlayer.transform.position;
    toy.transform.localScale = new Vector2(1, 1);    
    Actor.transform.position = PointEnemy.transform.position;
    StalloObj.transform.position = Boss.transform.position;
    }

private void Start()
    {
    Move.instance.StopinputTrue();
    Move.instance.poseStalmate();
    StartIntermezzo.gameObject.SetActive(true);
    StartCoroutine(DuringInter());
    //virtualCamera.Follow = PointForCamera.transform; 
    }

IEnumerator DuringInter()
    {
        yield return new WaitForSeconds(3f);
        StartIntermezzo.gameObject.SetActive(false);
        yield return new WaitForSeconds(1f);
        Tatakai.gameObject.SetActive(true);
        yield return new WaitForSeconds(3f);
        Tatakai.gameObject.SetActive(false);
        spineAnimation.state.SetAnimation(0, StartAnimationName, false);
        Move.instance.StartStalmate();        
        yield return new WaitForSeconds(1f);
        DuelStart = true;
    }

private void Update()
{  
    if(DuelStart)
    {
        UI.gameObject.SetActive(true);
        StalVFX.gameObject.SetActive(true);
        Smoke.gameObject.SetActive(true);
        Move.instance.LoopStalmate();
        spineAnimation.state.SetAnimation(1, LoopAnimationName, true);
        StalloBar.size = currentBar / maxBar;
        StalloBar.size = Mathf.Clamp(StalloBar.size, 0.01f, 1);
        _Time.text = DuelloTime.ToString();
        DuelloTime -= Time.deltaTime; //decrementa il timer ad ogni frame
        if (DuelloTime <= 0f) 
        {
        //DuelloTime = Duellomax;
        UI.gameObject.SetActive(false);
        if(currentBar >= maxBar)
        {
            Win = true;
        }
        else if(currentBar <= maxBar)
        {
            Win = false;
        }
        StartCoroutine(EndDuello());
        DuelStart = false;
        }   
    if (Input.GetButtonDown("Fire1"))
        {
            currentBar++;
        }     
    }
}


IEnumerator EndDuello()
{
    StalVFX.gameObject.SetActive(false);
    Smoke.gameObject.SetActive(false);
    print("Il duello è finito");
    if(Win)
    {
            print("Hai vinto");
            Move.instance.drawsword = true;
            Move.instance.Fatality(ChooseFatality);
            spineAnimation.state.SetAnimation(2, DefeatAnimationName, false);
            KiaiGive();
    }else if(!Win)
    {
            print("Hai perso");            
            Move.instance.drawsword = true;
            //Move.instance.KnockbackLong();
            Move.instance.BigHurt();
            spineAnimation.state.SetAnimation(2, WinAnimationName, false);
    }
    yield return new WaitForSeconds(5f);
    Boss.gameObject.SetActive(true);
    TriggerBoss.instance.vitalita -= 50;
    nemico_boss.instance.ColorChange();
    //virtualCamera.Follow = toy.transform;
    Move.instance.StopinputFalse();
    Destroy(gameObject);
}

    void KiaiGive()
{
    int randomChance = Random.Range(1, 10); // Genera un numero casuale compreso tra 1 e 10

    if (randomChance <= 8) // Se il numero casuale è compreso tra 1 e 8 (80% di probabilità), aggiungi 5 di essenza
    {
        PlayerHealth.Instance.currentKiai += 5;
    }
    else // Se il numero casuale è compreso tra 9 e 10 (20% di probabilità), aggiungi 10 di essenza
    {
        PlayerHealth.Instance.currentKiai += 10;
    }
}
}