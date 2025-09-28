using UnityEngine;
using UnityEngine.UI;

public class PlayerMouse : MonoBehaviour
{
    private Health _health;
    private Image[] HealthPoints;
    
    void Start()
    {
        HealthPoints = GameObject.FindGameObjectWithTag("MouseHealthPointContainer").GetComponentsInChildren<Image>();
        _health = GetComponent<Health>();
        _health.onHealthChanged.AddListener(OnHealthChanged);
        _health.onDeath.AddListener(OnDeath);
    }
    
    public void OnHealthChanged(int damage)
    {
        for (int i = 0; i < _health.GetMaxHealth(); i++)
        {
            HealthPoints[i].enabled = i <= _health.GetCurrHealth() - 1;
        }
    }

    public void OnDeath()
    {
        Debug.Log("Player Died. Respawning...");
        transform.position = new Vector3(0, 1, 0);
        _health.Heal(_health.GetMaxHealth());
    }
}