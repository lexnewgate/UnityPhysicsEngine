using System;
using FixMath.NET;

public class FireWork : Particle
{
    public int type;
    public Fix64 age;

    bool Update(Fix64 duration)
    {
        integrate(duration);
        age -= duration;
        return (age < Fix64.Zero) || (position.y < Fix64.Zero);
    }

}


struct FireworkRule
{
    /** The type of firework that is managed by this rule. */
    int type;

    /** The minimum length of the fuse. */
    Fix64 minAge;

    /** The maximum legnth of the fuse. */
    Fix64 maxAge;

    /** The minimum relative velocity of this firework. */
    FixVector3 minVelocity;

    /** The maximum relative velocity of this firework. */
    FixVector3 maxVelocity;

    /** The damping of this firework type. */
    Fix64 damping;

    int payLoadCount;

    Payload[] payloads;

    void init(int payLoadCount)
    {
        this.payLoadCount = payLoadCount;
        payloads = new Payload[payLoadCount];
    }

    void SetParams(int type, Fix64 minAge, Fix64 maxAge, FixVector3 minVelocity, FixVector3 maxVelocity, Fix64 damping)
    {
        this.type = type;
        this.minAge = minAge;
        this.maxAge = maxAge;
        this.minVelocity = minVelocity;
        this.maxVelocity = maxVelocity;
        this.damping = damping;
    }

    FireWork Create(FireWork parent = null)
    {
        SRandom random = new SRandom(200);

        FireWork fireWork = new FireWork();
        fireWork.type = type;
        fireWork.age = new SRandom(200).Range(minAge, maxAge);

        FixVector3 vel = FixVector3.Zero;

        if (parent != null)
        {
            fireWork.position = parent.position;
            vel += parent.velocity;
        }
        else
        {
            var start = FixVector3.Zero;
            start.x = random.Range((Fix64)(-20), (Fix64)20);
            fireWork.position = start;
        }

        vel += random.RangeVector3(minVelocity, maxVelocity);
        fireWork.velocity = vel;

        fireWork.Mass = Fix64.One;
        fireWork.damping = damping;
        fireWork.acceleration = PhysicsConfig.Gravity;
        return fireWork;
    }

    /** 
      * The payload is the new firework type to create when this
      * firework's fuse is over.
      */
    struct Payload
    {
        /** The type of the new particle to create. */
        int type;

        /** The number of particles in this payload. */
        int count;

        /** Sets the payload properties in one go. */
        void set(int type, int count)
        {
            this.type = type;
            this.count = count;
        }
    }


}