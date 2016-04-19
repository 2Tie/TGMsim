using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace TGMsim
{
    class Preferences
    {

        bool inputting = false;
        public bool muted = false;
        public Controller nPad = new Controller();
        public int menuState = 0;//main, input change
        int selection = 0;

        int dInput = 0;

        public Preferences(Profile prof, Controller pad)
        {
            nPad = pad;

        }

        public void logic()
        {
            if (inputting)
            {
                assignKey();
                return;
            }


            if (nPad.inputV != 0 && nPad.inputV != dInput)
            {
                selection -= nPad.inputV;
                dInput = nPad.inputV;
            }

            if (nPad.inputV == 0)
                dInput = 0;


            if (menuState == 0)
            {
                if (selection >= 0)
                    selection = selection % 2;
                else
                    selection = 1;
            }
            if (menuState == 1)
            {
                if (selection >= 0)
                    selection = selection % 9;
                else
                    selection = 8;
            }


            if (nPad.inputRot1 == 1)
            {
                if (menuState == 0)
                {
                    if (selection == 0)
                        muted = !muted;
                    if (selection == 1)
                    {
                        menuState = 1;
                        selection = 0;
                        return;
                    }
                }
                if (menuState == 1)
                {
                    if (selection == 8)
                    {
                        menuState = 0;
                        selection = 1;
                    }
                    else
                        inputting = true;
                }
            }

            if (nPad.inputRot2 == 1 && menuState == 1)
            {
                menuState = 0;
                selection = 1;
            }
        }

        public void render(Graphics drawBuffer)
        {
            if (menuState == 0)
            {
                drawBuffer.DrawString("Mute: " + muted.ToString(), SystemFonts.DefaultFont, new SolidBrush(Color.White), 100, 100);
                drawBuffer.DrawString("Rebind keys", SystemFonts.DefaultFont, new SolidBrush(Color.White), 100, 120);
            }
            if (menuState == 1)
            {
                drawBuffer.DrawString("UP: " + nPad.keyUp.ToString(), SystemFonts.DefaultFont, new SolidBrush(Color.White), 100, 100);
                drawBuffer.DrawString("DOWN: " + nPad.keyDown.ToString(), SystemFonts.DefaultFont, new SolidBrush(Color.White), 100, 120);
                drawBuffer.DrawString("LEFT: " + nPad.keyLeft.ToString(), SystemFonts.DefaultFont, new SolidBrush(Color.White), 100, 140);
                drawBuffer.DrawString("RIGHT: " + nPad.keyRight.ToString(), SystemFonts.DefaultFont, new SolidBrush(Color.White), 100, 160);
                drawBuffer.DrawString("ROTATE CCW: " + nPad.keyRot1.ToString(), SystemFonts.DefaultFont, new SolidBrush(Color.White), 100, 180);
                drawBuffer.DrawString("ROTATE CW: " + nPad.keyRot2.ToString(), SystemFonts.DefaultFont, new SolidBrush(Color.White), 100, 200);
                drawBuffer.DrawString("ROTATE CCW 2/SPEED: " + nPad.keyRot3.ToString(), SystemFonts.DefaultFont, new SolidBrush(Color.White), 100, 220);
                drawBuffer.DrawString("HOLD: " + nPad.keyHold.ToString(), SystemFonts.DefaultFont, new SolidBrush(Color.White), 100, 240);
                drawBuffer.DrawString("Back", SystemFonts.DefaultFont, new SolidBrush(Color.White), 100, 260);
            }

            drawBuffer.DrawString(">", SystemFonts.DefaultFont, new SolidBrush(Color.White), 92, 100 + (20*selection));
            
            if (inputting)
                drawBuffer.DrawString("INPUT YOUR NEW KEY", SystemFonts.DefaultFont, new SolidBrush(Color.White), 200, 300);

        }

        public void assignKey()
        {
            for (int i = 44; i < 70; i++)
                if (Keyboard.IsKeyDown((Key)i) && Keyboard.IsKeyToggled((Key)i) && Keyboard.IsKeyDown((Key)i))
                {
                    if ((Key)i != nPad.keyUp && (Key)i != nPad.keyDown && (Key)i != nPad.keyLeft && (Key)i != nPad.keyRight && (Key)i != nPad.keyRot1 && (Key)i != nPad.keyRot2 && (Key)i != nPad.keyRot3 && (Key)i != nPad.keyHold)
                    switch(selection)
                    {
                        case 0://up
                            nPad.keyUp = (Key)i;
                            break;
                        case 1://down
                            nPad.keyDown = (Key)i;
                            break;
                        case 2://left
                            nPad.keyLeft = (Key)i;
                            break;
                        case 3://right
                            nPad.keyRight = (Key)i;
                            break;
                        case 4://a
                            nPad.keyRot1 = (Key)i;
                            break;
                        case 5://b
                            nPad.keyRot2 = (Key)i;
                            break;
                        case 6://c
                            nPad.keyRot3 = (Key)i;
                            break;
                        case 7://hold
                            nPad.keyHold = (Key)i;
                            break;
                    }
                    nPad.poll();
                    dInput = nPad.inputV;
                    inputting = false;
                }
        }

    }
}
