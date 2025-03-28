using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.StateMachines.Dog
{
    public class DogGrabState : DogState
    {
        public DogGrabState(DogScript dogScript, DogStateMachine stateMachine, string animBoolName) : base(dogScript, stateMachine, animBoolName)
        {
        }

        public override void Enter()
        {
            base.Enter();

            _dog.SetMoveDir(0, 0);
        }

        public override void Update()
        {
            
        }

        public override void Exit()
        {
            base.Exit();
        }
    }
}
