using System.Collections;
using UnityEngine;

public class MechWeapon : MonoBehaviour
{
    public AudioClip chargeSFX;
    public AudioClip bulletSFX;
    [SerializeField] private LineRenderer shotLineRenderer;
    private MechAIController aiController;
    public IOffense Owner { get; set; }

    [Header("Configuration")]
    private float _timer;
    private bool isCharging = false;
    private float damage = 1f;
    [SerializeField] private float fireCooldown = 2f;
    [SerializeField] private float chargeTime = 0.5f;
    [SerializeField] private float shotDisplayTime = 0.25f;

    void Start()
    {
        aiController = MechAIController.Instance;
        Owner = aiController;
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
        if (aiController == null) return;

        GameObject target = aiController.GetCurrentTarget();
        DamageReceiver targetDamageReceiver = target.GetComponent<DamageReceiver>();
        if (target == null) return;
        if (!target.activeInHierarchy) return;
        if (targetDamageReceiver == null) return;

        Vector3 shootFrom = transform.position;
        Vector3 fireDirection = (target.transform.position - shootFrom).normalized;
        RaycastHit hit;
        
        if (Physics.Raycast(shootFrom, fireDirection, out hit, Mathf.Infinity))
        {
            if (hit.transform == target.transform)
            {
                if (bulletSFX != null) AudioManager.Instance.PlaySFX(bulletSFX);
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