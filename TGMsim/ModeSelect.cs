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
        private int prevSel;
        int dInput;

        SolidBrush active = new SolidBrush(Color.White);
        SolidBrush locked = new SolidBrush(Color.Gray);

        System.Windows.Media.MediaPlayer s_Roll = new System.Windows.Media.MediaPlayer();

        public ModeSelect(int g)
        {
            game = g;
            Audio.addSound(s_Roll, "/Res/Audio/SE/SEI_name_select.wav");
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

            if (prevSel != selection)
            {
                Audio.playSound(s_Roll);
                prevSel = selection;
            }
        }
        public void render(Graphics drawBuffer)
        {
            if (game < 4)
                drawBuffer.DrawString("Master", SystemFonts.DefaultFont, active, 300, 300);
            if (game == 4)
                drawBuffer.DrawString("Roads", SystemFonts.DefaultFont, locked, 300, 300);
            if (game == 5)
                drawBuffer.DrawString("Tetris", SystemFonts.DefaultFont, locked, 300, 300);
            if (game == 6)
                drawBuffer.DrawString("Custom", SystemFonts.DefaultFont, locked, 300, 300);
            drawBuffer.DrawString("→", SystemFonts.DefaultFont, new SolidBrush(Color.White), 285, 300 + 12*selection);
            switch(game)
            {
                case 2://tap
                    drawBuffer.DrawString("Death", SystemFonts.DefaultFont, active, 300, 312);
                    break;
                case 3://tgm3
                    drawBuffer.DrawString("Shirase", SystemFonts.DefaultFont, active, 300, 312);
                    break;
                case 4://ACE
                    drawBuffer.DrawString("Promotion Exam", SystemFonts.DefaultFont, locked, 300, 312);
                    break;
                case 5://TGM4
                    drawBuffer.DrawString("Bloxeed", SystemFonts.DefaultFont, locked, 300, 312);
                    drawBuffer.DrawString("Flash Point", SystemFonts.DefaultFont, locked, 300, 324);
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
