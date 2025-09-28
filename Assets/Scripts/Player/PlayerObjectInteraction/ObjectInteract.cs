using UnityEngine;

public class ObjectInteract : MonoBehaviour
{
    private Lever targetLever;

    private void OnTriggerEnter(Collider other)
    {
        Lever lever = other.GetComponent<Lever>();
        if (lever != null)
        {
            Debug.Log(lever.name);
            targetLever = lever;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Lever lever = other.GetComponent<Lever>();
        if (lever != null && lever == targetLever)
        {
            targetLever = null;
        }
    }

    private void Update()
    {
        if (targetLever != null && Input.GetButtonDown("Interact"))
        {
            targetLever.ToggleLever();
        }
    }
}
