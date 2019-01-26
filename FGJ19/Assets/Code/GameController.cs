using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{

    public Slider timerBar;
    public GameObject gameOverUI;
    public GameObject disableControls;
    public float decreasableAmount = 0.016f;
    bool gameOvered = false;



    void Start()
    {

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
            //MOVE CAMERA TO PLANET!!
            GameOver();
        }
    }

    void GameOver()
    {
        gameOvered = true;
        disableControls.SetActive(true);
        gameOverUI.SetActive(true);
    }

    public void Restart()
    {
        SceneManager.LoadScene("Scene00");
    }

    public void KeepGoing()
    {
        gameOverUI.SetActive(false);
        disableControls.SetActive(false);
        timerBar.value = 1;
        gameOvered = false;
        //MOVE CAMERA BACK TO PLAYER!!
    }
}
