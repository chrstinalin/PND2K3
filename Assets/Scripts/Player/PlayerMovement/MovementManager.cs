using System;
using UnityEngine;

/**
 * This script manages the player's movement state, and
 * switching between moving the mouse and the mech.
 **/
public class MovementManager : PlayerMovementManager
{
    [NonSerialized] public CameraManager CameraManager;

    public static MovementManager Instance;

    private GameObject Mouse;
    private GameObject Mech;

    private float _MouseMaxZoom = 4f;
    private float _MechMaxZoom = 10f;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        CameraManager = CameraManager.Instance;

        Mouse = PlayerMouse.Instance.gameObject;
        Mech = PlayerMech.Instance.gameObject;

        MovementConfig MouseConfig = new(Mouse, Config.MOUSE_MOVE_SPEED, Config.MOUSE_JUMP_FORCE, Config.MOUSE_DASH_SPEED);
        MovementConfig MechConfig = new(Mech, Config.MECH_MOVE_SPEED, Config.MECH_JUMP_FORCE, Config.MECH_DASH_SPEED);

        MouseMovementState = new MovementState();
        MechMovementState = new MovementState();

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
            else if (Vector3.Distance(Mouse.transform.position, Mech.transform.position) < Config.MECH_ENTER_DISTANCE) ToggleMouse(false);
        }
    }

    public override void ToggleMouse(bool toggle)
    {
        IsMouseActive = toggle;
        Mouse.SetActive(toggle);

        if (IsMouseActive)
        {
            CameraManager.SetFollowEntity(Mouse, _MouseMaxZoom);
            Mouse.transform.position = Mech.transform.position + Mech.transform.forward * -2;
            MechMovementState.UpdateJoyStick(Constant.JOY_RIGHT);
        }
        else
        {
            CameraManager.SetFollowEntity(Mech, _MechMaxZoom);
            MechMovementState.UpdateJoyStick(Constant.JOY_LEFT);
        }        
    }
}
