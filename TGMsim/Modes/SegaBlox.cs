using System.Collections.Generic;

using System.Drawing;

namespace TGMsim.Modes
{
    class SegaBlox : SegaTet
    {
        G_SEGA itemgen;
        int itemtimer = 0;
        int[] stackheighttimes = new int[4] { 1200, 900, 600, 300 };
        Tetromino.ItemType[,] itemtable = new Tetromino.ItemType[64, 4] {
        { Tetromino.ItemType.delfour, Tetromino.ItemType.delfour, Tetromino.ItemType.delfour, Tetromino.ItemType.delfour},
        { Tetromino.ItemType.delfour, Tetromino.ItemType.delfour, Tetromino.ItemType.delfour, Tetromino.ItemType.delfour},
        { Tetromino.ItemType.delfour, Tetromino.ItemType.delfour, Tetromino.ItemType.delfour, Tetromino.ItemType.delfour},
        { Tetromino.ItemType.delfour, Tetromino.ItemType.delfour, Tetromino.ItemType.delfour, Tetromino.ItemType.delfour},
        { Tetromino.ItemType.delfour, Tetromino.ItemType.delfour, Tetromino.ItemType.delfour, Tetromino.ItemType.delfour},
        { Tetromino.ItemType.delfour, Tetromino.ItemType.delfour, Tetromino.ItemType.delfour, Tetromino.ItemType.delfour},
        { Tetromino.ItemType.delfour, Tetromino.ItemType.delfour, Tetromino.ItemType.delfour, Tetromino.ItemType.delfour},
        { Tetromino.ItemType.delfour, Tetromino.ItemType.delfour, Tetromino.ItemType.delfour, Tetromino.ItemType.delfour},
        { Tetromino.ItemType.delfour, Tetromino.ItemType.delfour, Tetromino.ItemType.delfour, Tetromino.ItemType.delfour},
        { Tetromino.ItemType.delfour, Tetromino.ItemType.delfour, Tetromino.ItemType.delfour, Tetromino.ItemType.delfour},
        { Tetromino.ItemType.delfour, Tetromino.ItemType.delfour, Tetromino.ItemType.delfour, Tetromino.ItemType.delfour},
        { Tetromino.ItemType.delfour, Tetromino.ItemType.delfour, Tetromino.ItemType.delfour, Tetromino.ItemType.delfour},
        { Tetromino.ItemType.delfour, Tetromino.ItemType.delfour, Tetromino.ItemType.delfour, Tetromino.ItemType.delfour},
        { Tetromino.ItemType.delfour, Tetromino.ItemType.delfour, Tetromino.ItemType.delfour, Tetromino.ItemType.delfour},
        { Tetromino.ItemType.delfour, Tetromino.ItemType.delfour, Tetromino.ItemType.delfour, Tetromino.ItemType.delfour},
        { Tetromino.ItemType.delfour, Tetromino.ItemType.delfour, Tetromino.ItemType.delfour, Tetromino.ItemType.delfour},
        { Tetromino.ItemType.delfour, Tetromino.ItemType.delfour, Tetromino.ItemType.delfour, Tetromino.ItemType.delfour}, //0x10
        { Tetromino.ItemType.delfour, Tetromino.ItemType.delfour, Tetromino.ItemType.bird, Tetromino.ItemType.delfour},
        { Tetromino.ItemType.delfour, Tetromino.ItemType.bird, Tetromino.ItemType.bird, Tetromino.ItemType.delfour},
        { Tetromino.ItemType.delfour, Tetromino.ItemType.bird, Tetromino.ItemType.bird, Tetromino.ItemType.delfour},
        { Tetromino.ItemType.delfour, Tetromino.ItemType.bird, Tetromino.ItemType.bird, Tetromino.ItemType.delfour},
        { Tetromino.ItemType.delfour, Tetromino.ItemType.bird, Tetromino.ItemType.bird, Tetromino.ItemType.delfour},
        { Tetromino.ItemType.delfour, Tetromino.ItemType.bird, Tetromino.ItemType.bird, Tetromino.ItemType.delfour},
        { Tetromino.ItemType.delfour, Tetromino.ItemType.bird, Tetromino.ItemType.bird, Tetromino.ItemType.delfour},
        { Tetromino.ItemType.delfour, Tetromino.ItemType.bird, Tetromino.ItemType.bird, Tetromino.ItemType.bird},
        { Tetromino.ItemType.bird, Tetromino.ItemType.bird, Tetromino.ItemType.bird, Tetromino.ItemType.bird},
        { Tetromino.ItemType.bird, Tetromino.ItemType.bird, Tetromino.ItemType.shot, Tetromino.ItemType.bird},
        { Tetromino.ItemType.bird, Tetromino.ItemType.bird, Tetromino.ItemType.shot, Tetromino.ItemType.bird},
        { Tetromino.ItemType.bird, Tetromino.ItemType.bird, Tetromino.ItemType.shot, Tetromino.ItemType.bird},
        { Tetromino.ItemType.bird, Tetromino.ItemType.bird, Tetromino.ItemType.shot, Tetromino.ItemType.bird},
        { Tetromino.ItemType.bird, Tetromino.ItemType.bird, Tetromino.ItemType.shot, Tetromino.ItemType.bird},
        { Tetromino.ItemType.bird, Tetromino.ItemType.bird, Tetromino.ItemType.shot, Tetromino.ItemType.shot},
        { Tetromino.ItemType.bird, Tetromino.ItemType.bird, Tetromino.ItemType.shot, Tetromino.ItemType.shot},//0x20
        { Tetromino.ItemType.bird, Tetromino.ItemType.shot, Tetromino.ItemType.shot, Tetromino.ItemType.shot},
        { Tetromino.ItemType.bird, Tetromino.ItemType.shot, Tetromino.ItemType.shot, Tetromino.ItemType.shot},
        { Tetromino.ItemType.bird, Tetromino.ItemType.shot, Tetromino.ItemType.shot, Tetromino.ItemType.shot},
        { Tetromino.ItemType.shot, Tetromino.ItemType.shot, Tetromino.ItemType.weight16, Tetromino.ItemType.shot},
        { Tetromino.ItemType.shot, Tetromino.ItemType.shot, Tetromino.ItemType.weight16, Tetromino.ItemType.shot},
        { Tetromino.ItemType.shot, Tetromino.ItemType.shot, Tetromino.ItemType.weight16, Tetromino.ItemType.shot},
        { Tetromino.ItemType.shot, Tetromino.ItemType.shot, Tetromino.ItemType.weight16, Tetromino.ItemType.shot},
        { Tetromino.ItemType.shot, Tetromino.ItemType.shot, Tetromino.ItemType.weight16, Tetromino.ItemType.weight16},
        { Tetromino.ItemType.shot, Tetromino.ItemType.shot, Tetromino.ItemType.weight16, Tetromino.ItemType.weight16},
        { Tetromino.ItemType.shot, Tetromino.ItemType.shot, Tetromino.ItemType.weight16, Tetromino.ItemType.weight16},
        { Tetromino.ItemType.shot, Tetromino.ItemType.shot, Tetromino.ItemType.weight16, Tetromino.ItemType.weight16},
        { Tetromino.ItemType.shot, Tetromino.ItemType.shot, Tetromino.ItemType.weight16, Tetromino.ItemType.weight16},
        { Tetromino.ItemType.shot, Tetromino.ItemType.shot, Tetromino.ItemType.weight16, Tetromino.ItemType.weight16},
        { Tetromino.ItemType.weight16, Tetromino.ItemType.shot, Tetromino.ItemType.weight16, Tetromino.ItemType.weight16},
        { Tetromino.ItemType.weight16, Tetromino.ItemType.shot, Tetromino.ItemType.weight16, Tetromino.ItemType.weight16},
        { Tetromino.ItemType.weight16, Tetromino.ItemType.weight16, Tetromino.ItemType.weight16, Tetromino.ItemType.weight16},//0x30
        { Tetromino.ItemType.weight16, Tetromino.ItemType.weight16, Tetromino.ItemType.weight16, Tetromino.ItemType.bomb},
        { Tetromino.ItemType.weight16, Tetromino.ItemType.weight16, Tetromino.ItemType.weight16, Tetromino.ItemType.bomb},
        { Tetromino.ItemType.weight16, Tetromino.ItemType.weight16, Tetromino.ItemType.weight16, Tetromino.ItemType.bomb},
        { Tetromino.ItemType.weight16, Tetromino.ItemType.weight16, Tetromino.ItemType.weight16, Tetromino.ItemType.bomb},
        { Tetromino.ItemType.weight16, Tetromino.ItemType.weight16, Tetromino.ItemType.weight16, Tetromino.ItemType.bomb},
        { Tetromino.ItemType.bomb, Tetromino.ItemType.weight16, Tetromino.ItemType.bomb, Tetromino.ItemType.bomb},
        { Tetromino.ItemType.bomb, Tetromino.ItemType.weight16, Tetromino.ItemType.bomb, Tetromino.ItemType.bomb},
        { Tetromino.ItemType.bomb, Tetromino.ItemType.bomb, Tetromino.ItemType.bomb, Tetromino.ItemType.bomb},
        { Tetromino.ItemType.bomb, Tetromino.ItemType.bomb, Tetromino.ItemType.bomb, Tetromino.ItemType.bomb},
        { Tetromino.ItemType.bomb, Tetromino.ItemType.bomb, Tetromino.ItemType.bomb, Tetromino.ItemType.bomb},
        { Tetromino.ItemType.bomb, Tetromino.ItemType.bomb, Tetromino.ItemType.bomb, Tetromino.ItemType.bomb},
        { Tetromino.ItemType.bomb, Tetromino.ItemType.bomb, Tetromino.ItemType.bomb, Tetromino.ItemType.bomb},
        { Tetromino.ItemType.bomb, Tetromino.ItemType.bomb, Tetromino.ItemType.bomb, Tetromino.ItemType.bomb},
        { Tetromino.ItemType.bomb, Tetromino.ItemType.bomb, Tetromino.ItemType.bomb, Tetromino.ItemType.bomb},
        { Tetromino.ItemType.bomb, Tetromino.ItemType.bomb, Tetromino.ItemType.bomb, Tetromino.ItemType.bomb},
        };

