using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Audio;
using Spine.Unity;
using Spine;
using UnityEngine.SceneManagement;

public class PuppetM : MonoBehaviour
{
    [Header("Animations")]
    [SpineAnimation][SerializeField] private string idleAnimationName;
    [SpineAnimation][SerializeField] private string HitAnimationName;
     private string currentAnimationName;
    private SkeletonAnimation _skeletonAnimation;
    private Spine.AnimationState _spineAnimationState;
    private Spine.Skeleton _skeleton;
    Spine.EventData eventData;

public static PuppetM Instance;


void Awake()
{
        Instance = this;
}

 public void Idle()
{
             if (currentAnimationName != idleAnimationName)
                {  
                    _spineAnimationState.SetAnimation(1, idleAnimationName, true);
                    currentAnimationName = idleAnimationName;
                }            
}

public void Hit()
{
             if (currentAnimationName != HitAnimationName)
                { 
                    _spineAnimationState.ClearTrack(1);
                    _spineAnimationState.SetAnimation(1, HitAnimationName, false);
                    currentAnimationName = HitAnimationName;
                }
                // Add event listener for when the animation completes
                _spineAnimationState.GetCurrent(1).Complete += OnAttackAnimationComplete;
}

private void OnAttackAnimationComplete(Spine.TrackEntry trackEntry)
{
    // Remove the event listener
    trackEntry.Complete -= OnAttackAnimationComplete;

    // Clear the track 1 and reset to the idle animation
    _spineAnimationState.ClearTrack(1);
    _spineAnimationState.SetAnimation(1, idleAnimationName, true);
    currentAnimationName = idleAnimationName;

}



}
