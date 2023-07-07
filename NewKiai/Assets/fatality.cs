using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;
using Spine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;

public class fatality : MonoBehaviour
{
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

    private Transform toy; // Variabile per il player
    public Transform Enemy; // Variabile per il player
    //public Transform Spawn; // Variabile per il player
   
   
    [Tooltip("Quale fatality deve eseguire il player? 1-Jump 2-Dance(DemonMonk) 3-Back 4-For Lance and BigKatana 5-Veloce")]
    public int ChooseFatality;

    [SpineAnimation][SerializeField] private string tiredAnimationName;
    [SpineAnimation][SerializeField] private string fatalityAnimationName;

private void Awake()
    {
        toy = GameObject.FindWithTag("Player").transform;
        //transform.position = Spawn.transform.position;
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
        // Attendi che l'animazione di fatality sia completata
        yield return new WaitForSeconds(1f);
        //endFata = true;
        Move.instance.NotStrangeAnimationTalk = false;
        Move.instance.stopInput = false;
        yield return new WaitForSeconds(3f);
        if(needFinalVFX)
        {
            VFXEnding.gameObject.SetActive(true);
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