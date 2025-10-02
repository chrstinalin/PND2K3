using UnityEngine;

public class DestroyableWall : MonoBehaviour
{
    public AudioClip WallBreakSFX;
    private Health health;
    
    void Awake()
    {
        health = gameObject.GetComponent<Health>();
        health.onDeath.AddListener(OnWallDestroyed);
    }

    void OnWallDestroyed()
    {
        Debug.Log($"{gameObject.name} has been destroyed!");
        AudioManager.Instance.PlaySFX(WallBreakSFX);
        Destroy(gameObject);
    }
}