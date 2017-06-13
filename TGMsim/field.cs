using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;
using NAudio;
using System.Windows.Forms;
using System.Drawing.Text;

namespace TGMsim
{
    class Field
    {

        public bool gameRunning;


        public struct vanPip
        {
            public long time;
            public int x, y;
        }
        public struct flashPip
        {
            public int time;
            public int x, y;
        }

        Tetromino activeTet;
        public List<Tetromino> nextTet = new List<Tetromino>();
        public List<int> lastTet = new List<int>() { 6, 6, 6, 6};
        public Rotation RSYS = new R_ARS1();

        public List<List<int>> gameField = new List<List<int>>();

        public List<int> full = new List<int>();

        public List<int> medals = new List<int>() { 0, 0, 0, 0, 0, 0 };

        public List<long> sectionTimes = new List<long>();
        public List<bool> GMflags = new List<bool>();
        public List<int> secTet = new List<int>();
        public List<int> secCools = new List<int>();
        public List<vanPip> vanList = new List<vanPip>();
        public List<flashPip> flashList = new List<flashPip>();
        public int creditsType = 0;
        int fadeout = 0;


        public bool isGM = false;

        public Tetromino heldPiece;

        public Tetromino ghostPiece;

        public GameTimer timer = new GameTimer();
        public GameTimer contTime = new GameTimer();
        public GameTimer startTime = new GameTimer();
        public GameTimer sectionTime = new GameTimer();
        public GameTimer creditsPause = new GameTimer();
        public GameTimer coolTime = new GameTimer();
        public GameTimer bravoTime = new GameTimer();
        public long masterTime = 0;

        public int swappedHeld;
        bool justSpawned;
        bool safelock = false;

        public int x, y, width, height;
        public enum timerType { ARE, DAS, LockDelay, LineClear} ;
        public int currentTimer = 0;
        public int timerCount = 0;
        //public int groundTimer = 0;
        public int gravCounter = 0;
        public int gravLevel = 0;
        public int level = 0;
        public int grade = 0;
        public int gm2grade = 0;
        public int score = 0;
        public int combo = 1;
        public int gradeCombo = 1;
        public bool comboing = false;
        public int gradePoints = 0;
        public int gradeLevel = 0;
        public int gradeTime = 0;
        List<int> gravTable;

        public int starting = 1;
        public bool inCredits = false;
        int creditsProgress;
        public bool newHiscore = false;
        public bool torikan = false;
        public long torDef = 0;

        public int bravoCounter = 0;
        public int tetrises = 0;
        public int totalTets = 0;
        public int rotations = 0;
        bool recoverChecking = false;
        public int recoveries = 0;
        public int speedBonus = 0;
        public int creditGrades = 0;

        public int softCounter = 0;
        public int sonicCounter = 0;
        public int tetLife = 0;

        //Rules ruleset;
        //public Mode mode;
        public GameRules ruleset;
        int curSection;

        public GameResult results;

        public bool cheating = false;
        public bool godmode = false;
        public bool bigmode = false;
        public bool g20 = false;
        public bool g0 = false;
        public bool w4 = false;

        public List<int> activeGim = new List<int>();
        public int gimIndex = 0;
        int garbTimer = 0;

        public bool cont = false;
        public bool exit = false;

        List<Image> tetImgs = new List<Image>();
        List<Image> tetSImgs = new List<Image>();
        List<Image> bgs = new List<Image>();
        Image medalImg;
        Image gradeImg;
        Color frameColour;
        SolidBrush textBrush = new SolidBrush(Color.White);

        PrivateFontCollection fonts = new PrivateFontCollection();
        Font f_Maestro;

        NAudio.Wave.WaveOutEvent soundList = new NAudio.Wave.WaveOutEvent();
        NAudio.Vorbis.VorbisWaveReader vorbisStream;
        System.Windows.Media.MediaPlayer s_Ready = new System.Windows.Media.MediaPlayer();
        System.Windows.Media.MediaPlayer s_Go = new System.Windows.Media.MediaPlayer();
        System.Windows.Media.MediaPlayer s_Tet1 = new System.Windows.Media.MediaPlayer();
        System.Windows.Media.MediaPlayer s_Tet2 = new System.Windows.Media.MediaPlayer();
        System.Windows.Media.MediaPlayer s_Tet3 = new System.Windows.Media.MediaPlayer();
        System.Windows.Media.MediaPlayer s_Tet4 = new System.Windows.Media.MediaPlayer();
        System.Windows.Media.MediaPlayer s_Tet5 = new System.Windows.Media.MediaPlayer();
        System.Windows.Media.MediaPlayer s_Tet6 = new System.Windows.Media.MediaPlayer();
        System.Windows.Media.MediaPlayer s_Tet7 = new System.Windows.Media.MediaPlayer();
        System.Windows.Media.MediaPlayer s_PreRot = new System.Windows.Media.MediaPlayer();
        System.Windows.Media.MediaPlayer s_Contact = new System.Windows.Media.MediaPlayer();
        System.Windows.Media.MediaPlayer s_Lock = new System.Windows.Media.MediaPlayer();
        System.Windows.Media.MediaPlayer s_Clear = new System.Windows.Media.MediaPlayer();
        System.Windows.Media.MediaPlayer s_Impact = new System.Windows.Media.MediaPlayer();
        System.Windows.Media.MediaPlayer s_Grade = new System.Windows.Media.MediaPlayer();
        System.Windows.Media.MediaPlayer s_Hold = new System.Windows.Media.MediaPlayer();
        System.Windows.Media.MediaPlayer s_GameClear = new System.Windows.Media.MediaPlayer();
        System.Windows.Media.MediaPlayer s_Cool = new System.Windows.Media.MediaPlayer();
        System.Windows.Media.MediaPlayer s_Regret = new System.Windows.Media.MediaPlayer();
        System.Windows.Media.MediaPlayer s_Section = new System.Windows.Media.MediaPlayer();
        System.Windows.Media.MediaPlayer s_Tetris = new System.Windows.Media.MediaPlayer();
        System.Windows.Media.MediaPlayer s_Combo = new System.Windows.Media.MediaPlayer();
        System.Windows.Media.MediaPlayer s_Medal = new System.Windows.Media.MediaPlayer();

        Pen gridPen = new Pen(new SolidBrush(Color.White));

        Controller pad;
        int inputDelayH = 0, inputDelayDir = 0;

        public Field(Controller ctlr, GameRules rules, NAudio.Vorbis.VorbisWaveReader music)
        {
            x = 200;
            y = 50;
            width = 250;
            height = 500;

            tetImgs.Add(null);
            tetImgs.Add(Image.FromFile("Res/GFX/t1.png"));
            tetImgs.Add(Image.FromFile("Res/GFX/t2.png"));
            tetImgs.Add(Image.FromFile("Res/GFX/t3.png"));
            tetImgs.Add(Image.FromFile("Res/GFX/t4.png"));
            tetImgs.Add(Image.FromFile("Res/GFX/t5.png"));
            tetImgs.Add(Image.FromFile("Res/GFX/t6.png"));
            tetImgs.Add(Image.FromFile("Res/GFX/t7.png"));
            tetImgs.Add(null);
            tetImgs.Add(Image.FromFile("Res/GFX/t9.png"));
            tetImgs.Add(Image.FromFile("Res/GFX/t8.png"));

            tetSImgs.Add(null);
            tetSImgs.Add(Image.FromFile("Res/GFX/s1.png"));
            tetSImgs.Add(Image.FromFile("Res/GFX/s2.png"));
            tetSImgs.Add(Image.FromFile("Res/GFX/s3.png"));
            tetSImgs.Add(Image.FromFile("Res/GFX/s4.png"));
            tetSImgs.Add(Image.FromFile("Res/GFX/s5.png"));
            tetSImgs.Add(Image.FromFile("Res/GFX/s6.png"));
            tetSImgs.Add(Image.FromFile("Res/GFX/s7.png"));
            tetSImgs.Add(null);
            tetSImgs.Add(Image.FromFile("Res/GFX/s9.png"));
            tetSImgs.Add(Image.FromFile("Res/GFX/s8.png"));

            bgs.Add(Image.FromFile("Res/GFX/bgs/1.png"));
            bgs.Add(Image.FromFile("Res/GFX/bgs/2.png"));
            bgs.Add(null);
            bgs.Add(null);
            bgs.Add(null);
            bgs.Add(null);
            bgs.Add(null);
            bgs.Add(null);
            bgs.Add(null);
            bgs.Add(null);

            medalImg = Image.FromFile("Res/GFX/medals.png");
            gradeImg = Image.FromFile("Res/GFX/grades.png");


            Audio.addSound(s_Ready, @"\Res\Audio\SE\SEP_ready.wav");
            Audio.addSound(s_Go, @"/Res/Audio/SE/SEP_go.wav");
            Audio.addSound(s_Tet1, @"/Res/Audio/SE/SEB_mino1.wav");
            Audio.addSound(s_Tet2, @"/Res/Audio/SE/SEB_mino2.wav");
            Audio.addSound(s_Tet3, @"/Res/Audio/SE/SEB_mino3.wav");
            Audio.addSound(s_Tet4, @"/Res/Audio/SE/SEB_mino4.wav");
            Audio.addSound(s_Tet5, @"/Res/Audio/SE/SEB_mino5.wav");
            Audio.addSound(s_Tet6, @"/Res/Audio/SE/SEB_mino6.wav");
            Audio.addSound(s_Tet7, @"/Res/Audio/SE/SEB_mino7.wav");
            Audio.addSound(s_PreRot, @"/Res/Audio/SE/SEB_prerotate.wav");
            Audio.addSound(s_Contact, @"/Res/Audio/SE/SEB_fixa.wav");
            Audio.addSound(s_Lock, @"/Res/Audio/SE/SEB_instal.wav");
            Audio.addSound(s_Clear, @"/Res/Audio/SE/SEB_disappear.wav");
            Audio.addSound(s_Impact, @"/Res/Audio/SE/SEB_fall.wav");
            Audio.addSound(s_Grade,  @"/Res/Audio/SE/SEP_levelchange.wav");
            Audio.addSound(s_Hold, @"/Res/Audio/SE/SEB_prehold.wav");
            Audio.addSound(s_GameClear, @"/Res/Audio/SE/SEP_gameclear.wav");
            Audio.addSound(s_Cool, @"/Res/Audio/SE/SEP_cool.wav");
            Audio.addSound(s_Regret, @"/Res/Audio/SE/SEI_vs_select.wav");
            Audio.addSound(s_Section, @"/Res/Audio/SE/SEP_lankup.wav");
            Audio.addSound(s_Combo, @"/Res/Audio/SE/SEP_combo.wav");
            Audio.addSound(s_Tetris, @"/Res/Audio/SE/SEP_tetris.wav");
            Audio.addSound(s_Medal, @"/Res/Audio/SE/SEP_platinum.wav");

            fonts.AddFontFile(@"Res\Maestro.ttf");
            FontFamily fontFam = fonts.Families[0];
            f_Maestro = new System.Drawing.Font(fontFam, 16, GraphicsUnit.Pixel);

            pad = ctlr;
            ruleset = rules;
            vorbisStream = music;

            if (ruleset.rotation == 0) RSYS = new R_ARS1();
            if (ruleset.rotation == 1) RSYS = new R_ARS3();

            Random random = new Random();

            activeTet = new Tetromino(0, ruleset.bigmode); //first piece cannot be S, Z, or O

            if (nextTet.Count == 0 && ruleset.nextNum > 0) //generate nextTet
            {
                for (int i = 0; i < ruleset.nextNum; i++)
                {
                    nextTet.Add(generatePiece());
                }
            }

            g20 = ruleset.g20;

            gravTable = ruleset.gravTable;


            ruleset.baseARE = ruleset.delayTable[0][0];
            ruleset.baseARELine = ruleset.delayTable[1][0];
            ruleset.baseDAS = ruleset.delayTable[2][0];
            ruleset.baseLock = ruleset.delayTable[3][0];
            ruleset.baseLineClear = ruleset.delayTable[4][0];

            frameColour = ruleset.border;

            if (ruleset.exam != -1)
                frameColour = Color.Gold;

            speedBonus = ruleset.lvlBonus;

            gameRunning = true;
            starting = 1;

            
            while (gravLevel < gravTable.Count - 1) //update gravity
            {
                if (level + (speedBonus * 100) >= ruleset.gravLevels[gravLevel + 1])
                    gravLevel++;
                else
                    break;
            }

            if ((ruleset.g20 == true || gravTable[gravLevel] == Math.Pow(256, ruleset.gravType + 1) * 20) && ruleset.gameRules < 4)
                textBrush = new SolidBrush(Color.Gold);

            //timer.start();
            startTime.start();
            sectionTime.start();
            secTet.Add(0);

            for (int i = 0; i < ruleset.fieldW; i++)
            {
                List<int> tempList = new List<int>();
                for (int j = 0; j < ruleset.fieldH; j++)
                {
                    tempList.Add(0); // at least nine types; seven tetrominoes, invisible, and garbage
                }
                gameField.Add(tempList);
            }

            if (w4)
                w4ify();


            if (ruleset.id == 4)
                randomize();

            updateMusic();
            //playMusic("Level 1");
            //playSound(s_Ready);
        }

