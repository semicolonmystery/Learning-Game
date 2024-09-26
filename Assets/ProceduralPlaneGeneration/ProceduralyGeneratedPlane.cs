using UnityEngine;

public class ProceduralyGeneratedPlane : MonoBehaviour {
    public int width = 1;
    public int depth = 1;
    public float heightScale = 2f;
    public float widthRatio = 1f;
    public float depthRatio = 1f;

    private ProceduralLayer[] layers = new ProceduralLayer[] {
        new ProceduralLayer(1f   , 0.2f, 1f   ),
        new ProceduralLayer(0.5f , 1.5f, 0.5f ),
        new ProceduralLayer(0.02f, 2f  , 0.02f),
        new ProceduralLayer(0.01f, 50f , 0.01f)
    };
    private MeshFilter meshFilter;
    private MeshCollider meshCollider;

    void Start() {
        meshFilter = gameObject.AddComponent<MeshFilter>();
        MeshRenderer meshRenderer = gameObject.AddComponent<MeshRenderer>();
        meshRenderer.material = new Material(Shader.Find("Standard"));
        meshCollider = gameObject.AddComponent<MeshCollider>();
        LoadMesh(GenerateMesh());
    }

    void OnValidate() {
        LoadMesh(GenerateMesh());
    }

    private Mesh GenerateMesh() {
        Mesh mesh = new Mesh();
        int width = (int) (this.width / widthRatio);
        int depth = (int) (this.depth / depthRatio);
        float triangleWidth = this.width / (float) width;
        float triangleHeight = this.depth / (float) depth;
        if (width <= 0 || depth <= 0) return mesh;

        Vector3[] vertices = new Vector3[(width + 1) * (depth + 1)];
        for (int x = 0; x <= width; x++) {
            for (int z = 0; z <= depth; z++) {
                float y = ProceduralLayer.GetHeight(x / 5f, z / 5f, layers) * heightScale;
                vertices[x + z * (width + 1)] = new Vector3(x * triangleWidth, y, z * triangleHeight);
                Debug.Log(vertices[x + z * (width + 1)]);
            }
        }

        int[] triangles = new int[width * depth * 6];
        for (int x = 0; x < width; x++) {
            for (int z = 0; z < depth; z++) {
                int start = x + z * (width + 1);
                int a = x + z * width;
                triangles[a * 6 + 0] = start;
                triangles[a * 6 + 1] = start + width + 1;
                triangles[a * 6 + 2] = start + 1;
                triangles[a * 6 + 3] = start + 1;
                triangles[a * 6 + 4] = start + width + 1;
                triangles[a * 6 + 5] = start + width + 2;
            }
        }

        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();

        return mesh;
    }

    private void LoadMesh(Mesh mesh) {
        if (meshFilter == null || meshCollider == null) return;
        meshFilter.mesh = null;
        meshFilter.mesh = mesh;
        meshCollider.sharedMesh = null;
        meshCollider.sharedMesh = mesh;
    }
}