using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CollisionSystem
{
    public class BoundingSphere : BoundingVolumeBase
    {
        public Vector3 center;
        public float radius;

        public BoundingSphere()
        {

        }

        public BoundingSphere(Vector3 center, float radius)
        {
            this.center = center;
            this.radius = radius;
        }

        public override T CombineBoundingVolumes<T>(T one, T two) 
        {
                var first=one as BoundingSphere;
                var second=two as BoundingSphere;
                if(first==null&&second==null)
                {
                    throw new NotImplementedException();
                }

                Vector3 centerOffset = second.center - first.center;
                float sqrDistOfCenterOffset = centerOffset.sqrMagnitude;
                float radiusDiff = second.radius - first.radius;

                //larger sphere encloses the small one
                if (radiusDiff * radiusDiff >= sqrDistOfCenterOffset)
                {
                    if (first.radius > second.radius)
                    {
                        this.center = first.center;
                        this.radius = first.radius;
                    }
                    else
                    {
                        this.center = second.center;
                        this.radius = second.radius;
                    }
                }
                //otherwise partially overlapping spheres
                else
                {
                    var centerOffsetDistance = Mathf.Sqrt(sqrDistOfCenterOffset);
                    this.radius = (centerOffsetDistance + first.radius + second.radius) * 0.5f;
                    this.center = first.center;
                    if (centerOffsetDistance > 0)
                    {
                        this.center += centerOffset * ((radius - first.radius) / centerOffsetDistance);
                    }
                }
                return this as T;
        }

        //合并后增长大小
        public override float GetGrowth(BoundingVolumeBase other)
        {
            if (other is BoundingSphere)
            {
                var otherSphere = other as BoundingSphere;
                BoundingSphere newSphere = new BoundingSphere().CombineBoundingVolumes(this, otherSphere);
                return newSphere.radius * newSphere.radius - radius * radius;
            }
            throw new NotImplementedException();
        }

        public override float GetVolume()
        {
            return 4f / 3 * Mathf.PI * Mathf.Pow(radius, 3);
        }

        public override bool Overlaps(BoundingVolumeBase other)
        {
            if (other is BoundingSphere)
            {
                var otherSphere = other as BoundingSphere;
                float distSqrt = (center - otherSphere.center).sqrMagnitude;
                return distSqrt < (radius + otherSphere.radius) * (radius + otherSphere.radius);
            }
            else
            {
                throw new NotImplementedException();
            }
        }
    }
}