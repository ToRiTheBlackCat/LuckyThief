using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.StateMachines
{
    public class ThiefIdleState : ThiefMovementState
    {
        public ThiefIdleState(ThiefScript thief, ThiefStateMachine stateMachine, string animBoolName) : base(thief, stateMachine, animBoolName)
        {
        }

        public override void Enter()
        {
            //base.Enter();
            triggerCalled = false;
        }

        public override void Update()
        {
            base.Update();
            if (_thief.xAxis != 0 || _thief.yAxis != 0)
            {
                // Change state to Walking
                _stateMachine.EnterState(_thief.walkState);
            }
        }

        public override void Exit()
        {
            //base.Exit();
        }
    }
}
