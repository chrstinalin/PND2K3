using System.IO;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Rendering;
using UnityEngine.UI;
using System.Collections.Generic;

public class PlayerMech : MonoBehaviour, IOffense
{
    private Health _health;
    [SerializeField] MovementManager movementManager;

    private List<Renderer> renderers;
    private GameObject[] enemies;
    private int currEnemyIndex;
    private bool isTarget;
    private Dictionary<int, Renderer> enemyIdToRenderer;
    private List<Outline> outlines;
    private bool isAttacking;
    private float _DamageLerp = 5f;
    private float _ChipLerp = 50f;
    private float lerpTimer;

    public Image HealthFront;
    public Image HealthBack;

    private TargettedBulletEmitter bulletEmitter;

    void Awake()
    {
        bulletEmitter = GetComponent<TargettedBulletEmitter>();
    }

    void Start()
    {
        GameObject _HealthFront = GameObject.FindGameObjectWithTag("MechHealthFront");
        GameObject _HealthBack = GameObject.FindGameObjectWithTag("MechHealthBack");
        if (_HealthFront) HealthFront = _HealthFront.GetComponent<Image>();
        if (_HealthBack) HealthBack = _HealthBack.GetComponent<Image>();

        _health = GetComponent<Health>();
        _health.onDeath.AddListener(OnDeath);

        enemies = GameObject.FindGameObjectsWithTag("Enemy");
        renderers = new List<Renderer>();
        enemyIdToRenderer = new Dictionary<int, Renderer>();
        outlines = new List<Outline>();

        Debug.Log("Enemies found: " + enemies.Length);

        foreach (var enemy in enemies)
        {
            Debug.Log("Enemies found: " + enemies.Length);
            var renderer = enemy.GetComponentInChildren<Renderer>();

            if (renderer != null)
            {
                enemyIdToRenderer.Add(enemy.GetInstanceID(), renderer);
                renderers.Add(renderer);
                var outline = enemy.AddComponent<Outline>();
                outline.OutlineMode = Outline.Mode.OutlineHidden;
                outlines.Add(outline);
            }
        }
    }

    void Update()
    {   
        if (!isTarget && Input.GetButtonDown("SelectTarget"))
        {
            isTarget = true;
            Debug.Log("selecting enemy");
            OnHighlightEnemy();
        }
        else if (isTarget && Input.GetButtonDown("SelectTarget"))
        {
            Debug.Log("resetting");
            resetSelectEnemy();
            isTarget = false;
            isAttacking = false;
        }

        if (isTarget && Input.GetButtonDown("TabEnemy"))
        {
            var prevIndex = currEnemyIndex;

            if (currEnemyIndex < enemies.Length - 1)
            {
                currEnemyIndex += 1;
            }
            else
            {
                currEnemyIndex = 0;
            }
            OnSelectEnemy(prevIndex, currEnemyIndex);
        }

        if (isTarget && Input.GetButtonDown("AttackEnemy"))
        {
            bulletEmitter.SetTarget(enemies[currEnemyIndex].transform);
            isAttacking = true;
        }

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

    public void OnHighlightEnemy()
    {
        foreach (var outline in outlines)
        {
            outline.OutlineMode = Outline.Mode.OutlineAll;
            outline.OutlineColor = Color.purple;
            outline.OutlineWidth = 5f;
        }
    }

    public void OnSelectEnemy(int prevIndex, int index)
    {
        var outline = outlines[index];
        outline.OutlineMode = Outline.Mode.OutlineAll;
        outline.OutlineColor = Color.orange;
        outline.OutlineWidth = 5f;

        // reset prev outline
        outline = outlines[prevIndex];
        outline.OutlineMode = Outline.Mode.OutlineAll;
        outline.OutlineColor = Color.purple;
        outline.OutlineWidth = 5f;
    }

    public void resetSelectEnemy()
    {
        foreach (var outline in outlines)
        {
            outline.OutlineMode = Outline.Mode.OutlineHidden;
            outline.OutlineColor = Color.purple;
            outline.OutlineWidth = 5f;
        }
    }
    
    public bool isAttack()
    {
        return isAttacking;
    }
}