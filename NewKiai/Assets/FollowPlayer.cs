using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Audio;
using Cinemachine;

public class FollowPlayer : MonoBehaviour
{

    private CinemachineVirtualCamera vCam;
    public bool camFollowPlayer = true;
    private GameObject player;


    // Start is called before the first frame update
    void Start()
    {
        if(camFollowPlayer)
        {
        player = GameObject.FindGameObjectWithTag("Player");
        vCam = GameObject.FindWithTag("MainCamera").GetComponent<CinemachineVirtualCamera>(); //ottieni il riferimento alla virtual camera di Cinemachine
        vCam.Follow = player.transform;
        }
    }


private void OnTriggerEnter2D(Collider2D other)
{
    // Controlliamo se il player ha toccato il collider
    if (other.CompareTag("Player"))
    {
        vCam.Follow = player.transform;      
    }
}
}
