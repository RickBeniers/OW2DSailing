using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Threading;

public class MapGenerator : MonoBehaviour
{
    public enum DrawMode {NoiseMap, ColourMap, Mesh};

    public DrawMode drawMode;

    public const int mapChunkSize = 241;

    public NoiseGenerator.NormalizedMode normalizedMode;

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
    [SerializeField]
    private GameObject planeObj;
    [SerializeField]
    private GameObject meshObj;

    Queue<MapThreadInfo<MapData>> mapDataThreadInfoQueue = new Queue<MapThreadInfo<MapData>>();
    Queue<MapThreadInfo<MeshData>> meshDataThreadInfoQueue = new Queue<MapThreadInfo<MeshData>>();

    public void DrawMapInEditor() 
    {
        MapData mapData = GenerateMap(Vector2.zero);

        MapDisplay display = FindObjectOfType<MapDisplay>();
        planeObj = GameObject.Find("Plane");
        meshObj = GameObject.Find("Mesh");
        if (drawMode == DrawMode.NoiseMap)
        {
            display.DrawTexture(TextureGenerator.TextureFromHeightMap(mapData.heightMap));
            planeObj.transform.position = new Vector3(0, 1, 0);
            meshObj.transform.position = new Vector3(0, -200, 0);
        }
        else if (drawMode == DrawMode.ColourMap)
        {
            display.DrawTexture(TextureGenerator.TextureFromColourMap(mapData.colourMap, mapChunkSize, mapChunkSize));
            planeObj.transform.position = new Vector3(0, 1, 0);
            meshObj.transform.position = new Vector3(0, -200, 0);
        }
        else if (drawMode == DrawMode.Mesh)
        {
            display.DrawMesh(MeshGenerator.GenerateTerrainMesh(mapData.heightMap, meshHeightMultiplier, meshHeightCurve, levelOfDetail), TextureGenerator.TextureFromColourMap(mapData.colourMap, mapChunkSize, mapChunkSize));
            planeObj.transform.position = new Vector3(0, -200, 0);
            meshObj.transform.position = new Vector3(0, 1, 0);
        }
    }
    public void RequestMapData(Vector2 centre, Action<MapData> callback) 
    {
        ThreadStart threadStart = delegate
        {
            MapDataThread(centre, callback);
        };
        new Thread(threadStart).Start();
    }
    void MapDataThread(Vector2 center, Action<MapData> callback) 
    {
        MapData mapData = GenerateMap(center);
        lock (mapDataThreadInfoQueue)
        {
            mapDataThreadInfoQueue.Enqueue(new MapThreadInfo<MapData>(callback, mapData));
        }
    }
    public void RequestMeshData(MapData mapData, int lod, Action<MeshData> callback) 
    {
        ThreadStart threadStart = delegate
        {
            MeshDataThread(mapData, lod, callback);
        };
        new Thread(threadStart).Start();
    }
    void MeshDataThread(MapData mapData, int lod, Action<MeshData> callback) 
    {
        MeshData meshData = MeshGenerator.GenerateTerrainMesh(mapData.heightMap, meshHeightMultiplier, meshHeightCurve, lod);
        lock (meshDataThreadInfoQueue) 
        {
            meshDataThreadInfoQueue.Enqueue(new MapThreadInfo<MeshData>(callback, meshData));
        }
    }
    private void Update()
    {
        if (mapDataThreadInfoQueue.Count > 0) 
        {
            for (int i = 0; i < mapDataThreadInfoQueue.Count; i++) 
            {
                MapThreadInfo<MapData> threadInfo = mapDataThreadInfoQueue.Dequeue();
                threadInfo.callback(threadInfo.parameter);
            }
        }
        if (meshDataThreadInfoQueue.Count > 0) 
        {
            for (int i = 0; i < meshDataThreadInfoQueue.Count; i++) 
            {
                MapThreadInfo<MeshData> threadInfo = meshDataThreadInfoQueue.Dequeue();
                threadInfo.callback(threadInfo.parameter);
            }
        }
    }
    public MapData GenerateMap(Vector2 centre) 
    {
        float[,] noiseMap = NoiseGenerator.GenerateNoiseMap(mapChunkSize, mapChunkSize, seed, noiseScale, octaves, persistance, lacunarity, centre + offset, normalizedMode);

        Color[] colourMap = new Color[mapChunkSize * mapChunkSize];
        for (int y = 0; y < mapChunkSize; y++) 
        {
            for (int x = 0; x < mapChunkSize; x++) 
            {
                float currentHeight = noiseMap[x, y];
                for (int i = 0; i < regions.Length; i++) 
                {
                    if (currentHeight >= regions[i].height) 
                    {
                        colourMap[y * mapChunkSize + x] = regions[i].colour;
                    }
                    else 
                    {
                        break;
                    }
                }
            }
        }
        return new MapData(noiseMap, colourMap);
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
    struct MapThreadInfo<T> 
    {
        public readonly Action<T> callback;
        public readonly T parameter;
        public MapThreadInfo(Action<T> callback, T parameter) 
        {
            this.callback = callback;
            this.parameter = parameter;
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
public struct MapData 
{
    public readonly float[,] heightMap;
    public readonly Color[] colourMap;
    public MapData(float[,] heightMap,Color[] colourMap) 
    {
        this.heightMap = heightMap;
        this.colourMap = colourMap;
    }
}
