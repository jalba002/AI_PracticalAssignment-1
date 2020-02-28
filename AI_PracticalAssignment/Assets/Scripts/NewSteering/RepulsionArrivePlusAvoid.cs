using UnityEngine;

namespace Steerings
{
    // combine Keep Position + Linear Repulsion
    public class RepulsionArrivePlusAvoid : SteeringBehaviour
    {
        public RotationalPolicy rotationalPolicy = RotationalPolicy.LWYGI;

        // parameters required by KeepPosition
        public float closeEnoughRadius = 5f;  // also know as targetRadius
        public float slowDownRadius = 20f;    // if inside this radius, slow down
        public float timeToDesiredSpeed = 0.1f;

        public GameObject target;

        // parameters required by LinearRepulsion
        public string idTag = "REPULSIVE";
        public float repulsionThreshold = 8.0f;

        public override SteeringOutput GetSteering()
        {
            // no KS? get it
            if (this.ownKS == null) this.ownKS = GetComponent<KinematicState>();

            SteeringOutput result = RepulsionArrivePlusAvoid.GetSteering(this.ownKS, this.target, this.closeEnoughRadius, this.slowDownRadius, this.timeToDesiredSpeed, this.idTag, this.repulsionThreshold);
            base.applyRotationalPolicy(rotationalPolicy, result, target);
            return result;
        }

        public static SteeringOutput GetSteering(KinematicState ownKS, GameObject target, float distance = 10.0f, float slowRadius = 20.0f, float disredSpeed = 0.1f, string tag = "REPULSIVE", float repulsionTh = 8.0f)
        {
            SteeringOutput so = LinearRepulsion.GetSteering(ownKS, tag, repulsionTh);

            // Give priority to linear repulsion
            // (if linear repulsion is not SteeringBehaviour.NULL_STEERING return linear repulsion
            if (so != NULL_STEERING)
                return so;

            // else return Keep Position
            return Arrive.GetSteering(ownKS, target, distance, disredSpeed);
        }
    }
}
