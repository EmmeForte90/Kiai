using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;


public class TimelineSkipper : MonoBehaviour
{
    public PlayableDirector timeline = null;
    [SerializeField] 
    private float skipToSecond;
    public GameObject warningMes;
    private bool messageShown = false; // variabile per tenere traccia se il messaggio è stato mostrato
    private float messageTimer = 5f; // durata in secondi del timer per il messaggio

    private void Awake()
    {
        //timeline = GetComponent<PlayableDirector>();
    }

    private void Update()
{
    if (Input.GetButtonDown("Pause"))
    {
        if (!messageShown) // se il messaggio non è stato ancora mostrato
        {
            warningMes.gameObject.SetActive(true);
            messageShown = true; // il messaggio è stato mostrato
        }
        else // il messaggio è stato mostrato
        {
            if (timeline.time != 0) // controlla che la timeline non sia già in pausa
            {
                timeline.time = skipToSecond;
                StartCoroutine(skipOneFrame());
            }
        }
    }

    if (messageShown) // se il messaggio è stato mostrato
    {
        messageTimer -= Time.deltaTime; // diminuisci il timer
        if (messageTimer <= 0) // se il timer è arrivato a 0
        {
            messageShown = false; // il messaggio è stato nascosto
            warningMes.gameObject.SetActive(false);
            messageTimer = 5f; // resetta il timer per il prossimo utilizzo
        }
    }
}
    IEnumerator skipOneFrame()
    {
        yield return null;
        warningMes.gameObject.SetActive(false);
        timeline.time = timeline.time;
        timeline.playableGraph.GetRootPlayable(0).SetSpeed(1);
    }
}

