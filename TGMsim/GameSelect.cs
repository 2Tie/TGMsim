using System.Collections.Generic;
using System.Drawing;

namespace TGMsim
{
    class GameSelect
    {
        public int menuSelection = 1;
        private int prevSel = 1;
        public int pSel = 0;
        public bool prompt = false;
        int hInput = 0;
        int vInput = 0;
        Rectangle curBox, destBox;

        SolidBrush tb;

        List<string> desc = new List<string> {
            "WHERE IT ALL BEGAN", //SEGA
            "SLOW AND STEADY", //TGM1
            "PICKING UP THE PACE", //TGM2
            "ROUNDING OUT THE MODES", //TAP
            "MORE CONTROL, MORE SPEED", //TGM3
            "TEST A VARIETY OF SKILLS", //ACE
            "PUSH YOURSELF TO THE MAX!", //GMX
            "FUN EXTRAS", //BONUS
            "CHANGE YOUR SETTINGS" //PREFERENCES
        };

        float tempX = 0;
        float tempY = 150;
        float tempW = 200;
        float tempH = 299;

        System.Windows.Media.MediaPlayer s_Select = new System.Windows.Media.MediaPlayer();


        public GameSelect()
        {
            destBox = new Rectangle(0, 150, 200, 299);
            curBox = destBox;
            tb = new SolidBrush(Color.White);

            Audio.addSound(s_Select, "/Res/Audio/SE/SEI_roll.wav");
        }
        public void logic(Controller pad)
        {
            if(pad.inputH != hInput && pad.inputH != 0)
            {
                if (!prompt)
                {
                    switch (menuSelection)
                    {
                        case 0:
                            if (pad.inputH == -1)
                                menuSelection = 6;
                            if (pad.inputH == 1)
                                menuSelection = 2;
                            break;
                        case 1:
                            if (pad.inputH == -1)
                                menuSelection = 6;
                            if (pad.inputH == 1)
                                menuSelection = 3;
                            break;
                        case 2:
                            if (pad.inputH == -1)
                                menuSelection = 0;
                            if (pad.inputH == 1)
                                menuSelection = 4;
                            break;
                        case 3:
                            if (pad.inputH == -1)
                                menuSelection = 1;
                            if (pad.inputH == 1)
                                menuSelection = 4;
                            break;
                        case 4:
                        case 5:
                            if (pad.inputH == -1)
                                menuSelection = 3;
                            if (pad.inputH == 1)
                                menuSelection = 6;
                            break;
                        case 6:
                            if (pad.inputH == -1)
                                menuSelection = 4;
                            if (pad.inputH == 1)
                                menuSelection = 1;
                            break;
                        case 7:
                            menuSelection = 8;
                            break;
                        case 8:
                            menuSelection = 7;
                            break;
                    }
                }
                else
                {
                    if (pad.inputH == 1)
                        pSel = 1;
                    if (pad.inputH == -1)
                        pSel = 0;
                }
            }
            if (pad.inputV != vInput && pad.inputV != 0)
            {
                if (!prompt)
                {
                    switch (menuSelection)
                    {
                        case 0:
                            if (pad.inputV == 1)
                                menuSelection = 7;
                            else
                                menuSelection = 1;
                            break;
                        case 1:
                            if (pad.inputV == 1)
                                menuSelection = 0;
                            else
                                menuSelection = 7;
                            break;
                        case 2:
                            if (pad.inputV == 1)
                                menuSelection = 7;
                            else
                                menuSelection = 3;
                            break;
                        case 3:
                            if (pad.inputV == -1)
                                menuSelection = 7;
                            else
                                menuSelection = 2;
                            break;
                        case 4:
                            if (pad.inputV == 1)
                                menuSelection = 8;
                            else
                                menuSelection = 5;
                            break;
                        case 5:
                            if (pad.inputV == 1)
                                menuSelection = 4;
                            else
                                menuSelection = 8;
                            break;
                        case 6:
                            menuSelection = 8;
                            break;
                        case 7:
                            menuSelection = 1;
                            break;
                        case 8:
                            menuSelection = 6;
                            break;
                    }
                }
            }

            hInput = pad.inputH;
            vInput = pad.inputV;

            if (menuSelection != prevSel)
            {
                Audio.playSound(s_Select);
                prevSel = menuSelection;
            }

            //box logic
            int bhite = 449;
            if (menuSelection == 0 || menuSelection == 2 || menuSelection == 5)
                bhite = 149;
            if (menuSelection == 1 || menuSelection == 3 || menuSelection == 4)
                bhite = 299;
            if (menuSelection > 6)
                bhite = 119;
            int bwith = 199;
            if (menuSelection > 6)
                bwith = 399;
            int bX = 200 * (menuSelection-1);
            if (menuSelection == 0)
                bX += 200;
            if (menuSelection > 2)
                bX -= 200;
            if (menuSelection > 4)
                bX -= 200;
            if (menuSelection > 6)
                bX = (menuSelection - 7) * 400;
            int bY = 0;
            if (menuSelection == 1 || menuSelection == 3)
                bY = 150;
            if (menuSelection == 5)
                bY = 300;
            if (menuSelection > 6)
                bY = 450;
            destBox = new Rectangle(bX, bY, bwith, bhite);
            tempX += (float)(destBox.X - curBox.X) / 2;
            tempY += (float)(destBox.Y - curBox.Y) / 2;
            tempW += (float)(destBox.Width - curBox.Width) / 2;
            tempH += (float)(destBox.Height - curBox.Height) / 2;

            curBox = new Rectangle((int)tempX, (int)tempY, (int)tempW, (int)tempH);


        }
        public void render(Graphics drawBuffer, Font f)
        {
            drawBuffer.DrawImageUnscaled(Image.FromFile("Res/GFX/menus/bg0.png"), 0, 0);
            //placeholder until i get arts or something
            drawBuffer.DrawString("SEGA", f, tb, 80, 100);
            drawBuffer.DrawString("TGM", f, tb, 80, 200);
            drawBuffer.DrawString("GM2", f, tb, 290, 100);
            drawBuffer.DrawString("TAP", f, tb, 290, 200);
            drawBuffer.DrawString("GM3", f, tb, 480, 200);
            drawBuffer.DrawString("ACE", f, tb, 480, 300);
            drawBuffer.DrawString("GMX", f, tb, 680, 200);
            drawBuffer.DrawString("BONUS", f, tb, 180, 500);
            drawBuffer.DrawString("PREFERENCES", f, tb, 560, 500);
            //replay message
            drawBuffer.DrawString("PRESS HOLD TO LOAD A REPLAY!", f, tb, 280, 2);
            //game description bar
            drawBuffer.DrawString(desc[menuSelection], f, tb, 400 - (9 * desc[menuSelection].Length / 2), 580);


            //selection box
            drawBuffer.DrawRectangle(new Pen(tb), curBox);


            if (prompt)
            {
                drawBuffer.FillRectangle(new SolidBrush(Color.FromArgb(160, Color.Black)), 0, 0, 800, 600); //dim the BG
                drawBuffer.DrawString("Are you sure you want to log out?", SystemFonts.DefaultFont, tb, 320, 200);
                drawBuffer.DrawString("No...", SystemFonts.DefaultFont, tb, 300, 220);
                drawBuffer.DrawString("Yes!", SystemFonts.DefaultFont, tb, 500, 220);

                drawBuffer.DrawString(">", SystemFonts.DefaultFont, tb, 290 + (200 * pSel), 220);
            }
        }
    }
}
