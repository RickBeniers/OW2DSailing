using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    public enum DrawMode {NoiseMap, ColourMap, Mesh};
    public DrawMode drawMode;

    public const int mapChunkSize = 241;

    public int levelOfDetail;
    //[Range(0, 1)]
    public int octaves;
    public int seed;

    public float noiseScale;
    public float persistance;
    public float lacunarity;
    public float meshHeightMultiplier;

    public bool autoUpdate;

    public Vector2 offset;

    public TerrainType[] regions;

    public AnimationCurve meshHeightCurve;

    private GameObject planeObj;
    private GameObject meshObj;
    public void GenerateMap() 
    {
        float[,] noiseMap = NoiseGenerator.GenerateNoiseMap(mapChunkSize, mapChunkSize, seed, noiseScale, octaves, persistance, lacunarity, offset);

        Color[] colourMap = new Color[mapChunkSize * mapChunkSize];
        for (int y = 0; y < mapChunkSize; y++) 
        {
            for (int x = 0; x < mapChunkSize; x++) 
            {
                float currentHeight = noiseMap[x, y];
                for (int i = 0; i < regions.Length; i++) 
                {
                    if (currentHeight <= regions[i].height) 
                    {
                        colourMap[y * mapChunkSize + x] = regions[i].colour;
                        break;
                    }
                }
            }
        }

        MapDisplay display = FindObjectOfType<MapDisplay>();
        planeObj = GameObject.Find("Plane");
        meshObj = GameObject.Find("Mesh");
        if(drawMode == DrawMode.NoiseMap) 
        {
            display.DrawTexture(TextureGenerator.TextureFromHeightMap(noiseMap));
            planeObj.transform.position = new Vector3(0, 0, -2);
            meshObj.transform.position = new Vector3(0, 0, -1);
        }else if (drawMode == DrawMode.ColourMap) 
        {
            display.DrawTexture(TextureGenerator.TextureFromColourMap(colourMap, mapChunkSize, mapChunkSize));
            planeObj.transform.position = new Vector3(0, 0, -2);
            meshObj.transform.position = new Vector3(0, 0, -1);
        }
        else if (drawMode == DrawMode.Mesh) 
        {
            display.DrawMesh(MeshGenerator.GenerateTerrainMesh(noiseMap, meshHeightMultiplier, meshHeightCurve, levelOfDetail), TextureGenerator.TextureFromColourMap(colourMap, mapChunkSize, mapChunkSize));
            planeObj.transform.position = new Vector3(0, 0, -1);
            meshObj.transform.position = new Vector3(0, 0, -2);
        }
    }
    public void OnValidate()
    {
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
[System.Serializable]
public struct TerrainType
{
    public string name;
    public float height;
    public Color colour;
}
