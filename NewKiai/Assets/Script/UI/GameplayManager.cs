using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;
using UnityEngine.UI;
using Cinemachine;
using TMPro;
using Spine.Unity.AttachmentTools;
using Spine.Unity;
using Spine;

public class GameplayManager : MonoBehaviour
{
    public static bool playerExists;
    public GameObject GM; // Variabile per il player

    public GameObject player; // Variabile per il player
    public GameObject toy; // Variabile per il player
    public int Damage;
    private GameObject Actor; // Variabile per il player
    private GameObject Menu; // Variabile per il player
    //private Health Enemy;
    public GameObject comboC; // Variabile per il player
    [SerializeField] TextMeshProUGUI ComboValue;
    public float comboTimer = 3f; 
    public int C_Count = 0;

[Tooltip("Attiva la spunta solo se sei nel main menu")]
    public bool isStartGame;
    private string sceneName = "Mainmenu";
    private string congName = "ENDINGDEMO";

    private CinemachineVirtualCamera virtualCamera;
    [HideInInspector]

   public string spawnPointTag = "SpawnPoint";


    [Header("Money")]
    [SerializeField] public int money = 0;
    [SerializeField] public TextMeshProUGUI moneyText;
    [SerializeField] public TextMeshProUGUI moneyTextM;
    [SerializeField] public GameObject moneyObject;
    [SerializeField] public GameObject moneyObjectM;
    [HideInInspector]
    public bool PauseStop = false;
    //Variabile del testo dei money
    private VibrateCinemachine vibrateCinemachine;

    [Header("Style")]
    [SerializeField]  public GameObject[] StyleS;
    [SerializeField]  public GameObject[] StyleM;
    [SerializeField]  public GameObject Selector;
    [SerializeField]  public bool[] styleIcon;
    
    

    [Header("Fade")]
    [SerializeField] GameObject callFadeIn;
    [SerializeField] GameObject callFadeOut;

    [Header("Pause")]
    [SerializeField]  GameObject PauseMenu;
    private GameObject Scenary;
    public AudioMixer MSX;
    public AudioMixer SFX;
    public float dialogueDuration; // variable to set the duration of the dialogue

    [Header("Difficoltà del gioco")]
    public bool Easy = false;
    public bool Normal = true;
    public bool Hard = false;
    public int EnemyDefeated = 0;
    public int EnemyDWave = 0;
    public bool ordalia = false;
    public bool boss = false;
    public bool battle = false;
    [Tooltip("Musica di base")]
    public int MusicBefore;
    [Tooltip("Musica da attivare se necessario quando Si ritorna al mainmenu")]
    public int MusicAfter;
    [Header("Abilitazioni")]
    public bool[] SkillActive;
    public bool unlockWalljump = false;
    public bool unlockDoubleJump = false;
    public bool unlockDash = false;
    public bool unlockRampino = false;
   [SerializeField] public GameObject Walljump;   
   [SerializeField] public GameObject DoubleJump;   
   [SerializeField] public GameObject Dash;
   [SerializeField] public GameObject Rampino;

    public bool startGame = false;
    [SerializeField] public Animator myAnimator;

    [Header("Ordalie")]
    private GameObject[] Ordalia;
    public bool[] OrdaliaActive;
    
    [Header("Porte")]
    private GameObject[] Door;
    public bool[] DoorActive;

    [Header("Boss")]
    private GameObject[] Boss;
    public bool[] BoosActive;

    public static GameplayManager instance;


    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        Application.targetFrameRate = 60;
          // Verifica se un'istanza del GameObject esiste già e distruggila se necessario
        if (playerExists) //&& gameplayOff) 
        {
            Destroy(gameObject);
        }
        else 
        {
            playerExists = true;
            DontDestroyOnLoad(gameObject);
            
        }

        Scenary = GameObject.FindWithTag("Scenary");
        virtualCamera = GameObject.FindWithTag("MainCamera").GetComponent<CinemachineVirtualCamera>(); //ottieni il riferimento alla virtual camera di Cinemachine
        vibrateCinemachine = GameObject.FindWithTag("MainCamera").GetComponent<VibrateCinemachine>(); //ottieni il riferimento alla virtual camera di Cinemachine
        //player = GameObject.FindWithTag("Player");
        Menu = GameObject.FindWithTag("Bound");

