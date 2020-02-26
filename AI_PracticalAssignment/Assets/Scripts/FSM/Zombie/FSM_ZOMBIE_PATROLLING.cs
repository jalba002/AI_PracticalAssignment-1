using UnityEngine;
using FSM;
using Steerings;
using System.Collections;

namespace FSM
{
    [RequireComponent(typeof(Pursue))]
    [RequireComponent(typeof(WanderAroundPlusAvoid))]
    [RequireComponent(typeof(ZOMBIE_BlackBoard))]

    public class FSM_ZOMBIE_PATROLLING : FiniteStateMachine
    {
        public enum State { INITIAL, WANDERING, PURSUING, KILL_CIVILIAN };

        public State currentState = State.INITIAL;

        private WanderAroundPlusAvoid wander;
        private Pursue pursue;
        private ZOMBIE_BlackBoard blackboard;
        private KinematicState ks;

        private GameObject civilian;

        private float normalSpeed;
        private float normalAcc;

        void Start()
        {
            wander = GetComponent<WanderAroundPlusAvoid>();
            pursue = GetComponent<Pursue>();
            blackboard = GetComponent<ZOMBIE_BlackBoard>();
            ks = GetComponent<KinematicState>();

            wander.enabled = false;
            pursue.enabled = false;
            wander.attractor = blackboard.hazardZone;
            normalSpeed = ks.maxSpeed;
            normalAcc = ks.maxAcceleration;
        }

        public override void Exit()
        {
            wander.enabled = false;
            pursue.enabled = false;
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
                    ChangeState(State.WANDERING);
                    break;
                case State.WANDERING:
                    civilian = SensingUtils.FindInstanceWithinRadius(gameObject, blackboard.civilianTag, blackboard.civilianDetectableRadius);
                    if (civilian != null)
                    {
                        ChangeState(State.PURSUING);
                        break;
                    }
                    break;
                case State.PURSUING:
                    if (civilian == null || civilian.Equals(null))
                    {
                        ChangeState(State.WANDERING);
                        break;
                    }

                    if (SensingUtils.DistanceToTarget(gameObject, civilian) < blackboard.nearbyCivilianRadius)
                    {
                        if (civilian.GetComponent<CIVILIAN_BlackBoard>().followingPlayer)
                            GameController.Instance.civilianGlobalBB.CivilianFollowingCounter--;
                        ChangeState(State.KILL_CIVILIAN);
                        break;
                    }
                    break;
                case State.KILL_CIVILIAN:
                    ChangeState(State.WANDERING);
                    break;
            }
        }

        private void ChangeState(State newState)
        {
            // exit logic
            switch (currentState)
            {
                case State.WANDERING:
                    wander.enabled = false;
                    break;
                case State.PURSUING:
                    ks.maxAcceleration /= blackboard.fastVelocity;
                    ks.maxSpeed /= blackboard.fastVelocity;
                    pursue.target = null;
                    pursue.enabled = false;
                    break;
                case State.KILL_CIVILIAN:
                    Destroy(civilian);
                    break;
            }

            // enter logic
            switch (newState)
            {
                case State.WANDERING:
                    wander.enabled = true;
                    break;
                case State.PURSUING:
                    ks.maxAcceleration *= blackboard.fastVelocity;
                    ks.maxSpeed *= blackboard.fastVelocity;
                    pursue.target = civilian;
                    pursue.enabled = true;
                    break;
                case State.KILL_CIVILIAN:
                    var particle = Instantiate(GameController.Instance.civilianGlobalBB.civilianDead, transform.position, transform.rotation);
                    particle.Play();
                    break;
            }

            currentState = newState;
        }
    }
}