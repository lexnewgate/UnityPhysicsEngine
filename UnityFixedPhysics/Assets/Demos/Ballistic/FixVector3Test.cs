using System.Collections;
using System.Collections.Generic;
using FixMath.NET;
using UnityEngine;

public class FixVector3Test : MonoBehaviour
{

    struct Test 
    {
         public int x;
         public int y;

         public void Add(Test t)
         {
            this.x+=t.x;
            this.y+=t.y; 
         }

         public void  print()
         {
             Debug.Log($"{x} {y}");
         }
    }

     Test TestProperty;


    void Start()
    {
        Test test1=new Test{x=1,y=2};
        Test test2=new Test{x=1,y=2};
        test1.Add(test2);
        test1.print();

        TestProperty=new Test{x=1,y=2};
        TestProperty.Add(test2);
        TestProperty.print();

    }

    // public FixVector3 vel
    // {
    //     get;set;
    // }
    // public FixVector3 add;
    // // Start is called before the first frame update
    // void Start()
    // {
    //     this.vel=new FixVector3((Fix64)1,(Fix64)2,(Fix64)3);
    //     this.add=new FixVector3((Fix64)1,(Fix64)2,(Fix64)3);


    //     Debug.Log($"origin {this.vel}");
    //     this.vel.AddScaledVector(this.add,(Fix64)0.01);

    //     Debug.Log($"after {this.vel}");

    // }

    // // Update is called once per frame
    // void Update()
    // {
        
    // }
}
