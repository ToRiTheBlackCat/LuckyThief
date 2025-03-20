using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.StateMachines
{
    public class ThiefMovementState : ThiefState
    {
        public ThiefMovementState(ThiefScript thief, ThiefStateMachine stateMachine, string animBoolName) : base(thief, stateMachine, animBoolName)
        {
        }

        public override void Update()
        {
            //base.Update();
            _thief.currentInteractable = _thief.InteractCheck.currentGameObj;
            if (Input.GetKeyDown(KeyCode.E))
            {
                Debug.Log($"Running state: Movement");

                //var interactable = _thief.InteractCheck.currentGameObj?.GetComponent<InteractableScript>();
                //interactable?.onHandleInteract();
            }
        }
    }
}
