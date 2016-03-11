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
        public int baseARELine = 30;
        public int baseDAS = 14;
        public int baseLock = 30;
        public int baseLineClear = 41;
        public int gravType = 0; //b256, b65536
        public int baseGrav = 4;

        public int creditsLength = 2968;

        public int fieldW = 10;
        public int fieldH = 20;

        public double FPS = 60.00;

        public bool showGrade = true;

        public List<List<double>> comboTable = new List<List<double>>();

        public List<int> decayRate = new List<int>() { 125, 80, 80, 50, 45, 45, 45, 40, 40, 40, 40, 40, 30, 30, 30, 20, 20, 20, 20, 20, 15, 15, 15, 15, 15, 15, 15, 15, 15, 15, 10, 10};
        public List<List<int>> baseGradePts = new List<List<int>>();

        public List<int> gravTableTGM1 = new List<int> {4, 6, 8, 10, 12, 16, 32, 48, 64, 80, 96, 112, 128, 144, 4, 32, 64, 96, 128, 160, 192, 224, 256, 512, 768, 1024, 1280, 1024, 768, 5120};
        public List<int> gravLevelsTGM1 = new List<int> {0, 30, 35, 40, 50, 60, 70, 80, 90, 100, 120, 140, 160, 170, 200, 220, 230, 233, 236, 239, 243, 247, 251, 300, 330, 360, 400, 420, 450, 500};

        public List<int> gravTableTGM3 = new List<int> { 1024, 1536, 2048, 2560, 3072, 4096, 8192, 12288, 16384, 20480, 24576, 28672, 32768, 36864, 1024, 8192, 16348, 24576, 32768, 40960, 49152, 57344, 65536, 131072, 196608, 262144, 327680, 262144, 196608, 1310720 };

        public List<List<int>> delayTableTGM2 = new List<List<int>>();
        public List<List<int>> delayTableTAP = new List<List<int>>();
        public List<List<int>> delayTableDeath = new List<List<int>>();
        public List<List<int>> delayTableTGM3 = new List<List<int>>();
        public List<List<int>> delayTableShirase = new List<List<int>>();

        public List<int> gradePointsTGM1 = new List<int> { 0, 400, 800, 1400, 2000, 3500, 5500, 8000, 12000, 16000, 22000, 30000, 40000, 52000, 66000, 82000, 100000, 120000};

        public List<string> gradesTGM1 = new List<string> { "9", "8", "7", "6", "5", "4", "3", "2", "1", "S1", "S2", "S3", "S4", "S5", "S6", "S7", "S8", "S9"};
        public List<string> gradesTGM3 = new List<string> { "9", "8", "7", "6", "5", "4", "3", "2", "1", "S1", "S2", "S3","S4", "S5", "S6", "S7", "S8", "S9", "M1", "M2", "M3", "M4", "M5", "M6", "M7", "M8", "M9", "M", "MK", "MV", "MO", "MM" };

        public List<int> gradeIntTGM2 = new List<int> { 0, 1, 2, 3, 4, 5, 5, 6, 6, 7, 7, 7, 8, 8, 8, 9, 9, 9, 10, 11, 12, 12, 12, 13, 13, 14, 14, 15, 15, 16, 16, 17 };

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

            delayTableTGM2.Add(new List<int> {25, 25, 25, 16, 12, 12});//ARE
            delayTableTGM2.Add(new List<int> {16,10,10,10,10,8});//DAS
            delayTableTGM2.Add(new List<int> {30,30,30,30,30,17});//LOCK
            delayTableTGM2.Add(new List<int> {40,25,16,12,6,6,6});//LINE CLEAR

            delayTableTAP.Add(new List<int> { 25, 25, 25, 16, 12, 12 });
            delayTableTAP.Add(new List<int> { 25, 25, 16, 12, 6, 6 });//line ARE
            delayTableTAP.Add(new List<int> { 14, 8, 8, 8, 8, 6 });
            delayTableTAP.Add(new List<int> { 30, 30, 30, 30, 30, 17 });
            delayTableTAP.Add(new List<int> { 40, 25, 16, 12, 6, 6 });

            //death
            delayTableDeath.Add(new List<int> { 16, 12, 12, 6, 5, 4 });
            delayTableDeath.Add(new List<int> { 12, 6, 6, 6, 5, 4 });
            delayTableDeath.Add(new List<int> { 10, 10, 9, 8, 6, 6 });
            delayTableDeath.Add(new List<int> { 30, 26, 22, 18, 15, 15 });
            delayTableDeath.Add(new List<int> { 12, 6, 6, 6, 5, 4 });

            //tgm3
            delayTableTGM3.Add(new List<int> { 25, 25, 25, 16, 12, 12, 6, 5, 4 });
            delayTableTGM3.Add(new List<int> { 25, 25, 16, 12, 6, 6, 6, 5, 4 });
            delayTableTGM3.Add(new List<int> { 14, 8, 8, 8, 8, 6, 6, 6, 6 });
            delayTableTGM3.Add(new List<int> { 30, 30, 30, 30, 30, 17, 17, 15, 15 });
            delayTableTGM3.Add(new List<int> { 40, 25, 16, 12, 6, 6, 6, 6, 6 });

            //shirase
            delayTableShirase.Add(new List<int> { 10, 10, 10, 4, 4, 4, 4, 4, 4 });
            delayTableShirase.Add(new List<int> { 6, 5, 4, 4, 3, 3, 3, 3, 4 });
            delayTableShirase.Add(new List<int> { 8, 6, 6, 6, 4, 4, 4, 4, 4 });
            delayTableShirase.Add(new List<int> { 18, 18, 17, 15, 13, 12, 10, 8, 15 });
            delayTableShirase.Add(new List<int> { 6, 5, 4, 4, 3, 3, 3, 3, 6 });

        }

        public void setGame(int game)
        {
            gameRules = game;
            switch (game)
            {
                case 1: //TGM
                    FPS = 59.84;
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
                    FPS = 61.68;
                    nextNum = 1;
                    hold = false;
                    hardDrop = 1;
                    rotation = 0;
                    baseARE = 25;
                    baseARELine = 25;
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
                    FPS = 61.68;
                    nextNum = 1;
                    hold = false;
                    hardDrop = 1;
                    rotation = 0;
                    baseARE = 25;
                    baseARELine = 25;
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
                    FPS = 59.94;
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
                case 5:
                    FPS = 59.94;
                    nextNum = 4;
                    hold = true;
                    hardDrop = 1;
                    rotation = 1;
                    //recycled from TGM3 until more is known
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
