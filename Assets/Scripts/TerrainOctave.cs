using System;
using UnityEngine;

[Serializable]
public class TerrainOctave
{
    public TerrainOctave()
    {
        _noise = new FastNoiseLite();
    }

    public FastNoiseLite.NoiseType noiseType;
    public float bubble;
    public float scroll;
    public float earlyVerticalShift;
    public float verticalShift;
    public float amplitude;
    public float exponent;
    public float frequency;
    public bool absValue;
    
    private FastNoiseLite _noise;
    private float _bubble;
    private float _scroll;

    public float Generate(float x, float z)
    {
        x /= 100.1f;
        z /= 100.1f;
        
        var bubbleShift = _bubble / 100.1f;

        _noise.SetFrequency(frequency);
        var value = _noise.GetNoise(x + _scroll, z + _scroll, bubbleShift);
        value += earlyVerticalShift;

        if (absValue) value = Mathf.Abs(value);
        
        value = Mathf.Pow(value, exponent);
        value *= amplitude;
        value += verticalShift;
        
        return value;
    }

    public void UpdateFrequency(float newFreq)
    {
        _noise.SetFrequency(newFreq);
        frequency = newFreq;
    }

    public void UpdateNoiseType(FastNoiseLite.NoiseType newType)
    {
        _noise.SetNoiseType(newType);
        noiseType = newType;
    }

    public void Increment()
    {
        _bubble += bubble * Time.deltaTime;
        _scroll += scroll * Time.deltaTime;
    }
}