using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AuraAndHeal : MonoBehaviour
{
    public bool isAura = false;  
    public bool IsHeal = false;  
    [SerializeField] float lifeTime = 1f;
    // Start is called before the first frame update
    void Start()
    { 
        //Move.instance.Evocation();
        Move.instance.Stop();
        if(isAura)
    {
        //Ripristina gli utilizzi se hai gli slot pieni
          /*  if(UpdateMenuRapido.Instance.idup > 0 || 
            UpdateMenuRapido.Instance.idleft > 0 || 
            UpdateMenuRapido.Instance.idbottom > 0||
            UpdateMenuRapido.Instance.idright > 0 )
            {
            UpdateMenuRapido.Instance.Vleft = SkillMenu.Instance.MXVleft;
            UpdateMenuRapido.Instance.Vup = SkillMenu.Instance.MXVup;
            UpdateMenuRapido.Instance.Vright = SkillMenu.Instance.MXVright;
            UpdateMenuRapido.Instance.Vbottom = SkillMenu.Instance.MXVbottom;
        
            UpdateMenuRapido.Instance.SkillBottom_T.text = UpdateMenuRapido.Instance.Vbottom.ToString();
            UpdateMenuRapido.Instance.SkillUp_T.text = UpdateMenuRapido.Instance.Vup.ToString();
            UpdateMenuRapido.Instance.SkillLeft_T.text = UpdateMenuRapido.Instance.Vleft.ToString();
            UpdateMenuRapido.Instance.SkillRight_T.text = UpdateMenuRapido.Instance.Vright.ToString();

            Invoke("Destroy", lifeTime);
            }*/
    } else if(IsHeal)
    {
      //Ripristina L'essenza
          //  PlayerHealth.Instance.currentEssence = PlayerHealth.Instance.maxEssence;
            PlayerHealth.Instance.EssenceImg();
            Invoke("Destroy", lifeTime);

    }
            
    }

   
}
