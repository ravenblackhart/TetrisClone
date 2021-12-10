using System;
using System.Collections;
using System.Collections.Generic;
using tetrisVersion2;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour

{
    [Header("UI Canvas :")] 
    public Canvas ReadyCanvas;
    public Canvas GamePauseCanvas;
    public Canvas GameOverCanvas;

    [Header("Score Fields :")]
    public TextMeshProUGUI ScoreText;
    public TextMeshProUGUI HiScoreText;

    private int currentScore;
    private int prevScore;

    private void Awake()
    {
        
        var objectPooler = ObjectPooler.Instance; 
        objectPooler.InstantiatePool();
    }

    void Start()
    {
        Time.timeScale = 0f;

        //Disable Game UI Canvases
        GamePauseCanvas.enabled = false;
        GameOverCanvas.enabled = false;
        currentScore = 0;

        //Set initial score to 0 & display current score in game as 0
        currentScore = 0;
        ScoreText.text = currentScore.ToString();

        //Load Previous Hi Score
        HiScoreText.text = PlayerPrefs.GetFloat("HighScore").ToString();


    }


    public void UpdateScore()
    {
        currentScore += 100; 
        ScoreText.text = currentScore.ToString();
    }

    public void Play()
    {
        ReadyCanvas.enabled = false;
        Time.timeScale = 1f; 
        TetrisGrid.StartGame();
    }
    
    public void Pause()
    {
        if (GamePauseCanvas.isActiveAndEnabled || ReadyCanvas.isActiveAndEnabled)
        {
            GamePauseCanvas.enabled = false;
            Time.timeScale = 1f; 
        }
        
        else
        {
            GamePauseCanvas.enabled = true;
            Time.timeScale = 0f;
        }
    }

    public void GameOver()
    {
        GameOverCanvas.enabled = true;
        Time.timeScale = 0f;

        if (currentScore > PlayerPrefs.GetFloat("HighScore"))
        {
            PlayerPrefs.SetFloat("HighScore", currentScore);
            Debug.Log("High Score of " + currentScore.ToString() + " saved");
        }
        
        
    }
    
    public void Restart()
    {
        TetrisGrid.RefreshGrid(); 
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void Quit()
    {
        Application.Quit();
    }

  
}
