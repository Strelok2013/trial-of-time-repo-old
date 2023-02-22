using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightTrigger : MonoBehaviour
{


    Color m_ambientColor;
    float m_ambientIntensity;

    // Start is called before the first frame update
    void Start()
    {
        //RenderSettings.ambientSkyColor = new Color(0, 0, 0);
        m_ambientColor = new Color(0.3921f, 0.4901f, 0.5882f);
        m_ambientIntensity = 0.0f;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            if(GameManager.s_lightTrigger)
            {
                SwitchAmbientLightOn();
            }
            else
            {
                SwitchAmbientLightOff();
            }
        }

    }

    void SwitchAmbientLightOn()
    {
        GameManager.s_lightTrigger = false;
        RenderSettings.ambientLight = m_ambientColor;
        RenderSettings.ambientIntensity = m_ambientIntensity;
    }

    void SwitchAmbientLightOff()
    {
        GameManager.s_lightTrigger = true;
        RenderSettings.ambientLight= new Color(0, 0, 0);
        RenderSettings.ambientIntensity = m_ambientIntensity;
    }

}
