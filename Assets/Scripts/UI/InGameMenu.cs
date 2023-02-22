using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InGameMenu : MonoBehaviour
{
    public string m_mainMenuScene;
    public string m_startGameScene;
    public AudioSource m_audioSource;

    void Start()
    {
        if(!Cursor.visible)
        {
            Cursor.visible = true;
        }
    }

    public void UnPause()
    {
        Time.timeScale = 1;
        gameObject.SetActive(false);
    }

    public void StartGame()
    {
        SceneManager.LoadScene(m_startGameScene);
    }

    public void ReturnToMainMenu()
    {
        SceneManager.LoadScene(m_mainMenuScene);
        Time.timeScale = 1;
    }
}
