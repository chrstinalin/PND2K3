using UnityEngine;
using UnityEngine.UI;

public class InventoryUIManager : MonoBehaviour
{
    public MouseInventoryManager mouseInventory;
    public Image mouseItemImage;

    private void Update()
    {
        UpdateMouseUI();
    }

    private void UpdateMouseUI()
    {
        if (mouseInventory == null || mouseItemImage == null) return;

        ScrapCurrency carried = mouseInventory.GetCarriedItem();
        if (carried != null)
        {
            mouseItemImage.sprite = carried.icon;
            mouseItemImage.enabled = true;
        }
        else
        {
            mouseItemImage.enabled = false;
            mouseItemImage.sprite = null;
        }
    }
}
