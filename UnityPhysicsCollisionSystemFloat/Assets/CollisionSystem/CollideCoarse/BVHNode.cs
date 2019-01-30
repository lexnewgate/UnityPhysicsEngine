using System;
using System.Collections;
using System.Collections.Generic;

namespace CollisionSystem
{
    public class BVHNode<BoundingVolumeClass> : IEnumerable<BVHNode<BoundingVolumeClass>> where BoundingVolumeClass : BoundingVolumeBase, new()
    {
        public BVHNode<BoundingVolumeClass>[] children = new BVHNode<BoundingVolumeClass>[2];
        public BoundingVolumeClass volume;
        public Collider collider;
        public BVHNode<BoundingVolumeClass> parent;

        public BVHNode(BVHNode<BoundingVolumeClass> parent, BoundingVolumeClass volume, Collider collider)
        {
            this.parent = parent;
            this.volume = volume;
            this.collider = collider;

        }
        public bool isLeaf()
        {
            return collider != null;
        }

        public int GetPotentialContactsWith(BVHNode<BoundingVolumeClass> other, ref List<PotentialContact> contacts)
        {
            if (!Overlaps(other))
            {
                return 0;
            }

            if (isLeaf() && other.isLeaf())
            {
                PotentialContact potentialContact = new PotentialContact();
                potentialContact.colliderPair[0] = collider;
                potentialContact.colliderPair[1] = other.collider;
                contacts.Add(potentialContact);
                return 1;
            }

            // Determine which node to descend into. If either is
            // a leaf, then we descend the other. If both are branches, 
            // then we use the one with the largest size.

            if (other.isLeaf() || (!isLeaf() && volume.GetVolume() >= other.volume.GetVolume())) //decend ourself
            {
                int count = children[0].GetPotentialContactsWith(other, ref contacts);
                return count + children[1].GetPotentialContactsWith(other, ref contacts);
            }
            else //decend other
            {
                int count = GetPotentialContactsWith(other.children[0], ref contacts);
                return count + GetPotentialContactsWith(other.children[1], ref contacts);
            }
        }

        bool Overlaps(BVHNode<BoundingVolumeClass> other)
        {
            return volume.Overlaps(other.volume);
        }

        public BVHNode<BoundingVolumeClass>[] Insert(Collider newCollider, BoundingVolumeClass newVolume)
        {
            // If we are a leaf, then the only option is to spawn two
            // new children and place the new body in one.
            if (isLeaf())
            {
                children[0] = new BVHNode<BoundingVolumeClass>(this, volume, collider);
                children[1] = new BVHNode<BoundingVolumeClass>(this, newVolume, newCollider);
                this.collider = null;

                RecalculateBoundingVolume();
                return children;
            }
            // Otherwise we need to work out which child gets to keep 
            // the inserted body. We give it to whoever would grow the
            // least to incorporate it.
            else
            {
                if (children[0].volume.GetGrowth(newVolume) < children[1].volume.GetGrowth(newVolume))
                {
                   return children[0].Insert(newCollider, newVolume);
                }
                else
                {
                    return children[1].Insert(newCollider, newVolume);
                }
            }
        }

        void RecalculateBoundingVolume(bool recurse = true)
        {
            if (isLeaf())
            {
                return;
            }

            volume = new BoundingVolumeClass().CombineBoundingVolumes<BoundingVolumeClass>(children[0].volume, children[1].volume);
            if (parent != null)
            {
                parent.RecalculateBoundingVolume(true);
            }
        }

        public int GetPotentialContacts(out List<PotentialContact> contacts)
        {
            contacts = new List<PotentialContact>();
            if (isLeaf())
                return 0;
            return children[0].GetPotentialContactsWith(children[1], ref contacts);
        }


        public void RemoveFromBVH()
        {
            if (parent != null)
            {
                //find sibling
                BVHNode<BoundingVolumeClass> sibling;
                if (parent.children[0] == this)
                {
                    sibling = parent.children[1];
                }
                else
                {
                    sibling = parent.children[0];
                }
                //write sibling data to parent,sibling deleted automatically
                parent.volume = sibling.volume;
                parent.collider = sibling.collider;
                parent.children[0] = sibling.children[0];
                parent.children[1] = sibling.children[1];
            }
        }

        public IEnumerator<BVHNode<BoundingVolumeClass>> GetEnumerator()
        {
            if (this.children[0] != null)
            {
                foreach (var node in this.children[0])
                {
                    yield return node;
                }
            }
            yield return this;

            if (this.children[1] != null)
            {
                foreach (var node in this.children[1])
                {
                    yield return node;
                }
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }


}


