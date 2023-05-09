using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine;
using Spine.Unity;

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
    private bool bool_morte_attiva=false;
    private float distanza_temp;
    private Vector2 xTarget;

    private float tempo_attuale_guardia=0;

    private string stato;

    private float tempo_stanchezza;
    
    [SerializeField] private Vector3[] posizioni;
    private int index_posizioni;

    [SerializeField] private Rigidbody2D rb;
    // Start is called before the first frame update
    void Start()
    {
        GO_player=GameObject.Find("Nekotaro");
        skeletonAnimation = GetComponent<SkeletonAnimation>();
    }

    void Update(){
        if (tempo_stanchezza>0){
            print ("stanchezza: "+tempo_stanchezza);
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
        yield return new WaitForSeconds(0.25f);
        stato="tired";
        tempo_stanchezza=2.5f;
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
            case "attacco":{
                skeletonAnimation.AnimationName = "horizzontal/attack_horizzontal";
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
                skeletonAnimation.AnimationName = "idle_battle";
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
