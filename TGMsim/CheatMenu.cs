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
        public List<bool> cheats = new List<bool> { false, false, false, false, false, false };

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
                selection = (selection + 1) % cheats.Count;
                dInput = -1;
            }
            if (pad.inputV == 1 && dInput != 1)//go down a cheat
            {
                if (selection == 0) selection = cheats.Count;
                selection = (selection - 1);
                dInput = 1;
            }
            if (pad.inputV == 0)
                dInput = 0;
        }

        public void render(Graphics drawBuffer)
        {
            for (int i = 0; i < cheats.Count; i++)
            {
                drawBuffer.DrawString(cheats[i].ToString(), SystemFonts.DefaultFont, new SolidBrush(Color.White), 300, 200 + (15 * i));
            }
            drawBuffer.DrawString("GOD MODE", SystemFonts.DefaultFont, new SolidBrush(Color.White), 220, 200);
            drawBuffer.DrawString("20G", SystemFonts.DefaultFont, new SolidBrush(Color.White), 220, 215);
            drawBuffer.DrawString("0G", SystemFonts.DefaultFont, new SolidBrush(Color.White), 220, 230);
            drawBuffer.DrawString("BIG MODE", SystemFonts.DefaultFont, new SolidBrush(Color.White), 220, 245);
            drawBuffer.DrawString("MUTE MUSIC", SystemFonts.DefaultFont, new SolidBrush(Color.White), 220, 260);
            drawBuffer.DrawString("4W", SystemFonts.DefaultFont, new SolidBrush(Color.White), 220, 275);
            drawBuffer.DrawString("►", SystemFonts.DefaultFont, new SolidBrush(Color.White), 294, 200 + (15 * selection));
        }
    }
}
