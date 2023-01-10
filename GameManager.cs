using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    public GameObject startButton, pauseButton, contButton, backButton, backButtonFinal, finalScoreTextbox, playerObject;
    public PlayerBehaviour playerBehaviour;
    public ObstacleSpawner obstacleSpawner;
    public TMP_Text scoreText, scoreLabel, finalScoreText;
    public TMP_Dropdown dropdownMenu;
    
    public float waitTime = 1.5F;
    public float countTimer;

    private bool enumRunOnce;
    private int score = 0;
    private int dropdownIndex;
    public int DropdownIndex{get; set;}
    public bool gameIsStarted = false;

    // Start is called before the first frame update
    void Start()
    {
        playerObject.SetActive(false);
        pauseButton.SetActive(false);
        contButton.SetActive(false);
        backButton.SetActive(false);
        backButtonFinal.SetActive(false);
        finalScoreTextbox.SetActive(false);
        scoreText.gameObject.SetActive(false);
        finalScoreText.gameObject.SetActive(false);
        scoreLabel.gameObject.SetActive(false);
        dropdownMenu.gameObject.SetActive(true);
        countTimer = waitTime;
        enumRunOnce = false;
        Time.timeScale = 0;
    }

    private void Update()
    {
        if(obstacleSpawner.gameStateIsOver == true && obstacleSpawner.initialState == false)
        {
            if(enumRunOnce == false)
            {
                StartCoroutine(wait());
                enumRunOnce = true;
            }
        }     
    }

    public void StartGame()
    {
        this.DropdownIndex = dropdownMenu.value;
        gameIsStarted = true;
        playerObject.SetActive(true);
        dropdownMenu.gameObject.SetActive(false);
        startButton.SetActive(false);
        pauseButton.SetActive(true);
        backButton.SetActive(true);
        scoreText.gameObject.SetActive(true);
        scoreLabel.gameObject.SetActive(true);
        obstacleSpawner.initialState = false;
        obstacleSpawner.gameStateIsOver = false;
        Time.timeScale = 1;
    }

    public void GameOver()
    {
        gameIsStarted = false;
        obstacleSpawner.gameStateIsOver = true;
        Time.timeScale = 0;
        finalScoreText.text = "Final Score: " + score.ToString();
        playerObject.SetActive(false);
        finalScoreTextbox.SetActive(true);
        backButtonFinal.SetActive(true);
        finalScoreText.gameObject.SetActive(true);
        pauseButton.SetActive(false);
        contButton.SetActive(false);
        backButton.SetActive(false);
        scoreText.gameObject.SetActive(false);
        scoreLabel.gameObject.SetActive(false);
    }

    public void PauseGame()
    {
        pauseButton.SetActive(false);
        contButton.SetActive(true);
        Time.timeScale = 0;
    }

    public void ContinueGame()
    {
        pauseButton.SetActive(true);
        contButton.SetActive(false);
        Time.timeScale = 1;
    }

    public void ScoreKeeper()
    {
        score += 1;
        scoreText.text = score.ToString();
    }

    public void BackButtonHandler()
    {
        obstacleSpawner.gameStateIsOver = true;
        Time.timeScale = 0;
        SceneManager.LoadScene(0);
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(0);
    }

    public int DDI()
    {
        return dropdownIndex;
    }

    IEnumerator wait()
    {
        yield return new WaitForSecondsRealtime (3);
        GameOver();
    }
}
