using System;
using UnityEngine;
using UnityEngine.AI;

public class MechAIController : MonoBehaviour, IOffense
{
    [NonSerialized] public static MechAIController Instance;

    private NavMeshAgent Agent;
    public GameObject Target;
    private AIState CurrentState;

    private bool AttackActive = false;
    [SerializeField] private float attackRange = 10f;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        Agent = GetComponent<NavMeshAgent>();
        Agent.updateRotation = true;
        CurrentState = AIState.Idle;
        PlayerMarker.Instance.OnTargetChanged += SetTarget;
    }

    void Update()
    {
    if (Agent == null) return;
    
    if (Target == null)
    {
        AttackActive = false;
        if (CurrentState == AIState.Attack || CurrentState == AIState.Walk)
        {
            Agent.isStopped = true;
            CurrentState = AIState.Idle;
        }
        return;
    }

    Vector3 targetPos = Target.transform.position;
    Vector3 directionToTarget = targetPos - transform.position;
    directionToTarget.y = 0;
    float distance = directionToTarget.magnitude;

    bool isPlayerMouse = Target == PlayerMouse.Instance?.gameObject;
    bool isLockOnSelectable = Target.GetComponent<LockOnSelectable>() != null;
    bool isEnemy = Target.GetComponent<DamageReceiver>() != null && !isPlayerMouse;

    if (isEnemy)
    {
        if (distance <= attackRange)
        {
            CurrentState = AIState.Attack;
            Agent.isStopped = true;
            AttackActive = true;

            if (directionToTarget != Vector3.zero)
            {
                Quaternion targetRotation = Quaternion.LookRotation(directionToTarget);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 5f);
            }
        }
        else
        {
            CurrentState = AIState.Walk;
            Agent.isStopped = false;
            AttackActive = false;
            Agent.SetDestination(targetPos);
        }
    }
    else if (isLockOnSelectable)
    {
        CurrentState = (distance > Config.MIN_AI_DISTANCE) ? AIState.Walk : AIState.Idle;
        AttackActive = false;
        
        if (CurrentState == AIState.Idle)
        {
            Agent.isStopped = true;
        }
        else
        {
            Agent.isStopped = false;
            Vector3 destination = targetPos - directionToTarget.normalized * Config.MIN_AI_DISTANCE;
            Agent.SetDestination(destination);
        }
    }
    else
    {
        CurrentState = AIState.Walk;
        AttackActive = false;
        Agent.isStopped = false;
        Agent.SetDestination(targetPos);
    }
}

    public void SetTarget(GameObject NewTarget)
    {    
    if (Target != null)
    {
    Health oldHealth = Target.GetComponent<Health>();
    if (oldHealth != null)
    {
        oldHealth.onDeath.RemoveListener(HandleTargetDeath);
    }
    }
    bool targetChanged = Target != NewTarget;
    Target = NewTarget;

    if (Target != null)
    {
    if (targetChanged)
    {
        Health newHealth = Target.GetComponent<Health>();
        if (newHealth != null)
        {
            newHealth.onDeath.AddListener(HandleTargetDeath);
        }
    }

    if (targetChanged)
    {
        AttackActive = false;
        CurrentState = AIState.Walk;
    }

    if (Agent != null)
    {
        Agent.isStopped = false;
        Agent.SetDestination(Target.transform.position);
    }
    }
    else
    {
    CurrentState = AIState.Idle;
    AttackActive = false;

    if (Agent != null)
    {
        Agent.isStopped = true;
        Agent.ResetPath();
    }
    }
    }

    private void HandleTargetDeath()
    {
        if (Target != null)
        {
            Health deadHealth = Target.GetComponent<Health>();
            if (deadHealth != null)
            {
                deadHealth.onDeath.RemoveListener(HandleTargetDeath);
            }
        }
        Target = null;
        AttackActive = false;
        CurrentState = AIState.Idle;

        if (Agent != null)
        {
            Agent.isStopped = true;
            Agent.ResetPath();
        }
    }
    
    public bool isAttack()
    {
        return AttackActive;
    }

    public GameObject GetCurrentTarget()
    {
        return Target;
    }

    void OnDestroy()
    {
        if (Target != null)
        {
            Health health = Target.GetComponent<Health>();
            if (health != null) health.onDeath.RemoveListener(HandleTargetDeath);
        }
        
        if (PlayerMarker.Instance != null) PlayerMarker.Instance.OnTargetChanged -= SetTarget;
    }
}