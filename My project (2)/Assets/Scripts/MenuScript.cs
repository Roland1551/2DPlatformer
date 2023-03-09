using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuScript : MonoBehaviour
{
    public static bool GameIsPaused = false;
    public GameObject pauseMenuUI;
    public GameObject deathMenuUI;
    public bool isAlive = true;

    public Text healthText;

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if(GameIsPaused)
            {
                Resume();
            }
            else
            {
                PlayerDied();
            }
        }

        if(isAlive == false)
        {
            PlayerDied();
        }
    }

    public void StartGame()
    {
        SceneManager.LoadScene("Level1");
    }
    public void ExitGame()
    {
        Debug.Log("Quit");
        Application.Quit();
    }

    //Pause,Resume
    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        GameIsPaused = false;
    }
    public void Pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        GameIsPaused = true;
    }

    //PlayerDeath,Restart
    public void RestartGame()
    {
        SceneManager.LoadScene("Level1");
        Time.timeScale = 1f;
    }
    public void PlayerDied()
    {
        deathMenuUI.SetActive(true);
        Time.timeScale = 0f;
    }
}
