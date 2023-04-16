using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;
using Spine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;
using Cinemachine;

public class Duello : MonoBehaviour
{
    private GameObject toy; // Variabile per il player
    private GameObject Actor; // Variabile per il player
    private CinemachineVirtualCamera virtualCamera;
    public GameObject PointForCamera;
    private VibrateCinemachine vibrateCinemachine;

    public GameObject PointPlayer;
    public GameObject PointEnemy;

    public GameObject StartIntermezzo; // Variabile per il player
    public GameObject Tatakai; // Variabile per il player

    [Header("Tasti da premere")]
    [Tooltip("Qui ci sono i tasti che il player deve premere per vincere il duello")]
    public GameObject Triangolo;
    public GameObject Quadrato;
    public GameObject Cerchio;



    [Header("Audio")]
    [HideInInspector] public float basePitch = 1f;
    [HideInInspector] public float randomPitchOffset = 0.1f;
    [SerializeField] public AudioClip[] listSound; // array di AudioClip contenente tutti i suoni che si vogliono riprodurre
    private AudioSource[] sgm; // array di AudioSource che conterrà gli oggetti AudioSource creati
    public AudioMixer SFX;
    private bool sgmActive = false;
    private bool DuelStart = false;
    private int randomChance;
    private bool TimeButton = false;
    private bool buttonPressed = false;
    private bool buttonNotPressed = false;
    private bool canStartTimer = false;
    private bool Startbutton = true;
    private bool End = false;
    public float DuelloTime;
    public int Duellomax = 1;

    public int ContMosse;
    public int mosseMAX = 5;



    public static Duello instance;

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
//Debug.Log("AudioMixer aggiunto correttamente agli AudioSource.");
    vibrateCinemachine = GameObject.FindWithTag("MainCamera").GetComponent<VibrateCinemachine>(); //ottieni il riferimento alla virtual camera di Cinemachine
    virtualCamera = GameObject.FindWithTag("MainCamera").GetComponent<CinemachineVirtualCamera>(); //ottieni il riferimento alla virtual camera di Cinemachine
    //player = GameObject.FindWithTag("Player");
    toy = GameObject.FindWithTag("Player");
    Actor = GameObject.FindWithTag("Boss");
    Triangolo.gameObject.SetActive(false);
    Quadrato.gameObject.SetActive(false);
    Cerchio.gameObject.SetActive(false);
    }

private void Start()
    {
   
    Move.instance.StopinputTrue();
    StartIntermezzo.gameObject.SetActive(true);
    StartCoroutine(DuringInter());
    virtualCamera.Follow = PointForCamera.transform;
    toy.transform.position = PointPlayer.transform.position;
    Actor.transform.position = PointEnemy.transform.position;
    }

IEnumerator DuringInter()
    {
        yield return new WaitForSeconds(3f);
        StartIntermezzo.gameObject.SetActive(false);
        yield return new WaitForSeconds(1f);
        Tatakai.gameObject.SetActive(true);
        yield return new WaitForSeconds(3f);
        Tatakai.gameObject.SetActive(false);
        DuelStart = true;
    }

private void Update()
{
    if(DuelStart)
    {
        if (Startbutton)
        {
        RandomicDefence();
        Startbutton = false;
        }

    if(canStartTimer)
    {
        ChooseButton();
        DuelloTime -= Time.deltaTime; //decrementa il timer ad ogni frame
        if (DuelloTime <= 0f) 
        {
        Triangolo.gameObject.SetActive(false);
        Quadrato.gameObject.SetActive(false);
        Cerchio.gameObject.SetActive(false);
        DuelloTime = Duellomax;
        canStartTimer = false;
        End = true;
        }
    }   
        if(End && buttonPressed)
        { //Danno al boss
            buttonPressed = false; print("Hai vinto");
            End = false;
            ContMosse++;
            StartCoroutine(NextButton());
        }

        if(End && !buttonPressed)
        { //Danno al boss
            buttonNotPressed = false; print("Hai Perso");
            End = false;
            ContMosse++;
            StartCoroutine(NextButton());
        }
        

        if(ContMosse == mosseMAX)
        { 
            EndDuello();
        }
    }

}


private void ChooseButton()
{
if (TimeButton) 
        {       /////////////////////////////////////////////////
                if (randomChance == 1 || randomChance == 4)
                {
                if (Input.GetButton("Fire3"))
                {
                //triangolo
                buttonPressed = true;
                } 
                /////////////////////////////////////////////////
                } else if (randomChance == 2 || randomChance == 5)
                {
                if (Input.GetButton("Fire1"))
                {
                //Quadrato
                buttonPressed = true;
                } 
                }
                /////////////////////////////////////////////////
                else if (randomChance == 3 || randomChance == 6)
                {
                if (Input.GetButton("Fire2"))
                {
                //triangolo
                buttonPressed = true;
                } 
                /////////////////////////////////////////////////
                }
        }
}

void RandomicDefence()
{
    randomChance = Random.Range(1, 6); // Genera un numero casuale compreso tra 1 e 6
    TimeButton = true;


    if (randomChance == 1 || randomChance == 4) 
    {
    Triangolo.gameObject.SetActive(true);
    Quadrato.gameObject.SetActive(false);
    Cerchio.gameObject.SetActive(false);
    canStartTimer = true;
    } 
    else if (randomChance == 2 || randomChance == 5) 
    {
    Triangolo.gameObject.SetActive(false);
    Quadrato.gameObject.SetActive(true);
    Cerchio.gameObject.SetActive(false);
    canStartTimer = true;
    } 
    else if (randomChance == 3 || randomChance == 6) 
    {
    Triangolo.gameObject.SetActive(false);
    Quadrato.gameObject.SetActive(false);
    Cerchio.gameObject.SetActive(true);
    canStartTimer = true;
    }
}



IEnumerator NextButton()
    {
        yield return new WaitForSeconds(1f);
        Startbutton = true;
    }

public void sbam()
{
    vibrateCinemachine.Vibrate(0.2f, 0.2f);
    //SuonoCrash
}

public void EndDuello()
{
    print("Il duello è finito");
    Move.instance.StopinputFalse();
    KiaiGive();
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



