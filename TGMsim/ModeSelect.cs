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
                selection = (selection - pad.inputV);
                dInput = pad.inputV;
            }

            switch(game)
            {
                case 2:
                case 3:
                case 4:
                    if (selection == 2)
                        selection = 0;
                    if (selection == -1)
                        selection = 1;
                    break;
                case 5:
                case 6:
                    if (selection == 3)
                        selection = 0;
                    if (selection == -1)
                        selection = 2;
                    break;
                default:
                    if (selection == 1)
                        selection = 0;
                    break;
            }
        }
        public void render(Graphics drawBuffer)
        {
            if (game < 4)
                drawBuffer.DrawString("Master", SystemFonts.DefaultFont, new SolidBrush(Color.White), 400, 400);
            if (game == 4)
                drawBuffer.DrawString("Roads", SystemFonts.DefaultFont, new SolidBrush(Color.White), 400, 400);
            if (game == 5)
                drawBuffer.DrawString("World", SystemFonts.DefaultFont, new SolidBrush(Color.White), 400, 400);
            if (game == 6)
                drawBuffer.DrawString("Custom", SystemFonts.DefaultFont, new SolidBrush(Color.White), 400, 400);
            drawBuffer.DrawString("→", SystemFonts.DefaultFont, new SolidBrush(Color.White), 385, 400 + 12*selection);
            switch(game)
            {
                case 2://tap
                    drawBuffer.DrawString("Death", SystemFonts.DefaultFont, new SolidBrush(Color.White), 400, 412);
                    break;
                case 3://tgm3
                    drawBuffer.DrawString("Shirase", SystemFonts.DefaultFont, new SolidBrush(Color.White), 400, 412);
                    break;
                case 4://ACE
                    drawBuffer.DrawString("Promotion Exam", SystemFonts.DefaultFont, new SolidBrush(Color.White), 400, 412);
                    break;
                case 5://TGM4
                    drawBuffer.DrawString("Rounds", SystemFonts.DefaultFont, new SolidBrush(Color.White), 400, 412);
                    drawBuffer.DrawString("Konoha", SystemFonts.DefaultFont, new SolidBrush(Color.White), 400, 424);
                    break;
                case 6://bonus
                    drawBuffer.DrawString("~Eternal Shirase~", SystemFonts.DefaultFont, new SolidBrush(Color.White), 400, 412);
                    drawBuffer.DrawString("Garbage Clearer", SystemFonts.DefaultFont, new SolidBrush(Color.White), 400, 424);
                    break;
            }
            drawBuffer.DrawString(game.ToString(), SystemFonts.DefaultFont, new SolidBrush(Color.White), 200, 700);
            drawBuffer.DrawString(selection.ToString(), SystemFonts.DefaultFont, new SolidBrush(Color.White), 200, 700);
        }
    }
}
