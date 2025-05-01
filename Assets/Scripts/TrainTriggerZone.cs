using System.Collections;
using UnityEngine;

public class TrainMovementTrigger : MonoBehaviour
{
    public GameObject train;           // The train GameObject
    public Transform target;           // The object to detect (e.g., player)
    public Vector3 destination;        // Coordinate the train moves toward
    public float speed = 5f;           // Movement speed
    public float stopDistance = 1.0f;  // Distance to stop at destination

    private bool shouldMove = false;

    public SceneSwitcher sceneSwitcher; // Reference to the SceneSwitcher script

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
                Debug.Log("Train has reached the destination.");
                shouldMove = false; // Stop moving
                StartCoroutine(DelayedSwitch());
            }
        }
    }

    IEnumerator DelayedSwitch()
    {
        yield return new WaitForSeconds(1.5f);

        sceneSwitcher.SwitchScene(5);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            shouldMove = true;
        }
    }
}
