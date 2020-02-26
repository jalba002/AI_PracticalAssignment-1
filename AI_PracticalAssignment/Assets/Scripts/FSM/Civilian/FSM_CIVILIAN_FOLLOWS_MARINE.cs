using UnityEngine;
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
        private KinematicState ks;

        private CIVILIAN_BlackBoard blackboard;
        private GameObject player;

        private float normalSpeed;
        private float normalAcc;

        void Start()
        {
            fsmCivilianCamping = GetComponent<FSM_CIVILIAN_CAMPING>();
            leaderFollowing = GetComponent<LeaderFollowingBlended>();
            blackboard = GetComponent<CIVILIAN_BlackBoard>();
            ks = GetComponent<KinematicState>();

            fsmCivilianCamping.enabled = false;
            leaderFollowing.enabled = false;
            normalSpeed = ks.maxSpeed;
            normalAcc = ks.maxAcceleration;
        }

        public override void Exit()
        {
            fsmCivilianCamping.Exit();
            leaderFollowing.enabled = false;
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
                    ChangeState(State.CAMPING);
                    break;
                case State.CAMPING:
                    if (SensingUtils.DistanceToTarget(gameObject, GameController.Instance.playerController) <= blackboard.PlayerDetectionRadius &&
                        GameController.Instance.civilianGlobalBB.CivilianFollowingCounter < GameController.Instance.civilianGlobalBB.MaxCiviliansFollowing)
                    {
                        ChangeState(State.FOLLOWING);
                        break;
                    }
                    break;
                case State.FOLLOWING:
                    if (SensingUtils.DistanceToTarget(gameObject, GameController.Instance.playerController) > blackboard.farAwayPlayerRadius)
                    {
                        ChangeState(State.CAMPING);
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
                case State.CAMPING:
                    fsmCivilianCamping.Exit();
                    break;
                case State.FOLLOWING:
                    ks.maxAcceleration /= blackboard.fastVelocity;
                    ks.maxSpeed /= blackboard.fastVelocity;
                    blackboard.followingPlayer = false;
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
                    ks.maxAcceleration *= blackboard.fastVelocity;
                    ks.maxSpeed *= blackboard.fastVelocity;
                    blackboard.followingPlayer = true;
                    GameController.Instance.civilianGlobalBB.CivilianFollowingCounter++;
                    leaderFollowing.target = GameController.Instance.playerController;
                    leaderFollowing.enabled = true;
                    break;
            }
            currentState = newState;
        }
    }
}
