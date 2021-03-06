﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace TGMsim.Modes
{
    class Master3 : Mode
    {
        public List<List<double>> comboTable = new List<List<double>>();
        public List<int> gradeIntTGM2 = new List<int> { 0, 1, 2, 3, 4, 5, 5, 6, 6, 7, 7, 7, 8, 8, 8, 9, 9, 9, 10, 11, 12, 12, 12, 13, 13, 14, 14, 15, 15, 16, 16, 17 };
        public List<int> decayRate = new List<int>() { 125, 80, 80, 50, 45, 45, 45, 40, 40, 40, 40, 40, 30, 30, 30, 20, 20, 20, 20, 20, 15, 15, 15, 15, 15, 15, 15, 15, 15, 15, 10, 10 };
        public List<List<int>> baseGradePts = new List<List<int>>();
        public int gradePoints = 0;
        public int gradeCombo = 0;
        public int gradeTime = 0;
        public int intGrade = 0;
        public int pipCount = 0;
        public int tetCount = 0;
        public int rotCount = 0;
        public int bravos = 0;
        public bool recovering = false;
        public int recoveries = 0;
        
        public FrameTimer secTimer = new FrameTimer();
        public List<long> secTimes = new List<long>();
        public FrameTimer coolTime = new FrameTimer();
        public int cools = 0;
        public long t = 0;

        int creditGrades = 0;

        public List<int> coolCounter = new List<int>();

        public List<int> secCools = new List<int> { 52000, 52000, 49000, 45000, 45000, 42000, 42000, 38000, 38000 };
        public List<int> secRegrets = new List<int> { 90000, 75000, 75000, 68000, 60000, 60000, 50000, 50000, 50000, 50000 };

        List<List<int>> creditGradePoints = new List<List<int>> { new List<int>{ 4, 8, 12, 26 }, new List<int>{ 10, 20, 30, 100 } };


        public Master3()
        {
            ModeName = "MASTER 3";
            border = Color.Blue;
            grades = new List<string> { "9", "8", "7", "6", "5", "4", "3", "2", "1", "S1", "S2", "S3", "S4", "S5", "S6", "S7", "S8", "S9", "m1", "m2", "m3", "m4", "m5", "m6", "m7", "m8", "m9", "M", "MK", "MV", "MO", "MM", "GM" };
            showGrade = false;
            toriCredits = false;
            lockSafety = true;
            sections.Add(100);
            sections.Add(200);
            sections.Add(300);
            sections.Add(400);
            sections.Add(500);
            sections.Add(600);
            sections.Add(700);
            sections.Add(800);
            sections.Add(900);
            sections.Add(999);
            delayTable.Add(new List<int> { 26, 26, 26, 17, 13, 13, 7, 6, 5 });//ARE
            delayTable.Add(new List<int> { 27, 27, 18, 14, 8, 8, 8, 7, 6 });//LINE ARE
            delayTable.Add(new List<int> { 14, 8, 8, 8, 8, 6, 6, 6, 6 });//DAS
            delayTable.Add(new List<int> { 31, 31, 31, 31, 31, 18, 18, 16, 16 });//LOCK
            delayTable.Add(new List<int> { 39, 24, 15, 11, 5, 5, 5, 5, 5 });//LINE CLEAR
            comboTable.Add(new List<double>() { 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0 });
            comboTable.Add(new List<double>() { 1.0, 1.2, 1.2, 1.4, 1.4, 1.4, 1.4, 1.5, 1.5, 2.0 });
            comboTable.Add(new List<double>() { 1.0, 1.4, 1.5, 1.6, 1.7, 1.8, 1.9, 2.0, 2.1, 2.5 });
            comboTable.Add(new List<double>() { 1.0, 1.5, 1.8, 2.0, 2.2, 2.3, 2.4, 2.5, 2.6, 3.0 });
            baseGradePts.Add(new List<int>() { 10, 10, 10, 10, 10, 5, 5, 5, 5, 5 });
            baseGradePts.Add(new List<int>() { 20, 20, 20, 15, 15, 15, 10, 10, 10, 10 });
            baseGradePts.Add(new List<int>() { 40, 30, 30, 30, 20, 20, 20, 15, 15, 15 });
            baseGradePts.Add(new List<int>() { 50, 40, 40, 40, 40, 30, 30, 30, 30, 30 });
            for (int i = 0; i < 22; i++)
            {
                baseGradePts[0].Add(2);
                baseGradePts[1].Add(12);
                baseGradePts[2].Add(13);
                baseGradePts[3].Add(30);
            }
            secTet.Add(0);
            hasSecretGrade = true;
            minSecret = 5;
            secretGrades = new List<string> { "9", "8", "7", "6", "5", "4", "3", "2", "1", "S1", "S2", "S3", "S4", "S5", "S6", "S7", "S8", "S9", "GM" };
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
                {
                    level += 1;
                    if (level == sections[curSection] - 1)
                        Audio.playSound(Audio.s_Bell);
                }
                tetCount++;
            }
        }

        public override void onTick(long time, Field.timerType timer)
        {
            t = time;
            secTimer.tick();
            if (gradeCombo == 0 && !inCredits && timer == Field.timerType.LockDelay)
                gradeTime++;
            if (gradeTime > decayRate[grade])
            {
                gradeTime = 0;
                if (gradePoints != 0)
                    gradePoints--;
            }
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

            //COOLS
            if (level % 100 > 69)
            {
                if (coolCounter.Count <= curSection && curSection != 10)
                {
                    if (curSection != 0)
                    {
                        if ((coolCounter[curSection - 1] == 1 && secTimer.count < secTimes[curSection - 1] + 2000) || (coolCounter[curSection - 1] != 1 && secTimer.count < secCools[curSection]))
                        {
                            coolCounter.Add(1);
                            cools += 1;
                            Audio.playSound(Audio.s_Cool);
                            coolTime.tick();
                        }
                        else
                            coolCounter.Add(0);
                    }
                    else
                    {
                        if (secTimer.count < secCools[curSection])
                        {
                            coolCounter.Add(1);
                            Audio.playSound(Audio.s_Cool);
                            coolTime.tick();
                        }
                        else
                            coolCounter.Add(0);
                    }
                }
            }

            checkAudioFade();
        }

        public override void onClear(int lines, Tetromino tet, long time, bool bravo)
        {
            pipCount -= 10 * lines;

            int oldLvl = level;
            level += lines;
            if (lines > 2) level++;
            if (lines > 3) level++;

            if (level >= endLevel && endLevel != 0)
            {
                level = endLevel;
                if (!inCredits)//set credits type once at the start
                {
                    if (grade > 26 && secCools.Sum() == 8)
                    {
                        creditsType = CreditsTypes.invisible;
                        creditsSong = "crdtinvis";
                        outlineFlashing = true;
                        outlineFlashDelay = 2;
                    }
                    else
                        creditsType = CreditsTypes.vanishing;
                }
                inCredits = true;
            }

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

            int comboval = gradeCombo;
            if (comboval > 10) comboval = 10;

            int newPts = (int)(Math.Ceiling(baseGradePts[lines - 1][grade] * comboTable[lines - 1][gradeCombo]) * Math.Ceiling((double)level / 250));
            if (level > 249 && level < 500)
                newPts = newPts * 2;
            if (level > 499 && level < 750)
                newPts = newPts * 3;
            if (level > 749 && level < 1000)
                newPts = newPts * 4;

            if (lines > 1)
            {
                gradeCombo++;
            }
            if (!inCredits)
                gradePoints += newPts;
            else
                creditGrades += creditGradePoints[(int)creditsType - 1][lines];
            //update grade
            if (gradePoints > 99)
            {
                if (intGrade < gradeIntTGM2.Count - 1)
                {
                    intGrade++;
                    gradePoints = 0;
                    gradeTime = 0; //nullpo resets the decay here
                    if (gradeIntTGM2[intGrade] != grade)
                    {
                        grade++;
                        masteringTime.count = time;
                    }
                }
            }

            if (comboing && lines > 1)
                Audio.playSound(Audio.s_Combo);

            comboing = true;
            //section handling

            if (curSection < sections.Count())
                if (level >= sections[curSection])
                {
                    curSection++;
                    showGhost = false;
                    secTet.Add(0);
                    //GM FLAGS
                    //TORIKAN
                    if (curSection == 5 && t > 420000)// && exam == -1)
                    {
                        level = 500;
                        torikan = true;
                        torDef = t - 420000;
                    }
                    //MUSIC
                    updateMusic();
                    //DELAYS
                    secBonus = cools;
                    int num = curSection + cools - 4;
                    if (num > 0 && num < 9)
                    {
                        baseARE = delayTable[0][num];
                        baseARELine = delayTable[1][num];
                        baseDAS = delayTable[2][num];
                        baseLock = delayTable[3][num];
                        baseLineClear = delayTable[4][num];
                    }
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
                    //REGRET
                    if (secTimer.count > secRegrets[curSection - 1])
                    {
                        if (curSection != 9)
                            coolCounter[curSection - 1] = -1;
                        else
                            coolCounter.Add(-1);

                        Audio.playSound(Audio.s_Regret);
                        coolTime.tick();
                    }

                    secTimer = new FrameTimer();

                    //BACKGROUND

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

        public override void onGameOver()
        {
            if (creditsClear)
            {
                if (creditsType == CreditsTypes.vanishing)
                    creditGrades += 50;
                if (creditsType == CreditsTypes.invisible)
                    creditGrades += 160;
                orangeLine = true;
            }

            for (int i = 0; i < coolCounter.Count; i++)
                grade += coolCounter[i];

            for (; creditGrades > 100; creditGrades -= 100)
            {
                grade++;
            }
            if (grade < 0)
                grade = 0; //9
            if (grade > 32)
                grade = 32; //GM?
        }

        public override void updateMusic()
        {
            if (curSection >= 8)
            {
                if (Audio.song != "Level 3")
                {
                    Audio.stopMusic();
                    Audio.playMusic("Level 3");
                    return;
                }
            }
            else if (curSection >= 5)
            {
                if (Audio.song != "Level 2")
                {
                    Audio.stopMusic();
                    Audio.playMusic("Level 2");
                    return;
                }
            }
            else if (curSection >= 0 && Audio.song != "Level 1")
            {
                Audio.stopMusic();
                Audio.playMusic("Level 1");
                return;
            }
        }

        void checkAudioFade()
        {
            if (curSection + cools > 3 && level % 100 > 84 && Audio.song == "Level 1") Audio.stopMusic();
            if (curSection + cools > 6 && level % 100 > 84 && Audio.song == "Level 2") Audio.stopMusic();
        }

        public override void draw(bool replay)
        {
            if (replay)
            {
                Draw.buffer.DrawString(grade.ToString(), Draw.f_Maestro, Draw.tb, 20, 300);
                Draw.buffer.DrawString(intGrade.ToString(), Draw.f_Maestro, Draw.tb, 20, 312);
                Draw.buffer.DrawString(gradePoints.ToString(), Draw.f_Maestro, Draw.tb, 20, 324);
                for (int i = 0; i < coolCounter.Count; i++)
                    Draw.buffer.DrawString(coolCounter[i].ToString(), Draw.f_Maestro, Draw.tb, 20 + 10 * i, 336);
                Draw.buffer.DrawString(secTimer.count.ToString(), Draw.f_Maestro, Draw.tb, 20, 400);
            }

            string cTex = "REGRET!";
            if (coolTime.count > 0)
            {
                if (level % 100 >= 70)//cool
                {
                    cTex = "COOL!";
                }
                Draw.buffer.DrawString(cTex, SystemFonts.DefaultFont, Draw.tb, 200 + 300, 350);
            }
        }
    }
}
