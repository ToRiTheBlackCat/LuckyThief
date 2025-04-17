using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.StateMachines.Dog
{
    public class DogWalkState : DogInspectState
    {
        public DogWalkState(DogScript dogScript, DogStateMachine stateMachine, string animBoolName) : base(dogScript, stateMachine, animBoolName)
        {
        }

        public override void Enter()
        {
            base.Enter();
            stateTime = 8f;
        }

        public override void Update()
        {
            stateTime -= Time.deltaTime;
            if (_dog.TargetPosition != null)
            {
                Vector3 direction = (Vector3)(_dog.TargetPosition - _dog.transform.position);
                _dog.SetMoveDir(direction.x, direction.y);
            }

            if (stateTime <= 0f)
            {
                _dog.TargetPosition = null;
                var direction = _dog.HomePosition - _dog.transform.position;
                _dog.SetMoveDir(direction.x, direction.y);

                if (direction.magnitude < .3f)
                {
                    _stateMachine.EnterState(_dog.SleepState);
                    _dog.transform.position = _dog.HomePosition;
                }
            }

            base.Update();
        }
    }
}
