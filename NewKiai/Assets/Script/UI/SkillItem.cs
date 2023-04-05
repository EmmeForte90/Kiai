using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Audio;

public class SkillItem : MonoBehaviour
{
   [Header("Skill")]
    public Skill Skill;

public int id;


    [Header("VFX")]
    [SerializeField] GameObject VFX;
  



    public void Pickup()
    {
       GameplayManager.instance.SkillAc(id);
       AudioManager.instance.PlaySFX(0);
      // PlayMFX(0);
        Destroy(gameObject);
    }

public void SkillDosentExist()
    {
 Destroy(gameObject);
 }

 
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
        Pickup();
        Instantiate(VFX, transform.position, transform.rotation);
       // take.Play();

        }
    }
}

