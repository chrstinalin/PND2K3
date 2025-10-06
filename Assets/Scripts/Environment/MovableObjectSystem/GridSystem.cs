using UnityEngine;

public class GridSystem : MonoBehaviour
{
    [SerializeField] private Grid grid;

    public Vector3 SnapToGrid(Vector3 position)
    {
        Vector3Int cell = grid.WorldToCell(position);
        return grid.GetCellCenterWorld(cell);
    }

    public Vector3 GetNextCellPosition(Vector3 currentPos, Vector3 direction)
    {
        Vector3Int cell = grid.WorldToCell(currentPos);
        Vector3Int move = Vector3Int.RoundToInt(direction);
        Vector3Int targetCell = cell + move;
        Vector3 targetPos = grid.GetCellCenterWorld(targetCell);

        return targetPos;
    }
}
