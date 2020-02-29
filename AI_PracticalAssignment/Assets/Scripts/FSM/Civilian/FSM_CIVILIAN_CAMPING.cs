using UnityEngine;
using FSM;
using Steerings;
using System.Collections;

namespace FSM
{
    [RequireComponent(typeof(FleePlusAvoid))]
    [RequireComponent(typeof(WanderAroundPlusAvoid))]
    [RequireComponent(typeof(CIVILIAN_BlackBoard))]

    public class FSM_CIVILIAN_CAMPING : FiniteStateMachine
    {
        public enum State { INITIAL, WANDERING, FLEEING };
        public State currentState = State.INITIAL;

        private FleePlusAvoid flee;
        private WanderAroundPlusAvoid wander;
        private CIVILIAN_BlackBoard blackboard;

        private GameObject zombie;
        private GameObject otherZombie;

        void Start()
        {
            wander = GetComponent<WanderAroundPlusAvoid>();
            flee = GetComponent<FleePlusAvoid>();
            blackboard = GetComponent<CIVILIAN_BlackBoard>();

            wander.enabled = false;
            flee.enabled = false;
            wander.attractor = blackboard.campFire;
        }

        public override void Exit()
        {
            wander.enabled = false;
            flee.enabled = false;
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
                    zombie = SensingUtils.FindInstanceWithinRadius(gameObject, blackboard.zombieTag, blackboard.nearbyZombieRadius);
                    if (zombie != null)
                        ChangeState(State.FLEEING);
                    break;

                case State.FLEEING:
                    if (zombie != null)
                    {
                        otherZombie = SensingUtils.FindInstanceWithinRadius(gameObject, blackboard.zombieTag, blackboard.nearbyZombieRadius);
                        if (FindZombie(zombie, otherZombie))
                        {
                            zombie = null;
                            break;
                        }
                    }
                    else
                    {
                        zombie = SensingUtils.FindInstanceWithinRadius(gameObject, blackboard.zombieTag, blackboard.nearbyZombieRadius);
                        if (FindZombie(otherZombie, zombie))
                        {
                            otherZombie = null;
                            break;
                        }
                    }

                    if (SensingUtils.DistanceToTarget(gameObject, flee.target) >= blackboard.farAwayZombieRadius)
                    {
                        ChangeState(State.WANDERING);
                        break;
                    }
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
                case State.FLEEING:
                    flee.enabled = false;
                    flee.target = null;
                    break;
            }

            // enter logic
            switch (newState)
            {
                case State.WANDERING:
                    wander.enabled = true;
                    break;

                case State.FLEEING:
                    SetFleeTarget(zombie);
                    flee.enabled = true;
                    break;
            }

            currentState = newState;
        }

        bool FindZombie(GameObject currentZombie, GameObject newZombie)
        {
            if (newZombie != null && newZombie != currentZombie && SensingUtils.DistanceToTarget(gameObject, newZombie) < SensingUtils.DistanceToTarget(gameObject, currentZombie))
            {
                SetFleeTarget(newZombie);
                return true;
            }
            return false;
        }

        void SetFleeTarget(GameObject target)
        {
            flee.target = target;
        }

        private void OnDestroy()
        {
            if (GameController.Instance != null)
            {
                if (blackboard.followingPlayer)
                    GameController.Instance.civilianGlobalBB.CivilianFollowingCounter--;

                if (GameController.Instance.CanvasController != null)
                    GameController.Instance.CanvasController.CivilianDeadUpdate();
            }
        }
    }
}