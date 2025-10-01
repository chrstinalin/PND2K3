using UnityEngine;

/**
 * This script manages the player's movement state, and
 * switching between moving the mouse and the mech.
 **/
public class MovementManager : PlayerMovementManager
{
    private float _MouseMaxZoom = 4f;
    private float _MechMaxZoom = 10f;

    public AudioSource jumpSound;
    public AudioSource dashSound;

    void Start()
    {
        MovementConfig MouseConfig = new(Mouse, MouseMoveSpeed, 5f, true, 40f, jumpSound, dashSound);
        MovementConfig MechConfig = new(Mech, MouseMoveSpeed, 0f, false, 30f);

        MouseMovementState = new MovementState();
        MechMovementState = new MovementState();

        Debug.Log(dashSound);
        MouseMovementState.EnterState(this, MouseConfig);
        MechMovementState.EnterState(this, MechConfig);

        ToggleMouse(false);
    }

    void Update()
    {
        if (IsMouseActive) MouseMovementState.UpdateState(this, true);
        MechMovementState.UpdateState(this, !IsMouseActive);
        CameraManager.UpdateCamera();

        if (Input.GetButtonDown("MountKey"))
        {
            if (!IsMouseActive) ToggleMouse(true);
            else if (Vector3.Distance(Mouse.transform.position, Mech.transform.position) < MechEnterDistance) ToggleMouse(false);
        }
    }

    public override void ToggleMouse(bool toggle)
    {
        IsMouseActive = toggle;
        Mouse.SetActive(toggle);

        if (IsMouseActive)
        {
            CameraManager.SetFollowEntity(Mouse, _MouseMaxZoom);
            Mouse.GetComponent<Rigidbody>().MovePosition(Mech.transform.position + Mech.transform.forward * -2);
            MechMovementState.UpdateJoyStick(Constant.JOY_RIGHT);
        }
        else
        {
            CameraManager.SetFollowEntity(Mech, _MechMaxZoom);
            MechMovementState.UpdateJoyStick(Constant.JOY_LEFT);
        }        
    }
}
