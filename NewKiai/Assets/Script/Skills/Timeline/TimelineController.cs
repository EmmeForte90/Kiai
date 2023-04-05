using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;
using Cinemachine;

    public class TimelineController : MonoBehaviour 
    {
    public PlayableDirector _director;
    public GameObject Cutscene;
    public int ID;
    private CinemachineVirtualCamera virtualCamera; //riferimento alla virtual camera di Cinemachine
    private GameObject player; // Variabile per il player
    //private GameObject Camera;


    private void Awake()
    {
       // _director = GetComponent<PlayableDirector>();
       virtualCamera = GameObject.FindWithTag("MainCamera").GetComponent<CinemachineVirtualCamera>(); //ottieni il riferimento alla virtual camera di Cinemachine
        player = GameObject.FindWithTag("Player");
     //   Camera =  GameObject.FindWithTag("MainCamera");
    }

    private void Update()
    {
        if ((Input.GetButtonDown("Jump") || Input.GetMouseButtonDown(0)) && _director.time != 0)
        { 
            StartTimeline();
        }
    }


public  void StartMusicG()
{
    if(AudioManager.instance == null) return;
    AudioManager.instance.PlayMFX(1);
}

public  void ResetCamera()
{
GameplayManager.instance.TakeCamera();
}

public  void TimelineRepeat()
{
    CutsceneManager.Instance.TimelineStart(ID);
}

public  void TimelineDontRepeat()
{
    CutsceneManager.Instance.TimelineEnd(ID);
}

    public void StartTimeline()
    {  
        _director.time = _director.time;
        _director.playableGraph.GetRootPlayable(0).SetSpeed(1);
    }

    public void StopTimeline()
    {
        _director.playableGraph.GetRootPlayable(0).SetSpeed(0);
    }

    
}
