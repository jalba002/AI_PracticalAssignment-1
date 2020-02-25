using UnityEngine;
using FSM;
using Steerings;
using System.Collections;

namespace FSM
{
    [RequireComponent(typeof(WanderAroundPlusAvoid))]
    [RequireComponent(typeof(ZOMBIE_BlackBoard))]

    public class FSM_ZOMBIE_PATROLLING : FiniteStateMachine
    {
        public enum State { INITIAL, WANDERING };

        public State currentState = State.INITIAL;

        private WanderAroundPlusAvoid wander;
        private ZOMBIE_BlackBoard blackboard;

        private GameObject civilian;

        void Start()
        {
            wander = GetComponent<WanderAroundPlusAvoid>();
            blackboard = GetComponent<ZOMBIE_BlackBoard>();

            wander.enabled = false;
            wander.attractor = blackboard.hazardZone;

            // the global blackboard is better created by an external agent
            // or (if it has to be created here) must come from a class implementing the SINGLETON PATTERN

        }

        public override void Exit()
        {
            wander.enabled = false;
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
                    break;
            }
        }

        private void ChangeState(State newState)
        {
            // EXIT STATE LOGIC. Depends on current state
            switch (currentState)
            {
                case State.WANDERING:
                    wander.enabled = false;
                    break;
            }

            // ENTER STATE LOGIC. Depends on newState
            switch (newState)
            {
                case State.WANDERING:
                    wander.enabled = true;
                    break;
            }

            currentState = newState;
        }
    }
}