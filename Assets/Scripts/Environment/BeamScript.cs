using UnityEngine;

public class BeamScript : MonoBehaviour
{
    public PlayerMovementManager playerMovementManager;

    void OnTriggerEnter(Collider other)
    {
        Debug.Log("Beam Trigger Entered by: " + other.gameObject.name);
        Debug.Log("MovementState GroundCollider" + playerMovementManager.MouseMovementState.GroundCollider);
        if (other.gameObject == playerMovementManager.MouseMovementState.GroundCollider)
        {
            playerMovementManager.MouseMovementState.setFollowVector(transform.right);

        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject == playerMovementManager.MouseMovementState.GroundCollider)
        {
            playerMovementManager.MouseMovementState.setFollowVector(null);

        }
    }

}
