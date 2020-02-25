using UnityEngine;

namespace Steerings
{
    public class KeepPosition : SteeringBehaviour
    {
        public RotationalPolicy rotationalPolicy = RotationalPolicy.LWYGI;
        public GameObject target;
        public float requiredDistance;
        public float requiredAngle;

        public override SteeringOutput GetSteering()
        {
            if (this.ownKS == null) this.ownKS = GetComponent<KinematicState>();

            if (this.target == null)
                Debug.Log("Null target in Seek of " + this.gameObject);

            SteeringOutput result = KeepPosition.GetSteering(base.ownKS, this.target, this.requiredDistance, this.requiredAngle);
            base.applyRotationalPolicy(rotationalPolicy, result, target);
            return result;
        }

        public static SteeringOutput GetSteering(KinematicState me, GameObject target, float distance, float angle)
        {
            float desiredAngle = target.transform.eulerAngles.z + angle;
            Vector3 desiredDirectionFromTarget = Utils.OrientationToVector(desiredAngle).normalized;

            desiredDirectionFromTarget *= distance;
            SURROGATE_TARGET.transform.position = desiredDirectionFromTarget + target.transform.position;

            return Arrive.GetSteering(me, SURROGATE_TARGET, 0, 5, 0.1f);
        }
    }

}
