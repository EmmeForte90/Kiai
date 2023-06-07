using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class palla_fuoco_demone_rule : MonoBehaviour
{
    public float velocita_palla_di_fuoco = 5f;
    public float tempo_distruzione_sola = 5f;
    private GameObject target;
    public Rigidbody2D rb;
    public SpriteRenderer sprite;
    private bool bool_distrutta=false;

    private ParticleSystem[] particleSystems;
    // Start is called before the first frame update
    void Start()
    {
        target = GameObject.Find("Nekotaro");
        StartCoroutine(distruggi_col_tempo());
        particleSystems = GetComponentsInChildren<ParticleSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        //Vector3 newPosition = Vector3.Lerp(transform.position, target.transform.position, velocita_palla_di_fuoco * Time.deltaTime);
        //transform.position = newPosition;

        Vector3 direction = (target.transform.position - transform.position).normalized;
        transform.position += (direction * (velocita_palla_di_fuoco * Time.deltaTime));
    }

    void OnTriggerEnter2D(Collider2D other) {
        if(other.gameObject.name == "Nekotaro"){
            print ("colpito");
            distruggi_palla_di_fuoco();
        }
    }

    private IEnumerator distruggi_col_tempo(){
        yield return new WaitForSeconds(tempo_distruzione_sola);
        distruggi_palla_di_fuoco();
    }

    private void distruggi_palla_di_fuoco(){
        if (bool_distrutta){return;}
        bool_distrutta=true;
        Destroy(gameObject);
    }

    private void OnDestroy(){
        foreach (ParticleSystem ps in particleSystems){ps.Stop();}
    }
}
