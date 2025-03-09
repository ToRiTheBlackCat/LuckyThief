using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player; // Reference to the player's transform
    public Vector3 offset; // Offset distance between the player and camera

    void Start()
    {
        // Initialize the offset based on the initial positions
        offset = transform.position - player.position;
    }

    void LateUpdate()
    {
        // Update the camera's position to follow the player
        transform.position = player.position + offset;
    }
}