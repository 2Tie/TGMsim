using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TGMsim.Modes
{
    class Shirase : Mode
    {
        public int gradeCombo = 0;
        public int pipCount = 0;
        public int tetCount = 0;
        public int rotCount = 0;
        public int bravos = 0;
        public bool recovering = false;
        public int recoveries = 0;
        public long t = 0;

        public FrameTimer secTimer = new FrameTimer();
        public List<long> secTimes = new List<long>();
        public FrameTimer coolTime = new FrameTimer();

        public Shirase()
        {
            grades = new List<string> { "", "S1", "S2", "S3", "S4", "S5", "S6", "S7", "S8", "S9", "S10", "S11", "S12", "S13" };
            ModeName = "SHIRASE";
            toriCredits = false;
            showGhost = false;
            endLevel = 1300;
            sections.Add(100);
            sections.Add(200);
            sections.Add(300);
            sections.Add(400);
            sections.Add(500);
            sections.Add(600);
            sections.Add(700);
            sections.Add(800);
            sections.Add(900);
            sections.Add(1000);
            sections.Add(1100);
            sections.Add(1200);
            sections.Add(1300);
            var gL = new Gimmick();
            gL.type = Gimmick.Type.GARBAGE;
            gL.startLvl = 500;
            gL.endLvl = 600;
            gL.parameter = 20;
            gimList.Add(gL);
            gL = new Gimmick();
            gL.type = Gimmick.Type.GARBAGE;
            gL.startLvl = 600;
            gL.endLvl = 700;
            gL.parameter = 18;
            gimList.Add(gL);
            gL = new Gimmick();
            gL.type = Gimmick.Type.GARBAGE;
            gL.startLvl = 700;
            gL.endLvl = 800;
            gL.parameter = 10;
            gimList.Add(gL);
            gL = new Gimmick();
            gL.type = Gimmick.Type.GARBAGE;
            gL.startLvl = 800;
            gL.endLvl = 900;
            gL.parameter = 9;
            gimList.Add(gL);
            gL = new Gimmick();
            gL.type = Gimmick.Type.GARBAGE;
            gL.startLvl = 900;
            gL.endLvl = 1000;
            gL.parameter = 8;
            gimList.Add(gL);
            gL = new Gimmick();
            gL.type = Gimmick.Type.BONES;
            gL.startLvl = 1000;
            gL.endLvl = 1301;
            gimList.Add(gL);
            gL = new Gimmick();
            gL.type = Gimmick.Type.BIG;
            gL.startLvl = 1300;
            gL.endLvl = 1301;
            gimList.Add(gL);
            garbType = GarbType.COPY;
            border = Color.DarkRed;
            g20 = true;
            initialGrade = 0;
            delayTable.Add(new List<int> { 11, 11, 11, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5 });
            delayTable.Add(new List<int> { 8, 7, 6, 6, 5, 5, 5, 5, 5, 5, 5, 5, 5, 6 });
            delayTable.Add(new List<int> { 8, 6, 6, 6, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4 });
            delayTable.Add(new List<int> { 19, 19, 18, 16, 14, 13, 13, 13, 13, 13, 13, 11, 9, 16 });
            delayTable.Add(new List<int> { 5, 4, 3, 3, 2, 2, 2, 2, 2, 2, 2, 2, 2, 5 });
            secTet.Add(0);
        }

        public override void onSpawn()
        {
            if (firstPiece)
            {
                firstPiece = false;
            }
            else if (level < endLevel)
            {
                if (level < sections[curSection] - 1)
                    level += 1;
                tetCount++;
            }
        }

        public override void onGameOver()
        {
            if (creditsClear)
                orangeLine = true;
        }

        public override void onTick(long time)
        {
            t = time;
            secTimer.tick();
            if (coolTime.count > 0)
            {
                coolTime.tick();
                if (coolTime.count > 3000)
                    coolTime.count = 0;
            }
        }

        public override void onPut(Tetromino tet, bool clear)
        {
            pipCount += 4;
            if (pipCount >= 150)
                recovering = true;
            rotCount += tet.rotations;
            if (clear == false)
            {
                combo = 1;
                gradeCombo = 0;
                comboing = false;
            }
            if (level > 499 && level < 1000)
                garbTimer++;
            checkAudioFadeout();
        }

        public override void onClear(int lines, Tetromino tet, long time, bool bravo)
        {
            pipCount -= 10 * lines;

            int oldLvl = level;
            level += lines;

            if (level >= endLevel && endLevel != 0)
            {
                level = endLevel;
                inCredits = true;
            }

            garbTimer -= lines;
            if (garbTimer < 0)
                garbTimer = 0;

            //check for tetris
            if (lines == 4)
            {
                tetrises++;
                secTet[curSection]++;
                Audio.playSound(Audio.s_Tetris);
            }

            //bravo handling
            //int b = 1;
            if (bravo)
            {
                //b = 4;
                bravos++;
            }

            //score handling

            combo = combo + (2 * lines) - 2;

            int sped = baseLock - tet.life;
            if (sped < 0) sped = 0;
            score += ((int)Math.Ceiling((double)(oldLvl + lines) / 4) + tet.soft + tet.sonic) * lines * combo + (int)(Math.Ceiling((double)level / 2)) + sped;

            if (lines > 1)
            {
                gradeCombo++;
            }

            int comboval = gradeCombo;
            if (comboval > 10) comboval = 10;

            if (comboing)
                Audio.playSound(Audio.s_Combo);

            comboing = true;
            //section handling
            if (curSection < sections.Count())
                if (level >= sections[curSection])
                {
                    curSection++;
                    secTet.Add(0);
                    grade += 1;
                    //GM FLAGS
                    if (secTimer.count > 60000)
                    {
                        Audio.playSound(Audio.s_Regret);
                        grade -= 1;
                        coolTime.tick();
                    }

                    //TORIKAN
                    if (curSection == 5 && t > 148000)
                    {
                        level = 500;
                        torikan = true;
                        torDef = t - 148000;
                    }
                    if (curSection == 10 && t > 296000)
                    {
                        level = 1000;
                        torikan = true;
                        torDef = t - 296000;
                    }
                    //MUSIC
                    updateMusic();
                    //DELAYS
                    //int num = curSection;
                    //if (num < 6)
                    //{
                    baseARE = delayTable[0][curSection];
                    baseARELine = delayTable[1][curSection];
                    baseDAS = delayTable[2][curSection];
                    baseLock = delayTable[3][curSection];
                    baseLineClear = delayTable[4][curSection];
                    //}
                    //MEDALS
                    //RO
                    if (curSection == 3)
                        if ((rotCount * 5) / (tetCount * 6) >= 1)
                        {
                            medals[1] = 1;
                            rotCount = 0;
                            tetCount = 0;
                            Audio.playSound(Audio.s_Medal);
                        }
                    if (curSection == 7)
                        if ((rotCount * 5) / (tetCount * 6) >= 1)
                        {
                            medals[1] = 2;
                            rotCount = 0;
                            tetCount = 0;
                            Audio.playSound(Audio.s_Medal);
                        }
                    //ST
                    secTimes.Add(secTimer.count);
                    int st = 90000;
                    if (secTimer.count < st - 10000 && medals[2] < 1)
                    {
                        medals[2] = 1;
                        Audio.playSound(Audio.s_Medal);
                    }
                    if (secTimer.count < st - 5000 && medals[2] < 2)
                    {
                        medals[2] = 2;
                        Audio.playSound(Audio.s_Medal);
                    }
                    if (secTimer.count < st && medals[2] < 3)
                    {
                        medals[2] = 3;
                        Audio.playSound(Audio.s_Medal);
                    }

                    secTimer.count = 0;

                }
            //MEDALS
            //AC
            if (bravos == 1 && medals[0] == 0)
            {
                medals[0] = 1;
                Audio.playSound(Audio.s_Medal);
            }
            if (bravos == 2 && medals[0] == 1)
            {
                medals[0] = 2;
                Audio.playSound(Audio.s_Medal);
            }
            if (bravos == 3 && medals[0] == 2)
            {
                medals[0] = 3;
                Audio.playSound(Audio.s_Medal);
            }
            //SK
            if (tetrises == 10 && medals[3] == 0)
            {
                medals[3] = 1;
                Audio.playSound(Audio.s_Medal);
            }
            if (tetrises == 20 && medals[3] == 1)
            {
                medals[3] = 2;
                Audio.playSound(Audio.s_Medal);
            }
            if (tetrises == 35 && medals[3] == 2)
            {
                medals[3] = 3;
                Audio.playSound(Audio.s_Medal);
            }
            //RE
            if (recovering == true && pipCount <= 70)
            {
                recoveries++;
                recovering = false;
            }
            if (recoveries == 1 && medals[4] == 0)
            {
                medals[4] = 1;
                Audio.playSound(Audio.s_Medal);
            }
            if (recoveries == 2 && medals[4] == 0)
            {
                medals[4] = 2;
                Audio.playSound(Audio.s_Medal);
            }
            if (recoveries == 4 && medals[4] == 0)
            {
                medals[4] = 3;
                Audio.playSound(Audio.s_Medal);
            }
            //CO
            int big = 1;
            if (bigmode == true)
                big = 2;
            if ((int)(Math.Ceiling((double)4 / big)) == gradeCombo)
            {
                medals[5] = 1;
                Audio.playSound(Audio.s_Medal);
            }
            if ((int)(Math.Ceiling((double)5 / big)) == gradeCombo)
            {
                medals[5] = 2;
                Audio.playSound(Audio.s_Medal);
            }
            if ((int)(Math.Ceiling((double)7 / big)) == gradeCombo)
            {
                medals[5] = 3;
                Audio.playSound(Audio.s_Medal);
            }
        }

        public override void draw(Graphics drawBuffer, Font f, SolidBrush b, bool replay)
        {
            SolidBrush tb = new SolidBrush(Color.White);
            
            if (coolTime.count > 0)
            {
                drawBuffer.DrawString("REGRET!", SystemFonts.DefaultFont, tb, 200 + 300, 350);
            }
        }

        public override void updateMusic()
        {
            if (curSection == 0)
            {
                Audio.stopMusic();
                Audio.playMusic("Level 3");
                return;
            }
            if (curSection == 5)
            {
                Audio.stopMusic();
                Audio.playMusic("Level 4");
                return;
            }
            if (curSection == 7)
            {
                Audio.stopMusic();
                Audio.playMusic("Level 5");
                return;
            }
            if (curSection == 10)
            {
                Audio.stopMusic();
                Audio.playMusic("Level 6");
                return;
            }
            if (curSection == 14)
            {
                Audio.stopMusic();
                Audio.playMusic("Shirase");
                return;
            }
        }

        void checkAudioFadeout()
        {
            if (curSection == 4 && level % 100 > 84) Audio.stopMusic();
            if (curSection == 6 && level % 100 > 84) Audio.stopMusic();
            if (curSection == 9 && level % 100 > 84) Audio.stopMusic();
            if (curSection == 13 && level % 100 > 84) Audio.stopMusic();
        }
    }
}