        public void randomize()
        {
            Random rng = new Random();
            for (int i = 0; i < 80; i++)
            {
                gameField[rng.Next(10)][rng.Next(16) + 5] = rng.Next(8);
                
            }
        }

        public void draw(Graphics drawBuffer)
        {
            //draw the background
            int bg = 9;
            if (curSection < 9) bg = curSection;
            if (bgs[bg] != null) drawBuffer.DrawImage(bgs[bg], 0, 0, 800, 600);

            //draw the field
            drawBuffer.FillRectangle(new SolidBrush(Color.FromArgb(120, Color.Black)), x, y + 25, width, height);

            //draw the info bg
            drawBuffer.FillRectangle(new SolidBrush(Color.FromArgb(120, Color.Black)), x + 275, y + 20, 100, height + 10);

            //draw the pieces
            for (int i = 0; i < 10; i++)
            {
                for (int j = 2; j < 22; j++)
                {
                    bool flashing = false;
                    for (int k = 0; k < flashList.Count; k++)
                    {
                        if (i == flashList[k].x && j == flashList[k].y)
                            flashing = true;
                    }
                    if (flashing)
                    {
                        drawBuffer.DrawImageUnscaled(tetImgs[9], x + 25 * i, y - 25 + j * 25, 25, 25);
                    }
                    else
                    {

                        int block = gameField[i][j];
                        if (block == 9)//garbage
                            drawBuffer.DrawImageUnscaled(tetImgs[block], x + 25 * i, y - 25 + j * 25, 25, 25);
                        else if (block % 8 != 0)
                        {
                            drawBuffer.DrawImageUnscaled(tetImgs[block], x + 25 * i, y - 25 + j * 25, 25, 25);
                            drawBuffer.FillRectangle(new SolidBrush(Color.FromArgb(130, Color.Black)), x + 25 * i, y - 25 + j * 25, 25, 25);
                        }

                        //outline
                        if (block % 8 != 0 && block != 10)
                        {
                            if (i > 0)
                                if (gameField[i - 1][j] == 0)//left
                                    drawBuffer.FillRectangle(new SolidBrush(Color.FromArgb(150, Color.White)), x + 25 * i, y - 25 + j * 25, 3, 25);
                            if (i < 9)
                                if (gameField[i + 1][j] == 0)//right
                                    drawBuffer.FillRectangle(new SolidBrush(Color.FromArgb(150, Color.White)), x + 25 * i + 22, y - 25 + j * 25, 3, 25);
                            if (j > 0)
                                if (gameField[i][j - 1] == 0)//down
                                    drawBuffer.FillRectangle(new SolidBrush(Color.FromArgb(150, Color.White)), x + 25 * i, y - 25 + j * 25, 25, 3);
                            if (j < 21)
                                if (gameField[i][j + 1] == 0)//up
                                    drawBuffer.FillRectangle(new SolidBrush(Color.FromArgb(150, Color.White)), x + 25 * i, y - 25 + j * 25 + 22, 25, 3);
                        }
                    }
                }
            }

            //draw the frame
            drawBuffer.FillRectangle(new SolidBrush(frameColour), x - 5, y - 5 + 25, width + 10, 5);
            drawBuffer.FillRectangle(new SolidBrush(frameColour), x - 5, y + height + 25, width + 10, 5);
            drawBuffer.FillRectangle(new SolidBrush(frameColour), x - 5, y - 5 + 25, 5, height + 10);
            drawBuffer.FillRectangle(new SolidBrush(frameColour), x + width, y - 5 + 25, 5, height + 10);

            int big = 1;
            if (activeTet.big)
                big = 2;

            //draw the ghost piece
            if (level < 100 && ghostPiece != null && activeTet.id == ghostPiece.id)
            {

                for (int i = 0; i < activeTet.bits.Count; i++)
                {
                    if (ghostPiece.bits[i].y > 1)
                    {
                        drawBuffer.DrawImage(tetImgs[ghostPiece.id], x + 25 * ghostPiece.bits[i].x, y - 25 + 25 * ghostPiece.bits[i].y, 25 * big, 25 * big);
                        drawBuffer.FillRectangle(new SolidBrush(Color.FromArgb(130, Color.Black)), x + 25 * ghostPiece.bits[i].x, y - 25 + 25 * ghostPiece.bits[i].y, 25 * big, 25 * big);
                    }
                }
            }

            //draw the current piece
            if (activeTet.id != 0)
            {
                for (int i = 0; i < activeTet.bits.Count; i++)
                {

                    if (activeTet.bone == true)
                        drawBuffer.DrawImage(tetImgs[10], x + 25 * activeTet.bits[i].x, y - 25 + 25 * activeTet.bits[i].y, 25 * big, 25 * big);
                    else
                    {
                        if (activeTet.groundTimer > 0)
                        {
                            drawBuffer.DrawImage(tetImgs[activeTet.id], x + 25 * activeTet.bits[i].x, y - 25 + 25 * activeTet.bits[i].y, 25 * big, 25 * big);
                            drawBuffer.FillRectangle(new SolidBrush(Color.FromArgb((ruleset.baseLock - activeTet.groundTimer) * 127 / ruleset.baseLock, Color.Black)), x + 25 * activeTet.bits[i].x, y - 25 + 25 * activeTet.bits[i].y, 25 * big, 25 * big);
                        }
                        //else
                            //drawBuffer.DrawImage(tetImgs[9], x + 25 * (activeTet.bits[i].x * (2 / big) - (4 % (6 - big))), y - 25 + 25 * (activeTet.bits[i].y * (2 / big) - (lowY * ((2 / big) - 1))), 25 * (2 / big), 25 * (2 / big));
                    }
                }

                if (swappedHeld == 2)//draw held outline
                {
                    for (int i = 0; i < 4; i++)//for each piece
                    {
                        bool line = true;
                        for (int j = 1; j < 4; j++)//for each other piece, left
                        {
                            if (activeTet.bits[i].x - 1 == activeTet.bits[(i + j) % 4].x && activeTet.bits[i].y == activeTet.bits[(i + j) % 4].y)
                                line = false;

                        }
                        if (line)
                            drawBuffer.FillRectangle(new SolidBrush(Color.FromArgb(200, Color.Yellow)), x + 25 * activeTet.bits[i].x, y - 25 + 25 * activeTet.bits[i].y, 3, 25 * big);

                        line = true;
                        for (int j = 1; j < 4; j++)//for each other piece, up
                        {
                            if (activeTet.bits[i].x == activeTet.bits[(i + j) % 4].x && activeTet.bits[i].y - 1 == activeTet.bits[(i + j) % 4].y)
                                line = false;

                        }
                        if (line)
                            drawBuffer.FillRectangle(new SolidBrush(Color.FromArgb(200, Color.Yellow)), x + 25 * activeTet.bits[i].x, y - 25 + 25 * activeTet.bits[i].y, 25 * big, 3);

                        line = true;
                        for (int j = 1; j < 4; j++)//for each other piece, right
                        {
                            if (activeTet.bits[i].x + 1 == activeTet.bits[(i + j) % 4].x && activeTet.bits[i].y == activeTet.bits[(i + j) % 4].y)
                                line = false;

                        }
                        if (line)
                            drawBuffer.FillRectangle(new SolidBrush(Color.FromArgb(200, Color.Yellow)), x + 25 * activeTet.bits[i].x + 22 + 25 * (big-1), y - 25 + 25 * activeTet.bits[i].y, 3, 25 * big);

                        line = true;
                        for (int j = 1; j < 4; j++)//for each other piece, down
                        {
                            if (activeTet.bits[i].x == activeTet.bits[(i + j) % 4].x && activeTet.bits[i].y + 1 == activeTet.bits[(i + j) % 4].y)
                                line = false;

                        }
                        if (line)
                            drawBuffer.FillRectangle(new SolidBrush(Color.FromArgb(200, Color.Yellow)), x + 25 * activeTet.bits[i].x, y - 25 + 25 * activeTet.bits[i].y + 22 + 25 * (big-1), 25 * big, 3);
                    }
                }
            }

            //draw the next piece
            if (ruleset.nextNum > 0)
            {
                for (int i = 0; i < ruleset.nextNum; i++)
                {
                    for (int j = 0; j < 4; j++)
                    {
                        if (i == 0)//1st next drawn at full size
                        {
                            if (nextTet[i].bone == true)
                                drawBuffer.DrawImage(tetImgs[10], x + 25 * activeTet.bits[i].x, y - 75 + 25 * activeTet.bits[i].y, 25, 25);
                                //drawBuffer.DrawImageUnscaled(tetImgs[10], x + i * 70 + 16 * nextTet[i].bits[j].x + 40, y + 16 * nextTet[i].bits[j].y - 75);
                            else
                                drawBuffer.DrawImage(tetImgs[nextTet[i].id], x + 25 * nextTet[i].bits[j].x, y - 95 + 25 * nextTet[i].bits[j].y, 25, 25);
                                //drawBuffer.DrawImageUnscaled(tetImgs[nextTet[i].id], x + i * 70 + 16 * nextTet[i].bits[j].x + 40, y + 16 * nextTet[i].bits[j].y - 75);
                        }
                        else
                        {
                            if (nextTet[i].bone == true)
                                drawBuffer.DrawImageUnscaled(tetSImgs[10], x + i * 80 + 16 * nextTet[i].bits[j].x + 65, y + 16 * nextTet[i].bits[j].y - 75);
                            else
                                drawBuffer.DrawImageUnscaled(tetSImgs[nextTet[i].id], x + i * 80 + 16 * nextTet[i].bits[j].x + 65, y + 16 * nextTet[i].bits[j].y - 75);
                        }
                    }
                }
            }

            //draw the hold piece
            if(ruleset.hold == true && heldPiece != null)
            {
                for (int i = 0; i < 4; i++)
                {
                    if (heldPiece.bone == true)
                        drawBuffer.DrawImageUnscaled(tetSImgs[10], x - 75 + 16 * heldPiece.bits[i].x, y - 50 + 16 * heldPiece.bits[i].y);
                    else
                        if (swappedHeld != 0)
                            drawBuffer.DrawImageUnscaled(tetSImgs[9], x - 75 + 16 * heldPiece.bits[i].x, y - 50 + 16 * heldPiece.bits[i].y);
                        else
                            drawBuffer.DrawImageUnscaled(tetSImgs[heldPiece.id], x - 75 + 16 * heldPiece.bits[i].x, y - 50 + 16 * heldPiece.bits[i].y);
                }
            }

            //draw ice
            if (checkGimmick(4))
                drawBuffer.FillRectangle(new SolidBrush(Color.FromArgb(100, Color.Blue)), x, y + 275, width, 250);


            //GUI
            Color gravColor, gravMeter;
            switch (ruleset.gameRules)
            {
                case 1:
                    gravColor = Color.White;
                    gravMeter = Color.Teal;
                    break;
                case 2:
                case 3:
                    gravColor = Color.Green;
                    gravMeter = Color.Orange;
                    break;
                default:
                    gravColor = Color.White;
                    gravMeter = Color.Orange;
                    break;

            }//////ADJUST AFTER HERE

            if (ruleset.gameRules < 6) //grav meter
            {
                drawBuffer.FillRectangle(new SolidBrush(gravColor), x + 280, 505, 60, 8);
                drawBuffer.FillRectangle(new SolidBrush(gravMeter), x + 280, 505, (int)Math.Round(((double)gravTable[gravLevel] * 60) / ((Math.Pow(256, ruleset.gravType + 1) * 20))), 8);
                if (ruleset.g20 == true || gravTable[gravLevel] == Math.Pow(256, ruleset.gravType + 1) * 20)
                    drawBuffer.FillRectangle(new SolidBrush(Color.Red), x + 280, 505, 60, 8);
            }

            //SMALL TEXT
            //levels
            drawBuffer.DrawString(level.ToString(), f_Maestro, textBrush, x + 290, 485);
            if (ruleset.gameRules < 6)
            {
                if (ruleset.sections.Count == curSection)
                    drawBuffer.DrawString(ruleset.sections[curSection - 1].ToString(), f_Maestro, textBrush, x + 290, 525);
                else
                    drawBuffer.DrawString(ruleset.sections[curSection].ToString(), f_Maestro, textBrush, x + 290, 525);
            }

            drawBuffer.DrawString(score.ToString(), f_Maestro, textBrush, x + 280, 280);
            if (ruleset.gameRules == 1 && ruleset.id == 0)
            {
                drawBuffer.DrawString("NEXT GRADE:", f_Maestro, textBrush, x + 280, 140);
                if (grade != ruleset.gradePointsTGM1.Count)
                    drawBuffer.DrawString(ruleset.gradePointsTGM1[grade + 1].ToString(), f_Maestro, textBrush, x + 280, 160);
                else
                    drawBuffer.DrawString("??????", f_Maestro, textBrush, x + 280, 160);
            }

            if (godmode)
                drawBuffer.DrawString("GODMODE", f_Maestro, new SolidBrush(Color.Orange), 20, 680);
            if (g0)
                drawBuffer.DrawString("0G MODE", f_Maestro, new SolidBrush(Color.Orange), 20, 700);
            //if (mode.bigmode)
                //drawBuffer.DrawString("BIG MODE", f_Maestro, new SolidBrush(Color.Orange), 20, 720);

            //BIGGER TEXT
            //if (ruleset.gameRules == 1 && mode.id == 0 )
                drawBuffer.DrawString("POINTS:", f_Maestro, textBrush, x + 280, 260);

            //drawBuffer.DrawString(gradePoints.ToString(), SystemFonts.DefaultFont, textBrush, 20, 280);

            string cTex = "REGRET!";
            if (ruleset.gameRules == 4 && coolTime.elapsedTime > 0)
            {
                if (level % 100 >= 70)//cool
                {
                    cTex = "COOL!";
                }
                drawBuffer.DrawString(cTex, SystemFonts.DefaultFont, textBrush, x + 300, 350);
            }

            if (bravoTime.elapsedTime > 0)
                drawBuffer.DrawString("BRAVO! X" + bravoCounter, SystemFonts.DefaultFont, textBrush, x + 280, 400);

            if (ruleset.gameRules == 6)
                drawBuffer.DrawString("LEVEL:", f_Maestro, textBrush, x + 280, 465);
            
            if (ruleset.limitType == 3)//time limit?
                drawBuffer.DrawString(convertTime((long)((ruleset.limit - timer.elapsedTime) * ruleset.FPS / 60)), SystemFonts.DefaultFont, textBrush, x + 290, 550);
            else
                drawBuffer.DrawString(convertTime((long)(timer.elapsedTime * ruleset.FPS / 60)), SystemFonts.DefaultFont, textBrush, x + 290, 550);

            //GRADE TEXT
            if (ruleset.showGrade)
            {
                drawGrade(drawBuffer);
            }

            if (ruleset.exam != -1)
                drawBuffer.DrawString("EXAM: " + ruleset.grades[ruleset.exam].ToString(), f_Maestro, textBrush, x + 280, 100);


            //DRAW MEDALS
            if (ruleset.gameRules != 1)
                for (int i = 0; i < 6; i++)
                {
                    if (medals[i] != 0)
                        drawBuffer.DrawImage(medalImg, new Rectangle(x + 280 + (i % 3) * 20, 190 + (30 * (int)(Math.Floor((double)i / 3))), 25, 15), i * 26, (medals[i] - 1) * 16, 25, 15, GraphicsUnit.Pixel);
                }

            //fadeout
            if (fadeout > 22)
            {
                drawBuffer.FillRectangle(new SolidBrush(Color.FromArgb((fadeout - 22) * 3, Color.Black)), x, y + 25, width, height);
            }



            //Starting things
            if (starting == 2)
                drawBuffer.DrawString("Ready", SystemFonts.DefaultFont, new SolidBrush(Color.White), 200, 200);
            if (starting == 3)
                drawBuffer.DrawString("Go", SystemFonts.DefaultFont, new SolidBrush(Color.White), 200, 200);


            //endgame stats
            if (gameRunning == false && fadeout == 92)
            {
                if (ruleset.id == 0)
                {
                    drawBuffer.DrawString("Grade: " + ruleset.grades[results.grade], SystemFonts.DefaultFont, new SolidBrush(Color.White), x + 80, 200);
                }
                drawBuffer.DrawString("Score: " + results.score, SystemFonts.DefaultFont, new SolidBrush(Color.White), x + 80, 210);
                drawBuffer.DrawString("Time: " + convertTime(results.time), SystemFonts.DefaultFont, new SolidBrush(Color.White), x + 80, 220);
                drawBuffer.DrawString("Name: " + results.username, SystemFonts.DefaultFont, new SolidBrush(Color.White), x + 80, 230);
                if (results.username == "CHEATER")
                {
                    throw new DivideByZeroException();
                }

                if (torikan)
                {
                    drawBuffer.DrawString("Torikan hit!", SystemFonts.DefaultFont, new SolidBrush(Color.White), x + 80, 250);
                    drawBuffer.DrawString(convertTime((long)(torDef * ruleset.FPS / 60)) + " behind!", SystemFonts.DefaultFont, new SolidBrush(Color.White), x + 80, 260);
                }

                drawBuffer.DrawString("Press start to", SystemFonts.DefaultFont, new SolidBrush(Color.White), x + 80, 280);
                drawBuffer.DrawString("restart the field!", SystemFonts.DefaultFont, new SolidBrush(Color.White), x + 80, 290);

                drawBuffer.DrawString("Press B to", SystemFonts.DefaultFont, new SolidBrush(Color.White), x + 80, 310);
                drawBuffer.DrawString("return to menu!", SystemFonts.DefaultFont, new SolidBrush(Color.White), x + 80, 320);
            }
            
        }

