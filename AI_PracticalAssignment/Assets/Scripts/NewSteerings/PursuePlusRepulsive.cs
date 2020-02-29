using UnityEngine;

namespace Steerings
{
    public class PursuePlusRepulsive : SteeringBehaviour
    {
        public RotationalPolicy rotationalPolicy = RotationalPolicy.LWYGI;

        // parameters required by Pursue
        public float maxPredictionTime = 3f;
        public GameObject target;

        // parameters required by LinearRepulsion
        public string idTag = "REPULSIVE";
        public float repulsionThreshold = 8.0f;

        public override SteeringOutput GetSteering()
        {
            // no KS? get it
            if (this.ownKS == null) this.ownKS = GetComponent<KinematicState>();

            SteeringOutput result = PursuePlusRepulsive.GetSteering(this.ownKS, this.target, this.maxPredictionTime, this.idTag, this.repulsionThreshold);
            base.applyRotationalPolicy(rotationalPolicy, result, target);
            return result;
        }

        public static SteeringOutput GetSteering(KinematicState ownKS, GameObject target, float predictionTime = 3.0f, string tag = "REPULSIVE", float repulsionTh = 8.0f)
        {
            SteeringOutput so = LinearRepulsion.GetSteering(ownKS, tag, repulsionTh);

            if (so != NULL_STEERING)
                return so;

            return Pursue.GetSteering(ownKS, target, predictionTime);
        }
    }
}