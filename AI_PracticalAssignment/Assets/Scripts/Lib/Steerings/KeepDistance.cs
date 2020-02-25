using UnityEngine;

namespace Steerings
{
    public class KeepDistance : SteeringBehaviour
    {
        public RotationalPolicy rotationalPolicy = RotationalPolicy.LWYGI;
        public GameObject target;
        public float requiredDistance;

        public override SteeringOutput GetSteering()
        {
            if (this.ownKS == null) this.ownKS = GetComponent<KinematicState>();

            if (this.target == null)
                Debug.Log("Null target in Seek of " + this.gameObject);

            SteeringOutput result = KeepDistance.GetSteering(this.ownKS, this.target, requiredDistance);
            base.applyRotationalPolicy(rotationalPolicy, result, this.target);
            return result;
        }

        public static SteeringOutput GetSteering(KinematicState ownKS, GameObject target, float requiredDistance)
        {
            Vector3 directiofromTaget;

            directiofromTaget = ownKS.position - target.transform.position;
            directiofromTaget.Normalize();
            SURROGATE_TARGET.transform.position = target.transform.position + directiofromTaget * requiredDistance;

            //return Seek.GetSteering(ownKS, SURROGATE_TARGET);
            return Arrive.GetSteering(ownKS, SURROGATE_TARGET, 0, 5, 0.1f);
        }
    }

}
