using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatePitch : RotateBase
{
    public float pitch;
    public override void Rotate()
    {
        if (worldSpace)
        {

            transform.Rotate(Vector3.right, -pitch, Space.World);
        }
        else
        {
            transform.Rotate(transform.right, -pitch, Space.World);
        }
    }
}
