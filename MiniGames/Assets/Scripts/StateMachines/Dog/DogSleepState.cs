using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.StateMachines.Dog
{
    public class DogSleepState : DogState
    {
        public DogSleepState(DogScript dogScript, DogStateMachine stateMachine, string animBoolName) : base(dogScript, stateMachine, animBoolName)
        {
        }

        public override void Enter()
        {
            _dog._animator.SetTrigger("sleep");
            triggerCalled = false;
            _dog.SetMoveDir(0, 0);
        }

        public override void Update()
        {
            if (_dog.TargetPosition != null)
            {
                _dog._animator.SetTrigger("wakeUp");
                if (triggerCalled)
                {
                    _stateMachine.EnterState(_dog.IdleState);
                }
            }
        }

        public override void Exit()
        {
            //Do nothing
        }
    }
}
