using UnityEngine;

namespace FSM
{
    public class FiniteStateMachine : MonoBehaviour
    {
        public virtual void Exit()
        {
            this.enabled = false;
        }

        public virtual void ReEnter()
        {
            this.enabled = true;
        }
    }
}