using System;
using System.Collections;
using System.Collections.Generic;
using CollisionSystem;
using System.Linq;
namespace CoarseSystem
{
    public class CoarseSystemManager<BoundingVolumeClass> where BoundingVolumeClass : BoundingVolumeBase, new()
    {
        private static CoarseSystemManager<BoundingVolumeClass> _instance;
        private CoarseSystemManager() { }

        public static CoarseSystemManager<BoundingVolumeClass> Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new CoarseSystemManager<BoundingVolumeClass>();
                }
                return _instance;
            }
        }


        StaticCoarseGroup<BoundingVolumeClass> _staticCoarseGroup;
        StaticCoarseGroup<BoundingVolumeClass> staticCoarseGroup
        {
            get
            {
                if (_staticCoarseGroup == null)
                {
                    _staticCoarseGroup = new StaticCoarseGroup<BoundingVolumeClass>();
                }
                return _staticCoarseGroup;
            }
        }
        MovingCoareGroup<BoundingVolumeClass> _movingCoareGroup;
        MovingCoareGroup<BoundingVolumeClass> movingCoareGroup
        {
            get
            {
                if (_movingCoareGroup == null)
                {
                    _movingCoareGroup = new MovingCoareGroup<BoundingVolumeClass>();
                }
                return _movingCoareGroup;
            }
        }

        public IEnumerable<BVHNode<BoundingVolumeClass>> iterator()
        {
            foreach (var node in this.staticIterator())
            {
                yield return node;
            }
        }

        public IEnumerable<BVHNode<BoundingVolumeClass>> movingIterator()
        {
            foreach (var node in movingCoareGroup)
            {
                yield return node;
            }

        }

        public IEnumerable<BVHNode<BoundingVolumeClass>> staticIterator()
        {
            foreach (var node in staticCoarseGroup)
            {
                yield return node;
            }
        }

        public void AddEntity(bool isStatic, Collider collider, BoundingVolumeClass boundingVolume)
        {
            if (isStatic)
            {
                staticCoarseGroup.AddEntity(collider, boundingVolume);
            }
            else
            {
                movingCoareGroup.AddEntity(collider, boundingVolume);
                
            }
        }

        public void RemoveEntity(bool isStatic, Collider collider)
        {
            if (isStatic)
            {
                staticCoarseGroup.RemoveEntity(collider);
            }
            else
            {
                movingCoareGroup.RemoveEntity(collider);
            }

        }

        public void UpdateEntity(bool isStatic, Collider collider, BoundingVolumeClass boundingVolume)
        {
            if (isStatic)
            {
                staticCoarseGroup.UpdateEntity(collider,boundingVolume);
            }
            else
            {
                movingCoareGroup.UpdateEntity(collider,boundingVolume);
            }
        }

        public int GetPotentialContacts(ref List<PotentialContact> potentialContacts)
        {
            //todo moving vs moving
            return staticCoarseGroup.GetPotentialContacts(movingCoareGroup, ref potentialContacts);
        }

    }

    public abstract class CoarseGroupBase<BoundingVolumeClass> : IEnumerable<BVHNode<BoundingVolumeClass>> where BoundingVolumeClass : BoundingVolumeBase, new()
    {
        BVHNode<BoundingVolumeClass> rootNode;
        Dictionary<Collider, BVHNode<BoundingVolumeClass>> leafNodes = new Dictionary<Collider, BVHNode<BoundingVolumeClass>>();
        public virtual void AddEntity(Collider collider, BoundingVolumeClass volume)
        {
            if (rootNode == null)
            {
                rootNode = new BVHNode<BoundingVolumeClass>(null, volume, collider);
                leafNodes.Add(collider, rootNode);
            }
            else
            {
                var changeLeafNodes = rootNode.Insert(collider, volume);
                foreach (var node in changeLeafNodes)
                {
                    leafNodes[node.collider] = node;
                }

            }
        }

        public int GetPotentialContacts(CoarseGroupBase<BoundingVolumeClass> other, ref List<PotentialContact> potentialContacts)
        {
            return rootNode.GetPotentialContactsWith(other.rootNode, ref potentialContacts);
        }

        public void RemoveEntity(Collider collider)
        {
            if (rootNode.collider == collider)
            {
                rootNode = null;
            }
            else
            {
                var nodeToRemove = leafNodes[collider];
                nodeToRemove.RemoveFromBVH();
            }

            leafNodes.Remove(collider);
        }

        //remove from tree;but not clear
        public BVHNode<BoundingVolumeClass> RemoveEntityRelation(Collider collider)
        {
            if(rootNode.collider==collider)
            {
                return rootNode;
            }
            else
            {
                var nodeToRemove=leafNodes[collider];
                nodeToRemove.RemoveFromBVH();
                return nodeToRemove;
            }
        }

        public void UpdateEntity(Collider collider, BoundingVolumeClass boundingVolume)
        {
            var nodeToRemove=RemoveEntityRelation(collider);
            nodeToRemove.volume=boundingVolume;
            // AddEntity(collider, boundingVolume);
        }

        public BVHNode<BoundingVolumeClass> FindEntity(Collider collider)
        {
            return leafNodes[collider];
        }

        public IEnumerator<BVHNode<BoundingVolumeClass>> GetEnumerator()
        {
            return rootNode == null
                 ? Enumerable.Empty<BVHNode<BoundingVolumeClass>>().GetEnumerator()
                 : rootNode.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }


    public class StaticCoarseGroup<BoundingVolumeClass> : CoarseGroupBase<BoundingVolumeClass> where BoundingVolumeClass : BoundingVolumeBase, new()
    {

    }

    public class MovingCoareGroup<BoundingVolumeClass> : CoarseGroupBase<BoundingVolumeClass> where BoundingVolumeClass : BoundingVolumeBase, new()
    {

    }
}