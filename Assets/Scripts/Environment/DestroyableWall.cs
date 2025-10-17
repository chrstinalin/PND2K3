using UnityEngine;

public class DestroyableWall : MonoBehaviour
{
    public AudioClip WallBreakSFX;
    private DamageReceiver DamageReceiver;
    private Health Health;
    
    void Awake()
    {
        Health = gameObject.GetComponent<Health>();
        Health.onDeath.AddListener(OnWallDestroyed);
        DamageReceiver = gameObject.AddComponent<DamageReceiver>();
        DamageReceiver.onTakeDamage.AddListener(TakeDamage);
    }
    
    public void TakeDamage(int damage)
    {
        if (Health != null)
        {
            Health.TakeDamage(damage);
        }
    }

    void OnWallDestroyed()
    {
        Debug.Log($"{gameObject.name} has been destroyed!");
        if (WallBreakSFX != null)
        {
            AudioManager.Instance.PlaySFX(WallBreakSFX);
        }
        Destroy(gameObject);
    }
}