        // Cerca tutti i GameObjects con il tag "Timeline" all'inizio dello script
        //Ordalia = GameObject.FindGameObjectsWithTag("Ordalia");
        StartCoroutine(StartFadeInSTART());

        if(startGame)
        {
        Move.instance.stopInput = true;
        AudioManager.instance.PlayMFX(0);
        virtualCamera.Follow = Menu.transform;
        unlockWalljump = false;   
        Walljump.gameObject.SetActive(false);
        unlockDoubleJump = false; 
        DoubleJump.gameObject.SetActive(false);
        unlockDash = false;
        Dash.gameObject.SetActive(false);  
        unlockRampino = false; 
        Rampino.gameObject.SetActive(false);  
        }


        if(!moneyObject.gameObject)
        {
            moneyObject.gameObject.SetActive(true);
        }
        
        moneyText.text = money.ToString();
        moneyTextM.text = money.ToString();    
        //Il testo assume il valore dello money
    }

public void Update()
{
       if(unlockWalljump) 
       { Walljump.gameObject.SetActive(true);}
        if(unlockDoubleJump) 
       { DoubleJump.gameObject.SetActive(true);}
        if(unlockDash) 
       { Dash.gameObject.SetActive(true);}
        if(unlockRampino) 
       { Rampino.gameObject.SetActive(true);}

       if(C_Count > 0)
       {
        comboC.gameObject.SetActive(true);
       }else if(C_Count <= 0)
       {
        comboC.gameObject.SetActive(false);
       }

        if (C_Count > 0) 
        {
        comboTimer -= Time.deltaTime; //decrementa il timer ad ogni frame
        if (comboTimer <= 0f) {
        ResetComboCount(); //ripristina il comboCount a 0 quando il timer raggiunge 0
        }
        }

}
public void sbam()
{
    vibrateCinemachine = GameObject.FindWithTag("MainCamera").GetComponent<VibrateCinemachine>(); //ottieni il riferimento alla virtual camera di Cinemachine
    if(vibrateCinemachine == null)
    {
    vibrateCinemachine.Vibrate(0.2f, 0.2f);
    } else 
    {
    vibrateCinemachine.Vibrate(0.2f, 0.2f);
    } 
    //SuonoCrash
}


 public void FirstoOfPlay()
{
    startGame = false;
    player.gameObject.SetActive(true);
    toy.gameObject.SetActive(true);
    PauseMenu.gameObject.SetActive(false);
    player = GameObject.FindWithTag("Player");
    virtualCamera = GameObject.FindWithTag("MainCamera").GetComponent<CinemachineVirtualCamera>(); //ottieni il riferimento alla virtual camera di Cinemachine
    Move.instance.Stop();
    virtualCamera.Follow = player.transform;
    virtualCamera.LookAt = player.transform;
}

public void StopPlay()
{
    startGame = true;
    //toy = GameObject.FindWithTag("Player");
    toy.gameObject.SetActive(false);
    virtualCamera = GameObject.FindWithTag("MainCamera").GetComponent<CinemachineVirtualCamera>(); //ottieni il riferimento alla virtual camera di Cinemachine
    Move.instance.Stop();
    //virtualCamera.Follow = toy.transform;
    //virtualCamera.LookAt = toy.transform;
}

public void mainmenu()
    {
        StartCoroutine(fade());
        StartCoroutine(Returnmm());
        AudioManager.instance.CrossFadeOUTAudio(MusicBefore);
        AudioManager.instance.CrossFadeINAudio(MusicAfter);
        StopPlay();
    }
IEnumerator Returnmm()
    {
        yield return new WaitForSeconds(2);
        toy.gameObject.SetActive(false);
        player.gameObject.SetActive(false);
        StopPlay();
    }
IEnumerator fade()
    {
        FadeOut();
        Move.instance.stopInput = true;
        Move.instance.Stop();
        yield return new WaitForSeconds(2);
        player.gameObject.SetActive(false);
        // Invochiamo l'evento di cambio scena
        ChangeScene();
    }
