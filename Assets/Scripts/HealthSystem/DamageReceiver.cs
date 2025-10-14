using System;
using UnityEngine;
using UnityEngine.Events;

public class DamageReceiver : MonoBehaviour
{
    [NonSerialized] public UnityEvent<int> onTakeDamage = new();
    
    public void ReceiveDamage(int damage)
    {
        onTakeDamage.Invoke(damage);
    }
}