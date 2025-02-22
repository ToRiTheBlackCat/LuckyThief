using UnityEngine;
using UnityEngine.Events;

public class InteractCheckScript : MonoBehaviour
{
    public UnityEvent<Collision2D> OnInteractEnter;

    public void OnCollisionEnter2D(Collision2D collision)
    {
        OnInteractEnter.Invoke(collision);
    }
}
