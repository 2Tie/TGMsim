using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TGMsim
{
    class M_Dynamo : Mode
    {

        public List<List<double>> comboTable = new List<List<double>>();
        public List<int> gradeIntTGM2 = new List<int> { 0, 1, 2, 3, 4, 5, 5, 6, 6, 7, 7, 7, 8, 8, 8, 9, 9, 9, 10, 11, 12, 12, 12, 13, 13, 14, 14, 15, 15, 16, 16, 17 };
        public List<int> decayRate = new List<int>() { 125, 80, 80, 50, 45, 45, 45, 40, 40, 40, 40, 40, 30, 30, 30, 20, 20, 20, 20, 20, 15, 15, 15, 15, 15, 15, 15, 15, 15, 15, 10, 10 };
        public List<List<int>> baseGradePts = new List<List<int>>();
        public int gradePoints = 0;
        public int gradeCombo = 0;
        public int gradeTime = 0;
        public int intGrade = 0;

        public FrameTimer secTimer = new FrameTimer();
        public List<long> secTimes = new List<long>();
        public FrameTimer coolTime = new FrameTimer();
        public int cools = 0;
        public long t = 0;

        public List<int> coolCounter = new List<int>();

        public M_Dynamo(int v)
        {
            ModeName = "DYNAMO";
            border = Color.MediumPurple;
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
            showGrade = false;
            variant = v;
            if (v < 4)
                lvlBonus = v * 10;
            //if v == 4, final mode
            
            //ADD SHIRASE
            delayTable.Add(new List<int> { 27, 27, 27, 18, 14, 14, 12, 12, 12, 8,  7,  6,  6,  6,  6,  6,  6,  6,  6,  6,  6,  6 });//ARE
            delayTable.Add(new List<int> { 27, 27, 18, 14, 8,  8,  8,  7,  6,  6,  6,  6,  6,  5,  5,  5,  5,  5,  5,  5,  5,  5 });//LINE ARE
            delayTable.Add(new List<int> { 14, 8,  8,  8,  8,  6,  6,  6,  6,  6,  6,  6,  6,  4,  4,  4,  4,  4,  4,  4,  4,  4 });//DAS
            delayTable.Add(new List<int> { 30, 30, 30, 30, 30, 17, 17, 17, 17, 17, 15, 15, 15, 13, 12, 12, 12, 12, 12, 12, 10, 8 });//LOCK
            delayTable.Add(new List<int> { 40, 25, 16, 12, 6,  6,  6,  5,  5,  5,  5,  5,  4,  3,  3,  3,  3,  3,  3,  3,  3,  3 });//LINE CLEAR

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
        }

        public override void onTick(long time)
        {
            t = time;
            secTimer.tick();
            if (!comboing && !inCredits)
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
                combo = 1;
                gradeCombo = 0;
                comboing = false;
            }

            if(variant == 4)
            {
                //TODO: super curve, cools don't affect it
                //update delays

            }

            //COOLS
            if (level % 100 > 69)
            {
                if (coolCounter.Count <= curSection && curSection != 9)
                {
                    int framebonus = 24 - curSection; //15 was base
                    long cool = (baseARE + framebonus) * 60 + (baseLineClear) * 6;//cool time in frames
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
        }

        public override void onClear(int lines, Tetromino tet, long time, bool bravo)
        {
            level += lines;

            if (lines == 4)
                Audio.playSound(Audio.s_Tetris);

            int newPts = (int)(Math.Ceiling(baseGradePts[lines - 1][grade] * comboTable[lines - 1][gradeCombo]) * Math.Ceiling((double)level / 250));

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
                    }
                }
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
                        lvlBonus += 1;

                        int num = (curSection + lvlBonus) / 2 - 4;
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
            if (level >= endLevel && endLevel != 0)
            {
                level = endLevel;
                inCredits = true;
            }
        }

        public override void draw(Graphics drawBuffer, Font f, SolidBrush b, bool replay)
        {
            Brush tb = new SolidBrush(Color.White);
            for (int i = 0; i < variant; i++)
            {
                drawBuffer.DrawString("+", f, tb, 20 + 10 * i, 336);
            }
            for (int i = 0; i < cools; i++)
            {
                drawBuffer.DrawString("-", f, tb, 20 + 10 * i, 348);
            }
        }

        public override void updateMusic()
        {
            int section = curSection + cools + variant * 5;
            if (section >= 20 && Audio.song != "Level 5")
            {
                Audio.stopMusic();
                Audio.playMusic("Level 5");
                return;
            }
            else if (section >= 15 && Audio.song != "Level 4")
            {
                Audio.stopMusic();
                Audio.playMusic("Level 4");
                return;
            }
            else if (section >= 10 && Audio.song != "Level 3")
            {
                Audio.stopMusic();
                Audio.playMusic("Level 3");
                return;
            }
            else if (section >= 5 && Audio.song != "Level 2")
            {
                Audio.stopMusic();
                Audio.playMusic("Level 2");
                return;
            }
            else if (section >= 0 && Audio.song != "Level 1")
            {
                Audio.stopMusic();
                Audio.playMusic("Level 1");
                return;
            }
        }
    }
}
