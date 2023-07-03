using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveGame : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GameplayManager.instance.FirstoOfPlay();
        GameplayManager.instance.TakePlayer();
    }
}
