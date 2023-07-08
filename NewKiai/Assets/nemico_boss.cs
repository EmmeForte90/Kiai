using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;
using Spine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;
using UnityEngine.UI;

public class nemico_boss : MonoBehaviour
{
    public static nemico_boss instance;

    

    private float tempo_ricolpibile=0.5f;
    private bool bool_arrabbiato=false;
    private bool bool_colpibile=true;
    public GameObject pietra_lancio_pf;
    public pietre_terreno pietre_terreno;

    private float horizontal;
   // private float velocita = 4f;
    //private float velocita_corsa = 6f;
    private bool bool_dir_dx = true;
    private SkeletonAnimation skeletonAnimation;
    public GameObject GO_player;
   // public float distanza_guardia=2f;
  //  public float distanza_attacco=0.5f;
   // private float distanza_temp;
  //  private Vector2 xTarget;


    private int num_salti_totale=3;
    private int num_salti_attuale=0;

    private int num_pietre_lanciate_totale=3;
    private int num_pietre_lanciate_attuale=0;

    private float tempo_rage=1f;
    private float tempo_rage_attuale=0;

    private bool bool_morto=false;

    private string stato;
    [SerializeField] private Rigidbody2D rb;

    private float tempo_salto=1f;
    private float tempo_salto_attuale=0;
    private float tempo_lancio_pietra=2;
    private float tempo_lancio_pietra_attuale=0;

    private float tempo_riposo_salto=1f;
    private float tempo_riposo_salto_attuale=0;
    private float tempo_riposo_lancio_pietre=1f;
    private float tempo_riposo_lancio_pietre_attuale=0;

    private float tempo_riposo_attacco_attuale=3f;

    private float tempo_anim_attacco_mazza=2.1f;
    private float tempo_anim_attacco_mazza_attuale=0;

    private float tempo_anim_attacco_lancio_pietre=0.8f;
    private float tempo_anim_attacco_lancio_pietre_attuale=0;

    public float tempo_riposo_attacco_salti=3;
    public float tempo_riposo_attacco_mazza=3;
    public float tempo_riposo_attacco_pietre=3;

    public float tempo_riposo_attacco_salti_arrabbiato=2;
    public float tempo_riposo_attacco_mazza_arrabbiato=2;
    public float tempo_riposo_attacco_pietre_arrabbiato=2;



    private string attacco_tipo;

    //funzioni relativi alla parabola di salto
    private float t=10;
    private float salto_attivo=0;   //il valore và da 0 a 1
    private Vector3 origione_salto;
    private Vector3 destinazione_salto;
    private Vector3 destinazione_salto_media;
    private float x_destinazione_salto;
    private int i;
    private float j;

 [Header("VFX")]

   // [SerializeField] public Transform slashpoint;
    [SerializeField] public Transform hitpoint;
   // [SerializeField] GameObject attack;
    // [SerializeField] GameObject VFXSdeng;
    [SerializeField] GameObject VFXHurt;
 [Header("Audio")]
    [HideInInspector] public float basePitch = 1f;
    [HideInInspector] public float randomPitchOffset = 0.1f;
    [SerializeField] public AudioClip[] listSound; // array di AudioClip contenente tutti i suoni che si vogliono riprodurre
    private AudioSource[] bgm; // array di AudioSource che conterrà gli oggetti AudioSource creati
    public AudioMixer SFX;
   // private bool sgmActive = false;

[Header("Stallo")]
    public GameObject Stallo;
    public GameObject ActorStallo;

[Header("Fatality")]
    public GameObject Fatality;
[Header("Boss")]
    public GameObject Boss;
    public GameObject ActorBoss;

