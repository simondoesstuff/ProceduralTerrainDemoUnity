using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TerrainSettings : MonoBehaviour
{
    public ChunkMesh[] meshes;
    public Gradient gradient;
    public bool updateColorsButton = false;
    
    public TerrainOctave[] layers;
    private TerrainOctave[] _layers;

    private void Start()
    {
        _layers = layers;
    }
    
    public float Generate(int xIn, int zIn)
    {
        float value = 0;

        foreach (var terrainOctave in layers)
        {
            value += terrainOctave.Generate(xIn, zIn);
        }

        return value;
    }

    private void UpdateMeshStructures()
    {
        _layers = layers;

        foreach (var mesh in meshes)
        {
            mesh.UpdateHeights(Generate);
        }

        var absoMax = meshes[0].LocalMax;
        var absoMin = meshes[0].LocalMin;

        for (int i = 1; i < meshes.Length; i++)
        {
            if (meshes[i].LocalMax > absoMax) absoMax = meshes[i].LocalMax;
            if (meshes[i].LocalMin < absoMin) absoMin = meshes[i].LocalMin;
        }
            
        foreach (var mesh in meshes)
        {
            mesh.AbsoluteMax = absoMax;
            mesh.AbsoluteMin = absoMin;
            mesh.UpdateVertColors();
        }
    }

    private void UpdateMeshColors()
    {
        foreach (var mesh in meshes)
        {
            mesh.Gradient = gradient;
            mesh.UpdateVertColors();
        }
    }

    private void Update()
    {
        bool updateMeshStructures = false;
        
        if (updateColorsButton)
        {
            updateColorsButton = false;
            UpdateMeshColors();
        }

        // compare length (when an octave is added)
        if (layers.Length != _layers.Length)
        {
            _layers = layers;
            updateMeshStructures = true;
        }
        
        // check with .equals()
        for (int i = 0; i < layers.Length; i++) if (_layers[i] != layers[i])
        {
            updateMeshStructures = true;
            break;
        }
        
        // check frequency for changes
        for (int i = 0; i < layers.Length; i++) if (_layers[i].frequency != layers[i].frequency)
        {
            _layers[i].UpdateFrequency(layers[i].frequency);
            updateMeshStructures = true;
        }
        
        // check frequency for changes
        for (int i = 0; i < layers.Length; i++) if (_layers[i].noiseType != layers[i].noiseType)
        {
            _layers[i].UpdateNoiseType(layers[i].noiseType);
            updateMeshStructures = true;
        }
        
        // process the animations
        foreach (var layer in layers)
        {
            if (layer.bubble != 0 || layer.scroll != 0)
            {
                layer.Increment();
                updateMeshStructures = true;
            }
        }

        if (updateMeshStructures) UpdateMeshStructures();
    }
}
