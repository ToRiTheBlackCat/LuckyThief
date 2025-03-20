using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.StateMachines
{
    public class ThiefTakeState : ThiefState
    {
        public ThiefTakeState(ThiefScript thief, ThiefStateMachine stateMachine, string animBoolName) : base(thief, stateMachine, animBoolName)
        {
        }

        public override void Enter()
        {
            base.Enter();
            _thief.SetVelocity(0, 0);
        }

        public override void Update()
        {
            base.Update();

            if (triggerCalled)
            {
                _stateMachine.EnterState(_thief.idleState);
            }
        }

        public override void Exit()
        {
            base.Exit();
        }
    }
}