// Metodo per cambiare scena
private void ChangeScene()
{
    SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
    SceneManager.sceneLoaded += OnMainLoaded;
}

// Metodo eseguito quando la scena è stata caricata
private void OnMainLoaded(Scene scene, LoadSceneMode mode)
{
    FadeIn();
    SceneManager.sceneLoaded -= OnMainLoaded;
    StopFade();    
}
////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
//SOLO PER IL PROTOTIPO
public void Congrat()
    {
        StartCoroutine(fadeC());
        StopPlay();
    }

IEnumerator fadeC()
    {
        FadeOut();
        Move.instance.stopInput = true;
        Move.instance.Stop();
        yield return new WaitForSeconds(2);
        player.gameObject.SetActive(false);
        // Invochiamo l'evento di cambio scena
        ChangeSceneC();
    }
// Metodo per cambiare scena
private void ChangeSceneC()
{
    SceneManager.LoadScene(congName, LoadSceneMode.Single);
    SceneManager.sceneLoaded += OnCongLoaded;
}

// Metodo eseguito quando la scena è stata caricata
private void OnCongLoaded(Scene scene, LoadSceneMode mode)
{
    FadeIn();
    SceneManager.sceneLoaded -= OnCongLoaded;
    StopFade();    
}
////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
public void ComboCount()
{
C_Count++;
myAnimator.SetTrigger("Take");
comboTimer = 3f; //decrementa il timer ad ogni frame
ComboValue.text = C_Count.ToString();
}

public void ResetComboCount()
{
C_Count = 0;
ComboValue.text = C_Count.ToString();
}



public void SetDifficultAtt(){

if (Easy)
    {
        //Enemy = GameObject.FindWithTag("Enemy").GetComponent<Health>();
        //Enemy.maxHealth /= 2;
    }
    else if (Normal)
    {
        //Non succede nulla
    }
    else if (Hard)
    {
        //Enemy = GameObject.FindWithTag("Enemy").GetComponent<Health>();
        //Enemy.maxHealth *= 2;
    }
    
}


public void EasyG()
{
    Easy = true;
    Normal = false;
    Hard = false;
}
public void NormalG()
{
    Easy = false;
    Normal = true;
    Hard = false;
    
}
public void HardG()
{
    Easy = false;
    Normal = false;
    Hard = true;
}



public void ActivationGame()
{
    Actor = GameObject.FindWithTag("Actor");

     if(Actor == null)
        {
        //print("NotFoundActor");
        }else if(Actor != null)
        {
        //print("FIND IT!");    
        player.gameObject.SetActive(true);
        toy.transform.position = Actor.transform.position;
        Actor.gameObject.SetActive(false);
        }


}

public void DeactivationGame()
{
    Actor = GameObject.FindWithTag("Actor");

     if(Actor == null )
        {
        //print("NotFoundActor");
        }else
        {
        //print("FIND IT!");
        toy.transform.position = Actor.transform.position;
        toy.gameObject.SetActive(false);
        Actor.gameObject.SetActive(true);
        }

}
public void DeactivationGame2()
{
        //GM.gameObject.SetActive(false);
}
public void EnemyDefeat()
    {
           EnemyDefeated++;
           EnemyDWave++;
    }

 public void SetSFX(float volume)
    {
        SFX.SetFloat("Volume", volume);

    }

    public void SetVolume(float volume)
    {
        MSX.SetFloat("Volume", volume);

    }
public void SetDSpeed(float volume)
    {
       dialogueDuration = volume;

    }



public void Restore()
{
    if (PlayerHealth.Instance.gameObject.activeSelf)
    {
        PlayerHealth.Instance.currentHealth = PlayerHealth.Instance.maxHealth;
        PlayerHealth.Instance.healthBar.size = PlayerHealth.Instance.currentHealth / PlayerHealth.Instance.maxHealth;
    }
    // Ripristina L'essenza
    if (PlayerHealth.Instance.gameObject.activeSelf)
    {
        PlayerHealth.Instance.currentKiai = PlayerHealth.Instance.maxKiai;
        PlayerHealth.Instance.KiaiImg();
    }
}
  

