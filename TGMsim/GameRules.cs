using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TGMsim
{
    class GameRules
    {
        public int gameRules = 1;
        public int nextNum = 1;
        public bool hold = false;
        public int hardDrop = 0; //no, sonic, firm

        public int genAttps = 4;

        public int rotation = 0; //TGM, TGM3, SRS
        public int generator = 0; //dummy, TGM1, TGM2, TGM3, ACE, 
        public enum Gens { dummy = 0, TGM1, TGM2, TGM3, ACE, SEGA};

        public int baseARE = 30;
        public int baseARELine = 30;
        public int baseDAS = 14;
        public int baseLock = 30;
        public int baseLineClear = 41;
        public int gravType = 0; //b256, b65536
        public int baseGrav = 4;

        public int lockType = 0; //step-reset, move-reset

        public int creditsLength = 2968;//3238??

        public int fieldW = 10;
        public int fieldH = 22;

        public double FPS = 60.00;
        public int lag = 0;

        public bool showGrade = true;
        public int initialGrade = 0;

        public string GameName = "error";
        public string ModeName = "error";


        public List<List<double>> comboTable = new List<List<double>>();

        public List<int> decayRate = new List<int>() { 125, 80, 80, 50, 45, 45, 45, 40, 40, 40, 40, 40, 30, 30, 30, 20, 20, 20, 20, 20, 15, 15, 15, 15, 15, 15, 15, 15, 15, 15, 10, 10 };
        public List<List<int>> baseGradePts = new List<List<int>>();

        public List<int> gravTable = new List<int>();//TODO: grav table set plz
        public List<int> gravLevels = new List<int>() { 0, 30, 35, 40, 50, 60, 70, 80, 90, 100, 120, 140, 160, 170, 200, 220, 230, 233, 236, 239, 243, 247, 251, 300, 330, 360, 400, 420, 450, 500 };
        public List<List<int>> delayTable = new List<List<int>>();

        public List<int> gradePointsTGM1 = new List<int> { 0, 400, 800, 1400, 2000, 3500, 5500, 8000, 12000, 16000, 22000, 30000, 40000, 52000, 66000, 82000, 100000, 120000 };

        public List<string> grades = new List<string> { "9", "8", "7", "6", "5", "4", "3", "2", "1", "S1", "S2", "S3", "S4", "S5", "S6", "S7", "S8", "S9", "M1", "M2", "M3", "M4", "M5", "M6", "M7", "M8", "M9", "M", "MK", "MV", "MO", "MM", "GM" };

        public List<int> gradeIntTGM2 = new List<int> { 0, 1, 2, 3, 4, 5, 5, 6, 6, 7, 7, 7, 8, 8, 8, 9, 9, 9, 10, 11, 12, 12, 12, 13, 13, 14, 14, 15, 15, 16, 16, 17 };

        public List<int> secCools = new List<int> { 52000, 52000, 49000, 45000, 45000, 42000, 42000, 38000, 38000 };
        public List<int> secRegrets = new List<int> { 90000, 75000, 75000, 68000, 60000, 60000, 50000, 50000, 50000, 50000 };

        public struct Gimmick
        {
            public int type; //fading, vanishing, garbage, bones, ice, big
            public int startLvl;
            public int endLvl;
            public int parameter;
        }

        public int id = 0;
        public int endLevel = 999;
        public List<int> sections = new List<int>();
        public bool bigmode = false;
        public bool g20 = false;
        public bool shiraseGrades = false;
        public int exam = -1;
        public int lvlBonus = 0;
        public int gradedBy = 0; //points, grade points, level, bravo, time
        public int limitType = 0; //none, line, level, time, bravo
        public int limit = 0;
        public bool mute = false;
        public List<Gimmick> gimList = new List<Gimmick>();
        public int bigMove = 1;

        public Color border = Color.LightGray;

        public GameRules()
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

        public void setup(int game, int mode)
        {
            gameRules = game;
            switch (game)
            {
                case 1: //TGM
                    GameName = "TGM";
                    FPS = 59.84;
                    nextNum = 1;
                    hold = false;
                    hardDrop = 0;
                    rotation = 0;
                    generator = (int)Gens.TGM1;
                    lag = 2;
                    lockType = 0;
                    bigMove = 1;
                    baseARE = 30;
                    baseDAS = 14;
                    baseLock = 30;
                    baseLineClear = 41;
                    gravType = 0;
                    baseGrav = 4;
                    genAttps = 4;
                    fieldW = 10;
                    fieldH = 22;
                    creditsLength = 2968; //taken from nullpomino, though estimates place it around 2961. still need to verify
                    showGrade = true;
                    gravTable = new List<int> { 4, 6, 8, 10, 12, 16, 32, 48, 64, 80, 96, 112, 128, 144, 4, 32, 64, 96, 128, 160, 192, 224, 256, 512, 768, 1024, 1280, 1024, 768, 5120 };
                    break;
                case 2: //TGM2 OR TAP
                    GameName = "TGM2";
                    FPS = 61.68;
                    nextNum = 1;
                    hold = false;
                    hardDrop = 1;
                    rotation = 0;
                    generator = (int)Gens.TGM2;
                    lag = 0;
                    lockType = 0;
                    bigMove = 2;
                    baseARE = 25;
                    baseARELine = 25;
                    baseDAS = 14;
                    baseLock = 30;
                    baseLineClear = 40;
                    gravType = 0;
                    baseGrav = 4;
                    genAttps = 6;
                    fieldW = 10;
                    fieldH = 22;
                    showGrade = true;
                    gravTable = new List<int> { 4, 6, 8, 10, 12, 16, 32, 48, 64, 80, 96, 112, 128, 144, 4, 32, 64, 96, 128, 160, 192, 224, 256, 512, 768, 1024, 1280, 1024, 768, 5120 };
                    break;
                case 3: //TGM2 OR TAP
                    GameName = "TAP";
                    FPS = 61.68;
                    nextNum = 1;
                    hold = false;
                    hardDrop = 1;
                    rotation = 0;
                    generator = (int)Gens.TGM2;
                    lag = 0;
                    lockType = 0;
                    bigMove = 1;
                    baseARE = 25;
                    baseARELine = 25;
                    baseDAS = 14;
                    baseLock = 30;
                    baseLineClear = 40;
                    gravType = 0;
                    baseGrav = 4;
                    genAttps = 6;
                    fieldW = 10;
                    fieldH = 22;
                    creditsLength = 3238;
                    showGrade = true;
                    gravTable = new List<int> { 4, 6, 8, 10, 12, 16, 32, 48, 64, 80, 96, 112, 128, 144, 4, 32, 64, 96, 128, 160, 192, 224, 256, 512, 768, 1024, 1280, 1024, 768, 5120 };
                    break;
                case 4: //TI and ACE
                    GameName = "TGM3";
                    FPS = 60.00;
                    nextNum = 3;
                    hold = true;
                    hardDrop = 1;
                    rotation = 1;
                    generator = (int)Gens.TGM3;
                    lag = 3;
                    lockType = 0;
                    bigMove = 2;
                    baseARE = 25;
                    baseARELine = 25;
                    baseDAS = 14;
                    baseLock = 30;
                    baseLineClear = 40;
                    gravType = 1;
                    baseGrav = 1024;
                    genAttps = 6;
                    fieldW = 10;
                    fieldH = 22;
                    showGrade = false;
                    gravTable = new List<int> { 1024, 1536, 2048, 2560, 3072, 4096, 8192, 12288, 16384, 20480, 24576, 28672, 32768, 36864, 1024, 8192, 16348, 24576, 32768, 40960, 49152, 57344, 65536, 131072, 196608, 262144, 327680, 262144, 196608, 1310720 };
                    break;
                case 5:
                case 7:
                    GameName = "Custom";
                    FPS = 60.00;
                    nextNum = 3;
                    hold = true;
                    hardDrop = 1;
                    rotation = 1;
                    generator = (int)Gens.TGM3;
                    lag = 3;
                    lockType = 0;
                    bigMove = 2;
                    baseARE = 25;
                    baseARELine = 25;
                    baseDAS = 14;
                    baseLock = 30;
                    baseLineClear = 40;
                    gravType = 1;
                    baseGrav = 1024;
                    genAttps = 6;
                    fieldW = 10;
                    fieldH = 22;
                    showGrade = false;
                    gravTable = new List<int> { 1024, 1536, 2048, 2560, 3072, 4096, 8192, 12288, 16384, 20480, 24576, 28672, 32768, 36864, 1024, 8192, 16348, 24576, 32768, 40960, 49152, 57344, 65536, 131072, 196608, 262144, 327680, 262144, 196608, 1310720 };
                    break;
                case 6: //4
                    GameName = "SEGA";
                    FPS = 60.00;
                    nextNum = 4;
                    hold = true;
                    hardDrop = 1;
                    rotation = 1;
                    lockType = 1;
                    bigMove = 2;
                    //recycled from TGM3 until more is known
                    baseARE = 25;
                    baseARELine = 25;
                    baseDAS = 14;
                    baseLock = 30;
                    baseLineClear = 40;
                    gravType = 1;
                    baseGrav = 1024;
                    genAttps = 6;
                    fieldW = 10;
                    fieldH = 22;
                    showGrade = false;
                    gravTable = new List<int> { 1024, 1536, 2048, 2560, 3072, 4096, 8192, 12288, 16384, 20480, 24576, 28672, 32768, 36864, 1024, 8192, 16348, 24576, 32768, 40960, 49152, 57344, 65536, 131072, 196608, 262144, 327680, 262144, 196608, 1310720 };
                    break;
            }

            id = mode;
            switch (mode)//Master, Death, shirase, sprint, garbage clear, rounds, konoha, grav training
            {
                case 0:
                    ModeName = "MASTER";
                    gradedBy = 1;
                    endLevel = 999;
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
                    switch (game)
                    {
                        case 1:
                            delayTable.Add(new List<int> { 30 });
                            delayTable.Add(new List<int> { 30 });
                            delayTable.Add(new List<int> { 16 });
                            delayTable.Add(new List<int> { 30 });
                            delayTable.Add(new List<int> { 41 });
                            break;
                        case 2://tgm2
                            delayTable.Add(new List<int> { 27, 27, 27, 18, 14, 14 });//ARE
                            delayTable.Add(new List<int> { 27, 27, 27, 27, 27, 27 });//line ARE
                            delayTable.Add(new List<int> { 16, 10, 10, 10, 10, 8 });//DAS
                            delayTable.Add(new List<int> { 30, 30, 30, 30, 30, 17 });//LOCK
                            delayTable.Add(new List<int> { 40, 25, 16, 12, 6, 6, 6 });//LINE CLEAR
                            break;
                        case 3://tap
                            delayTable.Add(new List<int> { 27, 27, 27, 18, 14, 14 });//ARE
                            delayTable.Add(new List<int> { 27, 27, 18, 14, 8, 8 });//line ARE
                            delayTable.Add(new List<int> { 14, 8, 8, 8, 8, 6 });//DAS
                            delayTable.Add(new List<int> { 30, 30, 30, 30, 30, 17 });//LOCK
                            delayTable.Add(new List<int> { 40, 25, 16, 12, 6, 6 });//LINE CLEAR
                            break;
                        case 4://tgm3
                            delayTable.Add(new List<int> { 27, 27, 27, 18, 14, 14, 8, 7, 6 });
                            delayTable.Add(new List<int> { 27, 27, 18, 14, 8, 8, 8, 7, 6 });
                            delayTable.Add(new List<int> { 14, 8, 8, 8, 8, 6, 6, 6, 6 });
                            delayTable.Add(new List<int> { 30, 30, 30, 30, 30, 17, 17, 15, 15 });
                            delayTable.Add(new List<int> { 40, 25, 16, 12, 6, 6, 6, 6, 6 });
                            break;
                        case 5://ace
                            break;
                        case 6://tgm4
                            
                            break;
                    }
                    break;
                case 1://death
                    ModeName = "DEATH";
                    endLevel = 999;
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
                    lvlBonus = 5;
                    gradedBy = 2;
                    border = Color.DarkRed;
                    g20 = true;
                    showGrade = true;
                    initialGrade = -1;
                    delayTable.Add(new List<int> { 18, 14, 14, 8, 7, 6 });
                    delayTable.Add(new List<int> { 14, 8, 8, 8, 7, 6 });
                    delayTable.Add(new List<int> { 10, 10, 9, 8, 6, 6 });
                    delayTable.Add(new List<int> { 30, 26, 22, 18, 15, 15 });
                    delayTable.Add(new List<int> { 12, 6, 6, 6, 5, 4 });
                    break;
                case 2://shirase
                    ModeName = "SHIRASE";
                    endLevel = 1300;
                    sections.Add(100);
                    sections.Add(200);
                    sections.Add(300);
                    sections.Add(400);
                    sections.Add(500);
                    sections.Add(600);
                    sections.Add(700);
                    sections.Add(800);
                    sections.Add(900);
                    sections.Add(1000);
                    sections.Add(1100);
                    sections.Add(1200);
                    sections.Add(1300);
                    lvlBonus = 5;
                    var gL = new Gimmick();
                    gL.type = 2;
                    gL.startLvl = 500;
                    gL.endLvl = 600;
                    gL.parameter = 20;
                    gimList.Add(gL);
                    gL = new Gimmick();
                    gL.type = 2;
                    gL.startLvl = 600;
                    gL.endLvl = 700;
                    gL.parameter = 18;
                    gimList.Add(gL);
                    gL = new Gimmick();
                    gL.type = 2;
                    gL.startLvl = 700;
                    gL.endLvl = 800;
                    gL.parameter = 10;
                    gimList.Add(gL);
                    gL = new Gimmick();
                    gL.type = 2;
                    gL.startLvl = 800;
                    gL.endLvl = 900;
                    gL.parameter = 9;
                    gimList.Add(gL);
                    gL = new Gimmick();
                    gL.type = 2;
                    gL.startLvl = 900;
                    gL.endLvl = 1000;
                    gL.parameter = 8;
                    gimList.Add(gL);
                    gL = new Gimmick();
                    gL.type = 3;
                    gL.startLvl = 1000;
                    gL.endLvl = 1301;
                    gimList.Add(gL);
                    gL = new Gimmick();
                    gL.type = 5;
                    gL.startLvl = 1300;
                    gL.endLvl = 1301;
                    gimList.Add(gL);
                    gradedBy = 2;
                    shiraseGrades = true;
                    border = Color.DarkBlue;
                    g20 = true;
                    showGrade = false;
                    initialGrade = -1;
                    delayTable.Add(new List<int> { 12, 12, 12, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6 });
                    delayTable.Add(new List<int> { 8, 7, 6, 6, 5, 5, 5, 5, 5, 5, 5, 5, 5, 6 });
                    delayTable.Add(new List<int> { 8, 6, 6, 6, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4 });
                    delayTable.Add(new List<int> { 18, 18, 17, 15, 13, 12, 12, 12, 12, 12, 12, 10, 8, 15 });
                    delayTable.Add(new List<int> { 6, 5, 4, 4, 3, 3, 3, 3, 3, 3, 3, 3, 3, 6 });
                    break;
                case 3://sprint
                    gradedBy = 4;
                    limitType = 1;
                    border = Color.DarkGreen;
                    break;
                case 4://garbage clear
                    gradedBy = 4;
                    limitType = 4;
                    limit = 1;
                    sections.Add(999);
                    delayTable.Add(new List<int> { 30 });
                    delayTable.Add(new List<int> { 30 });
                    delayTable.Add(new List<int> { 16 });
                    delayTable.Add(new List<int> { 30 });
                    delayTable.Add(new List<int> { 41 });
                    break;
                case 5://rounds
                    ModeName = "ICY SHIRASE";
                    gradedBy = 2;
                    lvlBonus = 5;
                    endLevel = 1200;
                    g20 = true;

                    gL = new Gimmick();
                    gL.type = 4;
                    gL.startLvl = 300;
                    gL.endLvl = 400;
                    gimList.Add(gL);
                    gL = new Gimmick();
                    gL.type = 4;
                    gL.startLvl = 500;
                    gL.endLvl = 600;
                    gimList.Add(gL);
                    gL = new Gimmick();
                    gL.type = 4;
                    gL.startLvl = 700;
                    gL.endLvl = 800;
                    gimList.Add(gL);
                    gL = new Gimmick();
                    gL.type = 4;
                    gL.startLvl = 900;
                    gL.endLvl = 1000;
                    gimList.Add(gL);
                    gL = new Gimmick();
                    gL.type = 4;
                    gL.startLvl = 1100;
                    gL.endLvl = 1200;
                    gimList.Add(gL);
                    border = Color.DarkRed;

                    delayTable.Add(new List<int> { 12, 12, 12, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6 });
                    delayTable.Add(new List<int> { 8, 7, 6, 6, 5, 5, 5, 5, 5, 5, 5, 5, 6 });
                    delayTable.Add(new List<int> { 8, 6, 6, 6, 4, 4, 4, 4, 4, 4, 4, 4, 4 });
                    delayTable.Add(new List<int> { 18, 18, 17, 15, 13, 12, 12, 12, 12, 12, 10, 8, 15 });
                    delayTable.Add(new List<int> { 6, 5, 4, 4, 3, 3, 3, 3, 3, 3, 3, 3, 6 });
                    break;
                case 6://konoha
                    ModeName = "BIG BRAVO MANIA";
                    endLevel = 0;
                    gradedBy = 3;
                    limitType = 3;
                    limit = 180000;//three minutes
                    bigmode = true;
                    //easyGen = true;
                    sections.Add(100);
                    sections.Add(200);
                    sections.Add(300);
                    sections.Add(400);
                    sections.Add(500);
                    sections.Add(600);
                    sections.Add(700);
                    sections.Add(800);
                    sections.Add(900);
                    sections.Add(1000);
                    sections.Add(1100);
                    sections.Add(1200);
                    border = Color.PaleVioletRed;
                    delayTable.Add(new List<int> { 27, 27, 27, 18, 14, 14, 8, 7, 6 });
                    delayTable.Add(new List<int> { 27, 27, 18, 14, 8, 8, 8, 7, 6 });
                    delayTable.Add(new List<int> { 14, 8, 8, 8, 8, 6, 6, 6, 6 });
                    delayTable.Add(new List<int> { 30, 30, 30, 30, 30, 17, 17, 15, 15 });
                    delayTable.Add(new List<int> { 40, 25, 16, 12, 6, 6, 6, 6, 6 });
                    break;
                case 7://20g training
                    ModeName = "20G TRAINING";
                    endLevel = 200;
                    g20 = true;
                    gradedBy = 4;

                    sections.Add(200);
                    delayTable.Add(new List<int> { 27 });
                    delayTable.Add(new List<int> { 27 });
                    delayTable.Add(new List<int> { 14 });
                    delayTable.Add(new List<int> { 30 });
                    delayTable.Add(new List<int> { 40 });
                    break;
            }
        }
    }
}
