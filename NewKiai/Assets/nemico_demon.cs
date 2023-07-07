using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;
using Spine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;
using UnityEngine.UI;

public class nemico_demon : MonoBehaviour
{ 
    [Header("Fatality")]
    public Scrollbar HPBar;
    public Scrollbar staminaBar;
    private float stamina;
    public float stamina_max = 50;
    private float vitalita;
    public float vitalita_max = 200;
    //public float DamageStamina;
    [SerializeField] GameObject Staminaobj;
    [SerializeField] GameObject StaminaVFX;
    private float horizontal;
    private float velocita = 4f;
    private bool bool_dir_dx = true;
    private SkeletonAnimation skeletonAnimation;
    private GameObject GO_player;
    public GameObject Warining;
    public GameObject Aura;
    public float distanza_attacco=8f;
    private float distanza_temp;
    private Vector2 xTarget;
    //private float distanza_contrattacco=5f;

    private bool bool_colpibile=true;
    
    private float tempo_ricolpibile=0.5f;
    private bool bool_morto=false;

    private float tempo_sparo=1f;
    private float tempo_sparo_attuale=0;

    private float tempo_palla_nuova=10f;
    private float tempo_palla_nuova_attuale=0f;

    private float tempo_vulnerabile=5;
    private float tempo_vulnerabile_attuale=0f;

    private bool bool_colpibile_palla=true;
    private float tempo_ricolpibile_palla=0.5f;

    private bool bool_palla_appena_lanciata=false;

    public GameObject palla_fuoco;

    private string stato;
    
    [SerializeField] private Vector3[] posizioni;
    private int index_posizioni;

    [SerializeField] private Rigidbody2D rb;
    
    [Header("Fatality")]
    public GameObject Fatality;
    public GameObject Boss;

 [Header("VFX")]

    [SerializeField] public Transform slashpoint;
    [SerializeField] public Transform hitpoint;
    [SerializeField] GameObject attack;
    [SerializeField] GameObject VFXSdeng;
    [SerializeField] GameObject Noeff;
    [SerializeField] GameObject VFXExplode;
    [SerializeField] GameObject VFXHurt;

