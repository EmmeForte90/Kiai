using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;
using Spine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;

public class nemico_katana : MonoBehaviour
{
    private float horizontal;
    private float velocita = 4f;
    private float velocita_corsa = 6f;
    private bool bool_dir_dx = true;
    private SkeletonAnimation skeletonAnimation;
    public GameObject GO_player;
    public float distanza_guardia=2f;
    public float distanza_attacco=0.5f;
    private float distanza_temp;
    private Vector2 xTarget;

    // valori per gli stessi ma con la stamina
    private float velocita_ricarica_stamina=1;
    private float stamina;
    public float stamina_max=50;
    private float tempo_contrattacco=0;
    private float tempo_ritorna_idle=0;

    public stamina_vfx_rule stamina_vfx_rule;

    private bool bool_colpibile=true;
    private int vitalita;
    private int vitalita_max=50;
    private float tempo_ricolpibile=0.5f;
    private bool bool_morto=false;

    private float tempo_attuale_guardia=0;

    private string stato;

    private float tempo_stanchezza;

    public float tempo_attacco=0.3f;
    
    [SerializeField] private Vector3[] posizioni;
    private int index_posizioni;

    [SerializeField] private Rigidbody2D rb;

    public float tempo_knockback = 1f;
    private float tempo_knockback_attuale=0;
    private Vector2 direzione_knockback;
    public float forza_knockback=100f;

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

[Header("Drop")]
    public GameObject coinPrefab; // prefab per la moneta
    private bool  SpawnC = false;
    [SerializeField] public Transform CoinPoint;
    public int maxCoins = 5; // numero massimo di monete che possono essere rilasciate
    public float coinSpawnDelay = 5f; // ritardo tra la spawn di ogni moneta
    private int randomChance;
    private float coinForce = 5f; // forza con cui le monete saltano
    private Vector2 coinForceVariance = new Vector2(1, 0); // varianza della forza con cui le monete saltano
    private int coinCount; // conteggio delle monete

