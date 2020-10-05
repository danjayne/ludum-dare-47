using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public static bool GameIsPaused = false;
    public GameObject pauseMenuUI;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (GameIsPaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }

    private void ResumeGame()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        GameIsPaused = false;
        AudioManager.Instance.SetPitch(1f);
    }

    private void PauseGame()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        GameIsPaused = true;
        AudioManager.Instance.SetPitch(.9f);
    }

    public void ButtonPressResumeGame()
    {
        ResumeGame();
    }

    public void ButtonPressMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void ButtonPressQuitGame()
    {
        Application.Quit();
    }
}
