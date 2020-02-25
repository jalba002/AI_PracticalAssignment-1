using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Events;

public class GameController : Singleton<GameController>
{

    [System.Serializable]
    public class CIVILIAN_GLOBAL_BlackBoard
    {
        [Header("Civilian Following FSM parameters")]
        public int MaxCiviliansFollowing = 4;
        public int CivilianFollowingCounter = 0;
    }

    public CIVILIAN_GLOBAL_BlackBoard civilianGlobalBB = new CIVILIAN_GLOBAL_BlackBoard();

    [Header("Required attributes")]
    public GameObject playerController;

    [HideInInspector] public float civiliansSaved;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            Quit();
    }

    public void Quit()
    {
        Application.Quit();

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}