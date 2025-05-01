using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TekeTrigger : MonoBehaviour
{
    public TekeTekeTeleporter teke;
    public bool Enable = true;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (Enable)
            {
                teke.AllowAttack();
            } else
            {
                teke.DisallowAttack();
            }
        }
    }
}
