using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Multilunge : MonoBehaviour
{
    [SerializeField] int damage = 50;
    [SerializeField] float lifeTime = 2f;
    [SerializeField] GameObject Left;
    [SerializeField] GameObject Right;

    [Header("Audio")]
    [SerializeField] AudioSource SExp;
    [SerializeField] AudioSource SBomb;

    public void Start()
    {
         if(Move.instance.transform.localScale.x > 0)
        {
            Right.gameObject.SetActive(true);
        } 
        else if(Move.instance.transform.localScale.x < 0)
        {
            Left.gameObject.SetActive(true);
        }
       Move.instance.Multilunge();
        Invoke("Destroy", lifeTime);
    } 
    public void Update()
    {
        Move.instance.Stop();
        
    } 
    private void Destroy()
    {
        Destroy(gameObject);
    }
}
