using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts
{
    #nullable enable
    public abstract class MiniGameBase: MonoBehaviour
    {
        public abstract void StartGame(InteractableScript? attachedInteractable);
        public abstract void ExitGame(float delay = 0);
    }
}
