using UnityEngine;

public class TargettedBulletEmitter : MonoBehaviour, IBulletEmitter
{
    [SerializeField] private GameObject bulletSource;
    public GameObject BulletSource
    {
        get => bulletSource;
        set => bulletSource = value;
    }
    public IOffense Owner { get; set; }

    public int firingRate = 1;
    private float _timer;
    public int numBullets = 1;

    [SerializeField] private Transform target;

    void Start()
    {
        Owner = GetComponent<IOffense>() ?? GetComponentInParent<IOffense>();
    }

    void Update()
    {
        _timer += Time.deltaTime;
        if (_timer >= firingRate && Owner != null && Owner.isAttack())
        {
            Fire();
            _timer = 0;
        }
    }

    public virtual void Fire()
    {
        if (target == null)
        {
            Debug.LogWarning("No target assigned for bullet emitter!");
            return;
        }

        // 1. Calculate direction
        Vector3 direction = (target.position - transform.position).normalized;

        // 2. Get rotation that looks in that direction
        Quaternion rotation = Quaternion.LookRotation(direction);

        // 3. Spawn bullet
        Instantiate(BulletSource, transform.position, rotation);
        Debug.Log("firing targetted bullet");

        // (Optional) if your bullet script moves via Rigidbody or Update,
        // make sure it uses transform.forward to move.

        SFXManager.Instance.PlayBulletFire();
    }
    
    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
    }
}
