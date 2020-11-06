using System;
using System.Collections.Generic;
using System.Drawing;

namespace TGMsim.Modes
{
    class BigBravoMania : Mode
    {
        int bravos = 0;
        public BigBravoMania()
        {
            ModeName = "BIG BRAVO MANIA";
            showGrade = false;
            endLevel = 1200;
            limitType = 3;
            limit = 180000;//three minutes
            bigmode = true;
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
            border = Color.PaleVioletRed;
            delayTable.Add(new List<int> { 26 });
            delayTable.Add(new List<int> { 26 });
            delayTable.Add(new List<int> { 14, 8, 8, 8, 8, 6, 6, 6, 6 }); //das, dunno
            delayTable.Add(new List<int> { 30, 30, 30, 30, 30, 17, 17, 15, 15 }); //lock, dunno
            delayTable.Add(new List<int> { 21 });
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
            if (level < 1000)
            {
                limit += 1000 * (int)(Math.Floor((double)lines / 3));
            }
            if (bravo)
            {
                bravos++;
                if (level < 1000)
                {
                    if (lines == 1)
                        limit += 1000 * 5;
                    if (lines == 2)
                        limit += 1000 * 8;
                    if (lines == 3)
                        limit += 1000 * 10;
                    if (lines == 4)
                        limit += 1000 * 15;
                }
                else
                {
                    if (lines == 2)
                        limit += 1000 * 1;
                    if (lines == 3)
                        limit += 1000 * 2;
                    if (lines == 4)
                        limit += 1000 * 3;
                }
            }
            //time is added before level is increased
            level += lines*2;
            if (lines == 4)
                level += 4;

            if (level >= sections[curSection])
            {
                curSection++;
                secTet.Add(0);
                //GM FLAGS
                //MUSIC
                updateMusic();
                //DELAYS
                //BACKGROUND
                if (level > endLevel && endLevel != 0)
                    level = endLevel;
                if (curSection == 2)
                    g20 = true;
            }
            if(bravos == 11)
            {
                //disable generator duplicate prevention?
                generatorFlag = true;
            }
            if(bravos == 38) //13*3 - 1
            {
                inCredits = true;
            }
        }

        public override void draw(bool replay)
        {
            //if (replay)
            {
                Draw.buffer.DrawString("BRAVOS: " + bravos, Draw.f_Maestro, Draw.tb, 480, 340);
            }
        }
    }
}
