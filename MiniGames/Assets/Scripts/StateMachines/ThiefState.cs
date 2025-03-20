using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace Assets.Scripts.StateMachines
{
    public class ThiefState
    {
        protected readonly ThiefScript _thief;
        protected readonly ThiefStateMachine _stateMachine;
        protected readonly string _animBoolName;
        
        protected bool triggerCalled = false; // For changing state when the animation reach a specified point

        public ThiefState(ThiefScript thief, ThiefStateMachine stateMachine, string animBoolName)
        {
            _thief = thief;
            _stateMachine = stateMachine;
            _animBoolName = animBoolName;
        }

        public virtual void Enter()
        {
            //Debug.Log($"Enter state: {_animBoolName}");
            _thief._animator.SetBool(_animBoolName, true);
            triggerCalled = false;
        }

        public virtual void Update()
        {
            //Debug.Log($"Running state: {_animBoolName}");
        }

        public virtual void Exit()
        {
            //Debug.Log($"Exit state: {_animBoolName}");
            _thief._animator.SetBool(_animBoolName, false);
        }

        public virtual void AnimationFinishTrigger()
        {
            triggerCalled = true;
        }
    }
}
