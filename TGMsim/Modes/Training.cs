using System.Collections.Generic;
using System.Drawing;

namespace TGMsim.Modes
{
    class Training : Mode
    {
        public Training()
        {
            ModeName = "20G TRAINING";
            border = Color.DarkGreen;
            endLevel = 200;
            g20 = true;
            showGrade = false;
            showGhost = false;
            creditsSong = "crdtcas";
            sections.Add(200);
            delayTable.Add(new List<int> { 27 });
            delayTable.Add(new List<int> { 27 });
            delayTable.Add(new List<int> { 14 });
            delayTable.Add(new List<int> { 30 });
            delayTable.Add(new List<int> { 40 });
            hasSecretGrade = true;
        }

        public override void onSpawn()
        {
            if (firstPiece)
            {
                firstPiece = false;
            }
            else if (level < endLevel-1)
                level += 1;
        }

        public override void onClear(int lines, Tetromino tet, long time, bool bravo)
        {
            level += lines;
            if (level >= endLevel && endLevel != 0)
            {
                level = endLevel;
                inCredits = true;
            }
        }

        public override void updateMusic()
        {
            if (Audio.song != "Casual 2")
                Audio.playMusic("Casual 2");
        }
    }
}
