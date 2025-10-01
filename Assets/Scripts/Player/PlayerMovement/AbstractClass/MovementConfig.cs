using UnityEngine;

public class MovementConfig
{
    public GameObject Entity;
    public float MoveSpeed;
    public float JumpForce;
    public float DashSpeed;

    public bool CanJump;

    public AudioSource JumpSound;

    public AudioSource DashSound;

    public MovementConfig(GameObject entity, float moveSpeed, float jumpForce, bool canJump,
        float dashSpeed, AudioSource jumpSound = null, AudioSource dashSound = null)
    {
        Entity = entity;
        MoveSpeed = moveSpeed;
        JumpForce = jumpForce;
        CanJump = canJump;
        DashSpeed = dashSpeed;
        JumpSound = jumpSound;
        DashSound = dashSound;
    }
}