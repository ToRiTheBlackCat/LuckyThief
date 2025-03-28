using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Assets.Scripts.StateMachines.Dog
{
    public class DogPursuitState : DogState
    {
        private float grabCooldown;
        public DogPursuitState(DogScript dogScript, DogStateMachine stateMachine, string animBoolName) : base(dogScript, stateMachine, animBoolName)
        {
        }

        public override void Enter()
        {
            triggerCalled = false;
            _dog._animator.SetBool("isMoving", true);
            _dog._animator.SetBool("isPursuit", true);

            grabCooldown = 5f;
        }

        public override void Update()
        {
            grabCooldown -= Time.deltaTime;

            if (_dog.TargetThief != null)
            {
                var direction = _dog.TargetThief.transform.position - _dog.transform.position;
                _dog.SetMoveDir(direction.x, direction.y);

                if (grabCooldown <= 0)
                {
                    var raycast = Physics2D.CircleCastAll(_dog.AttackCheck.position, 1f, Vector2.zero);

                    ThiefScript thiefScript = null;
                    var foundObj = raycast
                        .FirstOrDefault(x =>
                            x.collider.gameObject.TryGetComponent<ThiefScript>(out thiefScript)
                        );

                    if (thiefScript != null)
                    {
                        thiefScript.grabbedState.Grabber = _dog;
                        thiefScript.stateMachine.EnterState(thiefScript.grabbedState);
                        _stateMachine.EnterState(_dog.GrabState);
                    }

                }
            }


            if (Vector2.Distance(_dog.transform.position, _dog.TargetThief.transform.position) > _dog.detectRange)
            {
                _dog.TargetThief = null;
                _stateMachine.EnterState(_dog.WalkState);
            }
        }

        public override void Exit()
        {
            _dog._animator.SetBool("isMoving", false);
            _dog._animator.SetBool("isPursuit", false);
        }
    }
}
