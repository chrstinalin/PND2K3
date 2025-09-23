using UnityEngine;

public class LiftableObject : MovableObject
{
    public Material ghostMaterial;

    private GameObject ghostInstance;

    public void ShowGhost(float floorHeight)
    {
        if (ghostInstance || !ghostMaterial) return;

        var mf = GetComponent<MeshFilter>();
        if (!mf) return;

        ghostInstance = new GameObject(name + "_Ghost");
        ghostInstance.transform.SetParent(null);

        var ghostMF = ghostInstance.AddComponent<MeshFilter>();
        ghostMF.sharedMesh = mf.sharedMesh;

        var ghostMR = ghostInstance.AddComponent<MeshRenderer>();
        ghostMR.sharedMaterial = ghostMaterial;

        Vector3 pos = transform.position;
        pos.y = floorHeight;
        ghostInstance.transform.position = pos;
        ghostInstance.transform.rotation = transform.rotation;
        ghostInstance.transform.localScale = transform.localScale;
    }

    public void UpdateGhostPosition(float floorHeight)
    {
        if (!ghostInstance || !grid) return;

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
