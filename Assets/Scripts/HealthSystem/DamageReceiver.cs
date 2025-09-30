using UnityEngine;
using UnityEngine.Events;

public class DamageReceiver : MonoBehaviour
{
    [SerializeField] private UnityEvent<int> onTakeDamage;
    
    void OnTriggerEnter(Collider other)
    {
        var bullet = other.GetComponent<Bullet>();
        if (bullet == null) return;
        Destroy(other.gameObject);
        if (!bullet.DamageDealt)
        {
            bullet.DamageDealt = true;
            onTakeDamage.Invoke(bullet.damage);
        }
    }
}