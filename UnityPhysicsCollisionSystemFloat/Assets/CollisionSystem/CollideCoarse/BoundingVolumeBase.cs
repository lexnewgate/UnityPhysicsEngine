namespace CollisionSystem
{
    public abstract class BoundingVolumeBase
    {
        public abstract T CombineBoundingVolumes<T>(T one,T two) where T:BoundingVolumeBase;
        public abstract bool Overlaps(BoundingVolumeBase other);
        public abstract float GetVolume();
        public abstract float GetGrowth(BoundingVolumeBase other);
    }
}