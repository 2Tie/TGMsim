using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Input;
using System.IO;

namespace TGMsim
{
    public partial class Form1 : Form
    {

        GameTimer timer = new GameTimer();
        double FPS = 60.00; //59.84 for TGM1, 61.68 for TGM2, 60.00 for TGM3
        long startTime;
        long interval;

        Controller pad1 = new Controller();
        Rules rules = new Rules();

        Image imgBuffer;
        Graphics graphics, drawBuffer;

        Profile player;
        

        int menuState = 0; //title, login, game select, mode select, ingame, results, hiscore roll, custom game, settings

        Field field1;
        Login login;
        GameSelect gSel;
        ModeSelect mSel;

        List<List<GameResult>> hiscoreTable = new List<List<GameResult>>();
        bool saved;

        NAudio.Vorbis.VorbisWaveReader musicStream;
        NAudio.Wave.WaveOutEvent songPlayer = new NAudio.Wave.WaveOutEvent();

        public Form1()
        {
            InitializeComponent();

            this.DoubleBuffered = true;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;

            this.ClientSize = new Size(1280, 780);

            interval = (long)TimeSpan.FromSeconds(1.0 / FPS).TotalMilliseconds;

            imgBuffer = (Image)new Bitmap(this.ClientSize.Width, this.ClientSize.Height);

            hiscoreTable.Add(new List<GameResult>());

            graphics = this.CreateGraphics();
            drawBuffer = Graphics.FromImage(imgBuffer);

            playMusic("title");
            
        }

        public void gameLoop()
        {
            timer.start();

            while (this.Created)
            {
                startTime = timer.elapsedTime;
                gameLogic();
                gameRender();

                Application.DoEvents();
                while (timer.elapsedTime - startTime < interval) ;
            }
        }

        void changeMenu(int newMenu)
        {

            switch (menuState) //clean up the current menu
            {  //title, login, game select, mode select, ingame, results, hiscore roll, custom game, settings
                case 0:
                    break;
                case 1:
                    break;
                case 4:
                    break;
            }

            switch (newMenu) //activate the new menu
            {  //title, login, game select, mode select, ingame, results, hiscore roll, custom game, settings
                case 0:
                    menuState = 0;
                    FPS = 60.00;
                    break;
                case 1:
                    menuState = 1;
                    login = new Login();
                    playMusic("login");
                    break;
                case 2:
                    if (menuState != 3)
                        playMusic("menu");
                    menuState = 2;
                    FPS = 60.00;
                    gSel = new GameSelect();
                    break;
                case 3:
                    menuState = 3;
                    loadHiscores(1);
                    mSel = new ModeSelect(gSel.menuSelection);
                    break;
                case 4:
                    saved = false;
                    menuState = 4;
                    FPS = 59.84;
                    Mode m = new Mode();
                    stopMusic();
                    m.setMode(0);
                    field1 = new Field(pad1, rules, m, musicStream);
                    if (player.name == "   ")
                        field1.godmode = true;
                    break;

                case 6: //hiscores
                    mSel.game += 0; //current game
                    mSel.selection += 0; //current mode
                    break;

                case 8:
                    menuState = 8;
                    playMusic("settings");
                    break;
            }

            menuState = newMenu;
        }

        void gameLogic()
        {
            pad1.poll();
            //deal with game logic
            switch (menuState)
            {
                case 0: //title
                    titleLogic();
                    break;
                case 1: //login
                    loginLogic();
                    break;
                case 2: //game select
                    gSel.logic(pad1);
                    if ((pad1.inputRot1 | pad1.inputRot3) == 1)
                    {
                        if (gSel.menuSelection == 5) //settings
                            changeMenu(8);
                        else if (gSel.menuSelection == 4) //Bonus
                        {
                            changeMenu(3);
                        }
                        else
                        {
                            rules.setGame(gSel.menuSelection + 1);
                            changeMenu(3);
                        }
                    }
                    break;
                case 3: //mode select
                    mSel.logic(pad1);
                    if ((pad1.inputRot1 | pad1.inputRot3) == 1)
                    {
                        //TODO: change rules based on what mode is selected
                        if (gSel.menuSelection != 4)
                            changeMenu(4);
                        else
                        {
                            switch (mSel.selection)
                            {
                                case 0://Custom
                                    changeMenu(7);
                                    break;
                                case 1://eternal shirase
                                    rules.setGame(4);
                                    //TODO
                                    break;
                                case 2://konoha
                                    rules.setGame(4);
                                    //TODO
                                    break;
                            }
                        }
                    }
                    if (pad1.inputRot2 == 1)
                        changeMenu(2);
                    if (pad1.inputHold == 1)
                        changeMenu(6);//hiscores for mode
                    break;
                case 4: //ingame
                    field1.logic();
                    if (field1.gameRunning == false)
                    {
                        //test and save a hiscore ONCE
                        if (saved == false && field1.godmode == false)
                        {
                            field1.results.username = player.name;
                            field1.newHiscore = testHiscore(field1.results);
                            saved = true;
                        }

                    }
                    if (field1.cont == true)
                    {
                        Mode m = new Mode();
                        m.setMode(0);
                        field1 = new Field(pad1, rules, m, musicStream);
                    }
                    if (field1.exit == true)
                        changeMenu(2);
                    break;
                case 8://settings
                    if (pad1.inputPressedRot2)
                        changeMenu(2);
                    break;
            }
        }

