using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace TGMsim.Modes
{
    class Dynamo : Mode
    {
        public int[] gradeTable = new int[17] { 20, 45, 75, 110, 150, 195, 245, 300, 360, 425, 495, 570, 650, 735, 825, 920, 1020 };
        public int gradePoints = 0;
        public double gradeCombo = 1;
        public int creditPoints = 0;

        int bonusGradeProgress = 0;
        int bonusGrades = 0;

        public FrameTimer secTimer = new FrameTimer();
        public List<long> secTimes = new List<long>();
        public FrameTimer coolTime = new FrameTimer();
        public int cools = 0;
        public long t = 0;

        public List<int> coolCounter = new List<int>();

        public Dynamo(int v)
        {
            ModeName = "DYNAMO";
            border = Color.MediumPurple;
            grades = new List<string> {
                "9", "8", "7", "6", "5", "4", "3", "2", "1",
                "S1", "S2", "S3", "S4", "S5", "S6", "S7", "S8", "S9", //the point grades
                "M1", "M2", "M3", "M4", "M5", "M6", "M7", "M8", "M9", "MM", //the COOL grades (one for each section)
                "V1", "V2", "V3", "V4", "V5", "V6", "V7", "V8", "V9", //the variant grades + clear grade
                "G1", "G2", "G3", "G4", "G5", "GM" }; //credit grades?
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
            showGrade = true;
            variant = v;
            if (v < 4)
                secBonus = v * 10;
            //if v == 4, final mode
            if (v == 0)
            {
                creditsType = CreditsTypes.plain;
                creditsSong = "crdtcas";
            }
            else
            {
                creditsType = CreditsTypes.vanishing;
                creditsSong = "crdtvanish";
            }
            //ADD SHIRASE
            delayTable.Add(new List<int> { 27, 27, 27, 18, 14, 14, 12, 12, 12, 8,  7,  6,  6,  6,  6,  6,  6,  6,  6,  6,  6,  6 });//ARE
            delayTable.Add(new List<int> { 27, 27, 18, 14, 8,  8,  8,  7,  6,  6,  6,  6,  6,  5,  5,  5,  5,  5,  5,  5,  5,  5 });//LINE ARE
            delayTable.Add(new List<int> { 14, 8,  8,  8,  8,  6,  6,  6,  6,  6,  6,  6,  6,  4,  4,  4,  4,  4,  4,  4,  4,  4 });//DAS
            delayTable.Add(new List<int> { 30, 30, 30, 30, 30, 17, 17, 17, 17, 17, 15, 15, 15, 13, 12, 12, 12, 12, 12, 12, 10, 8 });//LOCK
            delayTable.Add(new List<int> { 40, 25, 16, 12, 6,  6,  6,  5,  5,  5,  5,  5,  4,  3,  3,  3,  3,  3,  3,  3,  3,  3 });//LINE CLEAR

            hasSecretGrade = false;
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
                //tetCount++;
            }

        }

        public override void onPut(Tetromino tet, bool clear)
        {
            if (clear == false)
            {
                if (gradeCombo > 1.0)
                    gradeCombo -= 0.1;
            }

            if(variant == 4)
            {
                //TODO: super curve, cools don't affect it
                //update delays
                int num = 999 / (delayTable[0].Count + 1);
                baseARE = delayTable[0][level/num];
                baseARELine = delayTable[1][level/num];
                baseDAS = delayTable[2][level/num];
                baseLock = delayTable[3][level/num];
                baseLineClear = delayTable[4][level/num];
            }

            //COOLS
            if (level % 100 > 69)
            {
                if (coolCounter.Count <= curSection)
                {
                    int framebonus = 24 - curSection; //15 was base
                    long cool = (baseARE + framebonus) * 60 + (baseLineClear) * 6;//cool time in frames //TODO: this only works for the non-super variants
                    //long cool = (baseARE + 60) * 60 + (baseLineClear) * 6;//testing adjustment because i'm a scrub
                    cool = (cool / 60) * 1000;//cool time in ms

                    if (secTimer.count < cool)
                    {
                        coolCounter.Add(1);
                        Audio.playSound(Audio.s_Cool);
                        coolTime.tick();
                    }
                    else
                        coolCounter.Add(0);
                }
            }

            //bonus grades
            if(variant > 0)
            {
                int l = 999 / (variant * 2 + 1); //each variant has two more potential bonus grades, i.e. variant 4 has 8 extra grades
                if(level > l*bonusGradeProgress)
                {
                    bonusGradeProgress++;
                    bonusGrades++;
                }
            }
        }

        public override void onClear(int lines, Tetromino tet, long time, bool bravo)
        {
            level += lines;

            if (lines == 4)
            {
                Audio.playSound(Audio.s_Tetris);
            }

            //int newPts = (int)(Math.Ceiling(baseGradePts[lines - 1][grade] * comboTable[lines - 1][gradeCombo]) * Math.Ceiling((double)level / 250));

            //todo: new system. lines cleared squared. multiplier set to 1.3, with nonclearing locks dropping it to 1.2 then by 0.5 each. 1000 grade points the target for S9?
            if (!inCredits)
            {
                gradePoints += (int)(Math.Pow(lines, 2) * gradeCombo);
                gradeCombo = 1.5;
                //update grade
                if (grade < gradeTable.Count() - 1 && gradePoints > gradeTable[grade])
                {
                    grade++;
                    masteringTime.count = time;
                }

                if (curSection < sections.Count)
                {
                    if (level >= sections[curSection])
                    {
                        curSection++;
                        showGhost = false;
                        secTet.Add(0);
                        secTimes.Add(secTimer.count);
                        secTimer = new FrameTimer();
                        //GM FLAGS

                        //MUSIC
                        updateMusic();
                        //DELAYS
                        if (coolCounter.Count(p => p == 1) > cools)
                        {
                            cools++;
                            secBonus += 1;

                            int num = (curSection + secBonus) / 2 - 4;
                            if (num > 0 && variant < 4)
                            {
                                //update delays
                                baseARE = delayTable[0][num];
                                baseARELine = delayTable[1][num];
                                baseDAS = delayTable[2][num];
                                baseLock = delayTable[3][num];
                                baseLineClear = delayTable[4][num];
                            }
                        }
                    }
                }
            }
            else
            {
                creditPoints += lines * lines; 
            }
            if (level >= endLevel && endLevel != 0)
            {
                level = endLevel;
                inCredits = true;
                if(cools == 10)
                {
                    creditsType = CreditsTypes.invisible;
                    creditsSong = "crdtinvis";
                }
            }
        }

        public override void onGameOver()
        {
            if (creditsClear)
            {
                bonusGrades++; //one (1) bonus grade for clearing the credits
                orangeLine = true;
            }

            if (creditPoints > 16*6)
                creditPoints = 16*6;

            if (creditsType ==  CreditsTypes.vanishing)
                creditPoints /= 2;

            if (creditsType != CreditsTypes.plain)
                bonusGrades += creditPoints/16;

            grade += bonusGrades;
        }

        public override void draw(bool replay)
        {
            for (int i = 0; i < variant; i++)
            {
                Draw.buffer.DrawString("+", Draw.f_Maestro, Draw.tb, 20 + 10 * i, 336);
            }
            for (int i = 0; i < cools; i++)
            {
                Draw.buffer.DrawString("-", Draw.f_Maestro, Draw.tb, 20 + 10 * i, 348);
            }
            if (variant == 4)
                Draw.buffer.DrawString((level/(999 / (delayTable[0].Count + 1))).ToString(), Draw.f_Maestro, Draw.tb, 20, 324);
            Draw.buffer.DrawString(gradePoints.ToString(), Draw.f_Maestro, Draw.tb, 20, 312);
        }

        public override void updateMusic()
        {
            int section = curSection + cools + variant * 5;
            if (section >= 20)
            {
                if (Audio.song != "Level 5")
                {
                    Audio.stopMusic();
                    Audio.playMusic("Level 5");
                    return;
                }
            }
            else if (section >= 15)
            {
                if (Audio.song != "Level 4")
                {
                    Audio.stopMusic();
                    Audio.playMusic("Level 4");
                    return;
                }
            }
            else if (section >= 10)
            {
                if (Audio.song != "Level 3")
                {
                    Audio.stopMusic();
                    Audio.playMusic("Level 3");
                    return;
                }
            }
            else if (section >= 5)
            {
                if (Audio.song != "Level 2")
                {
                    Audio.stopMusic();
                    Audio.playMusic("Level 2");
                    return;
                }
            }
            else if (section == 0 && Audio.song != "Level 1")
            {
                Audio.stopMusic();
                Audio.playMusic("Level 1");
                return;
            }
        }
    }
}
