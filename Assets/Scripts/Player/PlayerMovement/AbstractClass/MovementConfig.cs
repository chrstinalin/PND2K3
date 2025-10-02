using UnityEngine;

public class MovementConfig
{
    public GameObject Entity;
    public float MoveSpeed;
    public float JumpForce;
    public float DashSpeed;
    public bool CanJump => JumpForce > 0f;
    
    public MovementConfig(GameObject entity, float moveSpeed, float jumpForce, float dashSpeed)
    {
        Entity = entity;
        MoveSpeed = moveSpeed;
        JumpForce = jumpForce;
        DashSpeed = dashSpeed;
    }
}