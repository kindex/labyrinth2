using System;
using System.Diagnostics;
using System.Collections.ObjectModel;

namespace Game.Physics.Newton.Joints
{
    public sealed class MultiBodyVehicleTire : CustomJoint
    {
        const float MIN_JOINT_PIN_LENGTH = 50.0f;

        public MultiBodyVehicleTire(Body hubBody, Body tire, float suspensionLength, float springConst, float damperConst, float radio)
            : base(6, hubBody, tire)
        {
            float mass;
            Vector3 inertia;

            radius = radio;
            steerAngle = 0.0f;
            brakeToque = 0.0f;
            enginetorque = 0.0f;
            angularDragCoef = 0.0f;
            spring = springConst;
            damper = damperConst;
            suspenstionSpan = suspensionLength;

            tire.GetMass(out mass, out inertia);
            Ixx = inertia.X;

            hubBody.GetMass(out mass, out inertia);
            effectiveSpringMass = mass * 0.25f;

            Matrix4 pinAndPivotFrame = tire.Matrix;
            CalculateLocalMatrix(pinAndPivotFrame, out chassisLocalMatrix, out tireLocalMatrix);

            refChassisLocalMatrix = chassisLocalMatrix;
        }

        protected override void GetInfo(ref JointRecord info)
        {
        }

        public float SteerAngle
        {
            get
            {
                return steerAngle;
            }
            set
            {
                if (Math.Abs(value - steerAngle) > 1.0e-4f)
                {
                    steerAngle = value;
                    chassisLocalMatrix = Matrix4.Multiply(Matrix4.RotateY(steerAngle), refChassisLocalMatrix);
                }
            }
        }

        public void SetTorque(float torque)
        {
            enginetorque = torque;
        }

        public void SetBrakeTorque(float torque)
        {
            brakeToque = torque;
        }

        public void SetAngulaRollingDrag(float angularDampingCoef)
        {
            this.angularDragCoef = angularDampingCoef;
        }

        public void ProjectTireMatrix()
        {
            Body tire = Body1;
            Body chassis = Body0;

            Matrix4 tireMatrix = tire.Matrix;
            Matrix4 chassisMatrix = chassis.Matrix;

            // project the tire matrix to the right space.
            Matrix4 tireMatrixInGlobalSpace = Matrix4.Multiply(tireLocalMatrix, tireMatrix);
            Matrix4 chassisMatrixInGlobalSpace = Matrix4.Multiply(chassisLocalMatrix, chassisMatrix);

            float projectDist = Vector3.Dot(tireMatrixInGlobalSpace.Posit - chassisMatrixInGlobalSpace.Posit, chassisMatrixInGlobalSpace.Up);
            chassisMatrixInGlobalSpace.Posit += chassisMatrixInGlobalSpace.Up * projectDist;

            chassisMatrixInGlobalSpace.Up = Vector3.Cross(tireMatrixInGlobalSpace.Right, chassisMatrixInGlobalSpace.Front);
            chassisMatrixInGlobalSpace.Up /= chassisMatrixInGlobalSpace.Up.Length;
            chassisMatrixInGlobalSpace.Right = Vector3.Cross(chassisMatrixInGlobalSpace.Front, chassisMatrixInGlobalSpace.Up);

            Matrix4 projectedChildMatrixInGlobalSpace = Matrix4.Multiply(Matrix4.InverseTransform(tireLocalMatrix), chassisMatrixInGlobalSpace);
            tire.Matrix = projectedChildMatrixInGlobalSpace;

            // project the tire velocity

            Vector3 tireVeloc = tire.Velocity;
            Vector3 chassisVeloc = chassis.Velocity;
            Vector3 chassisOmega = chassis.Omega;

            Vector3 chassisCom = chassis.CentreOfMass;

            chassisCom = Vector3.TransformPosition(chassisCom, chassisMatrix);
            chassisVeloc += Vector3.Cross(chassisOmega, chassisMatrixInGlobalSpace.Posit - chassisCom);

            Vector3 projTireVeloc = chassisVeloc - chassisMatrixInGlobalSpace.Up * Vector3.Dot(chassisVeloc, chassisMatrixInGlobalSpace.Up);
            projTireVeloc += chassisMatrixInGlobalSpace.Up * Vector3.Dot(tireVeloc, chassisMatrixInGlobalSpace.Up);
            tire.Velocity = projTireVeloc;

            // project angular velocity
            Vector3 tireOmega = tire.Omega;
            Vector3 projTireOmega = chassisOmega - chassisMatrixInGlobalSpace.Front * Vector3.Dot(chassisOmega, chassisMatrixInGlobalSpace.Front);
            projTireOmega += chassisMatrixInGlobalSpace.Front * Vector3.Dot(tireOmega, chassisMatrixInGlobalSpace.Front);
            tire.Omega = projTireOmega;
        }

