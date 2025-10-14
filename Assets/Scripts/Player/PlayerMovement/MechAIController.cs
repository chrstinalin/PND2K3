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

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        Agent = GetComponent<NavMeshAgent>();
        CurrentState = AIState.Idle;
        PlayerMarker.Instance.OnTargetChanged += SetTarget;
    }

    void Update()
    {
        if (Target == null) return;

        Debug.Log("Current Target: " + Target);

        Vector3 targetPos = Target.transform.position;
        Vector3 directionToTarget = targetPos - transform.position;
        directionToTarget.y = 0;
        float distance = directionToTarget.magnitude;

        bool isLockOnSelectable = Target.GetComponent<LockOnSelectable>() != null;

        if (directionToTarget != Vector3.zero)
            transform.rotation = Quaternion.LookRotation(directionToTarget);

        if (isLockOnSelectable)
        {
            CurrentState = (distance > Config.MIN_AI_DISTANCE) ? AIState.Walk : AIState.Idle;
        }
        else
        {
            CurrentState = AIState.Walk;
        }

        switch (CurrentState)
        {
            case AIState.Idle:
                Agent.isStopped = true;
                Target = null;
                break;

            case AIState.Walk:
                Agent.isStopped = false;

                Vector3 destination = targetPos;
                if (isLockOnSelectable && distance > Config.MIN_AI_DISTANCE)
                {
                    destination = targetPos - directionToTarget.normalized * Config.MIN_AI_DISTANCE;
                }

                Agent.SetDestination(destination);
                break;

            case AIState.Attack:
                // Attack logic
                break;
        }
    }


    public void SetTarget(GameObject NewTarget)
    {
        Target = NewTarget;

        if (Target != null)
        {
            Agent.SetDestination(Target.transform.position);
            CurrentState = AIState.Walk;
        }
        else
        {
            CurrentState = AIState.Idle;
        }
    }

    public bool isAttack()
    {
        return AttackActive;
    }
}
