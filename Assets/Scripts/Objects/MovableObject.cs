using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class MovableObject : MonoBehaviour
{
    public Grid grid;
    public Material ghostMaterial;
    public Material outlineMaterial;

    private GameObject ghostInstance;
    private Material[] originalMaterials;

    private void Start()
    {
        SnapToGrid();
    }

    public void SnapToGrid()
    {
        if (!grid) return;
        Vector3Int cellPos = grid.WorldToCell(transform.position);
        transform.position = grid.GetCellCenterWorld(cellPos);
    }

    public void ShowOutline()
    {
        MeshRenderer mr = GetComponent<MeshRenderer>();
        if (mr == null || outlineMaterial == null) return;

        originalMaterials = mr.sharedMaterials;

        Material[] mats = new Material[mr.sharedMaterials.Length];
        for (int i = 0; i < mats.Length; i++)
            mats[i] = outlineMaterial;
        mr.materials = mats;
    }

    public void HideOutline()
    {
        if (originalMaterials == null) return;
        MeshRenderer mr = GetComponent<MeshRenderer>();
        if (mr == null) return;

        mr.materials = originalMaterials;
        originalMaterials = null;
    }

    public void ShowGhost(float floorHeight)
    {
        if (ghostInstance != null) return;
        if (!ghostMaterial) return;

        MeshFilter mf = GetComponent<MeshFilter>();
        if (mf == null) return;

        ghostInstance = new GameObject(name + "_Ghost");
        ghostInstance.transform.SetParent(null);

        MeshFilter ghostMF = ghostInstance.AddComponent<MeshFilter>();
        ghostMF.sharedMesh = mf.sharedMesh;

        MeshRenderer ghostMR = ghostInstance.AddComponent<MeshRenderer>();
        ghostMR.sharedMaterial = ghostMaterial;

        Vector3 pos = transform.position;
        pos.y = floorHeight;
        ghostInstance.transform.position = pos;
        ghostInstance.transform.rotation = transform.rotation;
        ghostInstance.transform.localScale = transform.localScale;
    }

    public void UpdateGhostPosition(float floorHeight)
    {
        if (!ghostInstance || grid == null) return;

        Vector3Int cellPos = grid.WorldToCell(transform.position);
        Vector3 cellCenter = grid.GetCellCenterWorld(cellPos);
        cellCenter.y = floorHeight;

        ghostInstance.transform.position = cellCenter;
        ghostInstance.transform.rotation = transform.rotation;
        ghostInstance.transform.localScale = transform.localScale;
    }

    public void DestroyGhost()
    {
        if (ghostInstance)
        {
            Destroy(ghostInstance);
            ghostInstance = null;
        }
    }
}
