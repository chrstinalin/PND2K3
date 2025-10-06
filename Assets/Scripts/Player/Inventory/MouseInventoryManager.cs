using UnityEngine;

public class MouseInventoryManager : InventoryAbstractManager
{
    private CollectibleItem nearbyCollectible;

    private void Awake()
    {
        if (items == null)
            items = new System.Collections.Generic.List<ItemData>();

        maxNumItems = Config.MOUSE_INVENTORY_SIZE;
    }

    private void OnTriggerEnter(Collider other)
    {
        CollectibleItem collectible = other.GetComponent<CollectibleItem>();
        if (collectible != null)
        {
            nearbyCollectible = collectible;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        CollectibleItem collectible = other.GetComponent<CollectibleItem>();
        if (collectible != null && collectible == nearbyCollectible)
        {
            nearbyCollectible = null; 
        }
    }

    private void Update()
    {
        if (Input.GetButtonDown("Interact"))
        {
            // Check if we need to remove an item first
            RemoveItem(0);

            if (nearbyCollectible != null)
            {
                AddItem(nearbyCollectible.itemData);
                nearbyCollectible.OnPickUp();
                nearbyCollectible = null;
            }
        }
        
        
    }

    public override void AddItem(ItemData item)
    {
        if (items.Count >= maxNumItems)
        {
            Debug.Log("Mouse inventory full! Cannot pick up another item.");
            return;
        }

        items.Add(item);
        Debug.Log("Item picked up by Peanut");
    }

    public override void RemoveItem(int index)
    {
        if (index < 0 || index >= items.Count)
        {
            return;
        }

        ItemData removedItem = items[index];
        items.RemoveAt(index);
        Debug.Log("Removed item: " + removedItem.name);
    }
}
