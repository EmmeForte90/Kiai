using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class Torcia : MonoBehaviour
{
    public GameObject torcia;
    public GameObject VFX;
    public GameObject VFXOff;
    public Transform pointVFX;
    [Header("Audio")]
    [HideInInspector] public float basePitch = 1f;
    [HideInInspector] public float randomPitchOffset = 0.1f;
    [SerializeField] public AudioClip[] listSound; // array di AudioClip contenente tutti i suoni che si vogliono riprodurre
    private AudioSource[] bgm; // array di AudioSource che conterrà gli oggetti AudioSource creati
    public AudioMixer SFX;
    private bool bgmActive = false;
    private bool Go = false;
    public static Torcia instance;
    
    private void Awake()
    {
        torcia.gameObject.SetActive(false);
           
        bgm = new AudioSource[listSound.Length]; // inizializza l'array di AudioSource con la stessa lunghezza dell'array di AudioClip
        for (int i = 0; i < listSound.Length; i++) // scorre la lista di AudioClip
        {
            bgm[i] = gameObject.AddComponent<AudioSource>(); // crea un nuovo AudioSource come componente del game object attuale (quello a cui è attaccato lo script)
            bgm[i].clip = listSound[i]; // assegna l'AudioClip corrispondente all'AudioSource creato
            bgm[i].playOnAwake = false; // imposto il flag playOnAwake a false per evitare che il suono venga riprodotto automaticamente all'avvio del gioco
            bgm[i].loop = false; // imposto il flag playOnAwake a false per evitare che il suono venga riprodotto automaticamente all'avvio del gioco

        }
 // Aggiunge i canali audio degli AudioSource all'output del mixer
        foreach (AudioSource audioSource in bgm)
        {
        audioSource.outputAudioMixerGroup = SFX.FindMatchingGroups("Master")[0];
        }
    }
public void StopMFX(int soundToPlay)
    {
        if (bgmActive)
        {
            bgm[soundToPlay].Stop();
            bgmActive = false;
        }
    }
public void Fire()
    {
        PlayMFX(1);
        bgm[1].loop = true; 
        // imposto il flag playOnAwake a false per evitare che il suono venga riprodotto automaticamente all'avvio del gioco
    }
    
public void PlayMFX(int soundToPlay)
    {
        bgm[soundToPlay].Stop();
        // Imposta la pitch dell'AudioSource in base ai valori specificati.
        bgm[soundToPlay].pitch = basePitch + Random.Range(-randomPitchOffset, randomPitchOffset); 
        bgm[soundToPlay].Play();
    }
/*public void TorciaFun()
{}*/
void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Hitbox")
        {  
            if(Move.instance.style == 2)
            {torcia.gameObject.SetActive(true);
            Instantiate(VFX, pointVFX.transform.position, transform.rotation);
            PlayMFX(0);
            Fire();
            Go = true;
            }else if(Move.instance.style == 4 && Go)
            {torcia.gameObject.SetActive(false);
            Instantiate(VFXOff, pointVFX.transform.position, transform.rotation);
            PlayMFX(2);
            Go = false;
            }

        }   
    }

}
