using UnityEngine;
using System.Collections;

public class CIVILIAN_BlackBoard : MonoBehaviour
{
    [Header("Civilian Camping FSM parameters")]
    public float nearbyZombieRadius = 20.0f;
    public float farAwayZombieRadius = 30.0f;
    public string zombieTag = "Zombie";
    public GameObject campFire;

    [Header("Civilian Following FSM parameters")]
    public float PlayerDetectionRadius = 5.0f;
    public float farAwayPlayerRadius = 7.5f;
    public float fastVelocity = 1.2f;
    public bool followingPlayer;


    [Header("Civilian Militar Base FSM parameters")]
    public float MilitarBaseDetectableRadius = 20.0f;
    public float nearbyMilitarBaseRadius = .5f;
    public GameObject militarBase;

    void Awake()
    {
        if (campFire == null)
        {
            campFire = GameObject.Find("CampFire");

            if (campFire == null)
                Debug.Log("CampFire has been NOT found in " + this);
        }

        if (militarBase == null)
        {
            militarBase = GameObject.FindGameObjectWithTag("MilitarBase");

            if (militarBase == null)
                Debug.Log("MilitarBase has been NOT found in " + this);
        }
    }
}