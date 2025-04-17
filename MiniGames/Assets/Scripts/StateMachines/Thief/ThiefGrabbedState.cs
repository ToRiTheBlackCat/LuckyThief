using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.StateMachines.Thief
{
    public class ThiefGrabbedState : ThiefState
    {
        private const int GRAB_HEALTH = 15;
        private int currentGrabHealth;
        public int GrabHealth => currentGrabHealth;

        public DogScript Grabber;

        public ThiefGrabbedState(ThiefScript thief, ThiefStateMachine stateMachine, string animBoolName) : base(thief, stateMachine, animBoolName)
        {
        }

        public override void Enter()
        {
            base.Enter();
            currentGrabHealth = GRAB_HEALTH;
            _thief.SetVelocity(0f, 0f);
        }

        public override void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                currentGrabHealth--;
            }

            if (currentGrabHealth <= 0)
            {
                _stateMachine.EnterState(_thief.idleState);
                // Tell grabber the player has exit grab state
                Grabber.ExitGrabState();
                Grabber = null;
            }

            base.Update();
        }

        public override void Exit()
        {
            base.Exit();
        }
    }
}
