using UnityEngine;

public abstract class TriggerAbstract : MonoBehaviour
{
    public bool IsActive { get; protected set; }

    public AudioSource activateSound;
    public AudioSource deactivateSound;

    protected void PlayActivateSound()
    {
        if (activateSound)
            activateSound.Play();
    }

    protected void PlayDeactivateSound()
    {
        if (deactivateSound)
            deactivateSound.Play();
    }

    public abstract void Activate();
    public abstract void Deactivate();
}
