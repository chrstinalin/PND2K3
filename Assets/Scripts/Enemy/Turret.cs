using System;
using System.Collections;
using UnityEngine;

public class Turret : MonoBehaviour
{
    [NonSerialized] public DamageReceiver DamageReceiver;
    [NonSerialized] public Health Health;
    [NonSerialized] public ScrapCurrency scrapCurrency;
    [SerializeField] private GameObject scrapPilePrefab;

    void Start()
    {
        DamageReceiver = gameObject.AddComponent<DamageReceiver>();
        Health = gameObject.AddComponent<Health>();
        Health.SetMaxHealth(1);
        scrapCurrency = gameObject.AddComponent<ScrapCurrency>();

        Health.onDeath.AddListener(OnDeath);
        DamageReceiver.onTakeDamage.AddListener(TakeDamage);
    }

    public void TakeDamage(int damage)
    {
        Health.TakeDamage(damage);
    }

    void OnDeath()
    {
        LockOnSelectable selectable = GetComponent<LockOnSelectable>();
        if (selectable != null)
        {
            selectable.enabled = false;
            selectable.OnHover(false);
        }

        if (PlayerMarker.Instance != null && PlayerMarker.Instance.Target == gameObject)
        {
            PlayerMarker.Instance.ClearTarget();
        }

        if (scrapPilePrefab != null)
        {
            Vector3 dropPosition = transform.position;
            RaycastHit hit;
            if (Physics.Raycast(transform.position, Vector3.down, out hit, 100f))
            {
                dropPosition = hit.point;
            }

            GameObject scrapPile = Instantiate(scrapPilePrefab, dropPosition, Quaternion.identity);
            ScrapCurrency scrapComponent = scrapPile.GetComponent<ScrapCurrency>();
            if (scrapComponent != null)
            {
                scrapComponent.Drop(dropPosition);
            }
        }

        GameObject turretEnemyParent = transform.parent?.gameObject;
        if (turretEnemyParent != null) Destroy(turretEnemyParent, 0.2f);
        else Destroy(gameObject, 0.2f);
    }
}