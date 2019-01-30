using System.Collections;
using System.Collections.Generic;
using FixMath.NET;
using UnityEngine;

public class TestFixStructDefault : MonoBehaviour
{
    // Start is called before the first frame update

    FixVector3 vector3;
    Fix64 single;
    void Start()
    {
        printx();
    }

    void printx()
    {
        Debug.Log($"single:{single}");
        Debug.Log($"v3:{vector3}");
    }


    // Update is called once per frame
    void Update()
    {

    }
}
