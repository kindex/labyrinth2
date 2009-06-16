using System;
using System.Runtime.InteropServices;

namespace Game.Physics.Newton
{
    public delegate bool OnAABBOverlapHandler(MaterialCollision material, Body body0, Body body1, int threadIndex);
    public delegate bool ContactProcessEventHandler(MaterialCollision material, Body body0, Body body1, float timestep, int threadIndex);

    public sealed class MaterialPair : IDisposable
    {
        static private MaterialPair FromMaterialPair(IntPtr material)
        {
            return (MaterialPair)GCHandle.FromIntPtr(NativeAPI.MaterialGetMaterialPairUserData(material)).Target;
        }

        public MaterialPair(World world, int id0, int id1)
        {
            this.world = world;
            this.id0 = id0;
            this.id1 = id1;
        }

        public void Dispose()
        {
            if (gchandle.IsAllocated == false)
            {
                return;
            }

            if (world.handle != IntPtr.Zero)
            {
                NativeAPI.MaterialSetCollisionCallback(world.handle, id0, id1, IntPtr.Zero, null, null);
            }

            gchandle.Free();
        }

        public void SetSurfaceThickness(float thickness)
        {
            NativeAPI.MaterialSetSurfaceThickness(world.handle, id0, id1, thickness);
        }

        public void SetContinuousCollision(bool enabled)
        {
            NativeAPI.MaterialSetContinuousCollisionMode(world.handle, id0, id1, enabled ? 1 : 0);
        }

        public void SetDefaultSoftness(float softness)
        {
            NativeAPI.MaterialSetDefaultSoftness(world.handle, id0, id1, softness);
        }

        public void SetDefaultElasticity(float elasticity)
        {
            NativeAPI.MaterialSetDefaultElasticity(world.handle, id0, id1, elasticity);
        }

        public void SetDefaultCollidable(bool collidable)
        {
            NativeAPI.MaterialSetDefaultCollidable(world.handle, id0, id1, collidable ? 1 : 0);
        }

        public void SetDefaultFriction(float staticFriction, float kineticFriction)
        {
            NativeAPI.MaterialSetDefaultFriction(world.handle, id0, id1, staticFriction, kineticFriction);
        }

        public void SetCollisionCallback(OnAABBOverlapHandler aabboverlap, ContactProcessEventHandler process)
        {
            if (aabboverlap == null && process == null)
            {
                if (gchandle.IsAllocated)
                {
                    gchandle.Free();
                }
                NativeAPI.MaterialSetCollisionCallback(world.handle, id0, id1, IntPtr.Zero, null, null);
                return;
            }

            if (gchandle.IsAllocated == false)
            {
                gchandle = GCHandle.Alloc(this);
            }

            AABBOverlap = aabboverlap;
            ContactProcess = process;

            NativeAPI.MaterialSetCollisionCallback(world.handle, id0, id1, GCHandle.ToIntPtr(gchandle),
                aabboverlap == null ? null : NativeOnAABBOverlapHandler,
                process == null ? null : NativeContactProcessHandler);
        }

        World world;
        int id0;
        int id1;

        GCHandle gchandle;

        OnAABBOverlapHandler AABBOverlap;
        ContactProcessEventHandler ContactProcess;

        static int OnAABBOverlapHandler(IntPtr material, IntPtr body0, IntPtr body1, int threadIndex)
        {
            MaterialPair self = FromMaterialPair(material);
            return self.AABBOverlap(new MaterialCollision(material), Body.FromNewtonBody(body0), Body.FromNewtonBody(body1), threadIndex) ? 1 : 0;
        }

        static int ContactProcessHandler(IntPtr contact, float timestep, int threadIndex)
        {
            //ContactJoint joint = new ContactJoint(contact);
            //MaterialPair self = FromMaterialPair(material);

            //return self.ContactProcess(new MaterialCollision(material), Body.FromNewtonBody(body0), Body.FromNewtonBody(body1), timestep, threadIndex) ? 1 : 0;
            return 0;
        }

        static NativeAPI.NewtonOnAABBOverlap NativeOnAABBOverlapHandler = new NativeAPI.NewtonOnAABBOverlap(OnAABBOverlapHandler);
        static NativeAPI.NewtonContactProcess NativeContactProcessHandler = new NativeAPI.NewtonContactProcess(ContactProcessHandler);
    }
}
