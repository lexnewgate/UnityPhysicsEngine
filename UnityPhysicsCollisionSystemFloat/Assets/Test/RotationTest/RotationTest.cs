using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationTest : MonoBehaviour
{
    Quaternion originalOrientation;

    void Start()
    {
        this.originalOrientation = transform.rotation;
    }

    List<RotateBase> rotations = new List<RotateBase>();

    void Update()
    {
        transform.rotation = this.originalOrientation;
        GetComponents<RotateBase>(rotations);


        Debug.Log("begin:");
        foreach (var rotateAction in rotations)
        {

            rotateAction.Rotate();
            Debug.Log("---------");
            Debug.Log(transform.up);
            Debug.Log(transform.right);
            Debug.Log(transform.forward);
        }
        Debug.Log("end");

    }
}
