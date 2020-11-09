using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;

namespace TGC.Exam
{
    class FreeCamera
    {
        private readonly bool lockMouse;

        private Vector3 FrontDirection { get; set; }
        private Vector3 UpDirection { get; set; }

        private Vector3 RightDirection { get; set; }

        private readonly Point screenCenter;
        private bool changed;

        private Vector2 pastMousePosition;

        public Matrix View { private set; get; }
        public Matrix Projection { private set; get; }

        public Vector3 Position { private set; get; }

        // Angles
        private float yaw = -90f;
        private float pitch;

        public FreeCamera(float aspectRatio, Vector3 position, Point screenCenter) : this(aspectRatio, position)
        {
            lockMouse = true;
            this.screenCenter = screenCenter;
        }

        public FreeCamera(float aspectRatio, Vector3 position)
        {
            Position = position;
            pastMousePosition = Mouse.GetState().Position.ToVector2();
            UpdateCameraVectors();
            CalculateView();

            Projection = Matrix.CreatePerspectiveFieldOfView(MathF.PI / 3f, aspectRatio, .1f, 1000f);
        }

        public float MovementSpeed { get; set; } = 100f;
        public float MouseSensitivity { get; set; } = 2.5f;

        private void CalculateView()
        {
            View = Matrix.CreateLookAt(Position, Position + FrontDirection, UpDirection);
        }

        /// <inheritdoc />
        public void Update(GameTime gameTime)
        {
            var elapsedTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
            changed = false;
            ProcessKeyboard(elapsedTime);
            ProcessMouseMovement(elapsedTime);

            if (changed)
                CalculateView();
        }

        private void ProcessKeyboard(float elapsedTime)
        {
            var keyboardState = Keyboard.GetState();

            var currentMovementSpeed = MovementSpeed;
            if (keyboardState.IsKeyDown(Keys.LeftShift))
                currentMovementSpeed *= 5f;

            if (keyboardState.IsKeyDown(Keys.A) || keyboardState.IsKeyDown(Keys.Left))
            {
                Position += -RightDirection * currentMovementSpeed * elapsedTime;
                changed = true;
            }

            if (keyboardState.IsKeyDown(Keys.D) || keyboardState.IsKeyDown(Keys.Right))
            {
                Position += RightDirection * currentMovementSpeed * elapsedTime;
                changed = true;
            }

            if (keyboardState.IsKeyDown(Keys.W) || keyboardState.IsKeyDown(Keys.Up))
            {
                Position += FrontDirection * currentMovementSpeed * elapsedTime;
                changed = true;
            }

            if (keyboardState.IsKeyDown(Keys.S) || keyboardState.IsKeyDown(Keys.Down))
            {
                Position += -FrontDirection * currentMovementSpeed * elapsedTime;
                changed = true;
            }
        }

        private void ProcessMouseMovement(float elapsedTime)
        {
            var mouseState = Mouse.GetState();

            if (mouseState.RightButton.Equals(ButtonState.Pressed))
            {
                var mouseDelta = mouseState.Position.ToVector2() - pastMousePosition;
                mouseDelta *= MouseSensitivity * elapsedTime;

                yaw += -mouseDelta.X;
                pitch += mouseDelta.Y;

                changed = true;
                UpdateCameraVectors();


                if (lockMouse)
                {
                    Mouse.SetPosition(screenCenter.X, screenCenter.Y);
                    Mouse.SetCursor(MouseCursor.Crosshair);
                }
                else
                {
                    Mouse.SetCursor(MouseCursor.Arrow);
                }
            }

            pastMousePosition = Mouse.GetState().Position.ToVector2();
        }

        private void UpdateCameraVectors()
        {
            if (pitch > 89.0f)
                pitch = 89.0f;
            if (pitch < -89.0f)
                pitch = -89.0f;

            // Calculate the new Front vector
            Vector3 tempFront;
            tempFront.X = MathF.Cos(MathHelper.ToRadians(yaw)) * MathF.Cos(MathHelper.ToRadians(pitch));
            tempFront.Y = MathF.Sin(MathHelper.ToRadians(pitch));
            tempFront.Z = MathF.Sin(MathHelper.ToRadians(yaw)) * MathF.Cos(MathHelper.ToRadians(pitch));

            FrontDirection = Vector3.Normalize(tempFront);

            // Also re-calculate the Right and Up vector
            // Normalize the vectors, because their length gets closer to 0 the more you look up or down which results in slower movement.
            RightDirection = Vector3.Normalize(Vector3.Cross(FrontDirection, Vector3.Up));
            UpDirection = Vector3.Normalize(Vector3.Cross(RightDirection, FrontDirection));
        }
    }
}