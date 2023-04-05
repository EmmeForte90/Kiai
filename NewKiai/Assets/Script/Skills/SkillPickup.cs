using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillPickup : MonoBehaviour
{
    public Skill Skill;
     

    public void Pickup()
    {
        /*SkillsInventory.Instance.Add(Skill);
        SkillsInventory.Instance.Listabil();
        SkillInventoryHUD.Instance.Add(Skill);
        SkillInventoryHUD.Instance.Listabil();
        Destroy(gameObject);*/
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
        Pickup();
        }
    }
}
