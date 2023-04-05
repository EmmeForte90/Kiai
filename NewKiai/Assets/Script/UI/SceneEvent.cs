using UnityEngine;
using UnityEngine.Events;

public class SceneEvent : MonoBehaviour
{
   public UnityEvent onSceneChange = new UnityEvent();

    public void InvokeOnSceneChange()
    {
        onSceneChange.Invoke();
    }
}