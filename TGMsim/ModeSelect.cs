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
                    if (selection == 4)
                        selection = 0;
                    if (selection == -1)
                        selection = 3;
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
                drawBuffer.DrawString("Master", SystemFonts.DefaultFont, new SolidBrush(Color.White), 300, 300);
            if (game == 4)
                drawBuffer.DrawString("Roads", SystemFonts.DefaultFont, new SolidBrush(Color.White), 300, 300);
            if (game == 5)
                drawBuffer.DrawString("World", SystemFonts.DefaultFont, new SolidBrush(Color.White), 300, 300);
            if (game == 6)
                drawBuffer.DrawString("Custom", SystemFonts.DefaultFont, new SolidBrush(Color.White), 300, 300);
            drawBuffer.DrawString("→", SystemFonts.DefaultFont, new SolidBrush(Color.White), 285, 300 + 12*selection);
            switch(game)
            {
                case 2://tap
                    drawBuffer.DrawString("Death", SystemFonts.DefaultFont, new SolidBrush(Color.White), 300, 312);
                    break;
                case 3://tgm3
                    drawBuffer.DrawString("Shirase", SystemFonts.DefaultFont, new SolidBrush(Color.White), 300, 312);
                    break;
                case 4://ACE
                    drawBuffer.DrawString("Promotion Exam", SystemFonts.DefaultFont, new SolidBrush(Color.White), 300, 312);
                    break;
                case 5://TGM4
                    drawBuffer.DrawString("Rounds", SystemFonts.DefaultFont, new SolidBrush(Color.White), 300, 312);
                    drawBuffer.DrawString("Konoha", SystemFonts.DefaultFont, new SolidBrush(Color.White), 300, 324);
                    break;
                case 6://bonus
                    drawBuffer.DrawString("~Eternal Shirase~", SystemFonts.DefaultFont, new SolidBrush(Color.White), 300, 312);
                    drawBuffer.DrawString("Garbage Clearer", SystemFonts.DefaultFont, new SolidBrush(Color.White), 300, 324);
                    drawBuffer.DrawString("20G Practice", SystemFonts.DefaultFont, new SolidBrush(Color.White), 300, 336);
                    break;
            }
        }
    }
}
