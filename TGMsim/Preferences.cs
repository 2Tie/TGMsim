using System.Drawing;
using System.Windows.Input;

namespace TGMsim
{
    class Preferences
    {

        bool inputting = false;
        Key lastPressed;
        public bool delay = false;
        public Controller nPad = new Controller();
        public bool southpaw = false;
        public bool flashing = true;
        public int menuState = 0;//main, input change
        int selection = 0;

        int vInput = 0;
        int hInput = 0;

        public Preferences(Profile prof, Controller pad)
        {
            nPad = pad;
            
        }

        public void logic()
        {
            if (inputting)
            {
                inputting = !assignKey(selection);
                return;
            }


            if (nPad.inputV != 0 && nPad.inputV != vInput)
            {
                selection -= nPad.inputV;
                vInput = nPad.inputV;
            }

            if (nPad.inputV == 0)
                vInput = 0;


            if (menuState == 0)
            {
                if (selection >= 0)
                    selection = selection % 5;
                else
                    selection = 4;
            }
            if (menuState == 1)
            {
                if (selection >= 0)
                    selection = selection % 10;
                else
                    selection = 9;
            }


            if (nPad.inputRot1 == 1)
            {
                if (menuState == 0)
                {
                    if (selection == 4)
                    {
                        menuState = 1;
                        selection = 0;
                        return;
                    }
                }
                if (menuState == 1)
                {
                    if (selection == 9)
                    {
                        menuState = 0;
                        selection = 1;
                    }
                    else
                        inputting = true;
                }
            }

            if(nPad.inputH != 0 && menuState == 0)
            {
                if (selection == 0)
                {
                    if(hInput != nPad.inputH)
                        Audio.musVol += nPad.inputH;
                    if (Audio.musVol > 10) Audio.musVol = 10;
                    if (Audio.musVol < 0) Audio.musVol = 0;
                    Audio.setMusicVolume(Audio.musVol);
                }
                if (selection == 1)
                {
                    if (hInput != nPad.inputH)
                    {
                        Audio.sfxVol += nPad.inputH;
                        if (Audio.sfxVol > 10) Audio.sfxVol = 10;
                        if (Audio.sfxVol < 0) Audio.sfxVol = 0;
                        Audio.playSound(Audio.s_Contact);
                    }
                }
                if (selection == 2)
                    if (hInput != nPad.inputH)
                        delay = !delay;
                if (selection == 3)
                    if (hInput != nPad.inputH)
                        flashing = !flashing;
            }
            hInput = nPad.inputH;

            if (nPad.inputRot2 == 1 && menuState == 1)
            {
                menuState = 0;
                selection = 4;
            }
        }

        public void render()
        {
            SolidBrush tb = new SolidBrush(Color.White);
            if (menuState == 0)
            {
                Draw.buffer.DrawString("Music Volume: " + Audio.musVol*10 + "%", SystemFonts.DefaultFont, tb, 100, 100);
                Draw.buffer.DrawString("SFX Volume: " + Audio.sfxVol*10 + "%", SystemFonts.DefaultFont, tb, 100, 120);
                Draw.buffer.DrawString("Replicate Emulation Input Lag: " + delay, SystemFonts.DefaultFont, tb, 100, 140);
                Draw.buffer.DrawString("20G Gold Flashing: " + flashing, SystemFonts.DefaultFont, tb, 100, 160);
                Draw.buffer.DrawString("Rebind keys", SystemFonts.DefaultFont, tb, 100, 180);
            }
            if (menuState == 1)
            {
                Draw.buffer.DrawString("UP: " + nPad.keyUp.ToString(), SystemFonts.DefaultFont, tb, 100, 100);
                Draw.buffer.DrawString("DOWN: " + nPad.keyDown.ToString(), SystemFonts.DefaultFont, tb, 100, 120);
                Draw.buffer.DrawString("LEFT: " + nPad.keyLeft.ToString(), SystemFonts.DefaultFont, tb, 100, 140);
                Draw.buffer.DrawString("RIGHT: " + nPad.keyRight.ToString(), SystemFonts.DefaultFont, tb, 100, 160);
                Draw.buffer.DrawString("ROTATE CCW: " + nPad.keyRot1.ToString(), SystemFonts.DefaultFont, tb, 100, 180);
                Draw.buffer.DrawString("ROTATE CW: " + nPad.keyRot2.ToString(), SystemFonts.DefaultFont, tb, 100, 200);
                Draw.buffer.DrawString("ROTATE CCW 2/SPEED: " + nPad.keyRot3.ToString(), SystemFonts.DefaultFont, tb, 100, 220);
                Draw.buffer.DrawString("HOLD: " + nPad.keyHold.ToString(), SystemFonts.DefaultFont, tb, 100, 240);
                Draw.buffer.DrawString("START: " + nPad.keyStart.ToString(), SystemFonts.DefaultFont, tb, 100, 260);
                Draw.buffer.DrawString("Back", SystemFonts.DefaultFont, tb, 100, 280);
            }

            Draw.buffer.DrawString(">", SystemFonts.DefaultFont, tb, 92, 100 + (20*selection));
            
            if (inputting)
                Draw.buffer.DrawString("INPUT YOUR NEW KEY", SystemFonts.DefaultFont, tb, 200, 300);
                
        }

        public bool assignKey(int key)
        {
            for (int i = 1; i < 70; i++)
                if (Keyboard.IsKeyDown((Key)i) && Keyboard.IsKeyToggled((Key)i) && Keyboard.IsKeyDown((Key)i) && (Key)i != lastPressed)
                {
                    if ((Key)i != nPad.keyUp && (Key)i != nPad.keyDown && (Key)i != nPad.keyLeft && (Key)i != nPad.keyRight && (Key)i != nPad.keyRot1 && (Key)i != nPad.keyRot2 && (Key)i != nPad.keyRot3 && (Key)i != nPad.keyHold && (Key)i != nPad.keyStart)
                        switch (key)
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
                            case 8://start
                                nPad.keyStart = (Key)i;
                                break;
                        }
                    lastPressed = (Key)i;
                    nPad.poll(true);
                    vInput = nPad.inputV;
                    return true;
                }
            return false;
        }

    }
}
