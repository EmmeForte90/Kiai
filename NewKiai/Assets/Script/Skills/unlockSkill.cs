using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class unlockSkill : MonoBehaviour
{
    public bool Isdoublejump = false;
    public bool IsDash = false;
    public bool Iswalljump = false;
    public bool IsCrash = false;
    [SerializeField] GameObject VFX;


    public void Pickup()
    {
        if(Isdoublejump)
        {GameplayManager.instance.unlockDoubleJump = true;
        }
        else if(IsDash)
        {GameplayManager.instance.unlockDash = true;
        }
        else if(Iswalljump)
        {GameplayManager.instance.unlockWalljump = true;
        }else if(IsCrash)
        {GameplayManager.instance.unlockCrash = true;
        }
        Destroy(gameObject);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Pickup();
                    Instantiate(VFX, transform.position, transform.rotation);

        }
    }
}
