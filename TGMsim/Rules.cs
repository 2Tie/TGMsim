using System;
using System.Collections.Generic;
using System.Text;

namespace TGMsim
{
    class Rules
    {
        public int gameRules = 1;
        public int nextNum = 1;
        public bool hold = false;
        public int hardDrop = 0; //no, sonic, hard

        public int rotation = 0; //TGM, TGM3, SRS

        public int baseARE = 30;
        public int baseDAS = 14;
        public int baseLock = 30;
        public int baseLineClear = 41;
        public int gravType = 0; //b256, b65536
        public int baseGrav = 4;

        public int fieldW = 10;
        public int fieldH = 20;

        public bool showGrade = true;

        public List<int> gravTableTGM1 = new List<int> {4, 6, 8, 10, 12, 16, 32, 48, 64, 80, 96, 112, 128, 144, 4, 32, 64, 96, 128, 160, 192, 224, 256, 512, 768, 1024, 1280, 1024, 768, 5120};

        public List<int> gravLevelsTGM1 = new List<int> {0, 30, 35, 40, 50, 60, 70, 80, 90, 100, 120, 140, 160, 170, 200, 220, 230, 233, 236, 239, 243, 247, 251, 300, 330, 360, 400, 420, 450, 500};


        void SetGame(int game)
        {
            switch (game)
            {
                case 1:
                    nextNum = 1;
                    hold = false;
                    hardDrop = 0;
                    rotation = 0;
                    baseARE = 30;
                    baseDAS = 14;
                    baseLock = 30;
                    baseLineClear = 41;
                    gravType = 0;
                    baseGrav = 4;
                    fieldW = 10;
                    fieldH = 20;
                    showGrade = true;
                    break;
                case 2:
                    nextNum = 1;
                    hold = false;
                    hardDrop = 1;
                    rotation = 0;
                    baseARE = 25;
                    baseDAS = 14;
                    baseLock = 30;
                    baseLineClear = 40;
                    gravType = 0;
                    baseGrav = 4;
                    fieldW = 10;
                    fieldH = 20;
                    showGrade = true;
                    break;
                case 3:
                    nextNum = 3;
                    hold = true;
                    hardDrop = 1;
                    rotation = 1;
                    baseARE = 25;
                    baseDAS = 14;
                    baseLock = 30;
                    baseLineClear = 40;
                    gravType = 1;
                    baseGrav = 1024;
                    fieldW = 10;
                    fieldH = 20;
                    showGrade = false;
                    break;
            }

        }
    }
}
