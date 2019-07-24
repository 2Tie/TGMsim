using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TGMsim.Modes
{
    class Master2Plus : Master2
    {
        bool master = false;
        int creditLines = 0;

        public Master2Plus() : base()
        {
            delayTable[1] = new List<int> { 25, 25, 16, 12, 6, 6 };//line ARE
            ModeName = "MASTER 2+";
        }

        public override void onGameOver()
        {
            if (inCredits && master)
                grade = 18;//award M for reaching credits
            if (creditsClear && master)
                grade = 19;//award GM for clearing

            if (creditsClear && ((!master) || creditLines >= 32))//just clearing if below M gives orange, clearing with 32+ lines with M/GM awards orange
                orangeLine = true;
        }

        public override void onClear(int lines, Tetromino tet, long time, bool bravo)
        {
            pipCount -= 10 * lines;

            int oldLvl = level;
            level += lines;
            if (inCredits)
                creditLines += lines;

            if (level >= endLevel && endLevel != 0)
            {
                level = endLevel;
                if (!inCredits) //test for invis roll
                {
                    if (grade == 27)
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

            int sped = baseLock - tet.life;
            if (sped < 0) sped = 0;
            score += ((int)Math.Ceiling((double)(oldLvl + lines) / 4) + tet.soft + (2 * tet.sonic)) * lines * combo * b + (int)(Math.Ceiling((double)level / 2)) + (sped * 7);

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

            if (curSection < sections.Count())
                if (level >= sections[curSection])
                {
                    curSection++;
                    showGhost = false;
                    secTet.Add(0);
                    if (curSection >= 9)
                        lockSafety = true;

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

                    //GM FLAGS
                    if (GMflags.Count == 0 && level >= 100)
                    {
                        if (secTet[0] > 1 && secTimes[0] <= 65000)
                            GMflags.Add(true);
                        else
                            GMflags.Add(false);
                    }
                    else if (GMflags.Count == 1 && level >= 200)
                    {
                        if (secTet[1] > 1 && secTimes[1] <= 65000)
                            GMflags.Add(true);
                        else
                            GMflags.Add(false);
                    }
                    else if (GMflags.Count == 2 && level >= 300)
                    {
                        if (secTet[2] > 1 && secTimes[2] <= 65000)
                            GMflags.Add(true);
                        else
                            GMflags.Add(false);
                    }
                    else if (GMflags.Count == 3 && level >= 400)
                    {
                        if (secTet[3] > 1 && secTimes[3] <= 65000)
                            GMflags.Add(true);
                        else
                            GMflags.Add(false);
                    }
                    else if (GMflags.Count == 4 && level >= 500)
                    {
                        if (secTet[4] > 1 && secTimes[4] <= 65000)
                            GMflags.Add(true);
                        else
                            GMflags.Add(false);
                    }
                    else if (GMflags.Count == 5 && level >= 600)
                    {
                        if (secTet[5] > 0 && secTimes[5] <= ((secTimes[0] + secTimes[1] + secTimes[2] + secTimes[3] + secTimes[4]) / 5) + 2000)
                            GMflags.Add(true);
                        else
                            GMflags.Add(false);
                    }
                    else if (GMflags.Count == 6 && level >= 700)
                    {
                        if (secTet[6] > 0 && secTimes[6] <= secTimes[5] + 2000)
                            GMflags.Add(true);
                        else
                            GMflags.Add(false);
                    }
                    else if (GMflags.Count == 7 && level >= 800)
                    {
                        if (secTet[7] > 0 && secTimes[7] <= secTimes[6] + 2000)
                            GMflags.Add(true);
                        else
                            GMflags.Add(false);
                    }
                    else if (GMflags.Count == 8 && level >= 900)
                    {
                        if (secTet[8] > 0 && secTimes[8] <= secTimes[7] + 2000)
                            GMflags.Add(true);
                        else
                            GMflags.Add(false);
                    }
                    else if (GMflags.Count == 9 && level >= 999)
                    {
                        if (t <= 525000 && secTimes[9] <= secTimes[8] + 2000 && grade == 17)
                            GMflags.Add(true);
                        else
                            GMflags.Add(false);

                        master = true;
                        foreach (bool flag in GMflags)
                        {
                            if (flag == false)
                            {
                                master = false;
                                break;
                            }
                        }
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
    }
}
