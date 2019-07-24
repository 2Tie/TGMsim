using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TGMsim.Modes
{
    class Master2 : Mode
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

        public long t = 0;
        public List<bool> GMflags = new List<bool>();

        public Master2()
        {
            ModeName = "MASTER 2";
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
            delayTable.Add(new List<int> { 25, 25, 25, 16, 12, 12, 12 });//ARE
            delayTable.Add(new List<int> { 25, 25, 25, 25, 25, 25, 25 });//line ARE
            delayTable.Add(new List<int> { 14, 8, 8, 8, 8, 6, 6 });//DAS
            delayTable.Add(new List<int> { 30, 30, 30, 30, 30, 17, 17 });//LOCK
            delayTable.Add(new List<int> { 40, 25, 16, 12, 6, 6, 6 });//LINE CLEAR
            comboTable.Add(new List<double>() { 1.0, 1.2, 1.2, 1.4, 1.4, 1.4, 1.4, 1.5, 1.5, 2.0 });
            comboTable.Add(new List<double>() { 1.0, 1.4, 1.5, 1.6, 1.7, 1.8, 1.9, 2.0, 2.1, 2.5 });
            comboTable.Add(new List<double>() { 1.0, 1.5, 1.8, 2.0, 2.2, 2.3, 2.4, 2.5, 2.6, 3.0 });
            comboTable.Add(new List<double>() { 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0 });
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
            creditsType = 1;
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
                    level += 1;
                tetCount++;
            }
        }

        public override void onTick(long time)
        {
            t = time;
            secTimer.tick();
            if(!comboing && !inCredits)
                gradeTime++;
            if (gradeTime > decayRate[grade])
            {
                gradeTime = 0;
                if(gradePoints != 0)
                    gradePoints--;
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
            checkAudioFadeout();
        }

        public override void onGameOver()
        {
            if (creditsClear)
            {
                if(grade == 18)
                    grade = 19;//award GM
                orangeLine = true;
            }
        }

        public override void onClear(int lines, Tetromino tet, long time, bool bravo)
        {
            pipCount -= 10*lines;

            int oldLvl = level;
            level += lines;

            if (level >= endLevel && endLevel != 0)
            {
                level = endLevel;
                if(!inCredits) //test for invis roll - if M
                {
                    if (grade == 18)
                        creditsType = 2;
                }
                inCredits = true;
                outlineStack = false;
            }

            //check for tetris
            if (lines == 4)
            {
                tetrises++;
                secTet[curSection]++;
                Audio.playSound(Audio.s_Tetris);
            }

            //bravo handling
            int b = 1;
            if (bravo)
            {
                b = 4;
                bravos++;
            }

            //score handling

            combo = combo + (2 * lines) - 2;

            score += ((int)Math.Ceiling((double)(oldLvl + lines) / 4) + tet.soft + (2 * tet.sonic)) * lines * combo * b;

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
                        Audio.playSound(Audio.s_Grade);
                        masteringTime.count = time;
                    }
                }
            }

            if (comboing)
                Audio.playSound(Audio.s_Combo);

            comboing = true;
            //section handling

            if(curSection < sections.Count())
            if (level >= sections[curSection])
            {
                curSection++;
                showGhost = false;
                secTet.Add(0);
                if (curSection >= 9)
                    lockSafety = true;
                //GM FLAGS
                if (GMflags.Count == 0 && level >= 100)
                {
                    if (secTet[0] > 0 && t <= 90000)
                        GMflags.Add(true);
                    else
                        GMflags.Add(false);
                }
                else if (GMflags.Count == 1 && level >= 200)
                {
                    if (secTet[1] > 0)
                        GMflags.Add(true);
                    else
                        GMflags.Add(false);
                }
                else if (GMflags.Count == 2 && level >= 300)
                {
                    if (secTet[2] > 0)
                        GMflags.Add(true);
                    else
                        GMflags.Add(false);
                }
                else if (GMflags.Count == 3 && level >= 400)
                {
                    if (secTet[3] > 0)
                        GMflags.Add(true);
                    else
                        GMflags.Add(false);
                }
                else if (GMflags.Count == 4 && level >= 500)
                {
                    if (secTet[4] > 0)
                        GMflags.Add(true);
                    else
                        GMflags.Add(false);

                    if (t <= 360000)
                        GMflags.Add(true);
                    else
                        GMflags.Add(false);
                }
                else if (GMflags.Count == 6 && level >= 600)
                {
                    if (secTet[5] > 0 && secTimes[5] > (secTimes[0] + secTimes[1] + secTimes[2] + secTimes[3] + secTimes[4]) / 5)
                        GMflags.Add(true);
                    else
                        GMflags.Add(false);
                }
                else if (GMflags.Count == 7 && level >= 700)
                {
                    if (secTet[6] > 0 && secTimes[6] > (secTimes[0] + secTimes[1] + secTimes[2] + secTimes[3] + secTimes[4]) / 5)
                        GMflags.Add(true);
                    else
                        GMflags.Add(false);
                }
                else if (GMflags.Count == 8 && level >= 800)
                {
                    if (secTet[7] > 0 && secTimes[7] > (secTimes[0] + secTimes[1] + secTimes[2] + secTimes[3] + secTimes[4]) / 5)
                        GMflags.Add(true);
                    else
                        GMflags.Add(false);
                }
                else if (GMflags.Count == 9 && level >= 900)
                {
                    if (secTet[8] > 0 && secTimes[8] > (secTimes[0] + secTimes[1] + secTimes[2] + secTimes[3] + secTimes[4]) / 5)
                        GMflags.Add(true);
                    else
                        GMflags.Add(false);
                }
                else if (GMflags.Count == 10 && level >= 999)
                {
                    if (secTimer.count <= 45000 && t <= 570000 && grade == 17)
                        GMflags.Add(true);
                    else
                        GMflags.Add(false);

                    bool gm = true;
                        foreach (bool flag in GMflags)
                        {
                            if (flag == false)
                            {
                                gm = false;
                                break;
                            }
                        }
                    if (gm)
                        grade = 18;
                }
                //MUSIC
                updateMusic();
                //DELAYS
                int num = curSection - 4;
                if (num > 0)
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
                secTimer = new FrameTimer();
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

        public override void updateMusic()
        {
            if (curSection == 9 && Audio.song != "Level 4")
            {
                Audio.stopMusic();
                Audio.playMusic("Level 4");
                return;
            }
            else if (curSection == 7 && Audio.song != "Level 3")
            {
                Audio.stopMusic();
                Audio.playMusic("Level 3");
                return;
            }
            else if (curSection == 5 && Audio.song != "Level 2")
            {
                Audio.stopMusic();
                Audio.playMusic("Level 2");
                return;
            }
            else if (curSection == 0 && Audio.song != "Level 1")
            {
                Audio.stopMusic();
                Audio.playMusic("Level 1");
                return;
            }
        }

        void checkAudioFadeout()
        {
            if (curSection == 4 && level % 100 > 84) Audio.stopMusic();
            if (curSection == 6 && level % 100 > 84) Audio.stopMusic();
            if (curSection == 8 && level % 100 > 84) Audio.stopMusic();
        }

        public override void draw(Graphics drawBuffer, Font f, SolidBrush b, bool replay)
        {
            if (replay)
            {
                drawBuffer.DrawString(grade.ToString(), f, b, 20, 300);
                drawBuffer.DrawString(intGrade.ToString(), f, b, 20, 312);
                drawBuffer.DrawString(gradePoints.ToString(), f, b, 20, 324);

                for (int i = 0; i < GMflags.Count; i++)
                    drawBuffer.DrawString(GMflags[i] ? "+" : "-", f, b, 20 + 10 * i, 336);
            }
        }
    }
}
