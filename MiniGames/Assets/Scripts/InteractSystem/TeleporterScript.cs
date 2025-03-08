using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

namespace Assets.Scripts
{
    public class TeleporterScript : InteractableScript
    {
        [Header("Teleport config")]
        public Transform TargetPos;

        public override void onHandleInteract()
        {
            if (!interactable)
            {
                return;
            }

            var player = GameManagerSingleton.Player.gameObject;
            player.transform.position = TargetPos.position;
        }
    }
}
