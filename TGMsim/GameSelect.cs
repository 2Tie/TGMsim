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
        public GameSelect()
        {

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
        }
        public void render(Graphics drawBuffer)
        {
            
            //placeholder until i get arts or something
            drawBuffer.DrawString("TGM", SystemFonts.DefaultFont, new SolidBrush(Color.White), 300, 300);
            drawBuffer.DrawString("TGM2", SystemFonts.DefaultFont, new SolidBrush(Color.White), 500, 200);
            drawBuffer.DrawString("TAP", SystemFonts.DefaultFont, new SolidBrush(Color.White), 500, 300);
            drawBuffer.DrawString("TGM3", SystemFonts.DefaultFont, new SolidBrush(Color.White), 700, 300);
            drawBuffer.DrawString("ACE", SystemFonts.DefaultFont, new SolidBrush(Color.White), 700, 400);
            drawBuffer.DrawString("TGM4", SystemFonts.DefaultFont, new SolidBrush(Color.White), 900, 300);
            drawBuffer.DrawString("Bonus", SystemFonts.DefaultFont, new SolidBrush(Color.White), 400, 600);
            drawBuffer.DrawString("Preferences", SystemFonts.DefaultFont, new SolidBrush(Color.White), 800, 600);

            if (!prompt)
            {
                int vhelper = 0;
                if (menuSelection == 1)
                    vhelper = -100;
                if (menuSelection == 4)
                    vhelper = 100;

                if (menuSelection < 6)
                    drawBuffer.DrawString("↑", SystemFonts.DefaultFont, new SolidBrush(Color.White), 308 + ((int)Math.Ceiling((double)menuSelection / 2) * 200), 315 + vhelper);
                else
                    drawBuffer.DrawString("↑", SystemFonts.DefaultFont, new SolidBrush(Color.White), 408 + ((menuSelection - 6) * 400), 615);
            }
            else
            {
                drawBuffer.DrawRectangle(new Pen(new SolidBrush(Color.FromArgb(140, Color.Black))), 0, 0, 1280, 780); //dim the BG
                drawBuffer.DrawString("Are you sure you want to log out?", SystemFonts.DefaultFont, new SolidBrush(Color.White), 520, 300);
                drawBuffer.DrawString("No...", SystemFonts.DefaultFont, new SolidBrush(Color.White), 500, 320);
                drawBuffer.DrawString("Yes!", SystemFonts.DefaultFont, new SolidBrush(Color.White), 700, 320);

                drawBuffer.DrawString(">", SystemFonts.DefaultFont, new SolidBrush(Color.White), 480 + (200 * menuSelection), 320);
            }
        }
    }
}
