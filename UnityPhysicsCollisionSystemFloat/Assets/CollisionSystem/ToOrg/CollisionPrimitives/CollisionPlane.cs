using FixedMath;
/**
* The plane is not a primitive: it doesn't represent another
* rigid body. It is used for contacts with the immovable
* world geometry.
*/
namespace CollisionSystem
{
    public class CollisionPlane 
    {
        /** 
         * The plane normal
         */
        Vector3 direction;

        /**
         * The distance of the plane from the origin.
         */
        float offset;
    }
}