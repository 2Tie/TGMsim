using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TGMsim
{
    class CheatMenu
    {
        int selection = 0;
        int dInput = 0;
        public List<bool> cheats = new List<bool> { false, false, false, false };

        public void logic(Controller pad) 
        {
            if (pad.inputH == 1)
            //set current cheat to on
            {
                cheats[selection] = true;
            }
            if (pad.inputH == -1)//set current cheat off
            {
                cheats[selection] = false;
            }
            if (pad.inputV == -1 && dInput != -1)//go up a cheat
            {
                selection = (selection + 1) % 4;
                dInput = -1;
            }
            if (pad.inputV == 1 && dInput != 1)//go down a cheat
            {
                if (selection == 0) selection = 4;
                selection = (selection - 1);
                dInput = 1;
            }
            if (pad.inputV == 0)
                dInput = 0;
        }

        public void render(Graphics drawBuffer)
        {
            for (int i = 0; i < 4; i++)
            {
                drawBuffer.DrawString(cheats[i].ToString(), SystemFonts.DefaultFont, new SolidBrush(Color.White), 300, 200 + (15 * i));
            }
            drawBuffer.DrawString("►", SystemFonts.DefaultFont, new SolidBrush(Color.White), 294, 200 + (15 * selection));
        }
    }
}
