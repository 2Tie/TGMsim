using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TGMsim
{
    class M_SegaFlash : Mode
    {
        public List<int> levelUpTimes = new List<int> { 3480, 2320, 2320, 2320, 2320, 2320, 2320, 2320, 2320, 3480, 3480, 1740, 1740, 1740, 1740, 3480 };
        public int timeCounter = 0;
        public int lineCounter = 0;
        List<int> linePoints = new List<int> { 100, 400, 900, 2000 };

        public M_SegaFlash()
        {
            ModeName = "FLASH POINT";
            showGrade = false;
            drawSec = false;
            presetBoards = true;
            shadeStack = false;
            outlineStack = false;
            showGhost = false;
            boardsFile = "test1";
            boardsProgress = 0;
            sections.Add(2);
            sections.Add(4);
            sections.Add(6);
            sections.Add(8);
            sections.Add(9);
            sections.Add(10);
            sections.Add(11);
            sections.Add(13);
            sections.Add(15);
            endLevel = 0;
            delayTable.Add(new List<int> { 30 });
            delayTable.Add(new List<int> { 30 });
            delayTable.Add(new List<int> { 20 });
            delayTable.Add(new List<int> { 30 });
            delayTable.Add(new List<int> { 42 });
        }

        public override void onPut(Tetromino tet, bool clear)
        {
            if (timeCounter > levelUpTimes[level > 15 ? 15 : level])
            {
                level++;
                timeCounter = 0;
                lineCounter = 0;
            }
        }

        public override void onSoft()
        {
            //add softdrop points
            if (level < 15) //softdrop at 1G doesn't award points
                score += Math.Min(4, level / 2) + 1;
        }

        public override void onTick(long time)
        {
            timeCounter++;
        }

        public override void onClear(int lines, Tetromino tet, long time, bool bravo)
        {
            //add points
            int scorelevel = Math.Min(4, level / 2) + 1;
            score += scorelevel * linePoints[lines - 1] * (bravo ? 10 : 1);

            timeCounter = 0;
            lineCounter += lines;
            if (lineCounter >= 4)
            {
                level++;
                lineCounter = 0;
            }
        }

        public override void draw(Graphics drawBuffer, Font f, bool replay)
        {
            Brush tb = new SolidBrush(Color.White);
            drawBuffer.DrawString(timeCounter.ToString(), f, tb, 20, 300);
            drawBuffer.DrawString(levelUpTimes[level > 15 ? 15 : level].ToString(), f, tb, 20, 312);
        }
    }
}
