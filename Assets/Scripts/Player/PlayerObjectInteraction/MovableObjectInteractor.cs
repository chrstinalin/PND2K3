using UnityEngine;

public class MovableObjectInteractor : MovableObjectAbstractInteractor
{
    public float carryForwardOffset = 1.7f;
    public float carryHeightOffset = 1f;
    public float floorHeight = 0.7f;

    private MovableObject objectInReach;
    private Collider grabbedCollider;

    private void OnTriggerEnter(Collider other)
    {
        MovableObject mo = other.GetComponent<MovableObject>();
        if (mo != null && !IsGrabbing())
        {
            if (objectInReach != null && objectInReach != mo)
                objectInReach.HideOutline();

            objectInReach = mo;
            objectInReach.ShowOutline();
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (IsGrabbing()) return;

        MovableObject mo = other.GetComponent<MovableObject>();
        if (mo != null && objectInReach != mo)
        {
            if (objectInReach != null)
                objectInReach.HideOutline();
            objectInReach = mo;
            objectInReach.ShowOutline();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        MovableObject mo = other.GetComponent<MovableObject>();
        if (mo != null && objectInReach == mo)
        {
            objectInReach.HideOutline();
            objectInReach = null;
        }
    }

    private void Update()
    {
        bool isButtonHeld = Input.GetButton("MoveItem");

        if (isButtonHeld)
        {
            HandleGrab();
            MoveGrabbedObject();
        }
        else
        {
            Release();
        }
    }

    private void HandleGrab()
    {
        if (!IsGrabbing() && objectInReach != null)
        {
            Grab(objectInReach);

            grabbedCollider = grabbedObject.GetComponent<Collider>();
            if (grabbedCollider != null)
                grabbedCollider.isTrigger = true;

            grabbedObject.ShowGhost(floorHeight);

            grabbedObject.HideOutline();

            objectInReach = null;
        }
    }

    private void MoveGrabbedObject()
    {
        if (!IsGrabbing()) return;

        Vector3 target = transform.position + transform.forward * carryForwardOffset;
        target.y = floorHeight + carryHeightOffset;
        grabbedObject.transform.position = target;

        grabbedObject.UpdateGhostPosition(floorHeight);
    }

    public override void Release()
    {
        if (!IsGrabbing()) return;

        grabbedObject.SnapToGrid();

        Vector3 pos = grabbedObject.transform.position;
        pos.y = floorHeight;
        grabbedObject.transform.position = pos;

        if (grabbedCollider != null)
            grabbedCollider.isTrigger = false;

        grabbedObject.DestroyGhost();

        grabbedObject = null;
        grabbedCollider = null;
    }
}