        public SegaBlox() : base()
        {
            var gL = new Gimmick();
            gL.type = Gimmick.Type.GARBAGE;
            gL.startLvl = 0;
            gL.endLvl = 10;
            gL.parameter = 99*6;
            gimList.Add(gL);
            gL = new Gimmick();
            gL.type = Gimmick.Type.GARBAGE;
            gL.startLvl = 10;
            gL.endLvl = 13;
            gL.parameter = 79 * 6;
            gimList.Add(gL);
            gL = new Gimmick();
            gL.type = Gimmick.Type.GARBAGE;
            gL.startLvl = 13;
            gL.endLvl = 15;
            gL.parameter = 69 * 6;
            gimList.Add(gL);
            gL = new Gimmick();
            gL.type = Gimmick.Type.GARBAGE;
            gL.startLvl = 15;
            gL.endLvl = 17;
            gL.parameter = 59 * 6;
            gimList.Add(gL);
            gL = new Gimmick();
            gL.type = Gimmick.Type.GARBAGE;
            gL.startLvl = 17;
            gL.endLvl = 19;
            gL.parameter = 49 * 6;
            gimList.Add(gL);
            gL = new Gimmick();
            gL.type = Gimmick.Type.GARBAGE;
            gL.startLvl = 19;
            gL.endLvl = 31;
            gL.parameter = 39 * 6;
            gimList.Add(gL);
            gL = new Gimmick();
            gL.type = Gimmick.Type.GARBAGE;
            gL.startLvl = 31;
            gL.endLvl = 40;
            gL.parameter = 29 * 6;
            gimList.Add(gL);
            gL = new Gimmick();
            gL.type = Gimmick.Type.GARBAGE;
            gL.startLvl = 40;
            gL.endLvl = 100;
            gL.parameter = 19 * 6;
            gimList.Add(gL);
            garbType = GarbType.FIXED;
            raiseGarbOnClear = false;
            garbSafeLine = 16;
            garbMeter = true;
            garbDelay = 10;
            garbTemplate = new List<List<int>>();
            garbTemplate.Add(new List<int> { 1, 1, 1, 1, 1, 1, 1, 1, 1, 0 });
            garbTemplate.Add(new List<int> { 1, 1, 1, 1, 1, 1, 1, 1, 1, 0 });
            garbTemplate.Add(new List<int> { 1, 1, 1, 1, 1, 1, 1, 1, 1, 0 });
            garbTemplate.Add(new List<int> { 1, 1, 1, 1, 1, 1, 1, 1, 1, 0 });
            garbTemplate.Add(new List<int> { 0, 0, 4, 4, 4, 4, 4, 4, 4, 4 });
            garbTemplate.Add(new List<int> { 0, 4, 4, 4, 4, 4, 4, 4, 4, 4 });
            garbTemplate.Add(new List<int> { 0, 4, 4, 4, 4, 4, 4, 4, 4, 4 });
            garbTemplate.Add(new List<int> { 0, 4, 4, 4, 4, 4, 4, 4, 4, 4 });
            garbTemplate.Add(new List<int> { 5, 5, 5, 5, 5, 5, 5, 5, 0, 0 });
            garbTemplate.Add(new List<int> { 5, 5, 5, 5, 5, 5, 5, 5, 5, 0 });
            garbTemplate.Add(new List<int> { 5, 5, 5, 5, 5, 5, 5, 5, 5, 0 });
            garbTemplate.Add(new List<int> { 5, 5, 5, 5, 5, 5, 5, 5, 5, 0 });
            garbTemplate.Add(new List<int> { 0, 0, 2, 2, 2, 2, 2, 2, 2, 2 });
            garbTemplate.Add(new List<int> { 0, 2, 2, 2, 2, 2, 2, 2, 2, 2 });
            garbTemplate.Add(new List<int> { 2, 2, 0, 0, 2, 2, 2, 2, 2, 2 });
            garbTemplate.Add(new List<int> { 2, 2, 0, 2, 2, 2, 2, 2, 2, 2 });
            garbTemplate.Add(new List<int> { 3, 3, 3, 3, 3, 3, 3, 3, 0, 0 });
            garbTemplate.Add(new List<int> { 3, 3, 3, 3, 3, 3, 3, 3, 3, 0 });
            garbTemplate.Add(new List<int> { 3, 3, 3, 3, 3, 3, 0, 0, 3, 3 });
            garbTemplate.Add(new List<int> { 3, 3, 3, 3, 3, 3, 3, 0, 3, 3 });
            garbTemplate.Add(new List<int> { 0, 1, 1, 1, 1, 1, 1, 1, 1, 1 });
            garbTemplate.Add(new List<int> { 0, 1, 1, 1, 1, 1, 1, 1, 1, 1 });
            garbTemplate.Add(new List<int> { 0, 1, 1, 1, 1, 1, 1, 1, 1, 1 });
            garbTemplate.Add(new List<int> { 0, 1, 1, 1, 1, 1, 1, 1, 1, 1 });
            garbTemplate.Add(new List<int> { 6, 6, 6, 6, 0, 0, 6, 6, 6, 6 });
            garbTemplate.Add(new List<int> { 6, 6, 6, 6, 0, 0, 6, 6, 6, 6 });
            garbTemplate.Add(new List<int> { 6, 6, 6, 6, 0, 0, 6, 6, 6, 6 });
            garbTemplate.Add(new List<int> { 6, 6, 6, 6, 0, 0, 6, 6, 6, 6 });
            garbTemplate.Add(new List<int> { 7, 7, 7, 7, 0, 0, 0, 7, 7, 7 });
            garbTemplate.Add(new List<int> { 7, 7, 7, 7, 7, 0, 7, 7, 7, 7 });
            garbTemplate.Add(new List<int> { 7, 0, 0, 0, 7, 7, 7, 7, 7, 7 });
            garbTemplate.Add(new List<int> { 7, 7, 0, 7, 7, 7, 7, 7, 7, 7 });
            delayTable[2][0] = 12;
            delayTable[4][0] = 39;
            ModeName = "BLOXEED";
            showGhost = false;

            //the item randomizer
            itemgen = new G_SEGA(0x2A6D365B); //POP seed for now while it's tested
            itemgen.read();
            itemgen.read();

        }

