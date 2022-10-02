using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class ChunkMesh : MonoBehaviour
{
    [NonSerialized] public Gradient Gradient;
    [NonSerialized] public float AbsoluteMax;
    [NonSerialized] public float AbsoluteMin;
    
    public float LocalMax { get; private set; }
    public float LocalMin { get; private set; }
    
    public int xSize, zSize;

    private Mesh _mesh;
    private Vector3[] _verts;
    private int[] _tris;
    private Color[] _colors;

    private void Start()
    {
        _mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = _mesh;
        
        _verts = new Vector3[xSize * zSize];
        _colors = new Color[_verts.Length];
        _tris = new int[(xSize - 1) * (zSize - 1) * 6];
        
        InstantiateData();
        _mesh.vertices = _verts;
        _mesh.triangles = _tris;
        _mesh.RecalculateNormals();
    }

    private void InstantiateData()
    {
        int index = 0;

        // first step the cords of the verts
        for (int x = 0; x < xSize; x++)
        for (int z = 0; z < zSize; z++)
        {
            _verts[index++] = new Vector3(x, 0, z);
        }
    
        // reset and set the triangles for the verts
        index = 0;
        int triIndex = 0;

        for (int x = 0; x < xSize - 1; x++)
        {
            for (int z = 0; z < zSize - 1; z++)
            {
                _tris[triIndex++] = index + 1;
                _tris[triIndex++] = index + xSize + 1;
                _tris[triIndex++] = index;
                
                _tris[triIndex++] = index + xSize + 1;
                _tris[triIndex++] = index + xSize;
                _tris[triIndex++] = index;

                index++;
            }

            index++;
        }
    }

    public delegate float HeightGenerator(int x, int z);

    public void UpdateHeights(HeightGenerator gen)
    {
        int index = 0;
        LocalMin = gen(0, 0);
        LocalMax = LocalMin;
        
        for (int x = 0; x < xSize; x++)
        {
            for (int z = 0; z < zSize; z++)
            {
                var position = transform.position;
                var height = gen((int) (x + position.x), (int) (z + position.z));
                _verts[index++].y = height;

                if (height > LocalMax) LocalMax = height;
                else if (height < LocalMin) LocalMin = height;
            }
        }

        _mesh.vertices = _verts;
    }

    public void UpdateVertColors()
    {
        if (Gradient is null) return;
        
        for (var i = 0; i < _verts.Length; i++)
        {
            var percentage = Mathf.InverseLerp(AbsoluteMin, AbsoluteMax, _verts[i].y);
            var color = Gradient.Evaluate(percentage);
            _colors[i] = color;
        }

        _mesh.colors = _colors;
        _mesh.RecalculateNormals();
    }
}