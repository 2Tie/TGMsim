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
        int dInputV;
        int dInputH;

        public int variant = 0;

        public struct ModeSelObj
        {
            public List<string> names;
            public Mode.ModeType id;
            public bool enabled;

            public ModeSelObj(List<string> n, Mode.ModeType t, bool e)
            {
                names = n;
                id = t;
                enabled = e;
            }
        }

        public List<List<ModeSelObj>> modes = new List<List<ModeSelObj>>();


        SolidBrush active = new SolidBrush(Color.White);
        SolidBrush locked = new SolidBrush(Color.Gray);

        System.Windows.Media.MediaPlayer s_Roll = new System.Windows.Media.MediaPlayer();

        public ModeSelect(int g)
        {
            game = g;
            Audio.addSound(s_Roll, "/Res/Audio/SE/SEI_name_select.wav");

            List<ModeSelObj> tmp = new List<ModeSelObj>();//sega
            tmp.Add(new ModeSelObj(new List<string> { "Tetris" }, Mode.ModeType.SEGA, true));
            tmp.Add(new ModeSelObj(new List<string> { "Bloxeed" }, Mode.ModeType.BLOX, true));
            tmp.Add(new ModeSelObj(new List<string> { "Flash Point" }, Mode.ModeType.SEGA, false)); //TODO
            modes.Add(tmp);
            tmp = new List<ModeSelObj>();//tgm1
            tmp.Add(new ModeSelObj(new List<string> { "Master" }, Mode.ModeType.MASTER, true));
            modes.Add(tmp);
            tmp = new List<ModeSelObj>();//tgm2
            tmp.Add(new ModeSelObj(new List<string> { "Master" }, Mode.ModeType.MASTER, true));
            tmp.Add(new ModeSelObj(new List<string> { "Cardcaptor Sakura" }, Mode.ModeType.MASTER, false)); //TODO
            modes.Add(tmp);
            tmp = new List<ModeSelObj>();//tap
            tmp.Add(new ModeSelObj(new List<string> { "Normal" }, Mode.ModeType.SEGA, false));
            tmp.Add(new ModeSelObj(new List<string> { "Master" }, Mode.ModeType.MASTER, true));
            tmp.Add(new ModeSelObj(new List<string> { "TGM+" }, Mode.ModeType.PLUS, true));
            tmp.Add(new ModeSelObj(new List<string> { "Death" }, Mode.ModeType.DEATH, true));
            modes.Add(tmp);
            tmp = new List<ModeSelObj>();//tgm3
            tmp.Add(new ModeSelObj(new List<string> { "Easy" }, Mode.ModeType.SEGA, false));
            tmp.Add(new ModeSelObj(new List<string> { "Master" }, Mode.ModeType.MASTER, true));
            tmp.Add(new ModeSelObj(new List<string> { "Sakura" }, Mode.ModeType.SEGA, false)); //TODO
            tmp.Add(new ModeSelObj(new List<string> { "Shirase" }, Mode.ModeType.SHIRASE, true));
            modes.Add(tmp);
            tmp = new List<ModeSelObj>();//ACE
            //Normal
            //Hi-Speed 1
            //Hi-Speed 2
            //Big
            //UNLOCKED BELOW HERE
            //Another
            //Another 2

            //Match
            //UNLOCKED BELOW HERE
            //Eraser
            //Level Star
            //Target

            tmp.Add(new ModeSelObj(new List<string> { "Roads" }, Mode.ModeType.SEGA, false));
            tmp.Add(new ModeSelObj(new List<string> { "Exam" }, Mode.ModeType.SEGA, false));
            modes.Add(tmp);
            tmp = new List<ModeSelObj>(); //GMX
            tmp.Add(new ModeSelObj(new List<string> { "Dynamo", "Dynamo+", "Dynamo++", "Dynamo+++", "Dynamo*" }, Mode.ModeType.DYNAMO, true));
            tmp.Add(new ModeSelObj(new List<string> { "Endura" }, Mode.ModeType.ENDURA, false));
            modes.Add(tmp);
            tmp = new List<ModeSelObj>(); //bonus
            tmp.Add(new ModeSelObj(new List<string> { "Custom" }, Mode.ModeType.SEGA, false));
            tmp.Add(new ModeSelObj(new List<string> { "Miner", "Zen" }, Mode.ModeType.MINER, true));
            tmp.Add(new ModeSelObj(new List<string> { "Garbage Clear" }, Mode.ModeType.GARBAGE, true));
            tmp.Add(new ModeSelObj(new List<string> { "20G Practice" }, Mode.ModeType.TRAINING, true));
            tmp.Add(new ModeSelObj(new List<string> { "Icy Shirase" }, Mode.ModeType.ROUNDS, true));
            tmp.Add(new ModeSelObj(new List<string> { "Big Bravo Mania" }, Mode.ModeType.KONOHA, true));
            modes.Add(tmp);


        }
        public void logic(Controller pad)
        {
            if (pad.inputV != dInputV)
            {
                selection = (selection - pad.inputV);
                dInputV = pad.inputV;
            }

            if(pad.inputH != dInputH)
            {
                dInputH = pad.inputH;
                if (modes[game][selection].names.Count > 1)
                {
                    variant += pad.inputH;
                    if (variant < 0)
                        variant = modes[game][selection].names.Count - 1;
                    if (variant >= modes[game][selection].names.Count)
                        variant = 0;
                }
            }
            /*if (game == 7 && selection == 1)//zen
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
                case 4:
                case 5:
                case 6:
                    if (selection == 2)
                        selection = 0;
                    if (selection == -1)
                        selection = 1;
                    break;
                case 0:
                case 3:
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
                    if (Math.Abs(selection) == 1)
                        selection = 0;
                    break;
            }*/
            if (selection < 0)
                selection = modes[game].Count - 1;
            if (selection >= modes[game].Count)
                selection = 0;

            if (prevSel != selection)
            {
                Audio.playSound(s_Roll);
                prevSel = selection;
            }
        }
        public void render(Graphics drawBuffer)
        {
            /*if (game == 0)
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
            switch(game)
            {
                case 0://SEGA
                    drawBuffer.DrawString("Bloxeed", SystemFonts.DefaultFont, active, 300, 312);
                    drawBuffer.DrawString("Flash Point", SystemFonts.DefaultFont, locked, 300, 324);
                    break;
                case 3://tap
                    drawBuffer.DrawString("TGM+", SystemFonts.DefaultFont, active, 300, 312);
                    drawBuffer.DrawString("Death", SystemFonts.DefaultFont, active, 300, 324);
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
            }*/
            for(int i = 0; i < modes[game].Count; i++)
            {
                ModeSelObj m = modes[game][i];
                drawBuffer.DrawString(m.names.Count > 1 ? m.names[variant] : m.names[0], SystemFonts.DefaultFont, m.enabled ? active : locked, 300, 300 + 12*i);
            }
            drawBuffer.DrawString("→", SystemFonts.DefaultFont, new SolidBrush(Color.White), 285, 300 + 12 * selection);
        }
    }
}
