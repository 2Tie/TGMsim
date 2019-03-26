using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TGMsim
{
    class M_Training : Mode
    {
        public M_Training()
        {
            ModeName = "20G TRAINING";
            border = Color.DarkGreen;
            endLevel = 200;
            g20 = true;
            gradedBy = 4;
            showGrade = false;
            showGhost = false;
            sections.Add(200);
            delayTable.Add(new List<int> { 27 });
            delayTable.Add(new List<int> { 27 });
            delayTable.Add(new List<int> { 14 });
            delayTable.Add(new List<int> { 30 });
            delayTable.Add(new List<int> { 40 });
        }

        public override void onSpawn()
        {
            if (firstPiece)
            {
                firstPiece = false;
            }
            else if (level < endLevel)
                level += 1;
        }

        public override void onClear(int lines, Tetromino tet, long time, bool bravo)
        {
            level += lines;
            if (level > endLevel && endLevel != 0)
                level = endLevel;
        }
    }
}
