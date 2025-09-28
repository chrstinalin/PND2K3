using UnityEngine;

public class Door : TriggerableAbstract
{
    public GameObject ClosedDoor;
    public GameObject OpenDoor;

    private void Awake()
    {
        if (ClosedDoor != null) ClosedDoor.SetActive(true);
        if (OpenDoor != null) OpenDoor.SetActive(false);
        IsOn = false;
    }

    public override void TurnOn()
    {
        if (IsOn) return;
        IsOn = true;

        if (ClosedDoor != null) ClosedDoor.SetActive(false);
        if (OpenDoor != null) OpenDoor.SetActive(true);
    }

    public override void TurnOff()
    {
        if (!IsOn) return;
        IsOn = false;

        if (ClosedDoor != null) ClosedDoor.SetActive(true);
        if (OpenDoor != null) OpenDoor.SetActive(false);
    }
}
