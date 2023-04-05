using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Spine.Unity;

public class NPCDialogue : MonoBehaviour
{
    public Dialogues DManager;
    public List<Dialogues> ListDialogue;
    private bool changeD = false; // o la variabile che deve attivare la sostituzione
    private bool StopButton = false; // o la variabile che deve attivare la sostituzione

    public int IDCharater;
    private string[] dialogue; // array of string to store the dialogues
    private GameObject player; // Reference to the player's position
    public TextMeshProUGUI dialogueText; // Reference to the TextMeshProUGUI component
    public TextMeshProUGUI CharacterName; // Reference to the TextMeshProUGUI component
    public GameObject button;
    public GameObject dialogueBox;
    public float dialogueDuration; // variable to set the duration of the dialogue
    private int dialogueIndex; // variable to keep track of the dialogue status
    private float elapsedTime; // variable to keep track of the elapsed time
    private Animator anim; // componente Animator del personaggio
    public bool isInteragible;
    public bool heFlip;
    public bool moreDialogue;
    private bool _isInTrigger;
    private bool _isDialogueActive;
    private bool Talk = false;


[Header("Audio")]
[SerializeField] AudioSource talk;
[SerializeField] AudioSource Clang;

public static NPCDialogue instance;


void Awake()
{
    if (instance == null)
        {
            instance = this;
        }
    player = GameObject.FindWithTag("Player");
    dialogue = DManager.dialogue;
    CharacterName.text = DManager.CharacterName;
}

public void changeDialogue()
    {
        if (changeD) // o qualsiasi condizione basata sulla variabile specificata
        {
            foreach (Dialogues dialogueObject in ListDialogue)
            {
                if (dialogueObject != DManager)
                {
                    DManager = dialogueObject;
                    dialogue = DManager.dialogue;
                    break;
                }
            }
        }
    }


    void Start()
    {        
        button.gameObject.SetActive(false); // Initially hide the dialogue text
        dialogueText.gameObject.SetActive(false); // Initially hide the dialogue text
        dialogueBox.gameObject.SetActive(false); // Hide dialogue text when player exits the trigger
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        
        anim.SetBool("talk", Talk);
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
    }

public void clang()
{
Clang.Play();
}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Move.instance.NotStrangeAnimationTalk = true;
            button.gameObject.SetActive(true); // Initially hide the dialogue text
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
                talk.Stop();

            }
        }
    }

    IEnumerator ShowDialogue()
{
    Talk = true;
    talk.Play();
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
        yield return new WaitForSeconds(0.001f); // Wait before showing the next letter
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
            talk.Stop();
            dialogueIndex = 0;
            _isDialogueActive = false;
            Talk = false;
            dialogueBox.gameObject.SetActive(false); // Hide dialogue text when player exits the trigger
            dialogueText.gameObject.SetActive(false); // Hide dialogue text when player exits the trigger
            Move.instance.stopInput = false;
            if(moreDialogue)
            {changeD = true;}
            changeDialogue();
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
}
