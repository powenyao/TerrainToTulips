using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefaultStoriesStrategy : StoriesStrategy
{
    public override Story[] GenerateStories(BuildingSettings settings, RectInt bounds, int numberOfStories) {
        List<Story> storyList = new List<Story>();
        for (int i = 0; i < numberOfStories; i++) {
            storyList.Add(
                settings.storyStrategy != null ?
                settings.storyStrategy.GenerateStory(settings, bounds, i) :
                ((StoryStrategy)ScriptableObject.CreateInstance<DefaultStoryStrategy>()).GenerateStory(settings, bounds, i));
        }
        return storyList.ToArray();
    }
}
