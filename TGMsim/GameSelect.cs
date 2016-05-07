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
        public int menuSelection = 0;
        public bool prompt = false;
        int hInput = 0;
        int vInput = 0;
        Rectangle curBox, destBox;


        public GameSelect()
        {
            destBox = new Rectangle(0, 0, 200, 400);
            curBox = destBox;
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
                                menuSelection = 5;
                            if (pad.inputH == 1)
                                menuSelection = 2;
                            break;
                        case 1:
                        case 2:
                            if (pad.inputH == -1)
                                menuSelection = 0;
                            if (pad.inputH == 1)
                                menuSelection = 3;
                            break;
                        case 3:
                        case 4:
                            if (pad.inputH == -1)
                                menuSelection = 2;
                            if (pad.inputH == 1)
                                menuSelection = 5;
                            break;
                        case 5:
                            if (pad.inputH == -1)
                                menuSelection = 3;
                            if (pad.inputH == 1)
                                menuSelection = 0;
                            break;
                        case 6:
                            menuSelection = 7;
                            break;
                        case 7:
                            menuSelection = 6;
                            break;
                    }
                }
                else
                {
                    if (pad.inputH == 1)
                        menuSelection = 1;
                    if (pad.inputH == -1)
                        menuSelection = 0;
                }
            }
            if (pad.inputV != vInput && pad.inputV != 0)
            {
                switch (menuSelection)
                {
                    case 0:
                        menuSelection = 6;
                        break;
                    case 1:
                        if (pad.inputV == 1)
                            menuSelection = 6;
                        else
                            menuSelection = 2;
                        break;
                    case 2:
                        if (pad.inputV == -1)
                            menuSelection = 2;
                        else
                            menuSelection = 1;
                        break;
                    case 3:
                        if (pad.inputV == 1)
                            menuSelection = 7;
                        else
                            menuSelection = 4;
                        break;
                    case 4:
                        if (pad.inputV == 1)
                            menuSelection = 3;
                        else
                            menuSelection = 7;
                        break;
                    case 5:
                        menuSelection = 7;
                        break;
                    case 6:
                        menuSelection = 0;
                        break;
                    case 7:
                        menuSelection = 5;
                        break;
                }
            }

            hInput = pad.inputH;
            vInput = pad.inputV;


            //box logic
            int bhite = 400;
            if (menuSelection == 1 || menuSelection == 4 || menuSelection > 5)
                bhite = 200;
            int bwith = 200;
            if (menuSelection > 5)
                bwith = 400;
            int bX = 200 * menuSelection;
            if (menuSelection > 1)
                bX -= 200;
            if (menuSelection > 3)
                bX -= 200;
            destBox = new Rectangle(bX, 0, 200, bhite);
            curBox.X += (destBox.X - curBox.X) / 2;
            curBox.Y += (destBox.Y - curBox.Y) / 2;
            curBox.Width += (destBox.Width - curBox.Width) / 2;
            curBox.Height += (destBox.Height - curBox.Height) / 2;


        }
        public void render(Graphics drawBuffer)
        {
            
            //placeholder until i get arts or something
            drawBuffer.DrawString("TGM", SystemFonts.DefaultFont, new SolidBrush(Color.White), 100, 200);
            drawBuffer.DrawString("TGM2", SystemFonts.DefaultFont, new SolidBrush(Color.White), 300, 100);
            drawBuffer.DrawString("TAP", SystemFonts.DefaultFont, new SolidBrush(Color.White), 300, 200);
            drawBuffer.DrawString("TGM3", SystemFonts.DefaultFont, new SolidBrush(Color.White), 500, 200);
            drawBuffer.DrawString("ACE", SystemFonts.DefaultFont, new SolidBrush(Color.White), 500, 300);
            drawBuffer.DrawString("TGM4", SystemFonts.DefaultFont, new SolidBrush(Color.White), 700, 200);
            drawBuffer.DrawString("Bonus", SystemFonts.DefaultFont, new SolidBrush(Color.White), 200, 500);
            drawBuffer.DrawString("Preferences", SystemFonts.DefaultFont, new SolidBrush(Color.White), 600, 500);


            //selection box
            drawBuffer.DrawRectangle(new Pen(new SolidBrush(Color.White)), curBox);


            if (!prompt)
            {
                int vhelper = 0;
                if (menuSelection == 1)
                    vhelper = -100;
                if (menuSelection == 4)
                    vhelper = 100;

                if (menuSelection < 6)
                    drawBuffer.DrawString("↑", SystemFonts.DefaultFont, new SolidBrush(Color.White), 108 + ((int)Math.Ceiling((double)menuSelection / 2) * 200), 215 + vhelper);
                else
                    drawBuffer.DrawString("↑", SystemFonts.DefaultFont, new SolidBrush(Color.White), 208 + ((menuSelection - 6) * 400), 515);
            }
            else
            {
                drawBuffer.FillRectangle(new SolidBrush(Color.FromArgb(140, Color.Black)), 0, 0, 800, 600); //dim the BG
                drawBuffer.DrawString("Are you sure you want to log out?", SystemFonts.DefaultFont, new SolidBrush(Color.White), 320, 200);
                drawBuffer.DrawString("No...", SystemFonts.DefaultFont, new SolidBrush(Color.White), 300, 220);
                drawBuffer.DrawString("Yes!", SystemFonts.DefaultFont, new SolidBrush(Color.White), 500, 220);

                drawBuffer.DrawString(">", SystemFonts.DefaultFont, new SolidBrush(Color.White), 290 + (200 * menuSelection), 220);
            }
        }
    }
}
