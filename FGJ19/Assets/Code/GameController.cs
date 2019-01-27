using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    private EventManager em;
    bool gameOvered = false;

    public Slider timerBar;
    public GameObject gameOverUI;
    public GameObject disableControls;
    public float decreasableAmount = 0.016f;
    public GameObject disableOnGameOver;
    public GameObject mainMenu;
    public Player player;
    bool gameStarted = false;


    void Start()
    {
        em = EventManager._instance;
        disableOnGameOver.SetActive(false);
    }


    void Update()
    {
        if (timerBar.value > 0 && gameStarted)
        {
            timerBar.value -= decreasableAmount * Time.deltaTime;
        }
        else if (!gameOvered && gameStarted)
        {
            //DISABLE CONTROLS!!
            GameOver();
            em.BroadcastGameOver();
        }
    }

    void GameOver()
    {
        gameOvered = true;
        player.controlsEnabled = false;
        disableControls.SetActive(true);
        gameOverUI.SetActive(true);
        disableOnGameOver.SetActive(false);
    }

    public void Restart()
    {
        SceneManager.LoadScene("Scene00");
    }

    public void KeepGoing()
    {
        disableOnGameOver.SetActive(true);
        gameOverUI.SetActive(false);
        disableControls.SetActive(false);
        timerBar.value = 1;
        player.controlsEnabled = true;
        gameOvered = false;
        em.BroadcastKeepGoing();
    }

    public void StartGame()
    {
        gameStarted = true;
        mainMenu.SetActive(false);
        disableOnGameOver.SetActive(true);
        player.controlsEnabled = true;
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
