using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace TGMsim
{
    class Controller
    {
        public Key keyRot1, keyRot2, keyRot3, keyHold, keyUp, keyDown, keyLeft, keyRight, keyStart;

        public int inputH, inputV, inputStart, inputRot1, inputRot2, inputRot3, inputHold;
        public bool inputPressedRot1 = false, inputPressedRot2 = false, inputPressedRot3 = false, inputPressedHold = false;

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
            //reset inputs
            inputH = inputV = inputStart = inputRot1 = inputRot2 = inputRot3 = inputHold = 0;

            if (ApplicationIsActivated()) //todo: ignore background inputs optionally
            {
                //handle inputs

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
                if (Keyboard.IsKeyDown(keyRot1))
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

                if (Keyboard.IsKeyDown(keyRot3))
                {
                    if (!inputPressedRot3)
                    {
                        inputRot3 = 1;
                    }
                    inputPressedRot3 = true;
                }
                else
                {
                    inputPressedRot3 = false;
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
                {
                    if (!inputPressedHold)
                    {
                        inputHold = 1;
                    }
                    inputPressedHold = true;
                }
                else
                {
                    inputPressedHold = false;
                }
            }
        }

        public static bool ApplicationIsActivated()
        {
            var activatedHandle = GetForegroundWindow();
            if (activatedHandle == IntPtr.Zero)
            {
                return false;       // No window is currently activated
            }

            var procId = Process.GetCurrentProcess().Id;
            int activeProcId;
            GetWindowThreadProcessId(activatedHandle, out activeProcId);

            return activeProcId == procId;
        }


        [DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
        private static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern int GetWindowThreadProcessId(IntPtr handle, out int processId);
    
    }
}
