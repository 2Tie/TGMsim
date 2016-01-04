using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace TGMsim
{
    class Controller
    {
        public Key keyRot1, keyRot2, keyRot3, keyHold, keyUp, keyDown, keyLeft, keyRight, keyStart;

        public int inputH, inputV, inputStart, inputRot1, inputRot2, inputHold;
        public bool inputPressedRot1 = false, inputPressedRot2 = false;

        public Controller()
        {
            keyRot1 = Key.I;
            keyRot2 = Key.O;
            keyRot3 = Key.P;
            keyHold = Key.K;
            keyUp = Key.W;
            keyDown = Key.S;
            keyLeft = Key.A;
            keyRight = Key.D;
            keyStart = Key.Enter;
        }

        public void poll() 
        {
            if (true) //todo: ignore background inputs optionally
            {
                //handle inputs
                //reset inputs
                inputH = inputV = inputStart = inputRot1 = inputRot2 = inputHold = 0;

                //up or down = w or s
                if (Keyboard.IsKeyDown(keyUp))
                    inputV += 1;

                if (Keyboard.IsKeyDown(keyDown))
                    inputV -= 1;

                //left or right = a or d
                if (Keyboard.IsKeyDown(keyLeft))
                    inputH -= 1;

                if (Keyboard.IsKeyDown(keyRight))
                    inputH += 1;

                //start = enter
                if (Keyboard.IsKeyDown(keyStart))
                    inputStart = 1;

                //rot1 = o or [
                if (Keyboard.IsKeyDown(keyRot1) || Keyboard.IsKeyDown(keyRot3))
                {
                    if (!inputPressedRot1)
                    {
                        inputRot1 = 1;
                    }
                    inputPressedRot1 = true;
                }
                else
                {
                    inputPressedRot1 = false;
                }

                //rot2 = p
                if (Keyboard.IsKeyDown(keyRot2))
                {
                    if (!inputPressedRot2)
                    {
                        inputRot2 = 1;
                    }
                    inputPressedRot2 = true;
                }
                else
                {
                    inputPressedRot2 = false;
                }

                //hold = K
                if (Keyboard.IsKeyDown(keyHold))
                    inputHold = 1;
            }
        }
    }
}