public void TakeCamera()
    {
            virtualCamera = GameObject.FindWithTag("MainCamera").GetComponent<CinemachineVirtualCamera>(); //ottieni il riferimento alla virtual camera di Cinemachine
            virtualCamera.Follow = player.transform;
            virtualCamera.LookAt = player.transform;    
    } 

public void StartPlay()
    {
        StartCoroutine(StartStage());
    }

#region  Score
//Funziona
    public void AddTomoney(int pointsToAdd)
    {
        money += pointsToAdd;
        //Lo money aumenta
        moneyText.text = money.ToString(); 
        moneyTextM.text = money.ToString();    
        //il testo dello money viene aggiornato
    }


#endregion

#region Pausa
        public void Pause()
        //Funzione pausa
        {
            PauseMenu.gameObject.SetActive(true);
           // Scenary.gameObject.SetActive(false);
            UIControllers.instance.SetSelectedGameObjectToSettings();
            //Move.instance.Player.gameObject.SetActive(false);
            PauseStop = true;
            //Time.timeScale = 0f;
        }

        public void Resume()
        {
            PauseStop = false;
            PauseMenu.gameObject.SetActive(false);
        }
public void StopInput()
        //Funzione pausa
        {
            PauseStop = true;
        }

        public void StopInputResume()
        {
            PauseStop = false;
        }
#endregion

   
private void OnEnable()
{
    SceneManager.sceneLoaded += OnSceneLoaded;
    // Troviamo il game object del punto di spawn
      if(!isStartGame) 
      {
        TakeCamera();
        GameObject spawnPoint = GameObject.FindWithTag(spawnPointTag);
        player.transform.position = spawnPoint.transform.position;
            
      }
}
public void TakePlayer()
{
        TakeCamera();
        GameObject spawnPoint = GameObject.FindWithTag(spawnPointTag);
        player.transform.position = spawnPoint.transform.position;
}

private void OnDisable()
{
   // SceneManager.sceneLoaded -= OnSceneLoaded;
}


private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
{
    // Cerca tutti i GameObjects con il tag "Ch_Quest"
    GameObject[] Ordalia = GameObject.FindGameObjectsWithTag("Ordalia");
    GameObject[] Door = GameObject.FindGameObjectsWithTag("Door");
    GameObject[] styleIcon = GameObject.FindGameObjectsWithTag("Style");
    GameObject[] SkillIt = GameObject.FindGameObjectsWithTag("Skill");
    GameObject[] BossIt = GameObject.FindGameObjectsWithTag("Boss");

/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    // Itera attraverso tutti gli oggetti trovati
    foreach (GameObject Character in Ordalia)
    {
        // Ottiene il componente QuestCharacters
        TriggerOrdalia ordaliT = Character.GetComponent<TriggerOrdalia>();

        // Verifica se il componente esiste
        if (ordaliT != null)
        {
            // Verifica se l'id della quest corrisponde all'id di un gameobject in OrdaliaActive
            int Id = ordaliT.id;
            for (int i = 0; i < OrdaliaActive.Length; i++)
            {
                if (OrdaliaActive[i] && i == Id)
                {
                    // Imposta ordaliT.FirstD a false
                    ordaliT.OrdaliaDoesntExist();
                    break;
                }
            }
        }
    }
/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    foreach (GameObject Character in SkillIt)
    {
        // Ottiene il componente QuestCharacters
        unlockSkill Skillt = Character.GetComponent<unlockSkill>();

        // Verifica se il componente esiste
        if (Skillt != null)
        {
            // Verifica se l'id della quest corrisponde all'id di un gameobject in OrdaliaActive
            int Id = Skillt.IdSkill;
            for (int i = 0; i <  SkillActive.Length; i++)
            {
                if ( SkillActive[i] && i == Id)
                {
                    // Imposta ordaliT.FirstD a false
                    Skillt.Take();
                    break;
                }
            }
        }
    }
/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

   /*foreach (GameObject Character in styleIcon)
    {
        // Ottiene il componente QuestCharacters
        StyleTake styleIconS = Character.GetComponent<StyleTake>();

        // Verifica se il componente esiste
        if (styleIconS != null)
        {
            // Verifica se l'id della quest corrisponde all'id di un gameobject in OrdaliaActive
            int Id = styleIconS.StyleID;
            for (int i = 0; i <  styleIcon.Length; i++)
            {
                if ( styleIcon[i] && i == Id)
                {
                    // Imposta ordaliT.FirstD a false
                    styleIconS.Take();
                    break;
                }
            }
        }
    } */
/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    foreach (GameObject Character in Door)
    {
        // Ottiene il componente QuestCharacters
        Gate DoorT = Character.GetComponent<Gate>();

        // Verifica se il componente esiste
        if (DoorT != null)
        {
            // Verifica se l'id della quest corrisponde all'id di un gameobject in OrdaliaActive
            int Id = DoorT.id;
            for (int i = 0; i <  DoorActive.Length; i++)
            {
                if ( DoorActive[i] && i == Id)
                {
                    // Imposta ordaliT.FirstD a false
                    DoorT.DoorOpen();
                    break;
                }
            }
        }
    }

}
public void StyleActivated(int id)
{
    StyleS[id].SetActive(true);
    StyleM[id].SetActive(true);
    styleIcon[id] = true;   
}
public void OrdaliaEnd(int id)
{
    // Imposta lo stato della quest a true
    OrdaliaActive[id] = true;   
}



