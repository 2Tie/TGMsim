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

        Tetromino activeTet;
        public List<Tetromino> nextTet = new List<Tetromino>();
        public List<int> lastTet = new List<int>() { 6, 6, 6, 6};
        public Rotation RSYS = new R_ARIKA();

        public List<List<int>> gameField = new List<List<int>>();

        public List<int> full = new List<int>();

        public List<int> medals = new List<int>() { 0, 0, 0, 0, 0, 0 };

        public List<long> sectionTimes = new List<long>();
        public List<bool> GMflags = new List<bool>();
        public List<int> secTet = new List<int>();
        public List<int> secCools = new List<int>();
        public List<vanPip> vanList = new List<vanPip>();
        public int creditsType = 0;


        public bool isGM = false;

        public Tetromino heldPiece;

        public Tetromino ghostPiece;

        public GameTimer timer = new GameTimer();
        public GameTimer contTime = new GameTimer();
        public GameTimer startTime = new GameTimer();
        public GameTimer sectionTime = new GameTimer();
        public GameTimer creditsPause = new GameTimer();

        public bool swappedHeld;

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

        Rules ruleset;
        public Mode mode;
        int curSection;

        public GameResult results;

        public bool cheating = false;
        public bool godmode = false;
        public bool bigmode = false;
        public bool g20 = false;
        public bool g0 = false;

        public List<int> activeGim = new List<int>();
        public int gimIndex = 0;
        int garbTimer = 0;

        public bool cont = false;
        public bool exit = false;

        List<Image> tetImgs = new List<Image>();
        List<Image> tetSImgs = new List<Image>();
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
        System.Windows.Media.MediaPlayer s_Tetris = new System.Windows.Media.MediaPlayer();
        System.Windows.Media.MediaPlayer s_Combo = new System.Windows.Media.MediaPlayer();
        System.Windows.Media.MediaPlayer s_Medal = new System.Windows.Media.MediaPlayer();

        Pen gridPen = new Pen(new SolidBrush(Color.White));

        Controller pad;
        int inputDelayH = 0, inputDelayDir = 0;

        public Field(Controller ctlr, Rules rules, Mode m2, NAudio.Vorbis.VorbisWaveReader music)
        {
            x = 320;
            y = 100;
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

            s_Ready.Open(new Uri(Environment.CurrentDirectory + @"\Res\Audio\SE\SEP_ready.wav"));
            s_Go.Open(new Uri(Environment.CurrentDirectory + @"/Res/Audio/SE/SEP_go.wav"));
            s_Tet1.Open(new Uri(Environment.CurrentDirectory + @"/Res/Audio/SE/SEB_mino1.wav"));
            s_Tet2.Open(new Uri(Environment.CurrentDirectory + @"/Res/Audio/SE/SEB_mino2.wav"));
            s_Tet3.Open(new Uri(Environment.CurrentDirectory + @"/Res/Audio/SE/SEB_mino3.wav"));
            s_Tet4.Open(new Uri(Environment.CurrentDirectory + @"/Res/Audio/SE/SEB_mino4.wav"));
            s_Tet5.Open(new Uri(Environment.CurrentDirectory + @"/Res/Audio/SE/SEB_mino5.wav"));
            s_Tet6.Open(new Uri(Environment.CurrentDirectory + @"/Res/Audio/SE/SEB_mino6.wav"));
            s_Tet7.Open(new Uri(Environment.CurrentDirectory + @"/Res/Audio/SE/SEB_mino7.wav"));
            s_PreRot.Open(new Uri(Environment.CurrentDirectory + @"/Res/Audio/SE/SEB_prerotate.wav"));
            s_Contact.Open(new Uri(Environment.CurrentDirectory + @"/Res/Audio/SE/SEB_fixa.wav"));
            s_Lock.Open(new Uri(Environment.CurrentDirectory + @"/Res/Audio/SE/SEB_instal.wav"));
            s_Clear.Open(new Uri(Environment.CurrentDirectory + @"/Res/Audio/SE/SEB_disappear.wav"));
            s_Impact.Open(new Uri(Environment.CurrentDirectory + @"/Res/Audio/SE/SEB_fall.wav"));
            s_Grade.Open(new Uri(Environment.CurrentDirectory + @"/Res/Audio/SE/SEP_levelchange.wav"));
            s_Hold.Open(new Uri(Environment.CurrentDirectory + @"/Res/Audio/SE/SEB_prehold.wav"));
            s_GameClear.Open(new Uri(Environment.CurrentDirectory + @"/Res/Audio/SE/SEP_gameclear.wav"));
            s_Cool.Open(new Uri(Environment.CurrentDirectory + @"/Res/Audio/SE/SEP_cool.wav"));
            s_Combo.Open(new Uri(Environment.CurrentDirectory + @"/Res/Audio/SE/SEP_combo.wav"));
            s_Tetris.Open(new Uri(Environment.CurrentDirectory + @"/Res/Audio/SE/SEP_tetris.wav"));
            s_Medal.Open(new Uri(Environment.CurrentDirectory + @"/Res/Audio/SE/SEP_platinum.wav"));

            fonts.AddFontFile(@"Res\Maestro.ttf");
            FontFamily fontFam = fonts.Families[0];
            f_Maestro = new System.Drawing.Font(fontFam, 16, GraphicsUnit.Pixel);

            pad = ctlr;
            ruleset = rules;
            mode = m2;
            vorbisStream = music;

            Random random = new Random();

            activeTet = new Tetromino(0); //first piece cannot be S, Z, or O

            if (nextTet.Count == 0 && ruleset.nextNum > 0) //generate nextTet
            {
                for (int i = 0; i < ruleset.nextNum; i++)
                {
                    nextTet.Add(generatePiece());
                }
            }

            g20 = mode.g20;

            gravTable = ruleset.gravTableTGM1;
            if (ruleset.gameRules >= 4)
                gravTable = ruleset.gravTableTGM3;


            switch (mode.id)
            {
                case 0:
                    frameColour = Color.LightGray;
                    break;
                case 1:
                    frameColour = Color.DarkRed;
                    ruleset.baseARE = ruleset.delayTableDeath[0][0];
                    ruleset.baseARELine = ruleset.delayTableDeath[1][0];
                    ruleset.baseDAS = ruleset.delayTableDeath[2][0];
                    ruleset.baseLock = ruleset.delayTableDeath[3][0];
                    ruleset.baseLineClear = ruleset.delayTableDeath[4][0];
                    break;
                case 2:
                    frameColour = Color.DarkBlue;
                    ruleset.baseARE = ruleset.delayTableShirase[0][0];
                    ruleset.baseARELine = ruleset.delayTableShirase[1][0];
                    ruleset.baseDAS = ruleset.delayTableShirase[2][0];
                    ruleset.baseLock = ruleset.delayTableShirase[3][0];
                    ruleset.baseLineClear = ruleset.delayTableShirase[4][0];
                    break;
                case 3:
                    frameColour = Color.DarkGreen;
                    break;
            }

            if (mode.exam != -1)
                frameColour = Color.Gold;

            speedBonus = mode.lvlBonus;

            gameRunning = true;
            starting = 1;

            
            while (gravLevel < gravTable.Count - 1) //update gravity
            {
                if (level + (speedBonus * 100) >= ruleset.gravLevelsTGM1[gravLevel + 1])
                    gravLevel++;
                else
                    break;
            }

            if ((mode.g20 == true || gravTable[gravLevel] == Math.Pow(256, ruleset.gravType + 1) * 20) && ruleset.gameRules < 3)
                textBrush = new SolidBrush(Color.Gold);

            //timer.start();
            startTime.start();
            sectionTime.start();
            secTet.Add(0);

            for (int i = 0; i < 10; i++)
            {
                List<int> tempList = new List<int>();
                for (int j = 0; j < 22; j++)
                {
                    tempList.Add(0); // at least nine types; seven tetrominoes, invisible, and garbage
                }
                gameField.Add(tempList);
            }


            if (mode.id == 4)
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
            //draw the field
            drawBuffer.FillRectangle(new SolidBrush(Color.FromArgb(20, Color.Gray)), x, y + 25, width, height);

            //draw the pieces
            for (int i = 0; i < 10; i++)
            {
                for (int j = 2; j < 22; j++)
                {
                    int block = gameField[i][j];
                    if (block == 9)//garbage
                        drawBuffer.FillRectangle(new SolidBrush(Color.DarkGray), x + 25 * i, y - 25 + j * 25, 25, 25);
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

            //draw the frame
            drawBuffer.FillRectangle(new SolidBrush(frameColour), x - 5, y - 5 + 25, width + 10, 5);
            drawBuffer.FillRectangle(new SolidBrush(frameColour), x - 5, y + height + 25, width + 10, 5);
            drawBuffer.FillRectangle(new SolidBrush(frameColour), x - 5, y - 5 + 25, 5, height + 10);
            drawBuffer.FillRectangle(new SolidBrush(frameColour), x + width, y - 5 + 25, 5, height + 10);

            int big = 2;
            if (mode.bigmode)
                big = 1;

            //draw the ghost piece
            if (level < mode.sections[0] && ghostPiece != null && activeTet.id == ghostPiece.id)
            {
                int lowY = 22;
                for (int i = 0; i < activeTet.bits.Count; i++)
                {
                    if (ghostPiece.bits[i].y < lowY)
                        lowY = ghostPiece.bits[i].y;
                }

                for (int i = 0; i < activeTet.bits.Count; i++)
                {
                    if (ghostPiece.bits[i].y * (2 / big) - (lowY * ((2 / big) - 1)) > 1)
                    {
                        drawBuffer.DrawImage(tetImgs[ghostPiece.id], x + 25 * (ghostPiece.bits[i].x * (2 / big) - (4 % (6 - big))), y - 25 + 25 * (ghostPiece.bits[i].y * (2 / big) - (lowY * ((2 / big) - 1))), 25 * (2 / big), 25 * (2 / big));
                        drawBuffer.FillRectangle(new SolidBrush(Color.FromArgb(130, Color.Black)), x + 25 * (ghostPiece.bits[i].x * (2 / big) - (4 % (6 - big))), y - 25 + 25 * (ghostPiece.bits[i].y * (2 / big) - (lowY * ((2 / big) - 1))), 25 * (2 / big), 25 * (2 / big));
                    }
                }
            }

            //draw the current piece
            if (activeTet.id != 0)
            {
                int lowY = 22;
                for (int i = 0; i < activeTet.bits.Count; i++)
                {
                    if (activeTet.bits[i].y < lowY)
                        lowY = activeTet.bits[i].y;
                }
                for (int i = 0; i < activeTet.bits.Count; i++)
                {

                    if (activeTet.bone == true)
                        drawBuffer.DrawImage(tetImgs[10], x + 25 * (activeTet.bits[i].x * (2 / big) - (4 % (6 - big))), y - 25 + 25 * (activeTet.bits[i].y * (2 / big) - (lowY * ((2 / big) - 1))), 25 * (2 / big), 25 * (2 / big));
                    else
                    {
                        if (activeTet.groundTimer > 2 || activeTet.groundTimer == 0)
                        {
                            drawBuffer.DrawImage(tetImgs[activeTet.id], x + 25 * (activeTet.bits[i].x * (2 / big) - (4 % (6 - big))), y - 25 + 25 * (activeTet.bits[i].y * (2/big) - (lowY*((2/big) - 1))), 25 * (2 / big), 25 * (2 / big));
                            drawBuffer.FillRectangle(new SolidBrush(Color.FromArgb((ruleset.baseLock - activeTet.groundTimer) * 130 / ruleset.baseLock, Color.Black)), x + 25 * (activeTet.bits[i].x * (2 / big) - (4 % (6 - big))), y - 25 + 25 * (activeTet.bits[i].y * (2 / big) - (lowY * ((2 / big) - 1))), 25 * (2 / big), 25 * (2 / big));
                        }
                        else
                            drawBuffer.DrawImage(tetImgs[9], x + 25 * (activeTet.bits[i].x * (2 / big) - (4 % (6 - big))), y - 25 + 25 * (activeTet.bits[i].y * (2 / big) - (lowY * ((2 / big) - 1))), 25 * (2 / big), 25 * (2 / big));
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
                        if (nextTet[i].bone == true)
                            drawBuffer.DrawImageUnscaled(tetSImgs[10], x + i * 70 + 16 * nextTet[i].bits[j].x + 40, y + 16 * nextTet[i].bits[j].y - 75);
                        else
                            //drawBuffer.FillRectangle(new SolidBrush(tetColors[nextTet[i].id]), x + i*70 + 15 * nextTet[i].bits[j].x + 40, y + 15 * nextTet[i].bits[j].y - 75, 15, 15);
                            drawBuffer.DrawImageUnscaled(tetSImgs[nextTet[i].id], x + i * 70 + 16 * nextTet[i].bits[j].x + 40, y + 16 * nextTet[i].bits[j].y - 75);
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
                        if (swappedHeld == true)
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

            }


            drawBuffer.FillRectangle(new SolidBrush(gravColor), 600, 550, 60, 8);
            drawBuffer.FillRectangle(new SolidBrush(gravMeter), 600, 550, (int)Math.Round(((double)gravTable[gravLevel] * 60) / ((Math.Pow(256, ruleset.gravType + 1) * 20))), 8);
            if (mode.g20 == true || gravTable[gravLevel] == Math.Pow(256, ruleset.gravType + 1) * 20)
                drawBuffer.FillRectangle(new SolidBrush(Color.Red), 600, 550, 60, 8);

            //SMALL TEXT
            //levels
            drawBuffer.DrawString(level.ToString(), f_Maestro, textBrush, 610, 530);
            if (mode.sections.Count == curSection)
                drawBuffer.DrawString(mode.sections[curSection - 1].ToString(), f_Maestro, textBrush, 610, 570);
            else
                drawBuffer.DrawString(mode.sections[curSection].ToString(), f_Maestro, textBrush, 610, 570);

            if (ruleset.gameRules == 1)
            {
                drawBuffer.DrawString(score.ToString(), f_Maestro, textBrush, 610, 400);
                drawBuffer.DrawString("NEXT GRADE:", f_Maestro, textBrush, 600, 240);
                if (grade != ruleset.gradePointsTGM1.Count)
                    drawBuffer.DrawString(ruleset.gradePointsTGM1[grade + 1].ToString(), f_Maestro, textBrush, 600, 260);
                else
                    drawBuffer.DrawString("??????", f_Maestro, textBrush, 600, 260);
            }

            if (godmode)
                drawBuffer.DrawString("GODMODE", f_Maestro, new SolidBrush(Color.Orange), 20, 680);
            if (g0)
                drawBuffer.DrawString("0G MODE", f_Maestro, new SolidBrush(Color.Orange), 20, 700);
            //if (mode.bigmode)
                //drawBuffer.DrawString("BIG MODE", f_Maestro, new SolidBrush(Color.Orange), 20, 720);

            //BIGGER TEXT
            drawBuffer.DrawString("Points", SystemFonts.DefaultFont, textBrush, 600, 380);

            //GRADE TEXT
            if (ruleset.showGrade)
            {
                if (ruleset.gameRules == 1)
                    drawBuffer.DrawString(ruleset.gradesTGM1[grade].ToString(), SystemFonts.DefaultFont, textBrush, 600, 100);
                else
                    drawBuffer.DrawString(ruleset.gradesTGM3[gm2grade].ToString(), SystemFonts.DefaultFont, textBrush, 600, 100);
            }

            if (mode.exam != -1)
                drawBuffer.DrawString("EXAM: " + ruleset.gradesTGM3[mode.exam].ToString(), SystemFonts.DefaultFont, textBrush, 600, 100);

            //Starting things
            if (starting == 2)
                drawBuffer.DrawString("Ready", SystemFonts.DefaultFont, new SolidBrush(Color.White), 200, 200);
            if (starting == 3)
                drawBuffer.DrawString("Go", SystemFonts.DefaultFont, new SolidBrush(Color.White), 200, 200);


            //endgame stats
            if (gameRunning == false)
            {
                if (mode.id == 0)
                {
                    if (ruleset.gameRules == 1)
                        drawBuffer.DrawString("Grade: " + ruleset.gradesTGM1[results.grade], SystemFonts.DefaultFont, new SolidBrush(Color.White), 200, 200);
                    else
                        drawBuffer.DrawString("Grade: " + ruleset.gradesTGM3[results.grade], SystemFonts.DefaultFont, new SolidBrush(Color.White), 200, 200);
                }
                drawBuffer.DrawString("Score: " + results.score, SystemFonts.DefaultFont, new SolidBrush(Color.White), 200, 210);
                drawBuffer.DrawString("Time: " + results.time, SystemFonts.DefaultFont, new SolidBrush(Color.White), 200, 220);
                drawBuffer.DrawString("Name: " + results.username, SystemFonts.DefaultFont, new SolidBrush(Color.White), 200, 230);
                if (results.username == "CHEATER")
                {
                    throw new DivideByZeroException();
                }

                if (torikan)
                {
                    drawBuffer.DrawString("Torikan hit!", SystemFonts.DefaultFont, new SolidBrush(Color.White), 200, 250);
                    drawBuffer.DrawString(convertTime((long)(torDef * ruleset.FPS / 60)) + " behind!", SystemFonts.DefaultFont, new SolidBrush(Color.White), 200, 260);
                }

                drawBuffer.DrawString("Press start to", SystemFonts.DefaultFont, new SolidBrush(Color.White), 200, 280);
                drawBuffer.DrawString("restart the field!", SystemFonts.DefaultFont, new SolidBrush(Color.White), 200, 290);

                drawBuffer.DrawString("Press B to", SystemFonts.DefaultFont, new SolidBrush(Color.White), 200, 310);
                drawBuffer.DrawString("return to menu!", SystemFonts.DefaultFont, new SolidBrush(Color.White), 200, 320);
            }


#if DEBUG

            //draw the playfieldgrid
            //for (int i = 1; i < 11; i++)
                //drawBuffer.DrawLine(gridPen, x + 25 * i, y + 25, x + 25 * i, y + height + 25);
            //for (int i = 1; i < 22; i++)
                //drawBuffer.DrawLine(gridPen, x, y + 25 * i, x + width, y + 25 * i);

            SolidBrush debugBrush = new SolidBrush(Color.White);

            //tech stats
            drawBuffer.DrawString("Grav Level: " + gravTable[gravLevel].ToString() + " Bonus: " + speedBonus, SystemFonts.DefaultFont, debugBrush, 20, 760);
            if (!inCredits)
                drawBuffer.DrawString("Current Level: " + level, SystemFonts.DefaultFont, debugBrush, 200, 760);
            else
            {
                drawBuffer.DrawString("Credits", SystemFonts.DefaultFont, debugBrush, 200, 760);
                drawBuffer.DrawString("Credits Timer: " + creditsProgress.ToString(), SystemFonts.DefaultFont, debugBrush, 200, 750);
            }

            //game stats
            if (ruleset.gameRules == 1)
                drawBuffer.DrawString("Score: " + score, SystemFonts.DefaultFont, debugBrush, 90, 740);
            else
                drawBuffer.DrawString("gradeScore: " + gradePoints, SystemFonts.DefaultFont, debugBrush, 90, 740);
            drawBuffer.DrawString("Combo: " + combo, SystemFonts.DefaultFont, debugBrush, 90, 750);
            if (isGM)
                drawBuffer.DrawString("Grade: GM", SystemFonts.DefaultFont, debugBrush, 90, 730);
            else
                drawBuffer.DrawString("Grade: " + grade + "GM2: " + gm2grade, SystemFonts.DefaultFont, debugBrush, 90, 730);

            for (int i = 0; i < GMflags.Count; i++)
            {
                drawBuffer.DrawString("-*".Substring(Convert.ToInt32(GMflags[i]), 1), SystemFonts.DefaultFont, debugBrush, 200 + i * 8, 730);
            }

            if (ruleset.gameRules == 1)
                drawBuffer.DrawString("Bravos: " + bravoCounter, SystemFonts.DefaultFont, debugBrush, 200, 740);
            else
            {
                drawBuffer.DrawString(medals[0] + " " + medals[1] + " " + medals[2] + " " + medals[3] + " " + medals[4] + " " + medals[5], SystemFonts.DefaultFont, debugBrush, 200, 740);
                drawBuffer.DrawString(totalTets.ToString(), SystemFonts.DefaultFont, debugBrush, 400, 740);
            }

            if (newHiscore)
                drawBuffer.DrawString("New Hiscore", SystemFonts.DefaultFont, debugBrush, 200, 700);

            //time
            drawBuffer.DrawString(convertTime((long)(timer.elapsedTime * ruleset.FPS / 60)), SystemFonts.DefaultFont, debugBrush, 100, 700);

            drawBuffer.DrawString("groundTime " + activeTet.groundTimer, SystemFonts.DefaultFont, debugBrush, 400, 700);
#endif
        }

        public void logic()
        {
            if (startTime.elapsedTime > 1000 && starting == 1)
            {
                starting = 2;
                //play READY
                playSound(s_Ready);
            }
            if (startTime.elapsedTime > 2000 && starting == 2)
            {
                starting = 3;
                //play GO
                playSound(s_Go);
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


                    //check inputs and handle logic pertaining to them
                    //pad.poll();

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

                    if (inCredits && (ruleset.gameRules > 1 == creditsPause.elapsedTime >= 3000))
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
                                playSound(s_Impact);
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
                            if (timerCount <= 0 && ((inCredits == creditsPause.elapsedTime > 3000) || ruleset.gameRules == 1))
                            {
                                swappedHeld = false;
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

                        if (activeTet.id != 0)
                        {
                            int lowY = 22;
                            int big = 2;
                            if (mode.bigmode)
                                big = 1;
                            for (int i = 0; i < activeTet.bits.Count; i++)
                            {
                                if (activeTet.bits[i].y < lowY)
                                    lowY = activeTet.bits[i].y;
                            }
                            for (int i = 0; i < activeTet.bits.Count; i++)
                            {
                                if ((activeTet.bits[i].y * (2 / big) - (lowY * ((2 / big) - 1))) + (big % 2) < 0)
                                    continue;
                                if ((activeTet.bits[i].y * (2 / big) - (lowY * ((2 / big) - 1))) + (big % 2) + 1 >= 22)
                                {
                                    activeTet.floored = true;
                                    break;
                                }
                                else if (gameField[activeTet.bits[i].x * (2 / big) - (4 % (6 - big))][(activeTet.bits[i].y * (2 / big) - (lowY * ((2 / big) - 1))) + (big % 2) + 1] != 0)
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
                                    

                                    //GIMMICKS

                                    if (mode.gimList.Count > 0)
                                    {
                                        if (mode.gimList.Count > gimIndex)
                                            if (mode.gimList[gimIndex].startLvl <= level)
                                            {
                                                activeGim.Add(mode.gimList[gimIndex].type);
                                                gimIndex++;
                                            }
                                        for (int i = 0; i < activeGim.Count; i++)
                                        {
                                            if (mode.gimList[gimIndex - activeGim.Count + i].endLvl <= level)
                                            {
                                                activeGim.RemoveAt(i);
                                            }
                                        }
                                    }

                                    if (checkGimmick(2))
                                        garbTimer += 1;

                                    if (checkGimmick(5))
                                        mode.bigmode = true;

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

                                    for (int i = 0; i < activeTet.bits.Count; i++)
                                    {
                                        int big = 2;
                                        if (mode.bigmode)
                                            big = 1;

                                        for (int j = 0; j < (big % 2) + 1; j++ )
                                        {
                                            for (int k = 0; k < (big % 2) + 1; k++)
                                            {
                                                if ((activeTet.bits[i].y * (2 / big) - (lowY * ((2 / big) - 1))) + k < 0)
                                                    continue;
                                                if (checkGimmick(1) || creditsType == 2)
                                                    gameField[(activeTet.bits[i].x * (2 / big) - (4 % (6 - big))) + j][(activeTet.bits[i].y * (2 / big) - (lowY * ((2 / big) - 1))) + k] = 8;
                                                else if (activeTet.bone == true)
                                                    gameField[(activeTet.bits[i].x * (2 / big) - (4 % (6 - big))) + j][(activeTet.bits[i].y * (2 / big) - (lowY * ((2 / big) - 1))) + k] = 10;
                                                else
                                                    gameField[(activeTet.bits[i].x * (2 / big) - (4 % (6 - big))) + j][(activeTet.bits[i].y * (2 / big) - (lowY * ((2 / big) - 1))) + k] = activeTet.id;

                                                if (creditsType == 1)
                                                {
                                                    vanPip vP = new vanPip();
                                                    vP.time = creditsProgress;
                                                    vP.x = (activeTet.bits[i].x * (2 / big) - (4 % (6 - big))) + j;
                                                    vP.y = (activeTet.bits[i].y * (2 / big) - (lowY * ((2 / big) - 1))) + k;
                                                    vanList.Add(vP);
                                                }
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
                                            }
                                        }
                                    }

                                    if (tetCount >= 150)
                                        recoverChecking = true;

                                    if (full.Count > 0)  //if full rows, clear the rows, start the line clear timer, give points
                                    {
                                        int bigFull = full.Count;
                                        if (mode.bigmode)
                                            bigFull = bigFull / 2;
                                        for (int i = 0; i < full.Count; i++)
                                        {
                                            for (int j = 0; j < 10; j++)
                                                gameField[j][full[i]] = 0;
                                        }
                                        for (int i = 0; i < bigFull; i++)
                                        {
                                            level++;
                                            if (ruleset.gameRules > 3 && i > 2)
                                                level++;
                                            if (ruleset.gameRules > 3 && i > 3)
                                                level++;
                                        }
                                        if (level > mode.endLevel && mode.endLevel != 0)
                                            level = mode.endLevel;

                                        //calculate combo!

                                        if (bigFull == 4)
                                        {
                                            tetrises++;
                                            secTet[curSection]++;
                                            playSound(s_Tetris);
                                        }

                                        int bravo = 1;
                                        if (tetCount == 0)
                                        {
                                            bravoCounter++;
                                            bravo = 4;
                                        }
                                        //give points
                                        if (ruleset.gameRules == 1)
                                        {
                                            combo = combo + (2 * bigFull) - 2;
                                            if (!inCredits && mode.gradedBy == 1)
                                            {
                                                if (softCounter > 20)
                                                    softCounter = 20;
                                                int newscore = ((int)Math.Ceiling((double)(level + bigFull) / 4) + softCounter) * bigFull * ((bigFull * 2) - 1) * bravo;
                                                if (comboing)
                                                    newscore *= combo;

                                                score += newscore;
                                            }
                                        }
                                        else
                                        {
                                            if (mode.id == 0)
                                            {
                                                if (bigFull > 1)
                                                {
                                                    combo++;
                                                }
                                                if(mode.gradedBy == 1)
                                                {
                                                    if (!inCredits)
                                                    {
                                                        int newPts = (int)(Math.Ceiling(ruleset.baseGradePts[bigFull - 1][grade] * ruleset.comboTable[bigFull - 1][combo]) * Math.Ceiling((double)level / 250));
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
                                                        playSound(s_Combo);
                                                }
                                            }
                                            if (mode.id == 1)
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
                                                        playSound(s_Grade);
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
                                            if (gradePoints > 99 || (level > 749 && bigFull == 4))
                                            {
                                                if (grade < ruleset.gradeIntTGM2.Count - 1)
                                                {
                                                    grade++;
                                                    gradePoints = 0;
                                                    if (ruleset.gradeIntTGM2[grade] != gm2grade)
                                                    {
                                                        gm2grade++;
                                                        if (ruleset.gameRules < 4)
                                                            playSound(s_Grade);
                                                    }
                                                }
                                            }
                                        }

                                        if (mode.sections.Count != curSection)
                                            if (level >= mode.sections[curSection])
                                            {
                                                curSection++;
                                                secTet.Add(0);

                                                //CHECK TORIKANS
                                                if (mode.id == 1) //death
                                                {
                                                    if (curSection == 5 && temptime > 205000)
                                                    {
                                                        level = 500;
                                                    }
                                                }
                                                if (ruleset.gameRules == 4)
                                                {
                                                    if (mode.id == 0 && curSection == 5 && temptime > 420000) //ti master
                                                    {
                                                        level = 500;
                                                        torikan = true;
                                                        torDef = temptime - 420000;
                                                        endGame();
                                                        return;
                                                    }
                                                    if (mode.id == 2) //shirase
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
                                                if (mode.id == 0 || mode.id == 3)//Master
                                                {
                                                    if (ruleset.gameRules == 2)
                                                    {
                                                        switch (curSection + speedBonus)
                                                        {
                                                            case 9:
                                                            case 8:
                                                            case 7:
                                                            case 6:
                                                            case 5:
                                                                ruleset.baseARE = ruleset.delayTableTGM2[0][curSection - 4 + speedBonus];
                                                                ruleset.baseDAS = ruleset.delayTableTGM2[1][curSection - 4 + speedBonus];
                                                                ruleset.baseLock = ruleset.delayTableTGM2[2][curSection - 4 + speedBonus];
                                                                ruleset.baseLineClear = ruleset.delayTableTGM2[3][curSection - 4 + speedBonus];
                                                                break;
                                                            default:
                                                                break;
                                                        }
                                                    }
                                                    if (ruleset.gameRules == 3)
                                                    {
                                                        switch (curSection + speedBonus)
                                                        {
                                                            case 9:
                                                            case 8:
                                                            case 7:
                                                            case 6:
                                                            case 5:
                                                                ruleset.baseARE = ruleset.delayTableTAP[0][curSection - 4 + speedBonus];
                                                                ruleset.baseARELine = ruleset.delayTableTAP[1][curSection - 4 + speedBonus];
                                                                ruleset.baseDAS = ruleset.delayTableTAP[2][curSection - 4 + speedBonus];
                                                                ruleset.baseLock = ruleset.delayTableTAP[3][curSection - 4 + speedBonus];
                                                                ruleset.baseLineClear = ruleset.delayTableTAP[4][curSection - 4 + speedBonus];
                                                                break;
                                                            default:
                                                                break;
                                                        }
                                                    }

                                                    if (ruleset.gameRules >= 4)
                                                    {
                                                        switch (curSection + speedBonus)
                                                        {
                                                            case 17:
                                                            case 16:
                                                            case 15:
                                                            case 14:
                                                            case 13:
                                                            case 12:
                                                            case 11:
                                                            case 10:
                                                            case 9:
                                                            case 8:
                                                            case 7:
                                                            case 6:
                                                            case 5:
                                                                ruleset.baseARE = ruleset.delayTableTGM3[0][curSection - 4 + speedBonus];
                                                                ruleset.baseARELine = ruleset.delayTableTGM3[1][curSection - 4 + speedBonus];
                                                                ruleset.baseDAS = ruleset.delayTableTGM3[2][curSection - 4 + speedBonus];
                                                                ruleset.baseLock = ruleset.delayTableTGM3[3][curSection - 4 + speedBonus];
                                                                ruleset.baseLineClear = ruleset.delayTableTGM3[4][curSection - 4 + speedBonus];
                                                                break;
                                                            default:
                                                                break;
                                                        }
                                                    }
                                                }
                                                if (mode.id == 1)//death
                                                {
                                                    switch (curSection)
                                                    {
                                                        case 9:
                                                        case 8:
                                                        case 7:
                                                        case 6:
                                                        case 5:
                                                            ruleset.baseARE = ruleset.delayTableDeath[0][ruleset.delayTableDeath.Count - 1];
                                                            ruleset.baseARELine = ruleset.delayTableDeath[1][ruleset.delayTableDeath.Count - 1];
                                                            ruleset.baseDAS = ruleset.delayTableDeath[2][ruleset.delayTableDeath.Count - 1];
                                                            ruleset.baseLock = ruleset.delayTableDeath[3][ruleset.delayTableDeath.Count - 1];
                                                            ruleset.baseLineClear = ruleset.delayTableDeath[4][ruleset.delayTableDeath.Count - 1];
                                                            break;
                                                        case 4:
                                                        case 3:
                                                        case 2:
                                                        case 1:
                                                        case 0:
                                                            ruleset.baseARE = ruleset.delayTableDeath[0][curSection];
                                                            ruleset.baseARELine = ruleset.delayTableDeath[0][curSection];
                                                            ruleset.baseDAS = ruleset.delayTableDeath[0][curSection];
                                                            ruleset.baseLock = ruleset.delayTableDeath[0][curSection];
                                                            ruleset.baseLineClear = ruleset.delayTableDeath[0][curSection];
                                                            break;
                                                    }
                                                }
                                                if (mode.id == 2 || mode.id == 5) //shirase
                                                {
                                                    switch (curSection)
                                                    {
                                                        case 13:
                                                        case 12:
                                                        case 11:
                                                            ruleset.baseARE = ruleset.delayTableShirase[0][curSection - 5];
                                                            ruleset.baseARELine = ruleset.delayTableShirase[0][curSection - 5];
                                                            ruleset.baseDAS = ruleset.delayTableShirase[0][curSection - 5];
                                                            ruleset.baseLock = ruleset.delayTableShirase[0][curSection - 5];
                                                            ruleset.baseLineClear = ruleset.delayTableShirase[0][curSection - 5];
                                                            break;
                                                        case 10:
                                                        case 9:
                                                        case 8:
                                                        case 7:
                                                        case 6:
                                                            ruleset.baseARE = ruleset.delayTableShirase[0][5];
                                                            ruleset.baseARELine = ruleset.delayTableShirase[0][5];
                                                            ruleset.baseDAS = ruleset.delayTableShirase[0][5];
                                                            ruleset.baseLock = ruleset.delayTableShirase[0][5];
                                                            ruleset.baseLineClear = ruleset.delayTableShirase[0][5];
                                                            break;
                                                        case 5:
                                                            ruleset.baseARE = ruleset.delayTableShirase[0][4];
                                                            ruleset.baseARELine = ruleset.delayTableShirase[0][4];
                                                            ruleset.baseDAS = ruleset.delayTableShirase[0][4];
                                                            ruleset.baseLock = ruleset.delayTableShirase[0][4];
                                                            ruleset.baseLineClear = ruleset.delayTableShirase[0][4];
                                                            break;
                                                        case 4:
                                                        case 3:
                                                            ruleset.baseARE = ruleset.delayTableShirase[0][3];
                                                            ruleset.baseARELine = ruleset.delayTableShirase[0][3];
                                                            ruleset.baseDAS = ruleset.delayTableShirase[0][3];
                                                            ruleset.baseLock = ruleset.delayTableShirase[0][3];
                                                            ruleset.baseLineClear = ruleset.delayTableShirase[0][3];
                                                            break;
                                                        case 2:
                                                        case 1:
                                                        case 0:
                                                            ruleset.baseARE = ruleset.delayTableShirase[0][curSection];
                                                            ruleset.baseARELine = ruleset.delayTableShirase[0][curSection];
                                                            ruleset.baseDAS = ruleset.delayTableShirase[0][curSection];
                                                            ruleset.baseLock = ruleset.delayTableShirase[0][curSection];
                                                            ruleset.baseLineClear = ruleset.delayTableShirase[0][curSection];
                                                            break;
                                                    }
                                                }
                                                

                                                //MEDALS
                                                if (ruleset.gameRules != 1)
                                                {
                                                    int sectime = 60000;
                                                    if (mode.id == 1 || mode.id == 2)
                                                        sectime = 45000;
                                                    if (sectionTime.elapsedTime < sectime - 10000 && medals[2] < 1)
                                                    {
                                                        medals[2] = 1;
                                                        playSound(s_Medal);
                                                    }
                                                    if (sectionTime.elapsedTime < sectime - 5000 && medals[2] < 2)
                                                    {
                                                        medals[2] = 2;
                                                        playSound(s_Medal);
                                                    }
                                                    if (sectionTime.elapsedTime < sectime && medals[2] < 3)
                                                    {
                                                        medals[2] = 3;
                                                        playSound(s_Medal);
                                                    }

                                                    if (curSection == 3)
                                                        if ((rotations * 5) / (totalTets * 6) >= 1)
                                                        {
                                                            medals[1] = 1;
                                                            rotations = 0;
                                                            totalTets = 0;
                                                            playSound(s_Medal);
                                                        }
                                                    if (curSection == 7)
                                                        if ((rotations * 5) / (totalTets * 6) >= 1)
                                                        {
                                                            medals[1] = 2;
                                                            rotations = 0;
                                                            totalTets = 0;
                                                            playSound(s_Medal);
                                                        }


                                                    sectionTimes.Add(sectionTime.elapsedTime);

                                                    //check Regrets!
                                                    if (ruleset.gameRules == 4 && mode.id == 0)
                                                    {
                                                        if (sectionTime.elapsedTime > ruleset.secRegrets[curSection - 1])
                                                        {
                                                            if (curSection != 9)
                                                                secCools[curSection - 1] = -1;
                                                            else
                                                                secCools.Add(-1);
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
                                                playSound(s_Medal);
                                            }
                                            if (bravoCounter == 2 && medals[0] == 1)
                                            {
                                                medals[0] = 2;
                                                playSound(s_Medal);
                                            }
                                            if (bravoCounter == 3 && medals[0] == 2)
                                            {
                                                medals[0] = 3;
                                                playSound(s_Medal);
                                            }

                                            //RO
                                            
                                            //ST


                                            //SK
                                            int tt = 1;
                                            if (mode.id == 1 || mode.id == 2)
                                                tt = 2;
                                            if (mode.bigmode == true)
                                                tt = 10;
                                            if (tetrises == (int)(Math.Ceiling((double)10 / tt)) && medals[3] == 0)
                                            {
                                                medals[3] = 1;
                                                playSound(s_Medal);
                                            }

                                            //RE
                                            if (recoverChecking == true && tetCount <= 70)
                                                recoveries++;
                                            if (recoveries == 1 && medals[4] == 0)
                                            {
                                                medals[4] = 1;
                                                playSound(s_Medal);
                                            }
                                            if (recoveries == 2 && medals[4] == 0)
                                            {
                                                medals[4] = 2;
                                                playSound(s_Medal);
                                            }
                                            if (recoveries == 4 && medals[4] == 0)
                                            {
                                                medals[4] = 3;
                                                playSound(s_Medal);
                                            }
                                            //CO
                                            int big = 1;
                                            if (mode.bigmode == true)
                                                big = 2;
                                            if ((int)(Math.Ceiling((double)4 / big)) == combo)
                                            {
                                                medals[5] = 1;
                                                playSound(s_Medal);
                                            }
                                            if ((int)(Math.Ceiling((double)5 / big)) == combo)
                                            {
                                                medals[5] = 2;
                                                playSound(s_Medal);
                                            }
                                            if ((int)(Math.Ceiling((double)7 / big)) == combo)
                                            {
                                                medals[5] = 3;
                                                playSound(s_Medal);
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
                                                else if (GMflags.Count == 2 && level >= mode.endLevel)
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

                                        if (mode.endLevel != 0 && level >= mode.endLevel && inCredits == false)
                                        {
                                            triggerCredits();
                                        }

                                        //start timer
                                        currentTimer = (int)Field.timerType.LineClear;
                                        timerCount = ruleset.baseLineClear;
                                        playSound(s_Clear);

                                    }
                                    else //start the ARE, check if new grav level
                                    {
                                        currentTimer = (int)Field.timerType.ARE;
                                        timerCount = ruleset.baseARE;

                                        combo = 1;
                                        comboing = false;

                                    }

                                    checkFade();
                                    

                                    //check level for section cool
                                    if (level % 100 > 69)
                                    {
                                        

                                        
                                        if (ruleset.gameRules == 4 && mode.id == 0)
                                        {

                                            if (secCools.Count <= curSection && curSection != 9)
                                            {
                                                if (curSection != 0)
                                                {
                                                    if ((secCools[curSection - 1] == 1 && sectionTime.elapsedTime < sectionTimes[curSection - 1] + 2000) || (secCools[curSection - 1] != 1 && sectionTime.elapsedTime < ruleset.secCools[curSection]))
                                                    {
                                                        secCools.Add(1);
                                                        playSound(s_Cool);
                                                    }
                                                    else
                                                        secCools.Add(0);
                                                }
                                                else
                                                {
                                                    if (sectionTime.elapsedTime < ruleset.secCools[curSection])
                                                    {
                                                        secCools.Add(1);
                                                        playSound(s_Cool);
                                                    }
                                                    else
                                                        secCools.Add(0);
                                                }
                                            }
                                        }
                                    }


                                    while (gravLevel < gravTable.Count - 1)
                                    {
                                        if (level + (speedBonus * 100) >= ruleset.gravLevelsTGM1[gravLevel + 1])
                                            gravLevel++;
                                        else
                                            break;
                                    }

                                    if ((mode.g20 == true || gravTable[gravLevel] == Math.Pow(256, ruleset.gravType + 1) * 20) && ruleset.gameRules < 3)
                                        textBrush = new SolidBrush(Color.Gold);

                                    playSound(s_Contact);

                                    if (checkGimmick(2) && garbTimer >= 10)
                                    {
                                        raiseGarbage(1);
                                        garbTimer = 0;
                                    }

                                    

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
                                playSound(s_Lock);
                                //timerCount = ruleset.baseLock;
                            }
                        }
                        else
                            currentTimer = (int)Field.timerType.ARE;



                        int blockDrop = 0;// make it here so we can drop faster


                        //check saved inputs and act on them accordingly


                        if (pad.inputV == 1 && ruleset.hardDrop == 1)
                        {
                            blockDrop = 19;
                            gravCounter = 0;
                        }
                        else if (pad.inputV == -1)
                        {
                            if (!activeTet.floored)
                            {
                                blockDrop = 1;
                                softCounter++;
                            }
                            else
                            {
                                gravCounter = 0;
                                activeTet.groundTimer = 0;
                            }
                        }

                        if (ruleset.gameRules == 6 && ruleset.hardDrop == 1 && pad.inputV == 1 && pad.inputPressedRot3 == true)
                        {
                            gravCounter = 0;
                            activeTet.groundTimer = 0;
                        }

                        if (pad.inputHold == 1 && ruleset.hold == true && swappedHeld == false)
                        {
                            hold();
                        }
                        
                        int rot;
                        if (ruleset.gameRules < 6)
                            rot = (pad.inputRot1 | pad.inputRot3) - pad.inputRot2;
                        else
                            rot = pad.inputRot1 - pad.inputRot2;

                        if (rot != 0)
                            rotatePiece(activeTet, rot);



                        if (pad.inputH == 1 && (inputDelayH < 1 || inputDelayH == ruleset.baseDAS))
                        {
                            bool safe = true;
                            int lowY = 22;
                            int big = 2;
                            if (mode.bigmode)
                                big = 1;
                            for (int i = 0; i < activeTet.bits.Count; i++)
                            {
                                if (lowY > activeTet.bits[i].y)
                                    lowY = activeTet.bits[i].y;
                            }
                            //check to the right of each bit
                            for (int i = 0; i < activeTet.bits.Count; i++)
                            {
                                if ((activeTet.bits[i].y * (2 / big) - (lowY * ((2 / big) - 1))) < 0)
                                    continue;
                                if (activeTet.bits[i].x * (2 / big) - (4 % (6 - big)) + (big % 2) + 1 > 9)
                                {
                                    safe = false;
                                    break;
                                }
                                if (gameField[activeTet.bits[i].x * (2 / big) - (4 % (6 - big)) + (big % 2) + 1][activeTet.bits[i].y * (2 / big) - (lowY * ((2 / big) - 1))] != 0)
                                {
                                    safe = false;
                                    break;
                                }
                                if (mode.bigmode == true && activeTet.bits[i].y * (2 / big) - (lowY * ((2 / big) - 1)) + 1 < 22)
                                {
                                    if (gameField[activeTet.bits[i].x * (2 / big) - (4 % (6 - big)) + (big % 2) + 1][activeTet.bits[i].y * (2 / big) - (lowY * ((2 / big) - 1)) + 1] != 0)
                                    {
                                        safe = false;
                                        break;
                                    }
                                }
                            }
                            if (safe) //if it's fine, move them all right one
                            {
                                for (int i = 0; i < activeTet.bits.Count; i++)
                                {
                                    activeTet.bits[i].x += 1;
                                }
                            }
                        }
                        else if (pad.inputH == -1 && (inputDelayH < 1 || inputDelayH == ruleset.baseDAS))
                        {
                            bool safe = true;
                            int lowY = 22;
                            int big = 2;
                            if (mode.bigmode)
                                big = 1;
                            for (int i = 0; i < activeTet.bits.Count; i++)
                            {
                                if (lowY > activeTet.bits[i].y)
                                    lowY = activeTet.bits[i].y;
                            }
                            //check to the left of each bit
                            for (int i = 0; i < activeTet.bits.Count; i++)
                            {
                                if ((activeTet.bits[i].y * (2 / big) - (lowY * ((2 / big) - 1))) < 0)
                                    continue;
                                if (activeTet.bits[i].x * (2 / big) - (4 % (6 - big)) - 1 < 0)
                                {
                                    safe = false;
                                    break;
                                }
                                if (gameField[activeTet.bits[i].x * (2 / big) - (4 % (6 - big)) - 1][activeTet.bits[i].y * (2 / big) - (lowY * ((2 / big) - 1))] != 0)
                                {
                                    safe = false;
                                    break;
                                }
                                if (mode.bigmode == true && activeTet.bits[i].y * (2 / big) - (lowY * ((2 / big) - 1)) + 1 < 22)
                                {
                                    if (gameField[activeTet.bits[i].x * (2 / big) - (4 % (6 - big)) - 1][activeTet.bits[i].y * (2 / big) - (lowY * ((2 / big) - 1)) + 1] != 0)
                                    {
                                        safe = false;
                                        break;
                                    }
                                }
                            }
                            if (safe) //if it's fine, move them all right one
                            {
                                for (int i = 0; i < activeTet.bits.Count; i++)
                                {
                                    activeTet.bits[i].x -= 1;
                                }
                            }
                        }

                        //calc gravity LAST so I-jumps are doable?


                        if (!g0)
                            for (int tempGrav = gravCounter; tempGrav >= Math.Pow(256, ruleset.gravType + 1); tempGrav = tempGrav - (int)Math.Pow(256, ruleset.gravType + 1))
                        {
                            blockDrop++;
                        }

                        gravCounter += gravTable[gravLevel]; //add our current gravity strength


                        if (g20 || (gravTable[gravLevel] == Math.Pow(256, ruleset.gravType + 1) * 20))
                        {
                            blockDrop = 19;
                        }
                        

                        if (blockDrop > 0)// && currentTimer != (int)Field.timerType.LockDelay)
                        {
                            gravCounter = 0;
                            tetGrav(activeTet, blockDrop, false);


                        }

                        

                        //handle ghost piece logic
                        if (activeTet.id != 0)
                        {
                            ghostPiece = activeTet.clone();

                            tetGrav(ghostPiece, 22, true);
                        }

                    }


                }
                else //gamerunning == false
                {
                    if (pad.inputStart == 1)
                    {
                        cont = true;
                    }
                    if (pad.inputPressedRot2)
                        exit = true;
                }
            }
        }

        private void spawnPiece()
        {
            //get next tetromino, generate another for "next"
            if (ruleset.nextNum > 0)
            {
                activeTet = new Tetromino(nextTet[0].id);
                //if (mode.bigmode && activeTet.id == 5)
                    //activeTet.bits[0].x = 7;
                activeTet.groundTimer = ruleset.baseLock;
                for (int i = 0; i < nextTet.Count - 1; i++)
                {
                    nextTet[i] = nextTet[i + 1];
                }
                nextTet[nextTet.Count - 1] = generatePiece();
                if (pad.inputPressedHold)
                {
                    hold();
                    playSound(s_Hold);
                }
                else if (starting == 0)
                {
                    switch (nextTet[nextTet.Count - 1].id)
                    {
                        case 1:
                            playSound(s_Tet1);
                            break;
                        case 2:
                            playSound(s_Tet2);
                            break;
                        case 3:
                            playSound(s_Tet3);
                            break;
                        case 4:
                            playSound(s_Tet4);
                            break;
                        case 5:
                            playSound(s_Tet5);
                            break;
                        case 6:
                            playSound(s_Tet6);
                            break;
                        case 7:
                            playSound(s_Tet7);
                            break;
                    }
                }
            }

            int intRot = 0;
            if (pad.inputPressedRot1)
                intRot += 1;
            if (pad.inputPressedRot2)
                intRot -= 1;
            if (intRot != 0 && activeTet.id != 7 && pad.inputRot1 + pad.inputRot2 == 0)
            {
                if (ruleset.gameRules == 6 || pad.inputRot3 == 0)
                {
                    rotatePiece(activeTet, intRot);
                    playSound(s_PreRot);
                }
            }

            gravCounter = 0;

            if (level % 100 != 99 && level != (mode.endLevel - 1) && inCredits == false)
                level++;

            bool blocked = false;

            blocked = !emptyUnderTet(activeTet);

            if (blocked)
            {
                endGame();
            }
            softCounter = 0;
        }

        private void hold()
        {
            
                Tetromino tempTet;
                if (heldPiece != null)
                {
                    tempTet = new Tetromino(heldPiece.id);
                    heldPiece = new Tetromino(activeTet.id);
                    activeTet = tempTet;
                    activeTet.groundTimer = ruleset.baseLock;
                }
                else
                {
                    heldPiece = new Tetromino(activeTet.id);
                    spawnPiece();
                    currentTimer = (int)Field.timerType.ARE;
                    timerCount = ruleset.baseARE;
                }
                swappedHeld = true;
        }

        private void endGame()
        {
            if (godmode == true && !inCredits)
                clearField();
            else
            {
                gameRunning = false;
                //in the future, the little fadeout animation goes here!


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
                stopMusic();
                results.game = ruleset.gameRules - 1;
                results.username = "CHEATS";
                if (ruleset.gameRules == 1)
                    results.grade = grade;
                else
                    results.grade = gm2grade;
                results.score = score;
                results.time = (int)((timer.elapsedTime * ruleset.FPS)/60);
                results.level = level;
                results.medals = medals;
                contTime.start();
                stopMusic();
                playMusic("results");
            }
        }

        private void triggerCredits()
        {
            if ((rotations * 5) / (totalTets * 6) >= 1)
            {
                medals[1] = 3;
                rotations = 0;
                totalTets = 0;
                playSound(s_Medal);
            }

            timer.stop();
            inCredits = true;
            stopMusic();
            if (ruleset.gameRules == 1)
                playMusic("credits");
            else
            {
                playSound(s_GameClear); //allclear
                creditsPause.start();
                clearField();
            }

            if (mode.id == 0)
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

        private void rotatePiece(Tetromino tet, int p)
        {
            
            rotations++;
            activeTet = RSYS.rotate(tet, p, gameField, ruleset.gameRules, mode.bigmode == true);
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
                    if (mode.easyGen && !cheating)
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

            Tetromino tempTet = new Tetromino(tempID);
            lastTet[lastTet.Count - 1] = tempTet.id;
            if (checkGimmick(3))
                tempTet.bone = true;
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
            int lowY = 22;
            int big = 2;
            if (mode.bigmode)
                big = 1;
            for (int p = 0; p < tet.bits.Count; p++)
            {
                if (tet.bits[p].y < lowY)
                    lowY = tet.bits[p].y;
            }
            for (int i = 0; i < tet.bits.Count; i++)
            {
                if (gameField[tet.bits[i].x * (2 / big) - (4 % (6 - big))][(tet.bits[i].y * (2 / big) - (lowY * ((2 / big) - 1)))] != 0)
                {
                    status = false;
                    break;
                }
                if (gameField[tet.bits[i].x * (2 / big) - (4 % (6 - big))][(tet.bits[i].y * (2 / big) - (lowY * ((2 / big) - 1))) + (big%2)] != 0)
                {
                    status = false;
                    break;
                }
            }

            return status;
        }
        public void tetGrav(Tetromino tet, int i, bool ghost)
        {
            int g = 0;
            for (g = 0; g < i; g++)
            {
                bool breakout = false;
                int lowY = 22;
                int big = 2;
                if (mode.bigmode)
                    big = 1;
                for (int p = 0; p < tet.bits.Count; p++)
                {
                    if (tet.bits[p].y < lowY)
                        lowY = tet.bits[p].y;
                }

                for (int p = 0; p < tet.bits.Count; p++)
                {
                    if (tet.bits[p].y * (2 / big) - (lowY * ((2 / big) - 1)) < 0)
                        continue;
                    if (gameField[tet.bits[p].x * (2 / big) - (4 % (6 - big))][(tet.bits[p].y * (2 / big) - (lowY * ((2 / big) - 1))) + g + (big % 2)] != 0)
                    {
                        g = g - 1;
                        breakout = true;
                        break;
                    }
                }
                for (int p = 0; p < tet.bits.Count; p++)
                {
                    if ((tet.bits[p].y * (2 / big) - (lowY * ((2 / big) - 1))) + g + (big % 2) == 21)
                    {
                        breakout = true;
                        break;
                    }
                }

                if (breakout)
                    break;
            }
            for (int p = 0; p < tet.bits.Count; p++)
            {

                tet.bits[p].y += g;
            }

            if (g != 0 && ghost == false)
                activeTet.groundTimer = ruleset.baseLock;

            if (activeTet.kicked != 0)
                activeTet.groundTimer = 0;

            //failsafe for now, ugh
            if (!emptyUnderTet(tet))
            {
                for (int p = 0; p < tet.bits.Count; p++)
                {
                    tet.bits[p].y--;
                }
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

        private void playSound(System.Windows.Media.MediaPlayer sound)
        {
            sound.Position = new TimeSpan(0);
            sound.Play();
        }

        private void playMusic(string song)
        {
            if (!mode.mute)
            {
                try
                {
                    vorbisStream = new NAudio.Vorbis.VorbisWaveReader(@"Res\Audio\" + song + ".ogg");
                    LoopStream loop = new LoopStream(vorbisStream);
                    soundList.Init(loop);
                    soundList.Play();

                }
                catch (Exception)
                {
                    //MessageBox.Show("The file \"" + song + ".ogg\" was not found!", "Warning!", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                    //throw;
                }
            }
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

        private void stopMusic()
        {
            soundList.Stop();
            soundList.Dispose();
        }

        private void updateMusic()
        {
            if (curSection + speedBonus == 0)
            {
                stopMusic();
                playMusic("Level 1");
            }
            if (curSection + speedBonus == 3)
            {
                stopMusic();
                playMusic("Level 2");
            }
            if (curSection + speedBonus == 5)
            {
                stopMusic();
                playMusic("Level 3");
            }
            if (curSection + speedBonus == 8)
            {
                stopMusic();
                playMusic("Level 4");
            }
            if (curSection + speedBonus == 10)
            {
                stopMusic();
                playMusic("Level 5");
            }
            if (curSection + speedBonus == 15)
            {
                stopMusic();
                playMusic("Level 6");
            }
        }

        private void checkFade()
        {
            if (curSection + speedBonus == 2 && level % 100 > 79)
                stopMusic();
            if (curSection + speedBonus == 4 && level % 100 > 79)
                stopMusic();
            if (curSection + speedBonus == 7 && level % 100 > 79)
                stopMusic();
            if (curSection + speedBonus == 9 && level % 100 > 79)
                stopMusic();
            if (curSection + speedBonus == 14 && level % 100 > 79)
                stopMusic();
        }
    }
}
