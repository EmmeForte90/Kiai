using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine;
using Spine.Unity;

public class nemico_rifle : MonoBehaviour
{
    private float horizontal;
    private float velocita = 4f;
    private float velocita_indietreggio = 2f;
    private bool bool_dir_dx = true;
    private SkeletonAnimation skeletonAnimation;
    public GameObject GO_player;
    public float distanza_attacco=7f;
    private bool bool_morte_attiva=false;
    private float distanza_temp;
    private Vector2 xTarget;

    public GameObject proiettile_default;

    private float tempo_indietreggia=1.5f;
    private float tempo_indietreggia_attuale=0;

    private float tempo_mira=1f;
    private float tempo_mira_attuale=0;

    private float tempo_sparo=0.3f;
    private float tempo_sparo_attuale=0;

    private float tempo_ricarica=1f;
    private float tempo_ricarica_attuale=0;

    private bool bool_player_visto=false;

    private bool bool_colpibile=true;
    private int vitalita;
    private int vitalita_max=30;
    private float tempo_ricolpibile=0.5f;
    private bool bool_morto=false;

    private string stato;
    
    [SerializeField] private Vector3[] posizioni;
    private int index_posizioni;

    [SerializeField] private Rigidbody2D rb;
    // Start is called before the first frame update
    void Start(){
        vitalita=vitalita_max;
        proiettile_default.SetActive(false);
        //bool_player_visto=true; //debug
        GO_player=GameObject.Find("Nekotaro");
        skeletonAnimation = GetComponent<SkeletonAnimation>();
    }

    void Update(){
        if (bool_morto){return;}
        if (tempo_indietreggia_attuale>0){
            tempo_indietreggia_attuale-=(1f*Time.deltaTime);
            if (tempo_indietreggia_attuale>0){return;}
            stato="idle";
        }

        if (tempo_mira_attuale>0){
            tempo_mira_attuale-=(1f*Time.deltaTime);
            if (tempo_mira_attuale>0){return;}
            stato="spara";
            tempo_sparo_attuale+=tempo_sparo;
            GameObject go_temp=Instantiate(proiettile_default,transform);
            go_temp.SetActive(true);
        }

        if (tempo_sparo_attuale>0){
            tempo_sparo_attuale-=(1f*Time.deltaTime);
            if (tempo_sparo_attuale>0){return;}
            stato="ricarica";
            tempo_ricarica_attuale+=tempo_ricarica;
        }

        if (tempo_ricarica_attuale>0){
            tempo_ricarica_attuale-=(1f*Time.deltaTime);
            if (tempo_ricarica_attuale>0){return;}
            stato="idle";
        }

        distanza_temp=calcola_distanza((int)(GO_player.transform.position.x),(int)(GO_player.transform.position.y),(int)(transform.position.x),(int)(transform.position.y));
        //print ("distanza: "+distanza_temp);

        if (distanza_temp<distanza_attacco){
            if (!bool_player_visto){
                bool_player_visto=true;
                stato="indietreggia";
                tempo_indietreggia_attuale=tempo_indietreggia;
                distanza_attacco+=2;
            }
            else {
                switch (stato){
                    case "idle":{stato="mira";tempo_mira_attuale+=tempo_mira;break;}
                }
            }
        }
        else {
            stato="idle";
        }
        //print ("stato: "+stato);
    }

    private void FixedUpdate()
    {
        if (bool_morto){return;}
        if (bool_morte_attiva){return;}

        switch (stato){
            case "indietreggia":{
                skeletonAnimation.AnimationName = "walk";
                if (transform.position.x<GO_player.transform.position.x){horizontal=1;}
                else {horizontal=-1;}
                Flip();
                xTarget = new Vector2(transform.position.x, transform.position.y);
                if (horizontal==1){xTarget.x-=2;} else {xTarget.x+=2;}
                transform.position = Vector2.MoveTowards(transform.position,xTarget, Time.deltaTime*velocita_indietreggio);
                break;
            }
            case "spara":{
                skeletonAnimation.AnimationName = "shoot_front";
                if (transform.position.x<GO_player.transform.position.x){horizontal=1;}
                else {horizontal=-1;}
                Flip();
                break;  
            }
            case "mira":{
                skeletonAnimation.AnimationName = "aim_front";
                if (transform.position.x<GO_player.transform.position.x){horizontal=1;}
                else {horizontal=-1;}
                Flip();
                break;  
            }
            case "ricarica":{
                skeletonAnimation.AnimationName = "recharge";
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
        }

        return;
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (bool_morto){return;}
        //Debug.Log("triggo con "+col.name);
        switch (col.name){
            case "Hitbox":{
                if (bool_colpibile){
                    bool_colpibile=false;
                    StartCoroutine(ritorna_ricolpibile());
                    vitalita-=10;

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
