using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName="Building Generation/Walls/WallWithWindowsAndDoor")]
public class WallWithWindowsAndDoor : WallStrategy 
{
    public float WindowWallRatio;
    public override Wall[] GenerateWalls(BuildingSettings settings, RectInt bounds, int level)
    {
        Wall[] walls =  new Wall[(bounds.size.x + bounds.size.y) * 2];
        int maxWindowCount = Mathf.FloorToInt(walls.Length * WindowWallRatio);
        int currentWindowCount = 0;

        // Random.InitState(666);

        for (int i = 0; i < walls.Length; i++){
            if (level == 6) {
                if (i % 3 == 0)//  && Random.Range(0f, 1f) < 0.5) 
                {
                    walls[i] = Wall.Window;
                    currentWindowCount++;
                } 
            } else {
                if (currentWindowCount < maxWindowCount && Random.Range(0f, 1f) < 0.5){
                    walls[i] = Wall.Window;
                    currentWindowCount++;
                }
            }
            
        }
        if (level == 0) {
            walls[Random.Range(0, walls.Length)] = Wall.Door;
        }
        return walls;
    }
}