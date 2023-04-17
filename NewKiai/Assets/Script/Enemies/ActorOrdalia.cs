using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;
using Spine;

public class ActorOrdalia : MonoBehaviour
{
    [Header("Animations")]

    [SpineAnimation][SerializeField] private string WaitAnimationName;
    [SpineAnimation][SerializeField] private string StartAnimationName;
        [SpineAnimation][SerializeField] private string idleAnimationName;

    private string currentAnimationName;
    public SkeletonAnimation _skeletonAnimation;
    private Spine.AnimationState _spineAnimationState;
    private Spine.Skeleton _skeleton;
    Spine.EventData eventData;
       
    public static ActorOrdalia Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        _spineAnimationState = GetComponent<Spine.Unity.SkeletonAnimation>().AnimationState;
        _spineAnimationState = _skeletonAnimation.AnimationState;
        _skeleton = _skeletonAnimation.skeleton;
        _skeletonAnimation = GetComponent<SkeletonAnimation>();
        if (_skeletonAnimation == null) {
            Debug.LogError("Componente SkeletonAnimation non trovato!");
        }       
    }

public void FacePlayer()
    {
        
            if (Move.instance.transform.position.x > transform.position.x)
            {
                transform.localScale = new Vector3(1, 1, 1);
            }
            else
            {
                transform.localScale = new Vector3(-1, 1, 1);
            }
        }

   /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
//ANIMATIONS

public void Wait()
{
   if (currentAnimationName != WaitAnimationName)
                {
                    _spineAnimationState.SetAnimation(0, WaitAnimationName, true);
                    currentAnimationName = WaitAnimationName;

                   // Debug.Log("Combo Count: " + comboCount + ", Playing Animation: combo_1");
                }
}

public void Standup()
{
    if (currentAnimationName != StartAnimationName)
                {
                    _spineAnimationState.SetAnimation(0, StartAnimationName, false);
                    currentAnimationName = StartAnimationName;

                   // Debug.Log("Combo Count: " + comboCount + ", Playing Animation: combo_1");
                }
}

public void idle()
{
    if (currentAnimationName != idleAnimationName)
                {
                    _spineAnimationState.SetAnimation(0, idleAnimationName, true);
                    currentAnimationName = idleAnimationName;

                   // Debug.Log("Combo Count: " + comboCount + ", Playing Animation: combo_1");
                }
}
}

