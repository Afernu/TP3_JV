using UnityEngine;

public class SwirlingLavaDeformation : MonoBehaviour
{
    Mesh deformingMesh;
    Vector3[] originalVertices, displacedVertices;

    public float waveSpeed = 1f;
    public float waveHeight = 0.5f;
    public float swirlSize = 1f;
    public float swirlSpeed = 1f;

    void Start()
    {
        deformingMesh = GetComponent<MeshFilter>().mesh;
        originalVertices = deformingMesh.vertices;
        displacedVertices = new Vector3[originalVertices.Length];
    }

    void Update()
    {
        for (int i = 0; i < originalVertices.Length; i++)
        {
            Vector3 vertex = originalVertices[i];
            float swirlEffect = Mathf.Sin(swirlSpeed * Time.time + vertex.x * vertex.y) * swirlSize;
            vertex.y += (Mathf.Sin(Time.time * waveSpeed + vertex.x) + swirlEffect) * waveHeight;
            displacedVertices[i] = vertex;
        }

        deformingMesh.vertices = displacedVertices;
        deformingMesh.RecalculateNormals();
    }
}