        public void logic()
        {
            if (startTime.elapsedTime > 1000 && starting == 1)
            {
                starting = 2;
                //play READY
                Audio.playSound(s_Ready);
            }
            if (startTime.elapsedTime > 2000 && starting == 2)
            {
                starting = 3;
                //play GO
                Audio.playSound(s_Go);
            }
            if (startTime.elapsedTime > 3000 && starting == 3)
            {
                starting = 0;
                timer.start();
            }

            if (starting == 0)
            {


                if (gameRunning == true)
                {
                    //timing logic
                    long temptimeVAR = (long)(timer.elapsedTime * ruleset.FPS / 60);
                    
                    if (coolTime.elapsedTime > 3000)
                    {
                        coolTime.stop();
                        coolTime.reset();
                    }
                    if (bravoTime.elapsedTime > 1000)
                    {
                        bravoTime.stop();
                        bravoTime.reset();
                    }
                    justSpawned = false;
                    if (ruleset.limit - timer.elapsedTime <= 0 && ruleset.limitType == 3)
                        endGame();

                    //vanishing logic
                    int vpcount = 0;
                    List<int> remCell = new List<int>();
                    foreach (var vP in vanList)
                    {
                        if (vP.time + ruleset.FPS * 5 <= creditsProgress)
                        {
                            gameField[vP.x][vP.y] = 8;
                            remCell.Add(vpcount);
                        }
                        vpcount++;
                    }
                    for (int i = 0; i < remCell.Count; i++ )
                    {
                        vanList.RemoveAt(remCell[remCell.Count - i - 1]);
                    }

                    List<int> remFlash = new List<int>();
                    vpcount = 0;
                    foreach (var fP in flashList)
                    {
                        if (fP.time == 0)
                            remFlash.Add(vpcount);
                        vpcount++;
                    }
                    for (int i = 0; i < remFlash.Count; i++)
                    {
                        flashList.RemoveAt(remFlash[remFlash.Count - i - 1]);
                    }

                    for (int i = 0; i < flashList.Count; i++ )
                    {
                        var fP = flashList[i];
                        fP.time -= 1;
                        flashList[i] = fP;
                    }


                        //check inputs and handle logic pertaining to them

                    if (ruleset.gameRules == 6 && pad.inputPressedRot3 == true)
                        inputDelayH = 0;
                    else if (pad.inputH == 1 || pad.inputH == -1)
                    {
                        if (inputDelayH > 0)
                        {
                            inputDelayH--;
                        }
                        if (inputDelayH == -1 || inputDelayDir != pad.inputH)
                            inputDelayH = ruleset.baseDAS;
                    }
                    else
                        inputDelayH = -1;

                    inputDelayDir = pad.inputH;

                    if(creditsProgress == 0 &&((ruleset.gameRules > 1) == (creditsPause.elapsedTime >= 3000)) && inCredits)
                    {
                        if (creditsType < 2)
                            Audio.playMusic("crdtvanish");
                        else
                            Audio.playMusic("crdtinvis");
                    }

                    if (inCredits && ((ruleset.gameRules > 1) == (creditsPause.elapsedTime >= 3000)))
                    {
                        creditsProgress++;
                        if (pad.inputStart == 1 && ruleset.gameRules == 1)
                            creditsProgress += 3;
                    }

                    gradeTime++;
                    if (gradeTime > ruleset.decayRate[grade] && gradePoints != 0 && !comboing && !inCredits)
                    {
                        gradeTime = 0;
                        gradePoints--;
                    }

                    //GAME LOGIC

                    //check ID of current tetromino.
                    if (activeTet.id == 0)
                    {
                        if (currentTimer == (int)Field.timerType.LineClear)  //if timer is line clear and done, settle pieces and start ARE
                        {
                            if (timerCount == 0)
                            {
                                //settle pieces and start ARE
                                for (int i = 0; i < full.Count; i++)
                                {
                                    for (int j = full[i]; j > 0; j--)
                                    {
                                        for (int k = 0; k < 10; k++)
                                        {
                                            gameField[k][j] = gameField[k][j - 1];
                                        }
                                        for (int c = 0; c < vanList.Count; c++ )
                                        {
                                            if (vanList[c].y == j)
                                            {
                                                var vP = new vanPip();
                                                vP.time = vanList[c].time;
                                                vP.x = vanList[c].x;
                                                vP.y = vanList[c].y + 1;
                                                vanList[c] = vP;
                                            }
                                        }
                                        for (int d = 0; d < flashList.Count; d++)//update flash list, juuust in case of an instant lineclear
                                        {
                                            if (flashList[d].y == j)
                                            {
                                                var vP = new flashPip();
                                                vP.time = flashList[d].time;
                                                vP.x = flashList[d].x;
                                                vP.y = flashList[d].y + 1;
                                                flashList[d] = vP;
                                            }
                                        }
                                    }
                                    for (int k = 0; k < 10; k++)
                                    {
                                        gameField[k][0] = 0;
                                    }
                                }
                                
                                full.Clear();
                                currentTimer = (int)Field.timerType.ARE;
                                if (ruleset.gameRules >= 3)
                                    timerCount = ruleset.baseARELine;
                                else
                                    timerCount = ruleset.baseARE;
                                Audio.playSound(s_Impact);

                                if (checkGimmick(2) && garbTimer >= ruleset.garbAmt)
                                {
                                    raiseGarbage(1);
                                    garbTimer = 0;
                                }
                            }
                            else
                            {
                                timerCount--;
                                return;
                            }
                        }
                        //elseif timer is ARE and done, get next tetromino
                        else if (currentTimer == (int)Field.timerType.ARE)
                        {
                            if (timerCount <= 0 && ((inCredits == (creditsPause.elapsedTime > 3000)) || ruleset.gameRules == 1))
                            {
                                swappedHeld = 0;
                                spawnPiece();
                            }
                            else
                            {
                                timerCount--;
                                return;
                            }
                        }
                    }
                    if (activeTet.id != 0)//else, check collision below
                    {

                        activeTet.floored = false;
                        tetLife++;

                        if (activeTet.id != 0)
                        {
                            int big = 0;
                            if (ruleset.bigmode)
                                big = 1;
                            for (int i = 0; i < activeTet.bits.Count; i++)
                            {
                                if (activeTet.bits[i].y  < 0)
                                    continue;
                                if (activeTet.bits[i].y + big + 1 >= 22)
                                {
                                    activeTet.floored = true;
                                    break;
                                }
                                else if (gameField[activeTet.bits[i].x][activeTet.bits[i].y + big + 1] != 0)
                                {
                                    activeTet.floored = true;
                                    break;
                                }
                            }
                        }

                        if (activeTet.floored == true)
                        {
                            //check lock delay if grounded
                            if (currentTimer == (int)Field.timerType.LockDelay)
                            {
                                //if lock delay up, place piece.
                                if (activeTet.groundTimer == 0)
                                {

                                    safelock = true;

                                    //GIMMICKS

                                    

                                    if (checkGimmick(2))
                                        garbTimer += 1;

                                    
                                    if (inCredits == true && creditsProgress >= ruleset.creditsLength)
                                    {
                                        endGame();
                                    }

                                    int lowY = 22;
                                    for (int i = 0; i < activeTet.bits.Count; i++)
                                    {
                                        if (lowY > activeTet.bits[i].y)
                                            lowY = activeTet.bits[i].y;
                                    }

                                    for (int i = 0; i < activeTet.bits.Count; i++) //SET THE PIECES
                                    {
                                        int big = 2;
                                        if (ruleset.bigmode)
                                            big = 1;

                                        for (int j = 0; j < (big % 2) + 1; j++ )
                                        {
                                            for (int k = 0; k < (big % 2) + 1; k++)
                                            {
                                                if (activeTet.bits[i].y + k < 0)
                                                    continue;
                                                if (checkGimmick(1) || creditsType == 2)
                                                    gameField[activeTet.bits[i].x  + j][activeTet.bits[i].y + k] = 8;
                                                else if (activeTet.bone == true)
                                                    gameField[activeTet.bits[i].x + j][activeTet.bits[i].y + k] = 10;
                                                else
                                                    gameField[activeTet.bits[i].x + j][activeTet.bits[i].y + k] = activeTet.id;

                                                if (creditsType == 1)
                                                {
                                                    vanPip vP = new vanPip();
                                                    vP.time = creditsProgress;
                                                    vP.x = activeTet.bits[i].x + j;
                                                    vP.y = activeTet.bits[i].y + k;
                                                    vanList.Add(vP);
                                                }

                                                flashPip fP = new flashPip();
                                                fP.time = 2;
                                                fP.x = activeTet.bits[i].x  + j;
                                                fP.y = activeTet.bits[i].y  + k;
                                                flashList.Add(fP);
                                            }
                                        }
                                    }
                                    activeTet.id = 0;
                                    //check for full rows and screenclears

                                    int tetCount = 0;

                                    for (int i = 0; i < 22; i++)
                                    {
                                        int columnCount = 0;
                                        for (int j = 0; j < 10; j++)
                                        {
                                            if (gameField[j][i] != 0)
                                            {
                                                columnCount++;
                                                tetCount++;
                                            }
                                        }
                                        if (columnCount == 10)
                                        {
                                            if (!checkGimmick(4) || i < 11)
                                            {
                                                full.Add(i);
                                                tetCount -= 10;
                                                //clear these from vanishing list
                                                int count = 0;
                                                List<int> remcell = new List<int>();
                                                foreach (var vP in vanList)
                                                {
                                                    if (vP.y == i)
                                                        remcell.Add(count);
                                                    count++;
                                                }
                                                for (int c = 0; c < remcell.Count; c++ )
                                                {
                                                    vanList.RemoveAt(remcell[remcell.Count - c - 1]);
                                                }

                                                remcell = new List<int>();
                                                foreach (var vP in flashList)
                                                {
                                                    if (vP.y == i)
                                                        remcell.Add(count);
                                                    count++;
                                                }
                                                for (int c = 0; c < remcell.Count; c++)
                                                {
                                                    flashList.RemoveAt(remcell[remcell.Count - c - 1]);
                                                }
                                            }
                                        }
                                    }

                                    if (tetCount >= 150 && ruleset.gameRules > 4)
                                        recoverChecking = true;

                                    if (full.Count > 0)  //if full rows, clear the rows, start the line clear timer, give points
                                    {
                                        int bigFull = full.Count;
                                        int oldLvl = level;
                                        if (ruleset.bigmode)
                                            bigFull = bigFull / 2;
                                        for (int i = 0; i < full.Count; i++)
                                        {
                                            for (int j = 0; j < 10; j++)
                                                gameField[j][full[i]] = 0;
                                        }
                                        if (ruleset.id != 6)
                                            for (int i = 0; i < bigFull; i++)//1234 for tgm1 - tap, 1246 in tgm3, 1236 in tgm4
                                            {
                                                level++;
                                                if (ruleset.gameRules == 4 && i > 2)
                                                    level++;
                                                if (ruleset.gameRules > 3 && i > 3)
                                                    level++;
                                                if (ruleset.gameRules > 4 && i > 3)
                                                    level++;
                                            }
                                        else
                                        {
                                            for (int i = 0; i < full.Count; i++)
                                                level++;
                                            if (full.Count > 6)
                                                level += 4;
                                        }

                                        if (level > ruleset.endLevel && ruleset.endLevel != 0)
                                            level = ruleset.endLevel;

                                        //garbage!
                                        garbTimer -= bigFull;
                                        if (garbTimer < 0) garbTimer = 0;

                                        //calculate combo!

                                        if (bigFull == 4)
                                        {
                                            tetrises++;
                                            secTet[curSection]++;
                                            Audio.playSound(s_Tetris);
                                        }

                                        int bravo = 1;
                                        if (tetCount == 0)
                                        {
                                            bravoCounter++;
                                            bravo = 4;
                                            bravoTime.start();
                                        }
                                        //give points
                                        
                                            combo = combo + (2 * bigFull) - 2;
                                        if (!inCredits && ruleset.gradedBy == 1)
                                        {
                                            int newscore = 0;
                                            int sped = ruleset.baseLock - tetLife;
                                            if (sped < 0) sped = 0;
                                            if (softCounter > 20)
                                                softCounter = 20;
                                            if (ruleset.gameRules == 1)
                                                newscore = ((int)Math.Ceiling((double)(oldLvl + bigFull) / 4) + softCounter) * bigFull * combo * bravo;
                                            if (ruleset.gameRules == 2)
                                                newscore = ((int)Math.Ceiling((double)(oldLvl + bigFull) / 4) + softCounter + (2*sonicCounter)) * bigFull * combo * bravo;
                                            if (ruleset.gameRules == 3)
                                                newscore = ((int)Math.Ceiling((double)(oldLvl + bigFull) / 4) + softCounter + (2 * sonicCounter)) * bigFull * combo * bravo + (int)(Math.Ceiling((double)level/2)) + (sped*7);
                                            if (ruleset.gameRules == 4)
                                                newscore = ((int)Math.Ceiling((double)(oldLvl + bigFull) / 4) + softCounter) * bigFull * combo + (int)(Math.Ceiling((double)level/2)) + sped;

                                            score += newscore;
                                        }

                                        if (ruleset.gameRules != 1)
                                        {
                                            if (ruleset.id == 0)
                                            {
                                                if (bigFull > 1)
                                                {
                                                    gradeCombo++;
                                                }
                                                if (ruleset.gradedBy == 1)
                                                {
                                                    if (!inCredits)
                                                    {
                                                        int comboval = gradeCombo;
                                                        int comborow = bigFull - 1;

                                                        if (ruleset.gameRules > 3)
                                                        {
                                                            comborow = (comborow + 3) % 4;
                                                        }
                                                        if (comboval > 10) comboval = 10;

                                                        int newPts = (int)(Math.Ceiling(ruleset.baseGradePts[bigFull - 1][grade] * ruleset.comboTable[bigFull - 1][gradeCombo]) * Math.Ceiling((double)level / 250));
                                                        if (level > 249 && level < 500)
                                                            newPts = newPts * 2;
                                                        if (level > 499 && level < 750)
                                                            newPts = newPts * 3;
                                                        if (level > 749 && level < 1000)
                                                            newPts = newPts * 4;

                                                        gradePoints += newPts;
                                                    }
                                                    else if (ruleset.gameRules == 4)
                                                    {
                                                        if (creditsType == 1)
                                                            switch (bigFull)
                                                            {
                                                                case 0:
                                                                    break;
                                                                case 1:
                                                                    creditGrades += 4;
                                                                    break;
                                                                case 2:
                                                                    creditGrades += 8;
                                                                    break;
                                                                case 3:
                                                                    creditGrades += 12;
                                                                    break;
                                                                case 4:
                                                                    creditGrades += 26;
                                                                    break;
                                                            }
                                                        else
                                                            switch (bigFull)
                                                            {
                                                                case 0:
                                                                    break;
                                                                case 1:
                                                                    creditGrades += 10;
                                                                    break;
                                                                case 2:
                                                                    creditGrades += 20;
                                                                    break;
                                                                case 3:
                                                                    creditGrades += 30;
                                                                    break;
                                                                case 4:
                                                                    creditGrades += 100;
                                                                    break;
                                                            }
                                                    }

                                                    if (comboing)
                                                        Audio.playSound(s_Combo);
                                                }
                                            }
                                            if (ruleset.id == 1)
                                            {
                                                if (level >= 500)
                                                    gradeLevel = 27;
                                                if (level >= 999)
                                                    gradeLevel = 32;
                                            }

                                        }
                                        comboing = true;

                                        //check GM conditions
                                        long temptime = timer.elapsedTime;


                                        if (ruleset.id == 6)
                                        {
                                            if (level < 1000)
                                            {
                                                ruleset.limit += 1000 * (int)(Math.Floor((double)bigFull / 3));
                                            }
                                            if (bravo == 4)
                                            {
                                                if (level < 1000)
                                                {
                                                    if (bigFull == 1)
                                                        ruleset.limit += 1000 * 5;
                                                    if (bigFull == 1)
                                                        ruleset.limit += 1000 * 8;
                                                    if (bigFull == 1)
                                                        ruleset.limit += 1000 * 10;
                                                    if (bigFull == 1)
                                                        ruleset.limit += 1000 * 15;
                                                }
                                                else
                                                {
                                                    if (bigFull == 1)
                                                        ruleset.limit += 1000 * 1;
                                                    if (bigFull == 1)
                                                        ruleset.limit += 1000 * 2;
                                                    if (bigFull == 1)
                                                        ruleset.limit += 1000 * 3;
                                                }
                                            }
                                        }
                                        

                                        //update grade

                                        if (ruleset.gameRules == 1)
                                        {
                                            bool checking = true;
                                            while (checking == true)
                                            {

                                                if (grade < ruleset.gradePointsTGM1.Count - 1)
                                                {
                                                    if (score >= ruleset.gradePointsTGM1[grade + 1])
                                                    {
                                                        grade++;
                                                        Audio.playSound(s_Grade);
                                                        masterTime = timer.elapsedTime;
                                                    }
                                                    else
                                                        checking = false;
                                                }
                                                else
                                                    checking = false;

                                            }
                                        }
                                        else
                                        {
                                            if (gradePoints > 99)
                                            {
                                                if (grade < ruleset.gradeIntTGM2.Count - 1)
                                                {
                                                    grade++;
                                                    gradePoints = 0;
                                                    if (ruleset.gradeIntTGM2[grade] != gm2grade)
                                                    {
                                                        gm2grade++;
                                                        if (ruleset.gameRules < 4)
                                                            Audio.playSound(s_Grade);
                                                    }
                                                }
                                            }
                                        }

                                        if (ruleset.sections.Count != curSection)
                                            if (level >= ruleset.sections[curSection])
                                            {
                                                curSection++;
                                                secTet.Add(0);

                                                if (ruleset.gameRules > 3)
                                                    Audio.playSound(s_Section);

                                                //CHECK TORIKANS
                                                if (ruleset.id == 1) //death
                                                {
                                                    if (curSection == 5 && temptime > 205000)
                                                    {
                                                        level = 500;
                                                        torikan = true;
                                                        torDef = temptime - 205000;
                                                        endGame();
                                                        return;
                                                    }
                                                }
                                                if (ruleset.gameRules == 4)
                                                {
                                                    if (ruleset.id == 0 && curSection == 5 && temptime > 420000 && ruleset.exam == -1) //ti master
                                                    {
                                                        level = 500;
                                                        torikan = true;
                                                        torDef = temptime - 420000;
                                                        endGame();
                                                        return;
                                                    }
                                                    if (ruleset.id == 2) //shirase
                                                    {
                                                        if (curSection == 5 && temptime > 148000)
                                                        {
                                                            level = 500;
                                                            torikan = true;
                                                            torDef = temptime - 148000;
                                                            endGame();
                                                            return;
                                                        }
                                                        if (curSection == 10 && temptime > 296000)
                                                        {
                                                            level = 1000;
                                                            torikan = true;
                                                            torDef = temptime - 296000;
                                                            endGame();
                                                            return;
                                                        }
                                                    }
                                                }

                                                //MUSIC
                                                updateMusic();

                                                //DELAYS
                                                int num = curSection + speedBonus - 4;
                                                if (num > 0 && num < ruleset.delayTable[0].Count)
                                                {
                                                    ruleset.baseARE = ruleset.delayTable[0][num];
                                                    ruleset.baseARELine = ruleset.delayTable[1][num];
                                                    ruleset.baseDAS = ruleset.delayTable[2][num];
                                                    ruleset.baseLock = ruleset.delayTable[3][num];
                                                    ruleset.baseLineClear = ruleset.delayTable[4][num];
                                                }
                                                
                                                

                                                //MEDALS
                                                if (ruleset.gameRules != 1 && ruleset.id != 6)
                                                {
                                                    int sectime = 90000;
                                                    if (ruleset.id == 1 || ruleset.id == 2)
                                                        sectime = 42000;
                                                    if (sectionTime.elapsedTime < sectime - 10000 && medals[2] < 1)
                                                    {
                                                        medals[2] = 1;
                                                        Audio.playSound(s_Medal);
                                                    }
                                                    if (sectionTime.elapsedTime < sectime - 5000 && medals[2] < 2)
                                                    {
                                                        medals[2] = 2;
                                                        Audio.playSound(s_Medal);
                                                    }
                                                    if (sectionTime.elapsedTime < sectime && medals[2] < 3)
                                                    {
                                                        medals[2] = 3;
                                                        Audio.playSound(s_Medal);
                                                    }

                                                    if (curSection == 3 && ruleset.gameRules < 4)
                                                        if ((rotations * 5) / (totalTets * 6) >= 1)
                                                        {
                                                            medals[1] = 1;
                                                            rotations = 0;
                                                            totalTets = 0;
                                                            Audio.playSound(s_Medal);
                                                        }
                                                    if (curSection == 7 && ruleset.gameRules < 4)
                                                        if ((rotations * 5) / (totalTets * 6) >= 1)
                                                        {
                                                            medals[1] = 2;
                                                            rotations = 0;
                                                            totalTets = 0;
                                                            Audio.playSound(s_Medal);
                                                        }


                                                    sectionTimes.Add(sectionTime.elapsedTime);

                                                    //check Regrets!
                                                    if (ruleset.gameRules == 4 && ruleset.id == 0)
                                                    {
                                                        if (sectionTime.elapsedTime > ruleset.secRegrets[curSection - 1])
                                                        {
                                                            if (curSection != 9)
                                                                secCools[curSection - 1] = -1;
                                                            else
                                                                secCools.Add(-1);

                                                            Audio.playSound(s_Regret);
                                                            coolTime.start();
                                                        }
                                                        if (secCools[curSection - 1] == 1)
                                                            speedBonus += 1;
                                                    }
                                                    sectionTime.stop();

                                                    sectionTime.start();
                                                }
                                            }

                                        //check medal conditions!!
                                        if (ruleset.gameRules != 1)
                                        {
                                            //AC
                                            if (bravoCounter == 1 && medals[0] == 0)
                                            {
                                                medals[0] = 1;
                                                Audio.playSound(s_Medal);
                                            }
                                            if (bravoCounter == 2 && medals[0] == 1)
                                            {
                                                medals[0] = 2;
                                                Audio.playSound(s_Medal);
                                            }
                                            if (bravoCounter == 3 && medals[0] == 2)
                                            {
                                                medals[0] = 3;
                                                Audio.playSound(s_Medal);
                                            }

                                            //RO
                                            
                                            //ST


                                            //SK
                                            int tt = 1;
                                            if (ruleset.id == 1 || ruleset.id == 2)
                                                tt = 2;
                                            if (ruleset.bigmode == true)
                                                tt = 10;
                                            if (tetrises == (int)(Math.Ceiling((double)10 / tt)) && medals[3] == 0)
                                            {
                                                medals[3] = 1;
                                                Audio.playSound(s_Medal);
                                            }

                                            //RE
                                            if (recoverChecking == true && tetCount <= 70)
                                                recoveries++;
                                            if (recoveries == 1 && medals[4] == 0)
                                            {
                                                medals[4] = 1;
                                                Audio.playSound(s_Medal);
                                            }
                                            if (recoveries == 2 && medals[4] == 0)
                                            {
                                                medals[4] = 2;
                                                Audio.playSound(s_Medal);
                                            }
                                            if (recoveries == 4 && medals[4] == 0)
                                            {
                                                medals[4] = 3;
                                                Audio.playSound(s_Medal);
                                            }
                                            //CO
                                            int big = 1;
                                            if (ruleset.bigmode == true)
                                                big = 2;
                                            if ((int)(Math.Ceiling((double)4 / big)) == gradeCombo)
                                            {
                                                medals[5] = 1;
                                                Audio.playSound(s_Medal);
                                            }
                                            if ((int)(Math.Ceiling((double)5 / big)) == gradeCombo)
                                            {
                                                medals[5] = 2;
                                                Audio.playSound(s_Medal);
                                            }
                                            if ((int)(Math.Ceiling((double)7 / big)) == gradeCombo)
                                            {
                                                medals[5] = 3;
                                                Audio.playSound(s_Medal);
                                            }
                                            
                                        }


                                        //GM FLAG CHECKS
                                        switch (ruleset.gameRules)
                                        {
                                            case 1:
                                                if (GMflags.Count == 0 && level >= 300)
                                                {
                                                    if (score >= 12000 && temptime <= 255000)
                                                        GMflags.Add(true);
                                                    else
                                                        GMflags.Add(false);
                                                }
                                                else if (GMflags.Count == 1 && level >= 500)
                                                {
                                                    if (score >= 40000 && temptime <= 450000)
                                                        GMflags.Add(true);
                                                    else
                                                        GMflags.Add(false);
                                                }
                                                else if (GMflags.Count == 2 && level >= ruleset.endLevel)
                                                {
                                                    level = 999;
                                                    if (score >= 126000 && temptime <= 810000)
                                                        GMflags.Add(true);
                                                    else
                                                        GMflags.Add(false);


                                                    //check for awarding GM
                                                    if (GMflags[0] && GMflags[1] && GMflags[2])
                                                    {
                                                        isGM = true;
                                                    }
                                                }
                                                break;
                                            case 2:
                                                if (GMflags.Count == 0 && level >= 100)
                                                {
                                                    if (secTet[0] > 0 && temptime <= 90000)
                                                        GMflags.Add(true);
                                                    else
                                                        GMflags.Add(false);
                                                }
                                                else if (GMflags.Count == 1 && level >= 200)
                                                {
                                                    if (secTet[1] > 0)
                                                        GMflags.Add(true);
                                                    else
                                                        GMflags.Add(false);
                                                }
                                                else if (GMflags.Count == 2 && level >= 300)
                                                {
                                                    if (secTet[2] > 0)
                                                        GMflags.Add(true);
                                                    else
                                                        GMflags.Add(false);
                                                }
                                                else if (GMflags.Count == 3 && level >= 400)
                                                {
                                                    if (secTet[3] > 0)
                                                        GMflags.Add(true);
                                                    else
                                                        GMflags.Add(false);
                                                }
                                                else if (GMflags.Count == 4 && level >= 500)
                                                {
                                                    if (secTet[4] > 0)
                                                        GMflags.Add(true);
                                                    else
                                                        GMflags.Add(false);

                                                    if (temptime <= 360000)
                                                        GMflags.Add(true);
                                                    else
                                                        GMflags.Add(false);
                                                }
                                                else if (GMflags.Count == 6 && level >= 600)
                                                {
                                                    if (secTet[5] > 0 && sectionTimes[5] > (sectionTimes[0] + sectionTimes[1] + sectionTimes[2] + sectionTimes[3] + sectionTimes[4]) / 5)
                                                        GMflags.Add(true);
                                                    else
                                                        GMflags.Add(false);
                                                }
                                                else if (GMflags.Count == 7 && level >= 700)
                                                {
                                                    if (secTet[6] > 0 && sectionTimes[6] > (sectionTimes[0] + sectionTimes[1] + sectionTimes[2] + sectionTimes[3] + sectionTimes[4]) / 5)
                                                        GMflags.Add(true);
                                                    else
                                                        GMflags.Add(false);
                                                }
                                                else if (GMflags.Count == 8 && level >= 800)
                                                {
                                                    if (secTet[7] > 0 && sectionTimes[7] > (sectionTimes[0] + sectionTimes[1] + sectionTimes[2] + sectionTimes[3] + sectionTimes[4]) / 5)
                                                        GMflags.Add(true);
                                                    else
                                                        GMflags.Add(false);
                                                }
                                                else if (GMflags.Count == 9 && level >= 900)
                                                {
                                                    if (secTet[8] > 0 && sectionTimes[8] > (sectionTimes[0] + sectionTimes[1] + sectionTimes[2] + sectionTimes[3] + sectionTimes[4]) / 5)
                                                        GMflags.Add(true);
                                                    else
                                                        GMflags.Add(false);
                                                }
                                                else if (GMflags.Count == 10 && level == 999)
                                                {
                                                    if (sectionTime.elapsedTime <= 45000 && temptime <= 570000 && gm2grade == 17)
                                                        GMflags.Add(true);
                                                    else
                                                        GMflags.Add(false);
                                                }
                                                break;
                                            case 3: //add tap gm flags
                                                if (GMflags.Count == 0 && level >= 100)
                                                {
                                                    if (secTet[0] > 1 && sectionTimes[0] <= 65000)
                                                        GMflags.Add(true);
                                                    else
                                                        GMflags.Add(false);
                                                }
                                                else if (GMflags.Count == 1 && level >= 200)
                                                {
                                                    if (secTet[1] > 1 && sectionTimes[1] <= 65000)
                                                        GMflags.Add(true);
                                                    else
                                                        GMflags.Add(false);
                                                }
                                                else if (GMflags.Count == 2 && level >= 300)
                                                {
                                                    if (secTet[2] > 1 && sectionTimes[2] <= 65000)
                                                        GMflags.Add(true);
                                                    else
                                                        GMflags.Add(false);
                                                }
                                                else if (GMflags.Count == 3 && level >= 400)
                                                {
                                                    if (secTet[3] > 1 && sectionTimes[3] <= 65000)
                                                        GMflags.Add(true);
                                                    else
                                                        GMflags.Add(false);
                                                }
                                                else if (GMflags.Count == 4 && level >= 500)
                                                {
                                                    if (secTet[4] > 1 && sectionTimes[4] <= 65000)
                                                        GMflags.Add(true);
                                                    else
                                                        GMflags.Add(false);
                                                }
                                                else if (GMflags.Count == 5 && level >= 600)
                                                {
                                                    if (secTet[5] > 0 && sectionTimes[5] <= ((sectionTimes[0] + sectionTimes[1] + sectionTimes[2] + sectionTimes[3] + sectionTimes[4]) / 5) + 2000)
                                                        GMflags.Add(true);
                                                    else
                                                        GMflags.Add(false);
                                                }
                                                else if (GMflags.Count == 6 && level >= 700)
                                                {
                                                    if (secTet[6] > 0 && sectionTimes[6] <= sectionTimes[5] + 2000)
                                                        GMflags.Add(true);
                                                    else
                                                        GMflags.Add(false);
                                                }
                                                else if (GMflags.Count == 7 && level >= 800)
                                                {
                                                    if (secTet[7] > 0 && sectionTimes[7] <= sectionTimes[6] + 2000)
                                                        GMflags.Add(true);
                                                    else
                                                        GMflags.Add(false);
                                                }
                                                else if (GMflags.Count == 8 && level >= 900)
                                                {
                                                    if (secTet[8] > 0 && sectionTimes[8] <= sectionTimes[7] + 2000)
                                                        GMflags.Add(true);
                                                    else
                                                        GMflags.Add(false);
                                                }
                                                else if (GMflags.Count == 9 && level >= 999)
                                                {
                                                    if (temptime <= 525000 && sectionTimes[9] <= sectionTimes[8] + 2000 && gm2grade == 17)
                                                        GMflags.Add(true);
                                                    else
                                                        GMflags.Add(false);
                                                }
                                                break;
                                        }

                                        //update gimmicks
                                        bool thaw = false;
                                        if (ruleset.gimList.Count > 0)
                                        {
                                            if (ruleset.gimList.Count > gimIndex)
                                                if (ruleset.gimList[gimIndex].startLvl <= level)
                                                {
                                                    activeGim.Add(ruleset.gimList[gimIndex].type);
                                                    gimIndex++;
                                                }
                                            for (int i = 0; i < activeGim.Count; i++)
                                            {
                                                if (ruleset.gimList[gimIndex - activeGim.Count + i].endLvl <= level)
                                                {
                                                    if (ruleset.gimList[gimIndex - activeGim.Count + i].type == 4)
                                                        thaw = true;
                                                    activeGim.RemoveAt(i);

                                                }
                                            }
                                        }

                                        if (checkGimmick(5))
                                            ruleset.bigmode = true;

                                        //thaw ice
                                        if (thaw)
                                        {
                                            for (int i = 0; i < 22; i++)
                                            {
                                                int columnCount = 0;
                                                for (int j = 0; j < 10; j++)
                                                    if (gameField[j][i] != 0)
                                                        columnCount++;
                                                if (columnCount == 10)
                                                {
                                                    if (!checkGimmick(4) || i < 11)
                                                    {
                                                        full.Add(i);
                                                        //clear these from vanishing list
                                                        int count = 0;
                                                        List<int> remcell = new List<int>();
                                                        foreach (var vP in vanList)
                                                        {
                                                            if (vP.y == i)
                                                                remcell.Add(count);
                                                            count++;
                                                        }
                                                        for (int c = 0; c < remcell.Count; c++)
                                                        {
                                                            vanList.RemoveAt(remcell[remcell.Count - c - 1]);
                                                        }

                                                        remcell = new List<int>();
                                                        foreach (var vP in flashList)
                                                        {
                                                            if (vP.y == i)
                                                                remcell.Add(count);
                                                            count++;
                                                        }
                                                        for (int c = 0; c < remcell.Count; c++)
                                                        {
                                                            flashList.RemoveAt(remcell[remcell.Count - c - 1]);
                                                        }
                                                    }
                                                }
                                            }
                                            for (int i = 0; i < full.Count; i++)
                                            {
                                                for (int j = 0; j < 10; j++)
                                                    gameField[j][full[i]] = 0;
                                            }
                                            for (int i = 0; i < full.Count; i++)
                                            {
                                                for (int j = full[i]; j > 0; j--)
                                                {
                                                    for (int k = 0; k < 10; k++)
                                                    {
                                                        gameField[k][j] = gameField[k][j - 1];
                                                    }
                                                    for (int c = 0; c < vanList.Count; c++)
                                                    {
                                                        if (vanList[c].y == j)
                                                        {
                                                            var vP = new vanPip();
                                                            vP.time = vanList[c].time;
                                                            vP.x = vanList[c].x;
                                                            vP.y = vanList[c].y + 1;
                                                            vanList[c] = vP;
                                                        }
                                                    }
                                                }
                                                for (int k = 0; k < 10; k++)
                                                {
                                                    gameField[k][0] = 0;
                                                }
                                            }

                                            full.Clear();
                                        }


                                        //check finish condition
                                        if (ruleset.endLevel != 0 && level >= ruleset.endLevel && inCredits == false)
                                        {
                                            triggerCredits();
                                        }

                                        //start timer
                                        currentTimer = (int)Field.timerType.LineClear;
                                        timerCount = ruleset.baseLineClear;
                                        Audio.playSound(s_Clear);

                                    }
                                    else //start the ARE, check if new grav level
                                    {
                                        currentTimer = (int)Field.timerType.ARE;
                                        timerCount = ruleset.baseARE;

                                        combo = 1;
                                        gradeCombo = 1;
                                        comboing = false;

                                    }

                                    checkFade();
                                    

                                    //check level for section cool
                                    if (level % 100 > 69)
                                    {
                                        

                                        
                                        if (ruleset.gameRules == 4 && ruleset.id == 0)
                                        {

                                            if (secCools.Count <= curSection && curSection != 9)
                                            {
                                                if (curSection != 0)
                                                {
                                                    if ((secCools[curSection - 1] == 1 && sectionTime.elapsedTime < sectionTimes[curSection - 1] + 2000) || (secCools[curSection - 1] != 1 && sectionTime.elapsedTime < ruleset.secCools[curSection]))
                                                    {
                                                        secCools.Add(1);
                                                        Audio.playSound(s_Cool);
                                                        coolTime.start();
                                                    }
                                                    else
                                                        secCools.Add(0);
                                                }
                                                else
                                                {
                                                    if (sectionTime.elapsedTime < ruleset.secCools[curSection])
                                                    {
                                                        secCools.Add(1);
                                                        Audio.playSound(s_Cool);
                                                        coolTime.start();
                                                    }
                                                    else
                                                        secCools.Add(0);
                                                }
                                            }
                                        }
                                    }


                                    while (gravLevel < gravTable.Count - 1)
                                    {
                                        if (level + (speedBonus * 100) >= ruleset.gravLevels[gravLevel + 1])
                                            gravLevel++;
                                        else
                                            break;
                                    }

                                    if ((ruleset.g20 == true || gravTable[gravLevel] == Math.Pow(256, ruleset.gravType + 1) * 20) && ruleset.gameRules < 4)
                                        textBrush = new SolidBrush(Color.Gold);

                                    Audio.playSound(s_Contact);

                                    

                                    return;

                                }
                                else
                                {
                                    gravCounter = 0;
                                    activeTet.groundTimer--;
                                }
                            }
                            else
                            {
                                currentTimer = (int)Field.timerType.LockDelay;
                                Audio.playSound(s_Lock);
                                //timerCount = ruleset.baseLock;
                            }
                        }
                        else
                            currentTimer = (int)Field.timerType.ARE;



                        int blockDrop = 0;// make it here so we can drop faster


                        //check saved inputs and act on them accordingly

                        bool son = false;

                        if (pad.inputV == 1 && ruleset.hardDrop == 1)
                        {
                            blockDrop = 19;
                            gravCounter = 0;
                            son = true;
                        }
                        else if (pad.inputV == -1)
                        {
                            if (!activeTet.floored)
                            {
                                //blockDrop++;
                                softCounter++;
                            }
                            else if (!safelock || !(ruleset.gameRules > 3 || ruleset.id == 1 || (ruleset.gameRules == 2 && level > 899) || (ruleset.gameRules == 3 && level > 899)))
                            {
                                if(activeTet.floored)
                                    gravCounter = 0;
                                activeTet.groundTimer = 0;
                            }
                        }
                        else
                        {
                            safelock = false;
                        }

                        if (ruleset.gameRules == 6 && ruleset.hardDrop == 1 && pad.inputV == 1 && pad.inputPressedRot3 == true)
                        {
                            gravCounter = 0;
                            activeTet.groundTimer = 0;
                        }

                        if (pad.inputHold == 1 && ruleset.hold == true && swappedHeld == 0)
                        {
                            hold();
                        }

                        int rot;
                        if (ruleset.gameRules < 6)
                            rot = (pad.inputRot1 | pad.inputRot3) - pad.inputRot2;
                        else
                            rot = pad.inputRot1 - pad.inputRot2;

                        if (rot != 0)
                        {
                            //if (activeTet.kicked != 0)
                                //activeTet.groundTimer = 1;
                            rotatePiece(activeTet, rot, false);
                        }

                        if (!justSpawned)
                        {
                            int big = 0;
                            if (activeTet.big)
                                big = 1;
                            if (pad.inputH != 0 && (inputDelayH < 1 || inputDelayH == ruleset.baseDAS))
                            {
                                bool safe = true;

                                //check to the input direction of each bit
                                for (int i = 0; i < activeTet.bits.Count; i++)
                                {
                                    if (activeTet.bits[i].y < 0)
                                        continue;
                                    if (Math.Abs(activeTet.bits[i].x + ((big + 1) * ((pad.inputH + 1) / 2)) - 5) > 5 - ((ruleset.bigMove * big) + 1 - big))
                                    {
                                        safe = false;
                                        break;
                                    }
                                    if (gameField[activeTet.bits[i].x + (big * ((pad.inputH + 1) / 2)) + (pad.inputH* ((ruleset.bigMove * big) + 1 - big))][activeTet.bits[i].y] != 0)
                                    {
                                        safe = false;
                                        break;
                                    }
                                    if (activeTet.big == true)
                                    {
                                        if (gameField[activeTet.bits[i].x + (big * ((pad.inputH + 1) / 2)) + (pad.inputH* ((ruleset.bigMove * big) + 1 - big))][activeTet.bits[i].y + 1] != 0)
                                        {
                                            safe = false;
                                            break;
                                        }
                                    }
                                }
                                if (safe) //if it's fine, move them all by one
                                {
                                    activeTet.move(pad.inputH*((ruleset.bigMove*big)+1-big), 0);
                                    if (ruleset.lockType == 1) activeTet.groundTimer = ruleset.baseLock;
                                }
                            }
                        }

                        //calc gravity LAST so I-jumps are doable?


                        if (!g0)
                            for (int tempGrav = gravCounter; tempGrav >= Math.Pow(256, ruleset.gravType + 1); tempGrav = tempGrav - (int)Math.Pow(256, ruleset.gravType + 1))
                        {
                            blockDrop++;
                        }

                        if (!activeTet.floored)
                        {
                            if (pad.inputV == -1 && (gravTable[gravLevel] <= Math.Pow(256, ruleset.gravType + 1)))
                            {
                                blockDrop += 1;
                            }
                            else
                                gravCounter += gravTable[gravLevel]; //add our current gravity strength
                        }

                        if (gravCounter >= Math.Pow(256, ruleset.gravType + 1))
                        {
                            blockDrop += 1;
                            gravCounter = 0;
                        }

                        if (g20 || (gravTable[gravLevel] == Math.Pow(256, ruleset.gravType + 1) * 20))
                        {
                            blockDrop = 19;
                        }
                        

                        if (blockDrop > 0)// && currentTimer != (int)Field.timerType.LockDelay)
                        {
                            tetGrav(activeTet, blockDrop, false, son);
                        }

                        

                        //handle ghost piece logic
                        if (activeTet.id != 0)
                        {
                            ghostPiece = activeTet.clone();

                            tetGrav(ghostPiece, 22, true, false);
                        }

                    }


                }
                else //gamerunning == false
                {
                    if (fadeout < 22)
                    {
                        for (int i = 0; i < 10; i++)
                            if (gameField[i][21 - fadeout] != 0)
                                gameField[i][21 - fadeout] = 9;
                        fadeout++;
                    }
                    else if (fadeout - 22 < 70)
                    {
                        fadeout++;
                    }
                    else
                    {
                        if (pad.inputStart == 1)
                        {
                            cont = true;
                            Audio.stopMusic();
                        }
                        if (pad.inputPressedRot2)
                        {
                            exit = true;
                            Audio.stopMusic();
                        }
                    }
                    if(fadeout == 91)
                        Audio.playMusic("results");
                }
            }
        }

