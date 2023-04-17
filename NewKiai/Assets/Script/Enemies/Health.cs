using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    
        public static Health instance;
    //public int ID;
   [Header("Sistema Di HP")]
   // Enemy enemy;
    public float maxHealth = 100f;
    public float currentHealth = 100f;
    public Color originalColor;
    public float colorChangeDuration;
    public float chaseThreshold = 2f; // soglia di distanza per iniziare l'inseguimento

void Start()
{
 if (GameplayManager.instance == null) return;
    
    if (GameplayManager.instance.Easy)
    {
        maxHealth /= 2;
    }
    else if (GameplayManager.instance.Hard)
    {
        maxHealth *= 2;
    }
    
    currentHealth = maxHealth;
}





}
