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
        List<byte> tempPass;
        List<byte> verifyPass;
        bool registering;
        int menuSelection;
        public bool loggedin = false;
        int delaytimer = 5;

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
                    if (pad1.inputRot1 == 1)
                    {
                        menuSelection += 1;
                        delaytimer = 5;
                        if (menuSelection == 3) //then check if it's a registered nick, else register it!
                        {
                            string nick = getLetter(username[0]) + getLetter(username[1]) + getLetter(username[2]);
                            if (File.Exists(nick + ".usr"))
                            {
                                registering = false;
                            }
                            else  //for now we're just making a new one each time, sorry :P
                                registering = true;


                        }
                    }
                    else if (pad1.inputRot2 == 1 && menuSelection != 0)
                    {
                        menuSelection -= 1;
                        delaytimer = 5;
                    }

                    if (pad1.inputH == 1)
                    {
                        username[menuSelection] = (username[menuSelection] + 1) % 26; //increase the currently selected letter
                        delaytimer = 5;
                    }
                    else if (pad1.inputH == -1)
                    {
                        
                        username[menuSelection] = (username[menuSelection] - 1) % 27;//decrease the currently selected letter
                        if (username[menuSelection] == -1)
                        {
                            username[menuSelection] = 26;
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
                            temp.name = getLetter(username[0]) + getLetter(username[1]) + getLetter(username[2]);
                            loggedin = true;
                        }
                        if (menuSelection == 4) //verify password input
                        {

                        }
                        if (menuSelection == 3) //start password input
                        {
                            if (pad1.inputStart == 1)
                            {
                                if (tempPass.Count == 0)
                                {
                                    temp.passProtected = false;
                                }
                                else
                                {
                                    temp.passProtected = true;
                                }
                                menuSelection = 5;
                            }
                            else
                            {
                                //read input, then add to the list
                                //wait until input's released or a new one is added
                            }
                        }
                    }
                    else //verify the pass and compare it
                    {
                        throw new NotImplementedException();
                    }
                }
            }
            else
                delaytimer -= 1;
        }

        public void render(Graphics drawBuffer)
        {
            if (menuSelection == 3)
            {
                drawBuffer.DrawString("Press Start to skip password registration!", SystemFonts.DefaultFont, new SolidBrush(Color.White), 350, 350);
            }
            drawBuffer.DrawString(getLetter(username[0]), SystemFonts.DefaultFont, new SolidBrush(Color.White), 400, 300);
            drawBuffer.DrawString(getLetter(username[1]), SystemFonts.DefaultFont, new SolidBrush(Color.White), 409, 300);
            drawBuffer.DrawString(getLetter(username[2]), SystemFonts.DefaultFont, new SolidBrush(Color.White), 418, 300);
            if (menuSelection < 3)
                drawBuffer.DrawString("↑", SystemFonts.DefaultFont, new SolidBrush(Color.White), 398 + (9*menuSelection), 315);
        }

        string getLetter(int i)
        {
            return "ABCDEFGHIJKLMNOPQRSTUVWXYZ ".Substring(i, 1);
        }

        public bool writeUser()
        {
            using (StreamWriter sw = File.CreateText(temp.name + ".usr"))
            {
                sw.Write(temp.name);
                //one byte for password, the first bit if pass-protected, three bits for length (up to six) and then two bits for each digit (four possible inputs each, ABCH)
                UInt16 passData = new byte();
                if (!temp.passProtected)
                {
                    sw.Write(passData);
                }
                else
                {
                    passData += 0x8000;
                    passData += (UInt16)((tempPass.Count & 0xFf) << 12);//password length
                    for (int i = 0; i < tempPass.Count; i++)
                    {
                        passData += (UInt16)(tempPass[i] << (10 - 2 * i));
                    }
                    sw.Write(passData);
                    sw.Write(new char[4]);//global points
                    sw.Write(new char[4]);//TGM3 points
                    sw.Write(new char[2]);//Official GM certifications (1, 2, tap, tap death, 3, 3 shirase, konoha)
                }
            }
            return false;
        }
    }
}
