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
        int hInput = 0;
        int vInput = 0;
        public GameSelect()
        {

        }
        public void logic(Controller pad)
        {
            if(pad.inputH != hInput)
            {
                if (menuSelection == 0 && pad.inputH == -1)
                {
                    menuSelection = 3;
                    hInput = pad.inputH;
                    return;
                }
                if (menuSelection == 0 && pad.inputH == 1)
                {
                    menuSelection = 1;
                    hInput = pad.inputH;
                    return;
                }
                if (menuSelection == 3 && pad.inputH == 1)
                {
                    menuSelection = 0;
                    hInput = pad.inputH;
                    return;
                }
                if (menuSelection == 3 && pad.inputH == -1)
                {
                    menuSelection = 2;
                    hInput = pad.inputH;
                    return;
                }
                if (menuSelection == 4 && pad.inputH != 0)
                    menuSelection = 6;
                if (menuSelection == 5 && pad.inputH != 0)
                    menuSelection = 4;

                if (menuSelection == 1 || menuSelection == 2)
                    menuSelection += pad.inputH;

                if (menuSelection == 6)
                    menuSelection = 5;
                hInput = pad.inputH;
            }
            if (pad.inputV != vInput)
            {
                if (pad.inputV != 0)
                {
                    //menuSelection += pad.inputH;
                    if (menuSelection == 0 || menuSelection == 1)
                    {
                        menuSelection = 4;
                        vInput = pad.inputV;
                        return;
                    }
                    if (menuSelection == 2 || menuSelection == 3)
                    {
                        menuSelection = 5;
                        vInput = pad.inputV;
                        return;
                    }
                    if (menuSelection == 4)
                    {
                        menuSelection = 0;
                        vInput = pad.inputV;
                        return;
                    }
                    if (menuSelection == 5)
                    {
                        menuSelection = 3;
                        vInput = pad.inputV;
                        return;
                    }
                }
                vInput = pad.inputV;
            }
        }
        public void render(Graphics drawBuffer)
        {
            //placeholder until i get arts or something
            drawBuffer.DrawString("TGM", SystemFonts.DefaultFont, new SolidBrush(Color.White), 200, 400);
            drawBuffer.DrawString("TGM2", SystemFonts.DefaultFont, new SolidBrush(Color.White), 400, 400);
            drawBuffer.DrawString("TAP", SystemFonts.DefaultFont, new SolidBrush(Color.White), 600, 400);
            drawBuffer.DrawString("TGM3", SystemFonts.DefaultFont, new SolidBrush(Color.White), 800, 400);
            drawBuffer.DrawString("Bonus", SystemFonts.DefaultFont, new SolidBrush(Color.White), 300, 600);
            drawBuffer.DrawString("Preferences", SystemFonts.DefaultFont, new SolidBrush(Color.White), 700, 600);

            if (menuSelection < 4)
                drawBuffer.DrawString("↑", SystemFonts.DefaultFont, new SolidBrush(Color.White), 208 + (menuSelection*200), 415);
            else
                drawBuffer.DrawString("↑", SystemFonts.DefaultFont, new SolidBrush(Color.White), 308 + ((menuSelection - 4) * 400), 615);
        }
    }
}
