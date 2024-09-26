using System;
using UnityEngine;

public class Chunk : MonoBehaviour {
    private MeshFilter meshFilter;
    private MeshCollider meshCollider;
    private Mesh mesh;
    private bool meshChange = false;

    private void Start() {
        meshFilter = gameObject.AddComponent<MeshFilter>();
        MeshRenderer meshRenderer = gameObject.AddComponent<MeshRenderer>();
        meshRenderer.material = new Material(Shader.Find("Standard"));
        meshCollider = gameObject.AddComponent<MeshCollider>();
    }

    private void Update() {
        if (meshChange) {
            meshChange = false;
            ChangeMesh(mesh);
        }
    }

    public void ChangeMesh(Mesh mesh) {
        if (meshFilter == null || meshCollider == null) {
            meshChange = true;
            this.mesh = mesh;
            return;
        }
        meshFilter.mesh = null;
        meshFilter.mesh = mesh;
        meshCollider.sharedMesh = null;
        meshCollider.sharedMesh = mesh;
    }
}