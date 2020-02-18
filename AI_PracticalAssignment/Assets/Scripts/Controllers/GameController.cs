using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Events;

public class GameController : Singleton<GameController>
{
    [Header("Required attributes")]
    public PlayerController playerController;


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