using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Audio;
using Spine.Unity;
using Spine;
using UnityEngine.SceneManagement;

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
    private AudioSource[] sgm; // array di AudioSource che conterrà gli oggetti AudioSource creati
    public AudioMixer SFX;
    private bool sgmActive = false;

 [Header("Animations")]
    [SpineAnimation][SerializeField] private string idleAnimationName;
    [SpineAnimation][SerializeField] private string HitAnimationName;
    private string currentAnimationName;
    public SkeletonAnimation _skeletonAnimation;
    private Spine.AnimationState _spineAnimationState;
    private Spine.Skeleton _skeleton;
    Spine.EventData eventData;

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

}


    void Start()
    {        
        button.gameObject.SetActive(false); // Initially hide the dialogue text
        dialogueText.gameObject.SetActive(false); // Initially hide the dialogue text
        dialogueBox.gameObject.SetActive(false); // Hide dialogue text when player exits the trigger
       // anim = GetComponent<Animator>();
        

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
    

public void clang()
{
sgm[0].Play();
}

public void Buy(Item newItem)
{
    if(GameplayManager.instance.money >= prices)
    {
    InventoryManager.Instance.AddItem(newItem);
    InventoryManager.Instance.ListItem(newItem.id);
    dialogueMenu.text = "Thank you!"; // Reference to the TextMeshProUGUI component
    GameplayManager.instance.money -= prices;
    PuppetM.Instance.Idle();
    }else if(GameplayManager.instance.money < prices)
    {
    dialogueMenu.text = "Sorry, buddy, you don't have much money"; // Reference to the TextMeshProUGUI component
    PuppetM.Instance.Idle();
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
