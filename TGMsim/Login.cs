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
        int delaytimer = 5;
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
                        delaytimer = 5;
                        if (menuSelection == 3) //then check if it's a registered nick, else register it!
                        {
                            temp.name = getLetter(username[0]) + getLetter(username[1]) + getLetter(username[2]);
                            if (File.Exists(temp.name + ".usr"))
                            {
                                if (readUserData())
                                    registering = false;
                                else
                                {
                                    menuSelection = 0;
                                    loginErr = 1;
                                }
                            }
                            else
                                registering = true;
                            loginErr = 0;


                        }
                        else username[menuSelection] = username[menuSelection - 1];
                    }
                    else if (pad1.inputRot2 == 1 && menuSelection != 0)
                    {
                        menuSelection -= 1;
                        delaytimer = 5;
                    }

                    if (pad1.inputH == 1)
                    {
                        username[menuSelection] = (username[menuSelection] + 1) % 46; //increase the currently selected letter
                        delaytimer = 5;
                    }
                    else if (pad1.inputH == -1)
                    {
                        
                        username[menuSelection] = (username[menuSelection] - 1) % 46;//decrease the currently selected letter
                        if (username[menuSelection] == -1)
                        {
                            username[menuSelection] = 45;
                        }
                        delaytimer = 5;
                    }
                }
                else //password stuff
                {
                    if (registering) //input and verify new pass
                    {
                        if (menuSelection == 5) // wait for start!!!
                        {
                            writeUser();
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
                            readUserData();
                            loggedin = true;
                        }
                        if (menuSelection == 4)//verify
                        {
                            if (pad1.inputStart == 1)
                            {
                                if (!startPressed)
                                {
                                    if (verifyPass.SequenceEqual(tempPass))//if they match
                                    {
                                        menuSelection = 5;
                                    }
                                    else//if they don't
                                    {
                                        menuSelection = 2;
                                        loginErr = 2;
                                        verifyPass.Clear();
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
                            readPass();
                            if (verifyPass.Count == 0)
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
                delaytimer -= 1;
        }

        public void render(Graphics drawBuffer)
        {
            if (menuSelection == 3 || menuSelection == 4)
            {
                if (tempPass.Count == 0)
                drawBuffer.DrawString("Press Start to confirm inputs!", SystemFonts.DefaultFont, new SolidBrush(Color.White), 350, 360);
                
                for(int i = 0; i < tempPass.Count; i++)
                    drawBuffer.DrawString("*", SystemFonts.DefaultFont, new SolidBrush(Color.White), 400 + 10*i, 320);

                for (int i = 0; i < verifyPass.Count; i++)
                    drawBuffer.DrawString("*", SystemFonts.DefaultFont, new SolidBrush(Color.White), 400 + 10 * i, 340);
            }

            if (loginErr == 1)
                drawBuffer.DrawString("There was an error reading the profile specified. Try again or delete the file.", SystemFonts.DefaultFont, new SolidBrush(Color.White), 200, 200);
            if (loginErr == 2)
                drawBuffer.DrawString("That password was incorrect. Please try again.", SystemFonts.DefaultFont, new SolidBrush(Color.White), 200, 200);
            drawBuffer.DrawString(getLetter(username[0]), SystemFonts.DefaultFont, new SolidBrush(Color.White), 400, 300);
            if (menuSelection > 0)
            drawBuffer.DrawString(getLetter(username[1]), SystemFonts.DefaultFont, new SolidBrush(Color.White), 409, 300);
            if (menuSelection > 1)
            drawBuffer.DrawString(getLetter(username[2]), SystemFonts.DefaultFont, new SolidBrush(Color.White), 418, 300);
            //if (menuSelection < 3)
            //    drawBuffer.DrawString("↑", SystemFonts.DefaultFont, new SolidBrush(Color.White), 398 + (9*menuSelection), 315);

#if DEBUG
            if (menuSelection == 3 || menuSelection == 4)
            {
                drawBuffer.DrawString(tempPass.Count.ToString(), SystemFonts.DefaultFont, new SolidBrush(Color.White), 380, 320);
                drawBuffer.DrawString(verifyPass.Count.ToString(), SystemFonts.DefaultFont, new SolidBrush(Color.White), 380, 340);
            }

            drawBuffer.DrawString("Last Input: " + lastinput.ToString(), SystemFonts.DefaultFont, new SolidBrush(Color.White), 500, 500);
#endif
        }

        string getLetter(int i)
        {
            return "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789!?.#$%&'ß ".Substring(i, 1);
        }

        public bool writeUser()
        {
            using (FileStream fsStream = new FileStream(temp.name + ".usr", FileMode.Create))
            using (BinaryWriter sw = new BinaryWriter(fsStream, Encoding.UTF8))
            {
                sw.Write(temp.name);
                sw.Write((byte)0x02);//version number
                //one byte for password, the first bit if pass-protected, three bits for length (up to six) and then two bits for each digit (four possible inputs each, ABCH)
                UInt16 passData = new UInt16();
                if (!temp.passProtected)
                {
                    sw.Write(passData);
                }
                else
                {
                    passData += 0x8000;
                    passData += (UInt16)((tempPass.Count & 0x7) << 12);//password length
                    for (int i = 0; i < tempPass.Count; i++)
                    {
                        passData += (UInt16)(tempPass[i] << (10 - 2 * i));
                    }
                    sw.Write(passData);
                    sw.Write(new byte[4]);//global points
                    sw.Write(new byte[4]);//TGM3 points
                    sw.Write(new byte[2]);//Official GM certifications (1, 2, tap, tap death, 3, 3 shirase, konoha)
                    sw.Write(new byte[2]);//endless shirase hiscore?
                    sw.Write(new byte[8]);//current TI grade + previous seven rankings
                }
            }
            return false;
        }

        public bool readPass()
        {
            //read the pass and put it into verifyPass
            BinaryReader file = new BinaryReader(File.OpenRead(temp.name + ".usr"));
            if (file.ReadString() != temp.name)//read name
                return false;
            if (file.ReadByte() != 0x02)//read save version, compare to current
                return false;
            //read and parse the password
            UInt16 passdata = file.ReadUInt16();
            if (passdata >> 15 == 1)//pass protected
            {
                if (((passdata >> 12) & 0x3) == 7)//invalid pass length
                {
                    return false;
                }
                verifyPass.Add((byte)((passdata >> 10) & 0x0003));
                verifyPass.Add((byte)((passdata >> 8) & 0x0003));
                verifyPass.Add((byte)((passdata >> 6) & 0x0003));
                verifyPass.Add((byte)((passdata >> 4) & 0x0003));
                verifyPass.Add((byte)((passdata >> 2) & 0x0003));
                verifyPass.Add((byte)(passdata & 0x0003));
            }
            return true;
        }

        public bool readUserData()
        {
            //read the user data and pass it to the game
            BinaryReader file = new BinaryReader(File.OpenRead(temp.name + ".usr"));
            if (file.ReadString() != temp.name)//read name
                return false;
            if (file.ReadByte() != 0x02)//read save version, compare to current
                return false;
            //read and parse the password
            UInt16 passdata = file.ReadUInt16();
            /*if (passdata >> 15 == 1)//pass protected
            {
                if (((passdata >> 12) & 0x3) == 7)//invalid pass length
                {
                    return false;
                }
                verifyPass.Add((byte)((passdata >> 10) & 0x0003));
                verifyPass.Add((byte)((passdata >> 8) & 0x0003));
                verifyPass.Add((byte)((passdata >> 6) & 0x0003));
                verifyPass.Add((byte)((passdata >> 4) & 0x0003));
                verifyPass.Add((byte)((passdata >> 2) & 0x0003));
                verifyPass.Add((byte)(passdata & 0x0003));
            }*/
            //global points
            //tgm3 points
            //GM certs
            //shirase
            //TI grade data
            return true;
        }

        
    }
}
