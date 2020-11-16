using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelfRotation : MonoBehaviour
{
    public float rotationSpeed = 1f;

    public Vector3 rotationSpeedVector = Vector3.zero;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //this.transform.RotateAround(Vector3.up, rotationSpeed*Time.deltaTime);
        this.transform.Rotate(rotationSpeedVector * Time.deltaTime);
    }
}
