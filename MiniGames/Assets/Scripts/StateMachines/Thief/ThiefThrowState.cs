﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.StateMachines
{
    public class ThiefThrowState : ThiefState
    {
        public ThiefThrowState(ThiefScript thief, ThiefStateMachine stateMachine, string animBoolName) : base(thief, stateMachine, animBoolName)
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
