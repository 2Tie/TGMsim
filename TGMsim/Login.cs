using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;

namespace TGMsim
{
    class Login
    {
        public Profile temp = new Profile();
        List<int> username = new List<int>{0, 0, 0};
        List<byte> tempPass = new List<byte>();
        List<byte> verifyPass = new List<byte>();
        int menuSelection;
        public bool loggedin = false;
        int delaytimer = 10;
        int loginErr;


        System.Windows.Media.MediaPlayer s_Roll = new System.Windows.Media.MediaPlayer();
        System.Windows.Media.MediaPlayer s_Accept = new System.Windows.Media.MediaPlayer();
        System.Windows.Media.MediaPlayer s_Pass = new System.Windows.Media.MediaPlayer();


        Point linePos = new Point(400, 215);
        Point lineDes = new Point(400, 215);
        Point lineEnd = new Point(410, 215);
        float tempX = 399;

        public Login()
        {
            menuSelection = 0;
            addSound(s_Roll, "/Res/Audio/SE/SEI_name_select.wav");
            addSound(s_Accept, "/Res/Audio/SE/SEB_fixa.wav");
            addSound(s_Pass, "/Res/Audio/SE/SEB_instal.wav");
        }

        public void logic(Controller pad1)
        {
            if ((pad1.inputRot1 | pad1.inputRot3) == 1 && menuSelection < 3)
            {
                pSound(s_Accept);
                menuSelection += 1;
                delaytimer = 10;
                if (menuSelection == 3) //then check if it's a registered nick, else register it!
                {
                    temp.name = getLetter(username[0]) + getLetter(username[1]) + getLetter(username[2]);

                    if (temp.name == "   ")
                    {
                        //skip user creation
                        loggedin = true;
                        loginErr = 0;
                        pad1.inputPressedRot1 = false;
                        pad1.inputPressedRot2 = false;
                        pad1.inputPressedRot3 = false;
                    }

                    string filename = temp.name.Replace("?", "§");
                    if (File.Exists("Sav/" + filename + ".usr"))
                    {
                        if (temp.readUserData())
                            menuSelection = 3;
                        else
                        {
                            menuSelection = 0;
                            loginErr = 1;
                        }
                    }
                    else
                    {
                        temp.createUser();
                        temp.readUserData();
                        loginErr = 0;
                        menuSelection = 3;
                    }


                }
                else username[menuSelection] = username[menuSelection - 1];
            }
            else if (pad1.inputRot2 == 1 && menuSelection != 0)
            {
                menuSelection -= 1;
                delaytimer = 10;
            }

            if(menuSelection == 3 && pad1.inputStart == 1)
            {
                loggedin = true;
            }

            if (delaytimer == 0 && menuSelection < 3)
            {
                if (pad1.inputH != 0)
                {
                    username[menuSelection] = (username[menuSelection] + pad1.inputH) % 41; //increase the currently selected letter by input direction
                    if (username[menuSelection] == -1)
                    {
                        username[menuSelection] = 40;
                    }
                    delaytimer = 10;
                    pSound(s_Roll);
                }
            }
            else if (pad1.inputH == 0)
                delaytimer = 0;
            else
                delaytimer -= 1;

            //line logic
            if (menuSelection < 3)
            {
                lineDes.X = 400 + menuSelection * 10;
                lineDes.Y = 215;
            }

            //tempX = linePos.X;
            tempX += (lineDes.X - tempX) / 2;
            linePos.X = (int)tempX;
            linePos.Y += (lineDes.Y - linePos.Y) / 2;

            lineEnd.X = linePos.X + 10;
            lineEnd.Y = linePos.Y;
        }

        public void render()
        {
            if (menuSelection < 3)
                Draw.buffer.DrawLine(new Pen(new SolidBrush(Color.Blue)), linePos, lineEnd);
            else
                Draw.buffer.DrawString("PRESS START TO CONFIRM", Draw.f_Maestro, Draw.wb, 300, 260);

            if (loginErr == 1)
                Draw.buffer.DrawString("There was an error reading the profile specified. Try again or delete the file.", SystemFonts.DefaultFont, Draw.wb, 150, 100);
            if (loginErr == 2)
                Draw.buffer.DrawString("The password doesn't match. Please try again.", SystemFonts.DefaultFont, Draw.wb, 150, 100);
            Draw.buffer.DrawString("Profile name: ", SystemFonts.DefaultFont, Draw.wb, 300, 200);
            Draw.buffer.DrawString(getLetter(username[0]), Draw.f_Maestro, Draw.wb, 399, 200);
            if (menuSelection > 0)
            Draw.buffer.DrawString(getLetter(username[1]), Draw.f_Maestro, Draw.wb, 409, 200);
            if (menuSelection > 1)
            Draw.buffer.DrawString(getLetter(username[2]), Draw.f_Maestro, Draw.wb, 419, 200);
        }

        string getLetter(int i)
        {
            return "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789!?.& ".Substring(i, 1);
        }

        private void addSound(System.Windows.Media.MediaPlayer plr, string uri)
        {
            plr.IsMuted = true;
            plr.Open(new Uri(Environment.CurrentDirectory + uri));
        }

        void pSound(System.Windows.Media.MediaPlayer snd)
        {
            snd.IsMuted = false;
            snd.Position = new TimeSpan(0);
            snd.Play();
        }

        
    }
}
