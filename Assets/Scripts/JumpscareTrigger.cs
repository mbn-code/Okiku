using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpscareTrigger : MonoBehaviour
{
    public JumpscareManager jumpscareManager; // Reference to the JumpscareManager script
    public Transform MainChar;

    private void Update()
    {
        if(MainChar != null)
        {
            if(transform.position.z > MainChar.position.z)
            {
                jumpscareManager.TriggerJumpscare(); // Trigger the jumpscare
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other != null)
        {
            if (other.CompareTag("Player"))
            {
                jumpscareManager.TriggerJumpscare();
            }
        }
    }

}
