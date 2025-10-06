using System;
using UnityEngine;

public class PlayerMouse : MonoBehaviour
{
    public static PlayerMouse Instance;

    [NonSerialized] public Health Health;
    [NonSerialized] public DamageReceiver DamageReceiver;
    [NonSerialized] public MouseInventoryManager InventoryManager;

    [NonSerialized] public GameObject GroundCollider;

    void Awake()
    {
        Instance = this;
    }
    void Start()
    {
        InventoryManager = gameObject.AddComponent<MouseInventoryManager>();
        DamageReceiver = gameObject.AddComponent<DamageReceiver>();
        GroundCollider = GameObject.FindGameObjectWithTag("MouseGroundCollider");

        // Init Health
        Health = gameObject.GetComponent<Health>();
        Health.onDeath.AddListener(OnDeath);
        DamageReceiver.onTakeDamage.AddListener(Health.TakeDamage);
    }
    
    public void OnDeath()
    {
        Debug.Log("Player Died. Respawning...");
        transform.position = new Vector3(0, 1, 0);
        Health.Heal(Health.GetMaxHealth());
    }
}