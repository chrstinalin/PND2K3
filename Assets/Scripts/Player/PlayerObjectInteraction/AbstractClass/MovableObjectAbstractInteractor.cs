using UnityEngine;

public abstract class MovableObjectAbstractInteractor : MonoBehaviour
{
    protected MovableObject grabbedObject;

    public virtual void Grab(MovableObject obj)
    {
        if (obj == null) return;
        grabbedObject = obj;
    }

    public abstract void Release();

    protected bool IsGrabbing()
    {
        return grabbedObject != null;
    }
}