        public override void onTick(long time)
        {
            timeCounter++;
            garbTimer++;
            itemtimer++;
        }

        public override void onPieceGen(Tetromino tet, List<List<int>> gameField)
        {
            int table = itemgen.read();
            int stackheight;
            for (stackheight = gameField[0].Count - 1; stackheight > -1; stackheight--)
            {
                if (gameField[2][stackheight] != 0)
                    break;
                if (gameField[3][stackheight] != 0)
                    break;
                if (gameField[4][stackheight] != 0)
                    break;
                if (gameField[5][stackheight] != 0)
                    break;
                if (gameField[6][stackheight] != 0)
                    break;
                if (gameField[7][stackheight] != 0)
                    break;
            }
            int heightchunk = 0;
            if (stackheight > 8) heightchunk++;
            if (stackheight > 12) heightchunk++;
            if (stackheight > 16) heightchunk++;
            //get stack height, if timer above threshold then make the tet an item block
            if (itemtimer > stackheighttimes[heightchunk])
            {
                tet.item = itemtable[table, heightchunk];
                itemtimer = 0;
            }
        }

        public override void draw(bool replay)
        {
            if (replay)
            {
                Draw.buffer.DrawString(timeCounter.ToString(), Draw.f_Maestro, Draw.tb, 20, 300);
                Draw.buffer.DrawString(levelUpTimes[level > 15 ? 15 : level].ToString(), Draw.f_Maestro, Draw.tb, 20, 312);
                Draw.buffer.DrawString(garbTimer.ToString(), Draw.f_Maestro, Draw.tb, 20, 324);
            }

            Draw.buffer.DrawString(itemtimer.ToString(), Draw.f_Maestro, Draw.tb, 20, 336);
        }
    }
}
