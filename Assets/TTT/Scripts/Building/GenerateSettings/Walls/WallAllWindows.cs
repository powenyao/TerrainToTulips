using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName="Building Generation/Walls/WallFullOfWindows")]
public class WallAllWindows : WallStrategy
{
    public override Wall[] GenerateWalls(BuildingSettings settings, RectInt bounds, int level)
    {
        Wall[] walls =  new Wall[(bounds.size.x + bounds.size.y) * 2];
        for (int i = 0; i < walls.Length; i++){
            walls[i] = Wall.Window;
        }
        return walls;
    }
}
