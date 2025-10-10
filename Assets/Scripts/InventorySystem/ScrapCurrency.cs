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
    public void Drop(Vector3 dropPosition)
    {
        RaycastHit hit;
        if (Physics.Raycast(dropPosition + Vector3.up * 10f, Vector3.down, out hit, 100f))
        {
            transform.position = hit.point;
        }
        else
        {
            transform.position = dropPosition;
        }

        transform.rotation = initialRotation;
        gameObject.SetActive(true);
    }

}
