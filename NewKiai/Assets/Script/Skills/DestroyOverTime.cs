using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;


public class DestroyOverTime : MonoBehaviour
{
    [Header("Tempo di esplosione")]
    [SerializeField] public float lifeTime;

    [Header("Se è un esplosione")]
    [SerializeField] public bool isExplosion;
    [SerializeField] public float intensity = 5f;
    [SerializeField] public float time = .1f;

    void Update()
    {
        Destroy(gameObject, lifeTime);
        if(isExplosion)
        {
           // CinemachineShake.instance.ShakeCamera(intensity, time);
        }
    }
}
