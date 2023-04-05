using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public static class TimelineManager 
{
        public static bool[] filmati;

// Metodo per iniziare una timeline
// id: l'indice della quest nell'array
public static void TimelineStart(int id)
{
    // Imposta lo stato della timeline a true
    filmati[id] = true;   
}

public static void TimelineEnd(int id)
{
    // Imposta lo stato della timeline a false
    filmati[id] = false;   
}
   
}
