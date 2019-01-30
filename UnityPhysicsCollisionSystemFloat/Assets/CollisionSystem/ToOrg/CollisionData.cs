using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CollisionSystem
{
    public class CollisionData
    {
        public Contact[] contacts;

        /** Holds the maximum number of contacts the array can take. */
        public int contactsLeft;
        /** Holds the number of contacts found so far. */
        int contactCount;


        /** Holds the friction value to write into any collisions. */
        public float friction;

        /** Holds the restitution value to write into any collisions. */
        public float restitution;



        public CollisionData(int contactCapbility)
        {
            this.contactsLeft = contactCapbility;
            contacts = new Contact[contactCapbility];
        }


        public void AddContact(Contact contact)
        {
            contacts[contactCount] = contact;
            contactsLeft -= 1;
            contactCount++;
        }
    }



}
