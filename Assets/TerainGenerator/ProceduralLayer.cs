using System;
using UnityEngine;

[Serializable]
public class ProceduralLayer {
    [SerializeField] private float widthMult;
    [SerializeField] private float heightMult;
    [SerializeField] private float depthMult;
    [SerializeField] private float power;

    public ProceduralLayer(float widthMult, float heightMult, float depthMult, float power = 1) {
        this.widthMult = widthMult;
        this.heightMult = heightMult;
        this.depthMult = depthMult;
        this.power = power;
    }

    private float GetHeight(float x, float z) {
        float y = Mathf.PerlinNoise(x * widthMult, z * depthMult) * heightMult;
        return Mathf.Pow(Mathf.Abs(y), power) * (float.IsNegative(y) ? -1f : 1f);
    }

    public static float GetHeight(float x, float z, ProceduralLayer[] layers) {
        float height = 1f;
        foreach (ProceduralLayer layer in layers) height += layer.GetHeight(x, z);
        return Mathf.Clamp(height, -2500f, 2500f);
    }
}