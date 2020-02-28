using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasController : MonoBehaviour
{
    public Text civiliansAlive;
    public Text civiliansAliveShadow;
    public Text civiliansSaved;
    public Text civiliansSavedShadow;
    public Text flareNumber;
    public Text flareNumberShadow;
    private GameObject[] numberOfCivilians;
    private int numberOfCiviliansScore;
    private int civilianSaved;
    private int numberOfFlares;


    private void Start()
    {
        numberOfCivilians = GameObject.FindGameObjectsWithTag("Civilian");
        numberOfFlares = FindObjectOfType<WeaponController>().flares;
        civilianSaved = 0;
        numberOfCiviliansScore = numberOfCivilians.Length;

        civiliansAlive.text = "" + numberOfCiviliansScore;
        civiliansAliveShadow.text = "" + numberOfCiviliansScore;
        civiliansSaved.text = "" + civilianSaved;
        civiliansSavedShadow.text = "" + civilianSaved;
        flareNumber.text = "" + numberOfFlares;
        flareNumberShadow.text = "" + numberOfFlares;
    }

    public void CivilianSavedUpdate()
    {
        civilianSaved++;
        civiliansSaved.text = "" + civilianSaved;
        civiliansSavedShadow.text = "" + civilianSaved;
    }

    public void CivilianDeadUpdate()
    {
        numberOfCiviliansScore--;
        civiliansAlive.text = "" + numberOfCiviliansScore;
        civiliansAliveShadow.text = "" + numberOfCiviliansScore;

        if (numberOfCiviliansScore <= 0)
            Invoke("CallGameOver", 2.5f);
    }

    public void FlareCounterUpdate(int flares)
    {
        flareNumber.text = "" + flares;
        flareNumberShadow.text = "" + flares;
    }

    void CallGameOver()
    {
        GameController.Instance.GameOver();
    }
}