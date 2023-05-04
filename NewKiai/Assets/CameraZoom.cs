using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;


public class CameraZoom : MonoBehaviour
{
    public float zoomAmount = 5f; // L'ammontare dello zoom
    public float zoomSpeed = 2f; // La velocità di transizione dello zoom
    public float zoomResetSpeed = 2f; // La velocità di transizione per tornare allo zoom originario
    private float originalZoom; // Lo zoom originario della telecamera
    public CinemachineVirtualCamera vcam; // La telecamera virtuale Cinemachine
    
    public static CameraZoom instance;
 
 private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    private void Start()
    {
        // Salva lo zoom originario
        originalZoom = vcam.m_Lens.FieldOfView;
    }

public void ZoomIn()
    {
        if(!GameplayManager.instance.battle)
         {// Setta lo zoom della telecamera
            vcam.m_Lens.FieldOfView = zoomAmount;
            // Aggiorna la transizione dello zoom
            vcam.m_Lens.FieldOfView /= zoomSpeed;}
    }

public void ZoomOut()
    {
        if(!GameplayManager.instance.battle)
          {// Setta lo zoom della telecamera
            vcam.m_Lens.FieldOfView = originalZoom;
            // Aggiorna la transizione dello zoom
            vcam.m_Lens.FieldOfView /= zoomResetSpeed;}
    }
    
}

