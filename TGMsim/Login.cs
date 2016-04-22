using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace TGMsim
{
    class Login
    {
        public Profile temp = new Profile();
        List<int> username = new List<int>{0, 0, 0};
        List<byte> tempPass = new List<byte>();
        List<byte> verifyPass = new List<byte>();
        bool registering;
        int menuSelection;
        public bool loggedin = false;
        int delaytimer = 10;
        int lastinput = 0;
        bool startPressed = false;
        int loginErr;

        public Login()
        {
            menuSelection = 0;
        }

        public void logic(Controller pad1)
        {
            if (delaytimer == 0)
            {
                if (menuSelection < 3)
                {
                    if ((pad1.inputRot1 | pad1.inputRot3) == 1)
                    {
                        menuSelection += 1;
                        delaytimer = 10;
                        if (menuSelection == 3) //then check if it's a registered nick, else register it!
                        {
                            temp.name = getLetter(username[0]) + getLetter(username[1]) + getLetter(username[2]);

                            if (temp.name == "   ")
                            {
                                //skip user creation
                                loggedin = true;
                                pad1.inputPressedRot1 = false;
                                pad1.inputPressedRot2 = false;
                                pad1.inputPressedRot3 = false;
                            }

                            if (File.Exists("Sav/" + temp.name + ".usr"))
                            {
                                if (temp.readUserData())
                                    registering = false;
                                else
                                {
                                    menuSelection = 0;
                                    loginErr = 1;
                                }
                            }
                            else
                            {
                                registering = true;
                                loginErr = 0;
                            }


                        }
                        else username[menuSelection] = username[menuSelection - 1];
                    }
                    else if (pad1.inputRot2 == 1 && menuSelection != 0)
                    {
                        menuSelection -= 1;
                        delaytimer = 10;
                    }

                    if (pad1.inputH == 1)
                    {
                        username[menuSelection] = (username[menuSelection] + 1) % 46; //increase the currently selected letter
                        delaytimer = 10;
                    }
                    else if (pad1.inputH == -1)
                    {

                        username[menuSelection] = (username[menuSelection] - 1) % 46;//decrease the currently selected letter
                        if (username[menuSelection] == -1)
                        {
                            username[menuSelection] = 45;
                        }
                        delaytimer = 10;
                    }
                }
                else //password stuff
                {
                    if (registering) //input and verify new pass
                    {
                        if (menuSelection == 5) // wait for start!!!
                        {
                            temp.createUser();
                            loggedin = true;
                        }
                        if (menuSelection == 4) //verify password input
                        {
                            if (pad1.inputStart == 1)
                            {
                                if (!startPressed)
                                {
                                    if (verifyPass.SequenceEqual(tempPass))//if they match
                                    {
                                        temp.password = tempPass;
                                        menuSelection = 5;
                                    }
                                    else//if they don't
                                    {
                                        tempPass.Clear();
                                        verifyPass.Clear();
                                        loginErr = 2;
                                        menuSelection = 2;
                                    }
                                }
                                startPressed = true;
                            }
                            else if (verifyPass.Count < 6)
                            {
                                //read input, then add to the list
                                startPressed = false;
                                //wait until input's released or a new one is added

                                if (pad1.inputRot1 == 1)
                                {
                                    verifyPass.Add((byte)0x0);
                                }
                                if (pad1.inputRot2 == 1)
                                {
                                    verifyPass.Add((byte)0x1);
                                }
                                if (pad1.inputRot3 == 1)
                                {
                                    verifyPass.Add((byte)0x2);
                                }
                                if (pad1.inputHold == 1)
                                {
                                    verifyPass.Add((byte)0x3);
                                }

                            }
                        }
                        if (menuSelection == 3) //start password input
                        {
                            if (pad1.inputStart == 1)
                            {
                                if (!startPressed)
                                {
                                    startPressed = true;
                                    if (tempPass.Count == 0)
                                    {
                                        temp.passProtected = false;
                                        menuSelection = 5;
                                    }
                                    else
                                    {
                                        temp.passProtected = true;
                                        menuSelection = 4;
                                    }
                                }
                            }
                            else if (tempPass.Count < 6)
                            {
                                //read input, then add to the list
                                startPressed = false;
                                //wait until input's released or a new one is added
                                if (pad1.inputRot1 == 1)
                                {
                                    tempPass.Add((byte)0x0);
                                }
                                if (pad1.inputRot2 == 1)
                                {
                                    tempPass.Add((byte)0x1);
                                }
                                if (pad1.inputRot3 == 1)
                                {
                                    tempPass.Add((byte)0x2);
                                }
                                if (pad1.inputHold == 1)
                                {
                                    tempPass.Add((byte)0x3);
                                }

                            }
                        }
                    }
                    else //verify the pass and compare it
                    {
                        if (menuSelection == 5)//login
                        {
                            temp.readUserData();
                            loggedin = true;
                        }
                        if (menuSelection == 4)//verify
                        {
                            if (pad1.inputStart == 1)
                            {
                                if (!startPressed)
                                {
                                    if (temp.password.SequenceEqual(tempPass))//if they match
                                    {
                                        menuSelection = 5;
                                    }
                                    else//if they don't
                                    {
                                        menuSelection = 2;
                                        loginErr = 2;
                                        tempPass.Clear();
                                    }
                                }
                                startPressed = true;
                            }
                            else if (tempPass.Count < 6)
                            {
                                //read input, then add to the list
                                startPressed = false;
                                //wait until input's released or a new one is added

                                if (pad1.inputRot1 == 1)
                                {
                                    tempPass.Add((byte)0x0);
                                }
                                if (pad1.inputRot2 == 1)
                                {
                                    tempPass.Add((byte)0x1);
                                }
                                if (pad1.inputRot3 == 1)
                                {
                                    tempPass.Add((byte)0x2);
                                }
                                if (pad1.inputHold == 1)
                                {
                                    tempPass.Add((byte)0x3);
                                }

                            }
                        }
                        if (menuSelection == 3)//setup
                        {
                            temp.readPass();
                            if (temp.passProtected == false)
                            {
                                menuSelection = 5;
                            }
                            else
                                menuSelection = 4;
                        }
                    }
                }
            }
            else
                if (pad1.inputH == 0)
                    delaytimer = 0;
                else
                delaytimer -= 1;
        }

        public void render(Graphics drawBuffer)
        {
            if (menuSelection == 3 || menuSelection == 4)
            {
                if (tempPass.Count == 0)
                drawBuffer.DrawString("Press Start to confirm inputs!", SystemFonts.DefaultFont, new SolidBrush(Color.White), 300, 260);
                
                for(int i = 0; i < tempPass.Count; i++)
                    drawBuffer.DrawString("*", SystemFonts.DefaultFont, new SolidBrush(Color.White), 350 + 10*i, 220);

                for (int i = 0; i < verifyPass.Count; i++)
                    drawBuffer.DrawString("*", SystemFonts.DefaultFont, new SolidBrush(Color.White), 350 + 10 * i, 240);
            }

            if (loginErr == 1)
                drawBuffer.DrawString("There was an error reading the profile specified. Try again or delete the file.", SystemFonts.DefaultFont, new SolidBrush(Color.White), 150, 100);
            if (loginErr == 2)
                drawBuffer.DrawString("The password doesn't match. Please try again.", SystemFonts.DefaultFont, new SolidBrush(Color.White), 150, 100);
            drawBuffer.DrawString(getLetter(username[0]), SystemFonts.DefaultFont, new SolidBrush(Color.White), 350, 200);
            if (menuSelection > 0)
            drawBuffer.DrawString(getLetter(username[1]), SystemFonts.DefaultFont, new SolidBrush(Color.White), 359, 200);
            if (menuSelection > 1)
            drawBuffer.DrawString(getLetter(username[2]), SystemFonts.DefaultFont, new SolidBrush(Color.White), 368, 200);
            //if (menuSelection < 3)
            //    drawBuffer.DrawString("↑", SystemFonts.DefaultFont, new SolidBrush(Color.White), 398 + (9*menuSelection), 315);

#if DEBUG
            if (menuSelection == 3 || menuSelection == 4)
            {
                drawBuffer.DrawString(tempPass.Count.ToString(), SystemFonts.DefaultFont, new SolidBrush(Color.White), 380, 320);
                if (registering)
                    drawBuffer.DrawString(verifyPass.Count.ToString(), SystemFonts.DefaultFont, new SolidBrush(Color.White), 380, 340);
            }

            drawBuffer.DrawString("Last Input: " + lastinput.ToString(), SystemFonts.DefaultFont, new SolidBrush(Color.White), 500, 500);
#endif
        }

        string getLetter(int i)
        {
            return "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789!?.#$%&'ß ".Substring(i, 1);
        }

        

        
    }
}
