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

        [Header("Particle system")]
        public ParticleSystem civilianDead;
        public ParticleSystem civilianSaved;
    }

    [System.Serializable]
    public class ZOMBIE_GLOBAL_BlackBoard
    {
        [Header("Zombie Spotting Flare FSM parameters")]
        public GameObject flare;
        public float flareAnnounce = 20f;
        public float elapsedTime = 0;

        public void Update()
        {
            elapsedTime += Time.deltaTime;
            if (elapsedTime >= flareAnnounce)
                flare = null;
        }

        public void AnnounceFlare(GameObject flareLight)
        {
            flare = flareLight;
            elapsedTime = 0f;
        }
    }

    public CIVILIAN_GLOBAL_BlackBoard civilianGlobalBB = new CIVILIAN_GLOBAL_BlackBoard();
    public ZOMBIE_GLOBAL_BlackBoard zombieGlobalBB = new ZOMBIE_GLOBAL_BlackBoard();

    [Header("Required attributes")]
    public GameObject playerController;

    [HideInInspector] public float civiliansSaved;
    [HideInInspector] public bool gamePaused;
    [HideInInspector] public bool gameOver;
    public GameObject pauseMenu;
    public GameObject gameOverMenu;
    public CanvasController CanvasController;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !gamePaused)
            Pause();

        if (Input.GetKeyDown(KeyCode.R) && !gamePaused)
            GameOver();
    }

    void Pause()
    {
        pauseMenu.SetActive(true);
        gamePaused = true;
        Time.timeScale = 0;
    }

    public void GameOver()
    {
        gameOverMenu.SetActive(true);
        gamePaused = true;
        Time.timeScale = 0;
    }
}