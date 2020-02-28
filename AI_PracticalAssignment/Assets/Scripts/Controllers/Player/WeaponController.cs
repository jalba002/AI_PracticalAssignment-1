using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    public GameObject flarePrefab;
    public int flares = 10;
    public Transform damagePoint;

    void Update()
    {
        if (Input.GetButtonDown("Fire1") && flares > 0 && !GameController.Instance.gamePaused)
            ThrowFlare();
    }

    void ThrowFlare()
    {
        flares--;
        GameController.Instance.CanvasController.FlareCounterUpdate(flares);
        GameObject newFlare = Instantiate(flarePrefab, damagePoint.position, damagePoint.rotation);
    }
}
