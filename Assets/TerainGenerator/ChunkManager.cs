using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class ChunkManager : MonoBehaviour {
    [SerializeField] private GameObject world;
    [SerializeField] private int chunkWidth = 32;
    [SerializeField] private int chunkDepth = 32;
    [SerializeField] private float heightScale = 2f;
    [SerializeField] private float widthRatio = 1f;
    [SerializeField] private float depthRatio = 1f;
    private ProceduralLayer[] layers = new ProceduralLayer[] {
        new ProceduralLayer(0.5f, 1.5f, 0.5f),
        new ProceduralLayer(0.05f, 20f, 0.05f, 1.1f),
        new ProceduralLayer(0.01f, 40f, 0.01f, 1.7f)
    };

    private ChunkGenerator chunkGenerator;
    private Dictionary<Vector2Int, GameObject> loadedChunks = new Dictionary<Vector2Int, GameObject>();
    private static ChunkManager instance;

    private async void Start() {
        if (instance != null) {
            Destroy(this);
            return;
        }
        instance = this;
        chunkGenerator = new ChunkGenerator(chunkWidth, chunkDepth, heightScale, widthRatio, depthRatio, layers);
        if (world == null) world = new GameObject("WorldTerrain");

        await LoadChunksAroundPlayer(0, 0, 32);
    }

    async Task LoadChunksAroundPlayer(int playerChunkX, int playerChunkY, int renderDistance) {
        List<Task> chunkTasks = new List<Task>();
        int chunkX = playerChunkX;
        int chunkY = playerChunkY;

        await LoadChunk(chunkX, chunkY);

        int[] dirX = { 1, 0, -1, 0 };
        int[] dirY = { 0, 1, 0, -1 };

        int steps = 1;
        int dirIndex = 0;

        while (steps <= renderDistance * 2) {
            for (int i = 0; i < 2; i++) {
                for (int j = 0; j < steps; j++) {
                    chunkX += dirX[dirIndex];
                    chunkY += dirY[dirIndex];

                    if (Mathf.Abs(chunkX - playerChunkX) <= renderDistance && Mathf.Abs(chunkY - playerChunkY) <= renderDistance) {
                        chunkTasks.Add(LoadChunk(chunkX, chunkY));

                        if (chunkTasks.Count >= 32) {
                            await Task.WhenAll(chunkTasks);
                            chunkTasks.Clear();
                        }
                    }
                }

                dirIndex = (dirIndex + 1) % 4;
            }

            steps++;
        }

        if (chunkTasks.Count > 0) await Task.WhenAll(chunkTasks);
    }

    public async Task LoadChunk(int chunkX, int chunkY) {
        ChunkData chunkData = await chunkGenerator.GenerateChunkDataAsync(chunkX, chunkY);

        Mesh chunkMesh = new Mesh {
            vertices = chunkData.vertices,
            triangles = chunkData.triangles
        };
        chunkMesh.RecalculateNormals();

        GameObject chunk = new GameObject("Chunk_" + chunkX + "_" + chunkY);
        chunk.transform.parent = world.transform;
        chunk.transform.localPosition = new Vector3(chunkX * chunkWidth, chunkData.height, chunkY * chunkDepth);

        Chunk chunkComponent = chunk.AddComponent<Chunk>();
        chunkComponent.ChangeMesh(chunkMesh);

        loadedChunks.Add(new Vector2Int(chunkX, chunkY), chunk);
    }
}