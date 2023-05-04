using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;


public class CameraZoom : MonoBehaviour
{
    private float zoomAmount = 50; // L'ammontare dello zoom
    private float zoomSpeed = 0.5f; // velocità di transizione dello zoom
    private float zoomResetSpeed = 2f; // velocità di reset dello zoom
    private float originalZoom = 70; // Lo zoom originario della telecamera
    private bool zoomIn = false;
    private bool zoomout = false;
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
private void Update()
    {
        // Salva lo zoom originario
        if(zoomIn && !zoomout)
        {
        vcam.m_Lens.FieldOfView = Mathf.Lerp(vcam.m_Lens.FieldOfView, zoomAmount, zoomSpeed * Time.deltaTime);
        }else if(!zoomIn && zoomout)
        {
        vcam.m_Lens.FieldOfView = Mathf.Lerp(vcam.m_Lens.FieldOfView, originalZoom, zoomResetSpeed * Time.deltaTime);
        }

        if(vcam.m_Lens.FieldOfView == 70 || vcam.m_Lens.FieldOfView == 50)
        {
            zoomIn = false;
            zoomout = false;
        }
    }

public void ZoomIn()
{
    if (!GameplayManager.instance.battle)
    {
        // Setta lo zoom della telecamera gradualmente
        zoomIn = true;
        zoomout = false;
    }
}

public void ZoomOut()
{
    if (!GameplayManager.instance.battle)
    {
        // Resetta lo zoom della telecamera gradualmente
        zoomIn = false;
        zoomout = true;
    }
}
    
}

