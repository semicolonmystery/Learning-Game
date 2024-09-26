using UnityEngine;

public class ProceduralLayer {
    private float widthMult;
    private float heightMult;
    private float depthMult;

    public ProceduralLayer(float widthMult, float heightMult, float depthMult) {
        this.widthMult = widthMult;
        this.heightMult = heightMult;
        this.depthMult = depthMult;
    }

    private float GetHeight(float x, float z) {
        return Mathf.PerlinNoise(x * widthMult, z * depthMult) * heightMult;
    }

    public static float GetHeight(float x, float z, ProceduralLayer[] layers) {
        float height = 1f;
        foreach (ProceduralLayer layer in layers) height += layer.GetHeight(x, z);
        return height;
    }
}