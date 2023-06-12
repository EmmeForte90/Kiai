using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class stamina_vfx_rule : MonoBehaviour
{
    public GameObject VFXStamina_pf;
    public GameObject VFXStopStamina_pf;
    public GameObject VFXStamina;

    private float start_local_scale_x;
    private float start_local_scale_y;
    private float start_local_scale_z;

    private bool bool_stamina_zero=true;
    // Start is called before the first frame update
    void Awake()
    {
        print ("scale: "+VFXStamina_pf.transform.localScale.y);
        //start_local_scale_x=VFXStamina.transform.localScale.x;
        start_local_scale_y=VFXStamina_pf.transform.localScale.y;
        start_local_scale_z=VFXStamina_pf.transform.localScale.z;

        VFXStamina=Instantiate(VFXStamina_pf,transform);
        VFXStamina.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void scala_GO_stamina(float stamina, float stamina_max){
        float percentuale=100*stamina/stamina_max;
        float local_scale_y=percentuale*start_local_scale_y/100;
        VFXStamina.transform.localScale=new Vector3(local_scale_y,local_scale_y,start_local_scale_z);
    }

    public void stamina_zero(float stamina){
        print ("f stamina zero: "+stamina);
        if (stamina<=5){
            if (bool_stamina_zero){
                bool_stamina_zero=false;
                Instantiate(VFXStopStamina_pf,transform);
            }
            StartCoroutine(next_stamina_zero());
        }
    }

    private IEnumerator next_stamina_zero(){    
        yield return new WaitForSeconds(5f);
        bool_stamina_zero=true;
    }
}
