using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class unlockSkill : MonoBehaviour
{
    public int IdSkill;
    public bool Isdoublejump = false;
    public bool IsDash = false;
    public bool Iswalljump = false;
    public bool IsRampino = false;
    private bool isCool = false;
    [SerializeField] GameObject VFX;


    public void Pickup()
    {
        if(Isdoublejump)
        {GameplayManager.instance.unlockDoubleJump = true;
        GameplayManager.instance.SkillTaking(IdSkill);
        }
        else if(IsDash)
        {GameplayManager.instance.unlockDash = true;
        GameplayManager.instance.SkillTaking(IdSkill);
        }
        else if(Iswalljump)
        {GameplayManager.instance.unlockWalljump = true;
        GameplayManager.instance.SkillTaking(IdSkill);

        }else if(IsRampino)
        {GameplayManager.instance.unlockRampino = true;
        GameplayManager.instance.SkillTaking(IdSkill);
        }
        Destroy(gameObject);
    }

    public void Take()
    {
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if(!isCool)
            {
            Pickup();
            Instantiate(VFX, transform.position, transform.rotation);
            isCool = true;
            }

        }
    }
}
