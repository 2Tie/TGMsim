using System;
using System.Collections.Generic;

namespace TGMsim
{
    class GameRules
    {
        public Games gameRules;
        public int nextNum = 1;
        public bool hold = false;
        public int hardDrop = 0; //none, sonic, firm

        public int genAttps = 4;

        public int rotation = 0; //TGM, TGM3, SEGA, SRS
        public int generator = 0; //dummy, TGM1, TGM2, TGM3, ACE, 
        public enum Rots { ARS1 = 0, ARS3, SEGA, SEMIPRO};
        public enum Gens { dummy = 0, TGM1, TGM2, TGM3, ACE, SEGA, EZ, CCSEZ, CCS};
        public enum Games { SEGA = 0, TGM1, TGM2, TAP, TGM3, ACE, GMX, EXTRA, GUIDELINE}
        
        public int gravType = 0; //b256, b65536, frames
        public int baseGrav = 4;

        public int lockType = 0; //step-reset, move-reset
        public int flashLength = 3;
        public bool instaLock = true;

        public int creditsLength = 3238;
        public bool hasKillSpeed = false;
        public int killSpeedTime = 0;
        public List<int> killSpeedDelays;

        public int fieldW = 10;
        public int fieldH = 22;

        public double FPS = 60.00;
        public int lag = 0;

        public string GameName = "error";

        public Mode mod;


        public List<List<double>> comboTable = new List<List<double>>();

        public List<int> decayRate = new List<int>() { 125, 80, 80, 50, 45, 45, 45, 40, 40, 40, 40, 40, 30, 30, 30, 20, 20, 20, 20, 20, 15, 15, 15, 15, 15, 15, 15, 15, 15, 15, 10, 10 };
        public List<List<int>> baseGradePts = new List<List<int>>();

        public List<int> gravTable = new List<int>();
        public List<int> gravLevels = new List<int>() { 0, 30, 35, 40, 50, 60, 70, 80, 90, 100, 120, 140, 160, 170, 200, 220, 230, 233, 236, 239, 243, 247, 251, 300, 330, 360, 400, 420, 450, 500 };
        public List<List<int>> delayTable = new List<List<int>>();
        

        //public List<string> grades = new List<string> { "9", "8", "7", "6", "5", "4", "3", "2", "1", "S1", "S2", "S3", "S4", "S5", "S6", "S7", "S8", "S9", "m1", "m2", "m3", "m4", "m5", "m6", "m7", "m8", "m9", "M", "MK", "MV", "MO", "MM", "GM", "TM" };

        public List<int> gradeIntTGM2 = new List<int> { 0, 1, 2, 3, 4, 5, 5, 6, 6, 7, 7, 7, 8, 8, 8, 9, 9, 9, 10, 11, 12, 12, 12, 13, 13, 14, 14, 15, 15, 16, 16, 17 };
        
        public int exam = -1;
        public bool mute = false;
        public int bigMove = 1;

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

