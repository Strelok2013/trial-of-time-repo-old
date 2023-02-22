using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class Torch : MonoBehaviour
{
    Light m_torch;
    bool m_activeTorch = false;
    ParticleSystem m_particles;

    GameObject m_playerRootObject;
    // Start is called before the first frame update
    void Start()
    {
        m_particles = GetComponentInChildren<ParticleSystem>();
        m_torch = GetComponent<Light>();
    }

    // Update is called once per frame
    void Update()
    {
        if(!m_activeTorch)
        {
            ToggleTorch();
        }
    }

    void ToggleTorch()
    {
        if(transform.parent != null)
        {
            m_torch.enabled = true;
            m_playerRootObject = gameObject.GetComponent<PickUp>().GetPlayerRootObject();
            Transform dumbThing = m_playerRootObject.transform.Find("obj_player@T-Pose");
            dumbThing.transform.Find("Character").GetComponent<SkinnedMeshRenderer>().shadowCastingMode = ShadowCastingMode.Off;
            m_activeTorch = true;
        }

    }

    /*private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            this.gameObject.GetComponent<PickUp>().PickUpObject(collision.gameObject);
            torch.enabled = !torch.enabled;
        }
    }*/
}
