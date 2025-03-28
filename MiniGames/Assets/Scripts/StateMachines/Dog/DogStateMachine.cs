using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.StateMachines.Dog
{
    [Serializable]
    public class DogStateMachine
    {
        public DogState currentState { get; set; }

        public void Initialize(DogState state)
        {
            currentState = state;
        }

        public void EnterState(DogState newState)
        {
            currentState.Exit();
            currentState = newState;
            currentState.Enter();
        }
    }
}