        private void spawnPiece()
        {
            //get next tetromino, generate another for "next"
            if (ruleset.nextNum > 0)
            {
                activeTet = new Tetromino(nextTet[0].id, ruleset.bigmode);
                //if (mode.bigmode && activeTet.id == 5)
                    //activeTet.bits[0].x = 7;
                activeTet.groundTimer = ruleset.baseLock;
                for (int i = 0; i < nextTet.Count - 1; i++)
                {
                    nextTet[i] = nextTet[i + 1];
                }
                nextTet[nextTet.Count - 1] = generatePiece();
                if (pad.inputPressedHold && ruleset.hold == true && swappedHeld == 0)
                {
                    hold();
                    Audio.playSound(s_Hold);
                }
                else if (starting == 0)
                {
                    switch (nextTet[nextTet.Count - 1].id)
                    {
                        case 1:
                            Audio.playSound(s_Tet1);
                            break;
                        case 2:
                            Audio.playSound(s_Tet2);
                            break;
                        case 3:
                            Audio.playSound(s_Tet3);
                            break;
                        case 4:
                            Audio.playSound(s_Tet4);
                            break;
                        case 5:
                            Audio.playSound(s_Tet5);
                            break;
                        case 6:
                            Audio.playSound(s_Tet6);
                            break;
                        case 7:
                            Audio.playSound(s_Tet7);
                            break;
                    }
                }
            }

            int intRot = 0;
            if (pad.inputPressedRot1)
                intRot += 1;
            if (pad.inputPressedRot2)
                intRot -= 1;
            if (intRot != 0 && pad.inputRot1 + pad.inputRot2 == 0)
            {
                if (ruleset.gameRules == 6 || pad.inputRot3 == 0)
                {
                    if(rotatePiece(activeTet, intRot, true))
                        Audio.playSound(s_PreRot);
                }
            }

            gravCounter = 0;

            if (level % 100 != 99 && level != (ruleset.endLevel - 1) && inCredits == false)
                level++;

            bool blocked = false;

            blocked = !emptyUnderTet(activeTet);

            if (blocked)
            {
                endGame();
            }
            softCounter = 0;
            sonicCounter = 0;
            tetLife = 0;

            justSpawned = true;
        }

