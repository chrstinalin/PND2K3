using UnityEngine;

public class SFXManager : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip bulletFireClip;
    [SerializeField] private AudioClip wallBreakClip;
    
    public static SFXManager Instance;
    
    void Awake() 
    {
        if (Instance == null) Instance = this;
    }
    
    public void PlayBulletFire() 
    {
        audioSource.PlayOneShot(bulletFireClip);
    }
    
    public void PlayWallBreak()
    {
        audioSource.PlayOneShot(wallBreakClip);
    }
}
