using UnityEngine;

public class Lever : TriggerAbstract
{
    [SerializeField] private GameObject offModel;
    [SerializeField] private GameObject onModel;

    [SerializeField] private bool startOn = false;

    [SerializeField] private GameObject mouse;
    private bool mouseInside = false;

    private void Awake()
    {
        if (!offModel || !onModel)
            throw new MissingReferenceException($"{name}: Both off and on models must be assigned.");

        IsActive = startOn;
        UpdateVisuals();
    }

    private void Update()
    {
        if (mouseInside && Input.GetButtonDown("Interact"))
        {
            ToggleLever();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == mouse)
        {
            mouseInside = true;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject == mouse)
        {
            mouseInside = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject == mouse)
        {
            mouseInside = false;
        }
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
