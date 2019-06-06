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
    class Controller //4-way stick, horizontal input overrides vertical input. Three upper buttons, a lower button, and a start button.
    {
        public Key keyRot1, keyRot2, keyRot3, keyHold, keyUp, keyDown, keyLeft, keyRight, keyStart;

        public int inputH, inputV, inputStart, inputRot1, inputRot2, inputRot3, inputHold;
        public bool inputPressedRot1 = false, inputPressedRot2 = false, inputPressedRot3 = false, inputPressedHold = false, superStart = false;
        List<short> inputHistory = new List<short> {0,0,0,0,0,0}; //up, down, left, right, rot1, rot2, rot3, hold, start, 7 bits for frame length (unused for lag, used for replays).
        public int lag = 0;
        public bool southpaw = false;
        public bool recording = false;
        public bool playback = false;
        public List<short> replay = new List<short>();
        public int progress = 0;

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

        public void poll(bool focused) 
        {
            //reset inputs
            inputH = inputV = inputStart = inputRot1 = inputRot2 = inputRot3 = inputHold = 0;
            superStart = false;

            //SHIFT HISTORY
            for (int i = 0; i < 5; i++)
            {
                inputHistory[i] = inputHistory[i + 1];
            }
            inputHistory[5] = 0;

            if (focused)
            {
                if (playback == false)
                {
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
                }
                //start = enter
                if (Keyboard.IsKeyDown(keyStart))
                    superStart = true;
            }

            if (playback)
            {
                if (progress < replay.Count)
                {
                    inputHistory[lag] = replay[progress];
                    if ((replay[progress] & 0x007f) == 0)
                        progress++;
                    else
                        replay[progress] -= 1;
                }
            }

            //map history to inputs

            //left or right = a or d
            if ((inputHistory[0] & 0x2000) != 0)
                    inputH -= 1;

            if ((inputHistory[0] & 0x1000) != 0)
                inputH += 1;

            //up or down = w or s
            if ((inputHistory[0] & -32768) != 0 && inputH == 0)
                inputV += 1;

            if ((inputHistory[0] & 0x4000) != 0 && inputH == 0)
                inputV -= 1;

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

            //add inputs to replay
            if (recording == true)
            {
                if (replay.Count == 0)
                    replay.Add(inputHistory[0]);
                else
                {
                    if ((replay[replay.Count - 1] & 0xFF80) != (inputHistory[0] & 0xFF80) || (replay[replay.Count - 1] & 0x007F) == 0x7F)
                        replay.Add(inputHistory[0]);
                    else
                        replay[replay.Count - 1] += 1;
                }
            }
        }

        public void setLag(int l)
        {
            lag = l;
        }

        public void enableRecording()
        {
            recording = true;
            replay = new List<short>();
        }

        public void enablePlayback()
        {
            recording = false;
            playback = true;
            progress = 0;
        }
    
    }
}
