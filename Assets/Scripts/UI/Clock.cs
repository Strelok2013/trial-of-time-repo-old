using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class Clock : MonoBehaviour
{
    public float m_clockSpeedModifier = 1.0f;
    public float m_tickAmount = 1.0f;

    float m_timer;

    bool m_clockTickToggle;

    Transform m_secondHand;
    Transform m_minuteHand;
    Transform m_clockFill;

    float m_visualTimer;


    // Start is called before the first frame update
    void Start()
    {
        m_clockTickToggle = true;
        m_visualTimer = GameManager.s_timer;
        m_secondHand = transform.Find("SecondHand");
        m_minuteHand = transform.Find("MinuteHand");
        m_clockFill = transform.Find("ClockFill");
    }

    // Update is called once per frame
    void Update()
    {
        if(m_clockTickToggle)
        {
            m_timer += Time.deltaTime;
            if (m_timer > m_tickAmount)
            {
                m_visualTimer -= m_tickAmount;
                m_secondHand.transform.eulerAngles = new Vector3(0, 0, m_visualTimer * 6.0f);
                m_minuteHand.transform.eulerAngles = new Vector3(0, 0, m_visualTimer * 0.5f);
                m_clockFill.GetComponent<Image>().fillAmount = m_visualTimer * 1 / 60.0f;
                m_timer = 0.0f;
            }

            if (GameManager.s_timer <= 60.0f)
            {
                m_clockFill.GetComponent<Image>().enabled = true;
            }

            if (GameManager.s_timer > 60.0f)
            {
                m_clockFill.GetComponent<Image>().enabled = false;
                m_clockFill.GetComponent<Image>().fillAmount = 1.0f;
            }
        }
        else
        {
            m_visualTimer = 0;
            m_secondHand.transform.eulerAngles = new Vector3(0, 0, m_visualTimer * 6.0f);
            m_minuteHand.transform.eulerAngles = new Vector3(0, 0, m_visualTimer * 0.5f);
            m_clockFill.GetComponent<Image>().fillAmount = m_visualTimer * 1 / 60.0f;
        }
    }

    public void Updateclock(float timeset)
    {
        m_visualTimer = timeset;
    }

    public void StopClockTick()
    {
        m_clockTickToggle = false;
    }
}
