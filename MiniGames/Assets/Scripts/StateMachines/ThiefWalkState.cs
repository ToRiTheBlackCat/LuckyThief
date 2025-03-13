using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.StateMachines
{
    public class ThiefWalkState : ThiefMovementState
    {
        public ThiefWalkState(ThiefScript thief, ThiefStateMachine stateMachine, string animBoolName) : base(thief, stateMachine, animBoolName)
        {
        }

        public override void Enter()
        {
            base.Enter();
        }

        public override void Update()
        {
            base.Update();

            if (_thief.xAxis == 0 && _thief.yAxis == 0)
            {
                // Change state to Idle
                _stateMachine.EnterState(_thief.idleState);
            }
            
            _thief.SetSprite(_thief.xAxis, _thief.yAxis);
            _thief.SetVelocity(_thief.xAxis, _thief.yAxis);
            
        }

        public override void Exit()
        {
            base.Exit();
        }
    }
}
