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

        Vector3 direction = (target.position - transform.position).normalized;

        Quaternion rotation = Quaternion.LookRotation(direction);

        Instantiate(BulletSource, transform.position, rotation);
        Debug.Log("firing targetted bullet");

        SFXManager.Instance.PlayBulletFire();
    }
    
    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
    }
}
