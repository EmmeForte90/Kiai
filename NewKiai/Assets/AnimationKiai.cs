using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Spine.Unity;
using Spine;

public class AnimationKiai : MonoBehaviour
{

    private string currentAnimationName;
    
    public SkeletonGraphic _skeletonAnimation;
    public Spine.AnimationState _spineAnimationState;
    public Spine.Skeleton _skeleton;
    Spine.EventData eventData;

    [SpineAnimation][SerializeField] private string KiaiAnimationName;
    [SpineAnimation][SerializeField] private string KiaiCompleteAnimationName;
    [SpineAnimation][SerializeField] private string KiaiVoidAnimationName;

public static AnimationKiai Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        _skeletonAnimation = GetComponent<SkeletonGraphic>();
        if (_skeletonAnimation == null) {
        Debug.LogError("Componente SkeletonAnimation non trovato!");
        _spineAnimationState = GetComponent<Spine.Unity.SkeletonGraphic>().AnimationState;
        _spineAnimationState = _skeletonAnimation.AnimationState;
        //_skeleton = _skeletonAnimation.skeleton;
    }
    }

    public void KiaiComplete()
{
    if (currentAnimationName != KiaiCompleteAnimationName)
                {
                    Move.instance.Stop();
                    _spineAnimationState.SetAnimation(2, KiaiCompleteAnimationName, false);
                    currentAnimationName = KiaiCompleteAnimationName;
                    _spineAnimationState.Event += HandleEvent;

                    //Debug.Log("Combo Count: " + comboCount + ", Playing Animation: combo_1");
                }
                // Add event listener for when the animation completes
                //_spineAnimationState.GetCurrent(2).Complete += OnAttackAnimationComplete;
}
   public void KiaiVoid()
{
    if (currentAnimationName != KiaiVoidAnimationName)
                {
                    Move.instance.Stop();
                    _spineAnimationState.SetAnimation(2, KiaiVoidAnimationName, false);
                    currentAnimationName = KiaiVoidAnimationName;
                    _spineAnimationState.Event += HandleEvent;

                    //Debug.Log("Combo Count: " + comboCount + ", Playing Animation: combo_1");
                }
                // Add event listener for when the animation completes
                //_spineAnimationState.GetCurrent(2).Complete += OnAttackAnimationComplete;
}

 public void KiaiStart()
{
    if (currentAnimationName != KiaiAnimationName)
                {
                    Move.instance.Stop();
                    _spineAnimationState.SetAnimation(2, KiaiAnimationName, false);
                    currentAnimationName = KiaiAnimationName;
                    _spineAnimationState.Event += HandleEvent;

                    //Debug.Log("Combo Count: " + comboCount + ", Playing Animation: combo_1");
                }
                // Add event listener for when the animation completes
                //_spineAnimationState.GetCurrent(2).Complete += OnAttackAnimationComplete;
}

//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

//Non puoi giocare di local scale sui vfx perché sono vincolati dal localscale del player PERò puoi giocare sulla rotazione E ottenere gli
//stessi effetti
void HandleEvent (TrackEntry trackEntry, Spine.Event e) {

if (e.Data.Name == "VFXpesante") {
        // Inserisci qui il codice per gestire l'evento.
        //Instantiate(pesante, slashpoint.position, transform.rotation);
    }
}

}
