using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour

{
    [Header("UI Canvas :")] 
    public Canvas Ready;
    public Canvas GamePause;
    public Canvas GameOver;

    [Header("Score Fields :")]
    public TextMeshProUGUI ScoreText;
    public TextMeshProUGUI HiScoreText;

    public int currentScore;
    public int prevScore;
    void Start()
    {
        Time.timeScale = 0f;

        //Disable Game UI Canvases
        GamePause.enabled = false;
        GameOver.enabled = false;
        currentScore = 0;

        //Set initial score to 0 & display current score in game as 0
        currentScore = 0;
        ScoreText.text = currentScore.ToString();

        //Load Previous Hi Score
        HiScoreText.text = PlayerPrefs.GetFloat("High Score").ToString();


    }


    void Update()
    {
        //Update Score
        ScoreText.text = currentScore.ToString();

        if (GameOver.isActiveAndEnabled) Time.timeScale = 0f;
    }

    public void Play()
    {
        Ready.enabled = false;
        Time.timeScale = 1f; 
        TetrisGrid.StartGame();
    }
    
    public void Pause()
    {
        if (GamePause.isActiveAndEnabled || Ready.isActiveAndEnabled)
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
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        TetrisGrid.RefreshGrid(); 
    }

    public void Quit()
    {
        Application.Quit();
    }

  
}
