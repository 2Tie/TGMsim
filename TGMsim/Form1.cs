using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Windows.Input;
using System.IO;
using System.Drawing.Text;
using System.Globalization;

namespace TGMsim
{
    public partial class Form1 : Form
    {

        GameTimer timer = new GameTimer();
        double FPS = 60.00; //59.84 for TGM1, 61.68 for TGM2, 60.00 for TGM3
        long startTime;
        long interval;
        long freetime = 0;
        bool drawdebug = false;

        Controller pad1 = new Controller();
        GameRules rules = new GameRules();

        Image imgBuffer;
        //Graphics drawBuffer;

        Profile player;
        Preferences prefs;

        string fileName;

        Image medalImg;

        int menuState = 0; //title, login, game select, mode select, ingame, results, hiscore roll, custom game, settings

        Field field1;
        Login login;
        GameSelect gSel;
        ModeSelect mSel;
        CheatMenu cMen;
        CustomMenu customMenu;

        List<GameResult> hiscoreTable = new List<GameResult>();
        bool saved;
        string repNam;
        bool tasAdvance = true;
        bool tasEnabled = true;
        bool tasToggleEnabled = true;
        
        NAudio.Wave.WaveOutEvent songPlayer = new NAudio.Wave.WaveOutEvent();
        
        System.Windows.Media.MediaPlayer s_Start = new System.Windows.Media.MediaPlayer();
        System.Windows.Media.MediaPlayer s_Login = new System.Windows.Media.MediaPlayer();
        System.Windows.Media.MediaPlayer s_GSel = new System.Windows.Media.MediaPlayer();

        int firstrunProgress = 0;

        public Form1()
        {
            InitializeComponent();

            this.DoubleBuffered = true;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;

            this.ClientSize = new Size(800, 600);//Size(1280, 780);
            this.Icon = new Icon(@"Res/GFX/fundoshi.ico");

            //interval = (long)TimeSpan.FromSeconds(1.0 / FPS).TotalMilliseconds;
            medalImg = Image.FromFile("Res/GFX/medals.png");

            imgBuffer = new Bitmap(this.ClientSize.Width, this.ClientSize.Height);

            // hiscoreTable.Add(new List<GameResult>());

            cMen = new CheatMenu();
            player = new Profile();
            prefs = new Preferences(player, pad1);

            readPrefs();

            Draw.buffer = Graphics.FromImage(imgBuffer);
            this.BackgroundImage = imgBuffer;
            this.Invalidate();


            Audio.addSound(s_Start, "/Res/Audio/SE/SEI_class.wav");
            Audio.addSound(s_Login, "/Res/Audio/SE/SEI_data_ok.wav");
            Audio.addSound(s_GSel, "/Res/Audio/SE/SEI_mode_ok.wav");


            Audio.playMusic("Hello Again");
            
        }

        public void gameLoop()
        {
            timer.start();

            int cycle = 1;

            while (this.Created)
            {
                startTime = timer.elapsedTime;

                gameLogic();
                gameRender();

                interval =  16 + (cycle % 1);
                cycle++;
                if (cycle == 4) cycle = 1;

                Application.DoEvents();
                freetime = timer.elapsedTime - startTime;
                while (timer.elapsedTime - startTime < interval);
            }
        }

        void changeMenu(int newMenu)
        {

            switch (newMenu) //activate the new menu
            {  //title, login, game select, mode select, ingame, results, hiscore roll, custom game, settings, cheats, firstrun setup
                case 0:
                    menuState = 0;
                    FPS = 60.00;
                    break;
                case 1:
                    menuState = 1;
                    login = new Login();
                    break;
                case 2:
                    if (menuState > 3 && menuState != 8)
                    {
                        Audio.stopMusic();
                        Audio.playMusic("Hello Again");
                    }
                    menuState = 2;
                    FPS = 60.00;
                    pad1.setLag(0);
                    if (gSel == null)
                        gSel = new GameSelect();
                    break;
                case 3:
                    Audio.playSound(s_GSel);
                    menuState = 3;
                    //if (gSel.menuSelection != 5 && gSel.menuSelection != 0 && gSel.menuSelection != 7)
                        //loadHiscores(gSel.menuSelection);
                    mSel = new ModeSelect(gSel.menuSelection, player);
                    ModeSelect.ModeSelObj mode = mSel.modes[mSel.game][mSel.selection];
                    if (mode.enabled[mSel.variant[mSel.selection]])
                    {
                        rules.setup(mode.game, mode.id, mSel.variant[mSel.selection]);
                        loadHiscores(gSel.menuSelection, 1);
                    }
                    break;
                case 4:
                    saved = false;
                    menuState = 4;
                    setupGame();
                    break;

                case 6: //hiscores
                    menuState = 6;
                    loadHiscores(gSel.menuSelection, mSel.selection+1);
                    //Audio.stopMusic();
                    //Audio.playMusic("Hiscores");
                    break;

                case 7: //custom
                    menuState = 7;
                    customMenu = new CustomMenu();
                    break;

                case 8:
                    menuState = 8;
                    readPrefs();
                    break;
                case 9:
                    menuState = 9;
                    break;
                case 10:
                    menuState = 10;
                    break;
            }

            menuState = newMenu;
        }

