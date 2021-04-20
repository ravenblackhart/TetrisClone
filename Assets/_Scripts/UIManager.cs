using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour

{
    public Canvas GamePause;
    public Canvas GameOver;
    void Start()
    {
        Time.timeScale = 1f;
        GamePause.enabled = false;
        GameOver.enabled = false;
    }

    public void Pause()
    {
        if (GamePause.isActiveAndEnabled)
        {
            GamePause.enabled = false;
            Time.timeScale = 1f; 
        }
        
        else
        {
            GamePause.enabled = true;
            Time.timeScale = 0f;
        }
    }

    //Restart Game
    public void Restart()
    {
        SceneManager.LoadScene("tetris");
    }

    public void Quit()
    {
        Application.Quit();
    }
}
