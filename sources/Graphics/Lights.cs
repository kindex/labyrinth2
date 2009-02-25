using System;
using System.Collections.Generic;
using System.Text;

namespace Game
{
    public abstract class Light
    {
        public const float zNear = 0.01f;

        public virtual Vector3 Position { get; set; }

        public float Ambient { get; set; }
        public float Diffuse { get; set; }
        public float Specular { get; set; }
        public float Shininess { get; set; }

        public Vector3 Color { get; set; }
    }

    public sealed class PointLight : Light
    {
        public PointLight()
        {
            ViewMatrix = Matrix4.Identity;
            WorldMatrix = Matrix4.Identity;
            lightScale = Matrix4.Identity;
        }

        public override Vector3 Position
        {
            get
            {
                return position;
            }
            set
            {
                position = value;
                WorldMatrix = lightScale * Matrix4.Translation(position);
                ViewMatrix = Matrix4.LookAt(position, position + Vector3.UnitZ, Vector3.UnitY);
            }
        }

        public float Radius
        {
            get
            {
                return radius;
            }
            set
            {
                radius = value;
                Radius2 = radius * radius;
                DepthParam1 = - (zNear * radius) / (radius - zNear);
                DepthParam2 = 0.5f + 0.5f * (radius + zNear) / (radius - zNear);
                lightScale = Matrix4.Scale(new Vector3(radius / Radians.Cos(Radians.PI / 8)));
                WorldMatrix = lightScale * Matrix4.Translation(position);
            }
        }

        public float Radius2 { get; private set; }
        public float DepthParam1 { get; private set; }
        public float DepthParam2 { get; private set; }

        public Matrix4 ViewMatrix { get; private set; }
        public Matrix4 WorldMatrix { get; private set; }

        float radius;
        Vector3 position;
        Matrix4 lightScale;
    }

    public sealed class SpotLight : Light
    {
        public float Exponent { get; set; }

        public float Length
        {
            get
            {
                return length;
            }
            set
            {
                SetAngleAndLength(angle, value);
            }
        }

        public float Angle
        {
            get
            {
                return angle;
            }
            set
            {
                SetAngleAndLength(value, length);
            }
        }

        public override Vector3 Position
        {
            get
            {
                return position;
            }
            set
            {
                SetPosAndDir(value, direction);
            }
        }
        
        public Vector3 Direction
        {
            get
            {
                return direction;
            }
            set
            {
                SetPosAndDir(position, value);
            }
        }

        public void SetAngleAndLength(float angle, float length)
        {
            this.length = length;
            Length2 = length * length;
            if (angle == 0.0)
            {
                return;
            }

            this.angle = angle;

            CosAngle = Degrees.Cos(angle);
            AngleParam1 = 1.0f / (1.0f - CosAngle);
            AngleParam2 = -CosAngle / (1.0f - CosAngle);
            ProjectionMatrix = Matrix4.Perspective(2.0f * angle, 1.0f, zNear, length);

            float lightScaleXY = Length / Radians.Cos(Radians.PI / 8) * (float)System.Math.Sqrt(1.0f / (CosAngle * CosAngle) - 1.0f);
            float lightScaleZ = Length;
            lightScale = Matrix4.Scale(new Vector3(lightScaleXY, lightScaleXY, lightScaleZ));
        }

        public void SetPosAndDir(Vector3 position, Vector3 direction)
        {
            float dirLength2 = Vector3.Dot(direction, direction);
            if (dirLength2 != 1.0f)
            {
                direction /= (float)Math.Sqrt(dirLength2);
            }
            this.position = position;
            this.direction = direction;

            ViewMatrix = Matrix4.LookAt(position, position + direction, Vector3.UnitY);
            ShadowMatrix = ViewMatrix
                         * ProjectionMatrix
                         * biasMatrix;

            Matrix4 rot = Matrix4.Identity;
            if (direction != Vector3.UnitZ)
            {
                Vector3 up = Vector3.Cross(Vector3.UnitZ, direction);
                float angle = (float)System.Math.Acos(Vector3.Dot(Vector3.UnitZ, direction));
                rot = Matrix4.Rotate(up, angle);
            }

            WorldMatrix = lightScale
                         * rot
                         * Matrix4.Translation(position);
        }

        public float Length2 { get; private set; }
        public float CosAngle { get; private set; }
        public float AngleParam1 { get; private set; }
        public float AngleParam2 { get; private set; }
        public Matrix4 ProjectionMatrix { get; private set; }
        public Matrix4 ViewMatrix { get; private set; }
        public Matrix4 ShadowMatrix { get; private set; }
        public Matrix4 WorldMatrix { get; private set; }

        Vector3 position;
        Vector3 direction;
        float length;
        float angle;

        Matrix4 lightScale;
        Matrix4 biasMatrix = new Matrix4(
                               new Vector4(0.5f, 0.0f, 0.0f, 0.0f),
                               new Vector4(0.0f, 0.5f, 0.0f, 0.0f),
                               new Vector4(0.0f, 0.0f, 0.5f, 0.0f),
                               new Vector4(0.5f, 0.5f, 0.5f, 1.0f));
    }
}
