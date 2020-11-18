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
        
        int floor = PerlinOrRandom ?
            Mathf.RoundToInt(Mathf.PerlinNoise(position.x, position.z) * maxHeight) :
            Random.Range(1, maxHeight + 1);
        
        List<Wing> stories = new List<Wing>();
        for (int i = 0; i < numberOfWings; i++){
            stories.Add(
                settings.wingStrategy != null ?
                settings.wingStrategy.GenerateWing(
                    settings,
                    new RectInt(0, 0, settings.Size.x, settings.Size.y),
                    floor) :
                ((WingStrategy)ScriptableObject.CreateInstance<DefaultWingStrategy>()).GenerateWing(
                    settings, 
                    new RectInt(0, 0, settings.Size.x, settings.Size.y),
                    floor)
            );
        }
        return stories.ToArray();
    }
}
