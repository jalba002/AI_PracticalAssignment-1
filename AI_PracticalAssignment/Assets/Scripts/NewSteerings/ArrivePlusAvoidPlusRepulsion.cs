using UnityEngine;

namespace Steerings
{
    public class ArrivePlusAvoidPlusRepulsion : SteeringBehaviour
    {
        public RotationalPolicy rotationalPolicy = RotationalPolicy.FTI;

        // parameters required by Arrive
        public float closeEnoughRadius = 5f;
        public float slowDownRadius = 20f;
        public float timeToDesiredSpeed = 0.1f;
        public GameObject target;

        // parameters required by obstacle avoidance
        public bool showWhisker = true;
        public float lookAheadLength = 10f;
        public float avoidDistance = 10f;
        public float secondaryWhiskerAngle = 30f;
        public float secondaryWhiskerRatio = 0.7f;

        // parameters required by LinearRepulsion
        public string idTag = "REPULSIVE";
        public float repulsionThreshold = 8.0f;

        //weight for LinearRepulsion
        [Range(0f, 1f)] public float repulsionWeight = 0.5f;

        public override SteeringOutput GetSteering()
        {
            // no KS? get it
            if (this.ownKS == null) this.ownKS = GetComponent<KinematicState>();

            SteeringOutput result = ArrivePlusAvoidPlusRepulsion.GetSteering(this.ownKS, this.target, this.closeEnoughRadius, this.slowDownRadius,
                this.timeToDesiredSpeed, this.showWhisker, this.lookAheadLength, this.avoidDistance, this.secondaryWhiskerAngle, this.secondaryWhiskerRatio, this.idTag, this.repulsionThreshold, repulsionWeight);
            base.applyRotationalPolicy(rotationalPolicy, result, target);
            return result;
        }

        public static SteeringOutput GetSteering(KinematicState ownKS, GameObject target, float distance = 10.0f, float slowRadius = 20.0f,
            float disredSpeed = 0.1f, bool showWhishker = true, float lookAheadLength = 10f, float avoidDistance = 10f, float secondaryWhiskerAngle = 30f, float secondaryWhiskerRatio = 0.7f,
            string tag = "REPULSIVE", float repulsionTh = 8.0f, float repulsionWeight = 0.5f)
        {

            SteeringOutput linearRepulsion = LinearRepulsion.GetSteering(ownKS, tag, repulsionTh);
            SteeringOutput arrive = ArrivePlusAvoid.GetSteering(ownKS, target, distance, slowRadius, disredSpeed, showWhishker, lookAheadLength, avoidDistance, secondaryWhiskerAngle, secondaryWhiskerRatio);

            SteeringOutput result = new SteeringOutput();

            if (linearRepulsion == NULL_STEERING)
                return arrive;
            else if (arrive == NULL_STEERING)
                return linearRepulsion;

            result.linearAcceleration = arrive.linearAcceleration * (1 - repulsionWeight) + linearRepulsion.linearAcceleration * repulsionWeight;
            return result;
        }
    }
}