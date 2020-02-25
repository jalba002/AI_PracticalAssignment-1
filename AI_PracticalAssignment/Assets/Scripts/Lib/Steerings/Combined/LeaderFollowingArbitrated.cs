using UnityEngine;

namespace Steerings
{
    // combine Keep Position + Linear Repulsion
    public class LeaderFollowingArbitrated : SteeringBehaviour
    {
        public RotationalPolicy rotationalPolicy = RotationalPolicy.LWYGI;

        // parameters required by KeepPosition
        public GameObject target;
        public float requiredDistance = 10.0f;
        public float requiredAngle = 180.0f;

        // parameters required by LinearRepulsion
        public string idTag = "REPULSIVE";
        public float repulsionThreshold = 8.0f;

        public override SteeringOutput GetSteering()
        {
            // no KS? get it
            if (this.ownKS == null) this.ownKS = GetComponent<KinematicState>();

            SteeringOutput result = LeaderFollowingArbitrated.GetSteering(this.ownKS, this.target, this.requiredDistance, this.requiredAngle, this.idTag, this.repulsionThreshold);
            base.applyRotationalPolicy(rotationalPolicy, result, target);
            return result;
        }

        public static SteeringOutput GetSteering(KinematicState ownKS, GameObject target, float distance = 10.0f, float angle = 180.0f, string tag = "REPULSIVE", float repulsionTh = 8.0f)
        {
            SteeringOutput so = LinearRepulsion.GetSteering(ownKS, tag, repulsionTh);

            // Give priority to linear repulsion
            // (if linear repulsion is not SteeringBehaviour.NULL_STEERING return linear repulsion
            if (so != NULL_STEERING)
                return so;

            // else return Keep Position
            return KeepPosition.GetSteering(ownKS, target, distance, angle);
        }
    }
}
