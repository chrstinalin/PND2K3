using UnityEngine;
using UnityEngine.UI; // Use Unity UI
using System.Text;
using System.Collections.Generic;

public class InventoryUIManager : MonoBehaviour
{
    [Header("Mouse Inventory UI")]
    public MouseInventoryManager mouseInventory;
    public Image mouseItemImage;

    [Header("Mecha Inventory UI")]
    public MechaInventoryManager mechaInventory;
    public Image[] mechItemImage;

    public Text mechaInventoryText;

    private void Update()
    {
        mechItemImage = GameObject.FindGameObjectWithTag("MechInventoryContainer").GetComponentsInChildren<Image>();


        UpdateMouseUI();
        UpdateMechaUI();
    }

    private void UpdateMouseUI()
    {
        if (mouseInventory == null || mouseItemImage == null) return;

        if (mouseInventory.items.Count > 0 && mouseInventory.items[0].icon != null)
        {
            mouseItemImage.sprite = mouseInventory.items[0].icon;
        }
        else
        {
            mouseItemImage.sprite = null;
        }
    }

    private void UpdateMechaUI()
    {
        if (mechaInventory == null || mechaInventoryText == null) return;

        if (mechaInventory.items != null && mechaInventory.items.Count > 0)
        {
            for (int i = 0; i < mechaInventory.items.Count; i++)
            {
                var item = mechaInventory.items[i];
                mechItemImage[i].sprite = item.icon;
            }
        }
    }

}