        public void setup(Games game, Mode.ModeType mode, int vari)
        {
            gameRules = game;
            //variant = vari;
            switch (game)
            {
                case Games.SEGA: //SEGA
                    GameName = "SEGA";
                    FPS = 60.00;
                    nextNum = 1;
                    hold = false;
                    hardDrop = 0;
                    rotation = (int)Rots.SEGA;
                    generator = (int)Gens.SEGA;
                    lockType = 0;
                    flashLength = 22;
                    instaLock = false;
                    bigMove = 1;
                    gravType = 2;
                    baseGrav = 256;
                    genAttps = 6;
                    fieldW = 10;
                    fieldH = 20;
                    //gravTable = new List<int> { 5.3, 10.6, 14.2, 17.06, 21.3, 25.6, 32, 42.6, 64, 128, 25.6, 32, 42.6, 64, 128, 256 }; //actual. all final decimal places repeat.
                    gravTable = new List<int> { 48, 24, 18, 15, 12, 10, 8, 6, 4, 2, 10,  8, 6, 4, 2, 1 };
                    gravLevels = new List<int> { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15 };
                    break;
                case Games.TGM1: //TGM
                    GameName = "TGM";
                    FPS = 59.84;
                    nextNum = 1;
                    hold = false;
                    hardDrop = 0;
                    rotation = (int)Rots.ARS1;
                    generator = (int)Gens.TGM1;
                    lag = 2;
                    lockType = 0;
                    bigMove = 1;
                    gravType = 0;
                    baseGrav = 4;
                    genAttps = 4;
                    fieldW = 10;
                    fieldH = 22;
                    creditsLength = 2968; //taken from nullpomino, though estimates place it around 2961. still need to verify. this also cuts off credits song!
                    gravTable = new List<int> { 4, 6, 8, 10, 12, 16, 32, 48, 64, 80, 96, 112, 128, 144, 4, 32, 64, 96, 128, 160, 192, 224, 256, 512, 768, 1024, 1280, 1024, 768, 5120 };
                    break;
                case Games.TGM2: //TGM2
                    GameName = "TGM2";
                    FPS = 61.68;
                    nextNum = 1;
                    hold = false;
                    hardDrop = 1;
                    rotation = (int)Rots.ARS1;
                    generator = (int)Gens.TGM2;
                    lag = 0;
                    lockType = 0;
                    flashLength = 2;
                    bigMove = 2;
                    gravType = 0;
                    baseGrav = 4;
                    genAttps = 6;
                    fieldW = 10;
                    fieldH = 22;
                    gravTable = new List<int> { 4, 6, 8, 10, 12, 16, 32, 48, 64, 80, 96, 112, 128, 144, 4, 32, 64, 96, 128, 160, 192, 224, 256, 512, 768, 1024, 1280, 1024, 768, 5120 };
                    break;
                case Games.TAP: //TAP
                    GameName = "TAP";
                    FPS = 61.68;
                    nextNum = 1;
                    hold = false;
                    hardDrop = 1;
                    rotation = (int)Rots.ARS1;
                    generator = (int)Gens.TGM2;
                    lag = 0;
                    lockType = 0;
                    flashLength = 3; //one black, two white
                    bigMove = 1;
                    gravType = 0;
                    baseGrav = 4;
                    genAttps = 6;
                    fieldW = 10;
                    fieldH = 22;
                    gravTable = new List<int> { 4, 6, 8, 10, 12, 16, 32, 48, 64, 80, 96, 112, 128, 144, 4, 32, 64, 96, 128, 160, 192, 224, 256, 512, 768, 1024, 1280, 1024, 768, 5120 };
                    hasKillSpeed = true;
                    killSpeedTime = 15 * 60 * 1000; //15 minutes
                    killSpeedDelays = new List<int> { 7, 7, 16, 7 }; //no DAS
                    break;
                case Games.TGM3: //TI
                    GameName = "TGM3";
                    FPS = 60.00;
                    nextNum = 3;
                    hold = true;
                    hardDrop = 1;
                    rotation = (int)Rots.ARS3;
                    generator = (int)Gens.TGM3;
                    lag = 3;
                    lockType = 0;
                    flashLength = 1;
                    bigMove = 2;
                    gravType = 1;
                    baseGrav = 1024;
                    genAttps = 6;
                    fieldW = 10;
                    fieldH = 22;
                    gravTable = new List<int> { 1024, 1536, 2048, 2560, 3072, 4096, 8192, 12288, 16384, 20480, 24576, 28672, 32768, 36864, 1024, 8192, 16348, 24576, 32768, 40960, 49152, 57344, 65536, 131072, 196608, 262144, 327680, 262144, 196608, 1310720 };
                    hasKillSpeed = true;
                    killSpeedTime = 15 * 60 * 1000; //15 minutes
                    killSpeedDelays = new List<int> { 5, 6, 13, 2 }; //no DAS
                    break;
                case Games.ACE:
                case Games.EXTRA:
                    GameName = "BONUS";
                    FPS = 60.00;
                    nextNum = 3;
                    hold = true;
                    hardDrop = 1;
                    rotation = (int)Rots.ARS3;
                    generator = (int)Gens.TGM3;
                    lag = 3;
                    lockType = 0;
                    bigMove = 2;
                    gravType = 1;
                    baseGrav = 1024;
                    genAttps = 6;
                    fieldW = 10;
                    fieldH = 22;
                    gravTable = new List<int> { 1024, 1536, 2048, 2560, 3072, 4096, 8192, 12288, 16384, 20480, 24576, 28672, 32768, 36864, 1024, 8192, 16348, 24576, 32768, 40960, 49152, 57344, 65536, 131072, 196608, 262144, 327680, 262144, 196608, 1310720 };
                    break;
                case Games.GMX: //GMX?
                    GameName = "XTREME";
                    FPS = 60.00;
                    nextNum = 3;
                    hold = true;
                    hardDrop = 1;
                    rotation = (int)Rots.ARS3;
                    generator = (int)Gens.TGM3;
                    lockType = 0;
                    bigMove = 2;
                    gravType = 0;
                    baseGrav = 256;
                    genAttps = 6;
                    fieldW = 10;
                    fieldH = 22;
                    gravLevels = new List<int>() { 0, 25, 50, 75, 100, 125, 150, 175, 200, 225, 250, 275, 300, 325, 350, 375, 400, 425, 450, 475, 500, 600, 700,  800,  900, 1000};
                    gravTable = new List<int>    { 4, 11, 19, 26,  34,  41,  49,  56,  64,  72,  80,  88,  96, 104, 112, 120, 128, 160, 192, 224, 256, 512, 768, 1024, 1280, 5120};
                    break;
                case Games.GUIDELINE: //henk plz
                    GameName = "GUIDELINE";
                    FPS = 60.0;
                    nextNum = 4;
                    hold = true;
                    hardDrop = 1;
                    /*rotation = (int)Rots.Guideline;
                    generator = (int)Gens.Guideline;*/ //these aren't in scope, but others are free to contribute ;)
                    lockType = 1;
                    bigMove = 2;
                    gravType = 1;
                    baseGrav = 1024;
                    genAttps = 6;
                    fieldW = 10;
                    fieldH = 24;
                    gravTable = new List<int> { 1024, 1536, 2048, 2560, 3072, 4096, 8192, 12288, 16384, 20480, 24576, 28672, 32768, 36864, 1024, 8192, 16348, 24576, 32768, 40960, 49152, 57344, 65536, 131072, 196608, 262144, 327680, 262144, 196608, 1310720 };
                    break;
            }
            
            switch (mode)
            {
                case Mode.ModeType.MASTER: //Master
                    switch (game)
                    {
                        case Games.TGM1:
                            mod = new Modes.Master1();
                            break;
                        case Games.TGM2://tgm2
                            mod = new Modes.Master2();
                            break;
                        case Games.TAP://tap
                            mod = new Modes.Master2Plus();
                            break;
                        case Games.TGM3://tgm3
                            mod = new Modes.Master3();
                            break;
                    }
                    break;
                case Mode.ModeType.DEATH://death
                    mod = new Modes.Death();
                    break;
                case Mode.ModeType.SHIRASE://shirase
                    mod = new Modes.Shirase();
                    break;
                case Mode.ModeType.SPRINT://sprint
                    /*gradedBy = 4;
                    limitType = 1;
                    border = Color.DarkGreen;*/
                    break;
                case Mode.ModeType.GARBAGE://garbage clear
                    mod = new Modes.Garbo();
                    break;
                case Mode.ModeType.ROUNDS://rounds
                    mod = new Modes.IcyShirase();
                    break;
                case Mode.ModeType.KONOHA://konoha
                    generator = (int)Gens.EZ;
                    mod = new Modes.BigBravoMania();
                    break;
                case Mode.ModeType.TRAINING://20g training
                    mod = new Modes.Training();
                    break;
                case Mode.ModeType.SEGA: //segatet
                    mod = new Modes.SegaTet();
                    break;
                case Mode.ModeType.MINER: //miner
                    mod = new Modes.Miner(vari);
                    break;
                case Mode.ModeType.DYNAMO: //dynamo
                    mod = new Modes.Dynamo(vari);
                    break;
                case Mode.ModeType.ENDURA: //endura
                    break;
                case Mode.ModeType.BLOX: //bloxeed
                    mod = new Modes.SegaBlox();
                    gravTable = new List<int> { 16, 14, 12, 10, 8, 6, 4, 3, 2, 1, 10, 8, 6, 4, 2, 1 };
                    break;
                case Mode.ModeType.PLUS: //tgm+
                    mod = new Modes.Plus();
                    break;
                case Mode.ModeType.FLASH: //flash point
                    mod = new Modes.SegaFlash();
                    break;
                case Mode.ModeType.MARCH: //hell march
                    mod = new Modes.HellMarch();
                    break;
                case Mode.ModeType.EASY: //easy
                    mod = new Modes.Easy(); //uuuhhhhhhh?????
                    break;
                case Mode.ModeType.CCS:
                    mod = new Modes.CCS(vari);
                    break;
                case Mode.ModeType.PRACTICE:
                    setup(Games.TGM1, Mode.ModeType.MASTER, 0);
                    mod = new Modes.Master1Practice(vari);
                    break;
                case Mode.ModeType.CUSTOM:
                    throw new Exception("don't call this, nerd");
                    break;
                case Mode.ModeType.SHIMIZU:
                    setup(Games.SEGA, Mode.ModeType.SEGA, 0);
                    mod.g20 = true;
                    rotation = (int)Rots.SEMIPRO;
                    break;
                default:
                    throw new Exception("unknown mode");
                    break;
            }
            mod.modeID = mode;  
        }
    }
}
