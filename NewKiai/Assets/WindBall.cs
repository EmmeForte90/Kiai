using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class WindBall : MonoBehaviour
{
     public float rotationSpeed1 = 2f;
    public float rotationSpeed2 = 4f;
    public float rotationSpeed3 = 6f;
    public float rotationSpeed4 = 8f;
    public float rotationSpeed5 = 10f;
    public float rotationSpeed6 = 12f;
    public float rotationSpeed7 = 14f;

    public float shieldDuration = 5f;
    public float Distance = 3f;

    private float timer;
[SerializeField] GameObject Explode;
[SerializeField] Transform prefabExp;
[SerializeField] GameObject Globe1,Globe2,Globe3,Globe4,Globe5,Globe6,Globe7;

    [SerializeField] int damage = 50;
  [Header("Audio")]
    [HideInInspector] public float basePitch = 1f;
    [HideInInspector] public float randomPitchOffset = 0.1f;
    [SerializeField] public AudioClip[] listSound; // array di AudioClip contenente tutti i suoni che si vogliono riprodurre
    private AudioSource[] sgm; // array di AudioSource che conterrà gli oggetti AudioSource creati
    public AudioMixer SFX;
    private bool sgmActive = false;


void Start()
{
   // Move.instance.Evocation();
    Move.instance.Stop();
    timer = shieldDuration;
    sgm = new AudioSource[listSound.Length]; // inizializza l'array di AudioSource con la stessa lunghezza dell'array di AudioClip
        for (int i = 0; i < listSound.Length; i++) // scorre la lista di AudioClip
        {
            sgm[i] = gameObject.AddComponent<AudioSource>(); // crea un nuovo AudioSource come componente del game object attuale (quello a cui è attaccato lo script)
            sgm[i].clip = listSound[i]; // assegna l'AudioClip corrispondente all'AudioSource creato
            sgm[i].playOnAwake = false; // imposto il flag playOnAwake a false per evitare che il suono venga riprodotto automaticamente all'avvio del gioco
            sgm[i].loop = false; // imposto il flag playOnAwake a false per evitare che il suono venga riprodotto automaticamente all'avvio del gioco

        }
 // Aggiunge i canali audio degli AudioSource all'output del mixer
        foreach (AudioSource audioSource in sgm)
        {
        audioSource.outputAudioMixerGroup = SFX.FindMatchingGroups("Master")[0];
        }
}
/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
//Audio
public void PlayMFX(int soundToPlay)
    {
        sgm[soundToPlay].Stop();
        // Imposta la pitch dell'AudioSource in base ai valori specificati.
        sgm[soundToPlay].pitch = basePitch + Random.Range(-randomPitchOffset, randomPitchOffset); 
        sgm[soundToPlay].Play();
    }
/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
//Event
void Update()
{
    // Fluttua attorno al giocatore
     Globe1.transform.position = GameObject.FindWithTag("Player").transform.position + new Vector3(Mathf.Sin(Time.time * rotationSpeed1), Mathf.Cos(Time.time * rotationSpeed1)) * Distance;
     Globe2.transform.position = GameObject.FindWithTag("Player").transform.position + new Vector3(Mathf.Sin(Time.time * rotationSpeed2), Mathf.Cos(Time.time * rotationSpeed2)) * Distance;
     Globe3.transform.position = GameObject.FindWithTag("Player").transform.position + new Vector3(Mathf.Sin(Time.time * rotationSpeed3), Mathf.Cos(Time.time * rotationSpeed3)) * Distance;
     Globe4.transform.position = GameObject.FindWithTag("Player").transform.position + new Vector3(Mathf.Sin(Time.time * rotationSpeed4), Mathf.Cos(Time.time * rotationSpeed4)) * Distance;
     Globe5.transform.position = GameObject.FindWithTag("Player").transform.position + new Vector3(Mathf.Sin(Time.time * rotationSpeed5), Mathf.Cos(Time.time * rotationSpeed5)) * Distance;
     Globe6.transform.position = GameObject.FindWithTag("Player").transform.position + new Vector3(Mathf.Sin(Time.time * rotationSpeed6), Mathf.Cos(Time.time * rotationSpeed6)) * Distance;
     Globe7.transform.position = GameObject.FindWithTag("Player").transform.position + new Vector3(Mathf.Sin(Time.time * rotationSpeed7), Mathf.Cos(Time.time * rotationSpeed7)) * Distance;

    /// Ruota gli sprite attorno al giocatore con velocità diverse
    Globe1.transform.rotation = Quaternion.Euler(0f, 0f, -Mathf.Atan2(Mathf.Cos(Time.time * rotationSpeed1), Mathf.Sin(Time.time * rotationSpeed1)) * Mathf.Rad2Deg);
    Globe2.transform.rotation = Quaternion.Euler(0f, 0f, -Mathf.Atan2(Mathf.Cos(Time.time * rotationSpeed2), Mathf.Sin(Time.time * rotationSpeed2)) * Mathf.Rad2Deg);
    Globe3.transform.rotation = Quaternion.Euler(0f, 0f, -Mathf.Atan2(Mathf.Cos(Time.time * rotationSpeed3), Mathf.Sin(Time.time * rotationSpeed3)) * Mathf.Rad2Deg);
    Globe4.transform.rotation = Quaternion.Euler(0f, 0f, -Mathf.Atan2(Mathf.Cos(Time.time * rotationSpeed4), Mathf.Sin(Time.time * rotationSpeed4)) * Mathf.Rad2Deg);
    Globe5.transform.rotation = Quaternion.Euler(0f, 0f, -Mathf.Atan2(Mathf.Cos(Time.time * rotationSpeed5), Mathf.Sin(Time.time * rotationSpeed5)) * Mathf.Rad2Deg);
    Globe6.transform.rotation = Quaternion.Euler(0f, 0f, -Mathf.Atan2(Mathf.Cos(Time.time * rotationSpeed6), Mathf.Sin(Time.time * rotationSpeed6)) * Mathf.Rad2Deg);
    Globe7.transform.rotation = Quaternion.Euler(0f, 0f, -Mathf.Atan2(Mathf.Cos(Time.time * rotationSpeed7), Mathf.Sin(Time.time * rotationSpeed7)) * Mathf.Rad2Deg);


    // Timer di durata scudo
    timer -= Time.deltaTime;
    if (timer <= 0)
    {
        Instantiate(Explode, transform.position, transform.rotation);
        PlayMFX(0);    
        Destroy(gameObject);
    }
}

void OnTriggerEnter2D(Collider2D other) 
{
    if(other.gameObject.tag == "Enemy" || other.gameObject.tag == "Boss")
    //Se il proiettile tocca il nemico
    {            
        PlayMFX(0);    

        Instantiate(Explode, transform.position, transform.rotation);
        IDamegable hit = other.GetComponent<IDamegable>();
        hit.Damage(damage);
        Destroy(gameObject);
    }
}

}
