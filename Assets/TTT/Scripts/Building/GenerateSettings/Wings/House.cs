using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName="Building Generation/Wings/House")]
public class House : WingsStrategy
{
    public int maxHeight;
    public bool PerlinOrRandom;
    public override Wing[] GenerateWings(BuildingSettings settings, Vector3Int position) {
        int numberOfWings = 1;
        int sizeX;
        int sizeY;
        int floor;
        if (PerlinOrRandom) {
            float u = (float)position.x / 25;
            float v = (float)position.z / 25;

            sizeX = Mathf.RoundToInt(settings.Size.x * Mathf.PerlinNoise(u,v));
            sizeY = Mathf.RoundToInt(settings.Size.y * Mathf.PerlinNoise(v,u));
            floor = Mathf.RoundToInt(Mathf.PerlinNoise(0, u * 1.33f) * maxHeight);
        } else {
            Random.InitState(position.x + position.y);

            sizeX = Mathf.CeilToInt(settings.Size.x * Random.Range(0f,1f));
            sizeY = Mathf.CeilToInt(settings.Size.y * Random.Range(0f,1f));
            floor = Random.Range(1, maxHeight + 1);
        }
        
        List<Wing> stories = new List<Wing>();
        for (int i = 0; i < numberOfWings; i++){
            stories.Add(
                settings.wingStrategy != null ?
                settings.wingStrategy.GenerateWing(
                    settings,
                    new RectInt(0, 0, sizeX, sizeY),
                    floor) :
                ((WingStrategy)ScriptableObject.CreateInstance<DefaultWingStrategy>()).GenerateWing(
                    settings, 
                    new RectInt(0, 0, sizeX, sizeY),
                    floor)
            );
        }
        return stories.ToArray();
    }
}
