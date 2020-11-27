using System.Collections.Generic;
using System.Drawing;

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
            public GameRules.Games game;
            public List<bool> enabled;

            public ModeSelObj(List<string> n, Mode.ModeType t, GameRules.Games g, List<bool> e)
            {
                names = n;
                id = t;
                game = g;
                enabled = e;
            }
        }

        public List<List<ModeSelObj>> modes = new List<List<ModeSelObj>>();


        SolidBrush active = new SolidBrush(Color.White);
        SolidBrush locked = new SolidBrush(Color.Gray);

        System.Windows.Media.MediaPlayer s_Roll = new System.Windows.Media.MediaPlayer();

        public ModeSelect(int g, Profile p)
        {
            game = g;
            Audio.addSound(s_Roll, "/Res/Audio/SE/SEI_name_select.wav");

            List<ModeSelObj> tmp = new List<ModeSelObj>();//sega
            tmp.Add(new ModeSelObj(new List<string> { "Tetris" }, Mode.ModeType.SEGA, GameRules.Games.SEGA, new List<bool> { true }));
            tmp.Add(new ModeSelObj(new List<string> { "Bloxeed" }, Mode.ModeType.BLOX, GameRules.Games.SEGA, new List<bool> { true }));
            tmp.Add(new ModeSelObj(new List<string> { "Flash Point" }, Mode.ModeType.FLASH, GameRules.Games.SEGA, new List<bool> { true }));
            tmp.Add(new ModeSelObj(new List<string> { "SEMIPRO 20G" }, Mode.ModeType.SHIMIZU, GameRules.Games.SEGA, new List<bool> { true }));
            modes.Add(tmp);
            tmp = new List<ModeSelObj>();//tgm1
            tmp.Add(new ModeSelObj(new List<string> { "Master" }, Mode.ModeType.MASTER, GameRules.Games.TGM1, new List<bool> { true }));
            modes.Add(tmp);
            tmp = new List<ModeSelObj>();//tgm2
            tmp.Add(new ModeSelObj(new List<string> { "Master" }, Mode.ModeType.MASTER, GameRules.Games.TGM2, new List<bool> { true }));
            tmp.Add(new ModeSelObj(new List<string> { "Cardcaptor Sakura Easy", "Cardcaptor Sakura Normal" }, Mode.ModeType.CCS, GameRules.Games.CCS, new List<bool> { true, true }));
            modes.Add(tmp);
            tmp = new List<ModeSelObj>();//tap
            tmp.Add(new ModeSelObj(new List<string> { "Normal" }, Mode.ModeType.SEGA, GameRules.Games.TAP, new List<bool> { false })); //TODO
            tmp.Add(new ModeSelObj(new List<string> { "Master" }, Mode.ModeType.MASTER, GameRules.Games.TAP, new List<bool> { true }));
            tmp.Add(new ModeSelObj(new List<string> { "TGM+" }, Mode.ModeType.PLUS, GameRules.Games.TAP, new List<bool> { true }));
            tmp.Add(new ModeSelObj(new List<string> { "Death" }, Mode.ModeType.DEATH, GameRules.Games.TAP, new List<bool> { true }));
            modes.Add(tmp);
            tmp = new List<ModeSelObj>();//tgm3
            tmp.Add(new ModeSelObj(new List<string> { "Easy" }, Mode.ModeType.EASY, GameRules.Games.TGM3, new List<bool> { true }));
            tmp.Add(new ModeSelObj(new List<string> { "Master" }, Mode.ModeType.MASTER, GameRules.Games.TGM3, new List<bool> { true }));
            tmp.Add(new ModeSelObj(new List<string> { "Sakura" }, Mode.ModeType.SEGA, GameRules.Games.TGM3, new List<bool> { false })); //TODO
            tmp.Add(new ModeSelObj(new List<string> { "Shirase" }, Mode.ModeType.SHIRASE, GameRules.Games.TGM3, new List<bool> { true }));
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

            tmp.Add(new ModeSelObj(new List<string> { "Roads" }, Mode.ModeType.SEGA, GameRules.Games.ACE, new List<bool> { false }));
            tmp.Add(new ModeSelObj(new List<string> { "Exam" }, Mode.ModeType.SEGA, GameRules.Games.ACE, new List<bool> { false }));
            modes.Add(tmp);
            tmp = new List<ModeSelObj>(); //GMX
            tmp.Add(new ModeSelObj(new List<string> { "Dynamo", "Dynamo+", "Dynamo++", "Dynamo+++", "Dynamo*" }, Mode.ModeType.DYNAMO, GameRules.Games.GMX, new List<bool> { true, p.dynamoProgress > 0, p.dynamoProgress > 1, p.dynamoProgress > 2, p.dynamoProgress > 3 }));
            tmp.Add(new ModeSelObj(new List<string> { "Endura" }, Mode.ModeType.ENDURA, GameRules.Games.GMX, new List<bool> { false }));
            tmp.Add(new ModeSelObj(new List<string> { "Hell March" }, Mode.ModeType.MARCH, GameRules.Games.GMX, new List<bool> { true }));
            modes.Add(tmp);
            tmp = new List<ModeSelObj>(); //bonus
            tmp.Add(new ModeSelObj(new List<string> { "Custom" }, Mode.ModeType.CUSTOM, GameRules.Games.EXTRA, new List<bool> { true }));
            tmp.Add(new ModeSelObj(new List<string> { "Miner", "Zen" }, Mode.ModeType.MINER, GameRules.Games.EXTRA, new List<bool> { true, true }));
            tmp.Add(new ModeSelObj(new List<string> { "Garbage Clear" }, Mode.ModeType.GARBAGE, GameRules.Games.EXTRA, new List<bool> { true }));
            tmp.Add(new ModeSelObj(new List<string> { "20G Practice" }, Mode.ModeType.TRAINING, GameRules.Games.EXTRA, new List<bool> { true }));
            tmp.Add(new ModeSelObj(new List<string> { "Icy Shirase" }, Mode.ModeType.ROUNDS, GameRules.Games.M0R, new List<bool> { true }));
            tmp.Add(new ModeSelObj(new List<string> { "Big Bravo Mania" }, Mode.ModeType.KONOHA, GameRules.Games.M0R, new List<bool> { true }));
            tmp.Add(new ModeSelObj(new List<string> { "TGM Practice 0", "TGM Practice 100", "TGM Practice 200", "TGM Practice 1G", "TGM Practice 2G", "TGM Practice 5G", "TGM 20G" }, Mode.ModeType.PRACTICE, GameRules.Games.TGM1, new List<bool> { true, true, true, true, true, true, true, }));
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
        public void render()
        {
            for(int i = 0; i < modes[game].Count; i++)
            {
                ModeSelObj m = modes[game][i];
                Draw.buffer.DrawString(m.names[variant[i]], SystemFonts.DefaultFont, m.enabled[variant[i]] ? active : locked, 150, 250 + 12*i);
            }
            Draw.buffer.DrawString("→", SystemFonts.DefaultFont, Draw.wb, 135, 250 + 12 * selection);
        }
    }
}
