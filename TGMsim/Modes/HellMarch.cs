using System.Collections.Generic;
using System.Drawing;

namespace TGMsim.Modes
{
    class HellMarch : Mode
    {
        private int roadProgress = 0;
        private int teteri = 0;
        private bool brav = false;

        public HellMarch()
        {
            grades = new List<string> { "", "S", "M", "MM", "SM" };
            ModeName = "HELL MARCH";
            border = Color.DarkTurquoise;
            boardsProgress = 0;
            endLevel = 0;
            drawSec = false;
            creditsSong = "crdtinvis";

            for (int i = 0; i < 18; i++)
                targets.Add(i);
            
            delayTable.Add(new List<int> { 27, 27, 27, 18, 14, 14, 8, 7, 6 });//ARE
            delayTable.Add(new List<int> { 27, 27, 18, 14, 8, 8, 8, 7, 6 });//LINE ARE
            delayTable.Add(new List<int> { 14, 8, 8, 8, 8, 6, 6, 6, 6 });//DAS
            delayTable.Add(new List<int> { 30, 30, 30, 30, 30, 17, 17, 15, 15 });//LOCK
            delayTable.Add(new List<int> { 40, 25, 16, 12, 6, 6, 6, 6, 6 });//LINE CLEAR
        }

        public override void onPut(Tetromino tet, bool clear)
        {
            if (roadProgress == 1)
                garbTimer++;
        }

        public override void onClear(int lines, Tetromino tet, long time, bool bravo)
        {
            if(inCredits)
            {
                if (bravo)
                    brav = true;
            }
            if (roadProgress == 2 && lines == 4)
            {
                teteri++;
                if(teteri == 10)
                {
                    grade = 3;
                    modeClear = true;
                    inCredits = true;
                    creditsType = CreditsTypes.invisible;
                    g20 = true;
                }
            }
            if (roadProgress == 1 && boardGems == 0)
            {
                modeClear = true;
                presetBoards = false;

                gimList = new List<Gimmick>(); //replace garbo with invis
                Gimmick g = new Gimmick
                {
                    startLvl = 0,
                    endLvl = 0,
                    type = Gimmick.Type.INVIS
                };
                gimList.Add(g);
                grade = 2;
                showGrade = true;
                curSection = 2;
                continueMode = true;
                roadProgress = 2;
                outlineFlashing = true;
                outlineFlashDelay = 2;
                updateMusic();
            }
            else if (roadProgress == 0 && targets.Count == 0)
            {
                modeClear = true;

                presetBoards = true;
                boardsFile = "hell";

                Gimmick g = new Gimmick
                {
                    type = Gimmick.Type.GARBAGE,
                    startLvl = 0,
                    endLvl = 0,
                    parameter = 6
                };
                gimList.Add(g);

                raiseGarbOnClear = true;
                garbDelay = 12;
                
                garbLine = 1;
                garbType = GarbType.HIDDEN;

                curSection = 1;
                continueMode = true;
                roadProgress = 1;
                grade = 1;
                updateMusic();
            }
            level += lines;
        }

        public override void onGameOver()
        {
            if (inCredits && creditsClear)
            {
                grade = 4;
                if (brav)
                    orangeLine = true;
            }
        }

        public override void updateMusic()
        {
            if (roadProgress == 2 && Audio.song != "Level 5")
            {
                Audio.stopMusic();
                Audio.playMusic("Level 5");
                return;
            }
            else if (roadProgress == 1 && Audio.song != "Casual 2")
            {
                Audio.stopMusic();
                Audio.playMusic("Casual 2");
                return;
            }
            else if (roadProgress == 0 && Audio.song != "Casual 1")
            {
                Audio.stopMusic();
                Audio.playMusic("Casual 1");
                return;
            }
        }
    }
}
