using System;
using UnityEngine;
using UnityEngine.UI; // Use Unity UI

public class UIManager : MonoBehaviour
{
    [NonSerialized] public MouseInventoryManager mouseInventory;
    [NonSerialized] public Image mouseItemImage;

    [NonSerialized] public MechaInventoryManager mechaInventory;
    [NonSerialized] public Image[] MechItemImage;

    // Mouse Health UI
    [NonSerialized] private Health MouseHealth;
    [NonSerialized] private Image[] HealthPoints;

    // Mech Health UI
    [NonSerialized] private Health MechHealth;
    [NonSerialized] private Image HealthFront;
    [NonSerialized] private Image HealthBack;
    [NonSerialized] private float lerpTimer;
    [NonSerialized] private float DAMAGE_LERP = 5f;
    [NonSerialized] private float CHIP_LERP = 50f;

    private void Start()
    {
        GameObject HealthPointContainer = GameObject.FindGameObjectWithTag("MouseHealthPointContainer");
        GameObject MechInventoryContainer = GameObject.FindGameObjectWithTag("MechInventoryContainer");
        GameObject _HealthFront = GameObject.FindGameObjectWithTag("MechHealthFront");
        GameObject _HealthBack = GameObject.FindGameObjectWithTag("MechHealthBack");

        if(!HealthPointContainer || ! MechInventoryContainer || !_HealthFront || !_HealthBack)
        {
            Debug.LogError("UI elements not found.");
        }

        // MOUSE
        HealthPoints = HealthPointContainer.GetComponentsInChildren<Image>();

        MouseHealth = PlayerMouse.Instance.GetComponent<Health>();
        MouseHealth.onHealthChanged.AddListener(OnMouseHealthChanged);

        // MECH

        MechItemImage = MechInventoryContainer.GetComponentsInChildren<Image>();
        HealthFront = _HealthFront.GetComponent<Image>();
        HealthBack = _HealthBack.GetComponent<Image>();

        MechHealth = PlayerMech.Instance.GetComponent<Health>();
        MechHealth.onMaxedHealth.AddListener(OnMechMaxedHealth);
        MechHealth.onHealthChanged.AddListener(OnMechHealthChanged);
    }
    private void Update()
    {
        UpdateMouseInventoryUI();
        UpdateMechaInventoryUI();
        updateMechHealthUI();
    }

    private void UpdateMouseInventoryUI()
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

    private void UpdateMechaInventoryUI()
    {
        if (mechaInventory == null) return;
        if (mechaInventory.items != null && mechaInventory.items.Count > 0)
        {
            for (int i = 0; i < mechaInventory.items.Count; i++)
            {
                var item = mechaInventory.items[i];
                MechItemImage[i].sprite = item.icon;
            }
        }
    }

    private void updateMechHealthUI()
    {
        if(!HealthFront || !HealthBack || !MechHealth) return;

        float fillA = HealthFront.fillAmount;
        float fillB = HealthBack.fillAmount;
        float hFraction = (float) MechHealth.GetCurrHealth() / MechHealth.GetMaxHealth();
        if (fillB > hFraction)
        {
            HealthBack.color = Color.red;
            HealthFront.fillAmount = hFraction;

            lerpTimer += Time.deltaTime;

            HealthFront.fillAmount = Mathf.Lerp(fillA, hFraction, lerpTimer / DAMAGE_LERP);
            HealthBack.fillAmount = Mathf.Lerp(fillB, hFraction, lerpTimer / CHIP_LERP);
        }
        else if (fillA < hFraction)
        {
            HealthBack.color = Color.green;
            HealthBack.fillAmount = hFraction;
            lerpTimer += Time.deltaTime;

            HealthFront.fillAmount = Mathf.Lerp(fillA, hFraction, lerpTimer / DAMAGE_LERP);
            HealthBack.fillAmount = Mathf.Lerp(fillB, hFraction, lerpTimer / CHIP_LERP);
        }
    }

    public void OnMechHealthChanged(int damage) => lerpTimer = 0;

    private void OnMechMaxedHealth(int HealthChange)
    {
        HealthFront.fillAmount = 1f;
        HealthBack.fillAmount = 1f;
    }

    public void OnMouseHealthChanged(int damage)
    {
        for (int i = 0; i < MouseHealth.GetMaxHealth(); i++)
        {
            HealthPoints[i].enabled = i <= MouseHealth.GetCurrHealth() - 1;
        }
    }
}
