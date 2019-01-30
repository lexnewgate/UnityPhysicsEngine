using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnityAPITest: MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Vector3 v=Vector3.zero;
        v=new Vector3(7,100,3);
        print($"v:{v}");
        Vector3 v1= v.normalized;
        print("Instance normalized v dont change;");
        print($"v1:{v1} v:{v}");

        v.Normalize();

        print("Instance normalize v change;");
        print($"v:{v}");

        v=new Vector3(7,100,3);
        print($"v:{v}");

        Vector3 v2=Vector3.zero;

        v2= Vector3.Normalize(v);

        print("v is a Struct;So v doesn't change by pass paras");
        print("Class normalize v not change; ret change;");
        print($"v2:{v2} v:{v}");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
