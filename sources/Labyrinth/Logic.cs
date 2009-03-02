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
                    if (keyEvent.Key == Key.Escape)
                    {
                        if (MouseCaptured)
                        {
                            MouseCaptured = false;
                        }
                        else
                        {
                            Close();
                        }
                    }
                    else if (keyEvent.Key == Key.W)
                    {
                        move.Z += 1.0f;
                    }
                    else if (keyEvent.Key == Key.S)
                    {
                        move.Z -= 1.0f;
                    }
                    else if (keyEvent.Key == Key.A)
                    {
                        move.X -= 1.0f;
                    }
                    else if (keyEvent.Key == Key.D)
                    {
                        move.X += 1.0f;
                    }
                    else if (keyEvent.Key == Key.Q)
                    {
                        move.Y += 1.0f;
                    }
                    else if (keyEvent.Key == Key.Z)
                    {
                        move.Y -= 1.0f;
                    }
                    else if (keyEvent.Key == Key.Space)
                    {
                        visualize_volume = !visualize_volume;
                    }
                }
                else if (ev is KeyReleasedEvent)
                {
                    KeyReleasedEvent keyEvent = (KeyReleasedEvent)ev;
                    if (keyEvent.Key == Key.W)
                    {
                        move.Z -= 1.0f;
                    }
                    else if (keyEvent.Key == Key.S)
                    {
                        move.Z += 1.0f;
                    }
                    else if (keyEvent.Key == Key.A)
                    {
                        move.X += 1.0f;
                    }
                    else if (keyEvent.Key == Key.D)
                    {
                        move.X -= 1.0f;
                    }
                    else if (keyEvent.Key == Key.Q)
                    {
                        move.Y -= 1.0f;
                    }
                    else if (keyEvent.Key == Key.Z)
                    {
                        move.Y += 1.0f;
                    }
                }
            }

            if (MouseCaptured)
            {
                Point mousePos = this.MousePosition;
                camera.Rotate(mousePos.X * Radians.PI / Height, mousePos.Y * Radians.PI / Width);
            }

            Vector3 moveDirection = 3.0f * move;
            camera.Move(moveDirection, deltaTime);


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
