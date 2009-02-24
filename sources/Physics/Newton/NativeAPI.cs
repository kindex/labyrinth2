using System;
using System.Text;
using System.Security;
using System.Runtime.InteropServices;

namespace Game.Physics.Newton
{
    [NativeLibrary("newton.dll")]
    public static class NativeAPI
    {
        // Newton callback functions

        //typedef void* (*NewtonAllocMemory) (int sizeInBytes);
        //typedef void (*NewtonFreeMemory) (void *ptr, int sizeInBytes);

        //typedef unsigned (*NewtonGetTicksCountCallback) ();

        [UnmanagedFunctionPointer(CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public delegate void NewtonSerialize(IntPtr serializeHandle, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 2)] byte[] buffer, int size);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public delegate void NewtonDeserialize(IntPtr serializeHandle, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 2), Out] byte[] buffer, int size);

        // user collision callbacks	
        //typedef void (*NewtonUserMeshCollisionDestroyCallback) (void* userData);
        //typedef void (*NewtonUserMeshCollisionCollideCallback) (NewtonUserMeshCollisionCollideDesc* collideDescData);
        //typedef dFloat (*NewtonUserMeshCollisionRayHitCallback) (NewtonUserMeshCollisionRayHitDesc* lineDescData);
        //typedef void (*NewtonUserMeshCollisionGetCollisionInfo) (void* userData, NewtonCollisionInfoRecord* infoRecord);
        //typedef void (*NewtonUserMeshCollisionGetFacesInAABB) (void* userData, const dFloat* p0, const dFloat* p1,
        //                                                       const dFloat** vertexArray, int* vertexCount, int* vertexStrideInBytes, 
        //                                                       const int* indexList, int maxIndexCount, const int* userDataList);

        //typedef dFloat (*NewtonCollisionTreeRayCastCallback) (dFloat interception, dFloat* normal, int faceId, void* usedData);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public delegate void NewtonBodyDestructor(IntPtr body);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public delegate void NewtonBodyLeaveWorld(IntPtr body, int threadIndex);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public delegate void NewtonApplyForceAndTorque(IntPtr body, float timestep, int threadIndex);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public delegate void NewtonSetTransform(IntPtr body, ref Matrix4 matrix, int threadIndex);

        //typedef int (*NewtonIslandUpdate) (const void* islandHandle, int bodyCount);

        //typedef int (*NewtonGetBuoyancyPlane) (const int collisionID, void *context, const dFloat* globalSpaceMatrix, dFloat* globalSpacePlane);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public delegate uint NewtonWorldRayPrefilterCallback(IntPtr body, IntPtr collision, IntPtr userData);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public delegate float NewtonWorldRayFilterCallback(IntPtr body, ref Vector3 hitNormal, int collisionID, IntPtr userData, float intersectParam);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public delegate int NewtonOnAABBOverlap(IntPtr material, IntPtr body0, IntPtr body1, int threadIndex);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public delegate int NewtonContactProcess(IntPtr material, IntPtr body0, IntPtr body1, float timestemp, int threadIndex);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public delegate void NewtonBodyIterator(IntPtr body);

        //typedef void (*NewtonJointIterator) (const NewtonJoint* joint);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public delegate void NewtonCollisionIterator(IntPtr userData, int vertexCount, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 1)] Vector3[] faceArray, int faceId);

        //typedef void (*NewtonBallCallBack) (const NewtonJoint* ball, dFloat timestep);
        //typedef unsigned (*NewtonHingeCallBack) (const NewtonJoint* hinge, NewtonHingeSliderUpdateDesc* desc);
        //typedef unsigned (*NewtonSliderCallBack) (const NewtonJoint* slider, NewtonHingeSliderUpdateDesc* desc);
        //typedef unsigned (*NewtonUniversalCallBack) (const NewtonJoint* universal, NewtonHingeSliderUpdateDesc* desc);
        //typedef unsigned (*NewtonCorkscrewCallBack) (const NewtonJoint* corkscrew, NewtonHingeSliderUpdateDesc* desc);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public delegate void NewtonUserBilateralCallBack(IntPtr userJoint, float timestep, int threadIndex);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public delegate void NewtonUserBilateralGetInfoCallBack(IntPtr userJoint, out NativeAPI.NewtonJointRecordTag info);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public delegate void NewtonConstraintDestructor(IntPtr joint);

        public const int NEWTON_PROFILER_WORLD_UPDATE = 0;
        public const int NEWTON_PROFILER_COLLISION_UPDATE = 1;
        public const int NEWTON_PROFILER_COLLISION_UPDATE_BROAD_PHASE = 2;
        public const int NEWTON_PROFILER_COLLISION_UPDATE_NARROW_PHASE = 3;
        public const int NEWTON_PROFILER_DYNAMICS_UPDATE = 4;
        public const int NEWTON_PROFILER_DYNAMICS_CONSTRAINT_GRAPH = 5;
        public const int NEWTON_PROFILER_DYNAMICS_SOLVE_CONSTRAINT_GRAPH = 6;

        public const int SERIALIZE_ID_BOX = 0;
        public const int SERIALIZE_ID_CONE = 1;
        public const int SERIALIZE_ID_SPHERE = 2;
        public const int SERIALIZE_ID_CAPSULE = 3;
        public const int SERIALIZE_ID_CYLINDER = 4;
        public const int SERIALIZE_ID_COMPOUND = 5;
        public const int SERIALIZE_ID_CONVEXHULL = 6;
        public const int SERIALIZE_ID_CONVEXMODIFIER = 7;
        public const int SERIALIZE_ID_CHAMFERCYLINDER = 8;
        public const int SERIALIZE_ID_TREE = 9;
        public const int SERIALIZE_ID_NULL = 10;
        public const int SERIALIZE_ID_HEIGHFIELD = 11;
        public const int SERIALIZE_ID_SCENE = 12;

        [StructLayout(LayoutKind.Sequential)]
        public struct NewtonBoxParam
        {
            public float m_x;
            public float m_y;
            public float m_z;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct NewtonSphereParam
        {
            public float m_r0;
            public float m_r1;
            public float m_r2;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct NewtonCylinderParam
        {
            public float m_r0;
            public float m_r1;
            public float m_height;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct NewtonCapsuleParam
        {
            public float m_r0;
            public float m_r1;
            public float m_height;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct NewtonConeParam
        {
            public float m_r;
            public float m_height;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct NewtonChamferCylinderParam
        {
            public float m_r;
            public float m_height;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct NewtonConvexHullModifierParam
        {
            public IntPtr m_children;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct NewtonCompoundCollisionParam
        {
            public int m_childrenCount;
            public IntPtr m_children;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct NewtonHeightFieldCollisionParam
        {
            public int m_width;
            public int m_height;
            public int m_gridsDiagonals;
            public float m_horizontalScale;
            public float m_verticalScale;
            public IntPtr m_elevation; //unsigned short*
            public IntPtr m_attributes; // char*
        }

        [StructLayout(LayoutKind.Explicit, Size = 128)]
        public struct NewtonCollisionInfoRecord
        {
            [FieldOffset(0)] public Matrix4 m_offsetMatrix;
            [FieldOffset(64)] public int m_collisionType;
            [FieldOffset(68)] public int m_referenceCount;
            [FieldOffset(72)] public NewtonBoxParam m_box;
            [FieldOffset(72)] public NewtonConeParam m_cone;
            [FieldOffset(72)] public NewtonSphereParam m_sphere;
            [FieldOffset(72)] public NewtonCapsuleParam m_capsule;
            [FieldOffset(72)] public NewtonCylinderParam m_cylinder;
            [FieldOffset(72)] public NewtonChamferCylinderParam m_chamferCylinder;
            [FieldOffset(72)] public NewtonCompoundCollisionParam m_compoundCollision;
            [FieldOffset(72)] public NewtonConvexHullModifierParam m_convexHullModifier;
            [FieldOffset(72)] public NewtonHeightFieldCollisionParam m_heightField;
         //   [FieldOffset(72), MarshalAs(UnmanagedType.ByValArray, SizeConst = 56)] public byte[] m_param;
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
        public struct NewtonJointRecordTag
        {
            public Matrix4 m_attachmentMatrix_0;
            public Matrix4 m_attachmentMatrix_1;
            public Vector3 m_minLinearDof;
            public Vector3 m_maxLinearDof;
            public Vector3 m_minAngularDof;
            public Vector3 m_maxAngularDof;
            public IntPtr m_attachBody_0;
            public IntPtr m_attachBody_1;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
            public float[] m_extraParameters;
            public int m_bodiesCollisionOn;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
            public string m_descriptionType;
        }

        //typedef struct NewtonUserMeshCollisionCollideDescTag
        //{
        //    dFloat m_boxP0[4];						// lower bounding box of intersection query in local space
        //    dFloat m_boxP1[4];						// upper bounding box of intersection query in local space
        //    int m_threadNumber;						// current thread executing this query
        //    int	m_faceCount;                        // the application should set here how many polygons intersect the query box
        //    int m_vertexStrideInBytes;              // the application should set here the size of each vertex
        //    void* m_userData;                       // user data passed to the collision geometry at creation time
        //    dFloat* m_vertex;                       // the application should the pointer to the vertex array. 
        //    int* m_userAttribute;                   // the application should set here the pointer to the user data, one for each face
        //    int* m_faceIndexCount;                  // the application should set here the pointer to the vertex count of each face.
        //    int* m_faceVertexIndex;                 // the application should set here the pointer index array for each vertex on a face.
        //    NewtonBody* m_objBody;                  // pointer to the colliding body
        //    NewtonBody* m_polySoupBody;             // pointer to the rigid body owner of this collision tree 
        //} NewtonUserMeshCollisionCollideDesc;


        //typedef struct NewtonWorldConvexCastReturnInfoTag
        //{
        //    dFloat m_point[4];						// collision point in global space
        //    dFloat m_normal[4];						// surface normal at collision point in global space
        //    dFloat m_penetration;                   // contact penetration at collision point
        //    dFloat m_intersectionParam;				// parametric intersection parameter along ray
        //    int    m_contactID;	                    // collision ID at contact point
        //    const NewtonBody* m_hitBody;			// body hit at contact point
        //} NewtonWorldConvexCastReturnInfo;

        //typedef struct NewtonUserMeshCollisionRayHitDescTag
        //{
        //    dFloat m_p0[4];							// ray origin in collision local space
        //    dFloat m_p1[4];                         // ray destination in collision local space   
        //    dFloat m_normalOut[4];					// copy here the normal at the ray intersection
        //    int m_userIdOut;                        // copy here a user defined id for further feedback  
        //    void* m_userData;                       // user data passed to the collision geometry at creation time
        //} NewtonUserMeshCollisionRayHitDesc;

        //typedef struct NewtonHingeSliderUpdateDescTag
        //{
        //    dFloat m_accel;
        //    dFloat m_minFriction;
        //    dFloat m_maxFriction;
        //    dFloat m_timestep;
        //} NewtonHingeSliderUpdateDesc;

        // Newton callback functions

        //typedef struct NewtonUserMeshCollisionCollideDescTag
        //{
        //    dFloat m_boxP0[4];						// lower bounding box of intersection query in local space
        //    dFloat m_boxP1[4];						// upper bounding box of intersection query in local space
        //    void* m_userData;                       // user data passed to the collision geometry at creation time
        //    int	  m_faceCount;                      // the application should set here how many polygons intersect the query box
        //    dFloat* m_vertex;                       // the application should the pointer to the vertex array. 
        //    int m_vertexStrideInBytes;              // the application should set here the size of each vertex
        //    int* m_userAttribute;                   // the application should set here the pointer to the user data, one for each face
        //    int* m_faceIndexCount;                  // the application should set here the pointer to the vertex count of each face.
        //    int* m_faceVertexIndex;                 // the application should set here the pointer index array for each vertex on a face.
        //    NewtonBody* m_objBody;                  // pointer to the colliding body
        //    NewtonBody* m_polySoupBody;             // pointer to the rigid body owner of this collision tree 
        //} NewtonUserMeshCollisionCollideDesc;

        //typedef struct NewtonUserMeshCollisionRayHitDescTag
        //{
        //    dFloat m_p0[4];							// ray origin in collision local space
        //    dFloat m_p1[4];                         // ray destination in collision local space   
        //    dFloat m_normalOut[4];					// copy here the normal at the rat intersection
        //    int m_userIdOut;                        // copy here a user defined id for further feedback  
        //    void* m_userData;                       // user data passed to the collision geometry at creation time
        //} NewtonUserMeshCollisionRayHitDesc;

        //typedef struct NewtonHingeSliderUpdateDescTag
        //{
        //    dFloat m_accel;
        //    dFloat m_minFriction;
        //    dFloat m_maxFriction;
        //    dFloat m_timestep;
        //} NewtonHingeSliderUpdateDesc;


        // **********************************************************************************************
        //
        // world control functions
        //
        // **********************************************************************************************

        [UnmanagedFunctionPointer(CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public delegate IntPtr NewtonCreate(IntPtr malloc, IntPtr mFree);
        public static NewtonCreate Create;

        [UnmanagedFunctionPointer(CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public delegate void NewtonDestroy(IntPtr newtonWorld);
        public static NewtonDestroy Destroy;

        [UnmanagedFunctionPointer(CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public delegate void NewtonDestroyAllBodies(IntPtr newtonWorld);
        public static NewtonDestroyAllBodies DestroyAllBodies;

        [UnmanagedFunctionPointer(CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public delegate int NewtonGetMemoryUsed();
        public static NewtonGetMemoryUsed GetMemoryUsed;

        [UnmanagedFunctionPointer(CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public delegate void NewtonUpdate(IntPtr newtonWorld, float timestep);
        public static NewtonUpdate Update;

        [UnmanagedFunctionPointer(CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public delegate void NewtonCollisionUpdate(IntPtr newtonWorld);
        public static NewtonCollisionUpdate CollisionUpdate;

        [UnmanagedFunctionPointer(CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public delegate void NewtonInvalidateCache(IntPtr newtonWorld);
        public static NewtonInvalidateCache InvalidateCache;

        [UnmanagedFunctionPointer(CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public delegate void NewtonSetSolverModel(IntPtr newtonWorld, int model);
        public static NewtonSetSolverModel SetSolverModel;

        [UnmanagedFunctionPointer(CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public delegate void NewtonSetPlatformArchitecture(IntPtr newtonWorld, int mode);
        public static NewtonSetPlatformArchitecture SetPlatformArchitecture;

        //NEWTON_API int NewtonGetPlatformArchitecture(const NewtonWorld* newtonWorld, char* description);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public delegate int NewtonSetMultiThreadSolverOnSingleIsland(IntPtr newtonWorld, int mode);
        public static NewtonSetMultiThreadSolverOnSingleIsland SetMultiThreadSolverOnSingleIsland;

        //NEWTON_API void NewtonSetPerformanceClock (const NewtonWorld* newtonWorld, NewtonGetTicksCountCallback callback);
        
        [UnmanagedFunctionPointer(CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public delegate uint NewtonReadPerformanceTicks(IntPtr newtonWorld, uint thread, uint performanceEntry);
        public static NewtonReadPerformanceTicks ReadPerformanceTicks;

        [UnmanagedFunctionPointer(CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public delegate void NewtonWorldCriticalSectionLock(IntPtr newtonWorld);
        public static NewtonWorldCriticalSectionLock WorldCriticalSectionLock;
        
        [UnmanagedFunctionPointer(CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public delegate void NewtonWorldCriticalSectionUnlock(IntPtr newtonWorld);
        public static NewtonWorldCriticalSectionUnlock WorldCriticalSectionUnlock;

        [UnmanagedFunctionPointer(CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public delegate void NewtonSetThreadsCount(IntPtr newtonWorld, int threads);
        public static NewtonSetThreadsCount SetThreadsCount;

        [UnmanagedFunctionPointer(CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public delegate int NewtonGetThreadsCount(IntPtr newtonWorld);
        public static NewtonGetThreadsCount GetThreadsCount;

        [UnmanagedFunctionPointer(CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public delegate void NewtonSetFrictionModel(IntPtr newtonWorld, int model);
        public static NewtonSetFrictionModel SetFrictionModel;

        [UnmanagedFunctionPointer(CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public delegate void NewtonSetMinimumFrameRate(IntPtr newtonWorld, float frameRate);
        public static NewtonSetMinimumFrameRate SetMinimumFrameRate;

        [UnmanagedFunctionPointer(CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public delegate void NewtonSetBodyLeaveWorldEvent(IntPtr newtonWorld, NewtonBodyLeaveWorld callback);
        public static NewtonSetBodyLeaveWorldEvent SetBodyLeaveWorldEvent;
    
        [UnmanagedFunctionPointer(CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public delegate void NewtonSetWorldSize(IntPtr newtonWorld, [In] ref Vector3 minPoint, [In] ref Vector3 maxPoint);
        public static NewtonSetWorldSize SetWorldSize;

        //NEWTON_API void NewtonSetIslandUpdateEvent (const NewtonWorld* newtonWorld, NewtonIslandUpdate islandUpdate); 

        //NEWTON_API void NewtonWorldForEachJointDo (const NewtonWorld* newtonWorld, NewtonJointIterator callback);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public delegate void NewtonWorldForEachBodyInAABBDo(IntPtr newtonWorld, [In] ref Vector3 p0, [In] ref Vector3 p1, NewtonBodyIterator callback);
        public static NewtonWorldForEachBodyInAABBDo WorldForEachBodyInAABBDo;

        //NEWTON_API void NewtonWorldSetUserData (const NewtonWorld* newtonWorld, void* userData);
        //NEWTON_API void* NewtonWorldGetUserData (const NewtonWorld* newtonWorld);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public delegate int NewtonWorldGetVersion(IntPtr newtonWorld);
        public static NewtonWorldGetVersion WorldGetVersion;

        [UnmanagedFunctionPointer(CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public delegate void NewtonWorldRayCast(IntPtr newtonWorld, [In] ref Vector3 p0, [In] ref Vector3 p1, NewtonWorldRayFilterCallback filter, IntPtr userData, NewtonWorldRayPrefilterCallback prefilter);
        public static NewtonWorldRayCast WorldRayCast;

        //NEWTON_API int NewtonWorldConvexCast (const NewtonWorld* newtonWorld, const dFloat* matrix, const dFloat* target, const NewtonCollision* shape, dFloat* hitParam, void* userData,  
        //								      NewtonWorldRayPrefilterCallback prefilter, NewtonWorldConvexCastReturnInfo* info, int maxContactsCount);

        // world utility functions
        [UnmanagedFunctionPointer(CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public delegate int NewtonWorldGetBodyCount(IntPtr newtonWorld);
        public static NewtonWorldGetBodyCount WorldGetBodyCount;

        [UnmanagedFunctionPointer(CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public delegate int NewtonWorldGetConstraintCount(IntPtr newtonWorld);
        public static NewtonWorldGetConstraintCount WorldGetConstraintCount;
        
        // **********************************************************************************************
        //
        // Simulation islands 
        //
        // **********************************************************************************************
        
        //NEWTON_API NewtonBody* NewtonIslandGetBody (const void* island, int bodyIndex);
        //NEWTON_API void NewtonIslandGetBodyAABB (const void* island, int bodyIndex, const dFloat* p0, const dFloat* p1);

        // **********************************************************************************************
        //
        // Physics Material Section
        //
        // **********************************************************************************************

        [UnmanagedFunctionPointer(CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public delegate int NewtonMaterialCreateGroupID(IntPtr newtonWorld);
        public static NewtonMaterialCreateGroupID MaterialCreateGroupID;

        [UnmanagedFunctionPointer(CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public delegate int NewtonMaterialGetDefaultGroupID(IntPtr newtonWorld);
        public static NewtonMaterialGetDefaultGroupID MaterialGetDefaultGroupID;

        [UnmanagedFunctionPointer(CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public delegate void NewtonMaterialDestroyAllGroupID(IntPtr newtonWorld);
        public static NewtonMaterialDestroyAllGroupID MaterialDestroyAllGroupID;

        // material definitions that can not be overwritten in function callback

        //NEWTON_API void* NewtonMaterialGetUserData (const NewtonWorld* newtonWorld, int id0, int id1);
        
        [UnmanagedFunctionPointer(CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public delegate void NewtonMaterialSetSurfaceThickness(IntPtr newtonWorld, int id0, int id1, float thickness);
        public static NewtonMaterialSetSurfaceThickness MaterialSetSurfaceThickness;
        
        [UnmanagedFunctionPointer(CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public delegate void NewtonMaterialSetContinuousCollisionMode(IntPtr newtonWorld, int id0, int id1, int state);
        public static NewtonMaterialSetContinuousCollisionMode MaterialSetContinuousCollisionMode;

        [UnmanagedFunctionPointer(CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public delegate void NewtonMaterialSetCollisionCallback(IntPtr newtonWorld, int id0, int id1, IntPtr userData, NewtonOnAABBOverlap aabbOverlap, NewtonContactProcess process);
        public static NewtonMaterialSetCollisionCallback MaterialSetCollisionCallback;

        [UnmanagedFunctionPointer(CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public delegate void NewtonMaterialSetDefaultSoftness(IntPtr newtonWorld, int id0, int id1, float value);
        public static NewtonMaterialSetDefaultSoftness MaterialSetDefaultSoftness;

        [UnmanagedFunctionPointer(CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public delegate void NewtonMaterialSetDefaultElasticity(IntPtr newtonWorld, int id0, int id1, float elasticCoef);
        public static NewtonMaterialSetDefaultElasticity MaterialSetDefaultElasticity;

        [UnmanagedFunctionPointer(CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public delegate void NewtonMaterialSetDefaultCollidable(IntPtr newtonWorld, int id0, int id1, int state);
        public static NewtonMaterialSetDefaultCollidable MaterialSetDefaultCollidable;

        [UnmanagedFunctionPointer(CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public delegate void NewtonMaterialSetDefaultFriction(IntPtr newtonWorld, int id0, int id1, float staticFriction, float kineticFriction);
        public static NewtonMaterialSetDefaultFriction MaterialSetDefaultFriction;

        //NEWTON_API NewtonMaterial* NewtonWorldGetFirstMaterial (const NewtonWorld* world);
        //NEWTON_API NewtonMaterial* NewtonWorldGetNextMaterial (const NewtonWorld* world, const NewtonMaterial* material);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public delegate IntPtr NewtonWorldGetFirstBody(IntPtr world);
        public static NewtonWorldGetFirstBody WorldGetFirstBody;

        [UnmanagedFunctionPointer(CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public delegate IntPtr NewtonWorldGetNextBody(IntPtr world, IntPtr curBody);
        public static NewtonWorldGetNextBody WorldGetNextBody;
            
        // **********************************************************************************************
        //
        // Physics Contact control functions
        //
        // **********************************************************************************************

        [UnmanagedFunctionPointer(CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public delegate IntPtr NewtonMaterialGetMaterialPairUserData(IntPtr material);
        public static NewtonMaterialGetMaterialPairUserData MaterialGetMaterialPairUserData;

        [UnmanagedFunctionPointer(CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public delegate uint NewtonMaterialGetContactFaceAttribute(IntPtr material);
        public static NewtonMaterialGetContactFaceAttribute MaterialGetContactFaceAttribute;

        [UnmanagedFunctionPointer(CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public delegate uint NewtonMaterialGetBodyCollisionID(IntPtr material, IntPtr body);
        public static NewtonMaterialGetBodyCollisionID MaterialGetBodyCollisionID;

        [UnmanagedFunctionPointer(CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public delegate float NewtonMaterialGetContactNormalSpeed(IntPtr material);
        public static NewtonMaterialGetContactNormalSpeed MaterialGetContactNormalSpeed;

        [UnmanagedFunctionPointer(CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public delegate void NewtonMaterialGetContactForce(IntPtr material, out Vector3 force);
        public static NewtonMaterialGetContactForce MaterialGetContactForce;

        [UnmanagedFunctionPointer(CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public delegate void NewtonMaterialGetContactPositionAndNormal(IntPtr material, out Vector3 posit, out Vector3 normal);
        public static NewtonMaterialGetContactPositionAndNormal MaterialGetContactPositionAndNormal;

        [UnmanagedFunctionPointer(CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public delegate void NewtonMaterialGetContactTangentDirections(IntPtr material, out Vector3 dir0, out Vector3 dir);
        public static NewtonMaterialGetContactTangentDirections MaterialGetContactTangentDirections;

        [UnmanagedFunctionPointer(CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public delegate float NewtonMaterialGetContactTangentSpeed(IntPtr material, int index);
        public static NewtonMaterialGetContactTangentSpeed MaterialGetContactTangentSpeed;

        [UnmanagedFunctionPointer(CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public delegate void NewtonMaterialSetContactSoftness(IntPtr material, float softness);
        public static NewtonMaterialSetContactSoftness MaterialSetContactSoftness;

        [UnmanagedFunctionPointer(CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public delegate void NewtonMaterialSetContactElasticity(IntPtr material, float restitution);
        public static NewtonMaterialSetContactElasticity MaterialSetContactElasticity;

        [UnmanagedFunctionPointer(CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public delegate void NewtonMaterialSetContactFrictionState(IntPtr material, int state, int index);
        public static NewtonMaterialSetContactFrictionState MaterialSetContactFrictionState;

        [UnmanagedFunctionPointer(CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public delegate void NewtonMaterialSetContactFrictionCoef(IntPtr material, float staticFrictionCoef, float kineticFrictionCoef, int index);
        public static NewtonMaterialSetContactFrictionCoef MaterialSetContactFrictionCoef;

        [UnmanagedFunctionPointer(CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public delegate void NewtonMaterialSetContactNormalAcceleration(IntPtr material, float accel);
        public static NewtonMaterialSetContactNormalAcceleration MaterialSetContactNormalAcceleration;

        [UnmanagedFunctionPointer(CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public delegate void NewtonMaterialSetContactNormalDirection(IntPtr material, [In] ref Vector3 directionVector);
        public static NewtonMaterialSetContactNormalDirection MaterialSetContactNormalDirection;

        [UnmanagedFunctionPointer(CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public delegate void NewtonMaterialSetContactTangentAcceleration(IntPtr material, float accel, int index);
        public static NewtonMaterialSetContactTangentAcceleration MaterialSetContactTangentAcceleration;

        [UnmanagedFunctionPointer(CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public delegate void NewtonMaterialContactRotateTangentDirections(IntPtr material, [In] ref Vector3 directionVector);
        public static NewtonMaterialContactRotateTangentDirections MaterialContactRotateTangentDirections;

        // **********************************************************************************************
        //
        // convex collision primitives creation functions
        //
        // **********************************************************************************************

        [UnmanagedFunctionPointer(CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public delegate IntPtr NewtonCreateNull(IntPtr newtonWorld);
        public static NewtonCreateNull CreateNull;

        public static partial class Private
        {
            [UnmanagedFunctionPointer(CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
            public delegate IntPtr NewtonCreateSphere(IntPtr newtonWorld, float radiusX, float radiusY, float radiusZ, IntPtr offsetMatrix);
            public static NewtonCreateSphere CreateSphere;

            [UnmanagedFunctionPointer(CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
            public delegate IntPtr NewtonCreateBox(IntPtr newtonWorld, float dx, float dy, float dz, IntPtr offsetMatrix);
            public static NewtonCreateBox CreateBox;

            [UnmanagedFunctionPointer(CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
            public delegate IntPtr NewtonCreateCone(IntPtr newtonWorld, float radius, float height, IntPtr offsetMatrix);
            public static NewtonCreateCone CreateCone;

            [UnmanagedFunctionPointer(CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
            public delegate IntPtr NewtonCreateCapsule(IntPtr newtonWorld, float radius, float height, IntPtr offsetMatrix);
            public static NewtonCreateCapsule CreateCapsule;

            [UnmanagedFunctionPointer(CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
            public delegate IntPtr NewtonCreateCylinder(IntPtr newtonWorld, float radius, float height, IntPtr offsetMatrix);
            public static NewtonCreateCylinder CreateCylinder;

            [UnmanagedFunctionPointer(CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
            public delegate IntPtr NewtonCreateChamferCylinder(IntPtr newtonWorld, float radius, float height, IntPtr offsetMatrix);
            public static NewtonCreateChamferCylinder CreateChamferCylinder;

            [UnmanagedFunctionPointer(CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
            public delegate IntPtr NewtonCreateConvexHull(IntPtr newtonWorld, int count, [In, MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 1)] Vector3[] vertexCloud, int strideInBytes, float tolerance, IntPtr offsetMatrix);
            public static NewtonCreateConvexHull CreateConvexHull;
        }

        public static IntPtr CreateSphere(IntPtr newtonWorld, float radiusX, float radiusY, float radiusZ)
        {
            return Private.CreateSphere(newtonWorld, radiusX, radiusY, radiusZ, IntPtr.Zero);
        }

        public static IntPtr CreateSphere(IntPtr newtonWorld, float radiusX, float radiusY, float radiusZ, [In] ref Matrix4 offsetMatrix)
        {
            GCHandle handle = GCHandle.Alloc(offsetMatrix, GCHandleType.Pinned);
            try
            {
                return Private.CreateSphere(newtonWorld, radiusX, radiusY, radiusZ, handle.AddrOfPinnedObject());
            }
            finally
            {
                handle.Free();
            }
        }

        public static IntPtr CreateBox(IntPtr newtonWorld, float dx, float dy, float dz)
        {
            return Private.CreateBox(newtonWorld, dx, dy, dz, IntPtr.Zero);
        }

        public static IntPtr CreateBox(IntPtr newtonWorld, float dx, float dy, float dz, [In] ref Matrix4 offsetMatrix)
        {
            GCHandle handle = GCHandle.Alloc(offsetMatrix, GCHandleType.Pinned);
            try
            {
                return Private.CreateBox(newtonWorld, dx, dy, dz, handle.AddrOfPinnedObject());
            }
            finally
            {
                handle.Free();
            }
        }

        public static IntPtr CreateCone(IntPtr newtonWorld, float radius, float height)
        {
            return Private.CreateCone(newtonWorld, radius, height, IntPtr.Zero);
        }

        public static IntPtr CreateCone(IntPtr newtonWorld, float radius, float height, [In] ref Matrix4 offsetMatrix)
        {
            GCHandle handle = GCHandle.Alloc(offsetMatrix, GCHandleType.Pinned);
            try
            {
                return Private.CreateCone(newtonWorld, radius, height, handle.AddrOfPinnedObject());
            }
            finally
            {
                handle.Free();
            }
        }

        public static IntPtr CreateCapsule(IntPtr newtonWorld, float radius, float height)
        {
            return Private.CreateCapsule(newtonWorld, radius, height, IntPtr.Zero);
        }

        public static IntPtr CreateCapsule(IntPtr newtonWorld, float radius, float height, [In] ref Matrix4 offsetMatrix)
        {
            GCHandle handle = GCHandle.Alloc(offsetMatrix, GCHandleType.Pinned);
            try
            {
                return Private.CreateCapsule(newtonWorld, radius, height, handle.AddrOfPinnedObject());
            }
            finally
            {
                handle.Free();
            }
        }

        public static IntPtr CreateCylinder(IntPtr newtonWorld, float radius, float height)
        {
            return Private.CreateCylinder(newtonWorld, radius, height, IntPtr.Zero);
        }

        public static IntPtr CreateCylinder(IntPtr newtonWorld, float radius, float height, [In] ref Matrix4 offsetMatrix)
        {
            GCHandle handle = GCHandle.Alloc(offsetMatrix, GCHandleType.Pinned);
            try
            {
                return Private.CreateCylinder(newtonWorld, radius, height, handle.AddrOfPinnedObject());
            }
            finally
            {
                handle.Free();
            }
        }

        public static IntPtr CreateChamferCylinder(IntPtr newtonWorld, float radius, float height)
        {
            return Private.CreateChamferCylinder(newtonWorld, radius, height, IntPtr.Zero);
        }

        public static IntPtr CreateChamferCylinder(IntPtr newtonWorld, float radius, float height, [In] ref Matrix4 offsetMatrix)
        {
            GCHandle handle = GCHandle.Alloc(offsetMatrix, GCHandleType.Pinned);
            try
            {
                return Private.CreateChamferCylinder(newtonWorld, radius, height, handle.AddrOfPinnedObject());
            }
            finally
            {
                handle.Free();
            }
        }

        public static IntPtr CreateConvexHull(IntPtr newtonWorld, int count, Vector3[] vertexCloud, int strideInBytes, float tolerance)
        {
            return Private.CreateConvexHull(newtonWorld, count, vertexCloud, strideInBytes, tolerance, IntPtr.Zero);
        }

        public static IntPtr CreateConvexHull(IntPtr newtonWorld, int count, Vector3[] vertexCloud, int strideInBytes, float tolerance, [In] ref Matrix4 offsetMatrix)
        {
            GCHandle handle = GCHandle.Alloc(offsetMatrix, GCHandleType.Pinned);
            try
            {
                return Private.CreateConvexHull(newtonWorld, count, vertexCloud, strideInBytes, tolerance, handle.AddrOfPinnedObject());
            }
            finally
            {
                handle.Free();
            }
        }

        // NEWTON_API NewtonCollision* NewtonCreateConvexHullFromMesh (const NewtonWorld* newtonWorld, const NewtonMesh* mesh, dFloat tolerance);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public delegate IntPtr NewtonCreateConvexHullModifier(IntPtr newtonWorld, IntPtr convexHullCollision);
        public static NewtonCreateConvexHullModifier CreateConvexHullModifier;

        [UnmanagedFunctionPointer(CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public delegate void NewtonConvexHullModifierGetMatrix(IntPtr convexHullCollision, out Matrix4 matrix);
        public static NewtonConvexHullModifierGetMatrix ConvexHullModifierGetMatrix;

        [UnmanagedFunctionPointer(CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public delegate void NewtonConvexHullModifierSetMatrix(IntPtr convexHullCollision, [In] ref Matrix4 matrix);
        public static NewtonConvexHullModifierSetMatrix ConvexHullModifierSetMatrix;

        [UnmanagedFunctionPointer(CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public delegate int NewtonCollisionIsTriggerVolume(IntPtr convexHullCollision);
        public static NewtonCollisionIsTriggerVolume CollisionIsTriggerVolume;

        [UnmanagedFunctionPointer(CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public delegate void NewtonCollisionSetAsTriggerVolume(IntPtr convexHullCollision, int trigger);
        public static NewtonCollisionSetAsTriggerVolume CollisionSetAsTriggerVolume;

        [UnmanagedFunctionPointer(CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public delegate void NewtonConvexCollisionSetUserID(IntPtr convexCollision, uint id);
        public static NewtonConvexCollisionSetUserID ConvexCollisionSetUserID;

        [UnmanagedFunctionPointer(CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public delegate uint NewtonConvexCollisionGetUserID(IntPtr convexCollision);
        public static NewtonConvexCollisionGetUserID ConvexCollisionGetUserID;
        
        [UnmanagedFunctionPointer(CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public delegate float NewtonConvexCollisionCalculateVolume(IntPtr convexCollision);
        public static NewtonConvexCollisionCalculateVolume ConvexCollisionCalculateVolume;

        [UnmanagedFunctionPointer(CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public delegate void NewtonConvexCollisionCalculateInertialMatrix(IntPtr convexCollision, out Vector3 inertia, out Vector3 origin);
        public static NewtonConvexCollisionCalculateInertialMatrix ConvexCollisionCalculateInertialMatrix;
        
        [UnmanagedFunctionPointer(CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public delegate void NewtonCollisionMakeUnique(IntPtr newtonWorld, IntPtr collision);
        public static NewtonCollisionMakeUnique CollisionMakeUnique;

        [UnmanagedFunctionPointer(CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public delegate void NewtonReleaseCollision(IntPtr newtonWorld, IntPtr collision);
        public static NewtonReleaseCollision ReleaseCollision;

        [UnmanagedFunctionPointer(CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public delegate int NewtonAddCollisionReference(IntPtr collision);
        public static NewtonAddCollisionReference AddCollisionReference;

        // **********************************************************************************************
        //
        // complex collision primitives creation functions
        //
        // **********************************************************************************************

        [UnmanagedFunctionPointer(CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public delegate IntPtr NewtonCreateCompoundCollision(IntPtr newtonWorld, int count, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 1)] IntPtr[] collisionPrimitiveArray);
        public static NewtonCreateCompoundCollision CreateCompoundCollision;

        //NEWTON_API NewtonCollision* NewtonCreateCompoundCollisionFromMesh (const NewtonWorld* newtonWorld, const NewtonMesh* mesh, dFloat concavity, int maxShapeCount);

        //NEWTON_API NewtonCollision* NewtonCreateUserMeshCollision (const NewtonWorld* newtonWorld, const dFloat *minBox, 
        //    const dFloat *maxBox, void *userData, NewtonUserMeshCollisionCollideCallback collideCallback, 
        //    NewtonUserMeshCollisionRayHitCallback rayHitCallback, NewtonUserMeshCollisionDestroyCallback destroyCallback,
        //    NewtonUserMeshCollisionGetCollisionInfo getInfoCallback, NewtonUserMeshCollisionGetFacesInAABB facesInAABBCallback);

        //NEWTON_API NewtonCollision* NewtonCreateSceneCollision (const NewtonWorld* newtonWorld, dFloat size);
        //NEWTON_API NewtonSceneProxy* NewtonSceneCollisionCreateProxy (NewtonCollision* scene, NewtonCollision* collision);
        //NEWTON_API void NewtonSceneCollisionDestroyProxy (NewtonCollision* scene, NewtonSceneProxy* Proxy);
        //NEWTON_API void NewtonSceneProxySetMatrix (NewtonSceneProxy* proxy, const dFloat* matrix);
        //NEWTON_API void NewtonSceneProxyGetMatrix (NewtonSceneProxy* proxy, dFloat* matrix);

        // **********************************************************************************************
        //
        //	Collision serialization functions
        //
        // **********************************************************************************************

        [UnmanagedFunctionPointer(CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public delegate IntPtr NewtonCreateCollisionFromSerialization(IntPtr newtonWorld, NewtonDeserialize deserializeFunction, IntPtr serializeHandle);
        public static NewtonCreateCollisionFromSerialization CreateCollisionFromSerialization;

        [UnmanagedFunctionPointer(CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public delegate void NewtonCollisionSerialize(IntPtr collision, NewtonSerialize serializeFunction, IntPtr serializeHandle);
        public static NewtonCollisionSerialize CollisionSerialize;

        [UnmanagedFunctionPointer(CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public delegate void NewtonCollisionGetInfo(IntPtr collision, out NewtonCollisionInfoRecord collisionInfo);
        public static NewtonCollisionGetInfo CollisionGetInfo;

        // **********************************************************************************************
        //
        // Static collision shapes functions
        //
        // **********************************************************************************************

        [UnmanagedFunctionPointer(CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public delegate IntPtr NewtonCreateHeightFieldCollision(IntPtr newtonWorld, int width, int height, int gridsDiagonals, ushort[] elevationMap, sbyte[] attributeMap, float horizontalScale, float verticalScale);
        public static NewtonCreateHeightFieldCollision CreateHeightFieldCollision;

        [UnmanagedFunctionPointer(CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public delegate IntPtr NewtonCreateTreeCollision(IntPtr newtonWorld);
        public static NewtonCreateTreeCollision CreateTreeCollision;

        //NEWTON_API void NewtonTreeCollisionSetUserRayCastCallback (const NewtonCollision* treeCollision, NewtonCollisionTreeRayCastCallback rayHitCallback);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public delegate void NewtonTreeCollisionBeginBuild(IntPtr treeCollision);
        public static NewtonTreeCollisionBeginBuild TreeCollisionBeginBuild;

        [UnmanagedFunctionPointer(CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public delegate void NewtonTreeCollisionAddFace(IntPtr treeCollision, int vertexCount, [In, MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 1)] Vector3[] vertexPtr, int strideInBytes, int faceAttribute);
        public static NewtonTreeCollisionAddFace TreeCollisionAddFace;

        [UnmanagedFunctionPointer(CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public delegate void NewtonTreeCollisionEndBuild(IntPtr treeCollision, int optimize);
        public static NewtonTreeCollisionEndBuild TreeCollisionEndBuild;

        //NEWTON_API int NewtonTreeCollisionGetFaceAtribute (const NewtonCollision* treeCollision, const int* faceIndexArray); 
        //NEWTON_API void NewtonTreeCollisionSetFaceAtribute (const NewtonCollision* treeCollision, const int* faceIndexArray, int attribute); 
        //NEWTON_API int NewtonTreeCollisionGetVertexListIndexListInAABB (const NewtonCollision* treeCollision, const dFloat* p0, const dFloat* p1, const dFloat** vertexArray, int* vertexCount, int* vertexStrideInBytes, const int* indexList, int maxIndexCount, const int* faceAttribute); 


        //NEWTON_API void NewtonStaticCollisionSetDebugCallback (const NewtonCollision* staticCollision, NewtonTreeCollisionCallback userCallback);

        // **********************************************************************************************
        //
        // General purpose collision library functions
        //
        // **********************************************************************************************

        //NEWTON_API int NewtonCollisionPointDistance (const NewtonWorld* newtonWorld, const dFloat *point,
        //    const NewtonCollision* collision, const dFloat* matrix, dFloat* contact, dFloat* normal);

        //NEWTON_API int NewtonCollisionClosestPoint (const NewtonWorld* newtonWorld, 
        //    const NewtonCollision* collisionA, const dFloat* matrixA, const NewtonCollision* collisionB, const dFloat* matrixB,
        //    dFloat* contactA, dFloat* contactB, dFloat* normalAB);

        //NEWTON_API int NewtonCollisionCollide (const NewtonWorld* newtonWorld, int maxSize,
        //    const NewtonCollision* collisionA, const dFloat* matrixA, const NewtonCollision* collisionB, const dFloat* matrixB,
        //    dFloat* contacts, dFloat* normals, dFloat* penetration);

        //NEWTON_API int NewtonCollisionCollideContinue (const NewtonWorld* newtonWorld, int maxSize, const dFloat timestep, 
        //    const NewtonCollision* collisionA, const dFloat* matrixA, const dFloat* velocA, const dFloat* omegaA, 
        //    const NewtonCollision* collisionB, const dFloat* matrixB, const dFloat* velocB, const dFloat* omegaB, 
        //    dFloat* timeOfImpact, dFloat* contacts, dFloat* normals, dFloat* penetration);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public delegate float NewtonCollisionSupportVertex(IntPtr collision, [In] ref Vector3 dir, out Vector3 vertex);
        public static NewtonCollisionSupportVertex CollisionSupportVertex;

        [UnmanagedFunctionPointer(CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public delegate float NewtonCollisionRayCast(IntPtr collision, [In] ref Vector3 p0, [In] ref Vector3 p1, out Vector3 normals, out int attribute);
        public static NewtonCollisionRayCast CollisionRayCast;
        
        [UnmanagedFunctionPointer(CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public delegate void NewtonCollisionCalculateAABB(IntPtr collision, [In] ref Matrix4 matrix, out Vector3 p0, out Vector3 p1);
        public static NewtonCollisionCalculateAABB CollisionCalculateAABB;

        [UnmanagedFunctionPointer(CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public delegate void NewtonCollisionForEachPolygonDo(IntPtr collision, [In] ref Matrix4 matrix, NewtonCollisionIterator callback, IntPtr useData);
        public static NewtonCollisionForEachPolygonDo CollisionForEachPolygonDo;

        // **********************************************************************************************
        //
        // transforms utility functions
        //
        // **********************************************************************************************

        [UnmanagedFunctionPointer(CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public delegate void NewtonGetEulerAngle([In] ref Matrix4 matrix, out Vector3 eulersAngles);
        public static NewtonGetEulerAngle GetEulerAngle;

        [UnmanagedFunctionPointer(CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public delegate void NewtonSetEulerAngle([In] ref Vector3 eulersAngles, out Matrix4 matrix);
        public static NewtonSetEulerAngle SetEulerAngle;

        [UnmanagedFunctionPointer(CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public delegate float NewtonCalculateSpringDamperAcceleration(float dt, float ks, float x, float kd, float s);
        public static NewtonCalculateSpringDamperAcceleration CalculateSpringDamperAcceleration;

        // **********************************************************************************************
        //
        // body manipulation functions
        //
        // **********************************************************************************************

        [UnmanagedFunctionPointer(CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public delegate IntPtr NewtonCreateBody(IntPtr newtonWorld, IntPtr collision);
        public static NewtonCreateBody CreateBody;

        [UnmanagedFunctionPointer(CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public delegate void NewtonDestroyBody(IntPtr newtonWorld, IntPtr body);
        public static NewtonDestroyBody DestroyBody;

        [UnmanagedFunctionPointer(CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public delegate void NewtonBodyAddForce(IntPtr body, [In] ref Vector3 force);
        public static NewtonBodyAddForce BodyAddForce;

        [UnmanagedFunctionPointer(CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public delegate void NewtonBodyAddTorque(IntPtr body, [In] ref Vector3 torque);
        public static NewtonBodyAddTorque BodyAddTorque;

        [UnmanagedFunctionPointer(CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public delegate void NewtonBodyCalculateInverseDynamicsForce(IntPtr body, float timestep, [In] ref Vector3 desiredVeloc, out Vector3 forceOut);
        public static NewtonBodyCalculateInverseDynamicsForce BodyCalculateInverseDynamicsForce;

        [UnmanagedFunctionPointer(CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public delegate void NewtonBodySetMatrix(IntPtr body, [In] ref Matrix4 matrix);
        public static NewtonBodySetMatrix BodySetMatrix;

        [UnmanagedFunctionPointer(CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public delegate void NewtonBodySetMatrixRecursive(IntPtr body, [In] ref Matrix4 matrix);
        public static NewtonBodySetMatrixRecursive BodySetMatrixRecursive;

        [UnmanagedFunctionPointer(CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public delegate void NewtonBodySetMassMatrix(IntPtr body, float mass, float Ixx, float Iyy, float Izz);
        public static NewtonBodySetMassMatrix BodySetMassMatrix;

        [UnmanagedFunctionPointer(CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public delegate void NewtonBodySetMaterialGroupID(IntPtr body, int id);
        public static NewtonBodySetMaterialGroupID BodySetMaterialGroupID;

        [UnmanagedFunctionPointer(CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public delegate void NewtonBodySetContinuousCollisionMode(IntPtr body, uint state);
        public static NewtonBodySetContinuousCollisionMode BodySetContinuousCollisionMode;

        [UnmanagedFunctionPointer(CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public delegate void NewtonBodySetJointRecursiveCollision(IntPtr body, uint state);
        public static NewtonBodySetJointRecursiveCollision BodySetJointRecursiveCollision;

        [UnmanagedFunctionPointer(CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public delegate void NewtonBodySetOmega(IntPtr body, [In] ref Vector3 omega);
        public static NewtonBodySetOmega BodySetOmega;

        [UnmanagedFunctionPointer(CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public delegate void NewtonBodySetVelocity(IntPtr body, [In] ref Vector3 velocity);
        public static NewtonBodySetVelocity BodySetVelocity;

        [UnmanagedFunctionPointer(CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public delegate void NewtonBodySetForce(IntPtr body, [In] ref Vector3 force);
        public static NewtonBodySetForce BodySetForce;

        [UnmanagedFunctionPointer(CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public delegate void NewtonBodySetTorque(IntPtr body, [In] ref Vector3 torque);
        public static NewtonBodySetTorque BodySetTorque;

        [UnmanagedFunctionPointer(CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public delegate void NewtonBodySetCentreOfMass(IntPtr pNewtonBody, [In] ref Vector3 com);
        public static NewtonBodySetCentreOfMass BodySetCentreOfMass;

        [UnmanagedFunctionPointer(CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public delegate void NewtonBodySetLinearDamping(IntPtr pNewtonBody, float linearDamp);
        public static NewtonBodySetLinearDamping BodySetLinearDamping;

        [UnmanagedFunctionPointer(CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public delegate void NewtonBodySetAngularDamping(IntPtr body, [In] ref Vector3 angularDamp);
        public static NewtonBodySetAngularDamping BodySetAngularDamping;

        [UnmanagedFunctionPointer(CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public delegate void NewtonBodySetUserData(IntPtr body, IntPtr userData);
        public static NewtonBodySetUserData BodySetUserData;

        [UnmanagedFunctionPointer(CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public delegate void NewtonBodySetCollision(IntPtr body, IntPtr collision);
        public static NewtonBodySetCollision BodySetCollision;
        
        [UnmanagedFunctionPointer(CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public delegate int NewtonBodyGetSleepState(IntPtr body);
        public static NewtonBodyGetSleepState BodyGetSleepState;

        [UnmanagedFunctionPointer(CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public delegate int NewtonBodyGetAutoSleep(IntPtr body);
        public static NewtonBodyGetAutoSleep BodyGetAutoSleep;

        [UnmanagedFunctionPointer(CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public delegate void NewtonBodySetAutoSleep(IntPtr body, int state);
        public static NewtonBodySetAutoSleep BodySetAutoSleep;

        [UnmanagedFunctionPointer(CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public delegate int NewtonBodyGetFreezeState(IntPtr body);
        public static NewtonBodyGetFreezeState BodyGetFreezeState;

        [UnmanagedFunctionPointer(CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public delegate void NewtonBodySetFreezeState(IntPtr body, int state);
        public static NewtonBodySetFreezeState BodySetFreezeState;

        [UnmanagedFunctionPointer(CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public delegate void NewtonBodySetTransformCallback(IntPtr newtonBody, NewtonSetTransform callback);
        public static NewtonBodySetTransformCallback BodySetTransformCallback;

        [UnmanagedFunctionPointer(CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public delegate void NewtonBodySetDestructorCallback(IntPtr newtonBody, NewtonBodyDestructor callback);
        public static NewtonBodySetDestructorCallback BodySetDestructorCallback;

        [UnmanagedFunctionPointer(CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public delegate void NewtonBodySetForceAndTorqueCallback(IntPtr newtonBody, NewtonApplyForceAndTorque callback);
        public static NewtonBodySetForceAndTorqueCallback BodySetForceAndTorqueCallback;

        //NEWTON_API NewtonApplyForceAndTorque NewtonBodyGetForceAndTorqueCallback (const NewtonBody* body);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public delegate IntPtr NewtonBodyGetUserData(IntPtr body);
        public static NewtonBodyGetUserData BodyGetUserData;

        //NEWTON_API NewtonWorld* NewtonBodyGetWorld (const NewtonBody* body);

        //[UnmanagedFunctionPointer(CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        //public delegate IntPtr NewtonBodyGetCollision(IntPtr body);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public delegate int NewtonBodyGetMaterialGroupID(IntPtr body);
        public static NewtonBodyGetMaterialGroupID BodyGetMaterialGroupID;

        [UnmanagedFunctionPointer(CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public delegate int NewtonBodyGetContinuousCollisionMode(IntPtr body);
        public static NewtonBodyGetContinuousCollisionMode BodyGetContinuousCollisionMode;

        [UnmanagedFunctionPointer(CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public delegate int NewtonBodyGetJointRecursiveCollision(IntPtr body);
        public static NewtonBodyGetJointRecursiveCollision BodyGetJointRecursiveCollision;

        [UnmanagedFunctionPointer(CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public delegate void NewtonBodyGetMatrix(IntPtr body, out Matrix4 matrix);
        public static NewtonBodyGetMatrix BodyGetMatrix;

        [UnmanagedFunctionPointer(CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public delegate void NewtonBodyGetRotation(IntPtr body, out Quaternion rotation);
        public static NewtonBodyGetRotation BodyGetRotation;

        [UnmanagedFunctionPointer(CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public delegate void NewtonBodyGetMassMatrix(IntPtr body, out float mass, out float Ixx, out float Iyy, out float Izz);
        public static NewtonBodyGetMassMatrix BodyGetMassMatrix;

        [UnmanagedFunctionPointer(CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public delegate void NewtonBodyGetInvMass(IntPtr body, out float invMass, out float invIxx, out float invIyy, out float invIzz);
        public static NewtonBodyGetInvMass BodyGetInvMass;

        [UnmanagedFunctionPointer(CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public delegate void NewtonBodyGetOmega(IntPtr body, out Vector3 vector);
        public static NewtonBodyGetOmega BodyGetOmega;

        [UnmanagedFunctionPointer(CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public delegate void NewtonBodyGetVelocity(IntPtr body, out Vector3 vector);
        public static NewtonBodyGetVelocity BodyGetVelocity;

        [UnmanagedFunctionPointer(CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public delegate void NewtonBodyGetForce(IntPtr body, out Vector3 vector);
        public static NewtonBodyGetForce BodyGetForce;

        [UnmanagedFunctionPointer(CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public delegate void NewtonBodyGetTorque(IntPtr body, out Vector3 vector);
        public static NewtonBodyGetTorque BodyGetTorque;

        [UnmanagedFunctionPointer(CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public delegate void NewtonBodyGetForceAcc(IntPtr body, out Vector3 vector);
        public static NewtonBodyGetForceAcc BodyGetForceAcc;

        [UnmanagedFunctionPointer(CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public delegate void NewtonBodyGetTorqueAcc(IntPtr body, out Vector3 vector);
        public static NewtonBodyGetTorqueAcc BodyGetTorqueAcc;

        [UnmanagedFunctionPointer(CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public delegate void NewtonBodyGetCentreOfMass(IntPtr body, out Vector3 com);
        public static NewtonBodyGetCentreOfMass BodyGetCentreOfMass;

        [UnmanagedFunctionPointer(CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public delegate float NewtonBodyGetLinearDamping(IntPtr body);
        public static NewtonBodyGetLinearDamping BodyGetLinearDamping;

        [UnmanagedFunctionPointer(CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public delegate void NewtonBodyGetAngularDamping(IntPtr body, out Vector3 vector);
        public static NewtonBodyGetAngularDamping BodyGetAngularDamping;

        [UnmanagedFunctionPointer(CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public delegate void NewtonBodyGetAABB(IntPtr body, out Vector3 p0, out Vector3 p1);
        public static NewtonBodyGetAABB BodyGetAABB;

        //NEWTON_API NewtonJoint* NewtonBodyGetFirstJoint (const NewtonBody* body);
        //NEWTON_API NewtonJoint* NewtonBodyGetNextJoint (const NewtonBody* body, const NewtonJoint* joint);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
	    public delegate IntPtr NewtonBodyGetFirstContactJoint(IntPtr body);
        public static NewtonBodyGetFirstContactJoint BodyGetFirstContactJoint;

        [UnmanagedFunctionPointer(CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public delegate IntPtr NewtonBodyGetNextContactJoint(IntPtr body, IntPtr contactJoint);
        public static NewtonBodyGetNextContactJoint BodyGetNextContactJoint;

        [UnmanagedFunctionPointer(CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public delegate IntPtr NewtonContactJointGetFirstContact(IntPtr contactJoint);
        public static NewtonContactJointGetFirstContact ContactJointGetFirstContact;
        
        [UnmanagedFunctionPointer(CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public delegate IntPtr NewtonContactJointGetNextContact(IntPtr contactJoint, IntPtr contact);
        public static NewtonContactJointGetNextContact ContactJointGetNextContact;

        [UnmanagedFunctionPointer(CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public delegate int NewtonContactJointGetContactCount(IntPtr contactJoint);
        public static NewtonContactJointGetContactCount ContactJointGetContactCount;

        [UnmanagedFunctionPointer(CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public delegate void NewtonContactJointRemoveContact(IntPtr contactJoint, IntPtr contact);
        public static NewtonContactJointRemoveContact ContactJointRemoveContact;

        [UnmanagedFunctionPointer(CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public delegate IntPtr NewtonContactGetMaterial(IntPtr contact);
        public static NewtonContactGetMaterial ContactGetMaterial;

        //NEWTON_API void  NewtonBodyAddBuoyancyForce (const NewtonBody* body, dFloat fluidDensity, 
        //    dFloat fluidLinearViscosity, dFloat fluidAngularViscosity, 
        //    const dFloat* gravityVector, NewtonGetBuoyancyPlane buoyancyPlane, void *context);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public delegate void NewtonBodyAddImpulse(IntPtr body, [In] ref Vector3 pointDeltaVeloc, [In] ref Vector3 pointPosit);
        public static NewtonBodyAddImpulse BodyAddImpulse;

        // **********************************************************************************************
        //
        // Common joint functions
        //
        // **********************************************************************************************

        [UnmanagedFunctionPointer(CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public delegate IntPtr NewtonJointGetUserData(IntPtr joint);
        public static NewtonJointGetUserData JointGetUserData;

        [UnmanagedFunctionPointer(CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public delegate void NewtonJointSetUserData(IntPtr joint, IntPtr userData);
        public static NewtonJointSetUserData JointSetUserData;

        //NEWTON_API NewtonBody* NewtonJointGetBody0 (const NewtonJoint* joint);
        //NEWTON_API NewtonBody* NewtonJointGetBody1 (const NewtonJoint* joint);

        //NEWTON_API void NewtonJointGetInfo  (const NewtonJoint* joint, NewtonJointRecord* info);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public delegate int NewtonJointGetCollisionState(IntPtr joint);
        public static NewtonJointGetCollisionState JointGetCollisionState;

        [UnmanagedFunctionPointer(CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public delegate void NewtonJointSetCollisionState(IntPtr joint, int state);
        public static NewtonJointSetCollisionState JointSetCollisionState;

        //NEWTON_API dFloat NewtonJointGetStiffness (const NewtonJoint* joint);
        //NEWTON_API void NewtonJointSetStiffness (const NewtonJoint* joint, dFloat state);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public delegate void NewtonDestroyJoint(IntPtr world, IntPtr joint);
        public static NewtonDestroyJoint DestroyJoint;

        [UnmanagedFunctionPointer(CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public delegate void NewtonJointSetDestructor(IntPtr joint, NewtonConstraintDestructor destructor);
        public static NewtonJointSetDestructor JointSetDestructor;

        // **********************************************************************************************
        //
        // Ball and Socket joint functions
        //
        // **********************************************************************************************

        //NEWTON_API NewtonJoint* NewtonConstraintCreateBall (const NewtonWorld* newtonWorld, const dFloat* pivotPoint, 
        //                                                    const NewtonBody* childBody, const NewtonBody* parentBody);
        //NEWTON_API void NewtonBallSetUserCallback (const NewtonJoint* ball, NewtonBallCallBack callback);
        //NEWTON_API void NewtonBallGetJointAngle (const NewtonJoint* ball, dFloat* angle);
        //NEWTON_API void NewtonBallGetJointOmega (const NewtonJoint* ball, dFloat* omega);
        //NEWTON_API void NewtonBallGetJointForce (const NewtonJoint* ball, dFloat* force);
        //NEWTON_API void NewtonBallSetConeLimits (const NewtonJoint* ball, const dFloat* pin, dFloat maxConeAngle, dFloat maxTwistAngle);

        // **********************************************************************************************
        //
        // Hinge joint functions
        //
        // **********************************************************************************************

        //NEWTON_API NewtonJoint* NewtonConstraintCreateHinge (const NewtonWorld* newtonWorld, 
        //    const dFloat* pivotPoint, const dFloat* pinDir, 
        //    const NewtonBody* childBody, const NewtonBody* parentBody);

        //NEWTON_API void NewtonHingeSetUserCallback (const NewtonJoint* hinge, NewtonHingeCallBack callback);
        //NEWTON_API dFloat NewtonHingeGetJointAngle (const NewtonJoint* hinge);
        //NEWTON_API dFloat NewtonHingeGetJointOmega (const NewtonJoint* hinge);
        //NEWTON_API void NewtonHingeGetJointForce (const NewtonJoint* hinge, dFloat* force);
        //NEWTON_API dFloat NewtonHingeCalculateStopAlpha (const NewtonJoint* hinge, const NewtonHingeSliderUpdateDesc* desc, dFloat angle);

        // **********************************************************************************************
        //
        // Slider joint functions
        //
        // **********************************************************************************************

        //NEWTON_API NewtonJoint* NewtonConstraintCreateSlider (const NewtonWorld* newtonWorld, 
        //    const dFloat* pivotPoint, const dFloat* pinDir, 
        //    const NewtonBody* childBody, const NewtonBody* parentBody);
        //NEWTON_API void NewtonSliderSetUserCallback (const NewtonJoint* slider, NewtonSliderCallBack callback);
        //NEWTON_API dFloat NewtonSliderGetJointPosit (const NewtonJoint* slider);
        //NEWTON_API dFloat NewtonSliderGetJointVeloc (const NewtonJoint* slider);
        //NEWTON_API void NewtonSliderGetJointForce (const NewtonJoint* slider, dFloat* force);
        //NEWTON_API dFloat NewtonSliderCalculateStopAccel (const NewtonJoint* slider, const NewtonHingeSliderUpdateDesc* desc, dFloat position);

        // **********************************************************************************************
        //
        // Corkscrew joint functions
        //
        // **********************************************************************************************

        //NEWTON_API NewtonJoint* NewtonConstraintCreateCorkscrew (const NewtonWorld* newtonWorld, 
        //    const dFloat* pivotPoint, const dFloat* pinDir, 
        //    const NewtonBody* childBody, const NewtonBody* parentBody);
        //NEWTON_API void NewtonCorkscrewSetUserCallback (const NewtonJoint* corkscrew, NewtonCorkscrewCallBack callback);
        //NEWTON_API dFloat NewtonCorkscrewGetJointPosit (const NewtonJoint* corkscrew);
        //NEWTON_API dFloat NewtonCorkscrewGetJointAngle (const NewtonJoint* corkscrew);
        //NEWTON_API dFloat NewtonCorkscrewGetJointVeloc (const NewtonJoint* corkscrew);
        //NEWTON_API dFloat NewtonCorkscrewGetJointOmega (const NewtonJoint* corkscrew);
        //NEWTON_API void NewtonCorkscrewGetJointForce (const NewtonJoint* corkscrew, dFloat* force);
        //NEWTON_API dFloat NewtonCorkscrewCalculateStopAlpha (const NewtonJoint* corkscrew, const NewtonHingeSliderUpdateDesc* desc, dFloat angle);
        //NEWTON_API dFloat NewtonCorkscrewCalculateStopAccel (const NewtonJoint* corkscrew, const NewtonHingeSliderUpdateDesc* desc, dFloat position);

        // **********************************************************************************************
        //
        // Universal joint functions
        //
        // **********************************************************************************************

        //NEWTON_API NewtonJoint* NewtonConstraintCreateUniversal (const NewtonWorld* newtonWorld, 
        //    const dFloat* pivotPoint, const dFloat* pinDir0, const dFloat* pinDir1, 
        //    const NewtonBody* childBody, const NewtonBody* parentBody);
        //NEWTON_API void NewtonUniversalSetUserCallback (const NewtonJoint* universal, NewtonUniversalCallBack callback);
        //NEWTON_API dFloat NewtonUniversalGetJointAngle0 (const NewtonJoint* universal);
        //NEWTON_API dFloat NewtonUniversalGetJointAngle1 (const NewtonJoint* universal);
        //NEWTON_API dFloat NewtonUniversalGetJointOmega0 (const NewtonJoint* universal);
        //NEWTON_API dFloat NewtonUniversalGetJointOmega1 (const NewtonJoint* universal);
        //NEWTON_API void NewtonUniversalGetJointForce (const NewtonJoint* universal, dFloat* force);
        //NEWTON_API dFloat NewtonUniversalCalculateStopAlpha0 (const NewtonJoint* universal, const NewtonHingeSliderUpdateDesc* desc, dFloat angle);
        //NEWTON_API dFloat NewtonUniversalCalculateStopAlpha1 (const NewtonJoint* universal, const NewtonHingeSliderUpdateDesc* desc, dFloat angle);

        // **********************************************************************************************
        //
        // Up vector joint functions
        //
        // **********************************************************************************************

        //NEWTON_API NewtonJoint* NewtonConstraintCreateUpVector (const NewtonWorld* newtonWorld, const dFloat* pinDir, const NewtonBody* body); 
        //NEWTON_API void NewtonUpVectorGetPin (const NewtonJoint* upVector, dFloat *pin);
        //NEWTON_API void NewtonUpVectorSetPin (const NewtonJoint* upVector, const dFloat *pin);

        // **********************************************************************************************
        //
        // User defined bilateral Joint
        //
        // **********************************************************************************************

        [UnmanagedFunctionPointer(CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public delegate IntPtr NewtonConstraintCreateUserJoint(IntPtr world, int maxDOF, NewtonUserBilateralCallBack callback, NewtonUserBilateralGetInfoCallBack getInfo, IntPtr childBody, IntPtr parentBody);
        public static NewtonConstraintCreateUserJoint ConstraintCreateUserJoint;

        [UnmanagedFunctionPointer(CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public delegate void NewtonUserJointSetFeedbackCollectorCallback(IntPtr joint, NewtonUserBilateralCallBack getFeedback);
        public static NewtonUserJointSetFeedbackCollectorCallback UserJointSetFeedbackCollectorCallback;

        [UnmanagedFunctionPointer(CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public delegate void NewtonUserJointAddLinearRow(IntPtr joint, [In] ref Vector3 pivot0, [In] ref Vector3 pivot1, [In] ref Vector3 dir);
        public static NewtonUserJointAddLinearRow UserJointAddLinearRow;

        [UnmanagedFunctionPointer(CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public delegate void NewtonUserJointAddAngularRow(IntPtr joint, float relativeAngle, [In] ref Vector3 dir);
        public static NewtonUserJointAddAngularRow UserJointAddAngularRow;

        [UnmanagedFunctionPointer(CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public delegate void NewtonUserJointAddGeneralRow(IntPtr joint, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 2)] Vector3[] jacobian0, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 2)] Vector3[] jacobian1);
        public static NewtonUserJointAddGeneralRow UserJointAddGeneralRow;

        [UnmanagedFunctionPointer(CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public delegate void NewtonUserJointSetRowMinimumFriction(IntPtr joint, float friction);
        public static NewtonUserJointSetRowMinimumFriction UserJointSetRowMinimumFriction;

        [UnmanagedFunctionPointer(CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public delegate void NewtonUserJointSetRowMaximumFriction(IntPtr joint, float stiffness);
        public static NewtonUserJointSetRowMaximumFriction UserJointSetRowMaximumFriction;

        [UnmanagedFunctionPointer(CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public delegate void NewtonUserJointSetRowAcceleration(IntPtr joint, float acceleration);
        public static NewtonUserJointSetRowAcceleration UserJointSetRowAcceleration;

        //NEWTON_API void NewtonUserJointSetRowSpringDamperAcceleration (const NewtonJoint* joint, dFloat springK, dFloat springD);
        
        [UnmanagedFunctionPointer(CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        public delegate void NewtonUserJointSetRowStiffness(IntPtr joint, float stiffness);
        public static NewtonUserJointSetRowStiffness UserJointSetRowStiffness;

        //NEWTON_API dFloat NewtonUserJointGetRowForce (const NewtonJoint* joint, int row);

        // **********************************************************************************************
        //
        // Mesh joint functions
        //
        // **********************************************************************************************

        //NEWTON_API NewtonMesh* NewtonMeshCreate();
        //NEWTON_API NewtonMesh* NewtonMeshCreateFromCollision(const NewtonCollision* collision);
        //NEWTON_API void NewtonMeshDestroy(const NewtonMesh*mesh);

        //NEWTON_API void NewtonMeshCalculateVertexNormals(const NewtonMesh* mesh, dFloat angleInRadians);
        //NEWTON_API void NewtonMeshApplySphericalMapping(const NewtonMesh* mesh, int material);
        //NEWTON_API void NewtonMeshApplyBoxMapping(const NewtonMesh* mesh, int front, int side, int top);
        //NEWTON_API void NewtonMeshApplyCylindricalMapping(const NewtonMesh* mesh, int cylinderMaterial, int capMaterial);


        //NEWTON_API int NewtonMeshPlaneClip (const NewtonMesh* mesh, 
        //                                    const dFloat* plane, const dFloat* textureProjectionMatrix, 
        //                                    NewtonMesh** meshOut, int maxMeshCount, int capMaterial);

        //NEWTON_API void NewtonMeshBeginFace(const NewtonMesh *mesh);
        //NEWTON_API void NewtonMeshAddFace(const NewtonMesh *mesh, int vertexCount, const dFloat* vertex, int strideInBytes, int materialIndex);
        //NEWTON_API void NewtonMeshEndFace(const NewtonMesh *mesh);

        //NEWTON_API int NewtonMeshGetVertexCount (const NewtonMesh *mesh); 
        //NEWTON_API void NewtonMeshGetVertexStreams (const NewtonMesh *mesh, 
        //                                            int vertexStrideInByte, dFloat* vertex,
        //                                            int normalStrideInByte, dFloat* normal,
        //                                            int uvStrideInByte, dFloat* uv);
        //NEWTON_API void NewtonMeshGetIndirectVertexStreams(const NewtonMesh *mesh, 
        //                                                   int vertexStrideInByte, dFloat* vertex, int* vertexIndices, int* vertexCount,
        //                                                   int normalStrideInByte, dFloat* normal, int* normalIndices, int* normalCount,
        //                                                   int uvStrideInByte, dFloat* uv, int* uvIndices, int* uvCount);

        //NEWTON_API int NewtonMeshFirstMaterial (const NewtonMesh *mesh); 
        //NEWTON_API int NewtonMeshNextMaterial (const NewtonMesh *mesh, int materialHandle); 
        //NEWTON_API int NewtonMeshMaterialGetMaterial (const NewtonMesh *mesh, int materialHandle); 
        //NEWTON_API int NewtonMeshMaterialGetIndexCount (const NewtonMesh *mesh, int materialHandle); 
        //NEWTON_API void NewtonMeshMaterialGetIndexStream (const NewtonMesh *mesh, int materialHandle, int* index); 
        //NEWTON_API void NewtonMeshMaterialGetIndexStreamShort (const NewtonMesh *mesh, int materialHandle, short int* index); 

    }
}
