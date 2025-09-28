using System.Collections.Generic;
using UnityEngine;

public class CircuitController : MonoBehaviour
{
    public List<TriggerAbstract> Triggers = new List<TriggerAbstract>();

    public List<TriggerableAbstract> Triggerables = new List<TriggerableAbstract>();

    void Update()
    {
        bool allActive = true;
        foreach (var trigger in Triggers)
        {
            if (trigger == null || !trigger.IsActive)
            {
                allActive = false;
                break;
            }
        }

        foreach (var triggerable in Triggerables)
        {
            if (triggerable == null) continue;

            if (allActive && !triggerable.IsOn)
                triggerable.TurnOn();
            else if (!allActive && triggerable.IsOn)
                triggerable.TurnOff();
        }
    }
}
