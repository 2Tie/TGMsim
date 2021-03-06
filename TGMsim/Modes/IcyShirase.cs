﻿using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace TGMsim.Modes
{
    class IcyShirase : Mode
    {
        public IcyShirase()
        {
            ModeName = "ICY SHIRASE";
            showGrade = false;
            endLevel = 1200;
            sections.Add(100);
            sections.Add(200);
            sections.Add(300);
            sections.Add(400);
            sections.Add(500);
            sections.Add(600);
            sections.Add(700);
            sections.Add(800);
            sections.Add(900);
            sections.Add(1000);
            sections.Add(1100);
            sections.Add(1200);
            g20 = true;
            var gL = new Gimmick();
            gL = new Gimmick();
            gL.type = Gimmick.Type.ICE;
            gL.startLvl = 300;
            gL.endLvl = 400;
            gimList.Add(gL);
            gL = new Gimmick();
            gL.type = Gimmick.Type.ICE;
            gL.startLvl = 500;
            gL.endLvl = 600;
            gimList.Add(gL);
            gL = new Gimmick();
            gL.type = Gimmick.Type.ICE;
            gL.startLvl = 700;
            gL.endLvl = 800;
            gimList.Add(gL);
            gL = new Gimmick();
            gL.type = Gimmick.Type.ICE;
            gL.startLvl = 900;
            gL.endLvl = 1000;
            gimList.Add(gL);
            gL = new Gimmick();
            gL.type = Gimmick.Type.ICE;
            gL.startLvl = 1100;
            gL.endLvl = 1200;
            gimList.Add(gL);
            border = Color.DarkRed;
            showGhost = false;

            delayTable.Add(new List<int> { 12, 12, 12, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6 });
            delayTable.Add(new List<int> { 8, 7, 6, 6, 5, 5, 5, 5, 5, 5, 5, 5, 6 });
            delayTable.Add(new List<int> { 8, 6, 6, 6, 4, 4, 4, 4, 4, 4, 4, 4, 4 });
            delayTable.Add(new List<int> { 18, 18, 17, 15, 13, 12, 12, 12, 12, 12, 10, 8, 15 });
            delayTable.Add(new List<int> { 6, 5, 4, 4, 3, 3, 3, 3, 3, 3, 3, 3, 6 });
            secTet.Add(0);
        }

        public override void onSpawn()
        {
            if (firstPiece)
            {
                firstPiece = false;
            }
            else if (level < endLevel)
            {
                if (level < sections[curSection] - 1)
                    level += 1;
            }
        }

        public override void onClear(int lines, Tetromino tet, long time, bool bravo)
        {
            int oldLvl = level;
            level += lines;

            if (level > endLevel && endLevel != 0)
                level = endLevel;
            //check for tetris
            if (lines == 4)
            {
                tetrises++;
                secTet[curSection]++;
                Audio.playSound(Audio.s_Tetris);
            }

            //section handling
            if (curSection < sections.Count())
                if (level >= sections[curSection])
                {
                    curSection++;
                    secTet.Add(0);
                    //GM FLAGS
                    //MUSIC
                    updateMusic();
                    //DELAYS
                    //BACKGROUND
                }
        }
    }
}
