using UnityEngine;
using System.Collections;

public class ZOMBIE_BlackBoard : MonoBehaviour
{
    [Header("Zombie Patrolling FSM parameters")]
    public float civilianDetectableRadius = 8.0f;
    public float nearbyCivilianRadius = .4f;
    public float fastVelocity = 1.2f;
    public string civilianTag = "Civilian";
    public GameObject hazardZone;

    [Header("Zombie Spotting Flare FSM parameters")]
    public float flareDetectableRadius = 10.0f;
    public float nearbyflareRadius = .4f;

    void Awake()
    {
        if (hazardZone == null)
            hazardZone = GameObject.Find("HazardZone");
        if (hazardZone == null)
            Debug.LogError("HazardZone has been NOT found in " + this);

    }
}