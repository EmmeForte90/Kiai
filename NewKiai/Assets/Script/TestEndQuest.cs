using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class TestEndQuest : MonoBehaviour
{

    public Quests Quest;
    public GameObject VFX;


     public void Pickup()
    {
        Quest.isComplete = true;
        Quest.isActive = false;
        //QuestCharacters.Instance.Quest.isActive = true;
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Pickup();
            AudioManager.instance.PlaySFX(1);
            Instantiate(VFX, transform.position, transform.rotation);

        }
    }
}
