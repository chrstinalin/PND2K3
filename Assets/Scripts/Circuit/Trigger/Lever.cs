using UnityEngine;

public class Lever : TriggerAbstract
{
    public GameObject OffModel;
    public GameObject OnModel;

    public bool StartOn = false;

    private void Awake()
    {
        IsActive = StartOn;

        if (OffModel != null) OffModel.SetActive(!StartOn);
        if (OnModel != null) OnModel.SetActive(StartOn);
    }

    public override void Activate()
    {
        if (IsActive) return;
        IsActive = true;

        if (OffModel != null) OffModel.SetActive(false);
        if (OnModel != null) OnModel.SetActive(true);
    }

    public override void Deactivate()
    {
        if (!IsActive) return;
        IsActive = false;

        if (OffModel != null) OffModel.SetActive(true);
        if (OnModel != null) OnModel.SetActive(false);
    }

    public void ToggleLever()
    {
        if (IsActive)
            Deactivate();
        else
            Activate();
    }
}
