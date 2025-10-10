using UnityEngine;

public class MouseInventoryManager : MonoBehaviour
{
    private ScrapCurrency nearbyItem;
    public ScrapCurrency carriedItem;
    public Transform carryPoint;

    private void Awake()
    {
        carryPoint = transform.Find("CarryPoint");
        if (carryPoint == null)
            Debug.LogError(
                "A Transform named CarryPoint where the scrap currency " +
                "appears when carried must be a child of PlayerMouse"
            );
    }

    private void OnTriggerEnter(Collider other)
    {
        var item = other.GetComponent<ScrapCurrency>();
        if (item != null)
        {
            nearbyItem = item;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        var item = other.GetComponent<ScrapCurrency>();
        if (item != null && item == nearbyItem)
        {
            nearbyItem = null;
        }
    }

    private void Update()
    {
        if (Input.GetButtonDown("Interact"))
        {
            if (carriedItem == null && nearbyItem != null)
            {
                PickUpItem(nearbyItem);
            }
            else if (carriedItem != null)
            {
                DropItem();
            }
        }
    }

    private void PickUpItem(ScrapCurrency item)
    {
        carriedItem = item;
        nearbyItem = null;

        item.transform.SetParent(carryPoint);

        item.transform.localPosition = Vector3.zero;
        item.transform.localRotation = Quaternion.identity;

        item.transform.localScale = Vector3.one * 0.5f;

        Debug.Log("Mouse picked up: " + item.name);
    }


    private void DropItem()
    {
        if (carriedItem == null) return;

        Vector3 dropPosition = transform.position + transform.forward * 1f;

        carriedItem.transform.SetParent(null);

        carriedItem.transform.localScale = Vector3.one;

        carriedItem.Drop(dropPosition);

        Debug.Log("Mouse dropped: " + carriedItem.name);
        carriedItem = null;
    }

    public bool HasItem() => carriedItem != null;
    public ScrapCurrency GetCarriedItem() => carriedItem;
    public void RemoveCarriedItem() => carriedItem = null;
}
