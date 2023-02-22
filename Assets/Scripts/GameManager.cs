using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    List<GameObject> m_relicsInScene;

    public float m_timeLimit = 60.0f;
    public static float s_timer;
    public static bool s_lightTrigger = false;

    
    public int m_relicsNeeded = 1;

    public TextMeshProUGUI m_timerDisplay;
    public UIManager m_UIManagerRef;
    public Altar m_altar;

    public bool m_delayToggle;

    public float m_visualUpdateDelay = 2.1f;

    float m_visualDelayTimer;

    public GameObject m_playerRef;


    int m_relicsCollected = 0;

    public static bool s_gameWon = false;
    public static bool s_gameLost = false;


    // Start is called before the first frame update
    void Start()
    {
        s_timer = m_timeLimit;
        m_UIManagerRef.SetRelicCounter(m_relicsNeeded);
        m_UIManagerRef.UpdateClock(s_timer);
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(m_delayToggle)
        {
            m_visualDelayTimer += Time.deltaTime;
        }

        if(s_gameWon)
        {
            // The game is won, you cant lose
            // Disable player control
            m_playerRef.GetComponent<PlayerMovement>().TurnOffPlayerMovement();
            m_UIManagerRef.ToggleWinUI();
        }

        if (s_gameLost)
        {
            // The game is lost, you can't win
            // Also disable player control
            m_playerRef.GetComponent<PlayerMovement>().TurnOffPlayerMovement();
            m_playerRef.GetComponent<PlayerMovement>().PlayerDeathAnim();
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            PauseGame();
        }

        if (!s_gameWon || !s_gameLost)
        {
            if (s_timer < 0)
            {
                EndGame();
            }

            if (m_relicsCollected == m_relicsNeeded)
            {
                //m_winTimer += Time.deltaTime;
                WinGame();
            }
        }
    }

    void FixedUpdate()
    {
        s_timer -= Time.deltaTime;
        Debug.Log(s_timer);
    }

    public void AddTime(float timeToAdd)
    {
            UIManager.s_glowToggle = true;
            s_timer += timeToAdd;
            m_relicsCollected++;
            m_UIManagerRef.UpdateRelicCounter(m_relicsCollected);
            m_UIManagerRef.ToggleVisualDelayTimer();
    }

    public void SubtractTime(float timeToReduce)
    {
        s_timer -= timeToReduce;
    }

    void PauseGame()
    {
        m_UIManagerRef.TogglePauseMenu();
    }

    void WinGame()
    {
        s_gameWon = true;
        m_UIManagerRef.ToggleWinMenu();
    }

    void EndGame()
    {
        s_gameLost = true;
        m_UIManagerRef.ToggleLoseMenu();
    }

}
