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

    private float tempo_palla_nuova=5f;
    private float tempo_palla_nuova_attuale=0f;

    private bool bool_invulnerabile=true;

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
            GameObject go_temp=Instantiate(palla_fuoco,transform);
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
            case "Hitbox":{
                if (!bool_invulnerabile){
                    if (bool_colpibile){
                        bool_colpibile=false;
                        StartCoroutine(ritorna_ricolpibile());
                        vitalita-=10;

                        if (vitalita<=0){
                            bool_morto=true;
                            print ("è morto!");
                            skeletonAnimation.loop=false;
                            skeletonAnimation.AnimationName="die_back";
                            StartCoroutine(rimuovi());
                        }
                    }
                } else {
                    print ("sono invulnerabile...");
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
