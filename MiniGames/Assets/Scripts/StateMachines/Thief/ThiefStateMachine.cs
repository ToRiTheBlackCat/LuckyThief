using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;


namespace Assets.Scripts.StateMachines
{
    public class ThiefStateMachine
    {
        public ThiefState currentState;

        public void Initialize(ThiefState state)
        {
            currentState = state;
        }

        public void EnterState(ThiefState newState)
        {
            currentState.Exit();
            currentState = newState;
            currentState.Enter();
        }
    }
}
