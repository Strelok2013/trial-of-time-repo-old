using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    // The plan for this thing is to have it affect all the in-game UI elements

    int m_relicsNeeded;

    // Relic collection glow
    Image m_clockInnerGlow;
    Image m_clockOuterGlow;

    // Player hit swirl and glow
    Image m_clockSwirl;
    Image m_innerClockHurtGlow;
    Image m_outerClockHurtGlow;

    // Clock destroy glow and cracks
    Image m_clockDestroyCracks;
    Image m_clockDestroyGlow;

    public Clock m_clock;
    Transform m_clockFace;

    GameObject m_pauseMenu;
    GameObject m_winMenu;
    GameObject m_loseMenu;
    GameObject m_brokenClock;

    float m_glowTimer;
    float m_swirlAlphaTimer;
    float m_swirlRotationTimer;
    float m_destroyTimer;

    public TextMeshProUGUI m_pickupPrompt;
    public TextMeshProUGUI m_dropPrompt;
    public TextMeshProUGUI m_relicCounter;
    public static bool s_showPrompt = false;
    public static bool s_showDropPrompt = false;

    bool m_visualDelayToggle = false; // When this is tripped, start counting the thing to update the visuals
    bool m_winGameToggle;
    public float m_visualDelay = 2.1f;
    float m_visualDelayTimer;

    // This is for when the player collects a relic
    public static bool s_glowToggle = false; // glow is for the relic
    public float m_glowSpeedModifier = 1.0f;
    public float m_glowDuration = 1.0f;
    float m_destroyDuration = 1.0f;

    // This is for when the player is hit
    public static bool s_swirlToggle = false; // swirl is for when the player gets hit
    public float m_swirlDuration = 1.0f;
    public float m_swirlRotationSpeed = 1.0f;


    public AnimationCurve m_clockHurtAlpha;
    public AnimationCurve m_clockSwirlRotation;
    public AnimationCurve m_clockScoreAlpha;
    public AnimationCurve m_clockDestroyAlpha;

    [SerializeField]
    AudioClip m_clockTick;
    [SerializeField]
    AudioClip m_clockTickEX;

    public AudioSource m_clockTickSource;
    public AudioSource m_clockTickEXSource;

    bool m_isClockTicking = true;
    bool m_toggleChange = false;
    bool m_clockLessThan60 = false;
    bool m_gamePaused = false;

    // Start is called before the first frame update
    void Start()
    {

        // Setting the menus that appear in game
        m_pauseMenu = gameObject.transform.Find("PauseMenu").gameObject;
         m_winMenu = gameObject.transform.Find("WinMenu").gameObject;
         m_loseMenu = gameObject.transform.Find("LoseMenu").gameObject;
        m_brokenClock = gameObject.transform.Find("BrokenClock").gameObject;

        // Setting the hurt glow and swirl
        Transform newClock = transform.Find("NewClock");
        m_clockFace = newClock.transform.Find("ClockFace");
        m_clockSwirl = m_clockFace.Find("ClockSwirl").GetComponent<Image>();
        m_innerClockHurtGlow = m_clockFace.Find("InnerClockHurtGlow").GetComponent<Image>();
        m_outerClockHurtGlow = m_clockFace.Find("OuterClockHurtGlow").GetComponent<Image>();

        // Setting the clock destroy glow and cracks
        m_clockDestroyCracks = newClock.Find("ClockDestroyCracks").GetComponent<Image>();
        m_clockDestroyGlow = newClock.Find("ClockDestroyGlow").GetComponent<Image>();

        //Setting the collection glow
        m_clockInnerGlow = newClock.transform.Find("ClockInnerGlow").GetComponent<Image>();
        m_clockOuterGlow = newClock.transform.Find("ClockOuterGlow").GetComponent<Image>();
        m_pickupPrompt.text = "Press 'Space' to pick up";
        m_dropPrompt.text = "Press 'Space' to drop";

        m_clockTickSource = gameObject.GetComponent<AudioSource>();
        m_clockTickSource.clip = m_clockTick;
        m_clockTickEXSource.clip = m_clockTickEX;
        m_clockTickSource.Play();

        ResetClockHurt();
        ResetClockGlow();
        ResetClocKDestoy();
    }

    // Update is called once per frame
    void Update()
    {
       
        if(!m_gamePaused)
        {
            if(m_clockLessThan60)
            {
                if(!m_clockTickEXSource.isPlaying)
                { 
                    m_clockTickSource.Stop();
                    m_clockTickEXSource.Play();
                }
            }
            else
            {
                if (!m_clockTickSource.isPlaying)
                {
                    m_clockTickEXSource.Stop();
                    m_clockTickSource.Play();
                }
            }
        }
        else
        {
            m_clockTickSource.Stop();
            m_clockTickEXSource.Stop();
        }

        if(GameManager.s_timer < 60.0f)
        {
            m_clockLessThan60 = true;
        }
        else
        {
            m_clockLessThan60 = false;
        }

        
        //if (m_isClockTicking && m_toggleChange)
        //{
        //    m_AudioSourceRef.clip = m_clockTickEX;
        //    m_AudioSourceRef.Play();
        //    m_toggleChange = false;
        //}
        //if (m_isClockTicking && m_toggleChange)
        //{
        //    m_AudioSourceRef.clip = m_clockTick;
        //    m_AudioSourceRef.Play();
        //    m_toggleChange = false;
        //}




        if(s_glowToggle)
        {
            if(m_visualDelayToggle)
            {
                m_visualDelayTimer += Time.deltaTime;
            }
            if (m_visualDelayTimer > m_visualDelay)
            {
                if (m_glowTimer < m_glowDuration)
                {
                    m_glowTimer += Time.deltaTime;
                    m_clockInnerGlow.color = new Color(1, 1, 1, m_clockScoreAlpha.Evaluate(m_glowTimer));
                    m_clockOuterGlow.color = new Color(1, 1, 1, m_clockScoreAlpha.Evaluate(m_glowTimer));
                    m_clock.Updateclock(GameManager.s_timer);
                    if(m_winGameToggle)
                    {
                        m_clock.StopClockTick();
                        m_destroyTimer += Time.deltaTime;
                        m_clockDestroyGlow.color = new Color(1, 1, 1, m_clockDestroyAlpha.Evaluate(m_destroyTimer));
                        m_clockDestroyCracks.color = new Color(1, 1, 1, m_clockDestroyAlpha.Evaluate(m_destroyTimer));
                        //m_clockInnerGlow.gameObject.SetActive(false);
                        //m_clockOuterGlow.gameObject.SetActive(false);
                        if(m_destroyTimer > m_destroyDuration)
                        {
                            m_clock.gameObject.SetActive(false);
                            m_clockDestroyCracks.gameObject.SetActive(false);
                            m_clockDestroyGlow.gameObject.SetActive(false);
                            m_brokenClock.gameObject.SetActive(true);
                        }
                    }
                }
                else
                {
                    m_glowTimer = 0.0f;
                    s_glowToggle = false;
                    m_visualDelayTimer = 0.0f;
                    m_visualDelayToggle = false;
                }
            }
        }


        if (s_swirlToggle)
        {
            if(m_swirlAlphaTimer < m_swirlDuration)
            {
                m_swirlAlphaTimer += Time.deltaTime;
                m_swirlRotationTimer += Time.deltaTime;
                m_clockSwirl.color = new Color(1, 1, 1, m_clockHurtAlpha.Evaluate(m_swirlAlphaTimer));
                m_innerClockHurtGlow.color = new Color(1, 1, 1, m_clockHurtAlpha.Evaluate(m_swirlAlphaTimer));
                m_outerClockHurtGlow.color = new Color(1, 1, 1, m_clockHurtAlpha.Evaluate(m_swirlAlphaTimer));
                m_clockSwirl.transform.eulerAngles = new Vector3(0, 0, m_clockSwirlRotation.Evaluate(m_swirlRotationTimer) * m_swirlRotationSpeed);
                m_clock.Updateclock(GameManager.s_timer);
            }
            else
            {
                m_swirlAlphaTimer = 0.0f;
                m_swirlRotationTimer = 0.0f;
                s_swirlToggle = false;
            }
        }



        if(s_showPrompt)
        {
            m_pickupPrompt.gameObject.SetActive(true);
        }
        else
        {
            m_pickupPrompt.gameObject.SetActive(false);
        }

        if(s_showDropPrompt)
        {
            m_dropPrompt.gameObject.SetActive(true);
        }
        else
        {
            m_dropPrompt.gameObject.SetActive(false);
        }
    }
    void ResetClockHurt()
    {
        m_clockSwirl.color = new Color(1, 1, 1, 0);
        m_outerClockHurtGlow.color = new Color(1, 1, 1, 0);
        m_innerClockHurtGlow.color = new Color(1, 1, 1, 0);
    }

    void ResetClockGlow()
    {
        m_clockOuterGlow.color = new Color(1, 1, 1, 0);
        m_clockInnerGlow.color = new Color(1, 1, 1, 0);
    }

    void ResetClocKDestoy()
    {
        m_clockDestroyCracks.color = new Color(1, 1, 1, 0);
        m_clockDestroyGlow.color = new Color(1, 1, 1, 0);
    }

    public void TogglePauseMenu()
    {
        if(!m_pauseMenu.activeSelf)
        {
            if(!Cursor.visible)
            {
                Cursor.visible = true;
            }
            Time.timeScale = 0;
            m_pauseMenu.SetActive(true);
            m_gamePaused = true;
        }
        else
        {
            if (Cursor.visible)
            {
                Cursor.visible = false;
            }
            Time.timeScale = 1;
            m_pauseMenu.SetActive(false);
            m_gamePaused = false;
        }
    }

    public void ToggleWinMenu()
    {
        if (!m_winMenu.activeSelf)
        {
            m_winMenu.SetActive(true);
        }
        if(m_pauseMenu.activeSelf)
        {
            m_pauseMenu.SetActive(false);
        }
        if (!Cursor.visible)
        {
            Cursor.visible = true;
        }

        m_clockTickSource.Stop();
        m_clockTickEXSource.Stop();
    }

    public void ToggleLoseMenu()
    {
        if (!m_loseMenu.activeSelf)
        {
            m_loseMenu.SetActive(true);
        }
        if (m_pauseMenu.activeSelf)
        {
            m_pauseMenu.SetActive(false);
        }
        if (!Cursor.visible)
        {
            Cursor.visible = true;
        }

        m_clockFace.GetComponent<Clock>().StopClockTick();
        m_clockTickSource.Stop();
        m_clockTickEXSource.Stop();
    }

    public void SetRelicCounter(int relicsNeeded)
    {
        m_relicsNeeded = relicsNeeded;
        m_relicCounter.text = "0/" + m_relicsNeeded.ToString();
    }

    public void UpdateRelicCounter(int relicsCollected)
    {
        m_relicCounter.text = relicsCollected.ToString() + "/" + m_relicsNeeded.ToString();
    }

    public void ToggleVisualDelayTimer()
    {
        m_visualDelayToggle = true;
    }

    public void ToggleWinUI()
    {
        m_winGameToggle = true;
    }



    public void UpdateClock(float timeSet)
    {
        m_clock.Updateclock(timeSet);
    }
}
