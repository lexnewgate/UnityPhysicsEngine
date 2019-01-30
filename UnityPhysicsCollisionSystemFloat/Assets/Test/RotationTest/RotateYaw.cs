using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateYaw : RotateBase
{
    public float yaw;
    public override void Rotate()
    {
        if(worldSpace)
        {

        transform.Rotate(Vector3.up, yaw,Space.World);
        }
        else
        {

        transform.Rotate(transform.up, yaw,Space.World);
        }

    }
}
