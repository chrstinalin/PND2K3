using UnityEngine;

/*
 * Abstract bullet class.
 * Has a damage value, speed, and lifetime.
 */
public abstract class Bullet : MonoBehaviour
{
    public int damage;
    public int speed;
    public float lifetime = 5f;
    public bool DamageDealt = false;

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
            Destroy(gameObject);
        }
    }

    protected abstract void HandleMovement();
    
    void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<DamageReceiver>() != null)
        {
            return;
        }
        Destroy(gameObject);
    }
}
