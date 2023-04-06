using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VFX : MonoBehaviour
{
    private GameObject player;

    private void Start()
    {            
        if (player == null)
        {
        player = GameObject.FindWithTag("Player");
        }
        if(Move.instance.transform.localScale.x > 0)
        {
                transform.localScale = new Vector3(1, 1, 1);
        } 
        else if(Move.instance.transform.localScale.x < 0)
        {
                transform.localScale = new Vector3(-1, 1, 1);

        }

    }
    
}
