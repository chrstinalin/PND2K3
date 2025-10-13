using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;

public class NavMeshEdgeVisualizer : MonoBehaviour
{
    private GameObject edgeMeshObject;
    private GameObject fillMeshObject;
    public Color meshColour = Color.green;

    // Draws the navmesh boundary and create a separate filled mesh.
    public void ShowFilledArea()
    {
        ClearFilledArea();

        var triangulation = NavMesh.CalculateTriangulation();
        var verts = triangulation.vertices;
        var tris = triangulation.indices;

        if (verts == null || verts.Length == 0 || tris == null || tris.Length == 0)
        {
            return;
        }

        // 1. Build boundary line mesh using canonical vertices to remove duplicate positions
        const float posMultiplier = 1000f;
        var posToCanon = new Dictionary<string, int>(verts.Length);
        var canonVerts = new List<Vector3>(verts.Length);
        var indexMap = new int[verts.Length];

        for (int i = 0; i < verts.Length; i++)
        {
            Vector3 v = verts[i];
            int ix = Mathf.RoundToInt(v.x * posMultiplier);
            int iy = Mathf.RoundToInt(v.y * posMultiplier);
            int iz = Mathf.RoundToInt(v.z * posMultiplier);
            string key = ix + "|" + iy + "|" + iz;

            if (!posToCanon.TryGetValue(key, out int canonIndex))
            {
                canonIndex = canonVerts.Count;
                posToCanon[key] = canonIndex;
                canonVerts.Add(v);
            }

            indexMap[i] = canonIndex;
        }

        var edgeCount = new Dictionary<long, int>();
        int triCount = tris.Length / 3;
        for (int t = 0; t < triCount; t++)
        {
            int i0 = indexMap[tris[t * 3 + 0]];
            int i1 = indexMap[tris[t * 3 + 1]];
            int i2 = indexMap[tris[t * 3 + 2]];

            AddEdge(edgeCount, i0, i1);
            AddEdge(edgeCount, i1, i2);
            AddEdge(edgeCount, i2, i0);
        }

        var lineIndices = new List<int>(edgeCount.Count * 2);
        foreach (var kv in edgeCount)
        {
            if (kv.Value == 1)
            {
                long key = kv.Key;
                int a = (int)(key >> 32);
                int b = (int)(key & 0xFFFFFFFF);
                lineIndices.Add(a);
                lineIndices.Add(b);
            }
        }

        if (lineIndices.Count > 0)
        {
            var lineMesh = new Mesh();
            lineMesh.name = "NavMeshEdges_Mesh";
            lineMesh.vertices = canonVerts.ToArray();
            lineMesh.SetIndices(lineIndices.ToArray(), MeshTopology.Lines, 0);

            edgeMeshObject = new GameObject("NavMeshEdges");
            var edgeFilter = edgeMeshObject.AddComponent<MeshFilter>();
            var edgeRenderer = edgeMeshObject.AddComponent<MeshRenderer>();

            var edgeMaterial = new Material(Shader.Find("Sprites/Default"));
            edgeRenderer.material = edgeMaterial;
            edgeMaterial.color = meshColour;
            edgeFilter.mesh = lineMesh;
        }

        // 2. Build filled mesh using the raw triangulation
        var fillMesh = new Mesh();
        fillMesh.name = "NavMeshFill_Mesh";
        fillMesh.vertices = verts;
        fillMesh.triangles = tris;

        fillMeshObject = new GameObject("NavMeshFill");
        var fillFilter = fillMeshObject.AddComponent<MeshFilter>();
        var fillRenderer = fillMeshObject.AddComponent<MeshRenderer>();

        var fillColor = new Color(meshColour.r, meshColour.g, meshColour.b, 0.1f);
        var fillMaterial = new Material(Shader.Find("Sprites/Default"));
        fillRenderer.material = fillMaterial;
        fillMaterial.color = fillColor;
        fillFilter.mesh = fillMesh;
    }

    public void ClearFilledArea()
    {
        if (edgeMeshObject != null)
        {
            Destroy(edgeMeshObject);
            edgeMeshObject = null;
        }

        if (fillMeshObject != null)
        {
            Destroy(fillMeshObject);
            fillMeshObject = null;
        }
    }

    private static void AddEdge(Dictionary<long, int> map, int a, int b)
    {
        int min = a < b ? a : b;
        int max = a < b ? b : a;
        long key = ((long)min << 32) | (uint)max;
        if (map.TryGetValue(key, out int count))
            map[key] = count + 1;
        else
            map[key] = 1;
    }
}