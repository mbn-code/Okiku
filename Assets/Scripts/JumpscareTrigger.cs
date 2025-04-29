using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpscareTrigger : MonoBehaviour
{
    public JumpscareManager jumpscareManager; // Reference to the JumpscareManager script

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
