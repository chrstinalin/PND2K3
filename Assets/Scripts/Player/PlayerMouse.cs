using UnityEngine;
using UnityEngine.UI;

public class PlayerMouse : MonoBehaviour
{
    private Health _health;
    private Image[] HealthPoints;
    public float stepRate = 0.2f;
	public float stepCoolDown;
	public AudioSource mouseFootstep;
    private Rigidbody rb;

    void Start()
    {
        GameObject HealthPointContainer = GameObject.FindGameObjectWithTag("MouseHealthPointContainer");
        if (HealthPointContainer) HealthPoints = HealthPointContainer.GetComponentsInChildren<Image>();
        _health = GetComponent<Health>();
        _health.onHealthChanged.AddListener(OnHealthChanged);
        _health.onDeath.AddListener(OnDeath);

        rb = GetComponent<Rigidbody>();
    }

    public void OnHealthChanged(int damage)
    {
        for (int i = 0; i < _health.GetMaxHealth(); i++)
        {
            HealthPoints[i].enabled = i <= _health.GetCurrHealth() - 1;
        }
    }

    public void OnDeath()
    {
        Debug.Log("Player Died. Respawning...");
        transform.position = new Vector3(0, 1, 0);
        _health.Heal(_health.GetMaxHealth());
    }

    public void Update()
    {
        stepCoolDown -= Time.deltaTime;

        if (rb.linearVelocity.magnitude > 0.1f && stepCoolDown < 0f)
        {
            mouseFootstep.PlayOneShot(mouseFootstep.clip);
            stepCoolDown = stepRate;
        }
    }
}