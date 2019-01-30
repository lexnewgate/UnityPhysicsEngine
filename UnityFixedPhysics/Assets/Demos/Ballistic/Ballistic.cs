using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using FixMath.NET;

public class Ballistic : MonoBehaviour
{
    public enum ShotType
    {
        UNUSED = 0,
        PISTOL,
        ARTILLERY,
        FIREBALL,
        LASER
    };

    public static int ammoRounds = 16;
    AmmoRound[] ammos = new AmmoRound[ammoRounds];
    public ShotType currentShotType;

    void Start()
    {
        currentShotType = ShotType.LASER;
        for (int i = 0; i < ammoRounds; i++)
        {
            ammos[i] = new AmmoRound();
            ammos[i].type = ShotType.UNUSED;
        }
    }


    void Fire()
    {
        var ammo = ammos.FirstOrDefault((_ammo => _ammo.type == ShotType.UNUSED));
        if (ammo == null) return;
        ammo.type=currentShotType;
        switch (currentShotType)
        {
            case ShotType.PISTOL:
                ammo.particle.Mass = (Fix64)2;  // 2.0kg
                ammo.particle.velocity = new FixVector3((Fix64)0, (Fix64)0, (Fix64)35);
                ammo.particle.acceleration = new FixVector3((Fix64)0, (Fix64)(-1), (Fix64)0);
                ammo.particle.damping = (Fix64)0.99;
                break;

            case ShotType.ARTILLERY:
                ammo.particle.Mass = (Fix64)200;  // 2.0kg
                ammo.particle.velocity = new FixVector3((Fix64)0, (Fix64)30, (Fix64)40);
                ammo.particle.acceleration = new FixVector3((Fix64)0, (Fix64)(-20), (Fix64)0);
                ammo.particle.damping = (Fix64)0.99;
                break;

            case ShotType.FIREBALL:
                ammo.particle.Mass = (Fix64)1;  // 2.0kg
                ammo.particle.velocity = new FixVector3((Fix64)0, (Fix64)0, (Fix64)10);
                ammo.particle.acceleration = new FixVector3((Fix64)0, (Fix64)(0.6), (Fix64)0);
                ammo.particle.damping = (Fix64)0.9;
                break;

            case ShotType.LASER:

                ammo.particle.Mass = (Fix64)1;  // 2.0kg
                ammo.particle.velocity = new FixVector3((Fix64)0, (Fix64)0, (Fix64)100);
                ammo.particle.acceleration = new FixVector3((Fix64)0, (Fix64)(0), (Fix64)0);
                ammo.particle.damping = (Fix64)0.99;
                break;
        }

        ammo.particle.position = new FixVector3((Fix64)0, (Fix64)1.5, (Fix64)0);
        ammo.startTime = Time.realtimeSinceStartup;

    }


    void Update()
     {

         if(Input.GetMouseButtonDown(0))
         {
             Debug.Log("call fire");
             Fire();
         }

        foreach (var ammo in ammos)
        {
            if (ammo.type != ShotType.UNUSED)
            {
                ammo.Render();
                Debug.Log("find ammo");
                ammo.particle.integrate((Fix64)Time.deltaTime);
                if (ammo.startTime + 5 < Time.realtimeSinceStartup ||
                   ammo.particle.position.y < (Fix64)0 ||
                   ammo.particle.position.y > (Fix64)200
                   )
                {
                    ammo.type = ShotType.UNUSED;
                }

                
            }
        }
    }


    class AmmoRound
    {


        public Particle particle;

        ShotType _type;
        public ShotType type
        {
            get
            {
                return _type;
            }
            set
            {
                _type=value;
                if(_type==ShotType.UNUSED)
                {
                    gameObject.SetActive(false);
                }
                else
                {
                    gameObject.SetActive(true);
                }
            }
        }
        public float startTime;
        GameObject gameObject;


        public AmmoRound()
        {
            this.particle = new Particle();
            gameObject=GameObject.CreatePrimitive(PrimitiveType.Sphere);
        }

        public void Render()
        {
            this.gameObject.transform.position = particle.position.ToVector3();
            //update transform here
        }
    }
}
