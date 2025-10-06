using UnityEngine;

public class MechaInventoryManager : MonoBehaviour
{
    public GameObject Mouse;
    public Health health;

    private void Update()
    {
        if (Mouse == null) return;

        var mouseInventory = Mouse.GetComponent<MouseInventoryManager>();
        if (mouseInventory == null) return;

        // Auto-transfer if Mouse is inactive
        if (!Mouse.activeInHierarchy && mouseInventory.HasItem())
        {
            TakeFromMouse(mouseInventory);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        var mouseInventory = other.GetComponentInParent<MouseInventoryManager>();
        if (mouseInventory != null && mouseInventory.HasItem())
        {
            TakeFromMouse(mouseInventory);
        }
    }

    private void TakeFromMouse(MouseInventoryManager mouseInventory)
    {
        if (!mouseInventory.HasItem()) return;

        ScrapCurrency scrap = mouseInventory.GetCarriedItem();
        if (scrap != null)
        {
            // Heal Mecha
            if (health != null)
            {
                health.Heal(scrap.HPRestoreAmount);
            }

            // Destroy scrap object game object
            Destroy(scrap.gameObject);

            // Remove from Mouse inventory
            mouseInventory.RemoveCarriedItem();
        }
    }
}
