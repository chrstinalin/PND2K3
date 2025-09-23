using UnityEngine;

public class MovableObjectHandler : MonoBehaviour
{
    public LiftableObjectInteractor liftInteractor;
    public PushableObjectInteractor pushInteractor;

    private void Update()
    {
        bool holding = liftInteractor && liftInteractor.enabled && liftInteractorIsGrabbing() ||
                       pushInteractor && pushInteractor.enabled && pushInteractorIsGrabbing();

        if (Input.GetButtonDown("MoveItem"))
        {
            if (holding)
            {
                ReleaseCurrent();
            }
            else
            {
                if (liftInteractor) liftInteractor.TryGrab();
                if (!liftInteractorIsGrabbing() && pushInteractor)
                    pushInteractor.TryGrab();
            }
        }

        if (liftInteractorIsGrabbing()) liftInteractor.HoldUpdate();
        else if (pushInteractorIsGrabbing()) pushInteractor.HoldUpdate();
    }

    private bool liftInteractorIsGrabbing()
        => liftInteractor && liftInteractor.enabled && liftInteractor.GetType()
              .BaseType.GetMethod("IsGrabbing", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
              .Invoke(liftInteractor, null) as bool? == true;

    private bool pushInteractorIsGrabbing()
        => pushInteractor && pushInteractor.enabled && pushInteractor.GetType()
              .BaseType.GetMethod("IsGrabbing", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
              .Invoke(pushInteractor, null) as bool? == true;

    private void ReleaseCurrent()
    {
        if (liftInteractorIsGrabbing()) liftInteractor.Release();
        else if (pushInteractorIsGrabbing()) pushInteractor.Release();
    }
}
