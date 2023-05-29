using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine;
using Spine.Unity;

public class nemico_bigkatana : MonoBehaviour
{
    private float horizontal;
    private float velocita = 4f;
    private float velocita_corsa = 2f;
    private bool bool_dir_dx = true;
    private SkeletonAnimation skeletonAnimation;
    public GameObject GO_player;
    public float distanza_attacco=3f;
    private bool bool_morte_attiva=false;
    private float distanza_temp;
    private Vector2 xTarget;

    private float velocita_ricarica_stamina=2;
    private float stamina;
    private float stamina_max=50;

    private string stato;

    private float tempo_stanchezza;
    
    [SerializeField] private Vector3[] posizioni;
    private int index_posizioni;

    [SerializeField] private Rigidbody2D rb;
    // Start is called before the first frame update
    void Start(){
        stamina=stamina_max;
        GO_player=GameObject.Find("Nekotaro");
        skeletonAnimation = GetComponent<SkeletonAnimation>();
    }

    void Update(){
        if (stamina<stamina_max){
            stamina+=(velocita_ricarica_stamina*Time.deltaTime);
            print ("stamina: "+stamina);
        }

        if (tempo_stanchezza>0){
            //print ("stanchezza: "+tempo_stanchezza);
            tempo_stanchezza-=(1f*Time.deltaTime);
            if (tempo_stanchezza>0){return;}
            stato="idle";
        }

        distanza_temp=calcola_distanza((int)(GO_player.transform.position.x),(int)(GO_player.transform.position.y),(int)(transform.position.x),(int)(transform.position.y));
        //print ("distanza: "+distanza_temp);

        if (distanza_temp<distanza_attacco){
            if (stato=="idle"){
                if (stamina>=30){
                    stato="attacco_grosso";
                    stamina-=30;
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
    }

    private IEnumerator ferma_attacco(){    
        yield return new WaitForSeconds(0.83f);
        //stato="tired";
        //tempo_stanchezza=2.5f;
    }


    private void FixedUpdate()
    {
        //print ("stato: "+stato);
        if (tempo_stanchezza>0){
            if (stato=="tired"){
                skeletonAnimation.AnimationName = "tired";
            }
            return; 
        }
        if (bool_morte_attiva){return;}

        switch (stato){
            case "attacco_grosso":{
                skeletonAnimation.AnimationName = "attack_power/attack_power";
                StartCoroutine(ferma_attacco());
                if (transform.position.x<GO_player.transform.position.x){horizontal=1;}
                else {horizontal=-1;}
                xTarget = new Vector2(GO_player.transform.position.x, transform.position.y);
                transform.position = Vector2.MoveTowards(transform.position,xTarget, Time.deltaTime*velocita_corsa);
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
            case "guardia":{
                if (transform.position.x<GO_player.transform.position.x){horizontal=1;}
                else {horizontal=-1;}
                skeletonAnimation.AnimationName = "guard";
                Flip();
                break;
            }
            case "puo_attaccare":{
                if (transform.position.x<GO_player.transform.position.x){horizontal=1;}
                else {horizontal=-1;}
                skeletonAnimation.AnimationName = "walk";
                break;
            }
        }

        return;
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