        private void loginLogic()
        {

            login.logic(pad1);

            if (login.loggedin)
            {
                pad1.inputStart = 0;
                player = login.temp;
                changeMenu(2);
            }
        }

        private void titleLogic()
        {

            if (pad1.inputStart == 1)
            {
                pad1.inputStart = 0;
                changeMenu(1);
            }
        }

        void gameRender()
        {
            //draw temp BG so bleeding doesn't occur
            drawBuffer.FillRectangle(new SolidBrush(Color.Black), this.ClientRectangle);


            switch (menuState)
            {
                case 0:
                    drawBuffer.DrawString("TGM sim title screen thingy", DefaultFont, new SolidBrush(Color.White), 500, 300);
                    drawBuffer.DrawString("Press Start", DefaultFont, new SolidBrush(Color.White), 550, 400);
                    break;
                case 1:
                    drawBuffer.DrawString("login", DefaultFont, new SolidBrush(Color.White), 100, 20);
                    login.render(drawBuffer);
                    break;
                case 2:
                    drawBuffer.DrawString("game select", DefaultFont, new SolidBrush(Color.White), 100, 20);
                    gSel.render(drawBuffer);
                    break;
                case 3:
                    drawBuffer.DrawString("mode select", DefaultFont, new SolidBrush(Color.White), 100, 20);
                    mSel.render(drawBuffer);
                    break;
                case 4:
                    field1.draw(drawBuffer);
                    break;
                case 8:
                    drawBuffer.DrawString("preferences", DefaultFont, new SolidBrush(Color.White), 100, 20);
                    break;
            }

#if DEBUG
            SolidBrush debugBrush = new SolidBrush(Color.White);
            //denote debug
            drawBuffer.DrawString("DEBUG", DefaultFont, debugBrush, 20, 710);
            //draw the current inputs
            drawBuffer.DrawString(Keyboard.IsKeyDown(pad1.keyUp) ? "1" : "0", DefaultFont, debugBrush, 28, 720);
            drawBuffer.DrawString(Keyboard.IsKeyDown(pad1.keyLeft) ? "1" : "0", DefaultFont, debugBrush, 20, 730);
            drawBuffer.DrawString(Keyboard.IsKeyDown(pad1.keyDown) ? "1" : "0", DefaultFont, debugBrush, 28, 740);
            drawBuffer.DrawString(Keyboard.IsKeyDown(pad1.keyRight) ? "1" : "0", DefaultFont, debugBrush, 36, 730);
            drawBuffer.DrawString(Keyboard.IsKeyDown(pad1.keyRot1) ? "1" : "0", DefaultFont, debugBrush, 50, 720);
            drawBuffer.DrawString(Keyboard.IsKeyDown(pad1.keyRot2) ? "1" : "0", DefaultFont, debugBrush, 58, 720);
            drawBuffer.DrawString(Keyboard.IsKeyDown(pad1.keyRot3) ? "1" : "0", DefaultFont, debugBrush, 66, 720);
            drawBuffer.DrawString(Keyboard.IsKeyDown(pad1.keyHold) ? "1" : "0", DefaultFont, debugBrush, 50, 730);
            drawBuffer.DrawString(Keyboard.IsKeyDown(pad1.keyStart) ? "1" : "0", DefaultFont, debugBrush, 74, 730);

#endif

            //draw the buffer, then set to refresh
            this.BackgroundImage = imgBuffer;
            this.Invalidate();

        }

