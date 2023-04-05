using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;

public class Gate : MonoBehaviour
{

public float moveSpeed = 2f; // velocità di movimento
public int id;
public GameObject GateS;
public GameObject Box;
public GameObject Button;
public GameObject[] inter;
public Item TypesKey;
private bool _isInTrigger;
private bool unlock = false;
private bool dorOp = false;

private bool StopButton = false; // o la variabile che deve attivare la sostituzione
private bool _isDialogueActive;
public string request;
public string NotK;
public TextMeshProUGUI descriptions;
public Animator Anm;

    public Transform targetPosition;
    public GameObject Sprite;

    public float vibrationTime = 1f;
    public float vibrationStrength = 0.1f;

    private Vector3 startPosition;
    private float timer = 2f;


void Awake()
    {
        descriptions.text = request;
        startPosition = transform.position;
        //Anm = GetComponent<Animator>();
    }

void Update()
    {
        if(!unlock)
        {
        if (_isInTrigger && Input.GetButtonDown("Talk") && !_isDialogueActive)
        {
            Move.instance.stopInput = true;
            Move.instance.Stop();
            Move.instance.Stooping();
            Box.gameObject.SetActive(true);

        }
        else if (_isDialogueActive && Input.GetButtonDown("Talk") && StopButton)
        {

            StopButton = false;
        }
        } 
        else if(unlock)
        {
                // Sposta gradualmente l'oggetto verso la posizione obiettivo
                Sprite.transform.position = Vector2.MoveTowards(Sprite.transform.position, targetPosition.position, moveSpeed * Time.deltaTime);
                GameplayManager.instance.DoorAct(id);

        }
    }

 IEnumerator StartVibration()
    {
       
        Anm.SetBool("vibrate", true);
        yield return new WaitForSeconds(timer);
        Anm.SetBool("vibrate", false);
        unlock = true;
       

    }

    public void DoorOpen()
    {
    Sprite.transform.position = targetPosition.position;
    dorOp = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {            
            Move.instance.NotStrangeAnimationTalk = true;
        if(!dorOp){
            if(!unlock)
        {
         Button.gameObject.SetActive(true);
        _isInTrigger = true;
        }
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
            Box.gameObject.SetActive(false);
            Button.gameObject.SetActive(false); 
            StartCoroutine(StartVibration());

            }else
            {
                    print("niente da fare");
                 StartCoroutine(CloseBox());
            }
            
    }

IEnumerator CloseBox()
    {
    descriptions.text = NotK;
    foreach (GameObject But in inter)
        {
            But.SetActive(false);
            
        }
    yield return new WaitForSeconds(2);
    Move.instance.stopInput = false;
    Box.gameObject.SetActive(false);
    Button.gameObject.SetActive(false); // Initially hide the dialogue text   
    descriptions.text = request;
    foreach (GameObject But in inter)
        {
            But.SetActive(true);
            
        }
    }

public void Denied()
    {
            Move.instance.stopInput = false;
            Box.gameObject.SetActive(false);
            Button.gameObject.SetActive(false); // Initially hide the dialogue text   
            descriptions.text = request;
    
    }


 private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Move.instance.NotStrangeAnimationTalk = false;
            if(!dorOp){
            Button.gameObject.SetActive(false); // Initially hide the dialogue text
            _isInTrigger = false;
            Box.gameObject.SetActive(false);
            descriptions.text = request;
            }

        }
    }











}

