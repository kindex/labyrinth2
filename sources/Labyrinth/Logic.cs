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

            for (int i = 0; i < lights.Count; i++)
            {
                float x = 2.0f * Radians.Sin(time * (0.21718f * i + 1) / 10);
                float y = 0.1f;
                float z = 2.0f * Radians.Cos(time * (0.31434f * i + 1) / 10);

                lights[i].Position = new Vector3(x, y, z);
            }
        }
    }
}
