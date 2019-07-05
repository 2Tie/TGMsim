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
            showGrade = false;
            boardsProgress = 0;
            presetBoards = true;
            shadeStack = false;
            boardsFile = var==0?"ccseasy":"ccsnorm";
            hasCredits = false;
            limitType = 3; //time
            limit = variant==0?180000:1000*60*20;//three minutes or twenty minutes
            grades = new List<string> { "E1", "N1", "E2", "N2", "E3", "N3", "E4", "N4", "E5", "N5", "E6", "N6", "E7", "N7", "E8", "N8", "E9", "N9", "E10", "N10", "E11", "N11", "E12", "N12", "E13", "N13", "E14", "N14", "E15", "N15", "E16", "N16", "E17", "N17", "E18", "N18", "E CLEAR", "N CLEAR" };
        }

        public override void onPut(Tetromino tet, bool clear)
        {
            garbTimer++;
        }

        public override void onGameOver()
        {
            grade = boardsProgress * 2 + variant;
        }

        public override void onClear(int lines, Tetromino tet, long time, bool bravo)
        {
            level += lines;
            if (boardGems == 0)
            {
                garbTimer = 0;
                garbLine = 0;
                level = 0;
                if (variant == 0) limit = (int)(time + 1000 * 60 * 3);//reset easy time limit
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
