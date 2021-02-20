using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    public int mapWidth;
    public int mapHeight;
    //[Range(0, 1)]
    public int octaves;
    public int seed;

    public float noiseScale;
    public float persistance;
    public float lacunarity;

    public bool autoUpdate;

    public Vector2 offset;

    public void GenerateMap() 
    {
        float[,] noiseMap = NoiseGenerator.GenerateNoiseMap(mapWidth, mapHeight, seed, noiseScale, octaves, persistance, lacunarity, offset);

        MapDisplay display = FindObjectOfType<MapDisplay>();
        display.DrawNoiseMap(noiseMap);
    }
    public void OnValidate()
    {
        if (mapWidth < 1) 
        {
            mapWidth = 1;
        }
        if (mapHeight < 1)
        {
            mapHeight = 1;
        }
        if (lacunarity < 1) 
        {
            lacunarity = 1;
        }
        if (octaves < 0) 
        {
            octaves = 0;
        }
    }
}
