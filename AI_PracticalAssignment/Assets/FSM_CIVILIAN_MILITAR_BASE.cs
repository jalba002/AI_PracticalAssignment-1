using UnityEngine;
using FSM;
using Steerings;
using System.Collections;

namespace FSM
{
    [RequireComponent(typeof(FSM_CIVILIAN_FOLLOWS_MARINE))]
    [RequireComponent(typeof(Arrive))]
    [RequireComponent(typeof(CIVILIAN_BlackBoard))]

    public class FSM_CIVILIAN_MILITAR_BASE : FiniteStateMachine
    {
        public enum State { INITIAL, FOLLOWING_MARINE, GOING_BASE };
        public State currentState = State.INITIAL;

        private FSM_CIVILIAN_FOLLOWS_MARINE fsmCivilianFollowsMarine;
        private Arrive arrive;
        //private KinematicState ks;

        private CIVILIAN_BlackBoard blackboard;
        private GameObject player;

        //private float newLocalScale = 1.2f;
        //private float normalSpeed;
        //private float normalAcc;
        //private Vector3 normalLocalScale;

        void Start()
        {
            fsmCivilianFollowsMarine = GetComponent<FSM_CIVILIAN_FOLLOWS_MARINE>();
            arrive = GetComponent<Arrive>();
            //ks = GetComponent<KinematicState>();
            blackboard = GetComponent<CIVILIAN_BlackBoard>();

            fsmCivilianFollowsMarine.enabled = false;
            arrive.enabled = false;

            //normalSpeed = ks.maxSpeed;
            //normalAcc = ks.maxAcceleration;
            //normalLocalScale = gameObject.transform.localScale;

        }

        public override void Exit()
        {
            fsmCivilianFollowsMarine.Exit();
            arrive.enabled = false;
            //ks.maxSpeed = normalSpeed;
            //ks.maxAcceleration = normalAcc;
            //gameObject.transform.localScale = normalLocalScale;
            base.Exit();
        }

        public override void ReEnter()
        {
            currentState = State.INITIAL;
            base.ReEnter();
        }

        void Update()
        {
            switch (currentState)
            {
                case State.INITIAL:
                    ChangeState(State.FOLLOWING_MARINE);
                    break;
                case State.FOLLOWING_MARINE:
                    if (SensingUtils.DistanceToTarget(gameObject, blackboard.militarBase) < blackboard.MilitarBaseDetectableRadius)
                    {
                        ChangeState(State.GOING_BASE);
                        break;
                    }
                    break;
                case State.GOING_BASE:
                    if (SensingUtils.DistanceToTarget(gameObject, blackboard.militarBase) < blackboard.nearbyMilitarBaseRadius)
                    {
                        if (gameObject.tag == blackboard.transportingTag)
                            GameController.Instance.civilianGlobalBB.CivilianFollowingCounter--;

                        Destroy(gameObject);
                        break;
                    }
                    break;
            }
        }

        void ChangeState(State newState)
        {
            // exit logic
            switch (currentState)
            {
                case State.FOLLOWING_MARINE:
                    fsmCivilianFollowsMarine.Exit();
                    break;
                case State.GOING_BASE:
                    arrive.enabled = false;
                    arrive.target = null;
                    //gameObject.transform.localScale /= newLocalScale;
                    //ks.maxAcceleration /= 2;
                    //ks.maxSpeed /= 2;
                    break;
            }

            // enter logic
            switch (newState)
            {
                case State.FOLLOWING_MARINE:
                    fsmCivilianFollowsMarine.ReEnter();
                    break;
                case State.GOING_BASE:
                    arrive.target = blackboard.militarBase;
                    arrive.enabled = true;
                    //gameObject.transform.localScale *= newLocalScale;
                    //ks.maxAcceleration *= 2;
                    //ks.maxSpeed *= 2;
                    break;
            }
            currentState = newState;
        }
    }
}