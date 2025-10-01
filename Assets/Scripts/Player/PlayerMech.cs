using System.IO;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class PlayerMech : MonoBehaviour
{
    private Health _health;
    [SerializeField] MovementManager movementManager;

    private float _DamageLerp = 5f;
    private float _ChipLerp = 50f;
    private float lerpTimer;

    public Image HealthFront;
    public Image HealthBack;
    private Rigidbody rb;
    public float stepRate = 1f;
	public float stepCoolDown;
	public AudioSource mechFootstep;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        GameObject _HealthFront = GameObject.FindGameObjectWithTag("MechHealthFront");
        GameObject _HealthBack = GameObject.FindGameObjectWithTag("MechHealthBack");
        if (_HealthFront) HealthFront = _HealthFront.GetComponent<Image>();
        if (_HealthBack) HealthBack = _HealthBack.GetComponent<Image>();

        _health = GetComponent<Health>();
        _health.onDeath.AddListener(OnDeath);
    }

    void Update()
    {
        if (!HealthFront || !HealthBack) return;

        float fillA = HealthFront.fillAmount;
        float fillB = HealthBack.fillAmount;
        float hFraction = (float)_health.GetCurrHealth() / _health.GetMaxHealth();
        if (fillB > hFraction)
        {
            HealthBack.color = Color.red;
            HealthFront.fillAmount = hFraction;

            lerpTimer += Time.deltaTime;

            HealthFront.fillAmount = Mathf.Lerp(fillA, hFraction, lerpTimer / _DamageLerp);
            HealthBack.fillAmount = Mathf.Lerp(fillB, hFraction, lerpTimer / _ChipLerp);
        }
        else if (fillA < hFraction)
        {
            HealthBack.color = Color.green;
            HealthBack.fillAmount = hFraction;
            lerpTimer += Time.deltaTime;

            HealthFront.fillAmount = Mathf.Lerp(fillA, hFraction, lerpTimer / _DamageLerp);
            HealthBack.fillAmount = Mathf.Lerp(fillB, hFraction, lerpTimer / _ChipLerp);
        }
        
        stepCoolDown -= Time.deltaTime;
        if (rb.linearVelocity.magnitude > 0.1f && stepCoolDown <= 0f)
        {
            mechFootstep.PlayOneShot(mechFootstep.clip);
            stepCoolDown = stepRate;
        }
    }
    public void OnHealthChanged(int damage) => lerpTimer = 0;
    public void OnDeath()
    {
        transform.position = new Vector3(0, 1, 0);
        _health.Heal(_health.GetMaxHealth());
        resetHealthBar();
    }
    private void resetHealthBar()
    {
        if (!HealthFront || !HealthBack) return;
        HealthFront.fillAmount = 1f;
        HealthBack.fillAmount = 1f;
    }

}