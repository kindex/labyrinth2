﻿using System;
using System.Collections.Generic;
using System.Text;
using Game.Graphics;
using Game.Physics.Newton;

namespace Game.Labyrinth.Character
{
    class Character
    {
        public static readonly Vector3 box_size = Vector3.Half;
        public static readonly Vector3 gravity = new Vector3(0, -9.8f, 0);
        public static readonly float jump_height = box_size.Y*1.30f;


        const float mass = 1.0f;

        public string Name { get; private set; }
        public Box Body { get; private set; }

        Material material;
        Physics.Newton.World world;

        public Character(Material material)
        {
            this.material = material;
        }

        public void PlaceToScene(Vector3 position, Physics.Newton.World world)
        {
            this.world = world;
            Body = new Box(position - box_size / 2, position + box_size / 2, material, world);

            Body.physic_body.SetMass(mass, mass*Body.inertia);

            // global gravity
            Body.physic_body.SetForceAndTorqueEvent += delegate(Physics.Newton.Body body, float timestep, int threadIndex)
                {
                    body.AddForce(gravity * mass);
                };
        }

        public Vector3 Position
        {
            get
            {
                return Body.physic_body.Matrix.Posit;
            }
        }

        internal bool isStanding()
        {
            foreach (var contact_join in Body.physic_body.ContactJoints)
            {
                foreach (var contact in contact_join.Contacts)
                {
                    Vector3 pos, n;
                    contact.material.GetContactPositionAndNormal(out pos, out n);

                    if (Vector3.Dot(n, Vector3.UnitY) > 0.5f)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        internal void Jump()
        {
            if (isStanding())
            {
                float t = (float)Math.Sqrt(2.0f * jump_height / -gravity.Y);

                Vector3 force = new Vector3(0.0f, t * -gravity.Y, 0.0f) * mass;
                Body.physic_body.AddImpulse(force, Position);
            }
        }

        internal void Accelerate(Vector3 real_direction, float deltaTime)
        {
            bool stands = isStanding();
            float k = stands ? 10.0f : 5.0f; // on fly accedlerate two times slower
            {
                Body.physic_body.AddImpulse(real_direction * deltaTime * k, this.Position);
            }
        }
    }
}
