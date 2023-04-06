using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashAttack : MonoBehaviour
{
    [SerializeField] int damage = 50;
    [SerializeField] float lifeTime = 0.5f;
    public bool upper = false;    

    [Header("Audio")]
    [SerializeField] AudioSource SExp;
    [SerializeField] AudioSource SBomb;

    public void Start()
    {
        if(upper)
        {
        //Move.instance.attackupper();
        Invoke("Destroy", lifeTime);
        }else if(!upper)
        {
        {
        Move.instance.attackDash();
        Invoke("Destroy", lifeTime);
        }

    } 
    }
    private void Destroy()
    {
        Destroy(gameObject);
    }
}

