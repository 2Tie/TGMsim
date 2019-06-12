using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TGMsim
{
    class M_Plus : Mode
    {
        public M_Plus() : base()
        {
            ModeName = "TGM+";
            border = Color.DarkBlue;
            sections.Add(100);
            sections.Add(200);
            sections.Add(300);
            sections.Add(400);
            sections.Add(500);
            sections.Add(600);
            sections.Add(700);
            sections.Add(800);
            sections.Add(900);
            sections.Add(999);
            delayTable.Add(new List<int> { 25 });//ARE
            delayTable.Add(new List<int> { 25 });//line ARE
            delayTable.Add(new List<int> { 16 });//DAS
            delayTable.Add(new List<int> { 30 });//LOCK
            delayTable.Add(new List<int> { 40 });//LINE CLEAR
            var gL = new Gimmick();
            gL.type = Gimmick.Type.GARBAGE;
            gL.startLvl = 0;
            gL.endLvl = 100;
            gL.parameter = 13;
            gimList.Add(gL);
            gL = new Gimmick();
            gL.type = Gimmick.Type.GARBAGE;
            gL.startLvl = 100;
            gL.endLvl = 200;
            gL.parameter = 12;
            gimList.Add(gL);
            gL = new Gimmick();
            gL.type = Gimmick.Type.GARBAGE;
            gL.startLvl = 200;
            gL.endLvl = 300;
            gL.parameter = 11;
            gimList.Add(gL);
            gL = new Gimmick();
            gL.type = Gimmick.Type.GARBAGE;
            gL.startLvl = 300;
            gL.endLvl = 400;
            gL.parameter = 10;
            gimList.Add(gL);
            gL = new Gimmick();
            gL.type = Gimmick.Type.GARBAGE;
            gL.startLvl = 400;
            gL.endLvl = 500;
            gL.parameter = 9;
            gimList.Add(gL);
            gL = new Gimmick();
            gL.type = Gimmick.Type.GARBAGE;
            gL.startLvl = 500;
            gL.endLvl = 600;
            gL.parameter = 8;
            gimList.Add(gL);
            gL = new Gimmick();
            gL.type = Gimmick.Type.GARBAGE;
            gL.startLvl = 600;
            gL.endLvl = 700;
            gL.parameter = 7;
            gimList.Add(gL);
            gL = new Gimmick();
            gL.type = Gimmick.Type.GARBAGE;
            gL.startLvl = 700;
            gL.endLvl = 800;
            gL.parameter = 6;
            gimList.Add(gL);
            gL = new Gimmick();
            gL.type = Gimmick.Type.GARBAGE;
            gL.startLvl = 800;
            gL.endLvl = 900;
            gL.parameter = 5;
            gimList.Add(gL);
            gL = new Gimmick();
            gL.type = Gimmick.Type.GARBAGE;
            gL.startLvl = 900;
            gL.endLvl = 999;
            gL.parameter = 4;
            gimList.Add(gL);
            garbType = GarbType.FIXED;
            garbTemplate = new List<List<int>>
            {
                new List<int> { 0, 9, 9, 9, 9, 9, 9, 9, 9, 9 },
                new List<int> { 0, 9, 9, 9, 9, 9, 9, 9, 9, 9 },
                new List<int> { 0, 9, 9, 9, 9, 9, 9, 9, 9, 9 },
                new List<int> { 0, 9, 9, 9, 9, 9, 9, 9, 9, 9 },
                new List<int> { 9, 9, 9, 9, 9, 9, 9, 9, 9, 0 },
                new List<int> { 9, 9, 9, 9, 9, 9, 9, 9, 9, 0 },
                new List<int> { 9, 9, 9, 9, 9, 9, 9, 9, 9, 0 },
                new List<int> { 9, 9, 9, 9, 9, 9, 9, 9, 9, 0 },
                new List<int> { 0, 0, 9, 9, 9, 9, 9, 9, 9, 9 },
                new List<int> { 0, 9, 9, 9, 9, 9, 9, 9, 9, 9 },
                new List<int> { 0, 9, 9, 9, 9, 9, 9, 9, 9, 9 },
                new List<int> { 9, 9, 9, 9, 9, 9, 9, 9, 0, 0 },
                new List<int> { 9, 9, 9, 9, 9, 9, 9, 9, 9, 0 },
                new List<int> { 9, 9, 9, 9, 9, 9, 9, 9, 9, 0 },
                new List<int> { 9, 9, 0, 9, 9, 9, 9, 9, 9, 9 },
                new List<int> { 9, 0, 0, 9, 9, 9, 9, 9, 9, 9 },
                new List<int> { 9, 0, 9, 9, 9, 9, 9, 9, 9, 9 },
                new List<int> { 9, 9, 9, 9, 9, 9, 9, 0, 9, 9 },
                new List<int> { 9, 9, 9, 9, 9, 9, 9, 0, 0, 9 },
                new List<int> { 9, 9, 9, 9, 9, 9, 9, 9, 0, 9 },
                new List<int> { 9, 9, 9, 9, 0, 0, 9, 9, 9, 9 },
                new List<int> { 9, 9, 9, 9, 0, 0, 9, 9, 9, 9 },
                new List<int> { 9, 9, 9, 9, 0, 9, 9, 9, 9, 9 },
                new List<int> { 9, 9, 9, 0, 0, 0, 9, 9, 9, 9 }
            };
        }

        public override void onSpawn()
        {
            if(firstPiece)
            {
                firstPiece = false;
            }
            else if (level < endLevel)
            {
                if (level < sections[curSection] - 1)
                    level += 1;
            }
        }

        public override void onPut(Tetromino tet, bool clear)
        {
            garbTimer++;
            if (clear == false)
            {
                combo = 1;
            }
        }

        public override void onClear(int lines, Tetromino tet, long time, bool bravo)
        {
            garbTimer--;

            int oldLvl = level;
            level += lines;

            if (level >= endLevel && endLevel != 0)
            {
                level = endLevel;
                inCredits = true;
            }

            //bravo handling
            int b = 1;
            if (bravo)
                b = 4;

            //score handling

            combo = combo + (2 * lines) - 2;

            int sped = baseLock - tet.life;
            if (sped < 0) sped = 0;
            score += ((int)Math.Ceiling((double)(oldLvl + lines) / 4) + tet.soft + (2 * tet.sonic)) * lines * combo * b + (int)(Math.Ceiling((double)level / 2)) + (sped * 7);

            if (curSection < sections.Count())
                if (level >= sections[curSection])
                {
                    curSection++;
                    showGhost = false;
                    //MUSIC
                    updateMusic();
                }
        }

        public override void onGameOver()
        {
            if (creditsClear)
                orangeLine = true;
        }
    }
}
