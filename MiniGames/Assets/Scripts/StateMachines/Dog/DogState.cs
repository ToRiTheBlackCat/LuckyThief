using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.StateMachines.Dog
{
    public abstract class DogState
    {
        protected readonly DogScript _dog;
        protected readonly DogStateMachine _stateMachine;
        protected readonly string _animBoolName;

        protected bool triggerCalled = false; // For changing state when the animation reach a specified point
        protected float stateTime = 0f; // For exit states that last for limited time

        public DogState(DogScript dogScript, DogStateMachine stateMachine, string animBoolName)
        {
            _dog = dogScript;
            _stateMachine = stateMachine;
            _animBoolName = animBoolName;
        }

        public virtual void Enter()
        {
            //Debug.Log($"Enter state: {_animBoolName}");
            _dog._animator.SetBool(_animBoolName, true);
            triggerCalled = false;
        }

        public abstract void Update();

        public virtual void Exit()
        {
            //Debug.Log($"Exit state: {_animBoolName}");
            _dog._animator.SetBool(_animBoolName, false);
        }

        public virtual void AnimationFinishTrigger()
        {
            triggerCalled = true;
        }
    }
}
