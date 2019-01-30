//此文件专门处理和Unity相关的插件

#if _Client_
namespace FixMath.NET
{
    public partial struct FixVector2
    {
        public UnityEngine.Vector2 ToVector2()
        {
            return new UnityEngine.Vector2((float)x, (float)y);
        }
    }

    public partial struct FixVector3
    {
        public UnityEngine.Vector3 ToVector3()
        {
            return new UnityEngine.Vector3((float)x, (float)y, (float)z);
        }
    }

}
#endif