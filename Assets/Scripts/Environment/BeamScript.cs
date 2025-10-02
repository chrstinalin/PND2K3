using UnityEngine;

public class BeamScript : MonoBehaviour
{

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == PlayerMouse.Instance.GroundCollider)
        {
            MovementManager.Instance.MouseMovementState.setFollowVector(transform.right);

        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject == PlayerMouse.Instance.GroundCollider)
        {
            MovementManager.Instance.MouseMovementState.setFollowVector(null);

        }
    }

}
