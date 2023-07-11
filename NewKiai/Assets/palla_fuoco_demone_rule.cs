using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;
using Spine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;
using UnityEngine.UI;

public class palla_fuoco_demone_rule : MonoBehaviour
{
    public float velocita_palla_di_fuoco = 5f;
    public float tempo_distruzione_sola = 5f;
    private GameObject target;
    public Rigidbody2D rb;
    public SpriteRenderer sprite;
    private bool bool_distrutta=false;
    [SerializeField] GameObject VFXExplode;
    public int damage = 10;

 
    void Start()
    {
        target = GameObject.Find("Nekotaro");
        StartCoroutine(distruggi_col_tempo());
        StartCoroutine(co_palla_lanciata());
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 direction = (target.transform.position - transform.position).normalized;
        transform.position += (direction * (velocita_palla_di_fuoco * Time.deltaTime));
    }

    void OnTriggerEnter2D(Collider2D other) {
        if(other.CompareTag("Player")){
            Instantiate(VFXExplode, transform.position, transform.rotation);
            AudioManager.instance.PlaySFX(2);
            //print ("colpito il personaggio!");
            distruggi_palla_di_fuoco();
              if(!Move.instance.isGuard)
            {
            if (!Move.instance.isDeath)
            {
                if (!Move.instance.isHurt)
            {
            PlayerHealth.Instance.Damage(damage);
            //Move.instance.Knockback();            
            }
            }
        }}
    }

    private IEnumerator distruggi_col_tempo(){
        yield return new WaitForSeconds(tempo_distruzione_sola);
        distruggi_palla_di_fuoco();
    }

    private void distruggi_palla_di_fuoco(){
        if (bool_distrutta){return;}
        bool_distrutta=true;
        Instantiate(VFXExplode, transform.position, transform.rotation);
        AudioManager.instance.PlaySFX(2);
        Destroy(gameObject);
    }

    private IEnumerator co_palla_lanciata(){    
        yield return new WaitForSeconds(0.5f);
        //bool_palla_appena_lanciata=false;
    }
}
