using System;
using System.Collections.Generic;
using System.Text;
using Game.Graphics.Window.InputEvents;
using Game.Graphics.Window;

namespace Game
{
    sealed partial class Game
    {
        public override void OnUpdate(float deltaTime)
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
                        case Key.F1:
                            if (camera is ThirdPersonCamera)
                            {
                                camera = new SpectatorCamera(camera);
                            }
                            else
                            {
                                camera = new ThirdPersonCamera(camera);
                            }
                            break;

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
                    ThirdPersonCamera o_camera = (ThirdPersonCamera)camera;
                    Vector3 center = active_character.Position;

                    o_camera.SetTargetPosition(center);

                    o_camera.Rotate(mousePos.X * Radians.PI / Height, mousePos.Y * Radians.PI / Width);

                    Vector3 direction = (center - o_camera.Position).GetNormalized();
//                    o_camera.SetPosition((center - direction) * 5, Vector3.UnitY);

                    Vector3 real_direction = o_camera.GetMoveDirection(moveDirection);

                    active_character.Body.physic_body.AddImpulse(real_direction * deltaTime * 10, active_character.Position);
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
