using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitboxKiai : MonoBehaviour
{
    [SerializeField] public Transform Pos;
    private bool CanTake = false;
    public float TimeHitbox; 
    public int Damage = 10;

    public static HitboxKiai Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

private void Update()
    {
        if(CanTake){
        TimeHitbox -= Time.deltaTime; //decrementa il timer ad ogni frame
        if (TimeHitbox <= 0f) {
        CanTake = false; 
        }}
        
    }
////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
void OnTriggerEnter2D(Collider2D other) 
{
        if(other.gameObject.tag == "Enemy" || other.gameObject.tag == "Boss")
        //Se il proiettile tocca il nemico
        {   
        if(!CanTake)
         {
            //Debug.Log("Normal"+ Damage);

            TimeHitbox = 0.5f;
            GameplayManager.instance.ComboCount();
            CanTake = true;
            //IDamegable hit = other.GetComponent<IDamegable>();
             //hit.Damage(Damage);
    }
    }
}
}
