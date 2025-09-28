using UnityEngine;

public class Lever : TriggerAbstract
{
    [SerializeField] private GameObject offModel;
    [SerializeField] private GameObject onModel;

    [SerializeField] private bool startOn = false;

    private void Awake()
    {
        if (!offModel || !onModel)
        throw new MissingReferenceException(
            $"{name}: Both off and on models must be assigned.");

        IsActive = startOn;
        UpdateVisuals();
    }

    public override void Activate()
    {
        if (IsActive) return;
        IsActive = true;
        UpdateVisuals();
    }

    public override void Deactivate()
    {
        if (!IsActive) return;
        IsActive = false;
        UpdateVisuals();
    }

    public void ToggleLever()
    {
        if (IsActive) Deactivate();
        else Activate();
    }

    private void UpdateVisuals()
    {
        offModel.SetActive(!IsActive);
        onModel.SetActive(IsActive);
    }
}
