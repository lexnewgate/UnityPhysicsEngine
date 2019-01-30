using System;
using System.Collections;
using System.Collections.Generic;
using FixedMath;

namespace CollisionSystem
{
    public class Contact
    {
        /**
         * Holds the bodies that are involved in the contact. The
         * second of these can be NULL, for contacts with the scenery.
         */
        public RigidBody firstRigidBody, secondRigidBody;

        /**
         * Holds the lateral friction coefficient at the contact.
         */
        public float friction;

        /**
         * Holds the normal restitution coefficient at the contact.
         */
        public float restitution;

        /**
         * Holds the position of the contact in world coordinates.
         */
        public Vector3 contactPoint;

        /**
         * Holds the direction of the contact in world coordinates.
         */
        public Vector3 contactNormal;

        /**
         * Holds the depth of penetration at the contact point. If both
         * bodies are specified then the contact point should be midway
         * between the inter-penetrating points.
         */
        public float penetration;

        /**
        * Sets the data that doesn't normally depend on the position
        * of the contact (i.e. the bodies, and their material properties).
        */
        public void setBodyData(RigidBody one, RigidBody two,
                         float friction, float restitution)
        {
            //TODO:
            // throw new NotImplementedException();
        }
    }

}