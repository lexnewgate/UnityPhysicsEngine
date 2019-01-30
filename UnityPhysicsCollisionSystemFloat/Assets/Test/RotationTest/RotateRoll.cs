using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateRoll : RotateBase
{

    public float roll;
    public override void Rotate()
    {
        if(worldSpace)
        {
            transform.Rotate(Vector3.forward, roll,Space.World);
        }
        else
        {
            transform.Rotate(transform.forward, roll,Space.World);
        }
    }
}
