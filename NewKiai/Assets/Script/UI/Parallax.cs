using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class Parallax : MonoBehaviour
{
   public float speed; //velocità di scorrimento del background
    private Transform cam; //riferimento alla telecamera principale
    private Vector3 previousCamPos; //posizione precedente della telecamera
    private CinemachineVirtualCamera virtualCamera; //riferimento alla virtual camera di Cinemachine
   // private CinemachineBasicMultiChannelPerlin noise; //riferimento al modulo noise di Cinemachine
    private Collider2D myCollider; //riferimento al collider del background
public float depth; //fattore di profondità del background

// Start is called before the first frame update
void Start()
{
    cam = Camera.main.transform; //ottieni il riferimento alla telecamera principale
    previousCamPos = cam.position; //imposta la posizione precedente della telecamera
    virtualCamera = GameObject.FindWithTag("MainCamera").GetComponent<CinemachineVirtualCamera>(); //ottieni il riferimento alla virtual camera di Cinemachine
   // noise = virtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>(); //ottieni il riferimento al modulo noise di Cinemachine
    myCollider = GetComponent<Collider2D>(); //ottieni il riferimento al collider del background

    //aggiorna la posizione del background con la profondità di campo desiderata
    Vector3 newPosition = transform.position + depth * cam.forward;
    transform.position = newPosition;
}

// Update is called once per frame
void Update()
{
    //calcola la quantità di spostamento della telecamera
    float parallax = (previousCamPos.x - cam.position.x) * speed;

   

    //aggiorna la posizione precedente della telecamera
    previousCamPos = cam.position;  
}
}