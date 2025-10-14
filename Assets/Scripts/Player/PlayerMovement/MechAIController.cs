using System;
using System.Collections;
using Unity.VisualScripting;
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
        float distance = Vector3.Distance(transform.position, targetPos);
        Vector3 destination = transform.position;
        if (distance > Config.MIN_AI_DISTANCE) CurrentState = AIState.Walk;
        else CurrentState = AIState.Idle;

        switch (CurrentState)
            {
                case AIState.Idle:
                    Agent.isStopped = true;
                    break;
                case AIState.Walk:
                    Agent.isStopped = false;

                    if (distance > Config.MIN_AI_DISTANCE)
                        destination = targetPos - (targetPos - transform.position).normalized * Config.MIN_AI_DISTANCE;
                    else
                    {
                        Vector3 lookDirection = (targetPos - transform.position).normalized;
                        lookDirection.y = 0;
                        if (lookDirection != Vector3.zero) transform.rotation = Quaternion.LookRotation(lookDirection);
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