        protected override void SubmitConstraint(float timestep, int threadIndex)
        {
            ProjectTireMatrix();

            // calculate the position of the pivot point and the Jacobian direction vectors, in global space. 
            Matrix4 tirePivotMatrix;
            Matrix4 chassisPivotMatrix;
            CalculateGlobalMatrix(chassisLocalMatrix, tireLocalMatrix, out chassisPivotMatrix, out tirePivotMatrix);

            // Restrict the movement on the pivot point along all two orthonormal direction
            Vector3 centerInTire = tirePivotMatrix.Posit;
            Vector3 centerInChassis = chassisPivotMatrix.Posit + chassisPivotMatrix.Up * Vector3.Dot(centerInTire - chassisPivotMatrix.Posit, chassisPivotMatrix.Up);
            AddLinearRow(centerInChassis, centerInTire, chassisPivotMatrix.Front);
            AddLinearRow(centerInChassis, centerInTire, chassisPivotMatrix.Right);

            // get a point along the pin axis at some reasonable large distance from the pivot
            Vector3 pointInPinInTire = centerInChassis + chassisPivotMatrix.Front * MIN_JOINT_PIN_LENGTH;
            Vector3 pointInPinInChassis = centerInTire + tirePivotMatrix.Front * MIN_JOINT_PIN_LENGTH;
            AddLinearRow(pointInPinInTire, pointInPinInChassis, chassisPivotMatrix.Right);
            AddLinearRow(pointInPinInTire, pointInPinInChassis, chassisPivotMatrix.Up);

            //calculate the suspension spring and damper force

            // calculate the velocity of tire attachments point on the car chassis

            Body tire = Body1;
            Body chassis = Body0;

            Vector3 tireVeloc = tire.Velocity;
            Vector3 chassisVeloc = chassis.Velocity;
            Vector3 chassisOmega = chassis.Omega;
            Matrix4 chassisMatrix = chassis.Matrix;
            Vector3 chassisCom = chassis.CentreOfMass;
            chassisCom = Vector3.TransformPosition(chassisCom, chassisMatrix);
            chassisVeloc += Vector3.Cross(chassisOmega, centerInChassis - chassisCom);

            // get the spring damper parameters
            float speed = Vector3.Dot(chassisVeloc - tireVeloc, chassisPivotMatrix.Up);
            float dist = Vector3.Dot(chassisPivotMatrix.Posit - tirePivotMatrix.Posit, chassisPivotMatrix.Up);

            // check if the suspension pass the bumpers limits
            if (-dist > suspenstionSpan * 0.5f)
            {
                // if it hit the bumpers then speed is zero
                speed = 0;
                AddLinearRow(centerInChassis, centerInChassis, chassisPivotMatrix.Up);
                SetRowMinimumFriction(0.0f);
            }
            else if (dist > 0.0f)
            {
                // if it hit the bumpers then speed is zero
                speed = 0;
                AddLinearRow(centerInChassis, centerInChassis, chassisPivotMatrix.Up);
                SetRowMaximumFriction(0.0f);
            }

            // calculate magnitude of suspension force 
            float force = Utilities.CalculateSpringDamperAcceleration(timestep, spring, dist, damper, speed) * effectiveSpringMass;

            Vector3 chassisForce = chassisMatrix.Up * force;
            Vector3 chassisTorque = Vector3.Cross(centerInChassis - chassisCom, chassisForce);

            chassis.AddForce(chassisForce);
            chassis.AddTorque(chassisTorque);

            Vector3 tireForce = -chassisForce;
            tire.AddForce(tireForce);

            // apply the engine torque to tire torque

            Vector3 tireOmega = tire.Omega;
            Matrix4 tireMatrix = tire.Matrix;

            float relOmega = Vector3.Dot(tireOmega - chassisOmega, tireMatrix.Front);

            // apply engine torque plus some tire angular drag
            Vector3 tireTorque = tireMatrix.Front * (enginetorque - relOmega * Ixx * angularDragCoef);
            tire.AddTorque(tireTorque);

            Vector3 chassisRotationTorque = chassisMatrix.Right * -enginetorque;
            chassis.AddTorque(chassisRotationTorque);

            enginetorque = 0.0f;

            // add the brake torque row
            if (Math.Abs(brakeToque) > 1.0e-3f)
            {
                relOmega /= timestep;
                AddAngularRow(0.0f, tireMatrix.Front);
                SetRowAcceleration(relOmega);
                SetRowMaximumFriction(brakeToque);
                SetRowMinimumFriction(-brakeToque);
            }
            brakeToque = 0.0f;
        }

