using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class TerrainNoise
{
    public static float[,] GenerateTerrainNoise(int width, int lenght, int octaves, float lacunarity, float persistence, float scale, int seed)
    {
        float[,] heightsMat = new float[width, lenght];

        for(int i = 0; i < width; i++)
        {
            for(int j = 0; j < lenght; j++)
            {
                float frequency = 1;
                float amplitude = 1;
                heightsMat[i, j] = 0;
                for(int k = 0; k < octaves; k++)
                {
                    float sampleX = i / scale * frequency;
                    float sampleY = j / scale * frequency;
                    float height = Mathf.PerlinNoise(sampleX + seed, sampleY + seed);
                    heightsMat[i, j] += height * amplitude;
                    frequency *= lacunarity;
                    amplitude *= persistence;
                }
            }
        }

        return heightsMat;
    }
}
