using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TGMsim.Modes
{
    class CCS : Mode
    {
        
        public CCS( int var)
        {
            variant = var;
            ModeName = "CCS " + (var==0?"EASY":"NORMAL");
            border = Color.DarkTurquoise;
            delayTable.Add(new List<int> { 28 });//ARE
            delayTable.Add(new List<int> { 28 });//line ARE
            delayTable.Add(new List<int> { 12 });//DAS
            delayTable.Add(new List<int> { 32 });//lock
            delayTable.Add(new List<int> { 30 });//line clear
            endLevel = 0;
            drawSec = false;
            boardsProgress = 16;
            presetBoards = true;
            shadeStack = false;
            boardsFile = var==0?"ccseasy":"ccsnorm";
            hasCredits = false;
        }

        public override void onPut(Tetromino tet, bool clear)
        {
            garbTimer++;
        }

        public override void onClear(int lines, Tetromino tet, long time, bool bravo)
        {
            level += lines;
            if (boardGems == 0)
            {
                garbTimer = 0;
                garbLine = 0;
                level = 0;
                gimList = new List<Gimmick>(); //reset it for each level
                boardsProgress++;
                modeClear = true;
                if (boardsProgress < 18)
                    continueMode = true;

                if (boardsProgress == 4)
                {
                    Gimmick g = new Gimmick();
                    g.type = Gimmick.Type.MIRROR;
                    g.parameter = 3;
                    g.startLvl = 0;
                    g.endLvl = 0;
                    gimList.Add(g);
                }
                else if(boardsProgress == 8)
                {
                    Gimmick g = new Gimmick();
                    g.type = Gimmick.Type.HIDENEXT;
                    g.parameter = 17;
                    g.startLvl = 0;
                    g.endLvl = 0;
                    gimList.Add(g);
                    g = new Gimmick();
                    g.type = Gimmick.Type.GARBAGE;
                    g.parameter = 7;
                    g.startLvl = 0;
                    g.endLvl = 0;
                    gimList.Add(g);

                    raiseGarbOnClear = true;
                    garbDelay = 0;
                    garbLine = 11;
                    garbType = GarbType.HIDDEN;
                }
                else if(boardsProgress == 9)
                {
                    Gimmick g = new Gimmick();
                    g.type = Gimmick.Type.SHADE;
                    g.parameter = 2;
                    g.startLvl = 0;
                    g.endLvl = 0;
                    gimList.Add(g);
                }
                else if (boardsProgress == 11)
                {
                    Gimmick g = new Gimmick();
                    g.type = Gimmick.Type.XRAY;
                    g.parameter = variant==0?5:7;
                    g.startLvl = 0;
                    g.endLvl = 0;
                    gimList.Add(g);
                }
                else if (boardsProgress == 12)
                {
                    Gimmick g = new Gimmick();
                    g.type = Gimmick.Type.GARBAGE;
                    g.parameter = variant==0?7:3;
                    g.startLvl = 0;
                    g.endLvl = 0;
                    gimList.Add(g);
                    
                    garbLine = 7;
                }
                else if (boardsProgress == 16)
                {
                    Gimmick g = new Gimmick();
                    g.type = Gimmick.Type.XRAY;
                    g.parameter = 5;
                    g.startLvl = 0;
                    g.endLvl = 0;
                    gimList.Add(g);
                }
                else if (boardsProgress == 17)
                {
                    Gimmick g = new Gimmick();
                    g.type = Gimmick.Type.XRAY;
                    g.parameter = 5;
                    g.startLvl = 0;
                    g.endLvl = 0;
                    gimList.Add(g);
                    if(variant == 1)
                    {
                        g = new Gimmick();
                        g.type = Gimmick.Type.GARBAGE;
                        g.parameter = 5;
                        g.startLvl = 0;
                        g.endLvl = 0;
                        gimList.Add(g);
                        garbLine = 7;
                        garbType = GarbType.HIDDEN;
                    }
                }
            }
        }
    }
}
