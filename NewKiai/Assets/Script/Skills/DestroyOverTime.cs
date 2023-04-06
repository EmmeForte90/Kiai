using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;


public class DestroyOverTime : MonoBehaviour
{
    [Header("Tempo di esplosione")]
    [SerializeField] public float lifeTime;

   
    void Update()
    {
        Destroy(gameObject, lifeTime);
        
    }
}
