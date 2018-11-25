using System;
using System.Collections.Generic;
using System.Linq;
using Electronica.Graphics.Output;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Electronica.Input
{
    public struct Drag
    {
        public Drag(Vector2 start)
        {
            this.start = start;
            current = start;
            end = null;
        }

        public Vector2 start;
        public Vector2 current;
        public Vector2? end;
    }

    /// <summary>
    /// A list of mouse buttons.
    /// </summary>
    public enum MouseButton
    {
        Left,
        Right,
        Middle
    }

    /// <summary>
    /// A class for input handling.
    /// </summary>
    public static class InputHandler
    {
        private static readonly float DoubleClickTime = 250f;

        private static MouseState mCurrentMouseState, mLastMouseState;
        private static KeyboardState mCurrentKeyboardState, mLastKeyboardState;

        private static float mDoubleClickTimer;
        private static bool mDoubleClicked;

        private static Vector2 mMouseAnchor;
        private static bool mMouseAnchored;

        private static Dictionary<MouseButton, Drag> mMouseDrags;

        /// <summary>
        /// Methods for polling current input states.
        /// </summary>

        #region InputPolling

        public static bool FixedMousePosDrag { get; set; }

        public static Vector2 MousePosition { get; private set; }
        public static Vector2 DeltaMousePosition { get; private set; }
        public static float ScrollWheelPosition { get; private set; }
        public static float DeltaScrollWheel { get; private set; }

        public static Vector3 WorldSpacePosition { get; private set; }
        public static Vector3 WorldSpaceDirection { get; private set; }

        public static bool IsKeyPressed(Keys key) => mCurrentKeyboardState.IsKeyDown(key);

        public static bool IsKeyJustPressed(Keys key) => mCurrentKeyboardState.IsKeyDown(key) && !mLastKeyboardState.IsKeyDown(key);

        public static bool IsKeyJustReleased(Keys key) => !mCurrentKeyboardState.IsKeyDown(key) && mLastKeyboardState.IsKeyDown(key);

        public static bool IsMouseButtonPressed(MouseButton button) => GetMouseButtonPressed(mCurrentMouseState, button);

        public static bool IsMouseButtonJustPressed(MouseButton button) => GetMouseButtonPressed(mCurrentMouseState, button) && !GetMouseButtonPressed(mLastMouseState, button);

        public static bool IsMouseButtonJustReleased(MouseButton button) => !GetMouseButtonPressed(mCurrentMouseState, button) && GetMouseButtonPressed(mLastMouseState, button);

        public static bool DoubleClicked() => mDoubleClicked;

        public static bool IsMouseAnchored() => mMouseAnchored;

        public static Drag? GetDrag(MouseButton button)
        {
            if (mMouseDrags.TryGetValue(button, out Drag drag))
                return drag;
            else
                return null;
        }

        #endregion InputPolling

        /// <summary>
        /// Anchores the mouse at a position.
        /// </summary>
        /// <param name="anchor">The anchor position.</param>
        public static void SetAnchor(Vector2 anchor)
        {
            mMouseAnchor = anchor;
            mMouseAnchored = true;
        }

        /// <summary>
        /// Sets the mouse position.
        /// </summary>
        /// <param name="updateStates">Set the last mouse state to the set position so that the delta position is accurate.</param>
        /// <param name="position">The position.</param>
        public static void SetMousePosition(bool updateStates, Vector2 position)
        {
            Mouse.SetPosition((int)position.X, (int)position.Y);

            if (updateStates)
            {
                mCurrentMouseState = Mouse.GetState();
                mLastMouseState = mCurrentMouseState;
            }
        }

        /// <summary>
        /// Anchores the mouse at a position.
        /// </summary>
        /// <param name="x">The x value of the position.</param>
        /// <param name="y">The y value of the position.</param>
        public static void SetAnchor(float x, float y)
        {
            mMouseAnchor = new Vector2(x, y);
            mMouseAnchored = true;
        }

        /// <summary>
        /// Releases the anchor.
        /// </summary>
        public static void ReleaseAnchor() => mMouseAnchored = false;

        /// <summary>
        /// Initializes the input handler.
        /// </summary>
        public static void Initialize()
        {
            mCurrentMouseState = Mouse.GetState();
            mCurrentKeyboardState = Keyboard.GetState();
            mDoubleClickTimer = 0;
            mMouseAnchored = false;
            mMouseDrags = new Dictionary<MouseButton, Drag>(Enum.GetValues(typeof(MouseButton)).Length);
        }

        /// <summary>
        /// Projects the mouse position to world space.
        /// </summary>
        /// <param name="camera">The camera to unproject from.</param>
        /// <param name="graphics">The <see cref="GraphicsDeviceManager"/>.</param>
        public static void UpdateWorldSpace(Camera camera, GraphicsDeviceManager graphics)
        {
            Vector3 nearSource = new Vector3(MousePosition, 0f);
            Vector3 farSource = new Vector3(MousePosition, 1f);
            Vector3 nearPoint = graphics.GraphicsDevice.Viewport.Unproject(nearSource, camera.ProjectionMatrix, camera.ViewMatrix, Matrix.Identity);
            Vector3 farPoint = graphics.GraphicsDevice.Viewport.Unproject(farSource, camera.ProjectionMatrix, camera.ViewMatrix, Matrix.Identity);

            Vector3 direction = farPoint - nearPoint;
            direction.Normalize();

            WorldSpacePosition = nearPoint;
            WorldSpaceDirection = direction;
        }

        public static void Update(GameTime gameTime)
        {
            mLastKeyboardState = mCurrentKeyboardState;
            mCurrentKeyboardState = Keyboard.GetState();

            mLastMouseState = mCurrentMouseState;
            mCurrentMouseState = Mouse.GetState();

            if (mDoubleClickTimer > 0)
                mDoubleClickTimer -= (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            else
                mDoubleClicked = false;

            if (IsMouseButtonJustPressed(MouseButton.Left))
            {
                if (mDoubleClickTimer > 0)
                    mDoubleClicked = true;
                else
                    mDoubleClickTimer = DoubleClickTime;
            }

            MousePosition = mCurrentMouseState.Position.ToVector2();

            foreach (MouseButton button in Enum.GetValues(typeof(MouseButton)))
                CheckDrag(button);

            if (mMouseAnchored)
                DeltaMousePosition = mMouseAnchor - MousePosition;
            else
                DeltaMousePosition = mLastMouseState.Position.ToVector2() - MousePosition;

            ScrollWheelPosition = mCurrentMouseState.ScrollWheelValue;
            DeltaScrollWheel = ScrollWheelPosition - mLastMouseState.ScrollWheelValue;

            if (mMouseAnchored)
                Mouse.SetPosition((int)mMouseAnchor.X, (int)mMouseAnchor.Y);
        }

        /// <summary>
        /// Checks if the specified MouseButton is pressed.
        /// </summary>
        /// <param name="state">The state to check.</param>
        /// <param name="button">The button to check.</param>
        /// <returns>Is the mouse button pressed.</returns>
        private static bool GetMouseButtonPressed(MouseState state, MouseButton button)
        {
            switch (button)
            {
                case MouseButton.Left:
                    return state.LeftButton == ButtonState.Pressed;

                case MouseButton.Right:
                    return state.RightButton == ButtonState.Pressed;

                case MouseButton.Middle:
                    return state.MiddleButton == ButtonState.Pressed;
            }
            return false;
        }

        /// <summary>
        /// Updates the drag event of a specific button.
        /// </summary>
        /// <param name="button">The button to check.</param>
        private static void CheckDrag(MouseButton button)
        {
            Drag drag;
            bool hasValue;

            if (hasValue = mMouseDrags.TryGetValue(button, out drag))
            {
                if (drag.end.HasValue)
                {
                    mMouseDrags.Remove(button);
                    return;
                }
            }

            if (IsMouseButtonPressed(button))
            {
                if (hasValue)
                    drag.current = MousePosition;
                else
                {
                    if (!IsMouseAnchored() && FixedMousePosDrag)
                        SetAnchor(MousePosition);

                    mMouseDrags.Add(button, new Drag(MousePosition));
                }
            }
            else if (IsMouseButtonJustReleased(button))
            {
                if (hasValue)
                {
                    drag.end = MousePosition;
                    if (drag.start == mMouseAnchor && FixedMousePosDrag)
                        ReleaseAnchor();
                }
            }

            if (hasValue)
                mMouseDrags[button] = drag;
        }
    }
}