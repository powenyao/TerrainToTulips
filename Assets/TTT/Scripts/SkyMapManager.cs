using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkyMapManager : MonoBehaviour
{
    public enum SkyDrawMode
    {
        NoiseMap,
        ColorMap,
        Mesh
    }
    public SkyDrawMode drawMode;

    public bool autoUpdate = true;

    public const int mapChunkSize = 241;

    [Range(0, 6)]
    public int levelOfDetail;

    public float noiseScale;

    public int octaves;

    [Range(0, 1)]
    public float persistance;

    public float lacunarity;

    public int seed;

    public Vector2 offset;
    public Material Mat_Skybox;
    public Material Mat_Skybox2;

    public Renderer textureRenderer;
    public MeshFilter meshFilter;
    public MeshRenderer meshRenderer;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.W))
        {
            print("W");
            RenderSettings.skybox = Mat_Skybox;
        }

        if (Input.GetKeyUp(KeyCode.S))
        {
            print("S");
            RenderSettings.skybox = Mat_Skybox2;
        }

        if (Input.GetKeyUp(KeyCode.A))
        {
            print("A");
            CreateNoiseMap();
        }

        if (Input.GetKeyUp(KeyCode.D))
        {
            //RenderSettings.skybox = Mat_Skybox;
        }
    }

    public void DrawMapInEditor()
    {
        CreateNoiseMap();
    }

    public void CreateNoiseMap()
    {
        print("CreateNoiseMap");
        // var noiseMap = Noise.GenerateNoiseMap(mapChunkSize, mapChunkSize, seed, noiseScale, octaves, persistance,
        //     lacunarity, offset);

        var noiseMap = Noise.GenerateNoiseMap(mapChunkSize, mapChunkSize, 5);

        for (int i = 0; i < noiseMap.Length; i++)
        {
            for (int j = 0; j < noiseMap.Rank; j++)
            {
                if(noiseMap[i,j] < 0.8f)
                {
                    noiseMap[i, j] = 0;
                }
                else
                {
                    noiseMap[i, j] = 1f;
                }
            }
        }
        if (drawMode == SkyDrawMode.NoiseMap)
        {
            //display.DrawTexture(TextureGenerator.TextureFromHeightMap(mapData.heightMap));
            Texture2D texture = TextureGenerator.TextureFromHeightMap(noiseMap);
            textureRenderer.sharedMaterial.mainTexture = texture;
            
            //renderer.transform.localScale = new Vector3(texture.width, 1, texture.height);
            //RenderSettings.skybox.mainTexture = texture;
            //textureRender.transform.localScale = new Vector3(texture.width, 1, texture.height);
        }
    }
}