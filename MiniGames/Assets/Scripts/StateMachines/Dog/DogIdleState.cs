using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.StateMachines.Dog
{
    public class DogIdleState : DogInspectState
    {
        public DogIdleState(DogScript dogScript, DogStateMachine stateMachine, string animBoolName) : base(dogScript, stateMachine, animBoolName)
        {
        }

        public override void Enter()
        {
            _dog._animator.SetBool(_animBoolName, false);
            _dog.SetMoveDir(0, 0);
        }

        public override void Exit()
        {
        }
    }
}
