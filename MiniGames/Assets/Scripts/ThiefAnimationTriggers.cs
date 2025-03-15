using UnityEngine;
using UnityEngine.Events;

public class ThiefAnimationTriggers : MonoBehaviour
{
    private ThiefScript _thief;

    private void Awake()
    {
        _thief = GetComponentInParent<ThiefScript>();
    }

    public void ThrowTrigger()
    {
        _thief.AnimationTrigger();
    }
}