public void SkillTaking(int id)
{
    // Imposta lo stato della quest a true
    SkillActive[id] = true;   
}

public void DoorAct(int id)
{
    // Imposta lo stato della quest a true
    DoorActive[id] = true;   
}

public void BossEnd(int id)
{
    // Imposta lo stato della quest a true
    BoosActive[id] = true;   
}

    IEnumerator Restart()
    {
        callFadeIn.gameObject.SetActive(true);
        //Instantiate(callFadeIn, centerCanvas.transform.position, centerCanvas.transform.rotation);
        yield return new WaitForSeconds(5f);
        //Le vite del player vengono aggiornate
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        //Lo scenario assume il valore della build
        SceneManager.LoadScene(currentSceneIndex);
        //Lo scenario viene ricaricato
    }




public void StopFade()
    {
        StartCoroutine(EndFede());
    }
// Coroutine per attendere il caricamento della scena
IEnumerator EndFede()
{   
    yield return new WaitForSeconds(1f);
    callFadeOut.gameObject.SetActive(false);
    callFadeIn.gameObject.SetActive(false);
    
}
public void FadeIn()
    {

StartCoroutine(StartFadeIn());

    }

    public void FadeOut()
    {

StartCoroutine(StartFadeOut());

    }
IEnumerator StartFadeInSTART()
    {
        callFadeIn.gameObject.SetActive(true);
        yield return new WaitForSeconds(5f);
        callFadeIn.gameObject.SetActive(false);
        //callFadeIn.gameObject.SetActive(false);


    }


IEnumerator StartFadeIn()
    {
        callFadeOut.gameObject.SetActive(false);
        callFadeIn.gameObject.SetActive(true);
        yield return new WaitForSeconds(5f);
        //callFadeOut.gameObject.SetActive(false);
        //callFadeIn.gameObject.SetActive(false);


    }

IEnumerator StartFadeOut()
    {        
        callFadeIn.gameObject.SetActive(false);
        callFadeOut.gameObject.SetActive(true);
        yield return new WaitForSeconds(5f);
       // callFadeIn.gameObject.SetActive(false);
       // callFadeOut.gameObject.SetActive(false);


    }



    IEnumerator StartStage()
    {
        if(!isStartGame)
        {
        callFadeOut.gameObject.SetActive(true);
        //Instantiate(callFadeOut, centerCanvas.transform.position, centerCanvas.transform.rotation);
        }
        //FindObjectOfType<PlayerMovement>().ReactivatePlayer();
        yield return new WaitForSeconds(5f);
        callFadeOut.gameObject.SetActive(false);

    }


}