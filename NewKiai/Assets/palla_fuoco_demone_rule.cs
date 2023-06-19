using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;
using Spine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;
using UnityEngine.UI;

public class palla_fuoco_demone_rule : MonoBehaviour
{
    public float velocita_palla_di_fuoco = 5f;
    public float tempo_distruzione_sola = 5f;
    private GameObject target;
    public Rigidbody2D rb;
    public SpriteRenderer sprite;
    private bool bool_distrutta=false;

    private bool bool_palla_appena_lanciata=true;
 [Header("VFX")]

    [SerializeField] GameObject VFXExp;
[Header("Audio")]
    [HideInInspector] public float basePitch = 1f;
    [HideInInspector] public float randomPitchOffset = 0.1f;
    [SerializeField] public AudioClip[] listSound; // array di AudioClip contenente tutti i suoni che si vogliono riprodurre
    private AudioSource[] bgm; // array di AudioSource che conterrà gli oggetti AudioSource creati
    public AudioMixer SFX;
    private bool sgmActive = false;
    // Start is called before the first frame update
    void Start()
    {
        target = GameObject.Find("Nekotaro");
        StartCoroutine(distruggi_col_tempo());
        StartCoroutine(co_palla_lanciata());
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

    // Update is called once per frame
    void Update()
    {
        Vector3 direction = (target.transform.position - transform.position).normalized;
        transform.position += (direction * (velocita_palla_di_fuoco * Time.deltaTime));
    }

    void OnTriggerEnter2D(Collider2D other) {
        if(other.gameObject.name == "Nekotaro"){
            print ("colpito il personaggio!");
            distruggi_palla_di_fuoco();
        }
        else if(other.gameObject.name == "Demon"){
            if (!bool_palla_appena_lanciata){
                print ("colpito il demone!");
                distruggi_palla_di_fuoco();
                PlayMFX(0);
                Instantiate(VFXExp, transform.position, transform.rotation);

            }
        }
    }
 public void PlayMFX(int soundToPlay)
    {
        bgm[soundToPlay].Stop();
        // Imposta la pitch dell'AudioSource in base ai valori specificati.
        bgm[soundToPlay].pitch = basePitch + Random.Range(-randomPitchOffset, randomPitchOffset); 
        bgm[soundToPlay].Play();
    }
    private IEnumerator distruggi_col_tempo(){
        yield return new WaitForSeconds(tempo_distruzione_sola);
        distruggi_palla_di_fuoco();
    }

    private void distruggi_palla_di_fuoco(){
        if (bool_distrutta){return;}
        bool_distrutta=true;
        Destroy(gameObject);
    }

    private IEnumerator co_palla_lanciata(){    
        yield return new WaitForSeconds(0.5f);
        bool_palla_appena_lanciata=false;
    }
}
