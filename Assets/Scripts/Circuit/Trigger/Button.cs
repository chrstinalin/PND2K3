using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class Button : TriggerAbstract
{
    public List<GameObject> TriggerObjects = new List<GameObject>();

    public GameObject UnpressedModel;
    public GameObject PressedModel;

    private void Awake()
    {
        Collider col = GetComponent<Collider>();
        col.isTrigger = true;

        if (UnpressedModel != null) UnpressedModel.SetActive(true);
        if (PressedModel != null) PressedModel.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (TriggerObjects.Contains(other.gameObject))
            Activate();
    }

    private void OnTriggerExit(Collider other)
    {
        if (TriggerObjects.Contains(other.gameObject))
            Deactivate();
    }

    public override void Activate()
    {
        if (IsActive) return;
        IsActive = true;

        if (UnpressedModel != null) UnpressedModel.SetActive(false);
        if (PressedModel != null) PressedModel.SetActive(true);
    }

    public override void Deactivate()
    {
        if (!IsActive) return;
        IsActive = false;

        if (UnpressedModel != null) UnpressedModel.SetActive(true);
        if (PressedModel != null) PressedModel.SetActive(false);
    }
}
