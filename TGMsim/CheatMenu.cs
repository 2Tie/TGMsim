using System.Collections.Generic;
using System.Drawing;

namespace TGMsim
{
    class CheatMenu
    {
        int selection = 0;
        int dInput = 0;
        public List<bool> cheats = new List<bool> { false, false, false, false, false, false, false };

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

        public void render()
        {
            for (int i = 0; i < cheats.Count; i++)
            {
                Draw.buffer.DrawString(cheats[i].ToString(), SystemFonts.DefaultFont, Draw.wb, 300, 200 + (15 * i));
            }
            Draw.buffer.DrawString("GOD MODE", SystemFonts.DefaultFont, Draw.wb, 220, 200);
            Draw.buffer.DrawString("20G", SystemFonts.DefaultFont, Draw.wb, 220, 215);
            Draw.buffer.DrawString("0G", SystemFonts.DefaultFont, Draw.wb, 220, 230);
            Draw.buffer.DrawString("BIG MODE", SystemFonts.DefaultFont, Draw.wb, 220, 245);
            Draw.buffer.DrawString("MUTE MUSIC", SystemFonts.DefaultFont, Draw.wb, 220, 260);
            Draw.buffer.DrawString("4W", SystemFonts.DefaultFont, Draw.wb, 220, 275);
            Draw.buffer.DrawString("NO TORIKAN", SystemFonts.DefaultFont, Draw.wb, 220, 290);
            Draw.buffer.DrawString("►", SystemFonts.DefaultFont, Draw.wb, 294, 200 + (15 * selection));
        }
    }
}
