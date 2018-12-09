﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TGMsim
{
    class M_SegaTet : Mode
    {
        public List<int> levelUpTimes = new List<int> { 3480, 2320, 2320, 2320, 2320, 2320, 2320, 2320, 2320, 3480, 3480, 1740, 1740, 1740, 1740, 3480 };
        public int timeCounter = 0;
        public int lineCounter = 0;

        public M_SegaTet()
        {
            ModeName = "TETRIS";
            showGrade = false;
            drawSec = false;
            sections.Add(1);
            sections.Add(2);
            sections.Add(3);
            sections.Add(4);
            sections.Add(5);
            sections.Add(6);
            sections.Add(7);
            sections.Add(8);
            sections.Add(9);
            sections.Add(10);
            sections.Add(11);
            sections.Add(12);
            sections.Add(13);
            sections.Add(14);
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
            int l = level > 15 ? 15 : level;
            if (timeCounter > levelUpTimes[l])
            {
                level++;
                //curSection++;
                timeCounter = 0;
            }
        }

        public override void onTick(long time)
        {
            timeCounter++;
        }

        public override void onClear(int lines, Tetromino tet, long time, bool bravo)
        {
            timeCounter = 0;
            lineCounter += lines;
            if(lineCounter >= 4)
            {
                level++;
                //curSection++;
                lineCounter = 0;
            }
        }

        public override void draw(Graphics drawBuffer, Font f, bool replay)
        {
            Brush tb = new SolidBrush(Color.White);
            drawBuffer.DrawString(timeCounter.ToString(), f, tb, 20, 300);
            drawBuffer.DrawString(levelUpTimes[level].ToString(), f, tb, 20, 312);
        }
    }
}