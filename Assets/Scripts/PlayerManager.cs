using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    #region Singleton

    public static PlayerManager s_instance;
    private void Awake()
    {
        s_instance = this;
    }

    #endregion

    public GameObject m_gameObject_player;
}
