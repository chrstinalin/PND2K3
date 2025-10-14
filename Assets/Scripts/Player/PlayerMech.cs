using System;
using UnityEngine;

public class PlayerMech : MonoBehaviour
{
    public static PlayerMech Instance;

    [NonSerialized] public Health Health;
    [NonSerialized] public DamageReceiver DamageReceiver;
    [NonSerialized] public MechaInventoryManager InventoryManager;
    [NonSerialized] public MechAIController AIController;


    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        InventoryManager = gameObject.AddComponent<MechaInventoryManager>();
        DamageReceiver = gameObject.AddComponent<DamageReceiver>();
        AIController = gameObject.AddComponent<MechAIController>();

        Health = gameObject.GetComponent<Health>();
        Health.onDeath.AddListener(OnDeath);
        DamageReceiver.onTakeDamage.AddListener(TakeDamage);

    }

    public void TakeDamage(int damage)
    {
        Debug.Log("Mech took damage...");
        Health.TakeDamage(damage);
    }

    public void OnDeath()
    {
        transform.position = new Vector3(0, 1, 0);
        Health.Heal(Health.GetMaxHealth());
    }

}