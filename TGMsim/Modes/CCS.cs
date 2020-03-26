using System.Collections.Generic;
using System.Drawing;

namespace TGMsim.Modes
{
    class CCS : Mode
    {
        const byte RED = 0b01000000;
        const byte ORANGE = 0b00000100;
        const byte YELLOW = 0b0000010;
        const byte GREEN = 0b00100000;
        const byte CYAN = 0b00000001;
        const byte BLUE = 0b0001000;
        const byte PURPLE = 0b00010000;

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
            if (variant == 0)
            {
                recycleGems = true;
                recycleTiming rt = new recycleTiming();
                rt.delay = 6;
                rt.colours = YELLOW | CYAN | BLUE;
                recycleTimings.Add(rt);
                rt = new recycleTiming();
                rt.delay = 7;
                rt.colours = RED | ORANGE | GREEN | PURPLE;
                recycleTimings.Add(rt);
            }
            limit = variant==0?180000:1000*60*20;//three minutes or twenty minutes
            grades = new List<string> { "E1", "N1", "E2", "N2", "E3", "N3", "E4", "N4", "E5", "N5", "E6", "N6", "E7", "N7", "E8", "N8", "E9", "N9", "E10", "N10", "E11", "N11", "E12", "N12", "E13", "N13", "E14", "N14", "E15", "N15", "E16", "N16", "E17", "N17", "E18", "N18", "E CLEAR", "N CLEAR" };
        }

        public override void onPut(Tetromino tet, bool clear)
        {
            garbTimer++;
            if (boardsProgress == 4)
                gimCounter++;
        }

        public override void onGameOver()
        {
            grade = boardsProgress * 2 + variant;
        }

        public override void onTick(long time)
        {
            if (boardsProgress == 8)
            {
                gimCounter++;
                if (gimCounter + 4 * 60 > 20*60)
                    gimCounter = 0;
            }
        }

