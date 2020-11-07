using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Noise
{
    //Basic Perlin Noise
    public static float[,] GenerateNoiseMap(int mapWidth, int mapHeight, float scale)
    {
        var noiseMap = new float [mapWidth, mapHeight];

        if (scale <= 0)
        {
            scale = 0.0001f;
        }

        for (int y = 0; y < mapHeight; y++)
        {
            for (int x = 0; x < mapWidth; x++)
            {
                var sampleX = x / scale;
                var sampleY = y / scale;
                var perlinValue = Mathf.PerlinNoise(sampleX, sampleY);
                noiseMap[x, y] = perlinValue;
            }
        }

        return noiseMap;
    }

    public static float[,] GenerateNoiseMap(int mapWidth, int mapHeight, int seed, float scale, int octaves,
        float persistance, float lacunarity, Vector2 offset)
    {
        var noiseMap = new float [mapWidth, mapHeight];

        var prng = new System.Random(seed);
        var octaveOffsets = new Vector2[octaves];
        for (int i = 0; i < octaves; i++)
        {
            var offsetX = prng.Next(-100000, 10000) + offset.x;
            var offsetY = prng.Next(-10000, 10000) + offset.y;
            octaveOffsets[i] = new Vector2(offsetX, offsetY);
        }

        if (scale <= 0)
        {
            scale = 0.0001f;
        }

        var maxNoiseHeight = float.MinValue;
        var minNoiseHeight = float.MaxValue;

        float halfWidth = mapWidth / 2f;
        float halfHeight = mapHeight / 2f;

        for (int y = 0; y < mapHeight; y++)
        {
            for (int x = 0; x < mapWidth; x++)
            {
                float amplitude = 1;
                float frequency = 1;
                float noiseHeight = 0;

                for (int i = 0; i < octaves; i++)
                {
                    var sampleX = (x-halfWidth) / scale * frequency + octaveOffsets[i].x;
                    var sampleY = (y-halfHeight) / scale * frequency + octaveOffsets[i].y;

                    var perlinValue = Mathf.PerlinNoise(sampleX, sampleY) * 2 - 1; //between -1 to 1
                    noiseHeight += perlinValue * amplitude;

                    amplitude *= persistance; //decreases each octave
                    frequency *= lacunarity; //increases each octave
                }

                if (noiseHeight > maxNoiseHeight)
                {
                    maxNoiseHeight = noiseHeight;
                }

                if (noiseHeight < minNoiseHeight)
                {
                    minNoiseHeight = noiseHeight;
                }

                noiseMap[x, y] = noiseHeight;
            }
        }

        for (int y = 0; y < mapHeight; y++)
        {
            for (int x = 0; x < mapWidth; x++)
            {
                noiseMap[x, y] = Mathf.InverseLerp(minNoiseHeight, maxNoiseHeight, noiseMap[x, y]);
            }
        }

        return noiseMap;
    }
}