using System;
using System.IO;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace Game.Physics.Newton
{
    public delegate void CollisionIterator(Vector3[] vertices, int faceId);

    public class Collision
    {
        internal static Collision FromNewtonCollision(World world, IntPtr newtonCollision)
        {
            NativeAPI.NewtonCollisionInfoRecord info;
            NativeAPI.CollisionGetInfo(newtonCollision, out info);

            Collision collision;

            switch (info.m_collisionType)
            {
                case NativeAPI.SERIALIZE_ID_BOX:
                case NativeAPI.SERIALIZE_ID_CONE:
                case NativeAPI.SERIALIZE_ID_SPHERE:
                case NativeAPI.SERIALIZE_ID_CAPSULE:
                case NativeAPI.SERIALIZE_ID_CYLINDER:
                case NativeAPI.SERIALIZE_ID_COMPOUND:
                case NativeAPI.SERIALIZE_ID_CONVEXHULL:
                case NativeAPI.SERIALIZE_ID_CONVEXMODIFIER:
                case NativeAPI.SERIALIZE_ID_CHAMFERCYLINDER:
                case NativeAPI.SERIALIZE_ID_NULL:
                    collision = new ConvexCollision(world, newtonCollision);
                    break;

                case NativeAPI.SERIALIZE_ID_TREE:
                    collision = new TreeCollision(world, newtonCollision);
                    break;

                case NativeAPI.SERIALIZE_ID_HEIGHFIELD:
                    collision = new Collision(world, newtonCollision);
                    break;

                case NativeAPI.SERIALIZE_ID_SCENE: //TODO
                default:
                    collision = new Collision(world, newtonCollision);
                    break;
            }

            collision.AddReference();
            return collision;
        }

        internal Collision(World world, IntPtr handle)
        {
            this.world = world;
            this.handle = handle;
        }

        public void MakeUnique()
        {
            NativeAPI.CollisionMakeUnique(world.handle, handle);
        }

        public int AddReference()
        {
            return NativeAPI.AddCollisionReference(handle);
        }

        public void Release()
        {
            NativeAPI.ReleaseCollision(world.handle, handle);
        }

        public float RayCast(Vector3 p0, Vector3 p1, out Vector3 normal, out int attribute)
        {
            return NativeAPI.CollisionRayCast(handle, ref p0, ref p1, out normal, out attribute);
        }

        public void CalculateAABB(Matrix4 matrix, out Vector3 p0, out Vector3 p1)
        {
            NativeAPI.CollisionCalculateAABB(handle, ref matrix, out p0, out p1);
        }

        public Vector3 GetSupportVertex(Vector3 dir)
        {
            Vector3 result;
            NativeAPI.CollisionSupportVertex(handle, ref dir, out result);
            return result;
        }

        public void ForEachPolygonDo(Matrix4 matrix, CollisionIterator callback)
        {
            NativeAPI.CollisionForEachPolygonDo(
                handle,
                ref matrix,
                (IntPtr userData, int vertexCount, Vector3[] faceArray, int faceId) => callback(faceArray, faceId), 
                IntPtr.Zero);
        }

        public void Serialize(Stream stream)
        {
            NativeAPI.CollisionSerialize(
                handle, 
                (IntPtr serializeHandle, byte[] buffer, int size) => stream.Write(buffer, 0, size),
                IntPtr.Zero);
        }

        public CollisionInfo Info
        {
            get
            {
                NativeAPI.NewtonCollisionInfoRecord nativeInfo;
                NativeAPI.CollisionGetInfo(handle, out nativeInfo);
                return new CollisionInfo(world, nativeInfo);
            }
        }

        internal IntPtr handle;

        World world;
    }

    public class ConvexCollision : Collision
    {
        internal ConvexCollision(World world, IntPtr collision) : base(world, collision)
        {
        }

        public bool IsTriggerVolume
        {
            get
            {
                return NativeAPI.CollisionIsTriggerVolume(handle) != 0;
            }
            set
            {
                NativeAPI.CollisionSetAsTriggerVolume(handle, value ? 1 : 0);
            }
        }

        public uint UserID
        {
            get
            {
                return NativeAPI.ConvexCollisionGetUserID(handle);
            }
            set
            {
                NativeAPI.ConvexCollisionSetUserID(handle, value);
            }
        }
        
        public float CalculateVolume()
        {
            return NativeAPI.ConvexCollisionCalculateVolume(handle);
        }

        public void CalculateInertialMatrix(out Vector3 inertia, out Vector3 origin)
        {
            NativeAPI.ConvexCollisionCalculateInertialMatrix(handle, out inertia, out origin);
        }
    }

    public sealed class ConvexHullModifier : ConvexCollision
    {
        internal ConvexHullModifier(World world, IntPtr Handle)
            : base(world, Handle)
        {
        }
    
        public Matrix4 Matrix
        {
            get
            {
                Matrix4 result;
                NativeAPI.ConvexHullModifierGetMatrix(handle, out result);
                return result;
            }
            set
            {
                NativeAPI.ConvexHullModifierSetMatrix(handle, ref value);
            }
        }
    }

    public sealed class TreeCollision : Collision
    {
        internal TreeCollision(World world, IntPtr Handle)
            : base(world, Handle)
        {
        }

        public void BeginBuild()
        {
            NativeAPI.TreeCollisionBeginBuild(handle);
        }

        public void AddFace(Vector3[] vertices, int faceAttribute)
        {
            NativeAPI.TreeCollisionAddFace(handle, vertices.Length, vertices, 12, faceAttribute);
        }

        public void EndBuild(bool optimize)
        {
            NativeAPI.TreeCollisionEndBuild(handle, optimize ? 1 : 0);
        } 
    }
}
