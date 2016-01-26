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
        public int hardDrop = 0; //no, sonic, firm

        public int genAttps = 4;

        public int rotation = 0; //TGM, TGM3, SRS

        public int baseARE = 30;
        public int baseDAS = 14;
        public int baseLock = 30;
        public int baseLineClear = 41;
        public int gravType = 0; //b256, b65536
        public int baseGrav = 4;

        public int creditsLength = 2968;

        public int fieldW = 10;
        public int fieldH = 20;

        public bool showGrade = true;

        public List<List<double>> comboTable = new List<List<double>>();

        public List<int> decayRate = new List<int>() { 125, 80, 80, 50, 45, 45, 45, 40, 40, 40, 40, 40, 30, 30, 30, 20, 20, 20, 20, 20, 15, 15, 15, 15, 15, 15, 15, 15, 15, 15, 10, 10};
        public List<List<int>> baseGradePts = new List<List<int>>();

        public List<int> gravTableTGM1 = new List<int> {4, 6, 8, 10, 12, 16, 32, 48, 64, 80, 96, 112, 128, 144, 4, 32, 64, 96, 128, 160, 192, 224, 256, 512, 768, 1024, 1280, 1024, 768, 5120};
        //public List<int> gravTableTGM1 = new List<int> { 128, 128, 128, 128, 128, 128, 128, 128, 128, 128, 128, 128, 128, 128, 128, 128, 128, 128, 128, 160, 192, 224, 256, 512, 768, 1024, 1280, 1024, 768, 5120 };

        public List<int> gravLevelsTGM1 = new List<int> {0, 30, 35, 40, 50, 60, 70, 80, 90, 100, 120, 140, 160, 170, 200, 220, 230, 233, 236, 239, 243, 247, 251, 300, 330, 360, 400, 420, 450, 500};

        public List<int> gradePointsTGM1 = new List<int> { 0, 400, 800, 1400, 2000, 3500, 5500, 8000, 12000, 16000, 22000, 30000, 40000, 52000, 66000, 82000, 100000, 120000};

        public List<string> gradesTGM1 = new List<string> { "9", "8", "7", "6", "5", "4", "3", "2", "1", "S1", "S2", "S3", "S4", "S5", "S6", "S7", "S8", "S9"};
        public List<string> gradesTGM2 = new List<string> { "9", "8", "7", "6", "5", "4", "4", "3", "3", "2", "2", "2", "1", "1", "1", "S1", "S1", "S1", "S2", "S3", "S4", "S4", "S4", "S5", "S5", "S6", "S6", "S7", "S7", "S8", "S8", "S9" };

        public Rules()
        {
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
        }

        public void setGame(int game)
        {
            gameRules = game;
            switch (game)
            {
                case 1: //TGM
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
                    genAttps = 4;
                    fieldW = 10;
                    fieldH = 20;
                    creditsLength = 2968; //taken from nullpomino, though estimates place it around 2961. still need to verify
                    showGrade = true;
                    break;
                case 2: //TGM2 OR TAP
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
                    genAttps = 6;
                    fieldW = 10;
                    fieldH = 20;
                    showGrade = true;
                    break;
                case 3: //TGM2 OR TAP
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
                    genAttps = 6;
                    fieldW = 10;
                    fieldH = 20;
                    showGrade = true;
                    break;
                case 4: //TI
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
                    genAttps = 6;
                    fieldW = 10;
                    fieldH = 20;
                    showGrade = false;
                    break;
            }

        }
    }
}
