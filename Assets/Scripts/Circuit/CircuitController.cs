using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CircuitController : MonoBehaviour
{
    [SerializeField] private List<TriggerAbstract> triggerList = new List<TriggerAbstract>();
    [SerializeField] private List<TriggerableAbstract> triggerableList = new List<TriggerableAbstract>();

    public HashSet<TriggerAbstract> Triggers { get; private set; }
    public HashSet<TriggerableAbstract> Triggerables { get; private set; }

    void Awake()
    {
        Triggers = new HashSet<TriggerAbstract>(triggerList);
        Triggerables = new HashSet<TriggerableAbstract>(triggerableList);
    }

    void Update()
    {
        bool allActive = Triggers.All(t => t != null && t.IsActive);

        foreach (var t in Triggerables.Where(t => t != null))
        {
            if (allActive != t.IsOn)
            {
                if (allActive) t.TurnOn();
                else           t.TurnOff();
            }
        }
    }
}
