using UnityEngine;
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

    private TargettedBulletEmitter bulletEmitter;

    void Awake()
    {
        bulletEmitter = GetComponent<TargettedBulletEmitter>();
    }

    void Start()
    {
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

    public void OnHealthChanged(int damage)
    {
        if (damage < 0)
        {
            Debug.Log("$Healed {-damage} health!");
        }
        else
        {
            if (movementManager != null && !movementManager.IsMouseActive)
            {
                Debug.Log("Mech took damage! Ejecting mouse...");
                movementManager.ToggleMouse(true);
            }
        }
    }

    public void OnDeath()
    {
        Debug.Log("Mech Died. Respawning...");
        transform.position = new Vector3(0, 1, 0);
        _health.Heal(_health.GetMaxHealth());
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

    private void Update()
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
    }
    
    public bool isAttack()
    {
        return isAttacking;
    }
}