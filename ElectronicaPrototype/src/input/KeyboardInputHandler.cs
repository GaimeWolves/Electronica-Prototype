using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Input;

namespace Electronica.Input
{
    public class KeyboardInputHandler
    {
        private bool[] mPressedKeysLast, mPressedKeysCurrent;

        public KeyboardInputHandler()
        {
            mPressedKeysLast = new bool[1024];
            mPressedKeysCurrent = new bool[1024];
            for (int i = 0; i < mPressedKeysLast.Length; i++)
            {
                mPressedKeysCurrent[i] = false;
                mPressedKeysLast[i] = false;
            }
        }

        public void Update()
        {
            mPressedKeysCurrent.CopyTo(mPressedKeysLast, 0);

            for (int i = 0; i < mPressedKeysCurrent.Length; i++)
                mPressedKeysCurrent[i] = false;

            foreach (Keys key in Keyboard.GetState().GetPressedKeys())
                mPressedKeysCurrent[(int)key] = true;
        }

        /// <summary>
        /// Checks if a key is pressed.
        /// </summary>
        /// <param name="key">The key to check.</param>
        /// <returns>Is the button pressed?</returns>
        public bool IsKeyPressed(Keys key) 
            => mPressedKeysLast[(int)key];

        /// <summary>
        /// Checks if a key was just pressed.
        /// </summary>
        /// <param name="key">The key to check.</param>
        /// <returns>Was the key just pressed?</returns>
        public bool IsKeyJustPressed(Keys key) 
            => mPressedKeysLast[(int)key] == false && mPressedKeysCurrent[(int)key] == true;

        /// <summary>
        /// Checks if a key was just released.
        /// </summary>
        /// <param name="key">The key to check.</param>
        /// <returns>Was the key just released?</returns>
        public bool IsKeyJustReleased(Keys key) 
            => mPressedKeysLast[(int)key] == true && mPressedKeysCurrent[(int)key] == false;
    }
}
