using System;

namespace Game
{
    public abstract class Camera
    {
        public Camera()
        {
            XAxis = Vector3.UnitX;
            YAxis = Vector3.UnitY;
            ZAxis = Vector3.UnitZ;

            Position = Vector3.Zero;

            //Velocity = Vector3.Zero;
            //Acceleration = Vector3.Zero;
            //CurrentVelocity = Vector3.Zero;
        }

        public Matrix4 ViewMatrix
        {
            get
            {
                if (viewMatrixDirty)
                {
                    UpdateViewMatrix();
                }

                return viewMatrix;
            }
        }

        public abstract void Move(Vector3 direction, float elapsedTimeInSecs);
        public abstract void Rotate(float heading, float pitch);

        public Vector3 XAxis { get; protected set; }
        public Vector3 YAxis { get; protected set; }
        public Vector3 ZAxis { get; protected set; }

        public Vector3 Position { get; protected set; }

        //public Vector3 Velocity { get; protected set; }
        //public Vector3 Acceleration { get; protected set; }
        //protected Vector3 CurrentVelocity = Vector3.Zero;

        protected bool renormalizeAxes = false;
        protected bool viewMatrixDirty = false;

        protected void UpdateViewMatrix()
        {
            if (renormalizeAxes)
            {
                ZAxis = Vector3.Normalize(ZAxis);
                YAxis = Vector3.Normalize(Vector3.Cross(ZAxis, XAxis));
                XAxis = Vector3.Normalize(Vector3.Cross(YAxis, ZAxis));

                renormalizeAxes = false;
            }

            viewMatrix.Row0 = new Vector4(XAxis.X, YAxis.X, -ZAxis.X, 0.0f);
            viewMatrix.Row1 = new Vector4(XAxis.Y, YAxis.Y, -ZAxis.Y, 0.0f);
            viewMatrix.Row2 = new Vector4(XAxis.Z, YAxis.Z, -ZAxis.Z, 0.0f);
            viewMatrix.Row3.X = -Vector3.Dot(XAxis, Position);
            viewMatrix.Row3.Y = -Vector3.Dot(YAxis, Position);
            viewMatrix.Row3.Z = Vector3.Dot(ZAxis, Position);

            viewMatrixDirty = false;
        }

        Matrix4 viewMatrix = Matrix4.Identity;
    }

    public abstract class FreeLookingCamera : Camera
    {
        public override void Move(Vector3 direction, float elapsedTimeInSecs)
        {
            Position += (XAxis * direction.X + Vector3.UnitY * direction.Y + ForwardMovementDirection * direction.Z) * elapsedTimeInSecs;

            viewMatrixDirty = true;
        }

        public void SetPosition(Vector3 position, Vector3 target, Vector3 up)
        {
            Position = position;

            ZAxis = Vector3.Normalize(target - position);
            XAxis = Vector3.Normalize(Vector3.Cross(up, ZAxis));
            YAxis = Vector3.Normalize(Vector3.Cross(ZAxis, XAxis));

            UpdateViewMatrix();

            pitchRadians = (float)Math.Asin(ViewMatrix.Row1.Z);
        }

        public override void Rotate(float heading, float pitch)
        {
            pitchRadians += pitch;

            float halfPI = Radians.PI / 2.0f;

            if (pitchRadians > halfPI)
            {
                pitch = halfPI - (pitchRadians - pitch);
                pitchRadians = halfPI;
            }

            if (pitchRadians < -halfPI)
            {
                pitch = -halfPI - (pitchRadians - pitch);
                pitchRadians = -halfPI;
            }

            if (heading != 0.0f)
            {
                Matrix4 rotation = Matrix4.RotateY(heading);
                XAxis = Vector3.TransformVector(XAxis, rotation);
                ZAxis = Vector3.TransformVector(ZAxis, rotation);

                renormalizeAxes = true;
            }

            if (pitch != 0.0f)
            {
                Matrix4 rotation = Matrix4.Rotate(XAxis, pitch);
                YAxis = Vector3.TransformVector(YAxis, rotation);
                ZAxis = Vector3.TransformVector(ZAxis, rotation);

                renormalizeAxes = true;
            }
        }

        float pitchRadians = 0.0f;

        protected abstract Vector3 ForwardMovementDirection { get; }
    }

    public sealed class FirstPersonCamera : FreeLookingCamera
    {
        protected override Vector3 ForwardMovementDirection
        {
            get
            {
                return Vector3.Cross(XAxis, Vector3.UnitY);
            }
        }
    }

    public sealed class SpectatorCamera : FreeLookingCamera
    {
        protected override Vector3 ForwardMovementDirection
        {
            get
            {
                return ZAxis;
            }
        }
    }

    public abstract class FixedLookingCamera : Camera
    {
        public void SetPosition(Vector3 position, Vector3 up)
        {
            Position = position;

            ZAxis = Vector3.Normalize(TargetPosition - position);
            XAxis = Vector3.Normalize(Vector3.Cross(up, ZAxis));
            YAxis = Vector3.Normalize(Vector3.Cross(ZAxis, XAxis));

            UpdateViewMatrix();

            pitchRadians = (float)Math.Asin(ViewMatrix.Row1.Z);
        }

        public Vector3 TargetPosition;
        protected float pitchRadians = 0.0f;
    }

    public sealed class OrbitCamera : FixedLookingCamera
    {

        public override void Move(Vector3 direction, float elapsedTimeInSecs)
        {
        }

        public override void Rotate(float heading, float pitch)
        {
            pitchRadians += pitch;

            float halfPI = Radians.PI / 2.0f;

            if (pitchRadians > halfPI)
            {
                pitch = halfPI - (pitchRadians - pitch);
                pitchRadians = halfPI;
            }

            if (pitchRadians < -halfPI)
            {
                pitch = -halfPI - (pitchRadians - pitch);
                pitchRadians = -halfPI;
            } 
            
            Vector3 delta = Position - TargetPosition;

            if (heading != 0.0f)
            {
                Matrix4 rotation = Matrix4.RotateY(heading);
                XAxis = Vector3.TransformVector(XAxis, rotation);
                ZAxis = Vector3.TransformVector(ZAxis, rotation);

                delta = Vector3.TransformVector(delta, rotation);
                Position = delta + TargetPosition;

                renormalizeAxes = true;
            }

            if (pitch != 0.0f)
            {
                Matrix4 rotation = Matrix4.Rotate(XAxis, pitch);
                YAxis = Vector3.TransformVector(YAxis, rotation);
                ZAxis = Vector3.TransformVector(ZAxis, rotation);

                Position = Vector3.TransformVector(delta, rotation) + TargetPosition;

                renormalizeAxes = true;
            }
        }
    }

    public sealed class ThirdPersonCamera : FixedLookingCamera
    {
        public override void Move(Vector3 direction, float elapsedTimeInSecs)
        {
        }

        public override void Rotate(float heading, float pitch)
        {
        }
    }
}
