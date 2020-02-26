using UnityEngine;
using FSM;
using Steerings;
using System.Collections;

namespace FSM
{
    [RequireComponent(typeof(FSM_ZOMBIE_PATROLLING))]
    [RequireComponent(typeof(Arrive))]
    [RequireComponent(typeof(ZOMBIE_BlackBoard))]

    public class FSM_ZOMBIE_SPOTTING_FLARE : FiniteStateMachine
    {
        public enum State { INITIAL, PATROLLING, ALARM, GOING_TO_FLARE };
        public State currentState = State.INITIAL;

        private FSM_ZOMBIE_PATROLLING fsmZombiePatrolling;
        private Arrive arrive;
        private KinematicState ks;

        private ZOMBIE_BlackBoard blackboard;
        private GameObject flare;

        private float normalSpeed;
        private float normalAcc;

        void Start()
        {
            fsmZombiePatrolling = GetComponent<FSM_ZOMBIE_PATROLLING>();
            arrive = GetComponent<Arrive>();
            blackboard = GetComponent<ZOMBIE_BlackBoard>();
            ks = GetComponent<KinematicState>();

            fsmZombiePatrolling.enabled = false;
            arrive.enabled = false;
            normalSpeed = ks.maxSpeed;
            normalAcc = ks.maxAcceleration;
        }

        public override void Exit()
        {
            fsmZombiePatrolling.Exit();
            arrive.enabled = false;
            ks.maxSpeed = normalSpeed;
            ks.maxAcceleration = normalAcc;
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
                    ChangeState(State.PATROLLING);
                    break;
                case State.PATROLLING:
                    flare = SensingUtils.FindInstanceWithinRadius(gameObject, "Flare", blackboard.flareDetectableRadius);
                    if (flare != null)
                    {
                        ChangeState(State.ALARM);
                        break;
                    }

                    if (GameController.Instance.zombieGlobalBB.flare != null)
                    {
                        flare = GameController.Instance.zombieGlobalBB.flare;
                        ChangeState(State.GOING_TO_FLARE);
                        break;
                    }
                    break;
                case State.ALARM:
                    GameController.Instance.zombieGlobalBB.AnnounceFlare(flare);
                    ChangeState(State.GOING_TO_FLARE);
                    break;
                case State.GOING_TO_FLARE:
                    if (flare == null || flare.Equals(null))
                    {
                        ChangeState(State.PATROLLING);
                    }
                    break;
            }
        }

        void ChangeState(State newState)
        {
            // exit logic
            switch (currentState)
            {
                case State.PATROLLING:
                    fsmZombiePatrolling.Exit();
                    break;
                case State.ALARM:
                    ks.maxAcceleration /= blackboard.fastVelocity;
                    ks.maxSpeed /= blackboard.fastVelocity;
                    break;
                case State.GOING_TO_FLARE:
                    arrive.enabled = false;
                    arrive.target = null;
                    break;
            }

            // enter logic
            switch (newState)
            {
                case State.PATROLLING:
                    fsmZombiePatrolling.ReEnter();
                    break;
                case State.ALARM:
                    ks.maxAcceleration *= blackboard.fastVelocity;
                    ks.maxSpeed *= blackboard.fastVelocity;
                    break;
                case State.GOING_TO_FLARE:
                    arrive.target = flare;
                    arrive.enabled = true;
                    break;
            }
            currentState = newState;
        }
    }
}
