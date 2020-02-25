using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    public GameObject flarePrefab;
    public int flares = 10;
    public Transform damagePoint;


    void Start()
    {

    }


    void Update()
    {
        if (Input.GetButtonDown("Fire2") && flares > 0)
        {
            ThrowFlare();
        }
    }

    void ThrowFlare()
    {
        flares--;
        GameObject newFlare = Instantiate(flarePrefab, damagePoint.position, damagePoint.rotation);
    }
}
