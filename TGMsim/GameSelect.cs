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
        int dInput = 0;
        public GameSelect()
        {

        }
        public void logic(Controller pad)
        {
            if(pad.inputH != dInput)
            {
                menuSelection += pad.inputH;
                if (menuSelection == -1)
                    menuSelection = 0;
                if (menuSelection == 6)
                    menuSelection = 5;
                dInput = pad.inputH;
            }
        }
        public void render(Graphics drawBuffer)
        {
            drawBuffer.DrawString("TGM", SystemFonts.DefaultFont, new SolidBrush(Color.White), 200, 400);
            drawBuffer.DrawString("TGM2", SystemFonts.DefaultFont, new SolidBrush(Color.White), 300, 400);
            drawBuffer.DrawString("TAP", SystemFonts.DefaultFont, new SolidBrush(Color.White), 400, 400);
            drawBuffer.DrawString("TGM3", SystemFonts.DefaultFont, new SolidBrush(Color.White), 500, 400);
            drawBuffer.DrawString("Bonus", SystemFonts.DefaultFont, new SolidBrush(Color.White), 600, 400);
            drawBuffer.DrawString("Preferences", SystemFonts.DefaultFont, new SolidBrush(Color.White), 700, 400);


            drawBuffer.DrawString("↑", SystemFonts.DefaultFont, new SolidBrush(Color.White), 208 + (menuSelection*100), 415);
        }
    }
}
