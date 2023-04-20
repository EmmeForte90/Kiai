using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PauseMenu : MonoBehaviour
{

    public static PauseMenu instance;
    public string levelSelect, mainMenu;
    public GameObject GameManager;

    private void Awake()
    {
        instance = this;
    }




    public void LevelSelect()
    {
        SceneManager.LoadScene(levelSelect);
        //FindObjectOfType<ScenePersist>().ResetScenePersist();
        Time.timeScale = 1;
    }

    public void MainMenu()
    {
        //GameplayManager.instance.gameplayOff = true;
        SceneManager.LoadScene(mainMenu);
        Time.timeScale = 1;
        Destroy(GameManager);
    }


}