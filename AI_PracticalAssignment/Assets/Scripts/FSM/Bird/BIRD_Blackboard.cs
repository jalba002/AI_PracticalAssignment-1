using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BIRD_Blackboard : MonoBehaviour
{
    public GameObject blockingAttractor;
    public GameObject defaultAttractor;
    public float wanderingSeekWeight;
    public float blockingSeekWeight;
    public float maxWanderTime;
    public float maxBlockingTime;
    public float blockingWanderRate;
    public float wanderingWanderRate;
}
