using System;
using System.Collections;
using System.Collections.Generic;
using FixMath.NET;
using UnityEngine;

public class Particle
{
    public Fix64 damping;

    public FixVector3 position;
    public FixVector3 velocity;
    public FixVector3 acceleration;
    public Fix64 inverseMass;


    FixVector3 forceAccum;


    public Fix64 Mass
    {
        get
        {
            return Fix64.One / inverseMass;
        }
        set
        {
            inverseMass = Fix64.One / value;
        }
    }


    public void AddForce(FixVector3 force)
    {
        this.forceAccum += force;
    }



    public void ClearAccumulator()
    {
        forceAccum = FixVector3.Zero;
    }

    public bool hasFiniteMass
    {
        get
        {
            return inverseMass >= Fix64.Epsilon;
        }
    }

    public void integrate(Fix64 duration)
    {

        // Debug.Log("e:"+(float)Fix64.Epsilon);
        // Debug.Log("im:"+(float)inverseMass);
        if (inverseMass <= Fix64.Epsilon)
        {
            return;
        }

        Debug.Log($"before pos:{position} velocity:{velocity} dur:{duration}");
        position.AddScaledVector(velocity, duration);
        Debug.Log($"after pos:{position} velocity:{velocity} dur:{duration}");

        FixVector3 resultingAcc = acceleration;
        resultingAcc.AddScaledVector(forceAccum, inverseMass);
        velocity.AddScaledVector(resultingAcc, duration);
        velocity *= Fix64.Pow(damping, duration);
        ClearAccumulator();
    }




}
