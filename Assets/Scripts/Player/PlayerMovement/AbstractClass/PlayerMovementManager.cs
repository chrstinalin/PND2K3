using System;
using UnityEngine;

public abstract class PlayerMovementManager : MonoBehaviour
{
    [NonSerialized] public float zoom;

    public PlayerMovementState MouseMovementState;
    public PlayerMovementState MechMovementState;

    [NonSerialized] public bool IsMouseActive;

    public abstract void ToggleMouse(bool toggle);
}
