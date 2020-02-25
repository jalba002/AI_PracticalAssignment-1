using UnityEngine;
using System.Collections;

public class ZOMBIE_BlackBoard : MonoBehaviour
{
    [Header("Zombie Patrolling FSM parameters")]
    public GameObject hazardZone;

    void Awake()
    {
        if (hazardZone == null)
            hazardZone = GameObject.Find("HazardZone");
        if (hazardZone == null)
            Debug.LogError("HazardZone has been NOT found in " + this);

    }
}