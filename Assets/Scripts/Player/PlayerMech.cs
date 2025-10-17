using System;
using System.Collections;
using UnityEngine;

public class PlayerMech : MonoBehaviour
{
    public static PlayerMech Instance;

    [NonSerialized] public Health Health;
    [NonSerialized] public DamageReceiver DamageReceiver;
    [NonSerialized] public MechaInventoryManager InventoryManager;
    [NonSerialized] public MechAIController AIController;

    private bool isInvulnerable = false;
    private float iFrameDuration = 1.0f;
    private float iFrameTimer = 0f;

    void Awake()
    {
        Instance = this;
        InventoryManager = gameObject.AddComponent<MechaInventoryManager>();
        DamageReceiver = gameObject.AddComponent<DamageReceiver>();
        AIController = GetComponent<MechAIController>();
        Health = gameObject.AddComponent<Health>();
    }

    void Start()
    {
        if (AIController == null)
        {
            AIController = GetComponent<MechAIController>();
        }
        
        if (Health != null)
        {
            Health.onDeath.AddListener(OnDeath);
        }
        
        if (DamageReceiver != null)
        {
            DamageReceiver.onTakeDamage.AddListener(TakeDamage);
            DamageReceiver.onTakeDamageWithSource.AddListener(TakeDamageFromSource);
        }
    }

    void Update()
    {
        if (isInvulnerable)
        {
            iFrameTimer -= Time.deltaTime;
            if (iFrameTimer <= 0f)
            {
                isInvulnerable = false;
            }
        }
    }

    public void TakeDamage(int damage)
    {
        if (isInvulnerable || Health == null) return;
        Health.TakeDamage(damage);
        isInvulnerable = true;
        iFrameTimer = iFrameDuration;
        StartCoroutine(FlashSprite());
    }

    public void TakeDamageFromSource(int damage, GameObject source)
    {
        if (AIController == null)
        {
            AIController = GetComponent<MechAIController>();
            Debug.Log("Tried to get AIController: " + (AIController != null));
        }
    
        if (AIController != null && source != null)
        {
            AIController.OnAttackedBy(source);
        }
        
        if (isInvulnerable || Health == null) 
        {
            return;
        }
    
        Health.TakeDamage(damage);
        isInvulnerable = true;
        iFrameTimer = iFrameDuration;
        StartCoroutine(FlashSprite());
    }

    public void OnDeath()
    {
        transform.position = new Vector3(0, 1, 0);
        if (Health != null)
        {
            Health.Heal(Health.GetMaxHealth());
        }
    }

    private IEnumerator FlashSprite()
    {
        Renderer[] renderers = GetComponentsInChildren<Renderer>();
        Color[] originalColors = new Color[renderers.Length];
        for (int i = 0; i < renderers.Length; i++)
        {
            originalColors[i] = renderers[i].material.color;
        }

        float flashInterval = 0.2f;
        float elapsed = 0f;
        Color flashColour = Color.red;
        
        while (elapsed < iFrameDuration)
        {
            bool useFlashColour = ((int)(elapsed / flashInterval)) % 2 == 0;
            for (int i = 0; i < renderers.Length; i++)
            {
                renderers[i].material.color = useFlashColour ? flashColour : originalColors[i];
            }
            yield return new WaitForSeconds(flashInterval);
            elapsed += flashInterval;
        }
        
        for (int i = 0; i < renderers.Length; i++)
        {
            renderers[i].material.color = originalColors[i];
        }
    }
}