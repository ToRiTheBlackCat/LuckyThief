using UnityEngine;
using UnityEngine.Events;

public class ThiefAnimationTriggers : MonoBehaviour
{
    public ThiefScript thief
    {
        get
        {
            return GetComponentInParent<ThiefScript>();
        }
    }

    public void ThrowTrigger()
    {
        thief.AnimationTrigger();
    }
}
