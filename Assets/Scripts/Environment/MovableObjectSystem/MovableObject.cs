using UnityEngine;

public class MovableObject : MonoBehaviour
{
    [SerializeField] protected Grid grid;

    public virtual void SnapToGrid()
    {
        if (!grid) return;
        Vector3Int cellPos = grid.WorldToCell(transform.position);
        transform.position = grid.GetCellCenterWorld(cellPos);
    }
}
