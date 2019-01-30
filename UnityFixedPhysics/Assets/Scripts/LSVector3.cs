using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct LSVector3
{

    public float x;
    public float y;
    public float z;

    public LSVector3(float x, float y, float z)
    {
        this.x=x;
        this.y=y;
        this.z=z;
    }

    public void Invert()
    {
        this.x=-this.x;
        this.y=-this.y;
        this.z=-this.z;
    }

    public float Magnitude()
    {
        return Mathf.Sqrt(SquareMagnitude());
    }

    public float SquareMagnitude()
    {
        return this.x*this.x+this.y*this.y+this.z*this.z; 
    }

    public void Normalize()
    {
        float l=Magnitude();
        if(l>0)
        {
            
        }
    }
}
