using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine;
using Spine.Unity;

public class nemico_boss : MonoBehaviour
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

    //funzioni relativi alla parabola di salto
    private float t=5;
    private float salto_attivo=0;   //il valore và da 0 a 1
    private Vector3 origione_salto;
    private Vector3 destinazione_salto;
    private Vector3 destinazione_salto_media;
    private float x_destinazione_salto;

    void Start(){
        GO_player=GameObject.Find("Nekotaro");
        skeletonAnimation = GetComponent<SkeletonAnimation>();
        vitalita=vitalita_max;
    }

    // Update is called once per frame
    void Update(){
        if (bool_morto){return;}

        if (transform.position.x<GO_player.transform.position.x){horizontal=1;}
        else {horizontal=-1;}
        Flip();

        if (tempo_riposo_salto_attuale>0){
            tempo_riposo_salto_attuale-=(1f*Time.deltaTime);
            return;
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

        print ("posizione di partenza: "+transform.position);
        if (num_salti_attuale<num_salti_totale){
            origione_salto=transform.position;
            x_destinazione_salto=Random.Range(3,8);
            x_destinazione_salto*=horizontal;
            destinazione_salto=new Vector3((origione_salto.x+x_destinazione_salto),transform.position.y,transform.position.z);
            destinazione_salto_media=transform.position+(destinazione_salto-transform.position)/2 +Vector3.up *t;

            tempo_salto_attuale=tempo_salto;
            stato="salto";
            num_salti_attuale++;

        } else {
            print ("dovrei fare un altro tipo di attacco");
        }
    }

    private void FixedUpdate(){
        switch (stato){
            case "salto":{

                break;
            }
        }
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