        private bool testHiscore(GameResult gameResult)
        {
            if (hiscoreTable.Count == 0) //oh no! error reading the file!
                return false;

            switch (gameResult.game)
            {
                case 0:
                    for (int i = 0; i < hiscoreTable[0].Count; i++ ) //for each entry in TGM1
                    {
                        if (hiscoreTable[0][i].grade < gameResult.grade)
                        {
                            saveHiscore(gameResult, gameResult.game, i);
                            return true;
                        }
                        if (hiscoreTable[0][i].grade == gameResult.grade)
                        {
                            //compare time
                            if (hiscoreTable[0][i].time > gameResult.time)
                            {
                                saveHiscore(gameResult, gameResult.game, i);
                                return true;
                            }
                        }
                        //else try the next one.
                    }
                    break;
                /*default:
                    for (int i = 0; i < hiscoreTable[gameResult.game - 1].Count; i++)
                    {
                        if (hiscoreTable[gameResult.game - 1][i].grade < gameResult.grade)
                        {
                            //insert the new score here
                            return true;
                        }
                        if (hiscoreTable[gameResult.game - 1][i].grade == gameResult.grade)
                        {
                            //compare level
                            if (hiscoreTable[gameResult.game - 1][i].level < gameResult.level)
                            {
                                return true;
                            }
                            if (hiscoreTable[gameResult.game - 1][i].level == gameResult.level)
                            {
                                //compare time
                                if (hiscoreTable[gameResult.game - 1][i].time < gameResult.time)
                                {
                                    return true;
                                }
                            }
                        }
                        //else try the next one.
                    }
                    break;*/
        }
            return false;
        }

        private void saveHiscore(GameResult gameResult, int g, int place)
        {

            hiscoreTable[g].Insert(place, gameResult);
            hiscoreTable[g].RemoveAt(hiscoreTable[g].Count - 1);

            //TODO: write to the file
            string hiFile = "gm" + (g+1) + ".dat";
            File.Delete(hiFile);
            using (FileStream fsStream = new FileStream(hiFile, FileMode.Create))
            using (BinaryWriter sw = new BinaryWriter(fsStream, Encoding.UTF8))
            {
                for (int i = 0; i < hiscoreTable[g].Count; i++)
                {
                    sw.Write(hiscoreTable[g][i].username);
                    sw.Write(hiscoreTable[g][i].grade);
                    sw.Write(hiscoreTable[g][i].time);
                }
            }
        }
        
        private void loadHiscores (int game)
        {
            string filename = "gm" + game.ToString() + ".dat";
            if (!File.Exists(filename))
            {
                if (game == 1)
                    defaultTGMScores();
            }
            
            BinaryReader scores = new BinaryReader(File.OpenRead(filename));
            if (scores.BaseStream.Length == 0)
                defaultTGMScores();
            //otherwise, load up the hiscores into memory!

            
            while (true)
            {
                GameResult tempRes = new GameResult();
                tempRes.username = scores.ReadString();
                tempRes.grade = scores.ReadInt32();
                tempRes.time = scores.ReadInt64();
                hiscoreTable[game - 1].Add(tempRes);
                if (scores.BaseStream.Position == scores.BaseStream.Length)
                    break;
            }
        }

        public bool defaultTGMScores()
        {
            using (FileStream fsStream = new FileStream("gm1.dat", FileMode.OpenOrCreate))
            using (BinaryWriter sw = new BinaryWriter(fsStream, Encoding.UTF8))
            {
                long temptime;
                sw.Write("SAK");
                sw.Write(11);
                temptime = 540000;
                sw.Write(temptime);
                sw.Write("CHI");
                sw.Write(10);
                temptime = 480000;
                sw.Write(temptime);
                sw.Write("NAI");
                sw.Write(9);
                temptime = 420000;
                sw.Write(temptime);
                sw.Write("MIZ");
                sw.Write(6);
                temptime = 360000;
                sw.Write(temptime);
                sw.Write("KAR");
                sw.Write(5);
                temptime = 300000;
                sw.Write(temptime);
                sw.Write("NAG");
                sw.Write(4);
                temptime = 240000;
                sw.Write(temptime);
            }
            return false;
        }
        private void playMusic(string song)
        {
            try
            {
                musicStream = new NAudio.Vorbis.VorbisWaveReader(@"Audio\" + song + ".ogg");
                LoopStream loop = new LoopStream(musicStream);
                songPlayer.Init(loop);
                songPlayer.Play();

            }
            catch (Exception)
            {
                MessageBox.Show("The file \"" + song + ".ogg\" was not found!", "Warning!", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                //throw;
            }
        }

        private void stopMusic()
        {
            songPlayer.Stop();
            songPlayer.Dispose();
        }
    }
}
