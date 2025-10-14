using System.Collections;
using UnityEngine;

public class TurretWeapon : MonoBehaviour
{
    public AudioClip chargeSFX;
    public AudioClip bulletSFX;
    [SerializeField] private EnemyVisionManager visionManager;
    [SerializeField] private LineRenderer shotLineRenderer;
    public IOffense Owner { get; set; }

    [Header("Configuration")]
    private float _timer;
    private bool isCharging = false;
    private float damage = 1f;
    [SerializeField] private float fireCooldown = 3f;
    [SerializeField] private float chargeTime = 1f;
    [SerializeField] private float shotDisplayTime = 0.25f;

    void Start()
    {
        Owner = GetComponent<IOffense>() ?? GetComponentInParent<IOffense>();
        if (visionManager == null) visionManager = GetComponent<EnemyVisionManager>() ?? GetComponentInParent<EnemyVisionManager>();
        if (shotLineRenderer != null) shotLineRenderer.enabled = false;
    }

    void Update()
    {
        _timer += Time.deltaTime;
        if (_timer >= fireCooldown && Owner != null && Owner.isAttack() && !isCharging)
        {
            StartCoroutine(ChargeAndFire());
            _timer = 0;
        }
    }

    private IEnumerator ChargeAndFire()
    {
        isCharging = true;
        if (chargeSFX != null) AudioManager.Instance.PlaySFX(chargeSFX);
        yield return new WaitForSeconds(chargeTime);

        Fire();

        isCharging = false;
    }

    public virtual void Fire()
    {
        if (visionManager == null) return;

        GameObject target = null;

        if (visionManager.MouseIsSpotted) target = visionManager.Mouse;
        else if (visionManager.MechIsSpotted) target = visionManager.Mech;
        
        if (target == null) return;

        Vector3 shootFrom = transform.position;
        Vector3 fireDirection = (target.transform.position - shootFrom).normalized;
        RaycastHit hit;
        if (Physics.Raycast(shootFrom, fireDirection, out hit, Mathf.Infinity))
        {
            if (hit.transform == target.transform)
            {
                AudioManager.Instance.PlaySFX(bulletSFX);
                DamageReceiver damageReceiver = hit.transform.GetComponent<DamageReceiver>();
                if (damageReceiver != null)
                {
                    damageReceiver.ReceiveDamage((int)damage);
                }
                StartCoroutine(ShowShot(shootFrom, hit.point));
            }
        }
    }

    private IEnumerator ShowShot(Vector3 start, Vector3 end)
    {
        if (shotLineRenderer != null)
        {
            shotLineRenderer.enabled = true;
            shotLineRenderer.SetPosition(0, start);
            shotLineRenderer.SetPosition(1, end);

            yield return new WaitForSeconds(shotDisplayTime);
            shotLineRenderer.enabled = false;
        }
    }
}