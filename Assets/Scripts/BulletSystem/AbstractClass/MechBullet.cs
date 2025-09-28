using UnityEngine;

/*
 * Abstract bullet class.
 * Has a damage value, speed, and lifetime.
 */
public abstract class MechBullet : MonoBehaviour
{
    public int damage;
    public int speed;
    public float lifetime = 5f;

    void Update()
    {
        HandleLifetime();
        HandleMovement();
    }

    protected virtual void HandleLifetime()
    {
        lifetime -= Time.deltaTime;
        if (lifetime <= 0)
        {
            Debug.Log("destroy mech bullet");
            Destroy(gameObject);
        }
    }

    protected abstract void HandleMovement();

    void OnTriggerEnter(Collider other)
    {
        Debug.Log("Trigger enter");
        if (other.GetComponentInParent<MechaInventoryManager>() != null || other.GetComponent<DamageReceiver>() != null)
        {
            return;
        }
        Destroy(gameObject); 
    }
}
