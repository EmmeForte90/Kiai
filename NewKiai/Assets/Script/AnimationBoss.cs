using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;
using Spine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;


public class AnimationBoss : MonoBehaviour
{

    [Header("Animations")]
    [SpineAnimation][SerializeField] private string idleAnimationName;
    [SpineAnimation][SerializeField] private string walkAnimationName;
    [SpineAnimation][SerializeField] private string runAnimationName;
    [SpineAnimation][SerializeField] private string attack1AnimationName;
    [SpineAnimation][SerializeField] private string attack2AnimationName;
    [SpineAnimation][SerializeField] private string attack3AnimationName;
    [SpineAnimation][SerializeField] private string hurtAnimationName;
    [SpineAnimation][SerializeField] private string ThrowAnmAnimationName;
    [SpineAnimation][SerializeField] private string NearAttkAnmAnimationName;
    [SpineAnimation][SerializeField] private string JumpAnimationName;
    [SpineAnimation][SerializeField] private string GuardAnimationName;
    [SpineAnimation][SerializeField] private string CrushAnmAnimationName;
    [SpineAnimation][SerializeField] private string JumpCrushAnmAnimationName;
    [SpineAnimation][SerializeField] private string CrushRageAnmAnimationName;
    [SpineAnimation][SerializeField] private string ChargeCrushAnmAnimationName;

    private string currentAnimationName;
    public SkeletonAnimation _skeletonAnimation;
    private Spine.AnimationState _spineAnimationState;
    private Spine.Skeleton _skeleton;
    Spine.EventData eventData;

    [Header("Audio")]
    [HideInInspector] public float basePitch = 1f;
    [HideInInspector] public float randomPitchOffset = 0.1f;
    [SerializeField] public AudioClip[] listSound; // array di AudioClip contenente tutti i suoni che si vogliono riprodurre
    private AudioSource[] bgm; // array di AudioSource che conterrà gli oggetti AudioSource creati
    public AudioMixer SFX;
    //private bool sgmActive = false;
    
    public static AnimationBoss instance;

