using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "Building Generation/Building Settings")]
public class BuildingSettings : ScriptableObject
{
    public Vector2Int buildingSize;

    //TODO: Add other grammar element strategies.
    public WingsStrategy wingsStrategy;
    public WingStrategy wingStrategy;
    public StoriesStrategy storiesStrategy;
    public StoryStrategy storyStrategy;
    public WallStrategy wallStrategy;
    public RoofStrategy roofStrategy;
    public Vector2Int Size {get {return buildingSize;} }

}
