using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CameraController : MonoBehaviour
{
    

    public PlayerMovement m_player;
    public float m_cameraHeight = 10.0f;
    public float m_speedMultiplier = 10.0f;


    float m_screenWidth;
    float m_screenheight;

    Vector3 worldPos;
    public GameObject m_testGameObject;
    

    Vector2 m_playerPosition = Vector2.zero;
    Transform m_cameraTransform;
    private void Awake()
    {
        m_screenheight = Screen.height;
        m_screenWidth = Screen.width;
    }

    // Start is called before the first frame update
    void Start()
    {
        m_cameraTransform = gameObject.GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {


        m_playerPosition = m_player.PlayerRoom();
   
        MoveCamera();



    }

    public void MoveCamera()
    {
        float timer = 0;
        float transitionAmount = 0.5f; ;
        //if (m_timer > m_transitionTime) // Reset the timer
        //{
        //    m_timer = 0;
        //}

        timer += Time.deltaTime;

        float sineOffset = -Mathf.PI / 2;

        transitionAmount += Mathf.Sin((timer * m_speedMultiplier * Mathf.PI)+ sineOffset) * 0.5f;

        

        Vector3 currentCameraPos = m_cameraTransform.position;
        Vector3 targetCameraPos = new Vector3(m_playerPosition.x * 17, m_cameraHeight, m_playerPosition.y * 9);


        //cameraTransform.position = new Vector3(playerPosition.x * 10, cameraHeight, playerPosition.y * 10);
        m_cameraTransform.position = Vector3.Lerp(currentCameraPos, targetCameraPos, transitionAmount);
    }


    private void OnDrawGizmos()
    {
        Gizmos.DrawIcon(worldPos, "");
    }

}
