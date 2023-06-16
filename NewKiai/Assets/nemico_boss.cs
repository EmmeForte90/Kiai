using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine;
using Spine.Unity;

public class nemico_boss : MonoBehaviour
{
    public pietre_terreno pietre_terreno;

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
    private float vitalita;
    private float vitalita_max=500;

    private int num_salti_totale=3;
    private int num_salti_attuale=0;

    private bool bool_morto=false;

    private string stato;
    [SerializeField] private Rigidbody2D rb;

    private float tempo_salto=1f;
    private float tempo_salto_attuale=0;
    private float tempo_riposo_salto=1f;
    private float tempo_riposo_salto_attuale=0;
    private float tempo_riposo_attacco_attuale=3f;

    private float tempo_anim_attacco_mazza=0.4f;
    private float tempo_anim_attacco_mazza_attuale=0;

    public float tempo_riposo_attacco_salti=3;
    public float tempo_riposo_attacco_mazza=3;



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

    void Start(){
        GO_player=GameObject.Find("Nekotaro");
        skeletonAnimation = GetComponent<SkeletonAnimation>();
        vitalita=vitalita_max;
        attacco_tipo="mazza";
    }

    // Update is called once per frame
    void Update(){
        if (bool_morto){return;}

        if (transform.position.x<GO_player.transform.position.x){horizontal=1;}
        else {horizontal=-1;}
        Flip();

        if (tempo_anim_attacco_mazza_attuale>0){
            tempo_anim_attacco_mazza_attuale-=(1f*Time.deltaTime);
            if (tempo_anim_attacco_mazza_attuale<=0){
                tempo_riposo_attacco_attuale=tempo_riposo_attacco_mazza;
                stato="tired";
            }
            return;
        }

        if (tempo_riposo_salto_attuale>0){
            tempo_riposo_salto_attuale-=(1f*Time.deltaTime);
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
            attacco_tipo="mazza";
        }

        if (tempo_salto_attuale>0){
            tempo_salto_attuale-=(1f*Time.deltaTime);
            salto_attivo=1-(tempo_salto_attuale/tempo_salto);
            if (tempo_salto_attuale<0){tempo_salto_attuale=0;}
            transform.position=punto_parabola(origione_salto,destinazione_salto,destinazione_salto_media,t,salto_attivo);

            if (tempo_salto_attuale==0){
                tempo_riposo_salto_attuale=tempo_riposo_salto;
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
                    stato="salto";
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
                StartCoroutine(genera_pietre_suolo());
                break;
            }
        }
    }

    private void FixedUpdate(){
        switch (stato){
            case "salto":{

                break;
            }
            case "mazza":{
                skeletonAnimation.AnimationName="battle/attack_power/attack_power";
                break;
            }
            case "tired":{
                skeletonAnimation.AnimationName="battle/idle_battle";
                break;
            }
        }
    }

    private IEnumerator genera_pietre_suolo(){
        yield return new WaitForSeconds(0.2f);
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
}
