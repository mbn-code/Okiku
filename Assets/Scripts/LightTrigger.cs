using UnityEngine;
using UnityEngine.Events;

public class LightTrigger : MonoBehaviour
{
    [Header("Target Settings")]
    [Tooltip("The target to detect (e.g., the player).")]
    public Transform target;

    [Tooltip("Distance to trigger.")]
    public float triggerDistance = 5f;

    [Header("Lights to Control (Optional)")]
    [Tooltip("Lights that will be triggered.")]
    public Light[] lights;

    [Tooltip("Turn lights ON when in range (true) or OFF (false).")]
    public bool turnLightsOn = true;

    [Header("Script Trigger (Optional)")]
    [Tooltip("Extra event to trigger when the target enters range.")]
    public UnityEvent onTrigger;

    [Header("Trigger Behavior")]
    [Tooltip("Only trigger once? If false, it will toggle based on distance.")]
    public bool triggerOnce = false;

    private bool hasTriggered = false;

    void Update()
    {
        if (target == null) return;

        float distance = Vector3.Distance(transform.position, target.position);

        if (distance <= triggerDistance)
        {
            if (!hasTriggered)
            {
                SetLights(turnLightsOn);
                onTrigger?.Invoke(); // <- This calls any linked function

                if (triggerOnce)
                    hasTriggered = true;
            }
        }
        else
        {
            if (!triggerOnce && hasTriggered)
            {
                SetLights(!turnLightsOn);
                hasTriggered = false;
            }
        }
    }

    void SetLights(bool state)
    {
        if (lights == null || lights.Length == 0) return;

        foreach (var light in lights)
        {
            if (light != null)
                light.enabled = state;
        }
    }
}
