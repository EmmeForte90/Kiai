using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Audio;
using Spine.Unity;
using Spine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class MerkantKitai : MonoBehaviour
{
    public int IDCharacter;
    public TextMeshProUGUI CharacterName; // Reference to the TextMeshProUGUI component
    private GameObject player; // Reference to the player's position
    public TextMeshProUGUI dialogueText; // Reference to the TextMeshProUGUI component
    public TextMeshProUGUI dialogueMenu; // Reference to the TextMeshProUGUI component
    public TextMeshProUGUI Nameitem; // Reference to the TextMeshProUGUI component
    public TextMeshProUGUI Description; // Reference to the TextMeshProUGUI component
    public Image previewImages;
    private int prices;
    private int IDItem;

    public TextMeshProUGUI Value; // Reference to the TextMeshProUGUI component
    public Dialogues Dial;

    public GameObject button;
    public GameObject dialogueBox;
    public GameObject Menu;

    private string[] dialogue; // array of string to store the dialogues
    public float dialogueDuration; // variable to set the duration of the dialogue
    private int dialogueIndex; // variable to keep track of the dialogue status
    private float elapsedTime; // variable to keep track of the elapsed time
    //private Animator anim; // componente Animator del personaggio
    public bool isInteragible;
    public bool heFlip;
    private bool StopButton = false; // o la variabile che deve attivare la sostituzione
    private bool Talk = false;
    private bool EndDia = false;

    private bool _isInTrigger;
    private bool _isDialogueActive;

[Header("Audio")]
    [HideInInspector] public float basePitch = 1f;
    [HideInInspector] public float randomPitchOffset = 0.1f;
    [SerializeField] public AudioClip[] listSound; // array di AudioClip contenente tutti i suoni che si vogliono riprodurre
    private AudioSource[] bgm; // array di AudioSource che conterrà gli oggetti AudioSource creati
    public AudioMixer SFX;
    //private bool bgmActive = false;

 [Header("Animations")]
    [SpineAnimation][SerializeField] private string idleAnimationName;
    [SpineAnimation][SerializeField] private string HitAnimationName;
    private string currentAnimationName;
    public SkeletonAnimation _skeletonAnimation;
    private Spine.AnimationState _spineAnimationState;
    private Spine.Skeleton _skeleton;
    Spine.EventData eventData;

    public GameObject Sashimi, BackB,
        Kunai, Shuriken, Onigiri;


public static MerkantKitai Instance;


void Awake()
{
        Instance = this;   
        player = GameObject.FindWithTag("Player");
        _skeletonAnimation = GetComponent<SkeletonAnimation>();
        if (_skeletonAnimation == null) {
            Debug.LogError("Componente SkeletonAnimation non trovato!");
        }        
         _spineAnimationState = GetComponent<Spine.Unity.SkeletonAnimation>().AnimationState;
        _spineAnimationState = _skeletonAnimation.AnimationState;
        _skeleton = _skeletonAnimation.skeleton;
        //rb = GetComponent<Rigidbody2D>();
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


    void Start()
    {        
        button.gameObject.SetActive(false); // Initially hide the dialogue text
        dialogueText.gameObject.SetActive(false); // Initially hide the dialogue text
        dialogueBox.gameObject.SetActive(false); // Hide dialogue text when player exits the trigger
       // anim = GetComponent<Animator>();
        

    }

 public void KunaiI()
    {
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(Kunai);
    }

    public void ShurikenI()
    {
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(Shuriken);
    }

    public void OnighiriI()
    {
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(Onigiri);
    }

    public void SashimiI()
    {
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(Sashimi);
    }

    public void BackI()
    {
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(BackB);
    }

    void Update()
    {

dialogueDuration = GameplayManager.instance.dialogueDuration;
Value.text = GameplayManager.instance.money.ToString();
dialogue = Dial.dialogue; // Reference to the TextMeshProUGUI component


if(Talk)
{Hit();}
else
{Idle();}

if(EndDia)
{Menu.gameObject.SetActive(true);}
else
{Menu.gameObject.SetActive(false);}       
           
            

        //anim.SetBool("talk", Talk);
if(heFlip)
{
FacePlayer();
}


        if (_isInTrigger && Input.GetButtonDown("Talk") && !_isDialogueActive)
        {
            Move.instance.stopInput = true;
            Move.instance.Stop();
            Move.instance.Stooping();
            dialogueIndex = 0;
            StartCoroutine(ShowDialogue());
        }
        else if (_isDialogueActive && Input.GetButtonDown("Talk") && StopButton)
        {
            EventSystem.current.SetSelectedGameObject(null); //necessary clear the event system
            EventSystem.current.SetSelectedGameObject(Onigiri);
            Cursor.visible = true;
            NextDialogue();
             StopButton = false;
        }
        
        if (Input.GetButtonDown("Fire3") && EndDia)
        {
            Move.instance.NotStrangeAnimationTalk = false;
            button.gameObject.SetActive(false); // Initially hide the dialogue text
            _isInTrigger = false;
            _isDialogueActive = false;
            dialogueBox.gameObject.SetActive(false); // Hide dialogue text when player exits the trigger
            dialogueText.gameObject.SetActive(false); // Hide dialogue text when player exits the trigger
            Menu.gameObject.SetActive(false);
            Talk = false;
            EndDia = false;
            Move.instance.stopInput = false;
            Move.instance.NotStrangeAnimationTalk = false;
            
        }
    
}
    public void PlayMFX(int soundToPlay)
    {
        bgm[soundToPlay].Stop();
        // Imposta la pitch dell'AudioSource in base ai valori specificati.
        bgm[soundToPlay].pitch = basePitch + Random.Range(-randomPitchOffset, randomPitchOffset); 
        bgm[soundToPlay].Play();
    }

public void Back()
{
if (EndDia)
        {
            Move.instance.NotStrangeAnimationTalk = false;
            button.gameObject.SetActive(false); // Initially hide the dialogue text
            _isInTrigger = false;
            _isDialogueActive = false;
            dialogueBox.gameObject.SetActive(false); // Hide dialogue text when player exits the trigger
            dialogueText.gameObject.SetActive(false); // Hide dialogue text when player exits the trigger
            Menu.gameObject.SetActive(false);
            Talk = false;
            EndDia = false;
            Move.instance.stopInput = false;
            Move.instance.NotStrangeAnimationTalk = false;
            
        }

}


public void Buy(Item newItem)
{
    if(GameplayManager.instance.money >= prices)
    {
    //InventoryManager.Instance.RestoreSlot(newItem);
    //InventoryManager.Instance.AddItem(newItem);
    //InventoryManager.Instance.ListItem(newItem.id);
    //Add();
    IDItem = newItem.id;
    //newItem.value++;
    PlayMFX(0);
    dialogueMenu.text = "Thank you!"; // Reference to the TextMeshProUGUI component
    GameplayManager.instance.money -= prices;
    GameplayManager.instance.moneyText.text = GameplayManager.instance.money.ToString();
    GameplayManager.instance.moneyTextM.text = GameplayManager.instance.money.ToString();
    PuppetM.Instance.Idle();
    if(newItem.isConsumable)
    {InventoryManager.Instance.GadgetAc(IDItem);}
    InventoryManager.Instance.AddItem(newItem);
    InventoryManager.Instance.ListItem(IDItem);
    InventoryManager.Instance.ItemActive(IDItem);
    //AssignItem.Instance.UpdateNumId(newItem);
    }else if(GameplayManager.instance.money < prices)
    {
    dialogueMenu.text = "Sorry, buddy, you don't have much money"; // Reference to the TextMeshProUGUI component
    PlayMFX(1);
    PuppetM.Instance.Idle();
    }
}

// Questo metodo aggiunge un'unità all'inventario del giocatore nell'interfaccia utente del menu rapido.
// In base alla posizione corrente dell'elemento selezionato nel menu rapido, il metodo aggiunge un'unità all'elemento corrispondente.

public void Add()
{
// Se lo slot 4 del menu rapido è vuoto, aggiungi un'unità all'elemento corrispondente nel menu rapido.
if(UpdateMenuRapido.Instance.Slot4 <= 0 && UpdateMenuRapido.Instance.MXV4 <= 0 && ItemRapidMenu.Instance.MXV4 <= 0)//Bottom
{
//UpdateMenuRapido.Instance.MXV4++;
ItemRapidMenu.Instance.MXV4++;
InventoryManager.Instance.val++;
InventoryManager.Instance.Num_C.ToString();
}
// Altrimenti, Se lo slot 1 del menu rapido è vuoto, aggiungi un'unità all'elemento corrispondente nel menu rapido.
else if(UpdateMenuRapido.Instance.Slot1 <= 0 && UpdateMenuRapido.Instance.MXV1 <= 0 && ItemRapidMenu.Instance.MXV1 <= 0)//up
{
//UpdateMenuRapido.Instance.MXV1++;
ItemRapidMenu.Instance.MXV1++;
InventoryManager.Instance.val++;
InventoryManager.Instance.Num_C.ToString();

}
// Altrimenti, Se lo slot 2 del menu rapido è vuoto, aggiungi un'unità all'elemento corrispondente nel menu rapido.
else if(UpdateMenuRapido.Instance.Slot2 <= 0 && UpdateMenuRapido.Instance.MXV2 <= 0 && ItemRapidMenu.Instance.MXV2 <= 0)//Left
{
//UpdateMenuRapido.Instance.MXV2++;
ItemRapidMenu.Instance.MXV2++;
InventoryManager.Instance.val++;
InventoryManager.Instance.Num_C.ToString();

}
// Altrimenti, Se lo slot 3 del menu rapido è vuoto, aggiungi un'unità all'elemento corrispondente nel menu rapido.
else if(UpdateMenuRapido.Instance.Slot3 <= 0 && UpdateMenuRapido.Instance.MXV3 <= 0 && ItemRapidMenu.Instance.MXV3 <= 0)//Right
{
//UpdateMenuRapido.Instance.MXV3++;
ItemRapidMenu.Instance.MXV3++;
InventoryManager.Instance.val++;
InventoryManager.Instance.Num_C.ToString();

}
}
   

public void Preview(Item newItem)
{
prices = newItem.prices;
Nameitem.text = newItem.itemName; // Reference to the TextMeshProUGUI component
Description.text = newItem.Description; // Reference to the TextMeshProUGUI component
previewImages.sprite = newItem.icon;
dialogueMenu.text = newItem.Dialogue; // Reference to the TextMeshProUGUI component
PuppetM.Instance.Hit();
}



    private void OnTriggerEnter2D(Collider2D collision)
{
    if (collision.CompareTag("Player"))
    {
        Move.instance.NotStrangeAnimationTalk = true;
        button.gameObject.SetActive(true);
        _isInTrigger = true;
        if (!isInteragible)
        {
            dialogueIndex = 0; // Reset the dialogue index to start from the beginning
            StartCoroutine(ShowDialogue());
        }
    }
}

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Move.instance.NotStrangeAnimationTalk = false;
            button.gameObject.SetActive(false); // Initially hide the dialogue text
            _isInTrigger = false;
            StopCoroutine(ShowDialogue());
            dialogueIndex++; // Increment the dialogue index
            if (dialogueIndex >= dialogue.Length)
            {
                dialogueIndex = 0;
                _isDialogueActive = false;
                dialogueBox.gameObject.SetActive(false); // Hide dialogue text when player exits the trigger
                dialogueText.gameObject.SetActive(false); // Hide dialogue text when player exits the trigger
                //talk.Stop();

            }
        }
    }

    IEnumerator ShowDialogue()
{    
    Talk = true;
    //sgm[1].Play();
    _isDialogueActive = true;
    elapsedTime = 0; // reset elapsed time
    dialogueBox.gameObject.SetActive(true); // Show dialogue box
    dialogueText.gameObject.SetActive(true); // Show dialogue text
    string currentDialogue = dialogue[dialogueIndex]; // Get the current dialogue
    dialogueText.text = ""; // Clear the dialogue text
    for (int i = 0; i < currentDialogue.Length; i++)
    {
        dialogueText.text += currentDialogue[i]; // Add one letter at a time
        elapsedTime += Time.deltaTime; // Update the elapsed time
        if (elapsedTime >= dialogueDuration)
        {
            break;
        }
        yield return new WaitForSeconds(0); // Wait before showing the next letter
    }
            dialogueText.text = currentDialogue; // Set the dialogue text to the full current dialogue
            StopButton = true;
           
}



    void NextDialogue()
    {

        elapsedTime = 0; // reset elapsed time
        dialogueIndex++; // Increment the dialogue index
        if (dialogueIndex >= dialogue.Length)
        {
            EndDia = true;
            //talk.Stop();
            

        }
        else
        {
            StartCoroutine(ShowDialogue());
        }
    }



    void FacePlayer()
    {
        if (player != null)
        {
            if (player.transform.position.x > transform.position.x)
            {
                transform.localScale = new Vector3(1, 1, 1);
            }
            else
            {
                transform.localScale = new Vector3(-1, 1, 1);
            }
        }
    }



    public void Idle()
{
             if (currentAnimationName != idleAnimationName)
                {  
                    _spineAnimationState.SetAnimation(1, idleAnimationName, true);
                    currentAnimationName = idleAnimationName;
                }            
}

public void Hit()
{
             if (currentAnimationName != HitAnimationName)
                { 
                    //_spineAnimationState.ClearTrack(1);
                    _spineAnimationState.SetAnimation(1, HitAnimationName, false);
                    currentAnimationName = HitAnimationName;
                }
                // Add event listener for when the animation completes
                _spineAnimationState.GetCurrent(1).Complete += OnAttackAnimationComplete;
}

private void OnAttackAnimationComplete(Spine.TrackEntry trackEntry)
{
    // Remove the event listener
    trackEntry.Complete -= OnAttackAnimationComplete;

    // Clear the track 1 and reset to the idle animation
    //_spineAnimationState.ClearTrack(1);
    _spineAnimationState.SetAnimation(1, idleAnimationName, true);
    currentAnimationName = idleAnimationName;

}
}
