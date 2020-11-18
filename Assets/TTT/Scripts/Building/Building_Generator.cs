using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Building_Generator
{
    public static Building Generate(BuildingSettings settings){
        return new Building(
            settings.Size.x,
            settings.Size.y, 
            settings.wingsStrategy != null ?
            settings.wingsStrategy.GenerateWings(settings) :
            ((WingsStrategy)ScriptableObject.CreateInstance<DefaultWingsStrategy>()).GenerateWings(settings));
    }

    // static Wing[] GenerateWings(BuildingSettings settings) {
    //     return new Wing[] {
    //         settings.wingStrategy != null ?
    //         settings.wingStrategy.GenerateWing(settings, new RectInt(0, 0, settings.Size.x, settings.Size.y), 1) :
    //         ((WingStrategy)ScriptableObject.CreateInstance<WingStrategy>()).GenerateWing(settings, 
    //             new RectInt(0, 0, settings.Size.x, settings.Size.y), 1)
    //     };
    // }
    // static Wing GenerateWing(BuildingSettings settings, RectInt bounds, int numberOfStories){
    //     return new Wing(bounds,
    //     settings.storiesStrategy != null ? 
    //         settings.storiesStrategy.GenerateStories(settings, bounds, numberOfStories) :
    //         ((StoriesStrategy)ScriptableObject.CreateInstance<DefaultStoriesStrategy>()).GenerateStories(settings, bounds, numberOfStories),
    //     settings.roofStrategy != null ?
    //         settings.roofStrategy.GenerateRoof(settings, bounds) :
    //         ((RoofStrategy)ScriptableObject.CreateInstance<DefaultRoofStrategy>()).GenerateRoof(settings, bounds)
    //     );
    // }
    // static Story[] GenerateStories(BuildingSettings settings, RectInt bounds, int numberOfStories) {
    //     return new Story[] { settings.storyStrategy != null ?
    //         settings.storyStrategy.GenerateStory(settings, bounds, 1) :
    //         ((StoryStrategy)ScriptableObject.CreateInstance<DefaultStoryStrategy>()).GenerateStory(settings, bounds, 1)
    //     };
    // }
    // static Story GenerateStory(BuildingSettings settings, RectInt bounds, int level) {
    //     return new Story(0, settings.wallStrategy != null ?
    //         settings.wallStrategy.GenerateWalls(settings,bounds, level) : 
    //         ((WallStrategy)ScriptableObject.CreateInstance<DefaultWallStrategy>()).GenerateWalls(settings, bounds, level)
    //         );
    // }

    // static Wall[] GenerateWalls(BuildingSettings settings, RectInt bounds, int level) {

    //     return new Wall[(bounds.size.x + bounds.size.y) * 2];
    // }

    // static Roof GenerateRoof(BuildingSettings settings){
    //     return new Roof();
    // }
}
