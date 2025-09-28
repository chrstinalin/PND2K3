using UnityEngine;

public class Door : TriggerableAbstract
{
    public GameObject ClosedDoor;
    public GameObject OpenDoor;

    private void Awake()
    {
        if (ClosedDoor) ClosedDoor.SetActive(!IsOn);
        if (OpenDoor)   OpenDoor.SetActive(IsOn);
        IsOn = false;
    }

    public override void TurnOn()
    {
        if (IsOn) return;
        IsOn = true;

        if (ClosedDoor) ClosedDoor.SetActive(!IsOn);
        if (OpenDoor) OpenDoor.SetActive(IsOn);
    }

    public override void TurnOff()
    {
        if (!IsOn) return;
        IsOn = false;

        if (ClosedDoor) ClosedDoor.SetActive(!IsOn);
        if (OpenDoor) OpenDoor.SetActive(IsOn);
    }
}
