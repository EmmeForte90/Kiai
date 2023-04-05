using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ChangeScene : MonoBehaviour
{
    public string startScene;
    public string levelToLoad;
    public float Timelife;



void Awake()
{        
        StartCoroutine(FinishVideo());
}

public void changeScene()
{
    SceneManager.LoadScene(startScene);
    //PlayerPrefs.SetString("ContinueLevel", levelToLoad);


}


    IEnumerator FinishVideo()
    {
        yield return new WaitForSeconds(Timelife);
        SceneManager.LoadScene(startScene);
    }
}