    [Header("Audio")]
    [HideInInspector] public float basePitch = 1f;
    [HideInInspector] public float randomPitchOffset = 0.1f;
    [SerializeField] public AudioClip[] listSound; // array di AudioClip contenente tutti i suoni che si vogliono riprodurre
    private AudioSource[] bgm; // array di AudioSource che conterrà gli oggetti AudioSource creati
    public AudioMixer SFX;
    //private bool sgmActive = false;
    // Start is called before the first frame update
    void Start(){
        HPBar.size = vitalita / vitalita_max;
        HPBar.size = Mathf.Clamp(HPBar.size, 0.01f, 1);
        staminaBar.size = stamina / stamina_max;
        staminaBar.size = Mathf.Clamp(staminaBar.size, 0.01f, 1);
        vitalita=vitalita_max;
        stamina=stamina_max;
        palla_fuoco.SetActive(false);
        GO_player=GameObject.Find("Nekotaro");
        skeletonAnimation = GetComponent<SkeletonAnimation>();
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

    void Update(){
        
        staminaBar.size = stamina / stamina_max;
        staminaBar.size = Mathf.Clamp(staminaBar.size, 0.01f, 1);

        HPBar.size = vitalita / vitalita_max;
        HPBar.size = Mathf.Clamp(HPBar.size, 0.01f, 1);

        Staminaobj.gameObject.SetActive(true);
        StaminaVFX.gameObject.SetActive(true);
         if (!GameplayManager.instance.PauseStop)
        {
        if (bool_morto){return;}

        if (tempo_vulnerabile_attuale>0){
            tempo_vulnerabile_attuale-=(1f*Time.deltaTime);
        }

        distanza_temp=calcola_distanza((int)(GO_player.transform.position.x),(int)(GO_player.transform.position.y),(int)(transform.position.x),(int)(transform.position.y));
        //print ("distanza: "+distanza_temp);

        if (tempo_palla_nuova_attuale>0){
            tempo_palla_nuova_attuale-=(1f*Time.deltaTime);
            stato="guardia";
            PlayMFX(3);

        }

        if (tempo_sparo_attuale>0){
            tempo_sparo_attuale-=(1f*Time.deltaTime);
            if (tempo_sparo_attuale>0){return;}
            stato="spara";
            PlayMFX(4);
            bool_palla_appena_lanciata=true;
            StartCoroutine(co_palla_lanciata());
            GameObject go_temp=Instantiate(palla_fuoco,transform);
            go_temp.name="palla_fuoco_demone";
            go_temp.transform.parent = gameObject.transform.parent;
            go_temp.SetActive(true);
            tempo_palla_nuova_attuale+=tempo_palla_nuova;
        }

        //print ("tempi: "+tempo_palla_nuova_attuale+" - "+tempo_sparo_attuale);

        if (distanza_temp<distanza_attacco){
            if ((tempo_palla_nuova_attuale<=0)&&(tempo_sparo_attuale<=0)){
                tempo_sparo_attuale+=tempo_sparo;
                stato="spara";
            } else {
                if (tempo_vulnerabile_attuale>0){
                    stato="stun";
                } 
            }
        }
        else {
            if ((tempo_palla_nuova_attuale<=0)&&(tempo_sparo_attuale<=0)){
                stato="idle";
            }
        }
        //print ("stato: "+stato);
    }}

    private void FixedUpdate(){
         if (!GameplayManager.instance.PauseStop)
        {
        if (bool_morto){return;}

        switch (stato){
            case "idle":{
                Warining.gameObject.SetActive(false);
                Aura.gameObject.SetActive(true);
                skeletonAnimation.AnimationName = "walk";
                transform.position = Vector2.MoveTowards(transform.position,posizioni[index_posizioni], Time.deltaTime*velocita);
                if (transform.position.x<posizioni[index_posizioni].x){horizontal=1;}
                else {horizontal=-1;}
                
                if (transform.position==posizioni[index_posizioni]){
                    if (index_posizioni==posizioni.Length -1){
                        index_posizioni=0;
                    } else {index_posizioni++;}
                }
                Flip();
                break;
            }
            case "spara":{
                Warining.gameObject.SetActive(false);
                Aura.gameObject.SetActive(true);
                skeletonAnimation.AnimationName = "attack_shoot";
                if (transform.position.x<GO_player.transform.position.x){horizontal=1;}
                else {horizontal=-1;}
                Flip();
                break;
            }
            case "guardia":{
                Warining.gameObject.SetActive(false);
                Aura.gameObject.SetActive(true);
                stamina = stamina_max;
                skeletonAnimation.AnimationName = "guard";
                if (transform.position.x<GO_player.transform.position.x){horizontal=1;}
                else {horizontal=-1;}
                Flip();
                break;
            }
            case "stun":{
                skeletonAnimation.AnimationName = "tired";
                stamina = 0;
                Warining.gameObject.SetActive(true);
                Aura.gameObject.SetActive(false);
                break;
            }
        }

        return;
    }}

    void KiaiGive()
{
    int randomChance = Random.Range(1, 10); // Genera un numero casuale compreso tra 1 e 10

    if (randomChance <= 8) // Se il numero casuale è compreso tra 1 e 8 (80% di probabilità), aggiungi 5 di essenza
    {
        PlayerHealth.Instance.currentKiai += 5;
        PlayerHealth.Instance.IncreaseKiai(5);
    }
    else // Se il numero casuale è compreso tra 9 e 10 (20% di probabilità), aggiungi 10 di essenza
    {
        PlayerHealth.Instance.currentKiai += 10;
        PlayerHealth.Instance.IncreaseKiai(10);
    }
}

    void OnTriggerEnter2D(Collider2D col)
    {
        if (bool_morto){return;}
        //Debug.Log("triggo con "+col.name);

    if(col.gameObject.tag == "Hitbox")
        {
           
                if (tempo_vulnerabile_attuale>0){
                    if (bool_colpibile){
                        bool_colpibile=false;
                        print ("colpito dalla mia stessa palla");

                        StartCoroutine(ritorna_ricolpibile());
                        PlayMFX(1);
                        vitalita -= GameplayManager.instance.Damage;
                        skeletonAnimation.Skeleton.SetColor(Color.red);
                        KiaiGive();
                        Instantiate(VFXHurt, hitpoint.position, transform.rotation);
                        StartCoroutine(ripristina_colore());

                        if (vitalita<=0){
                            bool_morto=true;
                            print ("è morto!");
                           bool_morto=true;
                        skeletonAnimation.loop=false;
                        skeletonAnimation.AnimationName="tired";
                        Fatality.gameObject.SetActive(true);
                        Fatality.gameObject.transform.position = Boss.gameObject.transform.position; 
                        if(Boss.transform.localScale.x > 0)
                        {
                        Fatality.transform.localScale = new Vector2(1, 1);
                        }else if(Boss.transform.localScale.x < 0)
                        {
                        Fatality.transform.localScale = new Vector2(-1, 1);
                        }
                        Boss.gameObject.SetActive(false);
                        }
                    }
                } else {
                    print ("è invulnerabile...");
                    Instantiate(Noeff, hitpoint.position, transform.rotation);
                    PlayMFX(5);
                } 
    }
    switch (col.name){
            case "palla_fuoco_demone":{
                //print (bool_colpibile_palla+" - "+bool_palla_appena_lanciata);
                if (bool_colpibile_palla){
                    if (!bool_palla_appena_lanciata){
                        bool_colpibile_palla=false;
                        StartCoroutine(ritorna_ricolpibile_palla());
                        Instantiate(VFXExplode, hitpoint.position, transform.rotation);
                        PlayMFX(3);
                        print ("colpito dalla mia stessa palla");
                        tempo_vulnerabile_attuale=tempo_vulnerabile;
                    }
                }
                break;
            }      
        }
    }

    private IEnumerator ripristina_colore(){
        yield return new WaitForSeconds(0.1f);
        skeletonAnimation.Skeleton.SetColor(Color.white);
    }

    private IEnumerator rimuovi(){
        yield return new WaitForSeconds(2f);
        Destroy(gameObject);
    }

    private IEnumerator ritorna_ricolpibile(){    
        yield return new WaitForSeconds(tempo_ricolpibile);
        bool_colpibile=true;
    }

    private IEnumerator co_palla_lanciata(){    
        yield return new WaitForSeconds(0.5f);
        bool_palla_appena_lanciata=false;
    }

    private IEnumerator ritorna_ricolpibile_palla(){    
        yield return new WaitForSeconds(tempo_ricolpibile_palla);
        bool_colpibile_palla=true;
    }

    private void Flip()
    {
        if (bool_dir_dx && horizontal > 0f || !bool_dir_dx && horizontal < 0f)
        {
            bool_dir_dx = !bool_dir_dx;
            Vector3 localScale = transform.localScale;
            localScale.x *= -1f;
            transform.localScale = localScale;
        }
    }
    
    private float calcola_distanza(int xor, int yor, int xar, int yar){
        float distanza=0f;
        int dist_x=Mathf.Abs(xor - xar);
        int dist_y=Mathf.Abs(yor - yar);
        distanza=Mathf.Sqrt((dist_x*dist_x) + (dist_y*dist_y));
        return distanza;
    }

    public void PlayMFX(int soundToPlay)
    {
        bgm[soundToPlay].Stop();
        // Imposta la pitch dell'AudioSource in base ai valori specificati.
        bgm[soundToPlay].pitch = basePitch + Random.Range(-randomPitchOffset, randomPitchOffset); 
        bgm[soundToPlay].Play();
    }
    

}
