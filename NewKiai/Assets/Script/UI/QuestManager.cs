using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class QuestManager : MonoBehaviour
{    
    // Riferimento al contenitore dei pulsanti delle quest
    public Transform QuestContent;
   public GameObject InventoryQuest;
   [HideInInspector] public int qID;
    // Scriptable Object delle quest
public List<Quests> questDatabase;
private GameObject[] CharacterQ;

    // Riferimenti ai componenti delle immagini di preview e delle descrizioni
    public Image previewImages;
    public TextMeshProUGUI descriptions;
    public TextMeshProUGUI NameQ;


    // Array di booleani che mantengono lo stato delle quest
    public bool[] quest;
    public bool[] _QuestActive;
    public bool[] _QuestComplete;

     // Id unico del bottone della quest selezionata
    private static int uniqueId;
//public TextMeshProUGUI Title;
//public TextMeshProUGUI QuestsDescriptionText;
//public Image icon;

// Singleton pattern per accedere a QuestManager da altre classi
public static QuestManager Instance;

private void Awake()
{
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
}

private void Start()
{
    // Cerca tutti i GameObjects con il tag "Timeline" all'inizio dello script
    CharacterQ = GameObject.FindGameObjectsWithTag("Ch_Quest");
}

   

     // Metodo per aggiungere una nuova quest al database
    public void AddQuest(Quests newQuest)
{
    questDatabase.Add(newQuest);
}

public void ListQuest(int questId)
{
    // Cerca la quest con l'id specificato
    Quests quest = questDatabase.Find(q => q.id == questId);

    if (quest != null)
    {
        // Istanzia il prefab del bottone della quest nella lista UI
        GameObject obj = Instantiate(InventoryQuest, QuestContent);

        // Recupera il riferimento al componente del titolo della quest e del bottone
        var questT = obj.transform.Find("Title_quest").GetComponent<TextMeshProUGUI>();
        var questI = obj.transform.Find("Icon_item").GetComponent<Image>();

        // Assegna l'id univoco al game object istanziato
        obj.name = "QuestButton_" + quest.id;

        // Assegna il nome della quest al componente del titolo
        questT.text = quest.questName;
        questI.color = Color.red;

        // Assegna i valori desiderati ai componenti dell'immagine di preview e della descrizione del pulsante della quest
        previewImages.sprite = quest.Bigicon;
        descriptions.text = quest.Description;
        NameQ.text = quest.questName;

        // Aggiungi un listener per il click del bottone
        var button = obj.GetComponent<Button>();
        button.onClick.AddListener(() => OnQuestButtonClicked(quest.id, previewImages, descriptions, questI));
    }
}

public void OnQuestButtonClicked(int questId, Image previewImages, TextMeshProUGUI descriptions, Image questI)
{    
   // print(questId+"  "+questDatabase.Count);
    //if (questId >= 0 && questId < questDatabase.Count) non so perché non funzionava

    if (questId >= 0)
    {    
       // print("Ci sono");
        // Qui puoi fare qualcosa quando il pulsante della quest viene cliccato, ad esempio aprire una finestra con i dettagli della quest
        // Assegna i valori desiderati ai componenti dell'immagine di preview e della descrizione
        previewImages.sprite = questDatabase.Find(q => q.id == questId).Bigicon;
        descriptions.text = questDatabase.Find(q => q.id == questId).Description;
        NameQ.text = questDatabase.Find(q => q.id == questId).questName;
        if(_QuestComplete[questId])
        {
            questI.color = Color.black;
        }
    }
    
}





// Metodo per iniziare una quest
// id: l'indice della quest nell'array
public void QuestStart(int id)
{
    // Imposta lo stato della quest a true
    quest[id] = true;   
}
// Metodo per completare una quest
// id: l'indice della quest nell'array
public void QuestActive(int id)
{
    // Imposta lo stato di completamento della quest a true
    _QuestActive[id] = true;  
}


// Metodo per completare una quest
// id: l'indice della quest nell'array
public void QuestComplete(int id)
{
    // Imposta lo stato di completamento della quest a true
    _QuestComplete[id] = true;

  
}






private void OnEnable()
{
    SceneManager.sceneLoaded += OnSceneLoaded;
}

private void OnDisable()
{
    SceneManager.sceneLoaded -= OnSceneLoaded;
}


private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
{
    // Cerca tutti i GameObjects con il tag "Ch_Quest"
    GameObject[] CharacterQ = GameObject.FindGameObjectsWithTag("Ch_Quest");

    // Itera attraverso tutti gli oggetti trovati
    foreach (GameObject Character in CharacterQ)
    {
        // Ottiene il componente QuestCharacters
        QuestCharacters questCharacter = Character.GetComponent<QuestCharacters>();

        // Verifica se il componente esiste
        if (questCharacter != null)
        {
            // Verifica se l'id della quest corrisponde all'id di un gameobject in _QuestActive
            int questId = questCharacter.Quest.id;
            for (int i = 0; i < _QuestActive.Length; i++)
            {
                if (_QuestActive[i] && i == questId)
                {
                    // Imposta questCharacter.FirstD a false
                    questCharacter.FirstD = false;
                    break;
                }
            }
        }
    }
}




}






