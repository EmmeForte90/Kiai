using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StartGameSetting : MonoBehaviour
{
    public Transform respawnPoint; // il punto di respawn del giocatore
    public string sceneName; // il nome della scena in cui si trova il punto di respawn

 public static StartGameSetting instance;


    private void Awake()
    {
        if (instance == null)
        {    
            instance = this;
        }
        AudioManager.instance.CrossFadeINAudio(1);

    }

private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
        Move.instance.sceneName = sceneName;

        }

}
}
