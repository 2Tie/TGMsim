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

        public List<int> variant = new List<int>();

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
            tmp.Add(new ModeSelObj(new List<string> { "Flash Point" }, Mode.ModeType.FLASH, true));
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
            tmp.Add(new ModeSelObj(new List<string> { "Hell March" }, Mode.ModeType.MARCH, true));
            modes.Add(tmp);
            tmp = new List<ModeSelObj>(); //bonus
            tmp.Add(new ModeSelObj(new List<string> { "Custom" }, Mode.ModeType.SEGA, false));
            tmp.Add(new ModeSelObj(new List<string> { "Miner", "Zen" }, Mode.ModeType.MINER, true));
            tmp.Add(new ModeSelObj(new List<string> { "Garbage Clear" }, Mode.ModeType.GARBAGE, true));
            tmp.Add(new ModeSelObj(new List<string> { "20G Practice" }, Mode.ModeType.TRAINING, true));
            tmp.Add(new ModeSelObj(new List<string> { "Icy Shirase" }, Mode.ModeType.ROUNDS, true));
            tmp.Add(new ModeSelObj(new List<string> { "Big Bravo Mania" }, Mode.ModeType.KONOHA, true));
            modes.Add(tmp);

            for (int i = 0; i < modes[g].Count; i++)
                variant.Add(0);
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
                    variant[selection] += pad.inputH;
                    if (variant[selection] < 0)
                        variant[selection] = modes[game][selection].names.Count - 1;
                    if (variant[selection] >= modes[game][selection].names.Count)
                        variant[selection] = 0;
                }
            }
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
            for(int i = 0; i < modes[game].Count; i++)
            {
                ModeSelObj m = modes[game][i];
                drawBuffer.DrawString(m.names.Count > 1 ? m.names[variant[i]] : m.names[0], SystemFonts.DefaultFont, m.enabled ? active : locked, 300, 300 + 12*i);
            }
            drawBuffer.DrawString("→", SystemFonts.DefaultFont, new SolidBrush(Color.White), 285, 300 + 12 * selection);
        }
    }
}
