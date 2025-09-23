using UnityEngine;

public class LiftableObjectInteractor : MovableObjectAbstractInteractor
{
    public float carryForwardOffset = 1.7f;
    public float carryHeightOffset = 1f;
    public float floorHeight = 0.7f;

    private LiftableObject objectInReach;
    private Collider grabbedCollider;

    private void OnTriggerEnter(Collider other)
    {
        var liftableObject = other.GetComponent<LiftableObject>();
        if (!liftableObject || IsGrabbing()) return;

        if (objectInReach && objectInReach != liftableObject)
            objectInReach.HideOutline();

        objectInReach = liftableObject;
        objectInReach.ShowOutline();
    }

    private void OnTriggerExit(Collider other)
    {
        var liftableObject = other.GetComponent<LiftableObject>();
        if (liftableObject && objectInReach == liftableObject)
        {
            objectInReach.HideOutline();
            objectInReach = null;
        }
    }

    public void TryGrab()
    {
        if (IsGrabbing() || objectInReach == null) return;

        Grab(objectInReach);
        grabbedCollider = grabbedObject.GetComponent<Collider>();
        if (grabbedCollider) grabbedCollider.isTrigger = true;

        objectInReach.ShowGhost(floorHeight);
        objectInReach.HideOutline();
        objectInReach = null;
    }

    public void HoldUpdate()
    {
        if (!IsGrabbing()) return;

        Vector3 target = transform.position + transform.forward * carryForwardOffset;
        target.y = floorHeight + carryHeightOffset;
        grabbedObject.transform.position = target;

        ((LiftableObject)grabbedObject).UpdateGhostPosition(floorHeight);
    }

    public override void Release()
    {
        if (!IsGrabbing()) return;

        grabbedObject.SnapToGrid();
        Vector3 pos = grabbedObject.transform.position;
        pos.y = floorHeight;
        grabbedObject.transform.position = pos;

        if (grabbedCollider) grabbedCollider.isTrigger = false;
        ((LiftableObject)grabbedObject).DestroyGhost();

        grabbedObject = null;
        grabbedCollider = null;
    }
}
