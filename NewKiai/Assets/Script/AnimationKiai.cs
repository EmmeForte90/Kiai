using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity.AttachmentTools;
using Spine;
using Spine.Unity;
using UnityEngine.Audio;

public class AnimationKiai : MonoBehaviour
{

   

    [Header("Audio")]
    [HideInInspector] public float basePitch = 1f;
    [HideInInspector] public float randomPitchOffset = 0.1f;
    [SerializeField] public AudioClip[] listSound; // array di AudioClip contenente tutti i suoni che si vogliono riprodurre
    private AudioSource[] sgm; // array di AudioSource che conterrà gli oggetti AudioSource creati
    public AudioMixer SFX;
    private bool sgmActive = false;

    [Header("Skin")]

	[SpineSkin] public string KiaiS = "default";
	
	public SkeletonGraphic _skeletonGraphic;
	Skeleton skeleton;
	Skin characterSkin;

	// for repacking the skin to a new atlas texture
	private Material runtimeMaterial;
	private Texture2D runtimeAtlas;


// Equipment skins
public enum ItemSlot
{
	None,
	KiaiS,
	
}



    [Header("Animations")]
    [SpineAnimation][SerializeField] private string EmptyAnimationName;
    [SpineAnimation][SerializeField] private string FullAnimationName;
    [SpineAnimation][SerializeField] private string StartAnimationName;
    private string currentAnimationName;
    private Spine.AnimationState _spineAnimationState;
    public Spine.Skeleton _skeleton;
    Spine.EventData eventData;

public static AnimationKiai Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }

        
            
        _spineAnimationState = GetComponent<Spine.Unity.SkeletonGraphic>().AnimationState;
        _spineAnimationState = _skeletonGraphic.AnimationState;
        //_skeleton = _skeletonGraphic.skeleton;
        sgm = new AudioSource[listSound.Length]; // inizializza l'array di AudioSource con la stessa lunghezza dell'array di AudioClip
        for (int i = 0; i < listSound.Length; i++) // scorre la lista di AudioClip
        {
            sgm[i] = gameObject.AddComponent<AudioSource>(); // crea un nuovo AudioSource come componente del game object attuale (quello a cui è attaccato lo script)
            sgm[i].clip = listSound[i]; // assegna l'AudioClip corrispondente all'AudioSource creato
            sgm[i].playOnAwake = false; // imposto il flag playOnAwake a false per evitare che il suono venga riprodotto automaticamente all'avvio del gioco
            sgm[i].loop = false; // imposto il flag playOnAwake a false per evitare che il suono venga riprodotto automaticamente all'avvio del gioco

        }    
        foreach (AudioSource audioSource in sgm)
        {
        audioSource.outputAudioMixerGroup = SFX.FindMatchingGroups("Master")[0];
        }



    }
   /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////	

public void UpdateCharacterSkinUI(string CH)
{
_skeletonGraphic.Skeleton.SetSkin(CH);
 characterSkin = new Skin(CH);
_skeletonGraphic.LateUpdate();
}

	void AddEquipmentSkinsTo(Skin combinedSkin)
	{
		skeleton = _skeletonGraphic.Skeleton;
		SkeletonData skeletonData = skeleton.Data;
		if (!string.IsNullOrEmpty(KiaiS)) combinedSkin.AddSkin(skeletonData.FindSkin(KiaiS));
		
	}
	
////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    public void KiaiComplete()
{
    if (currentAnimationName != FullAnimationName)
                {
                    _spineAnimationState.SetAnimation(2, FullAnimationName, true);
                    currentAnimationName = FullAnimationName;
                    //_spineAnimationState.Event += HandleEvent;

                    //Debug.Log("Combo Count: " + comboCount + ", Playing Animation: combo_1");
                }
                // Add event listener for when the animation completes
                //_spineAnimationState.GetCurrent(2).Complete += OnAttackAnimationComplete;
}

 public void KiaiStart()
{
    if (currentAnimationName != StartAnimationName)
                {
                    _spineAnimationState.SetAnimation(2, StartAnimationName, false);
                    currentAnimationName = StartAnimationName;
                    //_spineAnimationState.Event += HandleEvent;

                    //Debug.Log("Combo Count: " + comboCount + ", Playing Animation: combo_1");
                }
                // Add event listener for when the animation completes
                _spineAnimationState.GetCurrent(2).Complete += OnAttackAnimationComplete;
}

   public void KiaiEmpty()
{
    if (currentAnimationName != EmptyAnimationName)
                {
                    _spineAnimationState.SetAnimation(2, EmptyAnimationName, true);
                    currentAnimationName = EmptyAnimationName;
                    //_spineAnimationState.Event += HandleEvent;

                    //Debug.Log("Combo Count: " + comboCount + ", Playing Animation: combo_1");
                }
                // Add event listener for when the animation completes
                //_spineAnimationState.GetCurrent(2).Complete += OnAttackAnimationComplete;
}

 private void OnAttackAnimationComplete(Spine.TrackEntry trackEntry)
{
    // Remove the event listener
    trackEntry.Complete -= OnAttackAnimationComplete;

    // Clear the track 1 and reset to the idle animation
    _spineAnimationState.ClearTrack(2);
                KiaiComplete();
   
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
