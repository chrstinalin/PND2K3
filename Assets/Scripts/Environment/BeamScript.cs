using UnityEngine;

public class BeamScript : MonoBehaviour
{
    public PlayerMovementManager playerMovementManager;

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == playerMovementManager.MouseMovementState.GroundCollider)
        {
            // Set FollowVector to the X axis of this beam (transform.right)
            playerMovementManager.MouseMovementState.setFollowVector(transform.right);

        }
    }

    private void OnTriggerExit(Collider other)
    {
        playerMovementManager.MouseMovementState.setFollowVector(null);
    }

}
