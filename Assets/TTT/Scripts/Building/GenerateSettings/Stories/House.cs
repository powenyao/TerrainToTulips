using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName="Building Generation/Stories/House")]
public class House : StoriesStrategy 
{
    public int MaxFloor;
    
    public bool PerlinOrRandom; // True for Perlin
    public override Story[] GenerateStories(BuildingSettings settings, RectInt bounds, int numberOfStories) {
        int maxFloor = PerlinOrRandom ?
            Mathf.RoundToInt(Mathf.PerlinNoise(bounds.x, bounds.y) * MaxFloor) :
            Random.Range(1, MaxFloor + 1);
        
        List<Story> stories = new List<Story>();
        for (int i = 0; i < maxFloor; i ++){
            stories.Add(settings.storyStrategy != null ?
            settings.storyStrategy.GenerateStory(settings, bounds, i) :
            ((StoryStrategy)ScriptableObject.CreateInstance<DefaultStoryStrategy>()).GenerateStory(settings, bounds, i)
            );
        }
        return stories.ToArray();
    }
}