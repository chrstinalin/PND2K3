using UnityEngine;

public class PushableObjectInteractor : MovableObjectAbstractInteractor
{
    public float pushForwardOffset = 1.7f;
    public float floorHeight       = 0.7f;

    private PushableObject objectInReach;

    private void OnTriggerEnter(Collider other)
    {
        var pushableObject = other.GetComponent<PushableObject>();
        if (!pushableObject || IsGrabbing()) return;

        if (objectInReach && objectInReach != pushableObject)
            objectInReach.HideOutline();

        objectInReach = pushableObject;
        objectInReach.ShowOutline();
    }

    private void OnTriggerExit(Collider other)
    {
        var pushableObject = other.GetComponent<PushableObject>();
        if (pushableObject && objectInReach == pushableObject)
        {
            objectInReach.HideOutline();
            objectInReach = null;
        }
    }

    public void TryGrab()
    {
        if (IsGrabbing() || objectInReach == null) return;

        Grab(objectInReach);
        objectInReach.HideOutline();
        objectInReach = null;
    }

    public void HoldUpdate()
    {
        if (!IsGrabbing()) return;

        Vector3 target = transform.position + transform.forward * pushForwardOffset;
        target.y = floorHeight;
        grabbedObject.transform.position = target;
    }

    public override void Release()
    {
        if (!IsGrabbing()) return;

        grabbedObject.SnapToGrid();
        Vector3 pos = grabbedObject.transform.position;
        pos.y = floorHeight;
        grabbedObject.transform.position = pos;

        grabbedObject = null;
    }
}
