using System.Threading.Tasks;
using UnityEngine;

public class ChunkData {
    public Vector3[] vertices;
    public int[] triangles;
    public float height;
}

public class ChunkGenerator {
    private int chunkWidth;
    private int chunkDepth;
    private float heightScale;
    private float widthRatio;
    private float depthRatio;
    private ProceduralLayer[] layers;

    public ChunkGenerator(int chunkWidth, int chunkDepth, float heightScale, float widthRatio, float depthRatio, ProceduralLayer[] layers) {
        this.chunkWidth = chunkWidth;
        this.chunkDepth = chunkDepth;
        this.heightScale = heightScale;
        this.widthRatio = widthRatio;
        this.depthRatio = depthRatio;
        this.layers = layers;
    }

    public async Task<ChunkData> GenerateChunkDataAsync(int chunkX, int chunkY) {
        return await Task.Run(() => {
            int width = (int)(chunkWidth / widthRatio);
            int depth = (int)(chunkDepth / depthRatio);
            float triangleWidth = chunkWidth / (float)width;
            float triangleHeight = chunkDepth / (float)depth;

            Vector3[] verticlesWithHeight = new Vector3[(width + 1) * (depth + 1)];
            float height = 0;
            for (int x = 0; x <= width; x++) {
                for (int z = 0; z <= depth; z++) {
                    int totalX = x + chunkX * width;
                    int totalZ = z + chunkY * depth;

                    float y = ProceduralLayer.GetHeight(totalX * 0.2f, totalZ * 0.2f, layers) * heightScale;
                    verticlesWithHeight[x + z * (width + 1)] = new Vector3(x * triangleWidth, y, z * triangleHeight);
                    if (y < height) height = y;
                }
            }
            Vector3[] vertices = new Vector3[verticlesWithHeight.Length];
            for (int i = 0; i < verticlesWithHeight.Length; i++) {
                Vector3 vertex = verticlesWithHeight[i];
                vertices[i] = new Vector3(vertex.x, vertex.y-height, vertex.z);
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

            return new ChunkData {
                vertices = vertices,
                triangles = triangles,
                height = height
            };
        });
    }
}