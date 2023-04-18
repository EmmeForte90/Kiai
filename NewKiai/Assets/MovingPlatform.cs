using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
   [SerializeField] private Vector3[] posizioni;
	private int index_posizioni;
	
	public float velocita_totale;
	
    private void FixedUpdate(){
		transform.position = Vector2.MoveTowards(transform.position,posizioni[index_posizioni], Time.deltaTime*velocita_totale);
		
		if (transform.position==posizioni[index_posizioni]){
			if (index_posizioni==posizioni.Length -1){
				index_posizioni=0;
			} else {index_posizioni++;}
		}
    }
}