using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;
using Spine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;
using Cinemachine;

public class fatality : MonoBehaviour
{
    [Header("End")]
    public GameObject EndPrototype;
    public bool EndPrototypebool = false;

    [Header("Fatality")]
    public GameObject ContentENM;

    public SkeletonAnimation spineAnimation;
    public GameObject FPoint;
    public GameObject fatalit;
    //private bool endFata = false;
    private bool HideB = false;
    private bool _isInTrigger = false;
        
    [Header("VFX Ending")]

    public bool needFinalVFX = false;
    public GameObject VFXEnding;
    
    [Header("VFX Ending")]
    private CinemachineVirtualCamera vCam;
    public GameObject CamFocus;

    private Transform toy; // Variabile per il player
    public Transform Enemy; // Variabile per il player
    //public Transform Spawn; // Variabile per il player
   
    [Header("Animations")]

    [Tooltip("Quale fatality deve eseguire il player? 1-Jump 2-Dance(DemonMonk) 3-Back 4-For Lance and BigKatana 5-Veloce")]
    public int ChooseFatality;

    public bool FatalityActive;
    [SpineAnimation][SerializeField] private string tiredAnimationName;
    [SpineAnimation][SerializeField] private string fatalityAnimationName;
       
    [Header("Musica")]

    [Tooltip("Musica di base")]
    public int MusicBefore;
    [Tooltip("Musica da attivare se necessario quando la telecamera inquadra l'evento")]
    public int MusicAfter;
    public bool needMusic = false;

    public static fatality instance;

private void Awake()
    {
         if (instance == null)
        {
            instance = this;
        }
        toy = GameObject.FindWithTag("Player").transform;
        //transform.position = Spawn.transform.position;
        vCam = GameObject.FindWithTag("MainCamera").GetComponent<CinemachineVirtualCamera>(); 
        if(needMusic)
        {
            AudioManager.instance.CrossFadeINAudio(MusicBefore);
        }
    }  



    private void OnTriggerStay2D(Collider2D collision)
{
    if (collision.CompareTag("Player"))
    {
        if(!HideB)
        {
        fatalit.gameObject.SetActive(true);
        } else if(HideB)
        {
        fatalit.gameObject.SetActive(false);
        }
        if (Input.GetButtonDown("Fire1") && !_isInTrigger)
        {
        HideB = true;
        FatalityActive = true;
        _isInTrigger = true;
        Move.instance.NotStrangeAnimationTalk = true;
        Move.instance.Stop();
        Move.instance.drawsword = true;
        Move.instance.stopInput = true;
        if(Move.instance.transform.localScale.x > 0)
        {
        toy.transform.localScale = new Vector2(1, 1);
        Enemy.transform.localScale = new Vector2(-1, 1);
        toy.transform.position = FPoint.transform.position;
        }else if(Move.instance.transform.localScale.x < 0)
        {
        Enemy.transform.localScale = new Vector2(1, 1);
        toy.transform.localScale = new Vector2(-1, 1);
        toy.transform.position = FPoint.transform.position;
        }
        StartCoroutine(PlayFatalityAnimation());
        }
    }
}
private void OnTriggerExit2D(Collider2D collision)
{
    if (collision.CompareTag("Player"))
    {
        fatalit.gameObject.SetActive(false);
        
    }
}


    private IEnumerator PlayFatalityAnimation()
    {
        // Sospende l'esecuzione dello script per un breve momento
        // in modo che il nemico non muoia immediatamente
        fatalit.gameObject.SetActive(false);
        yield return new WaitForSeconds(0.5f);

        // Avvia l'animazione di fatality sullo SpineAnimation
        Move.instance.Fatality(ChooseFatality);
        spineAnimation.state.SetAnimation(2, fatalityAnimationName, false);
        spineAnimation.state.Event += HandleEvent;
        // Attendi che l'animazione di fatality sia completata
        yield return new WaitForSeconds(1f);
        if(EndPrototypebool)
        {
        EndPrototype.gameObject.SetActive(true);
        }
        Move.instance.NotStrangeAnimationTalk = false;
        Move.instance.stopInput = false;
        yield return new WaitForSeconds(3f);
        FatalityActive = false;
        if(needFinalVFX)
        {
            AudioManager.instance.PlaySFX(1);
            VFXEnding.gameObject.SetActive(true);
        }
        if(needMusic)
        {
            AudioManager.instance.CrossFadeOUTAudio(MusicBefore);
            AudioManager.instance.CrossFadeINAudio(MusicAfter);
            vCam.Follow = toy.transform;
        }
        
        Destroy(ContentENM);
    }

    #region Events
void HandleEvent (TrackEntry trackEntry, Spine.Event e) 
{
    if (e.Data.Name == "SoundSword") 
    {     
        AudioManager.instance.PlaySFX(8);
    }

    if (e.Data.Name == "SoundDeath") 
    {     
        AudioManager.instance.PlaySFX(7);
    }

}
#endregion

}