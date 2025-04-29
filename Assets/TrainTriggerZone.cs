using UnityEngine;

public class TrainMovementTrigger : MonoBehaviour
{
    public GameObject train;           // The train GameObject
    public Transform target;           // The object to detect (e.g., player)
    public Vector3 destination;        // Coordinate the train moves toward
    public float speed = 5f;           // Movement speed
    public float stopDistance = 0.1f;  // Distance to stop at destination

    private bool shouldMove = false;

    void Update()
    {
        if (shouldMove && train != null)
        {
            Vector3 direction = (destination - train.transform.position).normalized;
            float distance = Vector3.Distance(train.transform.position, destination);

            if (distance > stopDistance)
            {
                train.transform.position += direction * speed * Time.deltaTime;
            }
            else
            {
                shouldMove = false; // Stop moving
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.transform == target)
        {
            shouldMove = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.transform == target)
        {
            // Optional: stop train if player exits early
            // shouldMove = false;
        }
    }
}
