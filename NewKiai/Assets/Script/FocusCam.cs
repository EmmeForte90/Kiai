using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class FocusCam : MonoBehaviour
{
private CinemachineVirtualCamera vCam;
private GameObject player;
public GameObject CamFocus;
[Tooltip("Musica di base")]
public int MusicBefore;
[Tooltip("Musica da attivare se necessario quando la telecamera inquadra l'evento")]
public int MusicAfter;
public bool needMusic = false;

private void Start()
    {
    player = GameObject.FindGameObjectWithTag("Player");
    vCam = GameObject.FindWithTag("MainCamera").GetComponent<CinemachineVirtualCamera>(); 
    //ottieni il riferimento alla virtual camera di Cinemachine
    if(needMusic)
    {
        AudioManager.instance.CrossFadeINAudio(MusicBefore);
    }

    }

    // Metodo eseguito quando il player entra nel trigger
private void OnTriggerEnter2D(Collider2D other)
{
    // Controlliamo se il player ha toccato il collider
    if (other.CompareTag("Player"))
    {
        vCam.Follow = CamFocus.transform;
        GameplayManager.instance.battle = true;
        if(needMusic)
    {
        AudioManager.instance.MusicBefore = MusicBefore;
        AudioManager.instance.MusicAfter = MusicAfter;
        AudioManager.instance.CrossFadeOUTAudio(MusicBefore);
        AudioManager.instance.CrossFadeINAudio(MusicAfter);

    }
    }
}

private void OnTriggerExit2D(Collider2D other)
{
    // Controlliamo se il player ha toccato il collider
    if (other.CompareTag("Player"))
    {
        vCam.Follow = player.transform;
        GameplayManager.instance.battle = false;
        if(needMusic)
        {
        AudioManager.instance.MusicBefore = MusicBefore;
        AudioManager.instance.MusicAfter = MusicAfter;
        AudioManager.instance.CrossFadeOUTAudio(MusicAfter);
        AudioManager.instance.CrossFadeINAudio(MusicBefore);
        }
    }
}

}