    void Start(){
         if (instance == null)
        {
            instance = this;
        }
        GO_player=GameObject.Find("Nekotaro");
        skeletonAnimation = GetComponent<SkeletonAnimation>();
        attacco_tipo="mazza";
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
    void Update(){
         if (!GameplayManager.instance.PauseStop)
        {
        if (bool_morto){return;}

        if (transform.position.x<GO_player.transform.position.x){horizontal=1;}
        else {horizontal=-1;}
        Flip();

        if (tempo_rage_attuale>0){
            tempo_rage_attuale-=(1f*Time.deltaTime);
            if (tempo_rage_attuale<=0){
                stato="tired";
            }
            return;
        }

        if (tempo_anim_attacco_mazza_attuale>0){
            tempo_anim_attacco_mazza_attuale-=(1f*Time.deltaTime);
            if (tempo_anim_attacco_mazza_attuale<=0){
                tempo_riposo_attacco_attuale=tempo_riposo_attacco_mazza;
                stato="tired";
            }
            return;
        }

        if (tempo_anim_attacco_lancio_pietre_attuale>0){
            tempo_anim_attacco_lancio_pietre_attuale-=(1f*Time.deltaTime);
            if (tempo_anim_attacco_lancio_pietre_attuale<=0){
                stato="tired";
            }
            return;
        }

        if (tempo_riposo_salto_attuale>0){
            tempo_riposo_salto_attuale-=(1f*Time.deltaTime);
            return;
        }

        if (tempo_riposo_lancio_pietre_attuale>0){
            tempo_riposo_lancio_pietre_attuale-=(1f*Time.deltaTime);
            return;
        }

        if (tempo_riposo_attacco_attuale>0){
            tempo_riposo_attacco_attuale-=(1f*Time.deltaTime);
            if (tempo_riposo_attacco_attuale>0){return;}

            switch (Random.Range(1,4)){
                case 1:{attacco_tipo="salti";break;}
                case 2:{attacco_tipo="mazza";break;}
                case 3:{attacco_tipo="pietre";break;}
            }
            //attacco_tipo="salti"; //debug
        }

        if (tempo_salto_attuale>0){
            salto_attivo=1-(tempo_salto_attuale/tempo_salto);
            if (tempo_salto_attuale<0){tempo_salto_attuale=0;}

            if (salto_attivo>=0.9f){
                if (stato!="schiacciare"){
                    stato="schiacciare";
                    StartCoroutine(termina_salto_crush());
                }
                return;
            }
            tempo_salto_attuale-=(1f*Time.deltaTime);
            transform.position=punto_parabola(origione_salto,destinazione_salto,destinazione_salto_media,t,salto_attivo);


            if (tempo_salto_attuale==0){
                tempo_riposo_salto_attuale=tempo_riposo_salto;
            }
            return;
        }

        if (tempo_lancio_pietra_attuale>0){
            tempo_lancio_pietra_attuale-=(1f*Time.deltaTime);

            if (tempo_lancio_pietra_attuale==0){
                tempo_riposo_lancio_pietre_attuale=tempo_riposo_lancio_pietre;
            }
            return;
        }

        switch (attacco_tipo){
            case "salti":{
                if (num_salti_attuale<num_salti_totale){
                    origione_salto=transform.position;
                    x_destinazione_salto=Random.Range(5,12);
                    x_destinazione_salto*=horizontal;
                    destinazione_salto=new Vector3((origione_salto.x+x_destinazione_salto),transform.position.y,transform.position.z);
                    destinazione_salto_media=transform.position+(destinazione_salto-transform.position)/2 +Vector3.up *t;

                    tempo_salto_attuale=tempo_salto;
                    stato="salti";
                    num_salti_attuale++;

                } else {
                    num_salti_attuale=0;
                    tempo_riposo_attacco_attuale=tempo_riposo_attacco_salti;
                    stato="tired";
                }
                break;
            }
            case "mazza":{
                stato="mazza";
                tempo_anim_attacco_mazza_attuale=tempo_anim_attacco_mazza;
                if (!bool_arrabbiato){
                    StartCoroutine(genera_pietre_suolo(1.75f));
                } else {
                    StartCoroutine(genera_pietre_suolo(1.5f));
                    //StartCoroutine(genera_pietre_suolo(2.85f));
                    StartCoroutine(genera_pietre_suolo(3.5f));
                }
                break;
            }
            case "pietre":{
                if (num_pietre_lanciate_attuale<num_pietre_lanciate_totale){
                    tempo_anim_attacco_lancio_pietre_attuale=tempo_anim_attacco_lancio_pietre;
                    tempo_lancio_pietra_attuale=tempo_lancio_pietra;
                    stato="lancio_pietre";
                    num_pietre_lanciate_attuale++;

                    StartCoroutine(lancia_pietra());
                } else {
                    print ("ho finito il lanciare pietre");
                    num_pietre_lanciate_attuale=0;
                    tempo_riposo_attacco_attuale=tempo_riposo_attacco_pietre;
                    stato="tired";
                }
                break;
            }}}}
void HandleEvent (TrackEntry trackEntry, Spine.Event e) 
    {
        if (e.Data.Name == "SpawnRock") 
    {
        PlayMFX(2);
    }
    }

/////////////////////////////////////////////////////////////////////////////////////////
//SS

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

/////////////////////////////////////////////////////////////////////////////////////////

    private void FixedUpdate(){
         if (!GameplayManager.instance.PauseStop)
        {
        if (bool_morto){return;}
        switch (stato){
            case "idle":{
                skeletonAnimation.AnimationName="battle/idle_battle";
                break;
            }
            case "schiacciare":{
                skeletonAnimation.AnimationName="battle/Jump/jump_crush";
                PlayMFX(2);
                break;
            }
            case "salti":{
                skeletonAnimation.AnimationName="battle/Jump/jump_loop";
                break;
            }
            case "mazza":{
                if (!bool_arrabbiato){
                    skeletonAnimation.AnimationName="battle/attack_power/attack_power_with_wait";
                } else {
                    skeletonAnimation.AnimationName="battle/attack_power/attack_power_3times";
                }
                break;
            }
            case "tired":{
                skeletonAnimation.AnimationName="battle/idle_battle";
                break;
            }
            case "lancio_pietre":{
                skeletonAnimation.AnimationName="battle/throw";
                break;
            }
            case "rage":{
                skeletonAnimation.AnimationName="battle/rage";
                break;
            }
            
        }
    }
    }

    private IEnumerator termina_salto_crush(){
        yield return new WaitForSeconds(0.5f);
        tempo_salto_attuale=0;
        tempo_riposo_salto_attuale=tempo_riposo_salto;
        stato="idle";
    }

    private IEnumerator lancia_pietra(){
        yield return new WaitForSeconds(0.6f);
        if (tempo_rage_attuale<=0){
            if (!bool_morto){
                float xor=transform.position.x;
                float yor=transform.position.y;
                if (horizontal<0){xor-=2;} else {xor+=2;}
                yor+=1.5f;
                GameObject go_temp=Instantiate(pietra_lancio_pf);
                go_temp.transform.position=new Vector3(xor,yor,transform.position.z);
            }
        }
    }

    private IEnumerator genera_pietre_suolo(float tempo){
        yield return new WaitForSeconds(tempo);
        if (tempo_rage_attuale<=0){
            if (!bool_morto){
                pietre_terreno.resetta();
                float xor=transform.position.x;
                if (horizontal<0){xor-=4;} else {xor+=4;}
                for (int i=1;i<=5;i++){
                    j=xor-5;
                    j+=(i*1.5f);
                    j-=1.5f;
                    j+=Random.Range(0f,3f);
                    
                    pietre_terreno.aggiungi(i, xor, transform.position.y, j, transform.position.y, transform.position.z);
                }
                pietre_terreno.avvia();
            }
        }
    }

    void OnTriggerEnter2D(Collider2D col){
        if (bool_morto){return;}

 if(col.gameObject.tag == "Hitbox")
        {

 if (bool_colpibile){
                    bool_colpibile=false;
                    StartCoroutine(ritorna_ricolpibile());

                    TriggerBoss.instance.vitalita -= GameplayManager.instance.Damage;

                    skeletonAnimation.Skeleton.SetColor(Color.red);
                    KiaiGive();
                    PlayMFX(1);
                    Instantiate(VFXHurt, hitpoint.position, transform.rotation);
                    StartCoroutine(ripristina_colore());

                    if (!bool_arrabbiato){
                        if (TriggerBoss.instance.vitalita<(TriggerBoss.instance.vitalita_max/2)){
                            bool_arrabbiato=true;
                            tempo_riposo_attacco_salti=tempo_riposo_attacco_salti_arrabbiato;
                            tempo_riposo_attacco_mazza=tempo_riposo_attacco_mazza_arrabbiato;
                            tempo_riposo_attacco_pietre=tempo_riposo_attacco_pietre_arrabbiato;

                            num_salti_totale=4;
                            num_pietre_lanciate_totale=4;

                            tempo_riposo_lancio_pietre/=2;
                            tempo_riposo_salto/=2;
                            tempo_lancio_pietra/=2;

                            tempo_anim_attacco_mazza=4.2f;

                            tempo_rage_attuale+=tempo_rage;
                            stato="rage";
                            Stallo.gameObject.SetActive(true);
                            ActorStallo.gameObject.transform.position = ActorBoss.gameObject.transform.position;
                            Boss.gameObject.SetActive(false);

                        }
                    }

                    if (TriggerBoss.instance.vitalita<=0){
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
    }}
    public void ColorChange(){
        StartCoroutine(ripristina_colore());
    }
    private IEnumerator ripristina_colore(){
        yield return new WaitForSeconds(0.1f);
        skeletonAnimation.Skeleton.SetColor(Color.white);
    }

    private IEnumerator rimuovi(){
        yield return new WaitForSeconds(10f);
         print ("è morto!");
        skeletonAnimation.loop=false;
        skeletonAnimation.AnimationName="die_back";
        Destroy(gameObject);
    }

    private IEnumerator ritorna_ricolpibile(){    
        yield return new WaitForSeconds(tempo_ricolpibile);
        bool_colpibile=true;
    }

    private Vector3 punto_parabola(Vector3 start_point, Vector3 end_point, Vector3 mid_point, float t, float count){
        Vector3 vor=start_point;
        Vector3 var=end_point;
        Vector3 vin=mid_point;
        Vector3 m1,m2,v_temp;

        m1 = Vector3.Lerp(vor, vin, count);
        m2 = Vector3.Lerp(vin, var, count);
        v_temp = Vector3.Lerp(m1, m2, count);
        return v_temp;
    }

    private void Flip(){
        if (bool_dir_dx && horizontal > 0f || !bool_dir_dx && horizontal < 0f)
        {
            bool_dir_dx = !bool_dir_dx;
            Vector3 localScale = transform.localScale;
            localScale.x *= -1f;
            transform.localScale = localScale;
        }
    }

    public void PlayMFX(int soundToPlay)
    {
        bgm[soundToPlay].Stop();
        // Imposta la pitch dell'AudioSource in base ai valori specificati.
        bgm[soundToPlay].pitch = basePitch + Random.Range(-randomPitchOffset, randomPitchOffset); 
        bgm[soundToPlay].Play();
    }

}