        void gameLogic()
        {
            if (player.name != "TAS" || menuState != 4)//limit this so it only logs relevant inputs while TASing
                pad1.poll(Focused);

            //check for debug toggle
            if (pad1.debug)
                drawdebug = true;

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
                        if (gSel.prompt)
                        {
                            if (gSel.pSel == 1)
                            {
                                cMen = new CheatMenu();
                                player.name = "   ";
                                gSel.prompt = false;
                                changeMenu(1);
                            }
                            else
                            {
                                gSel.prompt = false;
                            }
                        }
                        else
                        {
                            if (gSel.menuSelection == 8) //settings
                                changeMenu(8);
                            else //chose a game, let's choose a mode
                            {
                                changeMenu(3);
                            }
                        }
                    }
                    else if (pad1.inputRot2 == 1)
                    {
                        if (gSel.prompt)
                            gSel.prompt = false;
                        else
                        {
                            gSel.pSel = 0;
                            gSel.prompt = true;
                        }
                    }
                    else if (pad1.inputHold == 1)
                    {
                        if (queryReplay())
                        {
                            loadReplay();
                            saved = false;
                            menuState = 4;
                        }
                    }
                    break;
                case 3: //mode select
                    mSel.logic(pad1);
                    ModeSelect.ModeSelObj mode = mSel.modes[mSel.game][mSel.selection];
                    if (pad1.inputV != 0 && mode.enabled[mSel.variant[mSel.selection]])
                    {
                        rules.setup(mode.game, mode.id, mSel.variant[mSel.selection]);
                        if(mode.id != Mode.ModeType.CUSTOM)
                        loadHiscores((int)mode.game, mSel.selection + 1);
                    }
                    if ((pad1.inputRot1 | pad1.inputRot3) == 1)
                    {
                        if (!mode.enabled[mSel.variant[mSel.selection]])
                            return;
                        else if (mode.id == Mode.ModeType.CUSTOM) //custom
                            changeMenu(7);
                        else
                            changeMenu(4); //load mode
                    }
                    if (pad1.inputRot2 == 1)
                        changeMenu(2); //go back
                    break;
                case 4: //ingame
                    if (player.name == "TAS")//if this is a TAS, perform frameadvance
                    {
                        if (Keyboard.IsKeyDown(Key.G) && Focused) //only toggle when focused
                        {
                            if (tasToggleEnabled)
                            {
                                tasEnabled = !tasEnabled;
                                tasToggleEnabled = false;
                            }
                        }
                        else tasToggleEnabled = true;
                        if (!field1.isPlayback && tasEnabled)//if not playback, record
                        {
                            if (Keyboard.IsKeyDown(Key.F) && Focused) //only frameadvanvce while focused
                            {
                                if (tasAdvance)
                                {
                                    pad1.poll(Focused);
                                    field1.logic();
                                    tasAdvance = false;
                                }
                            }
                            else
                                tasAdvance = true;
                        }
                        else //otherwise advance the replay
                        {
                            pad1.poll(Focused);
                            field1.logic();
                        }
                    }
                    else field1.logic();
                    if (field1.gameRunning == false)
                    {
                        //test and save a hiscore ONCE
                        if (saved == false && field1.isPlayback == false && field1.custom == false)
                        {
                            field1.results.username = player.name;
                            if (field1.ruleset.gameRules == GameRules.Games.TAP)
                                field1.results.calcCode();
                            if (rules.gameRules == GameRules.Games.TGM3 && field1.MOD.modeID == Mode.ModeType.MASTER && player.name != "   " && player.name != "TAS")
                            {
                                //add result to history
                                for (int i = 0; i < 6; i++)
                                {
                                    player.TIHistory[i] = player.TIHistory[i + 1];
                                }
                                player.TIHistory[6] = field1.results.grade;

                                //if exam, check results for promotion
                                if (field1.ruleset.exam != -1)
                                {
                                    if (field1.results.grade >= field1.ruleset.exam)
                                    {
                                        player.TIGrade = field1.ruleset.exam;
                                    }
                                }

                                //scale GM back if unqualified
                                else if (field1.results.grade == 32 && player.TIGrade != 32)
                                {
                                    field1.results.grade = 31;
                                }
                            }

                            if (!field1.cheating && field1.ruleset.exam == -1 && player.name != "TAS")//don't save cheat, exam, or TAS hiscores
                                field1.newHiscore = testHiscore(field1.results);
                            if (player.name != "   " && player.name != "TAS")//don't update cheat or TAS profile
                                player.updateUser();
                            if (field1.MOD.modeID == Mode.ModeType.DYNAMO && field1.MOD.level == 999 && player.dynamoProgress == field1.MOD.variant && player.dynamoProgress != 4)
                                player.dynamoProgress = field1.MOD.variant + 1;
                            saved = true;
                        }
                        if(field1.isPlayback)
                        {
                            field1.results.username = repNam;
                        }

                    }
                    if (field1.cont == true)
                    {
                        field1.cont = false;
                        if (field1.isPlayback)
                            loadReplay();
                        else
                        {
                            if (field1.custom)
                                setupCustomGame();
                            else
                                setupGame();
                        }
                    }
                    if (field1.exit == true)
                        changeMenu(2);
                    if (field1.record == true)
                        if (player.name != "   ")//allow saving of TAS replays but not cheat replays
                            writeReplay();
                        else
                            field1.record = false;
                    break;
                /*case 6://hiscores
                    if (pad1.inputPressedRot2)
                        changeMenu(3);
                    break;*/
                case 7://custom game
                    //custom menu logic here
                    customMenu.logic(pad1);
                    if (pad1.inputRot2 == 1)
                        changeMenu(2);
                    else if (pad1.inputStart == 1)
                    {
                        setupCustomGame();
                        menuState = 4;
                    }
                    break;
                case 8://settings
                    if (pad1.inputRot2 == 1 && prefs.menuState == 0)
                    {
                        pad1 = prefs.nPad;
                        savePrefs();
                        changeMenu(2);
                    }
                    prefs.logic();
                    break;
                case 9://cheats
                    if (pad1.inputStart == 1)
                        changeMenu(2);
                    cMen.logic(pad1);
                    break;