        Matrix4 tireLocalMatrix;
        Matrix4 chassisLocalMatrix;
        Matrix4 refChassisLocalMatrix;

        float Ixx;
        float radius;
        float spring;
        float damper;

        float steerAngle;
        float brakeToque;
        float enginetorque;
        float angularDragCoef;
        float effectiveSpringMass;
        float suspenstionSpan;
    }

    public sealed class MultiBodyVehicleAxleDifferencial : CustomJoint
    {
        public MultiBodyVehicleAxleDifferencial(MultiBodyVehicleTire leftTire, MultiBodyVehicleTire rightTire, float maxFriction)
            : base(1, leftTire.Body1, rightTire.Body1)
        {
            Debug.Assert(leftTire.Body0.handle == rightTire.Body0.handle);

            this.maxFrition = Math.Abs(maxFriction);
            this.chassis = rightTire.Body0;
            this.leftTire = leftTire;
            this.rightTire = rightTire;
        }

        protected override void GetInfo(ref JointRecord info)
        {
        }

        protected override void SubmitConstraint(float timestep, int threadIndex)
        {
            Matrix4 chassisMatrix = chassis.Matrix;
            Matrix4 leftMatrix = leftTire.Body1.Matrix;
            Matrix4 rightMatrix = rightTire.Body1.Matrix;

            // calculate the geometrical turn radius of for this axle 

            Vector3 leftOrigin = Vector3.UntransformPosition(leftMatrix.Posit, chassisMatrix);
            Vector3 rightOrigin = Vector3.UntransformPosition(rightMatrix.Posit, chassisMatrix);
            Vector3 axleCenter = (rightOrigin + leftOrigin) * 0.5f;

            Vector3 tireAxisDir = Vector3.UntransformPosition((leftMatrix.Front + rightMatrix.Front) * 0.5f, chassisMatrix);
            axleCenter.Y = 0.0f;
            tireAxisDir.Y = 0.0f;

            Vector3 sideDir = Vector3.UnitZ;
            Vector3 deltaDir = tireAxisDir - sideDir;
            float relAccel = 0.0f;
            float den = deltaDir.Length2;
            if (den > 1.0e-6f)
            {
                float num = Vector3.Dot(axleCenter, deltaDir);
                float R = -num / den;

                float rr = Vector3.Dot(rightOrigin, sideDir);
                float rl = Vector3.Dot(leftOrigin, sideDir);
                float ratio = (R + rr) / (R + rl);

                // calculate the angular velocity for both bodies
                Vector3 omegaLeft = leftTire.Body1.Omega;
                Vector3 omegaRight = rightTire.Body1.Omega;

                // get angular velocity relative to the pin vector
                float wl = -Vector3.Dot(omegaLeft, leftMatrix.Front);
                float wr = Vector3.Dot(omegaRight, rightMatrix.Front);

                // establish the gear equation.
                float relOmega = wl + ratio * wr;
                relAccel = -0.5f * relOmega / timestep;
            }

            Vector3[] jacobian0 = new Vector3[] { Vector3.Zero, -leftMatrix.Front };
            Vector3[] jacobian1 = new Vector3[] { Vector3.Zero, rightMatrix.Front };

            AddGeneralRow(jacobian0, jacobian1);
            SetRowAcceleration(relAccel);
            SetRowMaximumFriction(maxFrition);
            SetRowMinimumFriction(-maxFrition);

        }

        float maxFrition;
        Body chassis;
        MultiBodyVehicleTire leftTire;
        MultiBodyVehicleTire rightTire;
    }

