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
        List<short> inputHistory = new List<short> {0,0,0,0,0,0}; //up, down, left, right, rot1, rot2, rot3, hold, start, 7 bits for frame length (unused for lag, used for replays).
        public int lag = 0;

        public Controller()
        {
            keyRot1 = Key.I;
            keyRot2 = Key.O;
            keyRot3 = Key.P;
            keyHold = Key.Space;
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

            if (ApplicationIsActivated())
            {

                //SHIFT HISTORY
                for (int i = 0; i < 5; i++)
                {
                    inputHistory[i] = inputHistory[i + 1];
                }
                inputHistory[5] = 0;

                //log inputs

                //up or down = w or s
                if (Keyboard.IsKeyDown(keyUp))
                    inputHistory[lag] += -32768;

                if (Keyboard.IsKeyDown(keyDown))
                    inputHistory[lag] += 0x4000;

                //left or right = a or d
                if (Keyboard.IsKeyDown(keyLeft))
                    inputHistory[lag] += 0x2000;

                if (Keyboard.IsKeyDown(keyRight))
                    inputHistory[lag] += 0x1000;

                //start = enter
                if (Keyboard.IsKeyDown(keyStart))
                    inputHistory[lag] += 0x0800;

                //rot1 = o or [
                if (Keyboard.IsKeyDown(keyRot1))
                    inputHistory[lag] += 0x0400;

                if (Keyboard.IsKeyDown(keyRot3))
                    inputHistory[lag] += 0x0100;

                //rot2 = p
                if (Keyboard.IsKeyDown(keyRot2))
                    inputHistory[lag] += 0x0200;

                //hold = Space
                if (Keyboard.IsKeyDown(keyHold))
                    inputHistory[lag] += 0x0080;


                //map history to inputs

                //up or down = w or s
                if ((inputHistory[0] & -32768) != 0)
                    inputV += 1;

                if ((inputHistory[0] & 0x4000) != 0)
                    inputV -= 1;

                //left or right = a or d
                if ((inputHistory[0] & 0x2000) != 0)
                    inputH -= 1;

                if ((inputHistory[0] & 0x1000) != 0)
                    inputH += 1;

                //start = enter
                if ((inputHistory[0] & 0x0800) != 0)
                    inputStart = 1;

                //rot1 = o or [
                if ((inputHistory[0] & 0x0400) != 0)
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

                if ((inputHistory[0] & 0x0100) != 0)
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
                if ((inputHistory[0] & 0x0200) != 0)
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

                //hold = Space
                if ((inputHistory[0] & 0x0080) != 0)
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

        public void setLag(int l)
        {
            lag = l;
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
