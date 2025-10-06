using UnityEngine;

public class MouseInventoryManager : MonoBehaviour
{
    private ScrapCurrency nearbyItem;
    private ScrapCurrency carriedItem;

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
        carriedItem.Collect();
        nearbyItem = null;
        Debug.Log("Mouse picked up: " + item.name);
    }

    private void DropItem()
    {
        if (carriedItem == null) return;

        Vector3 dropPosition = transform.position + transform.forward * 1f;
        carriedItem.Drop(dropPosition);
        Debug.Log("Mouse dropped: " + carriedItem.name);
        carriedItem = null;
    }

    public bool HasItem() => carriedItem != null;
    public ScrapCurrency GetCarriedItem() => carriedItem;
    public void RemoveCarriedItem() => carriedItem = null;
}
