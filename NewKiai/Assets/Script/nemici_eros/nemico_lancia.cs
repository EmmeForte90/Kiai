using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine;
using Spine.Unity;

public class nemico_lancia : MonoBehaviour
{
    private float horizontal;
    private float velocita = 4f;
    private float velocita_corsa = 6f;
    private bool bool_dir_dx = true;
    private SkeletonAnimation skeletonAnimation;
    public GameObject GO_player;
    public float distanza_guardia=7f;
    public float distanza_attacco=3f;
    private float distanza_temp;
    private Vector2 xTarget;

    private bool bool_colpibile=true;
    private int vitalita;
    private int vitalita_max=100;
    private float tempo_ricolpibile=0.5f;
    private bool bool_morto=false;

    private float tempo_attuale_guardia=0;

    private string stato;

    private float tempo_stanchezza;
    
    [SerializeField] private Vector3[] posizioni;
    private int index_posizioni;

    [SerializeField] private Rigidbody2D rb;
    // Start is called before the first frame update
    void Start(){
        GO_player=GameObject.Find("Nekotaro");
        skeletonAnimation = GetComponent<SkeletonAnimation>();
        vitalita=vitalita_max;
    }

    void Update(){
        if (bool_morto){return;}
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
                    stato="puo_attaccare";
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
    }

    private IEnumerator ferma_attacco(){    
        yield return new WaitForSeconds(1f);
        stato="tired";
        tempo_stanchezza=2.5f;
    }


    private void FixedUpdate()
    {
        if (bool_morto){return;}
        //print ("stato: "+stato);
        if (tempo_stanchezza>0){
            if (stato=="tired"){
                skeletonAnimation.AnimationName = "idle_battle";
            }
            return; 
        }

        switch (stato){
            case "attacco":{
                skeletonAnimation.AnimationName = "attack_lunge/attack_lunge";
                StartCoroutine(ferma_attacco());
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
            case "guardia":{
                if (transform.position.x<GO_player.transform.position.x){horizontal=1;}
                else {horizontal=-1;}
                skeletonAnimation.AnimationName = "idle_battle";
                Flip();
                break;
            }
            case "puo_attaccare":{
                if (transform.position.x<GO_player.transform.position.x){horizontal=1;}
                else {horizontal=-1;}
                skeletonAnimation.AnimationName = "run";
                xTarget = new Vector2(GO_player.transform.position.x, transform.position.y);
                transform.position = Vector2.MoveTowards(transform.position,xTarget, Time.deltaTime*velocita_corsa);
                Flip();
                break;
            }
        }

        return;
    }

    void OnTriggerEnter2D(Collider2D col){
        if (bool_morto){return;}
        //Debug.Log("triggo con "+col.name);
        switch (col.name){
            case "Hitbox":{
                if (bool_colpibile){
                    bool_colpibile=false;
                    StartCoroutine(ritorna_ricolpibile());
                    vitalita-=10;

                    skeletonAnimation.Skeleton.SetColor(Color.red);
                    StartCoroutine(ripristina_colore());

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
}
