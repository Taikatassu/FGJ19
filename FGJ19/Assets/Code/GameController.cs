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

    void Start()
    {
        em = EventManager._instance;
    }


    void Update()
    {
        if (timerBar.value > 0)
        {
            timerBar.value -= decreasableAmount * Time.deltaTime;
        }
        else if (!gameOvered)
        {
            //DISABLE CONTROLS!!
            GameOver();
            em.BroadcastGameOver();
        }
    }

    void GameOver()
    {
        gameOvered = true;
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
        gameOvered = false;
        em.BroadcastKeepGoing();
    }
}
