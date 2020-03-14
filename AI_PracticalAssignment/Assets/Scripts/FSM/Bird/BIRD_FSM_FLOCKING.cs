using System;
using System.Collections;
using System.Collections.Generic;
using Steerings;
using UnityEngine;

namespace FSM
{
    [RequireComponent(typeof(BIRD_Blackboard))]
    [RequireComponent(typeof(FlockingAround))]
    public class BIRD_FSM_FLOCKING : FiniteStateMachine
    {
        public enum State { INITIAL, WANDERING, BLOCKING };
        public State currentState = State.INITIAL;

        private FlockingAround flockingAround;
        private BIRD_Blackboard blackboard;

        private float currentTimer;

        void Start()
        {
            flockingAround = GetComponent<FlockingAround>();
            blackboard = GetComponent<BIRD_Blackboard>();

            flockingAround.enabled = false;
            ChangeState(State.INITIAL);
        }

        public override void Exit()
        {
            base.Exit();
            flockingAround.enabled = false;
        }

        public override void ReEnter()
        {
            base.ReEnter();
            ChangeState(State.INITIAL);
        }

        void Update()
        {
            switch (currentState)
            {
                case State.INITIAL:
                    ChangeState(State.WANDERING);
                    break;
                case State.WANDERING:
                    if (currentTimer >= blackboard.maxWanderTime)
                    {
                        ChangeState(State.BLOCKING);
                        break;
                    }
                    currentTimer += Time.deltaTime;
                    break;
                case State.BLOCKING:
                    if (currentTimer >= blackboard.maxBlockingTime)
                    {
                        ChangeState(State.WANDERING);
                        break;
                    }
                    currentTimer += Time.deltaTime;
                    break;
            }
        }

        private void ChangeState(State newState)
        {
            // exit logic
            switch (currentState)
            {
                case State.INITIAL:

                    break;
                case State.WANDERING:
                    flockingAround.enabled = false;
                    currentTimer = 0f;
                    break;
                case State.BLOCKING:
                    flockingAround.enabled = false;
                    currentTimer = 0f;
                    break;
            }

            // enter logic
            switch (newState)
            {
                case State.INITIAL:

                    break;
                case State.WANDERING:
                    flockingAround.enabled = true;
                    flockingAround.wanderRate = blackboard.wanderingWanderRate;
                    flockingAround.attractor = blackboard.defaultAttractor;
                    flockingAround.seekWeight = blackboard.wanderingSeekWeight;
                    break;
                case State.BLOCKING:
                    flockingAround.enabled = true;
                    flockingAround.wanderRate = blackboard.blockingWanderRate;
                    flockingAround.attractor = blackboard.blockingAttractor;
                    flockingAround.seekWeight = blackboard.blockingSeekWeight;
                    break;
            }

            currentState = newState;
        }
    }
}