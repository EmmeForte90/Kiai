using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;
using Spine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;

public class nemico_bigkatana : MonoBehaviour
{
    public stamina_vfx_rule stamina_vfx_rule;

    private float horizontal;
    private float velocita = 4f;
    private float velocita_corsa = 2f;
    private bool bool_dir_dx = true;
    private SkeletonAnimation skeletonAnimation;
    public GameObject GO_player;
    public float distanza_attacco=3f;
    private float distanza_temp;
    private Vector2 xTarget;

    private float velocita_ricarica_stamina=2;
    private float stamina;
    private float stamina_max=50;

    private float tempo_contrattacco=0;
    private float tempo_ritorna_idle=0;

    private bool bool_colpibile=true;
    private int vitalita;
    private int vitalita_max=200;
    private float tempo_ricolpibile=0.2f;
    private bool bool_morto=false;

    private string stato;
    
    [SerializeField] private Vector3[] posizioni;
    private int index_posizioni;

    [SerializeField] private Rigidbody2D rb;
    // Start is called before the first frame update
 [Header("VFX")]

    [SerializeField] public Transform slashpoint;
    [SerializeField] public Transform hitpoint;
    [SerializeField] GameObject attack;
     [SerializeField] GameObject VFXSdeng;
    [SerializeField] GameObject VFXHurt;
 [Header("Audio")]
    [HideInInspector] public float basePitch = 1f;
    [HideInInspector] public float randomPitchOffset = 0.1f;
    [SerializeField] public AudioClip[] listSound; // array di AudioClip contenente tutti i suoni che si vogliono riprodurre
    private AudioSource[] bgm; // array di AudioSource che conterrà gli oggetti AudioSource creati
    public AudioMixer SFX;
    private bool sgmActive = false;

    void Start(){
        stamina=stamina_max;
        vitalita=vitalita_max;
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
         if (!GameplayManager.instance.PauseStop)
        {
        if (bool_morto){return;}
        if (stamina_max>0){stamina_vfx_rule.scala_GO_stamina(stamina,stamina_max);}
        if (tempo_contrattacco>0){
            tempo_contrattacco-=(1*Time.deltaTime);
            //print ("tempo contrattacco: "+tempo_contrattacco);
        }
        if (stamina<stamina_max){
            stamina+=(velocita_ricarica_stamina*Time.deltaTime);
            //print ("stamina: "+stamina);
        }
        if (tempo_ritorna_idle>0){
            tempo_ritorna_idle-=(1*Time.deltaTime);
            if (tempo_ritorna_idle<=0){
                tempo_ritorna_idle=0;
                stato="idle";
            }
        }

        if ((stato=="attacco_grosso")||(stato=="contrattacco")){return;}

        distanza_temp=calcola_distanza((int)(GO_player.transform.position.x),(int)(GO_player.transform.position.y),(int)(transform.position.x),(int)(transform.position.y));
        //print ("distanza: "+distanza_temp);

        if (distanza_temp<distanza_attacco){
            if (stato=="idle"){
                if (stamina>=30){
                    stato="attacco_grosso";
                    stamina-=30;
                    stamina_vfx_rule.stamina_zero(stamina);
                    tempo_ritorna_idle+=2.5f;
                } else {
                    stato="guardia";
                }
                /*
                else {
                    stato="puo_attaccare_semplice";
                }
                */
                
            }
        }
        else {
            stato="idle";
        }
    }}


    private void FixedUpdate()
    { 
        if (!GameplayManager.instance.PauseStop)
        {
        if (bool_morto){return;}

        switch (stato){
            case "attacco_grosso":{
                skeletonAnimation.AnimationName = "attack_power/attack_power";
                PlayMFX(0);
                if (transform.position.x<GO_player.transform.position.x){horizontal=1;}
                else {horizontal=-1;}
                //xTarget = new Vector2(GO_player.transform.position.x, transform.position.y);
                //transform.position = Vector2.MoveTowards(transform.position,xTarget, Time.deltaTime*velocita_corsa);
                Flip();
                break;  
            }
            case "idle":{
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
            case "puo_attaccare":{
                if (transform.position.x<GO_player.transform.position.x){horizontal=1;}
                else {horizontal=-1;}
                skeletonAnimation.AnimationName = "walk";
                break;
            }
            case "contrattacco":{
                if (transform.position.x<GO_player.transform.position.x){horizontal=1;}
                else {horizontal=-1;}
                skeletonAnimation.AnimationName = "attack_horizontal/attack_horizontal";
                PlayMFX(0);
                Flip();
                break;
            }
            case "guardia":{
                if (transform.position.x<GO_player.transform.position.x){horizontal=1;}
                else {horizontal=-1;}
                skeletonAnimation.AnimationName = "guard";
                Flip();
                break;
            }
        }

        return;
    }}

    void OnTriggerEnter2D(Collider2D col){
        if (bool_morto){return;}
        //Debug.Log("triggo con "+col.name);
        switch (col.name){
            case "Hitbox":{
                if (stato=="contrattacco"){return;}
                if (bool_colpibile){
                    Move.instance.KnockbackS();            
                    bool_colpibile=false;
                    StartCoroutine(ritorna_ricolpibile());

                    //dopodicchè dobbiamo caricare il numero di volte che è stato colpito
                    tempo_contrattacco+=1.8f;
                    //print ("tempo contrattacco: "+tempo_contrattacco);
                    if (tempo_contrattacco>=3){
                        print ("devo contrattaccare");
                        stato="contrattacco";
                        PlayMFX(0);
                        tempo_ritorna_idle=1;
                        //return;   //senza questo return, quando lui contrattacca, potrebbe essere ancora colpito; Mauro
                    } else {

                        //dobbiamo vedere quì se ha la stamina intanto per pararsi
                        if (stamina>5){
                            stamina-=5;
                            stamina_vfx_rule.stamina_zero(stamina);
                            stato="guardia";
                            Instantiate(VFXSdeng, slashpoint.position, transform.rotation);
                            PlayMFX(2);
                            print ("devo pararmi");
                            tempo_ritorna_idle=1;
                            return;
                        }
                    }
                    vitalita-=10;

                    print ("colpito! vitalita: "+vitalita);

                    skeletonAnimation.Skeleton.SetColor(Color.red);
                    KiaiGive();
                    PlayMFX(1);
                    Instantiate(VFXHurt, hitpoint.position, transform.rotation);
                    StartCoroutine(ripristina_colore());

                    /*  //lui non và in knockback...
                    float posizione_x=transform.position.x;
                    if (GO_player.transform.position.x<transform.position.x){posizione_x+=1.5f;}
                    else {posizione_x-=1.5f;}
                    iTween.MoveTo(
                        this.gameObject, iTween.Hash(
                            "position",new Vector3(
                                posizione_x, transform.position.y,transform.position.z
                            ),"time", 0.3f, "easetype", iTween.EaseType.easeOutSine
                        )
                    );
                    */

                    if (vitalita<=0){
                        bool_morto=true;
                        print ("è morto!");
                        skeletonAnimation.loop=false;
                        skeletonAnimation.AnimationName="die_back";
                        StartCoroutine(rimuovi());
                    }
                }
                break;
            }
        }
    }
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
