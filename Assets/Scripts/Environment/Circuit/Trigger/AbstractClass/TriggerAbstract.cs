using UnityEngine;

public abstract class TriggerAbstract : MonoBehaviour
{
    public bool IsActive { get; protected set; }

    public abstract void Activate();
    public abstract void Deactivate();
}
