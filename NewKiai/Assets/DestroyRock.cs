using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class DestroyRock : MonoBehaviour
{
    [Header("Distruzione")]
    public GameObject rockPiecePrefab;
    public GameObject VFX;
    public GameObject VFXclang;
    public Transform pointVFX;
    public int numPieces = 10;

     [Header("Audio")]
    [HideInInspector] public float basePitch = 1f;
    [HideInInspector] public float randomPitchOffset = 0.1f;
    [SerializeField] public AudioClip[] listSound; // array di AudioClip contenente tutti i suoni che si vogliono riprodurre
    private AudioSource[] bgm; // array di AudioSource che conterrà gli oggetti AudioSource creati
    public AudioMixer SFX;
    private bool bgmActive = false;
    private bool Go = false;

public static DestroyRock instance;
    

    private void Awake()
    {
           
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

 public void PlayMFX(int soundToPlay)
    {
        bgm[soundToPlay].Stop();
        // Imposta la pitch dell'AudioSource in base ai valori specificati.
        bgm[soundToPlay].pitch = basePitch + Random.Range(-randomPitchOffset, randomPitchOffset); 
        bgm[soundToPlay].Play();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Hitbox")
        {
            if(Move.instance.style == 1 && Move.instance.isCrushRock)
            {
            PlayMFX(0);
            for (int i = 0; i < numPieces; i++)
            {
                Instantiate(VFX, pointVFX.transform.position, transform.rotation);
                GameObject rockPiece = Instantiate(rockPiecePrefab, transform.position, Quaternion.identity);
                rockPiece.GetComponent<Rigidbody2D>().AddForce(new Vector2(Random.Range(-50, 50), Random.Range(-50, 50)));
                Destroy(rockPiece, 2f);
            }
            Destroy(gameObject);
            }else 
            {
                Instantiate(VFXclang, pointVFX.transform.position, transform.rotation);
                PlayMFX(1);
                Move.instance.Bump();
                Move.instance.Knockback();
            }
        }
    }


   
}
