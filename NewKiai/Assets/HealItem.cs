using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class HealItem : MonoBehaviour
{
    [SerializeField] GameObject Explode;
   // [SerializeField] Transform prefabExp;
    [SerializeField] int heal = 50;

    [SerializeField] float lifeTime = 0.5f;
    Rigidbody2D rb;

    [Header("Audio")]
    [SerializeField] public AudioClip[] list; // array di AudioClip contenente tutti i suoni che si vogliono riprodurre
    private AudioSource[] sgm; // array di AudioSource che conterrà gli oggetti AudioSource creati
    private bool sgmActive = false;
    public AudioMixer SFX;

    // Start is called before the first frame update
    void Start()
    {
         sgm = new AudioSource[list.Length]; // inizializza l'array di AudioSource con la stessa lunghezza dell'array di AudioClip
        for (int i = 0; i < list.Length; i++) // scorre la lista di AudioClip
        {
            sgm[i] = gameObject.AddComponent<AudioSource>(); // crea un nuovo AudioSource come componente del game object attuale (quello a cui è attaccato lo script)
            sgm[i].clip = list[i]; // assegna l'AudioClip corrispondente all'AudioSource creato
            sgm[i].playOnAwake = false; // imposto il flag playOnAwake a false per evitare che il suono venga riprodotto automaticamente all'avvio del gioco
            sgm[i].loop = false; // imposto il flag playOnAwake a false per evitare che il suono venga riprodotto automaticamente all'avvio del gioco

        }
 // Aggiunge i canali audio degli AudioSource all'output del mixer
        foreach (AudioSource audioSource in sgm)
        {
        audioSource.outputAudioMixerGroup = SFX.FindMatchingGroups("Master")[0];
        }

Debug.Log("AudioMixer aggiunto correttamente agli AudioSource.");
        //Recupera i componenti del rigidbody
        Move.instance.AnimationHeal();
        Move.instance.Stop();
        Instantiate(Explode, transform.position, transform.rotation);
        PlayerHealth.Instance.IncreaseHP(heal);
        Invoke("Destroy", lifeTime);
    }

    public void PlaySFX(int soundToPlay)
    {
        if (!sgmActive)
        {
            sgm[soundToPlay].Play();
            sgmActive = true;
        }
    }
   
    
    private void Destroy()
    {
        Destroy(gameObject);
    }
}