    private void Awake()
    {
    if (_skeletonAnimation == null) 
    {
        Debug.LogError("Componente SkeletonAnimation non trovato!");
    }
    _spineAnimationState = _skeletonAnimation.AnimationState;
    _skeleton = _skeletonAnimation.skeleton;
    if (instance == null)
    {
        instance = this;
    }
    bgm = new AudioSource[listSound.Length]; // inizializza l'array di AudioSource con la stessa lunghezza dell'array di AudioClip
        for (int i = 0; i < listSound.Length; i++) // scorre la lista di AudioClip
        {
            bgm[i] = gameObject.AddComponent<AudioSource>(); // crea un nuovo AudioSource come componente del game object attuale (quello a cui è attaccato lo script)
            bgm[i].clip = listSound[i]; // assegna l'AudioClip corrispondente all'AudioSource creato
            bgm[i].playOnAwake = false; // imposto il flag playOnAwake a false per evitare che il suono venga riprodotto automaticamente all'avvio del gioco
            bgm[i].loop = false; // imposto il flag playOnAwake a false per evitare che il suono venga riprodotto automaticamente all'avvio del gioco

        }
 // Aggiunge i canali audio degli AudioSource all'output del mixer
        foreach (AudioSource audioSource in bgm)
        {
        audioSource.outputAudioMixerGroup = SFX.FindMatchingGroups("Master")[0];
        }
    }

/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
//Animations


public void Attack1Anm()
{
            
   
    if (currentAnimationName != attack1AnimationName)
                {
                    _spineAnimationState.SetAnimation(2, attack1AnimationName, false);
                    currentAnimationName = attack1AnimationName;
                    _spineAnimationState.Event += HandleEvent;
                }
                // Add event listener for when the animation completes
               _spineAnimationState.GetCurrent(2).Complete += OnAttackAnimationComplete;
    
}
public void Attack2Anm()
{
            
   
    if (currentAnimationName != attack2AnimationName)
                {
                    _spineAnimationState.SetAnimation(2, attack2AnimationName, false);
                    currentAnimationName = attack2AnimationName;
                    _spineAnimationState.Event += HandleEvent;
                }
                // Add event listener for when the animation completes
               _spineAnimationState.GetCurrent(2).Complete += OnAttackAnimationComplete;
    
}

public void Attack3Anm()
{
            
   
    if (currentAnimationName != attack3AnimationName)
                {
                    _spineAnimationState.SetAnimation(2, attack3AnimationName, false);
                    currentAnimationName = attack3AnimationName;
                    _spineAnimationState.Event += HandleEvent;
                }
                // Add event listener for when the animation completes
               _spineAnimationState.GetCurrent(2).Complete += OnAttackAnimationComplete;
    
}
public void HurtAnm()
{
            
    if (currentAnimationName != hurtAnimationName)
                {
                    //TemporaryChangeColor(Color.red);
                    _spineAnimationState.SetAnimation(2, hurtAnimationName, false);
                    currentAnimationName = hurtAnimationName;
                    _spineAnimationState.Event += HandleEvent;
                }
                // Add event listener for when the animation completes
               _spineAnimationState.GetCurrent(2).Complete += OnAttackAnimationComplete;
    
}

public void Guard()
{
    if (currentAnimationName != GuardAnimationName)
                {    
                    //rb.velocity = new Vector3(0, 0);
                    _spineAnimationState.ClearTrack(2);
                    _spineAnimationState.SetAnimation(1, GuardAnimationName, true);
                    currentAnimationName = GuardAnimationName;
                    _spineAnimationState.Event += HandleEvent;
                }
                // Add event listener for when the animation completes
                _spineAnimationState.GetCurrent(1).Complete += OnAttackAnimationComplete;
}

public void MovingAnm()
{
    
    if (currentAnimationName != walkAnimationName)
                {
                    _spineAnimationState.SetAnimation(2, walkAnimationName, true);
                    currentAnimationName = walkAnimationName;
                    _spineAnimationState.Event += HandleEvent;
                }
                // Add event listener for when the animation completes
                _spineAnimationState.GetCurrent(2).Complete += OnAttackAnimationComplete;
}

public void RunAnm()
{
    
    if (currentAnimationName != runAnimationName)
                {
                    _spineAnimationState.SetAnimation(2, runAnimationName, true);
                    currentAnimationName = runAnimationName;
                    _spineAnimationState.Event += HandleEvent;
                }
                // Add event listener for when the animation completes
                _spineAnimationState.GetCurrent(2).Complete += OnAttackAnimationComplete;
}

public void IdleAnm()
{
    if (currentAnimationName != idleAnimationName)
                {
                    _spineAnimationState.SetAnimation(1, idleAnimationName, true);
                    currentAnimationName = idleAnimationName;
                    _spineAnimationState.Event += HandleEvent;
                }
                // Add event listener for when the animation completes
                //_spineAnimationState.GetCurrent(2).Complete += OnAttackAnimationComplete;
}

public void Jump()
{
    
    if (currentAnimationName != JumpAnimationName)
                {
                    _spineAnimationState.SetAnimation(2, JumpAnimationName, true);
                    currentAnimationName = JumpAnimationName;
                    _spineAnimationState.Event += HandleEvent;
                }
                // Add event listener for when the animation completes
                _spineAnimationState.GetCurrent(2).Complete += OnAttackAnimationComplete;
}

public void CrushAnm()
{
    if (currentAnimationName != CrushAnmAnimationName)
                {
                    _spineAnimationState.SetAnimation(2, CrushAnmAnimationName, false);
                    currentAnimationName = CrushAnmAnimationName;
                    _spineAnimationState.Event += HandleEvent;
                }
                // Add event listener for when the animation completes
                _spineAnimationState.GetCurrent(2).Complete += OnAttackAnimationComplete;
}
public void CrushRageAnm()
{
    if (currentAnimationName != CrushRageAnmAnimationName)
                {
                    _spineAnimationState.SetAnimation(2, CrushRageAnmAnimationName, false);
                    currentAnimationName = CrushRageAnmAnimationName;
                    _spineAnimationState.Event += HandleEvent;
                }
                // Add event listener for when the animation completes
                _spineAnimationState.GetCurrent(2).Complete += OnAttackAnimationComplete;
}
public void ChargeCrushAnm()
{
    if (currentAnimationName != ChargeCrushAnmAnimationName)
                {
                    _spineAnimationState.SetAnimation(2, ChargeCrushAnmAnimationName, true);
                    currentAnimationName = ChargeCrushAnmAnimationName;
                    _spineAnimationState.Event += HandleEvent;
                }
                // Add event listener for when the animation completes
                _spineAnimationState.GetCurrent(2).Complete += OnChargeAnimationComplete;
}

public void JumpCrushAnm()
{
    if (currentAnimationName != JumpCrushAnmAnimationName)
                {
                    _spineAnimationState.SetAnimation(2, JumpCrushAnmAnimationName, false);
                    currentAnimationName = JumpCrushAnmAnimationName;
                    _spineAnimationState.Event += HandleEvent;
                }
                // Add event listener for when the animation completes
               _spineAnimationState.GetCurrent(2).Complete += OnAttackAnimationComplete;
}

public void Throw()
{
    if (currentAnimationName != ThrowAnmAnimationName)
                {
                    _spineAnimationState.SetAnimation(2, ThrowAnmAnimationName, false);
                    currentAnimationName = ThrowAnmAnimationName;
                    _spineAnimationState.Event += HandleEvent;
                }
                // Add event listener for when the animation completes
                _spineAnimationState.GetCurrent(2).Complete += OnAttackAnimationComplete;
}

public void NearAttk()
{
    if (currentAnimationName != NearAttkAnmAnimationName)
                {
                    _spineAnimationState.SetAnimation(1, NearAttkAnmAnimationName, false);
                    currentAnimationName = NearAttkAnmAnimationName;
                    _spineAnimationState.Event += HandleEvent;
                }
                // Add event listener for when the animation completes
                _spineAnimationState.GetCurrent(2).Complete += OnAttackAnimationComplete;
}

private void OnChargeAnimationComplete(Spine.TrackEntry trackEntry)
{
    // Remove the event listener
    trackEntry.Complete -= OnChargeAnimationComplete;

    // Clear the track 1 and reset to the idle animation
    _spineAnimationState.ClearTrack(2);
   _spineAnimationState.SetAnimation(2, CrushRageAnmAnimationName, false);
    currentAnimationName = CrushRageAnmAnimationName;
    _spineAnimationState.GetCurrent(2).Complete += OnEndAnimationComplete;

}
private void OnEndAnimationComplete(Spine.TrackEntry trackEntry)
{
    // Remove the event listener
    trackEntry.Complete -= OnEndAnimationComplete;

    // Clear the track 1 and reset to the idle animation
    _spineAnimationState.SetAnimation(2, idleAnimationName, true);
    currentAnimationName = idleAnimationName;
}

private void OnAttackAnimationComplete(Spine.TrackEntry trackEntry)
{
    // Remove the event listener
    trackEntry.Complete -= OnAttackAnimationComplete;

    // Clear the track 1 and reset to the idle animation
    _spineAnimationState.ClearTrack(2);
    _spineAnimationState.SetAnimation(1, idleAnimationName, true);
    currentAnimationName = idleAnimationName;
}
/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
//Audio
public void PlayMFX(int soundToPlay)
    {
        bgm[soundToPlay].Stop();
        // Imposta la pitch dell'AudioSource in base ai valori specificati.
        bgm[soundToPlay].pitch = basePitch + Random.Range(-randomPitchOffset, randomPitchOffset); 
        bgm[soundToPlay].Play();
    }
/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
//Event


void HandleEvent (TrackEntry trackEntry, Spine.Event e) 
    {

if (e.Data.Name == "StopCharge") 
    {
                                              
    //BossThief.instance.StartCharging = false;

    }
    
    }



}

