using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TGMsim
{
    class M_Master1 : Mode
    {

        public long t = 0;
        public List<bool> GMflags = new List<bool>();

        public M_Master1()
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
            delayTable.Add(new List<int> { 30 });
            delayTable.Add(new List<int> { 30 });
            delayTable.Add(new List<int> { 16 });
            delayTable.Add(new List<int> { 30 });
            delayTable.Add(new List<int> { 41 });
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

            score += ((int)Math.Ceiling((double)(level) / 4) + tet.soft) * lines * combo * b;

            //update grade
            bool checking = true;
            while (checking == true)
            {

                if (grade < gradePointsTGM1.Count - 1)
                {
                    if (score >= gradePointsTGM1[grade + 1])
                    {
                        grade++;
                        Audio.playSound(Audio.s_Grade);
                        //masterTime = timer.count;
                    }
                    else
                        checking = false;
                }
                else
                    checking = false;

            }
            //section handling

            if (level >= sections[curSection])
            {
                curSection++;
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
                        grade = 32;
                    }
                }
                //MUSIC
                updateMusic();
                //DELAYS
                //BACKGROUND
                if (level > endLevel && endLevel != 0)
                    level = endLevel;
            }
        }

        public override void updateMusic()
        {
            if (curSection >= 5 && Audio.song != "Level 2")
            {
                Audio.stopMusic();
                Audio.playMusic("Level 2");
                return;
            }
            if (curSection >= 0 && Audio.song != "Level 1")
            {
                Audio.stopMusic();
                Audio.playMusic("Level 1");
                return;
            }
        }
    }
}