        private void hold()
        {
            
                Tetromino tempTet;
                if (heldPiece != null)
                {
                    swappedHeld = 2;
                    tempTet = new Tetromino(heldPiece.id, ruleset.bigmode);
                    heldPiece = new Tetromino(activeTet.id, false);
                    activeTet = tempTet;
                    activeTet.groundTimer = ruleset.baseLock;
                }
                else
                {
                    swappedHeld = 1;
                    heldPiece = new Tetromino(activeTet.id, false);
                    spawnPiece();
                    currentTimer = (int)Field.timerType.ARE;
                    timerCount = ruleset.baseARE;
                }
        }

        private void endGame()
        {
            if (godmode == true && !inCredits && !torikan)
                clearField();
            else
            {
                gameRunning = false;

                int lowY = 22;
                for (int i = 0; i < activeTet.bits.Count; i++)
                {
                    if (lowY > activeTet.bits[i].y)
                        lowY = activeTet.bits[i].y;
                }

                for (int i = 0; i < activeTet.bits.Count; i++)
                {
                    int big = 2;
                    if (activeTet.big)
                        big = 1;

                    for (int j = 0; j < (big % 2) + 1; j++)
                    {
                        for (int k = 0; k < (big % 2) + 1; k++)
                        {
                            if (activeTet.bits[i].y + k < 0)
                                continue;
                            if (checkGimmick(1) || creditsType == 2)
                                gameField[activeTet.bits[i].x + j][activeTet.bits[i].y + k] = 8;
                            else if (activeTet.bone == true)
                                gameField[activeTet.bits[i].x + j][activeTet.bits[i].y + k] = 10;
                            else
                                gameField[activeTet.bits[i].x + j][activeTet.bits[i].y + k] = activeTet.id;

                        }
                    }
                }
                activeTet.id = 0;

                results = new GameResult();

                if (inCredits)
                {
                    if (creditsProgress >= ruleset.creditsLength)//cleared credits
                    {
                        results.lineC = 1;
                        if ((ruleset.gameRules == 2 || ruleset.gameRules == 3) && isGM == true)
                            gm2grade = 32;

                        if (ruleset.gameRules == 4)
                        {
                            if (creditsType == 1)
                                creditGrades += 50;
                            if (creditsType == 2)
                                creditGrades += 160;
                        }

                        //check credits line clears, award orange line if applicable
                        //play game clear jingle?
                    }
                    else//topped out in credits
                    {
                        if ((ruleset.gameRules == 2 || ruleset.gameRules == 3) && isGM == true)
                            gm2grade = 27;
                    }
                }

                for (; creditGrades > 100; creditGrades -= 100 )
                {
                    gm2grade++;
                }

                    //handle TGM3 section modifyers
                if (ruleset.gameRules == 4)
                {
                    foreach (int i in secCools)
                    {
                        gm2grade += i;
                    }

                    if (gm2grade < 0)
                        gm2grade = 0;
                    if (gm2grade > 32)
                        gm2grade = 32;
                }



                timer.stop();
                Audio.stopMusic();
                results.game = ruleset.gameRules - 1;
                results.username = "CHEATS";
                if (ruleset.gameRules == 1)
                    results.grade = grade;
                else
                    results.grade = gm2grade;
                results.score = score;
                if (ruleset.gameRules == 1)
                    results.time = (long)((masterTime * ruleset.FPS) / 60);
                else
                    results.time = (long)((timer.elapsedTime * ruleset.FPS)/60);
                results.level = level;
                results.medals = medals;
                results.delay = pad.lag != 0;
                contTime.start();
            }
        }