    public abstract class MultiBodyVehicle : CustomJoint
    {
        public MultiBodyVehicle(Vector3 front, Vector3 up, Body carBody)
            : base(2, carBody, null)
        {
            Matrix4 tmp = Body0.Matrix;
            Vector3 com = Body0.CentreOfMass;

            Matrix4 chassisMatrix = new Matrix4();
            chassisMatrix.Front = front;
            chassisMatrix.Up = up;
            chassisMatrix.Right = Vector3.Cross(front, up);
            chassisMatrix.Posit = Vector3.TransformPosition(com, tmp);

            CalculateLocalMatrix(chassisMatrix, out localFrame, out tmp);
        }

        protected override void GetInfo(ref JointRecord info)
        {
        }

        protected override void SubmitConstraint(float timestep, int threadIndex)
        {
        }

        public abstract void ApplyTorque(float torque);

        public abstract void ApplySteering(float angle);

        public abstract void ApplyBrake(float brakeTorque);

        public float GetSetTireSteerAngle(int index)
        {
            return Tires[index].SteerAngle;
        }

        public void ApplyTireSteerAngle(int index, float angle)
        {
            Tires[index].SteerAngle = angle;
        }

        public void ApplyTireBrake(int index, float brakeTorque)
        {
            Tires[index].SetBrakeTorque(brakeTorque);
        }

        public void ApplyTireTorque(int index, float torque)
        {
            Tires[index].SetTorque(torque);
        }

        public void ApplyTireRollingDrag(int index, float angularDampingCoef)
        {
            Tires[index].SetAngulaRollingDrag(angularDampingCoef);
        }

        public int GetTiresCount()
        {
            return Tires.Count;
        }

        public Body GetTireBody(int tireIndex)
        {
            return Tires[tireIndex].Body1;
        }

        public float GetSpeed()
        {
            Matrix4 chassisMatrix = Body0.Matrix;
            Vector3 veloc = Body0.Velocity;
            return Vector3.Dot(veloc, chassisMatrix.Front);
        }

        public int AddSingleSuspensionTire(Vector3 localPosition, float mass, float radius, float width, float suspensionLength, float springConst, float springDamper, SetForceAndTorqueEventHandler onForceAndTorqueEventHandler)
        {
            World world = Body0.World;

            // create the tire RogidBody 
            ConvexCollision collision = world.CreateChamferCylinder(radius, width, Matrix4.RotateY((float)Math.PI / 2));

            //create the rigid body
            Body tire = new Body(world, collision);

            // release the collision
            collision.Release();

            // set the material group id for vehicle
            //tire.MaterialGroupID = 0;
            //tire.MaterialGroupID = woodID;

            // set the force and torque call back function
            tire.SetForceAndTorqueEvent += onForceAndTorqueEventHandler;

            // body part do not collision
            tire.JointRecursiveCollision = false;

            // calculate the moment of inertia and the relative center of mass of the solid
            Vector3 origin;
            Vector3 inertia;
            collision.CalculateInertialMatrix(out inertia, out origin);

            // set the mass matrix
            tire.SetMass(mass, mass * inertia);

            // calculate the tire local base pose matrix
            Matrix4 tireMatrix = new Matrix4();
            tireMatrix.Front = localFrame.Right;
            tireMatrix.Up = localFrame.Up;
            tireMatrix.Right = Vector3.Cross(tireMatrix.Front, tireMatrix.Up);
            tireMatrix.Posit = localPosition;
            
            Matrix4 carMatrix = Body0.Matrix;

            // set the matrix for both the rigid body and the graphic body
            tire.Matrix = Matrix4.Multiply(tireMatrix, carMatrix);

            // add a single tire
            Tires.Add(new MultiBodyVehicleTire(Body0, tire, suspensionLength, springConst, springDamper, radius));

            return Tires.Count - 1;
        }

        public int AddSlipDifferencial(int leftTireIndex, int rightTireIndex, float maxFriction)
        { 
            Differentials.Add(new MultiBodyVehicleAxleDifferencial(Tires[leftTireIndex], Tires[rightTireIndex], maxFriction));
            return Differentials.Count - 1;
        }

        Collection<MultiBodyVehicleTire> Tires = new Collection<MultiBodyVehicleTire>();
        Collection<MultiBodyVehicleAxleDifferencial> Differentials = new Collection<MultiBodyVehicleAxleDifferencial>();

        Matrix4 localFrame;
    }
}
