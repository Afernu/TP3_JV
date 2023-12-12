using UnityEngine;

public class IcosahedronGenerator : MonoBehaviour
{
    void Start()
    {
        Mesh mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;

        // Define vertices for each face individually
        float phi = (1 + Mathf.Sqrt(5)) / 2; // Golden ratio
        Vector3[] baseVertices = new Vector3[] {
            new Vector3(-1,  phi, 0),
            new Vector3( 1,  phi, 0),
            new Vector3(-1, -phi, 0),
            new Vector3( 1, -phi, 0),

            new Vector3(0, -1,  phi),
            new Vector3(0,  1,  phi),
            new Vector3(0, -1, -phi),
            new Vector3(0,  1, -phi),

            new Vector3( phi, 0, -1),
            new Vector3( phi, 0,  1),
            new Vector3(-phi, 0, -1),
            new Vector3(-phi, 0,  1)
        };

        // Each face is now defined with separate vertices
        Vector3[] vertices = new Vector3[60]; // 20 faces * 3 vertices per face
        int[] triangles = new int[60]; // 20 faces * 3 vertices per face

        int[][] faceDefinitions = new int[][] {
            new int[] {0, 11, 5}, new int[] {0, 5, 1}, new int[] {0, 1, 7},
            new int[] {0, 7, 10}, new int[] {0, 10, 11}, new int[] {1, 5, 9},
            new int[] {5, 11, 4}, new int[] {11, 10, 2}, new int[] {10, 7, 6},
            new int[] {7, 1, 8}, new int[] {3, 9, 4}, new int[] {3, 4, 2},
            new int[] {3, 2, 6}, new int[] {3, 6, 8}, new int[] {3, 8, 9},
            new int[] {4, 9, 5}, new int[] {2, 4, 11}, new int[] {6, 2, 10},
            new int[] {8, 6, 7}, new int[] {9, 8, 1}
        };

        for (int i = 0; i < faceDefinitions.Length; i++)
        {
            int vertexIndex = i * 3;
            for (int j = 0; j < 3; j++)
            {
                vertices[vertexIndex + j] = baseVertices[faceDefinitions[i][j]];
                triangles[vertexIndex + j] = vertexIndex + j;
            }
        }

        // Assign vertices and triangles to the mesh
        mesh.vertices = vertices;
        mesh.triangles = triangles;

        // Update normals
        mesh.RecalculateNormals();
    }
}
