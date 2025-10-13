using System;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public abstract class PlayerMovementState {
    public abstract void EnterState(PlayerMovementManager manager, MovementConfig config);
    public abstract void UpdateState(PlayerMovementManager manager, bool isActive);
    public abstract void UpdateJoyStick(Joystick Input);
    public abstract void setFollowVector(Vector3? vec);
    public abstract void Reset();
}