    // Start is called before the first frame update
    void Start(){
        stamina=stamina_max;
        GO_player=GameObject.Find("Nekotaro");
        skeletonAnimation = GetComponent<SkeletonAnimation>();
        vitalita=vitalita_max;
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
public void SpawnCoins()
{
    if(!SpawnC)
    {

    for (int i = 0; i < maxCoins; i++)
    {
        // crea una nuova moneta
        GameObject newCoin = Instantiate(coinPrefab, CoinPoint.position, Quaternion.identity);

        // applica una forza casuale alla moneta per farla saltare
        Vector2 randomForce = new Vector2(
            Random.Range(-coinForceVariance.x, coinForceVariance.x), 2);
        newCoin.GetComponent<Rigidbody2D>().AddForce(randomForce * coinForce, ForceMode2D.Impulse);
    }
        SpawnC = true;
    }
}
    void Update(){
         if (!GameplayManager.instance.PauseStop)
        {
        if (bool_morto){return;}
        if (tempo_knockback_attuale>0){
            rb.AddForce(direzione_knockback*forza_knockback);
            tempo_knockback_attuale-=(1*Time.deltaTime);
        }
        if (tempo_contrattacco>0){
            tempo_contrattacco-=(1*Time.deltaTime);
            //print ("tempo contrattacco: "+tempo_contrattacco);
        }
        if (stamina_max>0){stamina_vfx_rule.scala_GO_stamina(stamina,stamina_max);}
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
        if (tempo_stanchezza>0){
            //print ("stanchezza: "+tempo_stanchezza);
            tempo_stanchezza-=(1f*Time.deltaTime);
            if (tempo_stanchezza>0){return;}
            stato="idle";
        }

        distanza_temp=calcola_distanza((int)(GO_player.transform.position.x),(int)(GO_player.transform.position.y),(int)(transform.position.x),(int)(transform.position.y));
        //print ("distanza: "+distanza_temp);

        if (distanza_temp<distanza_guardia){
            if (stato=="idle"){
                stato="guardia";
                tempo_attuale_guardia=1;
            }
            if (stato=="guardia"){
                tempo_attuale_guardia-=(1f*Time.deltaTime);
                if (tempo_attuale_guardia<=0){
                    if (stamina_max>0){
                        if (stamina>=20){
                            stamina-=20;
                            stamina_vfx_rule.stamina_zero(stamina);
                            stato="puo_attaccare";
                        }
                    }
                    else {stato="puo_attaccare";}
                }
            }
            if (stato=="puo_attaccare"){
                if (distanza_temp<distanza_attacco){
                    stato="attacco";
                }
            }
        }
        else {
            stato="idle";
        }
    }}

    private IEnumerator ferma_attacco(){    
        yield return new WaitForSeconds(tempo_attacco);
        //print ("fermo attacco");
        stato="tired";
        tempo_stanchezza=2.5f;
    }


    private void FixedUpdate()
    {
         if (!GameplayManager.instance.PauseStop)
        {
        if (bool_morto){return;}
        //print ("stato: "+stato);
        if (tempo_stanchezza>0){
            if (stato=="tired"){
                if ((stamina_max==0)||(stamina>0)){
                    skeletonAnimation.AnimationName = "idle_battle";
                }
                else {skeletonAnimation.AnimationName = "tired";}
            } else if (stato=="guardia"){
                skeletonAnimation.AnimationName = "guard";
            }
            return; 
        }

        switch (stato){
            case "contrattacco":{
                if (transform.position.x<GO_player.transform.position.x){horizontal=1;}
                else {horizontal=-1;}
                skeletonAnimation.AnimationName = "vertical_up/attack_vertical_up";
                PlayMFX(0);
                Flip();
                break;  
            }
            case "attacco":{
                skeletonAnimation.AnimationName = "vertical_bottom/attack_vertical_bottom";
                PlayMFX(0);
                StartCoroutine(ferma_attacco());
                break;  
            }
            case "idle":{
                skeletonAnimation.AnimationName = "walk_thief";
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
            case "guardia":{
                if (transform.position.x<GO_player.transform.position.x){horizontal=1;}
                else {horizontal=-1;}
                //skeletonAnimation.AnimationName = "idle_battle";
                skeletonAnimation.AnimationName = "guard";
                Flip();
                break;
            }
            case "puo_attaccare":{
                if (transform.position.x<GO_player.transform.position.x){horizontal=1;}
                else {horizontal=-1;}
                skeletonAnimation.AnimationName = "run_thief";
                xTarget = new Vector2(GO_player.transform.position.x, transform.position.y);
                transform.position = Vector2.MoveTowards(transform.position,xTarget, Time.deltaTime*velocita_corsa);
                Flip();
                break;
            }
        }

        return;
    }}

    void OnTriggerEnter2D(Collider2D col)
    {
        if (bool_morto){return;}
        //Debug.Log("triggo con "+col.name);
        switch (col.name){
            case "Hitbox":{
                if (bool_colpibile){
                    bool_colpibile=false;
                    StartCoroutine(ritorna_ricolpibile());

                    if (stamina_max>0){//vuol dire che è un nemico con stamina
                        if (stamina>5){
                            stamina-=5;
                            stamina_vfx_rule.stamina_zero(stamina);
                            tempo_contrattacco+=1.8f;
                            print ("tempo contrattacco: "+tempo_contrattacco);
                            if (tempo_contrattacco>=3){
                                stato="contrattacco";
                                tempo_ritorna_idle=0.3f;
                                return;   //senza questo return, quando lui contrattacca, potrebbe essere ancora colpito; Mauro
                            } else {
                                print ("paro. Stamina: "+stamina);
                                Instantiate(VFXSdeng, hitpoint.position, transform.rotation);
                                PlayMFX(2);
                                stato="guardia";
                                tempo_ritorna_idle=1;
                                return;
                            }
                        }
                    }

                    vitalita-=10;

                    skeletonAnimation.Skeleton.SetColor(Color.red);
                    KiaiGive();
                    PlayMFX(1);
                    Instantiate(VFXHurt, hitpoint.position, transform.rotation);
                    StartCoroutine(ripristina_colore());

                    direzione_knockback = (transform.position - GO_player.transform.position).normalized;
                    tempo_knockback_attuale+=tempo_knockback;

                    /*  transizionale
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
                        SpawnCoins();
                        skeletonAnimation.loop=false;
                        skeletonAnimation.AnimationName="die1";
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
