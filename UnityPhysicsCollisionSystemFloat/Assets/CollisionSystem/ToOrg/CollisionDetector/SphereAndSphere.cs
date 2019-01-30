using FixedMath;

namespace CollisionSystem
{
    public partial class CollisionDetector
    {
        //ret -1 算法失效  球心重合
        public static int SphereAndSphere(CollisionSphere one, CollisionSphere two, CollisionData data)
        {

            // // Make sure we have contacts
            // if (data.contactsLeft <= 0) return 0;

            // // Cache the sphere positions
            // Vector3 positionOne = one.GetAxis(4);
            // Vector3 positionTwo = two.GetAxis(4);

            // // Find the vector between the objects
            // Vector3 midline =positionTwo- positionOne;
            // float size = midline.Magnitude();


            // if(size==0)
            // {
            //     return -1;
            // }

            // // See if it is large enough.
            // if (size >= one.radius + two.radius)
            // {
            //     return 0;
            // }

            // // We manually create the normal, because we have the
            // // size to hand.
            // Vector3 normal = midline * (((float)1.0) / size);


            // Contact contact = new Contact();
            // contact.contactNormal = normal;
            // contact.contactPoint = positionOne + midline * (float)0.5;
            // contact.penetration = (one.radius + two.radius - size);
            // contact.setBodyData(one.body, two.body,
            //     data.friction, data.restitution);

            // data.AddContact(contact);
            return 1;
        }
    }
}