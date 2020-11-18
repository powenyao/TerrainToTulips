using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building
{
    // Start is called before the first frame update
    Vector2Int size;
    Wing[] wings;

    public Vector2Int Size { get { return size;} }
    public Wing[] Wings { get { return wings;} }

    public Building(int sizeX, int sizeY, Wing[] wings){
        size = new Vector2Int(sizeX, sizeY);
        this.wings = wings;
    }

    public override string ToString()
    {
        string building = "Building:(" + size.ToString() + "; " + wings.Length + ")\n";
        foreach (Wing w in wings) {
            building += "\t" + w.ToString() + "\n";
        }
        return building;
    }
}
