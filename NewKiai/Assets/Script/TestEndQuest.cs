using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestEndQuest : MonoBehaviour
{

    public Quests Quest;

     public void Pickup()
    {
        Quest.isComplete = true;
        Quest.isActive = false;
        //QuestCharacters.Instance.Quest.isActive = true;
        Destroy(gameObject);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Pickup();
            //Instantiate(VFX, transform.position, transform.rotation);

        }
    }
}
