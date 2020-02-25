﻿using UnityEngine;
using FSM;
using Steerings;
using System.Collections;

namespace FSM
{
    [RequireComponent(typeof(FSM_CIVILIAN_CAMPING))]
    [RequireComponent(typeof(LeaderFollowingBlended))]
    [RequireComponent(typeof(CIVILIAN_BlackBoard))]

    public class FSM_CIVILIAN_FOLLOWS_MARINE : FiniteStateMachine
    {
        public enum State { INITIAL, CAMPING, FOLLOWING };
        public State currentState = State.INITIAL;

        private FSM_CIVILIAN_CAMPING fsmCivilianCamping;
        private LeaderFollowingBlended leaderFollowing;
        //private KinematicState ks;

        private CIVILIAN_BlackBoard blackboard;
        private GameObject player;

        //private float newLocalScale = 1.2f;
        //private float normalSpeed;
        //private float normalAcc;
        //private Vector3 normalLocalScale;

        void Start()
        {
            fsmCivilianCamping = GetComponent<FSM_CIVILIAN_CAMPING>();
            leaderFollowing = GetComponent<LeaderFollowingBlended>();
            //ks = GetComponent<KinematicState>();
            blackboard = GetComponent<CIVILIAN_BlackBoard>();

            fsmCivilianCamping.enabled = false;
            leaderFollowing.enabled = false;

            //normalSpeed = ks.maxSpeed;
            //normalAcc = ks.maxAcceleration;
            //normalLocalScale = gameObject.transform.localScale;

        }

        public override void Exit()
        {
            fsmCivilianCamping.Exit();
            leaderFollowing.enabled = false;
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
                    ChangeState(State.CAMPING);
                    break;
                case State.CAMPING:
                    if (SensingUtils.DistanceToTarget(gameObject, GameController.Instance.playerController) <= blackboard.PlayerDetectionRadius &&
                        GameController.Instance.civilianGlobalBB.CivilianFollowingCounter < GameController.Instance.civilianGlobalBB.MaxCiviliansFollowing)
                    {
                        ChangeState(State.FOLLOWING);
                    }
                    break;
                case State.FOLLOWING:
                    if (SensingUtils.DistanceToTarget(gameObject, GameController.Instance.playerController) > blackboard.farAwayPlayerRadius)
                    {
                        ChangeState(State.CAMPING);
                    }
                    break;
            }
        }

        void ChangeState(State newState)
        {
            // exit logic
            switch (currentState)
            {
                case State.CAMPING:
                    fsmCivilianCamping.Exit();
                    break;
                case State.FOLLOWING:
                    //gameObject.transform.localScale /= newLocalScale;
                    //ks.maxAcceleration /= 2;
                    //ks.maxSpeed /= 2;
                    leaderFollowing.enabled = false;
                    leaderFollowing.target = null;
                    GameController.Instance.civilianGlobalBB.CivilianFollowingCounter--;
                    break;
            }

            // enter logic
            switch (newState)
            {
                case State.CAMPING:
                    fsmCivilianCamping.ReEnter();
                    break;
                case State.FOLLOWING:
                    //gameObject.transform.localScale *= newLocalScale;
                    //ks.maxAcceleration *= 2;
                    //ks.maxSpeed *= 2;
                    GameController.Instance.civilianGlobalBB.CivilianFollowingCounter++;
                    leaderFollowing.target = GameController.Instance.playerController;
                    leaderFollowing.enabled = true;
                    break;
            }
            currentState = newState;
        }
    }
}
