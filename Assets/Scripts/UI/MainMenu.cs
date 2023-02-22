using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public string m_startGameScene;



    public void StartGame()
    {
        SceneManager.LoadScene(m_startGameScene);
    }

    public void Quit()
    {
        Application.Quit();
    }
}
