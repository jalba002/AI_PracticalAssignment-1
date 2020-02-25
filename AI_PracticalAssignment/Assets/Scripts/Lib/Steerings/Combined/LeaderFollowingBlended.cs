using UnityEngine;

namespace Steerings
{
    public class LeaderFollowingBlended : SteeringBehaviour
    {
        public RotationalPolicy rotationalPolicy = RotationalPolicy.LWYGI;

        // parameters required by KeepPosition
        public GameObject target;
        public float requiredDistance = 10.0f;
        public float requiredAngle = 180.0f;

        // parameters required by LinearRepulsion
        public string idTag = "REPULSIVE";
        public float repulsionThreshold = 8.0f;

        //weight for LinearRepulsion
        public float wlr = 0.5f;

        public override SteeringOutput GetSteering()
        {
            // no KS? get it
            if (this.ownKS == null) this.ownKS = GetComponent<KinematicState>();

            SteeringOutput result = LeaderFollowingBlended.GetSteering(this.ownKS, this.target, this.requiredDistance, this.requiredAngle, this.idTag, this.repulsionThreshold, this.wlr);
            base.applyRotationalPolicy(rotationalPolicy, result, target);
            return result;
        }

        public static SteeringOutput GetSteering(KinematicState ownKS, GameObject target, float distance = 10.0f, float angle = 180.0f, string tag = "REPULSIVE", float repulsionTh = 8.0f, float repulsiveWeight = 0.5f)
        {
            // compute both steerings
            SteeringOutput lr = LinearRepulsion.GetSteering(ownKS, tag, repulsionTh);
            SteeringOutput kp = KeepPosition.GetSteering(ownKS, target, distance, angle);

            // blend result
            SteeringOutput result = new SteeringOutput();

            // (if one is SteeringBehaviour.NULL_STEERING return the other
            if (lr == NULL_STEERING)
                return kp;
            else if (kp == NULL_STEERING)
                return lr;

            // if none is SteeringBehaviour.NULL_STEERING blend with weights wlr and 1-wlr)
            result.linearAcceleration = kp.linearAcceleration * (1 - repulsiveWeight) + lr.linearAcceleration * repulsiveWeight;
            return result;
        }
    }
}