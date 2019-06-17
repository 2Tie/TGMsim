using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TGMsim.Modes
{
    class Miner : Mode
    {
        public Miner(int v)
        {
            ModeName = "MINER";
            border = Color.DarkGreen;
            limitType = 4;
            limit = 1;
            sections.Add(100);
            sections.Add(200);
            sections.Add(300);
            sections.Add(400);
            endLevel = 500;
            autoGarbage = true;
            creditsSong = "crdtcas";
            delayTable.Add(new List<int> { 30 });
            delayTable.Add(new List<int> { 30 });
            delayTable.Add(new List<int> { 16 });
            delayTable.Add(new List<int> { 30 });
            delayTable.Add(new List<int> { 41 });

            if(v == 1)
            {
                ModeName = "ZEN";
                endLevel = -1;
                startEnd = true;
                keepFieldSafe = true;
            }
        }

        public override void onClear(int lines, Tetromino tet, long time, bool bravo)
        {
            level += lines;
            if (level >= endLevel && endLevel != 0)
            {
                level = endLevel;
                inCredits = true;
            }
            if (curSection < sections.Count)
                if (level >= sections[curSection])
                {
                    curSection++;
                }
        }

        public override void updateMusic()
        {
            if (Audio.song != "Casual 1")
                Audio.playMusic("Casual 1");
        }
    }
}
