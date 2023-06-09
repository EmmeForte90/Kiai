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
    void Start(){
        stamina=stamina_max;
        vitalita=vitalita_max;
        GO_player=GameObject.Find("Nekotaro");
        skeletonAnimation = GetComponent<SkeletonAnimation>();
    }

    void Update(){
        if (bool_morto){return;}
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
                    tempo_ritorna_idle+=2.5f;
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


    private void FixedUpdate()
    {
        if (bool_morto){return;}

        switch (stato){
            case "attacco_grosso":{
                skeletonAnimation.AnimationName = "attack_power/attack_power";
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
    }

    void OnTriggerEnter2D(Collider2D col){
        if (bool_morto){return;}
        //Debug.Log("triggo con "+col.name);
        switch (col.name){
            case "Hitbox":{
                if (stato=="contrattacco"){return;}
                if (bool_colpibile){
                    bool_colpibile=false;
                    StartCoroutine(ritorna_ricolpibile());

                    //dopodicchè dobbiamo caricare il numero di volte che è stato colpito
                    tempo_contrattacco+=1.8f;
                    print ("tempo contrattacco: "+tempo_contrattacco);
                    if (tempo_contrattacco>=3){
                        print ("devo contrattaccare");
                        stato="contrattacco";
                        tempo_ritorna_idle=1;
                        //return;   //senza questo return, quando lui contrattacca, potrebbe essere ancora colpito; Mauro
                    } else {

                        //dobbiamo vedere quì se ha la stamina intanto per pararsi
                        if (stamina>5){
                            stamina-=5;
                            stato="guardia";
                            print ("devo pararmi");
                            tempo_ritorna_idle=1;
                            return;
                        }
                    }

                    vitalita-=10;

                    skeletonAnimation.Skeleton.SetColor(Color.red);
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
