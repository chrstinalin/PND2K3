using UnityEngine;

public abstract class MovableObject : MonoBehaviour
{
    public Grid grid;
    public Material outlineMaterial;

    protected Material[] originalMaterials;

    protected virtual void Start()
    {
        SnapToGrid();
    }

    public void SnapToGrid()
    {
        if (!grid) return;
        Vector3Int cellPos = grid.WorldToCell(transform.position);
        transform.position = grid.GetCellCenterWorld(cellPos);
    }

    public virtual void ShowOutline()
    {
        var mr = GetComponent<MeshRenderer>();
        if (!mr || !outlineMaterial) return;

        originalMaterials = mr.sharedMaterials;
        var mats = new Material[originalMaterials.Length];
        for (int i = 0; i < mats.Length; i++) mats[i] = outlineMaterial;
        mr.materials = mats;
    }

    public virtual void HideOutline()
    {
        if (originalMaterials == null) return;
        var mr = GetComponent<MeshRenderer>();
        if (!mr) return;

        mr.materials = originalMaterials;
        originalMaterials = null;
    }
}
