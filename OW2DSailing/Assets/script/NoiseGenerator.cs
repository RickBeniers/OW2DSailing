using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class NoiseGenerator
{
    public static float[,] GenerateNoiseMap(int mapWidth, int mapheight, int seed, float scale, int octaves, float persistance, float lacunarity, Vector2 offset) 
    {
        float[,] noiseMap = new float[mapWidth, mapheight];

        System.Random prng = new System.Random(seed);
        Vector2[] octavesOffsets = new Vector2[octaves];
        for(int i = 0; i < octaves; i++) 
        {
            float offsetX = prng.Next(-100000, 100000) + offset.x;
            float offsetY = prng.Next(-100000, 100000) + offset.y;
            octavesOffsets[i] = new Vector2(offsetX, offsetY);
        }
        if (scale <= 0) 
        {
            scale = 0.0001f;
        }

        float maxNoiseHeight = float.MinValue;
        float minNoiseHeight = float.MaxValue;

        float halfwidth = mapWidth / 2f;
        float halfheight = mapheight / 2f;

        for(int y =0; y < mapheight; y++) 
        { 
            for(int x =0; x < mapWidth; x++) 
            {
                float amplitude = 1;
                float frequency = 1;
                float noiseHeight = 0;

                for (int i = 0; i < octaves; i++)
                {
                    float sampleX = (x-halfwidth) / scale * frequency + octavesOffsets[i].x;
                    float sampleY = (y-halfheight) / scale * frequency + octavesOffsets[i].y;

                    float perlinValue = Mathf.PerlinNoise(sampleX, sampleY) * 2 - 1;
                    noiseHeight += perlinValue * amplitude;

                    amplitude *= persistance;
                    frequency *= lacunarity;
                }
                if (noiseHeight > maxNoiseHeight) 
                {
                    maxNoiseHeight = noiseHeight;
                }else if (noiseHeight < minNoiseHeight) 
                {
                    minNoiseHeight = noiseHeight;
                }
                noiseMap[x, y] = noiseHeight;
            }
        }
        for (int y = 0; y < mapheight; y++)
        {
            for (int x = 0; x < mapWidth; x++)
            {
                noiseMap[x, y] = Mathf.InverseLerp(minNoiseHeight, maxNoiseHeight, noiseMap[x, y]);
            }
        }
                return noiseMap;
    }
}
