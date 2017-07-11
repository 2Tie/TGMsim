using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TGMsim
{
    class GameSelect
    {
        public int menuSelection = 1;
        private int prevSel = 0;
        public int pSel = 0;
        public bool prompt = false;
        int hInput = 0;
        int vInput = 0;
        Rectangle curBox, destBox;

        System.Windows.Media.MediaPlayer s_Select = new System.Windows.Media.MediaPlayer();


        public GameSelect()
        {
            destBox = new Rectangle(0, 150, 200, 300);
            curBox = destBox;

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
            int bhite = 450;
            if (menuSelection == 0 || menuSelection == 2 || menuSelection == 5)
                bhite = 150;
            if (menuSelection == 1 || menuSelection == 3 || menuSelection == 4)
                bhite = 300;
            if (menuSelection > 6)
                bhite = 149;
            int bwith = 200;
            if (menuSelection > 6)
                bwith = 400;
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
            curBox.X += (destBox.X - curBox.X) / 2;
            curBox.Y += (destBox.Y - curBox.Y) / 2;
            curBox.Width += (destBox.Width - curBox.Width) / 2;
            curBox.Height += (destBox.Height - curBox.Height) / 2;


        }
        public void render(Graphics drawBuffer)
        {

            //placeholder until i get arts or something
            drawBuffer.DrawString("SEGA", SystemFonts.DefaultFont, new SolidBrush(Color.White), 100, 100);
            drawBuffer.DrawString("TGM", SystemFonts.DefaultFont, new SolidBrush(Color.White), 100, 200);
            drawBuffer.DrawString("GM2", SystemFonts.DefaultFont, new SolidBrush(Color.White), 300, 100);
            drawBuffer.DrawString("TAP", SystemFonts.DefaultFont, new SolidBrush(Color.White), 300, 200);
            drawBuffer.DrawString("GM3", SystemFonts.DefaultFont, new SolidBrush(Color.White), 500, 200);
            drawBuffer.DrawString("ACE", SystemFonts.DefaultFont, new SolidBrush(Color.White), 500, 300);
            drawBuffer.DrawString("GMX", SystemFonts.DefaultFont, new SolidBrush(Color.White), 700, 200);
            drawBuffer.DrawString("Bonus", SystemFonts.DefaultFont, new SolidBrush(Color.White), 200, 500);
            drawBuffer.DrawString("Preferences", SystemFonts.DefaultFont, new SolidBrush(Color.White), 600, 500);


            //selection box
            drawBuffer.DrawRectangle(new Pen(new SolidBrush(Color.White)), curBox);


            if (prompt)
            {
                drawBuffer.FillRectangle(new SolidBrush(Color.FromArgb(160, Color.Black)), 0, 0, 800, 600); //dim the BG
                drawBuffer.DrawString("Are you sure you want to log out?", SystemFonts.DefaultFont, new SolidBrush(Color.White), 320, 200);
                drawBuffer.DrawString("No...", SystemFonts.DefaultFont, new SolidBrush(Color.White), 300, 220);
                drawBuffer.DrawString("Yes!", SystemFonts.DefaultFont, new SolidBrush(Color.White), 500, 220);

                drawBuffer.DrawString(">", SystemFonts.DefaultFont, new SolidBrush(Color.White), 290 + (200 * pSel), 220);
            }
        }
    }
}
