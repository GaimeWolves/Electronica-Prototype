using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Electronica.Input
{
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

        /// <summary>
        /// Methods for polling current input states.
        /// </summary>

        #region InputPolling

        public static Vector2 MousePosition { get; private set; }
        public static Vector2 DeltaMousePosition { get; private set; }
        public static float ScrollWheelPosition { get; private set; }
        public static float DeltaScrollWheel { get; private set; }

        public static bool IsKeyPressed(Keys key) => mCurrentKeyboardState.IsKeyDown(key);

        public static bool IsKeyJustPressed(Keys key) => mCurrentKeyboardState.IsKeyDown(key) && !mLastKeyboardState.IsKeyDown(key);

        public static bool IsKeyJustReleased(Keys key) => !mCurrentKeyboardState.IsKeyDown(key) && mLastKeyboardState.IsKeyDown(key);

        public static bool IsMouseButtonPressed(MouseButton button) => GetMouseButtonPressed(mCurrentMouseState, button);

        public static bool IsMouseButtonJustPressed(MouseButton button) => GetMouseButtonPressed(mCurrentMouseState, button) && !GetMouseButtonPressed(mLastMouseState, button);

        public static bool IsMouseButtonJustReleased(MouseButton button) => !GetMouseButtonPressed(mCurrentMouseState, button) && GetMouseButtonPressed(mLastMouseState, button);

        public static bool DoubleClicked() => mDoubleClicked;

        public static bool IsMouseAnchored() => mMouseAnchored;

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
    }
}