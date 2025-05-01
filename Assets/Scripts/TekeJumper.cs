using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TekeJumper : MonoBehaviour
{
    public JumpscareManager jumpscareManager;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (other.transform.position.y < 0.3f)
            {
                jumpscareManager.TriggerJumpscare(); // Trigger the jumpscare
            }
        }
    }
}
