using UnityEngine;
using Cinemachine;

public class RespawnObject : MonoBehaviour
{
    public Transform respawnPoint; // il punto di respawn del giocatore
    public string sceneName; // il nome della scena in cui si trova il punto di respawn
    [SerializeField] GameObject Sdeng;
    //[SerializeField] GameObject Selectionmenu;
    //[SerializeField] public Transform Pos;
    //public GameObject Camera;
    //[SerializeField] public GameplayManager gM;
   // public GameObject button;
    //private CinemachineVirtualCamera virtualCamera; //riferimento alla virtual camera di Cinemachine
    private GameObject player; // Variabile per il player

    //private bool _isInTrigger;
    //[HideInInspector]public bool isPray = false;

 public static RespawnObject instance;


    private void Awake()
    {
        if (instance == null)
        {    
            instance = this;
        }
    //   virtualCamera = GameObject.FindWithTag("MainCamera").GetComponent<CinemachineVirtualCamera>(); //ottieni il riferimento alla virtual camera di Cinemachine
        player = GameObject.FindWithTag("Player");
    }

/*void Update()
    {

        if (_isInTrigger && Input.GetButtonDown("Talk") && !isPray)
        {
            virtualCamera.Follow = Camera.transform;
            Instantiate(Sdeng, Pos.transform.position, transform.rotation);
            GameplayManager.instance.StopInput();
            Move.instance.StopinputTrue();
            Move.instance.AnimationRest();
            Move.instance.Stop();
            GameplayManager.instance.Restore();

    
            Move.instance.isPray = true;
            isPray = true;

        }
        else if (isPray && Input.GetButtonDown("Talk"))
        {
            Move.instance.animationWakeup();
            GameplayManager.instance.StopInputResume();
            Move.instance.StopinputFalse();
            UIControllers.instance.SetSelectedGameObjectToSettings();
          //  GameplayManager.instance.Shrine.gameObject.SetActive(false);
            //Selectionmenu.gameObject.SetActive(false);   
            isPray = false;
            Move.instance.isPray = false;
            _isInTrigger = false;
            virtualCamera.Follow = player.transform;
        

    
        }
        
    }*/

    
private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
           // Move.instance.NotStrangeAnimationTalk = true;
            Move.instance.sceneName = sceneName;
            //GameplayManager.instance.StopDefaultSkill = true;
           // button.gameObject.SetActive(true); // Initially hide the dialogue text
           // _isInTrigger = true;
            
        }
    }
    /*
private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {          
            Move.instance.NotStrangeAnimationTalk = false;
            button.gameObject.SetActive(false); // Initially hide the dialogue text
            _isInTrigger = false;
            
        }
    }
*/
}

