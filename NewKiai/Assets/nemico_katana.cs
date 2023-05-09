using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine;
using Spine.Unity;

public class nemico_katana : MonoBehaviour
{
    private float horizontal;
    private float velocita = 8f;
    private float velocita_difesa = 4f;
    private float velocita_totale;
    private bool bool_dir_dx = true;
    private bool bool_difesa=true;
    private SkeletonAnimation skeletonAnimation;
    private bool bool_movimento=true;
    private bool bool_attacca_personaggio=false;
    public GameObject GO_player;
    public float distanza_rottura=20f;
    private bool bool_morte_attiva=false;
    
    [SerializeField] private Vector3[] posizioni;
    private int index_posizioni;

    [SerializeField] private Rigidbody2D rb;
    // Start is called before the first frame update
    void Start()
    {
        GO_player=GameObject.Find("Nekotaro");
        skeletonAnimation = GetComponent<SkeletonAnimation>();
    }


    private void FixedUpdate()
    {
        if (bool_morte_attiva){return;}
        //print(transform.rotation+" - "+launchoffset.rotation+" - "+bool_dir_dx);
        velocita_totale=velocita;
        if (bool_difesa){velocita_totale=velocita_difesa;}
        
        if (
            calcola_distanza(
                (int)(GO_player.transform.position.x),
                (int)(GO_player.transform.position.y),
                (int)(transform.position.x),
                (int)(transform.position.y)
            )<distanza_rottura){
            bool_movimento=false;
            bool_difesa=true;
            bool_attacca_personaggio=true;
            //skeletonAnimation.AnimationName = "shoot";
            if (transform.position.x<GO_player.transform.position.x){horizontal=1;}
            else {horizontal=-1;}
            
            float animTime = skeletonAnimation.state.GetCurrent(0).AnimationTime;
            
            Flip();
            return;
        }
        else {bool_movimento=true;bool_attacca_personaggio=false;bool_difesa=false;}
        
        if (bool_attacca_personaggio){
        }
        else {
            if (bool_movimento){
                if (!bool_difesa){skeletonAnimation.AnimationName = "walk_thief";}
                else {skeletonAnimation.AnimationName = "run";}


                /*
                transform.position = Vector2.MoveTowards(transform.position,posizioni[index_posizioni], Time.deltaTime*velocita_totale);
                if (transform.position.x<posizioni[index_posizioni].x){horizontal=1;}
                else {horizontal=-1;}
                
                if (transform.position==posizioni[index_posizioni]){
                    if (index_posizioni==posizioni.Length -1){
                        index_posizioni=0;
                    } else {index_posizioni++;}
                }
                */
                Flip();
            } else {
                skeletonAnimation.AnimationName = "idle";
            }
        }
        
        //rb.velocity = new Vector2(horizontal * velocita_totale, rb.velocity.y);
    }

    private void Flip()
    {
        if (bool_dir_dx && horizontal < 0f || !bool_dir_dx && horizontal > 0f)
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
