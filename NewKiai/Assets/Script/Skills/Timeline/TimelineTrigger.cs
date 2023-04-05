using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class TimelineTrigger : MonoBehaviour
{
    //public TimelineManager.filmati filmati;
    private PlayableDirector timeline;
    private void Awake()
    {
        timeline = GetComponent<PlayableDirector>();
        //CutsceneManager.Instance.TimelineEnd(id);

    }

    void OnTriggerEnter2D(Collider2D test)
    {
        if (test.gameObject.tag == "Player")
        {
            test.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            timeline.Play();
            this.enabled = false;
          //  CutsceneManager.Instance.TimelineEnd(id);

        }
    }
}
