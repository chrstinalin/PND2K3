using UnityEngine;

public class ScrapCurrency : MonoBehaviour
{
    public int HPRestoreAmount;

    public Sprite icon;

    private Vector3 initialPosition;
    private Quaternion initialRotation;

    void Awake()
    {
        initialPosition = transform.position;
        initialRotation = transform.rotation;
    }

    public void Collect()
    {
        gameObject.SetActive(false);
    }

    public void Drop(Vector3 dropPosition, float heightAboveFloor = 0.5f)
    {
        RaycastHit hit;
        if (Physics.Raycast(dropPosition + Vector3.up * 10f, Vector3.down, out hit, 100f))
        {
            transform.position = hit.point + Vector3.up * heightAboveFloor;
        }
        else
        {
            transform.position = dropPosition;
        }

        transform.rotation = initialRotation;
        gameObject.SetActive(true);
    }

}