        private void triggerCredits()
        {
            if ((rotations * 5) / (totalTets * 6) >= 1 && ruleset.gameRules > 1 && ruleset.gameRules < 4)
            {
                medals[1] = 3;
                rotations = 0;
                totalTets = 0;
                Audio.playSound(s_Medal);
            }

            timer.stop();
            inCredits = true;
            Audio.stopMusic();
            if (ruleset.gameRules == 1)
                Audio.playMusic("crdtvanish");
            else
            {
                Audio.playSound(s_GameClear); //allclear
                creditsPause.start();
                clearField();
            }

            if (ruleset.id == 0)
            {
                isGM = true;
                for (int i = 0; i < GMflags.Count; i++ )
                {
                    if (GMflags[i] == false)
                        isGM = false;
                }
                if ((ruleset.gameRules == 2 && GMflags.Count(p => p == true) == 11) || (ruleset.gameRules == 3 && GMflags.Count(p => p == true) == 10))
                {
                    isGM = true;
                }

                if ((ruleset.gameRules == 2 && isGM) || (ruleset.gameRules == 3 && isGM)) //tgm2 always has invisible
                    creditsType = 2;

                if (ruleset.gameRules == 4)
                {
                    if (grade > 26 && secCools.Sum() == 8)
                        creditsType = 2;
                    else
                        creditsType = 1;
                }
            }
        }

