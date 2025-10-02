using System;
using UnityEngine;
using UnityEngine.Events;

public class DamageReceiver : MonoBehaviour
{
    [NonSerialized] public UnityEvent<int> onTakeDamage = new();
    
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