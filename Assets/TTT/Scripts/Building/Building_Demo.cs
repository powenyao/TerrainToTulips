using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building_Demo : MonoBehaviour
{
    public BuildingSettings settings;
    // Start is called before the first frame update
    void Start()
    {
        Building b = Building_Generator.Generate(settings);
        // Building c = new Building(4,4, new Wing[] {
        //     new Wing(
        //         new RectInt(0,0,4,4),
        //         new Story[]{
        //             new Story(0, new Wall[(4+4)*2])
        //         },
        //         new Roof()
        //     )
        // });
        GetComponent<BuildingRenderer>().Render(b);
        // Debug.Log(b.ToString());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
