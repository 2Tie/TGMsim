using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TGMsim.Modes
{
    class Easy : Mode
    {
        int strCombo = 0;
        int strBravo = 0;
        int strComboF = 0;
        int strBravoF = 0;
        int strCreditsF = 0;
        int amtBase = 0;
        int interval30s = 0;
        int intervalLevel = 0;
        bool giveInterval = false;
        int graceF = 0;
        int comboSpeed = 0;
        List<float> lineTable = new List<float> { 1.0f, 2.9f, 3.8f, 4.7f };
        List<float> comboTable = new List<float> { 1f, 1f, 1.5f, 1.9f, 2.2f, 2.9f, 3.5f, 3.9f, 4.2f, 4,5f };

        int hanabi = 0;

        public Easy()
        {
            ModeName = "EASY";
            sections.Add(100);
            sections.Add(200);
            border = Color.LimeGreen;
            delayTable.Add(new List<int> { 27 });
            delayTable.Add(new List<int> { 27 });
            delayTable.Add(new List<int> { 14 });
            delayTable.Add(new List<int> { 30 });
            delayTable.Add(new List<int> { 40 });
            endLevel = 200;
            lockSafety = true;
        }

        public override void onSpawn()
        {
            if (firstPiece)
                firstPiece = false;
            else if (level < endLevel)
            {
                if (level < sections[curSection] - 1)
                    level += 1;
            }
            if (comboSpeed == 0)
                comboSpeed++;
        }

        public override void onTick(long time)
        {
            //increment the framecounters
            if (strCombo > 0)
                strComboF++;
            if (strComboF == 10)
                fireHanabi(0);

            if (strBravo > 0)
                strBravoF++;
            if (strBravoF == 10)
                fireHanabi(1);

            if (inCredits)
            {
                strCreditsF++;
                if (strCreditsF >= 3265 / amtBase)
                    fireHanabi(2);
            }
            if (graceF > 0)
                graceF -= 1;

            if (comboSpeed > 0)
                comboSpeed++;

            interval30s++;
            if(interval30s >= 60*30)//30 seconds
            {
                if (level >= intervalLevel + 30)
                    giveInterval = true;
                interval30s = 0;
                intervalLevel = level;
            }
        }

        public override void onPut(Tetromino tet, bool clear)
        {
            if (!clear && !inCredits)
                combo = 0;
        }

        public override void onClear(int lines, Tetromino tet, long time, bool bravo, bool split)
        {
            if (!inCredits)
            {
                level += lines;
                if (lines > 2)
                {
                    level++;
                    combo++;
                    if (combo > 9)
                        combo = 9;
                }
                if (lines > 3)
                    level++;
            }
            int comboVal = combo;
            if(inCredits)
            {
                comboVal = 1;
                if (combo < 9) comboVal += 4; //due to an error, 9 loops around to entry 1
                if (combo < 4) comboVal--;
                if (combo < 3) comboVal--;
                if (combo < 2) comboVal--;

            }
            if (bravo && !inCredits)
                strBravo += 6;
            bool lucky = false;
            if (new List<int> { 25, 50, 75, 125, 150, 175, 199 }.Contains(level - lines))
                lucky = true;
            int finesse = level - tet.life;
            strCombo += (int)(lineTable[lines-1]*comboTable[comboVal]*(graceF>0?1.3f:1f)*(split?1.4f:1)*(giveInterval?1.4f:1)*(lucky?1.3f:1)*(finesse>120?finesse/120f:1)*(level - comboSpeed>100?comboSpeed/100:1)*(tet.spun?(tet.id==Tetromino.Piece.T?(lines==3?4:3):2):1));

            if(curSection < sections.Count - 1)
            {
                if (level >= sections[curSection])
                    curSection++;
            }

            if (level >= endLevel)
            {
                level = endLevel;
                inCredits = true;
                g20 = true;
                amtBase = hanabi;
            }

            comboSpeed = 0;//reset here so hanabi calc is unaffected
        }

        public override void onGameOver()
        {
            int extraFrames = 0;
            if (creditsClear)
            {
                hanabi += 24;//for completing
                hanabi += strCombo;//automatically added
                orangeLine = true;
            }
            else //if topped out
            {
                extraFrames = 8 * 26;//this is how long it takes to fade out the field if topped out

                extraFrames += 120; //hanabi count length, can still earn more during this

                hanabi += Math.Min(strCombo, extraFrames / 10);
                hanabi += Math.Min(strBravo, extraFrames / 10);
            }

            score = hanabi;
            
        }

        private void fireHanabi(int type) //combo, bravo, credits
        {
            if (type == 0) //combo
            {
                strCombo -= 1;
                strComboF = 0;
            }else if (type == 1) //bravo
            {
                strBravo -= 1;
                strBravoF = 0;
            }else if (type == 2)
            {
                strCreditsF = 0;
            }
            hanabi++;
            graceF = 100;
        }

        public override void draw(Graphics drawBuffer, Font f, SolidBrush textBrush, bool replay)
        {
            drawBuffer.DrawString(combo.ToString(), f, textBrush, 10, 60);
            drawBuffer.DrawString(hanabi.ToString(), f, textBrush, 10, 68);
        }
    }
}
