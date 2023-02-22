using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerMovement : MonoBehaviour
{
    public Camera m_mainCamera;
    public float m_playerAcceleration = 1.0f;
    public float m_playerSpeedCap = 1.0f;
    public float m_invulnerabilityTimer = 3.0f;
    public Animator m_playerAnimation;

    Rigidbody m_playerBody;
    InventorySystem m_inventorySysRef;

    public bool m_toggleKinematicMovement;


    float m_timer = 0.0f;

    public bool m_hasCompass = false;

    bool m_playerMovementToggle = true;
    bool m_playerDead = false;

    bool m_wKey = false;
    bool m_aKey = false;
    bool m_sKey = false;
    bool m_dKey = false;

    public Sound[] m_audioSounds;

    private void Awake()
    {
        foreach(Sound sound in m_audioSounds)
        {
            sound.m_source = gameObject.AddComponent<AudioSource>();
            sound.m_source.clip = sound.m_clip;
            sound.m_source.volume = sound.m_volume;
            sound.m_source.pitch = sound.m_pitch;
            sound.m_source.loop = sound.m_loop;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        m_playerBody = gameObject.GetComponent<Rigidbody>();
        m_inventorySysRef = gameObject.GetComponent<InventorySystem>();
    }

    // Update is called once per frame
    void Update()
    {
        m_timer += Time.deltaTime;

        if (m_playerMovementToggle)
        {
            if (Input.GetKey(KeyCode.W))
            {
                m_wKey = true;
            }
            else
            {
                m_wKey = false;
            }
            if (Input.GetKey(KeyCode.A))
            {
                m_aKey = true;
            }
            else
            {
                m_aKey = false;
            }
            if (Input.GetKey(KeyCode.S))
            {
                m_sKey = true;
            }
            else
            {
                m_sKey = false;
            }
            if (Input.GetKey(KeyCode.D))
            {
                m_dKey = true;
            }
            else
            {
                m_dKey = false;
            }
        }
        else
        {
            m_wKey = false;
            m_aKey = false;
            m_sKey = false;
            m_dKey = false;

        }

        if (m_playerBody.velocity.magnitude > 0.35f)
        {
            m_playerAnimation.SetBool("isRunning", true);
        }
        else
        {
            m_playerAnimation.SetBool("isRunning", false);
        }

        if(GameManager.s_timer <= 0)
        {
            m_playerDead = true;
        }
    }

    void FixedUpdate()
    {
        if(!m_toggleKinematicMovement)
        {
            Vector3 heading = Vector3.zero;

            if(m_wKey)
            {
                heading.z += 1;
            }
   
            if(m_aKey)
            {
                heading.x -= 1;
            }
            if(m_sKey)
            {
                heading.z -= 1;
            }
            if(m_dKey)
            {
                heading.x += 1;
            }



            heading = Vector3.Normalize(heading);

            gameObject.transform.LookAt(heading + gameObject.transform.position);

            transform.position += heading * m_playerAcceleration * Time.deltaTime;
        }
        else
        {
            Vector3 heading = Vector3.zero;
            
            if (m_wKey)
            {
                heading.z += 1;
            }

            if (m_aKey)
            {
                heading.x -= 1;
            }
            if (m_sKey)
            {
                heading.z -= 1;
            }
            if (m_dKey)
            {
                heading.x += 1;
            }

            heading = Vector3.Normalize(heading);

            gameObject.transform.LookAt(heading + gameObject.transform.position);

            if(m_playerBody.velocity.magnitude < m_playerSpeedCap)
            {
                m_playerBody.AddForce(heading * m_playerAcceleration, ForceMode.Acceleration);
            }
        }
    }

    public Vector2 PlayerRoom()
    {

        float xPos = transform.position.x + 8.5f;
        float zPos = transform.position.z + 4.5f;

        float xRoom = xPos / 17; // 5 is a placeholder, depends on the room width from its center
        float zRoom = zPos / 9;

        int xRoomNumber = (int)xRoom;
        int zRoomNumber = (int)zRoom;

        if(xPos < 0)
        {
            xRoomNumber -= 1;
        }
        if(zPos < 0)
        {
            zRoomNumber -= 1;
        }



        return new Vector2(xRoomNumber, zRoomNumber);
    }

    // Ok so basically the player needs to get hit and then knocked back
    // There also needs to be a grace period between each hit (ie invulnerability)
    public void DamagePlayer(Vector3 targetVec3, float knockback, float timeDamage) // If the player is hit by something, could add a member variable for knockback
    {
        if(!m_playerDead)
        { 
            if(m_timer > m_invulnerabilityTimer)
            {
                m_playerBody.AddForce(targetVec3 * knockback, ForceMode.Impulse);
                GameManager.s_timer -= timeDamage;
                UIManager.s_swirlToggle = true;
                m_timer = 0.0f;
                int randNum = Random.Range(0, 8);
                PlaySound(randNum);
            }
        }
    }

    public void TurnOffPlayerMovement()
    {
        m_playerMovementToggle = false; 
    }

    public void PlaySound(int randomNumber)
    {
        Sound s = m_audioSounds[randomNumber];
        if(s == null)
        {
            return;
        }
        s.m_source.Play();
    }

    public void PlayerPickUpAnim()
    {

    }

    public void PlayerDropAnim()
    {

    }

    public void PlayerDeathAnim()
    {
        m_playerAnimation.SetBool("isDead", true);
    }

    public void PlayerTorch()
    {

    }

}
