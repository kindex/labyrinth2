using System;
using System.Runtime.InteropServices;

namespace Game.Physics.Newton
{
    public enum CollisionID : int
    {
        Box = NativeAPI.SERIALIZE_ID_BOX,
        Cone = NativeAPI.SERIALIZE_ID_CONE,
        Sphere = NativeAPI.SERIALIZE_ID_SPHERE,
        Capsule = NativeAPI.SERIALIZE_ID_CAPSULE,
        Cylinder = NativeAPI.SERIALIZE_ID_CYLINDER,
        Compound = NativeAPI.SERIALIZE_ID_COMPOUND,
        ConvexHull = NativeAPI.SERIALIZE_ID_CONVEXHULL,
        ConvexModifier = NativeAPI.SERIALIZE_ID_CONVEXMODIFIER,
        ChamferCylinder = NativeAPI.SERIALIZE_ID_CHAMFERCYLINDER,
        Tree = NativeAPI.SERIALIZE_ID_TREE,
        Null = NativeAPI.SERIALIZE_ID_NULL,
        HeightField = NativeAPI.SERIALIZE_ID_HEIGHFIELD,
        Scene = NativeAPI.SERIALIZE_ID_SCENE,
    };

    public struct BoxInfo
    {
        internal BoxInfo(Vector3 size) : this()
        {
            this.Size = size;
        }

        public Vector3 Size { get; private set; }
    }

    public struct SphereInfo
    {
        internal SphereInfo(Vector3 radius) : this()
        {
            this.Radius = radius;
        }

        public Vector3 Radius { get; private set; }
    }

    public struct CylinderInfo
    {
        internal CylinderInfo(float radius0, float radius1, float height) : this()
        {
            this.Radius0 = radius0;
            this.Radius1 = radius1;
            this.Height = height;
        }

        public float Radius0 { get; private set; }
        public float Radius1 { get; private set; }
        public float Height { get; private set; }
    }

    public struct CapsuleInfo
    {
        internal CapsuleInfo(float radius0, float radius1, float height) : this()
        {
            this.Radius0 = radius0;
            this.Radius1 = radius1;
            this.Height = height;
        }

        public float Radius0 { get; private set; }
        public float Radius1 { get; private set; }
        public float Height { get; private set; }
    }

    public struct ConeInfo
    {
        internal ConeInfo(float radius, float height) : this()
        {
            this.Radius = radius;
            this.Height = height;
        }

        public float Radius { get; private set; }
        public float Height { get; private set; }
    }

    public struct ChamferCylinderInfo
    {
        internal ChamferCylinderInfo(float radius, float height) : this()
        {
            this.Radius = radius;
            this.Height = height;
        }

        public float Radius { get; private set; }
        public float Height { get; private set; }
    }

    public struct CompoundCollisionInfo
    {
        internal CompoundCollisionInfo(World world, int childrenCount, IntPtr children) : this()
        {
            this.children = new Collision[childrenCount];
            for (int i = 0; i < childrenCount; i++)
            {
                IntPtr newtonCollision = Marshal.ReadIntPtr(new IntPtr(children.ToInt64() + i * IntPtr.Size));
                this.children[i] = Collision.FromNewtonCollision(world, newtonCollision);
            }
        }

        public Collision[] Children
        {
            get
            {
                return children;
            }
        }

        private Collision[] children;
    }

    public struct UserParamInfo
    {
        public UserParamInfo(params float[] array) : this()
        {
            this.array = array;
        }

        public float[] Array
        {
            get
            {
                return array;
            }
        }

        private float[] array;
    }

    public class CollisionInfo
    {

        internal CollisionInfo(World world, NativeAPI.NewtonCollisionInfoRecord info)
        {
            this.info = info;
            this.world = world;
        }

        public Matrix4 OffsetMatrix
        {
            get
            {
                return info.m_offsetMatrix;
            }
        }

        public CollisionID CollisionType
        {
            get
            {
                return (CollisionID)info.m_collisionType;
            }
        }

        public int ReferenceCount
        {
            get
            {
                return info.m_referenceCount;
            }
        }

        public BoxInfo AsBox
        {
            get
            {
                if (CollisionType != CollisionID.Box)
                {
                    throw new InvalidOperationException();
                }
                return new BoxInfo(new Vector3(info.m_box.m_x, info.m_box.m_y, info.m_box.m_z));
            }
        }

        public SphereInfo AsSphere
        {
            get
            {
                if (CollisionType != CollisionID.Sphere)
                {
                    throw new InvalidOperationException();
                }
                return new SphereInfo(new Vector3(info.m_sphere.m_r0, info.m_sphere.m_r1, info.m_sphere.m_r2));
            }
        }

        public CylinderInfo AsCylinder
        {
            get
            {
                if (CollisionType != CollisionID.Cylinder)
                {
                    throw new InvalidOperationException();
                }
                return new CylinderInfo(info.m_cylinder.m_r0, info.m_cylinder.m_r1, info.m_cylinder.m_height);
            }
        }

        public CapsuleInfo AsCapsule
        {
            get
            {
                if (CollisionType != CollisionID.Capsule)
                {
                    throw new InvalidOperationException();
                }
                return new CapsuleInfo(info.m_capsule.m_r0, info.m_capsule.m_r1, info.m_capsule.m_height);
            }
        }

        public ConeInfo AsCone
        {
            get
            {
                if (CollisionType != CollisionID.Cone)
                {
                    throw new InvalidOperationException();
                }
                return new ConeInfo(info.m_cone.m_r, info.m_cone.m_height);
            }
        }

        public ChamferCylinderInfo AsChamferCylinder
        {
            get
            {
                if (CollisionType != CollisionID.ChamferCylinder)
                {
                    throw new InvalidOperationException();
                }
                return new ChamferCylinderInfo(info.m_chamferCylinder.m_r, info.m_chamferCylinder.m_height);
            }
        }

        // TODO
        //public ConvexHullModifierInfo AsConvexHullModifier

        public CompoundCollisionInfo AsCompoundCollision
        {
            get
            {
                if (CollisionType != CollisionID.Compound)
                {
                    throw new InvalidOperationException();
                }
                return new CompoundCollisionInfo(world, info.m_compoundCollision.m_childrenCount, info.m_compoundCollision.m_children);
            }
        }

        //public HeightFieldInfo AsHeightField
        //public UserParamInfo AsUserParam

        private NativeAPI.NewtonCollisionInfoRecord info;
        private World world;
    }
}
