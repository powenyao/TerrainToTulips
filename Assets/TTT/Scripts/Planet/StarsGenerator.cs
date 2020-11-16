using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarsGenerator : MonoBehaviour
{
    public GameObject Prefab_Stars;

    private float[,] noiseMap;
    // Start is called before the first frame update
    void Start()
    {
        noiseMap = Noise.GenerateNoiseMap(100, 100, 2);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
