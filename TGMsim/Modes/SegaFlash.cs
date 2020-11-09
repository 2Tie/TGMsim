using System;
using System.Collections.Generic;
using System.Drawing;

namespace TGMsim.Modes
{
    class SegaFlash : Mode
    {
        public List<int> levelUpTimes = new List<int> { 3480, 2320, 2320, 2320, 2320, 2320, 2320, 2320, 2320, 3480, 3480, 1740, 1740, 1740, 1740, 3480 };
        public int timeCounter = 0;
        public int lineCounter = 0;
        List<int> linePoints = new List<int> { 100, 400, 900, 2000 };
        int bonusP = 1000;
        byte bonusF = 0;

        public SegaFlash()
        {
            ModeName = "FLASH POINT";
            showGrade = false;
            drawSec = false;
            presetBoards = true;
            shadeStack = false;
            outlineStack = false;
            showGhost = false;
            hasDAD = true;
            boardsFile = "flash";
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
            delayTable.Add(new List<int> { 29 });
            delayTable.Add(new List<int> { 29 });
            delayTable.Add(new List<int> { 12 });
            delayTable.Add(new List<int> { 31 });
            delayTable.Add(new List<int> { 39 });
            grades = new List<string> { "" };
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

        public override void onTick(long time, Field.timerType type)
        {
            timeCounter++;
            if (type == Field.timerType.LockDelay)
            {
                ++bonusF;
                if (bonusF % 4 == 0)
                    --bonusP;
            }
            if (bonusP < 0) bonusP = 0;
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
            if (boardGems == 0)
            {
                modeClear = true;
                score += bonusP;
                boardsProgress++;
                if (boardsProgress < 100)
                {
                    continueMode = true;
                    bonusP = ((boardsProgress / 4) + 1) * 1000;
                    if (lineCounter == 0)
                    {
                        lineCounter = 3;
                        level -= (level > 0) ? 1 : 0;
                    }
                    else
                        --lineCounter;
                }
            }
        }

        public override void draw(bool replay)
        {
            Draw.buffer.DrawString(boardGems.ToString(), Draw.f_Maestro, Draw.tb, 20, 300);
            Draw.buffer.DrawString(bonusP.ToString(), Draw.f_Maestro, Draw.tb, 20, 324);
            Draw.buffer.DrawString(levelUpTimes[level > 15 ? 15 : level].ToString(), Draw.f_Maestro, Draw.tb, 20, 312);
        }
    }
}
