using UnityEngine;

public class BeamScript : MonoBehaviour
{
    public PlayerMovementManager playerMovementManager;

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == playerMovementManager.MouseMovementState.GroundCollider)
        {
            playerMovementManager.MouseMovementState.setFollowVector(transform.right);

        }
    }

    private void OnTriggerExit(Collider other)
    {
        playerMovementManager.MouseMovementState.setFollowVector(null);
    }

}
