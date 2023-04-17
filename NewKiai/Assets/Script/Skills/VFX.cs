using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VFX : MonoBehaviour
{
    private GameObject player;
    public bool vertical;
    public bool OriginalSize;
    public bool followPlayer = false;
    
private void Start()
{            
        if (player == null)
        {
        player = GameObject.FindWithTag("Player");
        }
        
        
        if(!followPlayer)
        {
            if(!OriginalSize)
        {     
           if(!vertical)
        { 
        if(Move.instance.transform.localScale.x > 0)
        {
                transform.localScale = new Vector3(1, 1, 1);
        } 
        else if(Move.instance.transform.localScale.x < 0)
        {
                transform.localScale = new Vector3(1, -1, 1);

        }}else if(vertical)
        { 
        if(Move.instance.transform.localScale.x > 0)
        {
                transform.localScale = new Vector3(1, 1, 1);
        } 
        else if(Move.instance.transform.localScale.x < 0)
        {
                transform.localScale = new Vector3(-1, 1, 1);

        }
        }} else if (OriginalSize)
        {
        //Preserva le sue dimensioni
        }
        }
}

private void Update()
    {  
        if(followPlayer)
        {
        transform.position = Move.instance.transform.position;

        }
    }

}

