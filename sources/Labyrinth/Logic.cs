using System;
using System.Collections.Generic;
using System.Text;
using Game.Graphics.Window.InputEvents;
using Game.Graphics.Window;
using Game.Physics.Newton;
using Game.Labyrinth.Character;

namespace Game
{
    sealed partial class Game
    {
        public override void OnUpdate(float deltaTime, double totalTime)
        {
            // Physic
            physic_world.Update(deltaTime);

            // Controls
            InputEvent ev;
            while ((ev = GetNextInputEvent()) != null)
            {
                if (ev is MouseButtonPressedEvent)
                {
                    MouseButtonPressedEvent buttonEvent = (MouseButtonPressedEvent)ev;

                    if (buttonEvent.Button == MouseButton.Left)
                    {
                        MouseCaptured = true;
                    }
                }
                else if (ev is KeyPressedEvent)
                {
                    KeyPressedEvent keyEvent = (KeyPressedEvent)ev;
                    //if (keyEvent.Key == Key.M)
                    //{
                    //    Samples = 4 - Samples;
                    //    OnResize(Width, Height);
                    //}
                    //else
                    switch (keyEvent.Key)
                    {
                        case Key.Escape:
                            if (MouseCaptured)
                            {
                                MouseCaptured = false;
                            }
                            else
                            {
                                Close();
                            }
                            break;

                        case Key.Return:
                            if (camera is ThirdPersonCamera)
                            {
                                ThirdPersonCamera oldCamera = (ThirdPersonCamera)camera;
                                camera = new SpectatorCamera(oldCamera.Position, oldCamera.TargetPosition);
                            }
                            else
                            {
                                SpectatorCamera oldCamera = (SpectatorCamera)camera;
                                camera = new ThirdPersonCamera(active_character.Position, Vector3.UnitY, maxOrbitRadius);
                            }
                            break;

                        case Key.F1:
                            gameFlags.debugWifreframe = !gameFlags.debugWifreframe;
                            break;

                        case Key.W:
                            move.Z += 1.0f;
                            break;

                        case Key.S:
                            move.Z -= 1.0f;
                            break;

                        case Key.A:
                            move.X -= 1.0f;
                            break;

                        case Key.D:
                            move.X += 1.0f;
                            break;

                        case Key.Q:
                            move.Y += 1.0f;
                            break;

                        case Key.Z:
                            move.Y -= 1.0f;
                            break;

                        case Key.Space:
                            visualize_volume = !visualize_volume;
                            break;

                        case Key.J:
                            active_character.Jump();
                            break;

                    }
                }
                else if (ev is KeyReleasedEvent)
                {
                    KeyReleasedEvent keyEvent = (KeyReleasedEvent)ev;
                    switch (keyEvent.Key)
                    {
                        case Key.W:
                            move.Z -= 1.0f;
                            break;

                        case Key.S:
                            move.Z += 1.0f;
                            break;

                        case Key.A:
                            move.X += 1.0f;
                            break;

                        case Key.D:
                            move.X -= 1.0f;
                            break;

                        case Key.Q:
                            move.Y -= 1.0f;
                            break;

                        case Key.Z:
                            move.Y += 1.0f;
                            break;
                    }
                }
            }


            if (MouseCaptured)
            {
                Point mousePos = this.MousePosition;
                Vector3 moveDirection = 3.0f * move;

                if (camera is SpectatorCamera)
                {
                    camera.Rotate(mousePos.X * Radians.PI / Height, mousePos.Y * Radians.PI / Width);
                    camera.Move(moveDirection, deltaTime);
                }
                else if (camera is ThirdPersonCamera) // fixed
                {
                    // rotate camera
                    ThirdPersonCamera o_camera = (ThirdPersonCamera)camera;
                    o_camera.SetTargetPosition(active_character.Position);
                    o_camera.Rotate(mousePos.X * Radians.PI / Height, mousePos.Y * Radians.PI / Width);

                    //move character
                    Vector3 real_direction = o_camera.GetMoveDirection(moveDirection);
                    active_character.Accelerate(real_direction, deltaTime);

                    //clip walls
                    //Vector3 direction = (center - o_camera.Position).GetNormalized();
                    bool found = false;

                    o_camera.SetPosition(active_character.Position + (o_camera.Position - active_character.Position).GetNormalized() * maxOrbitRadius, Vector3.UnitY);

                    float p = 1.0f;

                    physic_world.RayCast(active_character.Position, o_camera.Position,
                        delegate(Body body, Vector3 hitNormal, int collisionID, float intersectParam)
                        {
                            found = true;
                            if (p > intersectParam)
                            {
                                p = intersectParam;
                            }
                            return p;
                        },
                        delegate(Body body, Collision collision)
                        {
                            return 1;
                        });

                    if (found)
                    {
                        float dist = (o_camera.Position - active_character.Position).Length * p;
                        if (dist < Character.box_size.Length / 2)
                        {
                            dist = Character.box_size.Length / 2;
                        }
                        else
                        {
                            p *= 0.96f;
                        }
                        o_camera.SetPosition(active_character.Position + (o_camera.Position - active_character.Position).GetNormalized() * dist, Vector3.UnitY);
                    }
                }
            }

            // Animate lights
            float speed = 3f;
            foreach (Light light in lights)
            {
                int ceil_x = (int)Math.Floor(light.Position.X / ceil_size.X);
                int ceil_y = (int)Math.Floor(light.Position.Z / ceil_size.Z);

                labyrinth_matrix.GetNextToExitCell(ref ceil_x, ref ceil_y);
                Vector3 next = new Vector3((ceil_x + 0.5f) * ceil_size.X, light.Position.Y, (ceil_y + 0.5f) * ceil_size.Z);

                float distance = speed*deltaTime;

                if ((light.Position - next).Length < distance)
                {
                    light.Position = next;
                }
                else
                {
                    light.Position += (next - light.Position).GetNormalized() * distance;
                }

                speed *= 0.9f;
            }
        }
    }
}
