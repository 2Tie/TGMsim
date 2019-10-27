using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TGMsim.Modes
{
    class Master1Practice : Mode
    {
        int clearedLines;

        public Master1Practice(int var)
        {
            ModeName = "MASTER 1 PRACTICE";
            switch(var)
            {
                case 0:
                    level = 0;
                    break;
                case 1:
                    level = 100;
                    break;
                case 2:
                    level = 170;
                    break;
                case 3:
                    level = 251;
                    break;
                case 4:
                    level = 300;
                    break;
                case 5:
                    level = 400;
                    break;
                case 6:
                    level = 500;
                    break;
            }

            sections.Add(999);
            hasCredits = false;
            delayTable.Add(new List<int> { 30 });//ARE
            delayTable.Add(new List<int> { 27 });//line ARE
            delayTable.Add(new List<int> { 16 });//DAS
            delayTable.Add(new List<int> { 30 });//lock
            delayTable.Add(new List<int> { 44 });//line clear
        }

        public override void onClear(int lines, Tetromino tet, long time, bool bravo, bool split)
        {
            clearedLines += lines;
            if(clearedLines >= 100)
            {
                clearedLines = 100;
                inCredits = true;
            }
        }

        public override void draw(Graphics drawBuffer, Font f_Maestro, SolidBrush b, bool replay)
        {
            drawBuffer.DrawString("CLEARED LINES:", f_Maestro, b, 480, 140);
                drawBuffer.DrawString(clearedLines.ToString(), f_Maestro, b, 480, 160);
        }
    }
}
