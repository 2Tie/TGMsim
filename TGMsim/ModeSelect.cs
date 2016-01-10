using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TGMsim
{
    class ModeSelect
    {
        public int game;
        public int selection;
        int dInput;

        public ModeSelect(int g)
        {
            game = g;
        }
        public void logic(Controller pad)
        {
            if (pad.inputV != dInput)
            {
                selection += pad.inputV;
                if (selection == -1)
                    selection = 0;
                dInput = pad.inputH;
            }
            switch(game)
            {
                case 2:
                case 3:
                case 4:
                    if (selection == 2)
                        selection = 1;
                    break;
                default:
                    if (selection == 1)
                        selection = 0;
                    break;
            }
        }
        public void render(Graphics drawBuffer)
        {
            if (game == 4)
            {
                drawBuffer.DrawString("Konoha", SystemFonts.DefaultFont, new SolidBrush(Color.White), 400, 400);
            }
            else
                drawBuffer.DrawString("Master", SystemFonts.DefaultFont, new SolidBrush(Color.White), 400, 400);
            drawBuffer.DrawString("→", SystemFonts.DefaultFont, new SolidBrush(Color.White), 385, 400 + 12*selection);
            switch(game)
            {
                case 2://tap
                    drawBuffer.DrawString("Death", SystemFonts.DefaultFont, new SolidBrush(Color.White), 400, 412);
                    break;
                case 3://tgm3
                    drawBuffer.DrawString("Shirase", SystemFonts.DefaultFont, new SolidBrush(Color.White), 400, 412);
                    break;
                case 4://bonus
                    drawBuffer.DrawString("~Eternal Shirase~", SystemFonts.DefaultFont, new SolidBrush(Color.White), 400, 412);
                    break;
            }
            drawBuffer.DrawString(game.ToString(), SystemFonts.DefaultFont, new SolidBrush(Color.White), 200, 700);
        }
    }
}