        public override void onClear(int lines, Tetromino tet, long time, bool bravo)
        {
            level += lines;
            if(boardsProgress == 9)
            {
                gimCounter += lines;
            }
            if (boardGems == 0)
            {
                garbTimer = 0;
                garbLine = 0;
                level = 0;
                if (variant == 0) limit = (int)(time + 1000 * 60 * 3);//reset easy time limit
                gimList = new List<Gimmick>(); //reset it for each level
                boardsProgress++;
                recycleProgress = 0;
                modeClear = true;
                if (boardsProgress < 18)
                    continueMode = true;

                recycleTimings = new List<recycleTiming>();

                if (boardsProgress == 4)
                {
                    Gimmick g = new Gimmick();
                    g.type = Gimmick.Type.MIRROR;
                    g.parameter = 3;
                    g.startLvl = 0;
                    g.endLvl = 0;
                    g.delay = 40;
                    gimList.Add(g);
                }
                else if(boardsProgress == 8)
                {
                    Gimmick g = new Gimmick();
                    g.type = Gimmick.Type.HIDENEXT;
                    g.parameter = 13*60;
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

                if(variant == 0)
                {
                    recycleTiming rt = new recycleTiming();
                    switch (boardsProgress)
                    {
                        case 1:
                            rt = new recycleTiming();
                            rt.delay = 5;
                            rt.colours = ORANGE | YELLOW;
                            recycleTimings.Add(rt);
                            rt = new recycleTiming();
                            rt.delay = 6;
                            rt.colours = CYAN | BLUE | PURPLE;
                            recycleTimings.Add(rt);
                            rt = new recycleTiming();
                            rt.delay = 7;
                            rt.colours = RED | GREEN;
                            recycleTimings.Add(rt);
                            break;
                        case 2:
                            rt = new recycleTiming();
                            rt.delay = 5;
                            rt.colours = YELLOW | GREEN | CYAN;
                            recycleTimings.Add(rt);
                            rt = new recycleTiming();
                            rt.delay = 6;
                            rt.colours = ORANGE | BLUE;
                            recycleTimings.Add(rt);
                            rt = new recycleTiming();
                            rt.delay = 8;
                            rt.colours = RED;
                            recycleTimings.Add(rt);
                            rt = new recycleTiming();
                            rt.delay = 9;
                            rt.colours = PURPLE;
                            recycleTimings.Add(rt);
                            break;
                        case 3:
                            rt = new recycleTiming();
                            rt.delay = 6;
                            rt.colours = ORANGE | BLUE;
                            recycleTimings.Add(rt);
                            rt = new recycleTiming();
                            rt.delay = 7;
                            rt.colours = YELLOW;
                            recycleTimings.Add(rt);
                            rt = new recycleTiming();
                            rt.delay = 8;
                            rt.colours = GREEN | PURPLE;
                            recycleTimings.Add(rt);
                            rt = new recycleTiming();
                            rt.delay = 9;
                            rt.colours = RED | CYAN;
                            recycleTimings.Add(rt);
                            break;
                        case 4:
                            rt = new recycleTiming();
                            rt.delay = 6;
                            rt.colours = ORANGE | YELLOW | GREEN | BLUE;
                            recycleTimings.Add(rt);
                            break;
                        case 5:
                            rt = new recycleTiming();
                            rt.delay = 6;
                            rt.colours = ORANGE | YELLOW | CYAN | BLUE;
                            recycleTimings.Add(rt);
                            break;
                        case 6:
                            rt = new recycleTiming();
                            rt.delay = 5;
                            rt.colours = RED | ORANGE | BLUE | PURPLE;
                            recycleTimings.Add(rt);
                            rt = new recycleTiming();
                            rt.delay = 6;
                            rt.colours = YELLOW | GREEN | CYAN;
                            recycleTimings.Add(rt);
                            break;
                        case 7:
                            rt = new recycleTiming();
                            rt.delay = 1;
                            rt.colours = RED | CYAN;
                            recycleTimings.Add(rt);
                            rt = new recycleTiming();
                            rt.delay = 2;
                            rt.colours = YELLOW | GREEN | BLUE;
                            recycleTimings.Add(rt);
                            rt = new recycleTiming();
                            rt.delay = 3;
                            rt.colours = ORANGE | PURPLE;
                            recycleTimings.Add(rt);
                            break;
                        case 9:
                            rt = new recycleTiming();
                            rt.delay = 6;
                            rt.colours = YELLOW | GREEN | CYAN | PURPLE;
                            recycleTimings.Add(rt);
                            rt = new recycleTiming();
                            rt.delay = 7;
                            rt.colours = RED;
                            recycleTimings.Add(rt);
                            rt = new recycleTiming();
                            rt.delay = 9;
                            rt.colours = ORANGE | BLUE;
                            recycleTimings.Add(rt);
                            break;
                        case 10:
                            rt = new recycleTiming();
                            rt.delay = 5;
                            rt.colours = RED | ORANGE | YELLOW;
                            recycleTimings.Add(rt);
                            rt = new recycleTiming();
                            rt.delay = 6;
                            rt.colours = BLUE | PURPLE;
                            recycleTimings.Add(rt);
                            rt = new recycleTiming();
                            rt.delay = 7;
                            rt.colours = CYAN;
                            recycleTimings.Add(rt);
                            rt = new recycleTiming();
                            rt.delay = 8;
                            rt.colours = GREEN;
                            recycleTimings.Add(rt);
                            break;
                        case 11:
                            rt = new recycleTiming();
                            rt.delay = 6;
                            rt.colours = RED | GREEN | CYAN | BLUE | PURPLE;
                            recycleTimings.Add(rt);
                            rt = new recycleTiming();
                            rt.delay = 7;
                            rt.colours = ORANGE;
                            recycleTimings.Add(rt);
                            rt = new recycleTiming();
                            rt.delay = 8;
                            rt.colours = YELLOW;
                            recycleTimings.Add(rt);
                            break;
                        case 12:
                            rt = new recycleTiming();
                            rt.delay = 3;
                            rt.colours = YELLOW | CYAN;
                            recycleTimings.Add(rt);
                            rt = new recycleTiming();
                            rt.delay = 4;
                            rt.colours = ORANGE | GREEN | BLUE;
                            recycleTimings.Add(rt);
                            rt = new recycleTiming();
                            rt.delay = 5;
                            rt.colours = RED | PURPLE;
                            recycleTimings.Add(rt);
                            break;
                        case 13:
                            rt = new recycleTiming();
                            rt.delay = 6;
                            rt.colours = YELLOW | CYAN;
                            recycleTimings.Add(rt);
                            rt = new recycleTiming();
                            rt.delay = 8;
                            rt.colours = ORANGE | BLUE | PURPLE;
                            recycleTimings.Add(rt);
                            rt = new recycleTiming();
                            rt.delay = 9;
                            rt.colours = RED | GREEN;
                            recycleTimings.Add(rt);
                            break;
                        case 14:
                            rt = new recycleTiming();
                            rt.delay = 1;
                            rt.colours = CYAN;
                            recycleTimings.Add(rt);
                            rt = new recycleTiming();
                            rt.delay = 6;
                            rt.colours = BLUE;
                            recycleTimings.Add(rt);
                            rt = new recycleTiming();
                            rt.delay = 7;
                            rt.colours = PURPLE | GREEN;
                            recycleTimings.Add(rt);
                            rt = new recycleTiming();
                            rt.delay = 8;
                            rt.colours = RED | ORANGE | YELLOW;
                            recycleTimings.Add(rt);
                            break;
                        case 15:
                            rt = new recycleTiming();
                            rt.delay = 6;
                            rt.colours = RED | ORANGE | YELLOW | GREEN | CYAN | BLUE | PURPLE;
                            recycleTimings.Add(rt);
                            break;
                        case 16:
                            rt = new recycleTiming();
                            rt.delay = 3;
                            rt.colours = GREEN;
                            recycleTimings.Add(rt);
                            rt = new recycleTiming();
                            rt.delay = 4;
                            rt.colours = PURPLE;
                            recycleTimings.Add(rt);
                            rt = new recycleTiming();
                            rt.delay = 5;
                            rt.colours = BLUE;
                            recycleTimings.Add(rt);
                            rt = new recycleTiming();
                            rt.delay = 6;
                            rt.colours = RED | ORANGE;
                            recycleTimings.Add(rt);
                            rt = new recycleTiming();
                            rt.delay = 7;
                            rt.colours = YELLOW;
                            recycleTimings.Add(rt);
                            rt = new recycleTiming();
                            rt.delay = 8;
                            rt.colours = CYAN;
                            recycleTimings.Add(rt);
                            break;
                        default:
                            break;
                    }
                }
            }
        }
    }
}
