using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

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

            var raycast = Physics2D.CircleCastAll(_dog.AttackCheck.position, _dog.detectRange, Vector2.zero);
            raycast.FirstOrDefault(x =>
                    x.collider.gameObject.TryGetComponent<ThiefScript>(out _dog.TargetThief)
                );

            //var dirToPlayer
            //var wallCast = Physics2D.Raycast(_dog.transform.position, )
        }
    }
}

