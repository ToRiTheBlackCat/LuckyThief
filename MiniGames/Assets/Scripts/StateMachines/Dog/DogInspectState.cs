using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.StateMachines.Dog
{
    public class DogInspectState : DogState
    {
        public DogInspectState(DogScript dogScript, DogStateMachine stateMachine, string animBoolName) : base(dogScript, stateMachine, animBoolName)
        {
        }

        public override void Update()
        {
            if (_dog.TargetThief != null)
            {
                _stateMachine.EnterState(_dog.PursuitState);
                return;
            }
        }
    }
}
