using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.StateMachines.Dog
{
    [System.Serializable]
    public class DogStateMachine
    {
        public DogState CurrentState;

        public void Initialize(DogState state)
        {
            CurrentState = state;
        }

        public void EnterState(DogState newState)
        {
            CurrentState.Exit();
            CurrentState = newState;
            CurrentState.Enter();
        }
    }
}