        private bool rotatePiece(Tetromino tet, int p, bool spawn)
        {
            bool success = true;
            rotations++;
            Tetromino tmptet = RSYS.rotate(tet, p, gameField, ruleset.gameRules, ruleset.bigmode, spawn);
            if (activeTet == tmptet)
                success = false;
            activeTet = tmptet;
            return success;
        }

        public Tetromino generatePiece()
        {
            totalTets++;
            Random piece = new Random();
            int tempID = 0;
            for (int j = 0; j < ruleset.genAttps; j++)
            {
                bool copy = false;
                tempID = piece.Next(7) + 1;

                

                for (int k = 0; k < lastTet.Count; k++)
                {
                    if (ruleset.easyGen || starting != 0)
                    {
                        tempID = piece.Next(5) + 1;//no S or Z

                        if (tempID == 5)//offset the O back to proper ID
                            tempID = 7;
                    }
                    else
                    {
                        tempID = piece.Next(7) + 1;
                    }

                    if (tempID == lastTet[k])
                    {
                        copy = true;
                    }
                }
                if (copy == false)
                    break;
            }

            for (int j = 0; j < lastTet.Count - 1; j++)
            {
                lastTet[j] = lastTet[j + 1];
            }

            Tetromino tempTet = new Tetromino(tempID, false);//force non-big here so they're not stretched out in the queue
            lastTet[lastTet.Count - 1] = tempTet.id;
            if (checkGimmick(3))
                tempTet.bone = true;
            if (w4)
                w4ify();
            return tempTet;
        }

