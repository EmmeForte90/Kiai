using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine;
using Spine.Unity;

public class pietre_terreno : MonoBehaviour
{
    public GameObject pietre_terra_pf;
    public Dictionary<int, float> lista_pietre_terra_attivi = new Dictionary<int,float>();
    public Dictionary<int, GameObject> lista_pietre_terra_GO = new Dictionary<int, GameObject>(); 
    public Dictionary<int, Vector3> lista_pietre_terra_origine = new Dictionary<int, Vector3>();
    public Dictionary<int, Vector3> lista_pietre_terra_destinazione = new Dictionary<int, Vector3>();
    public Dictionary<int, Vector3> lista_pietre_terra_mid_destinazione = new Dictionary<int, Vector3>(); 
    public Dictionary<int, float> lista_pietre_terra_rotazione = new Dictionary<int,float>();
    public Dictionary<int, bool> lista_pietre_terra_ritardo = new Dictionary<int,bool>();
    private bool bool_attivo=false;
    private float t;

    // Start is called before the first frame update
    void Start()
    {
        t=10f;
    }

    // Update is called once per frame
    void Update()
{
    if (bool_attivo)
    {
        foreach (KeyValuePair<int, Vector3> attachStat in lista_pietre_terra_destinazione)
        {
            if (lista_pietre_terra_attivi[attachStat.Key] < 1)
            {
                lista_pietre_terra_attivi[attachStat.Key] += (1f * Time.deltaTime);
                lista_pietre_terra_GO[attachStat.Key].transform.position = punto_parabola(lista_pietre_terra_origine[attachStat.Key], 
                lista_pietre_terra_destinazione[attachStat.Key], lista_pietre_terra_mid_destinazione[attachStat.Key], t, 
                lista_pietre_terra_attivi[attachStat.Key]);
                lista_pietre_terra_GO[attachStat.Key].transform.Rotate(0, 0, 6 * lista_pietre_terra_rotazione[attachStat.Key] * Time.deltaTime);

                if (lista_pietre_terra_attivi[attachStat.Key] >= 1)
                {
                    // Add a null check before accessing the GameObject
                    if (lista_pietre_terra_GO[attachStat.Key] != null)
                    {
                        lista_pietre_terra_GO[attachStat.Key].SetActive(false);
                    }
                }
            }
            else
            {
                lista_pietre_terra_ritardo[attachStat.Key] = false;
            }
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

    public void resetta(){
        lista_pietre_terra_attivi.Clear();
        lista_pietre_terra_GO.Clear();
        lista_pietre_terra_destinazione.Clear();
        lista_pietre_terra_origine.Clear();
        lista_pietre_terra_mid_destinazione.Clear();
        lista_pietre_terra_rotazione.Clear();
        lista_pietre_terra_ritardo.Clear();
    }

    public void aggiungi(int num, float xor, float yor, float xar, float yar, float zar){
        Vector3 destinazione=new Vector3(xar,yar,-1);
        GameObject go_temp;

        go_temp=Instantiate(pietre_terra_pf);
        go_temp.transform.SetParent(gameObject.transform);
        go_temp.transform.localPosition = new Vector3(xor, yor, zar);
        go_temp.SetActive(true);
        lista_pietre_terra_attivi.Add(num,0);
        lista_pietre_terra_GO.Add(num,go_temp);
        lista_pietre_terra_origine.Add(num,go_temp.transform.localPosition);
        lista_pietre_terra_destinazione.Add(num,destinazione);

        Vector3 vin=go_temp.transform.localPosition+(destinazione-go_temp.transform.localPosition)/2 +Vector3.up *t;
        lista_pietre_terra_mid_destinazione.Add(num,vin);

        lista_pietre_terra_ritardo.Add(num,false);

        StartCoroutine(pietre_terra_ritardo(num));

        lista_pietre_terra_rotazione.Add(num,Random.Range(0,20));
        if (Random.Range(0,2)==1){lista_pietre_terra_rotazione[num]*=-1;}
    }

    private IEnumerator pietre_terra_ritardo(int num){
        //yield return new WaitForSeconds(num*0.5f);
        yield return new WaitForSeconds(0.5f);
        lista_pietre_terra_ritardo[num]=true;
    }
    
    public void avvia(){
        bool_attivo=true;
    }
}
