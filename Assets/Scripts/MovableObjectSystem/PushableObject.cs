using UnityEngine;

public class PushableObject : MovableObject
{
    public float floorHeight = 0f;
    public SideTrigger[] sideTriggers;
    public GameObject player;

    private void Awake()
    {
        foreach (var t in sideTriggers)
            t.player = player;
    }

    private void Update()
    {
        if (Input.GetButtonDown("Interact"))
            TryPush();
    }

    private void TryPush()
    {
        if (player == null) return;

        SideTrigger trigger = null;
        foreach (var t in sideTriggers)
        {
            if (t.playerInRange)
            {
                trigger = t;
                break;
            }
        }

        if (trigger == null) return;

        CardinalDirection oppositeSide = GetOpposite(trigger.side);
        SideTrigger oppositeTrigger = GetTrigger(oppositeSide);

        if (oppositeTrigger != null && oppositeTrigger.blocked)
            return;

        Vector3Int pushDir = GetPushDirection(trigger.side);

        SnapToGrid();
        Vector3Int currentCell = grid.WorldToCell(transform.position);
        Vector3 targetPos = grid.GetCellCenterWorld(currentCell + pushDir);

        transform.position = targetPos;

        Vector3 pos = transform.position;
        pos.y = floorHeight;
        transform.position = pos;
    }

    private SideTrigger GetTrigger(CardinalDirection side)
    {
        foreach (var t in sideTriggers)
            if (t.side == side) return t;
        return null;
    }

    private CardinalDirection GetOpposite(CardinalDirection side)
    {
        switch (side)
        {
            case CardinalDirection.North: return CardinalDirection.South;
            case CardinalDirection.South: return CardinalDirection.North;
            case CardinalDirection.East:  return CardinalDirection.West;
            case CardinalDirection.West:  return CardinalDirection.East;
            default: return side;
        }
    }

    private Vector3Int GetPushDirection(CardinalDirection side)
    {
        switch (side)
        {
            case CardinalDirection.North: return GridDirection.North;
            case CardinalDirection.South: return GridDirection.South;
            case CardinalDirection.East:  return GridDirection.East;
            case CardinalDirection.West:  return GridDirection.West;
            default: return Vector3Int.zero;
        }
    }
}
