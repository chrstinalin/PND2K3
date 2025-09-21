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
        MovableObject movableObject = other.GetComponent<MovableObject>();
        if (movableObject != null && !IsGrabbing())
        {
            if (objectInReach != null && objectInReach != movableObject)
                objectInReach.HideOutline();

            objectInReach = movableObject;
            objectInReach.ShowOutline();
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (IsGrabbing()) return;

        MovableObject movableObject = other.GetComponent<MovableObject>();
        if (movableObject != null && objectInReach != movableObject)
        {
            if (objectInReach != null)
                objectInReach.HideOutline();

            objectInReach = movableObject;
            objectInReach.ShowOutline();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        MovableObject movableObject = other.GetComponent<MovableObject>();
        if (movableObject != null && objectInReach == movableObject)
        {
            objectInReach.HideOutline();
            objectInReach = null;
        }
    }

    private void Update()
    {
        if (Input.GetButtonDown("MoveItem"))
        {
            if (IsGrabbing())
                Release();
            else
                HandleGrab();
        }

        if (IsGrabbing())
            MoveGrabbedObject();
    }

    private void HandleGrab()
    {
        if (!IsGrabbing() && objectInReach != null)
        {
            Grab(objectInReach);

            grabbedCollider = grabbedObject.GetComponent<Collider>();
            if (grabbedCollider != null)
                grabbedCollider.isTrigger = true;

            if (grabbedObject.interactionType == MovableObject.MovableType.Pickup)
            {
                grabbedObject.ShowGhost(floorHeight);
            }

            grabbedObject.HideOutline();
            objectInReach = null;
        }
    }

    private void MoveGrabbedObject()
    {
        if (!IsGrabbing()) return;

        if (grabbedObject.interactionType == MovableObject.MovableType.Pickup)
        {
            Vector3 target = transform.position + transform.forward * carryForwardOffset;
            target.y = floorHeight + carryHeightOffset;
            grabbedObject.transform.position = target;

            grabbedObject.UpdateGhostPosition(floorHeight);
        }
        else if (grabbedObject.interactionType == MovableObject.MovableType.Push)
        {
            Vector3 target = transform.position + transform.forward * carryForwardOffset;
            target.y = floorHeight;
            grabbedObject.transform.position = target;
        }
    }

    public override void Release()
    {
        if (!IsGrabbing()) return;

        grabbedObject.SnapToGrid();

        Vector3 pos = grabbedObject.transform.position;
        pos.y = floorHeight;
        grabbedObject.transform.position = pos;

        if (grabbedObject.interactionType == MovableObject.MovableType.Pickup && grabbedCollider != null)
            grabbedCollider.isTrigger = false;

        if (grabbedObject.interactionType == MovableObject.MovableType.Pickup)
            grabbedObject.DestroyGhost();

        grabbedObject = null;
        grabbedCollider = null;
    }
}
