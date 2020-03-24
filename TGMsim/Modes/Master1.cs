using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace TGMsim.Modes
{
    class Master1 : Mode
    {

        public long t = 0;
        public List<bool> GMflags = new List<bool>();
        List<int> gradePoints = new List<int> { 0, 400, 800, 1400, 2000, 3500, 5500, 8000, 12000, 16000, 22000, 30000, 40000, 52000, 66000, 82000, 100000, 120000 };

        public Master1()
        {
            ModeName = "MASTER 1";
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
            delayTable.Add(new List<int> { 30 });//ARE
            delayTable.Add(new List<int> { 27 });//line ARE
            delayTable.Add(new List<int> { 16 });//DAS
            delayTable.Add(new List<int> { 30 });//lock
            delayTable.Add(new List<int> { 44 });//line clear
            secTet.Add(0);
            hasCreditsPause = false;
            speedUpCredits = true;
            secretOnlyOnTopOut = true;
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
            }
        }

        public override void onPut(Tetromino tet, bool clear)
        {
            combo = 1;
        }

        public override void onClear(int lines, Tetromino tet, long time, bool bravo)
        {
            int oldLvl = level;
            level += lines;

            if (level >= endLevel && endLevel != 0)
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
                b = 4;

            //score handling

            combo = combo + (2 * lines) - 2;

            if(!inCredits)
            score += ((int)Math.Ceiling((double)(level) / 4) + tet.soft) * lines * combo * b;

            //update grade
            bool checking = true;
            while (checking == true)
            {

                if (grade < gradePoints.Count - 1)
                {
                    if (score >= gradePoints[grade + 1])
                    {
                        grade++;
                        Audio.playSound(Audio.s_Grade);
                        masteringTime.count = time;
                    }
                    else
                        checking = false;
                }
                else
                    checking = false;

            }
            //section handling

            if (curSection < sections.Count())
            {
                if (level >= sections[curSection])
                {
                    curSection++;
                    showGhost = false;
                    secTet.Add(0);
                    //GM FLAGS
                    if (GMflags.Count == 0 && level >= 300)
                    {
                        if (score >= 12000 && t <= 255000)
                            GMflags.Add(true);
                        else
                            GMflags.Add(false);
                    }
                    else if (GMflags.Count == 1 && level >= 500)
                    {
                        if (score >= 40000 && t <= 450000)
                            GMflags.Add(true);
                        else
                            GMflags.Add(false);
                    }
                    else if (GMflags.Count == 2 && level >= endLevel)
                    {
                        level = 999;
                        if (score >= 126000 && t <= 810000)
                            GMflags.Add(true);
                        else
                            GMflags.Add(false);


                        //check for awarding GM
                        if (GMflags[0] && GMflags[1] && GMflags[2])
                        {
                            grade = grades.Count - 1;
                        }
                    }
                    //MUSIC
                    updateMusic();
                    //DELAYS
                    //BACKGROUND
                }
            }
        }

        public override void updateMusic()
        {
            if (curSection == 5 && Audio.song != "Level 2")
            {
                Audio.stopMusic();
                Audio.playMusic("Level 2");
                return;
            }
            if (curSection == 0 && Audio.song != "Level 1")
            {
                Audio.stopMusic();
                Audio.playMusic("Level 1");
                return;
            }
        }

        public override void draw(Graphics drawBuffer, Font f_Maestro, SolidBrush b, bool replay)
        {
            //drawBuffer.DrawString(curSection.ToString(), f_Maestro, new SolidBrush(Color.White), 40, 80);
            drawBuffer.DrawString("NEXT GRADE:", f_Maestro, b, 480, 140);
            if (grade != 18)
                drawBuffer.DrawString(gradePoints[grade + 1].ToString(), f_Maestro, b, 480, 160);
            else
                drawBuffer.DrawString("??????", f_Maestro, b, 480, 160);
        }
    }
}
