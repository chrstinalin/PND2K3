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

    public void Drop(Vector3 dropPosition)
    {
        transform.position = dropPosition;
        transform.rotation = initialRotation;
        gameObject.SetActive(true);
    }
}