        private bool checkGimmick(int gim)
        {
            for (int i = 0; i < activeGim.Count; i++)
            {
                if (activeGim[i] == gim)
                    return true;
            }
            return false;
        }

        private void raiseGarbage(int num)
        {
            for(int i = 0; i < num; i++)//skim the top
            {
                for (int j = 0; j < 10; j++ )
                    gameField[j][i] = 0;
            }
            for (int i = 0; i < 21; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    gameField[j][i] = gameField[j][i + num];
                }
            }
            for (int i = 0; i < num; i++)//works
                for (int j = 0; j < 10; j++)
                    if (gameField[j][21 - num] != 0)
                        gameField[j][21 - i] = 9;
        }

        public bool emptyUnderTet(Tetromino tet)
        {
            bool status = true;
            for (int i = 0; i < tet.bits.Count; i++)
            {
                if (tet.bits[i].y < 0)
                    continue;
                if (gameField[tet.bits[i].x][tet.bits[i].y] != 0)
                {
                    status = false;
                    break;
                }
                if(tet.big)
                {
                    if (tet.bits[i].y + 1 > 21)
                    {
                        status = false;
                        break;
                    }
                    if (gameField[tet.bits[i].x+1][tet.bits[i].y] != 0)
                    {
                        status = false;
                        break;
                    }
                    if (gameField[tet.bits[i].x][tet.bits[i].y+1] != 0)
                    {
                        status = false;
                        break;
                    }
                    if (gameField[tet.bits[i].x+1][tet.bits[i].y+1] != 0)
                    {
                        status = false;
                        break;
                    }
                }
            }

            return status;
        }
        public void tetGrav(Tetromino tet, int i, bool ghost, bool sonic)
        {
            int g = 0;
            for (g = 0; g < i; g++)
            {
                bool breakout = false;

                for (int p = 0; p < tet.bits.Count; p++)
                {
                    if (tet.bits[p].y < 0)
                        continue;
                    if (gameField[tet.bits[p].x][tet.bits[p].y + g] != 0)
                    {
                        g = g - 1;
                        breakout = true;
                        break;
                    }
                }
                for (int p = 0; p < tet.bits.Count; p++)
                {
                    if (tet.bits[p].y + g == 21)
                    {
                        breakout = true;
                        break;
                    }
                }

                if (breakout)
                    break;
            }
            tet.move(0, g);

            if (g != 0 && ghost == false && activeTet.kicked == 0 && ruleset.lockType == 0)
                activeTet.groundTimer = ruleset.baseLock;

            if (i == 19 && g20 == false && g > sonicCounter && sonic)
                sonicCounter = g;

            //if (activeTet.kicked != 0)
                //activeTet.groundTimer = 0;

            //failsafe for now, ugh
            if (!emptyUnderTet(tet))
            {
                tet.move(0, -1);
            }
        }

        private string convertTime(long numtime)
        {
            int min, sec, msec, msec10;
            min = (int)Math.Floor((double)numtime / 60000);
            numtime -= min * 60000;
            sec = (int)Math.Floor((double)numtime / 1000);
            numtime -= sec * 1000;
            msec = (int)numtime;
            msec10 = (int)(msec / 10);
            return string.Format("{0,2:00}:{1,2:00}:{2,2:00}", min, sec, msec10);
        }

        

        private void clearField()
        {
            gameField = new List<List<int>>();
            for (int i = 0; i < 10; i++)
            {
                List<int> tempList = new List<int>();
                for (int j = 0; j < 22; j++)
                {
                    tempList.Add(0); // at least nine types; seven tetrominoes, invisible, and garbage
                }
                gameField.Add(tempList);
            }
        }

        private void w4ify()
        {
            List<List<int>> newField = new List<List<int>>();
            for (int i = 0; i < 3; i++)
            {
                List<int> tempList = new List<int>();
                for (int j = 0; j < 22; j++)
                {
                    tempList.Add(9); // at least nine types; seven tetrominoes, invisible, and garbage
                }
                newField.Add(tempList);
            }
            for (int i = 0; i < 4; i++)
            {
                List<int> tempList = new List<int>();
                for (int j = 0; j < 22; j++)
                {
                    tempList.Add(gameField[i+3][j]); // at least nine types; seven tetrominoes, invisible, and garbage
                }
                newField.Add(tempList);
            }
            for (int i = 0; i < 3; i++)
            {
                List<int> tempList = new List<int>();
                for (int j = 0; j < 22; j++)
                {
                    tempList.Add(9); // at least nine types; seven tetrominoes, invisible, and garbage
                }
                newField.Add(tempList);
            }
            gameField = newField;
        }

        private void drawGrade(Graphics drawBuffer)
        {
            string gd;
            int gold = 0;
            if (textBrush.Color == Color.Gold)
                gold = 78;

            if (ruleset.gameRules == 1)
                gd = ruleset.grades[grade];
            else
                gd = ruleset.grades[gm2grade];

            for (int i = 0; i < gd.Length; i++)
            {
                int dex = "0123456789SmMVOKTG".IndexOf(gd.Substring(i, 1));
                drawBuffer.DrawImage(gradeImg, new Rectangle(x + 280 + i * 26, 70, 25, 25), new Rectangle(1 + (dex % 6) * 26, 1 + (int)Math.Floor((double)dex / 6) * 26 + gold, 25, 25), GraphicsUnit.Pixel);
            }
        }

        

        private void updateMusic()
        {
            int cools = 0;
            foreach (int i in secCools)
            {
                if (i == 1) cools++;
            }
            int section = curSection + cools;

            if(ruleset.id == 0)
            {
                if (section == 0)
                {
                    Audio.stopMusic();
                    Audio.playMusic("Level 1");
                }
                if (section == 5)
                {
                    Audio.stopMusic();
                    Audio.playMusic("Level 2");
                }
                if(ruleset.gameRules == 2 || ruleset.gameRules == 3)
                {
                    if (section == 7)
                    {
                        Audio.stopMusic();
                        Audio.playMusic("Level 3");
                    }
                    if (section == 9)
                    {
                        Audio.stopMusic();
                        Audio.playMusic("Level 4");
                    }
                }
                if (ruleset.gameRules == 4)
                {
                    if (section == 8)
                    {
                        Audio.stopMusic();
                        Audio.playMusic("Level 3");
                    }
                }
            }

            else if (ruleset.id == 1)//death
            {
                if (curSection == 0)
                {
                    Audio.stopMusic();
                    Audio.playMusic("Level 2");
                }
                if (curSection == 3)
                {
                    Audio.stopMusic();
                    Audio.playMusic("Level 3");
                }
                if (curSection == 5)
                {
                    Audio.stopMusic();
                    Audio.playMusic("Level 4");
                }
            }
            else if (ruleset.id == 2)//shirase
            {
                if (curSection == 0)
                {
                    Audio.stopMusic();
                    Audio.playMusic("Level 3");
                }
                if (curSection == 5)
                {
                    Audio.stopMusic();
                    Audio.playMusic("Level 4");
                }
                if (curSection == 7)
                {
                    Audio.stopMusic();
                    Audio.playMusic("Level 5");
                }
                if (curSection == 10)
                {
                    Audio.stopMusic();
                    Audio.playMusic("Level 6");
                }
                if (curSection == 14)
                {
                    Audio.stopMusic();
                    Audio.playMusic("Shirase");
                }
            }
            else
            {
                if(curSection == 0)
                {
                    Audio.stopMusic();
                    Audio.playMusic("Casual 1");
                }
            }
        }

        private void checkFade()
        {
            int section = curSection + speedBonus;
            if(ruleset.id == 0)//master
            {
                if (section == 4 && level % 100 > 84) Audio.stopMusic();
                if ((ruleset.gameRules == 2 || ruleset.gameRules == 3) && section == 6 && level % 100 > 84) Audio.stopMusic();
                if ((ruleset.gameRules == 2 || ruleset.gameRules == 3) && section == 8 && level % 100 > 84) Audio.stopMusic();
                if (ruleset.gameRules == 4 && section == 7 && level % 100 > 84) Audio.stopMusic();
            }
            if(ruleset.id == 1)//death
            {
                if (curSection == 2 && level % 100 > 84) Audio.stopMusic();
                if (curSection == 4 && level % 100 > 84) Audio.stopMusic();
            }
            if (ruleset.id == 3)//shirase
            {
                if (curSection == 4 && level % 100 > 84) Audio.stopMusic();
                if (curSection == 6 && level % 100 > 84) Audio.stopMusic();
                if (curSection == 9 && level % 100 > 84) Audio.stopMusic();
                if (curSection == 13 && level % 100 > 84) Audio.stopMusic();
            }
        }
    }
}
