using UnityEngine;
using UnityEngine.UI;

public class PlayerMouse : MonoBehaviour
{
    private Health _health;
    private Image[] HealthPoints;
    public float stepRate = 0.2f;
	public float stepCoolDown;
	public AudioSource footStep;

    void Start()
    {
        GameObject HealthPointContainer = GameObject.FindGameObjectWithTag("MouseHealthPointContainer");
        if (HealthPointContainer) HealthPoints = HealthPointContainer.GetComponentsInChildren<Image>();
        _health = GetComponent<Health>();
        _health.onHealthChanged.AddListener(OnHealthChanged);
        _health.onDeath.AddListener(OnDeath);
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
		if ((Input.GetAxis("Horizontal") != 0f || Input.GetAxis("Vertical") != 0f) && stepCoolDown < 0f){
            Debug.Log("Playing footsteps");
			footStep.PlayOneShot (footStep.clip);
			stepCoolDown = stepRate;
		}
    }
}