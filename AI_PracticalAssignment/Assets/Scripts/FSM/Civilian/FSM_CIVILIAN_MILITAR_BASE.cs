using System;
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
        public enum State { INITIAL, FOLLOWING_MARINE, GOING_BASE, CIVILIAN_SAVED };

        public State currentState = State.INITIAL;

        private FSM_CIVILIAN_FOLLOWS_MARINE fsmCivilianFollowsMarine;
        private Arrive arrive;

        private CIVILIAN_BlackBoard blackboard;
        private GameObject bird;

        void Start()
        {
            fsmCivilianFollowsMarine = GetComponent<FSM_CIVILIAN_FOLLOWS_MARINE>();
            arrive = GetComponent<Arrive>();
            blackboard = GetComponent<CIVILIAN_BlackBoard>();

            fsmCivilianFollowsMarine.enabled = false;
            arrive.enabled = false;
        }

        public override void Exit()
        {
            fsmCivilianFollowsMarine.Exit();
            arrive.enabled = false;
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
                        bird = SensingUtils.FindInstanceWithinRadius(gameObject, "Bird", blackboard.birdDetectionRadius);
                        if (bird != null)
                        {
                            if (SensingUtils.DistanceToTarget(gameObject, bird) > blackboard.birdScareRadius)
                            {
                                ChangeState(State.GOING_BASE);
                                break;
                            }
                            break;
                        }
                        else
                        {
                            ChangeState(State.GOING_BASE);
                            break;
                        }
                    }
                    break;
                case State.GOING_BASE:
                    if (SensingUtils.DistanceToTarget(gameObject, blackboard.militarBase) < blackboard.nearbyMilitarBaseRadius)
                    {
                        ChangeState(State.CIVILIAN_SAVED);
                        break;
                    }
                    break;
                case State.CIVILIAN_SAVED:
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
                    break;
                case State.CIVILIAN_SAVED:
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
                    break;
                case State.CIVILIAN_SAVED:
                    var particle = Instantiate(GameController.Instance.civilianGlobalBB.civilianSaved, transform.position, transform.rotation);
                    particle.Play();
                    Destroy(gameObject);
                    break;
            }

            currentState = newState;
        }
    }
}