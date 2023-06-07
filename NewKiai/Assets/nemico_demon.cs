using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine;
using Spine.Unity;

public class nemico_demon : MonoBehaviour
{
    private float horizontal;
    private float velocita = 4f;
    private float velocita_indietreggio = 2f;
    private bool bool_dir_dx = true;
    private SkeletonAnimation skeletonAnimation;
    public GameObject GO_player;
    public float distanza_attacco=7f;
    private float distanza_temp;
    private Vector2 xTarget;

    private bool bool_colpibile=true;
    private int vitalita;
    private int vitalita_max=200;
    private float tempo_ricolpibile=0.5f;
    private bool bool_morto=false;

    private float tempo_sparo=1f;
    private float tempo_sparo_attuale=0;

    private float tempo_palla_nuova=10f;
    private float tempo_palla_nuova_attuale=0f;

    private float tempo_vulnerabile=3;
    private float tempo_vulnerabile_attuale=0f;

    private bool bool_colpibile_palla=true;
    private float tempo_ricolpibile_palla=0.5f;

    private bool bool_palla_appena_lanciata=false;

    public GameObject palla_fuoco;

    private string stato;
    
    [SerializeField] private Vector3[] posizioni;
    private int index_posizioni;

    [SerializeField] private Rigidbody2D rb;
    // Start is called before the first frame update
    void Start(){
        vitalita=vitalita_max;
        palla_fuoco.SetActive(false);
        GO_player=GameObject.Find("Nekotaro");
        skeletonAnimation = GetComponent<SkeletonAnimation>();
    }

    void Update(){
        if (bool_morto){return;}

        if (tempo_vulnerabile_attuale>0){
            tempo_vulnerabile_attuale-=(1f*Time.deltaTime);
        }

        distanza_temp=calcola_distanza((int)(GO_player.transform.position.x),(int)(GO_player.transform.position.y),(int)(transform.position.x),(int)(transform.position.y));
        //print ("distanza: "+distanza_temp);

        if (tempo_palla_nuova_attuale>0){
            tempo_palla_nuova_attuale-=(1f*Time.deltaTime);
            stato="guardia";
        }

        if (tempo_sparo_attuale>0){
            tempo_sparo_attuale-=(1f*Time.deltaTime);
            if (tempo_sparo_attuale>0){return;}
            stato="spara";
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
                stato="guardia";
            }
        }
        else {
            if ((tempo_palla_nuova_attuale<=0)&&(tempo_sparo_attuale<=0)){
                stato="idle";
            }
        }
        //print ("stato: "+stato);
    }

    private void FixedUpdate(){
        if (bool_morto){return;}

        switch (stato){
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
            case "spara":{
                skeletonAnimation.AnimationName = "attack_shoot";
                if (transform.position.x<GO_player.transform.position.x){horizontal=1;}
                else {horizontal=-1;}
                Flip();
                break;
            }
            case "guardia":{
                skeletonAnimation.AnimationName = "guard";
                if (transform.position.x<GO_player.transform.position.x){horizontal=1;}
                else {horizontal=-1;}
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
            case "palla_fuoco_demone":{
                print (bool_colpibile_palla+" - "+bool_palla_appena_lanciata);
                if (bool_colpibile_palla){
                    if (!bool_palla_appena_lanciata){
                        bool_colpibile_palla=false;
                        StartCoroutine(ritorna_ricolpibile_palla());
                        print ("colpito dalla mia stessa palla");
                        tempo_vulnerabile_attuale=tempo_vulnerabile;
                    }
                }
                break;
            }
            case "Hitbox":{
                if (tempo_vulnerabile_attuale>0){
                    if (bool_colpibile){
                        bool_colpibile=false;
                        StartCoroutine(ritorna_ricolpibile());
                        vitalita-=10;
                        print ("vitalita: "+vitalita);

                        if (vitalita<=0){
                            bool_morto=true;
                            print ("è morto!");
                            skeletonAnimation.loop=false;
                            skeletonAnimation.AnimationName="die_back";
                            StartCoroutine(rimuovi());
                        }
                    }
                } else {
                    print ("è invulnerabile...");
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
}
