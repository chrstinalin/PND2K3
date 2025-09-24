using UnityEngine;

public class MovementConfig
{
    public GameObject Entity;
    public float MoveSpeed;
    public float JumpForce;
    public float DashSpeed;

    public bool CanJump;
    
    public MovementConfig(GameObject entity, float moveSpeed, float jumpForce, bool canJump, 
        float dashSpeed)
    {
        Entity = entity;
        MoveSpeed = moveSpeed;
        JumpForce = jumpForce;
        CanJump = canJump;
        DashSpeed = dashSpeed;
    }
}