using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetPlanetRadius : MonoBehaviour
{
    public Planet myPlanet;

    private Vector3 startingPosition;
    // Start is called before the first frame update
    void Start()
    {
        startingPosition = this.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = transform.position + new Vector3(0, myPlanet.shapeSettings.planetRadius-transform.position.y, 0);
    }
}
