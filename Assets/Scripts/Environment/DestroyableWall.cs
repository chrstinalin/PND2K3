using UnityEngine;

public class DestroyableWall : MonoBehaviour
{
    [SerializeField] private GameObject intactModel;
    [SerializeField] private GameObject brokenModel;

    private Health health;
    private MeshRenderer meshRenderer;
    
    void Awake()
    {
        health = GetComponent<Health>();
        meshRenderer = GetComponent<MeshRenderer>();
        
        if (health == null)
        {
            Debug.LogError("DestroyableWall requires a Health component!");
            return;
        }
        
        // Make sure the intact model is active at start
        if (intactModel != null && brokenModel != null)
        {
            intactModel.SetActive(true);
            brokenModel.SetActive(false);
        }
        
        // Subscribe to the death event
        health.onDeath.AddListener(OnWallDestroyed);
    }
    
    void OnWallDestroyed()
    {
        Debug.Log($"{gameObject.name} has been destroyed!");
        
        if (SFXManager.Instance != null)
        {
            SFXManager.Instance.PlayWallBreak();
        }
        
        if (intactModel != null && brokenModel != null)
        {
            // Swap models
            intactModel.SetActive(false);
            brokenModel.SetActive(true);
        }
        
        // Disables the collider to allow player to pass through.
        var collider = GetComponent<Collider>();
        if (collider != null)
        {
            collider.enabled = false;
        }
        
        var damageReceiver = GetComponent<DamageReceiver>();
        if (damageReceiver != null)
        {
            damageReceiver.enabled = false;
        }
    }
}