                case 10://firstrun
                    if (prefs.assignKey(firstrunProgress))
                        firstrunProgress++;
                    if (firstrunProgress == 9)
                    {
                        changeMenu(0);
                        savePrefs();
                    }
                    break;
            }
        }

        private void loginLogic()
        {
            if (player.name == "   ")
            {

                login.logic(pad1);

                if (login.loggedin)
                {
                    pad1.inputStart = 0;
                    player = login.temp;
                    Audio.playSound(s_Login);
                    savePrefs();
                    if (player.name == "   ")
                        changeMenu(9);
                    else
                        changeMenu(2);
                }
            } else {
                if (player.readUserData())
                    changeMenu(2);
                else player.name = "   ";
            }
        }

        private void titleLogic()
        {
            if (pad1.inputStart == 1 && Audio.loadedTally == Audio.loadedWaiting)
            {
                pad1.inputStart = 0;
                Audio.playSound(s_Start);
                changeMenu(1);
            }
        }

        void gameRender()
        {
            //draw temp BG so bleeding doesn't occur
            Draw.buffer.FillRectangle(Draw.bb, ClientRectangle);


            switch (menuState)
            {
                case 0:
                    Draw.buffer.DrawString("TGM sim title screen thingy", DefaultFont, Draw.wb, 325, 250);
                    if( Audio.loadedTally < Audio.loadedWaiting)
                        Draw.buffer.DrawString("LOADING", Draw.f_Maestro, Draw.wb, 360, 500);
                    else
                        Draw.buffer.DrawString("PRESS START", Draw.f_Maestro, Draw.wb, 350, 500);
                    break;
                case 1:
                    Draw.buffer.DrawString("login", DefaultFont, Draw.wb, 5, 5);
                    login.render();
                    break;
                case 2:
                    //drawBuffer.DrawString("game select", DefaultFont, new SolidBrush(Color.White), 5, 5);
                    gSel.render();
                    break;
                case 3:
                    Draw.buffer.DrawString("mode select", DefaultFont, Draw.wb, 5, 5);
                    mSel.render();
                    ModeSelect.ModeSelObj mode = mSel.modes[mSel.game][mSel.selection];
                    if (mode.enabled[0]) //draw hiscores //TODO: this check should be obsolete after all are playable
                        for (int i = 0; i < 6; i++)
                        {
                            int hX = 300;
                            int hY = 200;
                            if (hiscoreTable[i].lineC == 1)
                            {
                                Draw.buffer.DrawLine(new Pen(Color.LimeGreen), hX - 10, hY + 11 + 30 * i, hX + 160, hY + 11 + 30 * i);
                            }
                            if (hiscoreTable[i].lineC == 2)
                            {
                                Draw.buffer.DrawLine(new Pen(Color.Orange), hX - 10, hY + 11 + 30 * i, hX + 300, hY + 11 + 30 * i);
                            }
                            Draw.buffer.DrawString(hiscoreTable[i].username, DefaultFont, Draw.wb, hX, hY + 30 * i);
                            if(mode.id != Mode.ModeType.CCS)
                                Draw.buffer.DrawString(hiscoreTable[i].level.ToString(), DefaultFont, Draw.wb, hX + 40, hY + 30 * i);
                            if (mode.id == Mode.ModeType.EASY)
                                Draw.buffer.DrawString(hiscoreTable[i].score.ToString(), DefaultFont, Draw.wb, hX + 80, hY + 30 * i);
                            else if(mode.id != Mode.ModeType.CCS)
                                Draw.buffer.DrawString(rules.mod.grades[hiscoreTable[i].grade], DefaultFont, Draw.wb, hX + 80, hY + 30 * i);
                            else
                                Draw.buffer.DrawString(rules.mod.grades[hiscoreTable[i].grade], DefaultFont, Draw.wb, hX + 40, hY + 30 * i);
                            var temptimeVAR = hiscoreTable[i].time;
                            var min = (int)Math.Floor((double)temptimeVAR / 60000);
                            temptimeVAR -= min * 60000;
                            var sec = (int)Math.Floor((double)temptimeVAR / 1000);
                            temptimeVAR -= sec * 1000;
                            var msec = (int)temptimeVAR;
                            var msec10 = (int)(msec / 10);
                            Draw.buffer.DrawString(string.Format("{0,2:00}:{1,2:00}:{2,2:00}", min, sec, msec10), DefaultFont, Draw.wb, hX + 110, hY + 30 * i);
                            if (mSel.game > 1)
                            {
                                for (int j = 0; j < 6; j++)
                                {
                                    if (hiscoreTable[i].medals[j] > 0)
                                        Draw.buffer.DrawImage(medalImg, new Rectangle(hX + 170 + 26 * j, hY + 30 * i, 25, 15), j * 26, (hiscoreTable[i].medals[j] - 1) * 16, 25, 16, GraphicsUnit.Pixel);
                                    //drawBuffer.DrawString(hiscoreTable[i].medals[j].ToString(), DefaultFont, new SolidBrush(Color.White), hX + 170 + 10 * j, hY + 30 * i);
                                }
                            }
                        }
                    if(mode.game == GameRules.Games.TGM3)
                    {
                        List<string> gm3grades = new List<string> { "9", "8", "7", "6", "5", "4", "3", "2", "1", "S1", "S2", "S3", "S4", "S5", "S6", "S7", "S8", "S9", "m1", "m2", "m3", "m4", "m5", "m6", "m7", "m8", "m9", "M", "MK", "MV", "MO", "MM", "GM" };
                        Draw.buffer.DrawString("Qualified Grade: " + gm3grades[player.TIGrade], DefaultFont, Draw.wb, 200, 5);
                        for (int i = 0; i < player.TIHistory.Count; i++)
                            Draw.buffer.DrawString(gm3grades[player.TIHistory[i]], DefaultFont, Draw.wb, 200 + 10*i, 15);
                    }
                    break;
                case 4:
                    field1.draw();
                    break;
                case 7:
                    customMenu.render();
                    Draw.buffer.DrawString("custom game", DefaultFont, Draw.wb, 5, 5);
                    break;
                case 8:
                    Draw.buffer.DrawString("preferences", DefaultFont, Draw.wb, 5, 5);
                    prefs.render();
                    break;
                case 9:
                    Draw.buffer.DrawString("cheats", DefaultFont, Draw.wb, 5, 5);
                    cMen.render();
                    break;
                case 10:
                    Draw.buffer.DrawString("firstrun", DefaultFont, Draw.wb, 5, 5);
                    Draw.buffer.DrawString("INPUT KEY FOR:", SystemFonts.DefaultFont, Draw.wb, 100, 100);
                    switch (firstrunProgress)
                    {
                        case 0://prompt for up
                            Draw.buffer.DrawString("UP", SystemFonts.DefaultFont, Draw.wb, 100, 160);
                            break;
                        case 1://prompt for down
                            Draw.buffer.DrawString("DOWN", SystemFonts.DefaultFont, Draw.wb, 100, 160);
                            break;
                        case 2://prompt for left
                            Draw.buffer.DrawString("LEFT", SystemFonts.DefaultFont, Draw.wb, 100, 160);
                            break;
                        case 3://prompt for right
                            Draw.buffer.DrawString("RIGHT", SystemFonts.DefaultFont, Draw.wb, 100, 160);
                            break;
                        case 4://prompt for rot1/select
                            Draw.buffer.DrawString("ROTATION 1 / SELECT", SystemFonts.DefaultFont, Draw.wb, 100, 160);
                            break;
                        case 5://prompt for rot2/back
                            Draw.buffer.DrawString("ROTATION 2 / BACK", SystemFonts.DefaultFont, Draw.wb, 100, 160);
                            break;
                        case 6://prompt for alt rot1
                            Draw.buffer.DrawString("ALTERNATE ROTATION 1", SystemFonts.DefaultFont, Draw.wb, 100, 160);
                            break;
                        case 7://prompt for hold
                            Draw.buffer.DrawString("HOLD", SystemFonts.DefaultFont, Draw.wb, 100, 160);
                            break;
                        case 8://prompt for start
                            Draw.buffer.DrawString("START", SystemFonts.DefaultFont, Draw.wb, 100, 160);
                            break;
                    }
                    break;
            }
            if (menuState > 1)
                Draw.buffer.DrawString(player.name, Draw.f_Maestro, Draw.wb, 765, 5);

            if (drawdebug)
                Draw.buffer.DrawString(freetime.ToString(), Draw.f_Maestro, Draw.wb, 765, 20);
#if DEBUG
            /*SolidBrush debugBrush = new SolidBrush(Color.White);
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
            drawBuffer.DrawString(Keyboard.IsKeyDown(pad1.keyStart) ? "1" : "0", DefaultFont, debugBrush, 74, 730);*/

#endif

            //draw the buffer, then set to refresh
            //BackgroundImage = imgBuffer;
            Invalidate();

        }

        private int checkExam()
        {
            List<int> cream = new List<int>(player.TIHistory);

            //cream = player.TIHistory;
            cream.Sort();

            int avg = (cream[5] + cream[4] + cream[3])/3;

            Random rand = new Random();
            if (avg > player.TIGrade && rand.Next(2) == 0)
                return avg;
            return -1;
            
        }

        private void setupCustomGame()
        {
            Audio.stopMusic();
            rules = new GameRules();
            rules.setup((GameRules.Games)customMenu.game, Mode.ModeType.CUSTOM, 0);
            rules.mod.ModeName = "CUSTOM";
            rules.mod.border = Color.DarkGreen;
            rules.generator = (GameRules.Gens)customMenu.generator;
            rules.rotation = (GameRules.Rots)customMenu.rotation;
            rules.mod.g20 = customMenu.grav == 1;
            rules.mod.bigmode = customMenu.big;
            rules.mod.invpiece = customMenu.invPiece;
            rules.mod.invpreview = customMenu.invPreview;
            rules.mod.invstack = customMenu.invStack;

            pad1.southpaw = prefs.southpaw;
            FPS = rules.FPS;
            field1 = new Field(pad1, rules, -1, false);
            field1.disableGoldFlash = !prefs.flashing;
            field1.custom = true;
            field1.g0 = customMenu.grav == 2;
        }

        private void setupGame()
        {
            Audio.stopMusic();
            rules = new GameRules();
            ModeSelect.ModeSelObj mode = mSel.modes[mSel.game][mSel.selection];
            rules.setup(mode.game, mode.id, mSel.variant[mSel.selection]);

            //m.mute = prefs.muted;

            if (player.name == "   ")
            {
                if (!rules.mod.bigmode)
                    rules.mod.bigmode = cMen.cheats[3];
                Audio.muted = cMen.cheats[4];
            }
            else if (mode.game == GameRules.Games.TGM3 && mode.id == Mode.ModeType.MASTER)
                rules.exam = checkExam();

            if (prefs.delay)
                pad1.setLag(rules.lag);
            pad1.southpaw = prefs.southpaw;
            FPS = rules.FPS;

            field1 = new Field(pad1, rules, -1, false);
            if (player.name == "   ")
            {
                field1.godmode = cMen.cheats[0];
                field1.cheating = true;
                if (cMen.cheats[1])
                    field1.MOD.g20 = cMen.cheats[1];
                field1.g0 = cMen.cheats[2];
                if (cMen.cheats[5])
                    field1.w4 = true;
                if (cMen.cheats[6])
                    field1.toriless = true;
            }
            field1.disableGoldFlash = !prefs.flashing;
            saved = false;
        }

        private bool testHiscore(GameResult gameResult)
        {
            int m = mSel.selection + 1;
            loadHiscores(gameResult.game, m);
            if (hiscoreTable.Count == 0) //oh no! error reading the file!
                return false;

            switch (gameResult.game)
            {
                /*case 1:
                    for (int i = 0; i < hiscoreTable.Count; i++) //for each entry in TGM1
                    {
                        if (hiscoreTable[i].grade < gameResult.grade)
                        {
                            saveHiscore(gameResult, gameResult.game, m, i);
                            return true;
                        }
                        if (hiscoreTable[i].grade == gameResult.grade)
                        {
                            //compare time
                            if (hiscoreTable[i].time > gameResult.time)
                            {
                                saveHiscore(gameResult, gameResult.game, m, i);
                                return true;
                            }
                        }
                        //else try the next one.
                    }
                    break;*/
                case 4:
                    for (int i = 0; i < hiscoreTable.Count; i++)
                    {
                        if (gameResult.mode == Mode.ModeType.EASY) //rank by score
                        {
                            if(hiscoreTable[i].score < gameResult.score)
                            {
                                saveHiscore(gameResult, m, i);
                                return true;
                            }
                            if(hiscoreTable[i].score == gameResult.score)
                            {
                                if (hiscoreTable[i].lineC < gameResult.lineC)
                                {
                                    saveHiscore(gameResult, m, i);
                                    return true;
                                }
                                if(hiscoreTable[i].lineC == gameResult.lineC)
                                {
                                    if(hiscoreTable[i].time < gameResult.time)
                                    {
                                        saveHiscore(gameResult, m, i);
                                        return true;
                                    }
                                }
                            }
                        }
                        else
                        {
                            if (hiscoreTable[i].level < gameResult.level)
                            {
                                saveHiscore(gameResult, m, i);
                                return true;
                            }
                            if (hiscoreTable[i].level == gameResult.level)
                            {
                                if (hiscoreTable[i].grade < gameResult.grade)
                                {
                                    saveHiscore(gameResult, m, i);
                                    return true;
                                }
                                if (hiscoreTable[i].grade == gameResult.grade)
                                {
                                    if (hiscoreTable[i].time > gameResult.time)
                                    {
                                        saveHiscore(gameResult, m, i);
                                        return true;
                                    }
                                }
                            }
                        }

                    }
                    break;
                default:
                    for (int i = 0; i < hiscoreTable.Count; i++)
                    {
                        if (hiscoreTable[i].grade < gameResult.grade)
                        {
                            saveHiscore(gameResult, m, i);
                            return true;
                        }
                        else if (hiscoreTable[i].grade == gameResult.grade)
                        {
                            //compare line
                            if (hiscoreTable[i].lineC < gameResult.lineC)
                            {
                                saveHiscore(gameResult, m, i);
                                return true;
                            }
                            //compare level
                            if (hiscoreTable[i].level < gameResult.level)
                            {
                                saveHiscore(gameResult, m, i);
                                return true;
                            }
                            //compare time
                            if (hiscoreTable[i].time > gameResult.time)
                            {
                                saveHiscore(gameResult, m, i);
                                return true;
                            }
                        }
                    }
                    break;
            }
            return false;
        }

        private void writeReplay()
        {
            var tim = System.DateTime.Now.ToString("G", new CultureInfo("ja-JP"));
            tim = tim.Replace("/", "_").Replace(":", "_");
            string repFile = "Sav/Replays/"+tim+" "+rules.GameName+" "+rules.mod.ModeName+".rep";
            FileStream fsStream;
            try
            {
                fsStream = new FileStream(repFile, FileMode.Create);
            }
            catch (DirectoryNotFoundException)
            {
                Directory.CreateDirectory("Sav/Replays/");
                fsStream = new FileStream(repFile, FileMode.Create);
            }
            using (BinaryWriter sw = new BinaryWriter(fsStream, Encoding.UTF8))
            {
                sw.Write(player.name);
                byte temp = (byte)(1 + ((prefs.southpaw ? 1 : 0) << 7));//if i don't explicitly store this in a byte,
                sw.Write(temp); //this stores an int instead of a byte, breaking compat
                sw.Write(pad1.replay.Count);
                sw.Write((int)rules.gameRules);
                sw.Write((int)(rules.mod.modeID) + (rules.mod.variant << 13));
                sw.Write(field1.seed);
                for (int i = 0; i < pad1.replay.Count; i++)
                    sw.Write(pad1.replay[i]);
            }
            field1.record = false;
            field1.recorded = true;
        }

        private bool queryReplay()
        {
            OpenFileDialog box = new OpenFileDialog();
            box.Filter = "replay files (*.rep)|*.rep";
            box.RestoreDirectory = true;

            if (box.ShowDialog() == DialogResult.OK)
            {
                fileName = box.FileName;
                return true;
            }
            else
                return false;
            
        }

        private void loadReplay()
        {
            FileStream fs = new FileStream(fileName, FileMode.Open);
            BinaryReader sr = new BinaryReader(fs);
            pad1.replay = new List<short>();
            repNam = sr.ReadString();
            byte temp = sr.ReadByte();
            byte ver = (byte)(temp & 0x7F);
            pad1.southpaw = ((temp >> 7) & 0x1) == 1;
            int length = sr.ReadInt32();
            int gam = sr.ReadInt32();
            int t = sr.ReadInt32();
            int mod = t & 0x0FFF;
            int v = (t >> 13) & 0x000F;
            int s = sr.ReadInt32();
            for (int i = 0; i < length; i++)
                pad1.replay.Add(sr.ReadInt16());
            sr.Close();
            fs.Dispose();

            if (ver != 1)
                throw new Exception();
            rules = new GameRules();
            rules.setup((GameRules.Games)gam, (Mode.ModeType)mod, v);
            field1 = new Field(pad1, rules, s, true);
            Audio.stopMusic();
        }

        private void saveHiscore(GameResult gameResult, int m, int place)
        {

            hiscoreTable.Insert(place, gameResult);
            hiscoreTable.RemoveAt(hiscoreTable.Count - 1);

            string hiFile = "Sav/Hiscores/gm" + gameResult.game + m + ".hst";
            File.Delete(hiFile);
            using (FileStream fsStream = new FileStream(hiFile, FileMode.Create))
            using (BinaryWriter sw = new BinaryWriter(fsStream, Encoding.UTF8))
            {
                for (int i = 0; i < hiscoreTable.Count; i++)
                {
                    sw.Write(hiscoreTable[i].username);
                    if (gameResult.mode == Mode.ModeType.EASY)
                        sw.Write(hiscoreTable[i].score);
                    else
                        sw.Write(hiscoreTable[i].grade);
                    sw.Write(hiscoreTable[i].time);
                    for (int j = 0; j < 6; j++)
                        sw.Write((byte)(hiscoreTable[i].medals[j] & 0xFF));
                    sw.Write((byte)hiscoreTable[i].lineC);
                    sw.Write((int)hiscoreTable[i].level);
                }
            }
        }
        
        private void loadHiscores(int game, int mode)
        {
            string filename = "Sav/Hiscores/gm" + game.ToString() + mode.ToString() + ".hst";
            if (!File.Exists(filename))
            {
                if (game == 1 && mode == 1)
                    defaultTGMScores();
                else if (game == 2 && mode == 1)
                    defaultTGM2Scores();
                else if (game == 3 && mode == 2)
                    defaultTAPScores();
                else if (game == 4 && mode == 2)
                    defaultTGM3Scores();
                else
                    defaultGenericScores(game, mode);
            }
            hiscoreTable = new List<GameResult>();
            BinaryReader scores = new BinaryReader(File.OpenRead(filename));
            //otherwise, load up the hiscores into memory!
            
            while (true)
            {

                GameResult tempRes = new GameResult();

                tempRes.username = scores.ReadString();
                if (game == 4 && mode == 1) //ugh, easy check
                    tempRes.score = scores.ReadInt32();
                else
                    tempRes.grade = scores.ReadInt32();
                tempRes.time = scores.ReadInt64();
                tempRes.medals = new List<int>();
                for (int i = 0; i < 6; i++)
                {
                    tempRes.medals.Add(scores.ReadByte());
                }
                tempRes.lineC = scores.ReadByte();
                tempRes.level = scores.ReadInt32();
                hiscoreTable.Add(tempRes);
                if (scores.BaseStream.Position == scores.BaseStream.Length)
                    break;
            }
            scores.Close();
        }

        public bool defaultTGMScores()
        {
            FileStream fsStream;
            try
            {
                fsStream = new FileStream("Sav/Hiscores/gm11.hst", FileMode.Create);
            }
            catch (DirectoryNotFoundException)
            {
                Directory.CreateDirectory("Sav/Hiscores/");
                fsStream = new FileStream("Sav/Hiscores/gm11.hst", FileMode.Create);
            }
            using (BinaryWriter sw = new BinaryWriter(fsStream, Encoding.UTF8))
            {
                long temptime;
                sw.Write("SAK");
                sw.Write(11);
                temptime = 540000;
                sw.Write(temptime);
                sw.Write(new byte[7]);
                sw.Write(0);
                sw.Write("CHI");
                sw.Write(10);
                temptime = 480000;
                sw.Write(temptime);
                sw.Write(new byte[7]);
                sw.Write(0);
                sw.Write("NAI");
                sw.Write(9);
                temptime = 420000;
                sw.Write(temptime);
                sw.Write(new byte[7]);
                sw.Write(0);
                sw.Write("MIZ");
                sw.Write(6);
                temptime = 360000;
                sw.Write(temptime);
                sw.Write(new byte[7]);
                sw.Write(0);
                sw.Write("KAR");
                sw.Write(5);
                temptime = 300000;
                sw.Write(temptime);
                sw.Write(new byte[7]);
                sw.Write(0);
                sw.Write("NAG");
                sw.Write(4);
                temptime = 240000;
                sw.Write(temptime);
                sw.Write(new byte[7]);
                sw.Write(0);
            }
            return true;
        }
        public bool defaultTGM2Scores()
        {
            FileStream fsStream;
            try
            {
                fsStream = new FileStream("Sav/Hiscores/gm21.hst", FileMode.Create);
            }
            catch (DirectoryNotFoundException)
            {
                Directory.CreateDirectory("Sav/Hiscores/");
                fsStream = new FileStream("Sav/Hiscores/gm21.hst", FileMode.Create);
            }
            using (BinaryWriter sw = new BinaryWriter(fsStream, Encoding.UTF8))
            {
                long temptime;
                sw.Write("T.A");
                sw.Write(9);
                temptime = 1200000;
                sw.Write(temptime);
                sw.Write(new byte[7]);
                sw.Write(0);
                sw.Write("T.A");
                sw.Write(6);
                temptime = 1080000;
                sw.Write(temptime);
                sw.Write(new byte[7]);
                sw.Write(0);
                sw.Write("T.A");
                sw.Write(3);
                temptime = 960000;
                sw.Write(temptime);
                sw.Write(new byte[7]);
                sw.Write(0);
                sw.Write("T.A");
                sw.Write(3);
                temptime = 1200000;
                sw.Write(temptime);
                sw.Write(new byte[7]);
                sw.Write(0);
                sw.Write("T.A");
                sw.Write(2);
                temptime = 1080000;
                sw.Write(temptime);
                sw.Write(new byte[7]);
                sw.Write(0);
                sw.Write("T.A");
                sw.Write(1);
                temptime = 960000;
                sw.Write(temptime);
                sw.Write(new byte[7]);
                sw.Write(0);
            }
            return true;
        }

        public bool defaultTAPScores()
        {
            FileStream fsStream;
            try
            {
                fsStream = new FileStream("Sav/Hiscores/gm32.hst", FileMode.Create);
            }
            catch (DirectoryNotFoundException)
            {
                Directory.CreateDirectory("Sav/Hiscores/");
                fsStream = new FileStream("Sav/Hiscores/gm32.hst", FileMode.Create);
            }
            using (BinaryWriter sw = new BinaryWriter(fsStream, Encoding.UTF8))
            {
                long temptime;
                sw.Write("T.A");
                sw.Write(9);
                temptime = 1200000;
                sw.Write(temptime);
                sw.Write(new byte[7]);
                sw.Write(0);
                sw.Write("T.A");
                sw.Write(6);
                temptime = 1080000;
                sw.Write(temptime);
                sw.Write(new byte[7]);
                sw.Write(0);
                sw.Write("T.A");
                sw.Write(3);
                temptime = 960000;
                sw.Write(temptime);
                sw.Write(new byte[7]);
                sw.Write(0);
                sw.Write("T.A");
                sw.Write(3);
                temptime = 1200000;
                sw.Write(temptime);
                sw.Write(new byte[7]);
                sw.Write(0);
                sw.Write("T.A");
                sw.Write(2);
                temptime = 1080000;
                sw.Write(temptime);
                sw.Write(new byte[7]);
                sw.Write(0);
                sw.Write("T.A");
                sw.Write(1);
                temptime = 960000;
                sw.Write(temptime);
                sw.Write(new byte[7]);
                sw.Write(0);
            }
            return true;
        }
        public bool defaultTGM3Scores()
        {
            FileStream fsStream;
            try
            {
                fsStream = new FileStream("Sav/Hiscores/gm42.hst", FileMode.Create);
            }
            catch (DirectoryNotFoundException)
            {
                Directory.CreateDirectory("Sav/Hiscores/");
                fsStream = new FileStream("Sav/Hiscores/gm42.hst", FileMode.Create);
            }
            using (BinaryWriter sw = new BinaryWriter(fsStream, Encoding.UTF8))
            {
                long temptime;
                sw.Write("ARK");
                sw.Write(6);
                temptime = 1200000;
                sw.Write(temptime);
                sw.Write(new byte[7]);
                sw.Write(500);
                sw.Write("ARK");
                sw.Write(4);
                temptime = 1080000;
                sw.Write(temptime);
                sw.Write(new byte[7]);
                sw.Write(400);
                sw.Write("ARK");
                sw.Write(3);
                temptime = 1200000;
                sw.Write(temptime);
                sw.Write(new byte[7]);
                sw.Write(400);
                sw.Write("ARK");
                sw.Write(2);
                temptime = 960000;
                sw.Write(temptime);
                sw.Write(new byte[7]);
                sw.Write(300);
                sw.Write("ARK");
                sw.Write(2);
                temptime = 1080000;
                sw.Write(temptime);
                sw.Write(new byte[7]);
                sw.Write(300);
                sw.Write("ARK");
                sw.Write(1);
                temptime = 960000;
                sw.Write(temptime);
                sw.Write(new byte[7]);
                sw.Write(200);
            }
            return true;
        }

        public bool defaultGenericScores(int game, int mode)
        {
            FileStream fsStream;
            try
            {
                fsStream = new FileStream("Sav/Hiscores/gm" + game.ToString() + mode.ToString() + ".hst", FileMode.Create);
            }
            catch (DirectoryNotFoundException)
            {
                Directory.CreateDirectory("Sav/Hiscores/");
                fsStream = new FileStream("Sav/Hiscores/gm" + game.ToString() + mode.ToString() + ".hst", FileMode.Create);
            }
            using (BinaryWriter sw = new BinaryWriter(fsStream, Encoding.UTF8))
            {
                for (int i = 0; i < 6; i++)
                {
                    sw.Write("NIL");
                    sw.Write(0);
                    sw.Write((long)0);
                    sw.Write(new byte[7]);
                    sw.Write(0);
                }
            }
            return true;
        }

        private void savePrefs()
        {
            using (FileStream fsStream = new FileStream("Sav/prefs.dat", FileMode.Create))
            using (BinaryWriter sw = new BinaryWriter(fsStream, Encoding.UTF8))
            {
                sw.Write(prefs.delay);
                sw.Write((char)prefs.nPad.keyUp);
                sw.Write((char)prefs.nPad.keyDown);
                sw.Write((char)prefs.nPad.keyLeft);
                sw.Write((char)prefs.nPad.keyRight);
                sw.Write((char)prefs.nPad.keyRot1);
                sw.Write((char)prefs.nPad.keyRot2);
                sw.Write((char)prefs.nPad.keyRot3);
                sw.Write((char)prefs.nPad.keyHold);
                sw.Write((char)prefs.nPad.keyStart);
                byte temp = (byte)(((Audio.musVol & 0xF) << 4) + (Audio.sfxVol & 0xF));
                sw.Write(temp);
                sw.Write(player.name);
                sw.Write((prefs.southpaw?1:0)+(prefs.flashing?2:0));
            }
        }

        private void readPrefs()
        {
            try
            {
                BinaryReader prf = new BinaryReader(File.OpenRead("Sav/prefs.dat"));
                prefs.delay = prf.ReadBoolean();
                prefs.nPad.keyUp = (Key)prf.ReadByte();
                prefs.nPad.keyDown = (Key)prf.ReadByte();
                prefs.nPad.keyLeft = (Key)prf.ReadByte();
                prefs.nPad.keyRight = (Key)prf.ReadByte();
                prefs.nPad.keyRot1 = (Key)prf.ReadByte();
                prefs.nPad.keyRot2 = (Key)prf.ReadByte();
                prefs.nPad.keyRot3 = (Key)prf.ReadByte();
                prefs.nPad.keyHold = (Key)prf.ReadByte();
                prefs.nPad.keyStart = (Key)prf.ReadByte();
                byte temp = prf.ReadByte();
                Audio.musVol = (temp >> 4) & 0x0F;
                Audio.sfxVol = temp & 0x0F;
                player.name = prf.ReadString();
                temp = prf.ReadByte();
                prefs.southpaw = (temp & 0x01) == 1 ? true : false;
                prefs.flashing = (temp & 0x02) == 2 ? true : false;
                prf.Close();
            }
            catch (DirectoryNotFoundException)
            {
                createFolder("Sav/");
                changeMenu(10);
            }
            catch (FileNotFoundException)
            {
                changeMenu(10);
            }
        }

        private void createFolder(string nom)
        {
            Directory.CreateDirectory(nom);
        }
    }
}
