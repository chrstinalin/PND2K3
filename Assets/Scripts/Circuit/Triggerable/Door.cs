using UnityEngine;

public class Door : TriggerableAbstract
{
    [SerializeField] private GameObject closedDoor;
    [SerializeField] private GameObject OpenDoor;

    private void Awake()
    {
        if (!closedDoor || !OpenDoor)
        throw new MissingReferenceException(
            $"{name}: Both closed and open door models must be assigned.");

        closedDoor.SetActive(!IsOn);
        OpenDoor.SetActive(IsOn);
        IsOn = false;
    }

    public override void TurnOn()
    {
        if (IsOn) return;
        IsOn = true;

        closedDoor.SetActive(!IsOn);
        OpenDoor.SetActive(IsOn);
    }

    public override void TurnOff()
    {
        if (!IsOn) return;
        IsOn = false;

        closedDoor.SetActive(!IsOn);
        OpenDoor.SetActive(IsOn);
    }
}
