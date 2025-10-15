using System;
using UnityEngine;
using UnityEngine.Events;

public class Health : MonoBehaviour
{
    [SerializeField] private int maxHealth = 3;
    [SerializeField] private int currHealth;

    [NonSerialized] public UnityEvent<int> onMaxedHealth = new();
    [NonSerialized] public UnityEvent<int> onHealthChanged = new();
    [NonSerialized] public UnityEvent onDeath = new();

    public int GetCurrHealth() => currHealth;
    public int GetMaxHealth() => maxHealth;
    public bool IsAlive() => currHealth > 0;

    void Start()
    {
        currHealth = maxHealth;
    }
    
    public void SetMaxHealth(int newMaxHealth)
    {
        maxHealth = newMaxHealth;
        currHealth = Mathf.Clamp(currHealth, 0, maxHealth);
        onHealthChanged.Invoke(currHealth);
    }

    public void TakeDamage(int damage)
    {
        int damageAmount = damage;
        currHealth = Mathf.Clamp(currHealth - damageAmount, 0, maxHealth);

        Debug.Log($"Took {damageAmount} damage. Health: {currHealth}/{maxHealth}");

        onHealthChanged.Invoke(currHealth);
        if (currHealth <= 0) onDeath.Invoke();
    }

    public void Heal(int healAmount)
    {
        Debug.Log($"Gained {healAmount} health. Health: {currHealth}/{maxHealth}");

        currHealth = Mathf.Clamp(currHealth + healAmount, 0, maxHealth);
        
        onHealthChanged.Invoke(currHealth);
        if(currHealth == maxHealth) onMaxedHealth.Invoke(currHealth);

        Debug.Log($"Health: {currHealth}/{maxHealth}");
    }
}