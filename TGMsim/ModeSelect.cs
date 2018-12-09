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

        public int variant = 0;

        List<List<string>> modes = new List<List<string>>();


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
            if (game == 7 && selection == 1)//zen
            {
                if (pad.inputH == 1)
                    variant = 1;
                if (pad.inputH == -1)
                    variant = 0;
            }
            if (game == 6 && selection == 0)//dynamo
            {
                if (pad.inputH == 1)
                    variant = 1;
                if (pad.inputH == -1)
                    variant = 0;
            }

            switch (game)
            {
                case 3:
                case 4:
                case 5:
                case 6:
                    if (selection == 2)
                        selection = 0;
                    if (selection == -1)
                        selection = 1;
                    break;
                case 0:
                    if (selection == 3)
                        selection = 0;
                    if (selection == -1)
                        selection = 2;
                    break;
                case 7:
                    if (selection == 6)
                        selection = 0;
                    if (selection == -1)
                        selection = 5;
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
            if (game == 0)
                drawBuffer.DrawString("Tetris", SystemFonts.DefaultFont, active, 300, 300);
            else if (game < 5)
                drawBuffer.DrawString("Master", SystemFonts.DefaultFont, active, 300, 300);
            if (game == 5)
                drawBuffer.DrawString("Roads", SystemFonts.DefaultFont, locked, 300, 300);
            if (game == 6)
            {
                drawBuffer.DrawString("Dynamo", SystemFonts.DefaultFont, active, 300, 300);
                if (variant < 4)
                    for (int i = 0; i < variant; i++)
                    {
                        drawBuffer.DrawString("+", SystemFonts.DefaultFont, active, 342 + 8 * i, 300);
                    }
                else
                    drawBuffer.DrawString("*", SystemFonts.DefaultFont, active, 342, 300);
            }
            if (game == 7)
                drawBuffer.DrawString("Custom", SystemFonts.DefaultFont, locked, 300, 300);
            drawBuffer.DrawString("→", SystemFonts.DefaultFont, new SolidBrush(Color.White), 285, 300 + 12*selection);
            switch(game)
            {
                case 0://SEGA
                    drawBuffer.DrawString("Bloxeed", SystemFonts.DefaultFont, locked, 300, 312);
                    drawBuffer.DrawString("Flash Point", SystemFonts.DefaultFont, locked, 300, 324);
                    break;
                case 3://tap
                    drawBuffer.DrawString("Death", SystemFonts.DefaultFont, active, 300, 312);
                    break;
                case 4://tgm3
                    drawBuffer.DrawString("Shirase", SystemFonts.DefaultFont, active, 300, 312);
                    break;
                case 5://ACE
                    drawBuffer.DrawString("Promotion Exam", SystemFonts.DefaultFont, locked, 300, 312);
                    break;
                case 6://GMX
                    drawBuffer.DrawString("Endura", SystemFonts.DefaultFont, locked, 300, 312);
                    break;
                case 7://bonus
                    if (variant == 0)
                        drawBuffer.DrawString("Miner", SystemFonts.DefaultFont, new SolidBrush(Color.White), 300, 312);
                    else
                        drawBuffer.DrawString("Zen", SystemFonts.DefaultFont, new SolidBrush(Color.White), 300, 312);
                    drawBuffer.DrawString("Garbage Clearer", SystemFonts.DefaultFont, new SolidBrush(Color.White), 300, 324);
                    drawBuffer.DrawString("20G Practice", SystemFonts.DefaultFont, new SolidBrush(Color.White), 300, 336);
                    drawBuffer.DrawString("Icy Shirase", SystemFonts.DefaultFont, new SolidBrush(Color.White), 300, 348);
                    drawBuffer.DrawString("Big Bravo Mania", SystemFonts.DefaultFont, new SolidBrush(Color.White), 300, 360);
                    break;
            }
        }
    }
}
