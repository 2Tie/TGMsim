using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TGMsim.Modes
{
    class Death : Mode
    {
        public int gradeCombo = 0;
        public int pipCount = 0;
        public int tetCount = 0;
        public int rotCount = 0;
        public int bravos = 0;
        public bool recovering = false;
        public int recoveries = 0;
        public long t = 0;

        public Death()
        {
            ModeName = "DEATH";
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
            border = Color.DarkRed;
            g20 = true;
            showGrade = true;
            showGhost = false;
            delayTable.Add(new List<int> { 16, 12, 12, 6, 5, 4 });
            delayTable.Add(new List<int> { 12, 6, 6, 6, 5, 4 });
            delayTable.Add(new List<int> { 10, 10, 9, 8, 6, 6 });
            delayTable.Add(new List<int> { 30, 26, 22, 18, 15, 15 });
            delayTable.Add(new List<int> { 12, 6, 6, 6, 5, 4 });
            secTet.Add(0);
            hasCreditsPause = false;
        }

        public override void onTick(long time)
        {
            t = time;
        }

        public override void onSpawn()
        {
            if (firstPiece)
            {
                firstPiece = false;
            }
            else if (level < endLevel)
            {
                if (level < sections[curSection] - 1 && !torikan)
                    level += 1;
                tetCount++;
            }
        }

        public override void onGameOver()
        {
            if (creditsClear)
                orangeLine = true;
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

        public override void onClear(int lines, Tetromino tet, long time, bool bravo)
        {
            pipCount -= 10 * lines;

            int oldLvl = level;
            level += lines;

            if (level >= endLevel)
            {
                level = endLevel;
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
                    //GM FLAGS
                    if (level >= 999)
                        grade = 19;
                    //TORIKAN
                    if (curSection == 5)
                    {
                        if (t > 205000)
                        {
                            toriLevel = 500;
                            torikan = true;
                            torDef = t - 205000;
                            //triggerCredits();
                        }
                        else
                            grade = 18;
                    }
                    //MUSIC
                    updateMusic();
                    //DELAYS
                    int num = curSection;
                    if (num < 6)
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

            if (torikan && level > toriLevel)
                level = toriLevel;
        }

        public override void updateMusic()
        {
            if (curSection == 0)
            {
                Audio.stopMusic();
                Audio.playMusic("Level 2");
                return;
            }
            if (curSection == 3)
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
        }

        void checkAudioFadeout()
        {
            if (curSection == 2 && level % 100 > 84) Audio.stopMusic();
            if (curSection == 4 && level % 100 > 84) Audio.stopMusic();
        }
    }
}
