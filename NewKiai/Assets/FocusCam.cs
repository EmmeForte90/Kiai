using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class FocusCam : MonoBehaviour
{
private CinemachineVirtualCamera vCam;
private GameObject player;
public GameObject CamFocus;

private void Start()
    {
    player = GameObject.FindGameObjectWithTag("Player");
    vCam = GameObject.FindWithTag("MainCamera").GetComponent<CinemachineVirtualCamera>(); 
    //ottieni il riferimento alla virtual camera di Cinemachine

    }

    // Metodo eseguito quando il player entra nel trigger
private void OnTriggerEnter2D(Collider2D other)
{
    // Controlliamo se il player ha toccato il collider
    if (other.CompareTag("Player"))
    {
        vCam.Follow = CamFocus.transform;
    }
}

private void OnTriggerExit2D(Collider2D other)
{
    // Controlliamo se il player ha toccato il collider
    if (other.CompareTag("Player"))
    {
        vCam.Follow = player.transform;

    }
}

}
