using System;
using System.IO;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace Game.Physics.Newton
{
    public enum PlatformArchitecture : int
    {
        LowerCommonDenominator = 0,
        CommonFloatingPointOptimizations = 1,
        BestPossibleHardwareSetting = 2,
    }

    public enum SolverModel : int
    {
        Exact = 0,
        Adaptive = 1,
        Linear = 2,
    }

    public enum FrictionModel : int
    {
        Exact = 0,
        Adaptive = 1,
    }

    public enum ProfilerEntry : int
    {
        WorldUpdate = NativeAPI.NEWTON_PROFILER_WORLD_UPDATE,
        CollisionUpdate = NativeAPI.NEWTON_PROFILER_COLLISION_UPDATE,
        CollisionUpdateBroadPhase = NativeAPI.NEWTON_PROFILER_COLLISION_UPDATE_BROAD_PHASE,
        CollisionUpdateNarrowPhase = NativeAPI.NEWTON_PROFILER_COLLISION_UPDATE_NARROW_PHASE,
        DynamicsUpdate = NativeAPI.NEWTON_PROFILER_DYNAMICS_UPDATE,
        DynamicsConstraintGraph = NativeAPI.NEWTON_PROFILER_DYNAMICS_CONSTRAINT_GRAPH,
        DynamicsSolveConstraintGraph = NativeAPI.NEWTON_PROFILER_DYNAMICS_SOLVE_CONSTRAINT_GRAPH,
    };

    public delegate void BodyIterator(Body body);
    public delegate uint RayPrefilter(Body body, Collision collision);
    public delegate float RayFilter(Body body, Vector3 hitNormal, int collisionID, float intersectParam);
    public delegate void BodyLeaveWorldEventHandler(World world, Body body, int threadIndex);

    public sealed class World : NewtonObject
    {
        public World()
            : base(NativeAPI.Create(IntPtr.Zero, IntPtr.Zero))
        {
        }

        protected override void ReleaseHandle()
        {
            NativeAPI.Destroy(handle);
        }

        public void DestroyAllBodies()
        {
            NativeAPI.DestroyAllBodies(handle);
        }

        public void Update(float timestep)
        {
            NativeAPI.Update(handle, timestep);
        }

        public void CollisionUpdate()
        {
            NativeAPI.CollisionUpdate(handle);
        }

        public void InvalidateCache()
        {
            NativeAPI.InvalidateCache(handle);
        }

        public void SetSolverModel(SolverModel model)
        {
            NativeAPI.SetSolverModel(handle, (int)model);
        }
        
        public void SetPlatformArchitecture(PlatformArchitecture mode)
        {
            NativeAPI.SetPlatformArchitecture(handle, (int)mode);
        }

        public int SetMultiThreadSolverOnSingleIsland(int mode)
        {
            return NativeAPI.SetMultiThreadSolverOnSingleIsland(handle, mode);
        }

        public uint ReadPerformanceTicks(uint thread, ProfilerEntry entry)
        {
            return NativeAPI.ReadPerformanceTicks(handle, thread, (uint)entry);
        }

        public void CriticalSectionLock()
        {
            NativeAPI.WorldCriticalSectionLock(handle);
        }

        public void CriticalSectionUnlock()
        {
            NativeAPI.WorldCriticalSectionUnlock(handle);
        }

        public void SetFrictionModel(FrictionModel model)
        {
            NativeAPI.SetFrictionModel(handle, (int)model);
        }

        public void SetMinimumFrameRate(float frameRate)
        {
            NativeAPI.SetMinimumFrameRate(handle, frameRate);
        }

        public void SetWorldSize(Vector3 minPoint, Vector3 maxPoint)
        {
            NativeAPI.SetWorldSize(handle, ref minPoint, ref maxPoint);
        }

        public ConvexCollision CreateNull()
        {
            return new ConvexCollision(this, NativeAPI.CreateNull(handle));
        }

        public ConvexCollision CreateSphere(float radiusX, float radiusY, float radiusZ)
        {
            return new ConvexCollision(this, NativeAPI.CreateSphere(handle, radiusX, radiusY, radiusZ));
        }

        public ConvexCollision CreateSphere(float radiusX, float radiusY, float radiusZ, Matrix4 offsetMatrix)
        {
            return new ConvexCollision(this, NativeAPI.CreateSphere(handle, radiusX, radiusY, radiusZ, ref offsetMatrix));
        }

        public ConvexCollision CreateSphere(Vector3 radius)
        {
            return new ConvexCollision(this, NativeAPI.CreateSphere(handle, radius.X, radius.Y, radius.Z));
        }

        public ConvexCollision CreateSphere(Vector3 radius, Matrix4 offsetMatrix)
        {
            return new ConvexCollision(this, NativeAPI.CreateSphere(handle, radius.X, radius.Y, radius.Z, ref offsetMatrix));
        }

        public ConvexCollision CreateBox(float sizeX, float sizeY, float sizeZ)
        {
            return new ConvexCollision(this, NativeAPI.CreateBox(handle, sizeX, sizeY, sizeZ));
        }

        public ConvexCollision CreateBox(float sizeX, float sizeY, float sizeZ, Matrix4 offsetMatrix)
        {
            return new ConvexCollision(this, NativeAPI.CreateBox(handle, sizeX, sizeY, sizeZ, ref offsetMatrix));
        }

        public ConvexCollision CreateBox(Vector3 size)
        {
            return new ConvexCollision(this, NativeAPI.CreateBox(handle, size.X, size.Y, size.Z));
        }

        public ConvexCollision CreateBox(Vector3 size, Matrix4 offsetMatrix)
        {
            return new ConvexCollision(this, NativeAPI.CreateBox(handle, size.X, size.Y, size.Z, ref offsetMatrix));
        }

        public ConvexCollision CreateCone(float radius, float height)
        {
            return new ConvexCollision(this, NativeAPI.CreateCone(handle, radius, height));
        }

        public ConvexCollision CreateCone(float radius, float height, Matrix4 offsetMatrix)
        {
            return new ConvexCollision(this, NativeAPI.CreateCone(handle, radius, height, ref offsetMatrix));
        }

        public ConvexCollision CreateCapsule(float radius, float height)
        {
            return new ConvexCollision(this, NativeAPI.CreateCapsule(handle, radius, height));
        }

        public ConvexCollision CreateCapsule(float radius, float height, Matrix4 offsetMatrix)
        {
            return new ConvexCollision(this, NativeAPI.CreateCapsule(handle, radius, height, ref offsetMatrix));
        }

        public ConvexCollision CreateCylinder(float radius, float height)
        {
            return new ConvexCollision(this, NativeAPI.CreateCylinder(handle, radius, height));
        }

        public ConvexCollision CreateCylinder(float radius, float height, Matrix4 offsetMatrix)
        {
            return new ConvexCollision(this, NativeAPI.CreateCylinder(handle, radius, height, ref offsetMatrix));
        }

        public ConvexCollision CreateChamferCylinder(float radius, float height)
        {
            return new ConvexCollision(this, NativeAPI.CreateChamferCylinder(handle, radius, height));
        }

        public ConvexCollision CreateChamferCylinder(float radius, float height, Matrix4 offsetMatrix)
        {
            return new ConvexCollision(this, NativeAPI.CreateChamferCylinder(handle, radius, height, ref offsetMatrix));
        }

        public ConvexCollision CreateConvexHull(Vector3[] vertexCloud, float tolerance)
        {
            return new ConvexCollision(this, NativeAPI.CreateConvexHull(handle, vertexCloud.Length, vertexCloud, 12, tolerance));
        }

        public ConvexCollision CreateConvexHull(Vector3[] vertexCloud, float tolerance, Matrix4 offsetMatrix)
        {
            return new ConvexCollision(this, NativeAPI.CreateConvexHull(handle, vertexCloud.Length, vertexCloud, 12, tolerance, ref offsetMatrix));
        }

        public ConvexHullModifier CreateConvexHullModifier(ConvexCollision convexHullCollision)
        {
            return new ConvexHullModifier(this, NativeAPI.CreateConvexHullModifier(handle, convexHullCollision.handle));
        }

        public ConvexCollision CreateCompoundCollision(params ConvexCollision[] collisions)
        {
            int length = collisions.Length;
            
            IntPtr[] colllisionPtrs = new IntPtr[length];
            for (int i=0; i<length; i++)
            {
                colllisionPtrs[i] = collisions[i].handle;
            }

            return new ConvexCollision(this, NativeAPI.CreateCompoundCollision(handle, length, colllisionPtrs));
        }

        public TreeCollision CreateTree()
        {
            return new TreeCollision(this, NativeAPI.CreateTreeCollision(handle));
        }

        public Collision CreateHeightField(int width, int height, int gridsDiagonals, ushort[] elevationMap, sbyte[] attributeMap, float horizontalScale, float verticalScale)
        {
            return new Collision(this, NativeAPI.CreateHeightFieldCollision(handle, width, height, gridsDiagonals, elevationMap, attributeMap, horizontalScale, verticalScale));
        }

        public Collision CreateCollisionFromSerialization(Stream stream)
        {
            IntPtr collision = NativeAPI.CreateCollisionFromSerialization(
                handle,
                (IntPtr serializeHandle, byte[] buffer, int size) => stream.Read(buffer, 0, size),
                IntPtr.Zero);

            return Collision.FromNewtonCollision(this, collision);
        }

        public IEnumerator<Body> Bodies
        {
            get
            {
                IntPtr newtonBody = NativeAPI.WorldGetFirstBody(handle);
                while (newtonBody != IntPtr.Zero)
                {
                    yield return Body.FromNewtonBody(newtonBody);
                    newtonBody = NativeAPI.WorldGetNextBody(handle, newtonBody);
                }
            }
        }

        public void ForEachBodyInAABB(Vector3 p0, Vector3 p1, BodyIterator callback)
        {
            NativeAPI.WorldForEachBodyInAABBDo(
                handle,
                ref p0,
                ref p1,
                (IntPtr newtonBody) => callback(Body.FromNewtonBody(newtonBody)));
        }

        public void RayCast(Vector3 p0, Vector3 p1, RayFilter filter, RayPrefilter prefilter)
        {
            NativeAPI.WorldRayCast(
                handle,
                ref p0,
                ref p1,
                (IntPtr body, ref Vector3 hitNormal, int collisionID, IntPtr userData, float intersectParam) => filter(Body.FromNewtonBody(body), hitNormal, collisionID, intersectParam),
                IntPtr.Zero,
                (IntPtr body, IntPtr collision, IntPtr userData) => prefilter(Body.FromNewtonBody(body), Collision.FromNewtonCollision(this, collision)));
        }

        public int CreateMaterialID()
        {
            return NativeAPI.MaterialCreateGroupID(handle);
        }

        public void DestroyAllMaterialID()
        {
            NativeAPI.MaterialDestroyAllGroupID(handle);
        }

        public int DefaultMaterialID
        {
            get
            {
                return NativeAPI.MaterialGetDefaultGroupID(handle);
            }
        }

        public static int MemoryUsed
        {
            get
            {
                return NativeAPI.GetMemoryUsed();
            }
        }

        public int ThreadsCount
        {
            get
            {
                return NativeAPI.GetThreadsCount(handle);
            }
            set
            {
                NativeAPI.SetThreadsCount(handle, value);
            }
        }

        public int Version
        {
            get
            {
                return NativeAPI.WorldGetVersion(handle);
            }
        }

        public int BodyCount
        {
            get
            {
                return NativeAPI.WorldGetBodyCount(handle);
            }
        }

        public int ConstraintCount
        {
            get
            {
                return NativeAPI.WorldGetConstraintCount(handle);
            }
        }

        public event BodyLeaveWorldEventHandler BodyLeaveWorldEvent
        {
            add
            {
                if (BodyLeaveWorld == null)
                {
                    NativeAPI.SetBodyLeaveWorldEvent(handle, NativeBodyLeaveWorldHandler);
                }

                BodyLeaveWorld += value;
            }
            remove
            {
                BodyLeaveWorld -= value;

                if (BodyLeaveWorld == null)
                {
                    NativeAPI.SetBodyLeaveWorldEvent(handle, null);
                }
            }
        }

        event BodyLeaveWorldEventHandler BodyLeaveWorld;

        static void BodyLeaveWorldHandler(IntPtr newtonBody, int threadIndex)
        {
            Body body = Body.FromNewtonBody(newtonBody);
            body.World.BodyLeaveWorld(body.World, body, threadIndex);
        }

        static NativeAPI.NewtonBodyLeaveWorld NativeBodyLeaveWorldHandler = new NativeAPI.NewtonBodyLeaveWorld(BodyLeaveWorldHandler);
    }
}
