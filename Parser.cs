using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Text.RegularExpressions;

namespace PEP___Assembly_Parser
{
    class Parser
    {
        List<string> _lines; //The lines from the assembly file

        List<string> _hexaLines; //The instructions in hexa that we'll put in the file

        string outputpath;

        public Parser()
        {
            _hexaLines = new List<string>();

        }//Parser()


        public void ReadFromFile()
        {
            Console.WriteLine("Enter the ABSOLUTE path of the Assembly file you want to parse : \n");
            string path = Console.ReadLine();

            Console.WriteLine("\nEnter the ABSOLUTE path of the output directory :\n");
            this.outputpath = Console.ReadLine();
            //Puts the lines in Assembly in a string array
            string[] lines = System.IO.File.ReadAllLines(@""+path);



            foreach (string item in lines) //We add the hexadecimal equivalent of the Assembly line in _hexaLines
            {
                this.AddHexaLine(item);
            }

            foreach (string item in this._hexaLines)//We write the hexadecimal lines from _hexaLines in the output file
            {
                this.WriteInFile(item);
            }

        }//ReadFromFile()

        public void AddHexaLine(string item)
        {
            StringBuilder hexaLine = new StringBuilder("");

            AddHexaLine(item.ToLower(), ref hexaLine); //We turn the Assembly line in hexa

            _hexaLines.Add(hexaLine.ToString()); //We add the hexa line to all the lines

            Console.WriteLine("Final line : " + hexaLine);
            Console.WriteLine("\n---------------__________LINE ADDED_______--------------------\n");

        }//AddHexalines()

        public void AddHexaLine(string item, ref StringBuilder hexaLine)
        {
            this.AddInstruction(item, ref hexaLine);


        }//AddHexaLine()

        public void WriteInFile(string line)
        {
            System.IO.File.AppendAllText(@""+this.outputpath + "\\output.hex", line);

            File.AppendAllText(@"" + this.outputpath + "\\output.hex", "\n");
        }//WriteInFile()

        public void AddInstruction(string item, ref StringBuilder hexaLine)
        {
            char[] delimiters = new char[] { ' ', ',' };          
           
            string[] toEvaluate = item.Split(delimiters, StringSplitOptions.RemoveEmptyEntries);

            EvaluateDataProcessing(item, ref hexaLine); //A compléter en fonction de comment on gère l'hexa
            EvaluateImmediateOperation(item, ref hexaLine); //A compléter en fonction de comment on gère l'hexa
            EvaluateLoadStore(item, ref hexaLine); //A compléter (rajouter store)
            EvaluateBranch(item, ref hexaLine);
            EvaluateCondition(item, ref hexaLine);
            EvaluateRegister(item, ref hexaLine, toEvaluate);
            EvaluateLabel(item, ref hexaLine, toEvaluate);
            EvaluateImmediateValue(item, ref hexaLine, toEvaluate);

        }//AddHexaInstruction()

        private void EvaluateLoadStore(string item, ref StringBuilder hexaLine)
        {
            Console.WriteLine("In EvaluateDataProcessing()");

            if (item.Contains(AssemblyData.LDR))
            {
                Console.WriteLine("The line contains " + AssemblyData.LDR);
                Console.WriteLine("hexaLine before : " + hexaLine);
                hexaLine.Append(Convert.ToInt32(HexaData.LDR, 2).ToString("X"));
                Console.WriteLine("hexaLine after : " + hexaLine);
            }

            else if (item.Contains(AssemblyData.STR))
            {
                Console.WriteLine("The line contains " + AssemblyData.STR);
                Console.WriteLine("hexaLine before : " + hexaLine);
                hexaLine.Append(Convert.ToInt32(HexaData.STR, 2).ToString("X"));
                Console.WriteLine("hexaLine after : " + hexaLine);
            }


        }//EvaluateLoadStore()

        public void EvaluateDataProcessing(string item, ref StringBuilder hexaLine)
        {
            Console.WriteLine("In EvaluateDataProcessing()");
            if (item.Contains(AssemblyData.AND))
            {
                Console.WriteLine("The line contains " + AssemblyData.AND);
                Console.WriteLine("hexaLine before : " + hexaLine);
                Console.WriteLine("DP = " + Convert.ToInt32(HexaData.DP).ToString("X"));
                hexaLine.Append(HexaData.DP + HexaData.AND);

                Console.WriteLine("hexaLine after : " + hexaLine);
            }

            else if (item.Contains(AssemblyData.ADC))
            {
                Console.WriteLine("The line contains " + AssemblyData.ADC);
                Console.WriteLine("hexaLine before :  " + hexaLine);
                hexaLine.Append(Convert.ToInt32(HexaData.DP, 2).ToString("X") + Convert.ToInt32(HexaData.ADC, 2).ToString("X"));
                Console.WriteLine("hexaLine after : " + hexaLine);
            }

            else if (item.Contains(AssemblyData.LSL))
            {
                Console.WriteLine("Contains LSL");
                if (GetWordCounter(item) <= 3 || (GetWordCounter(item) == 4 && hasCondition(item)))
                { 
                    Console.WriteLine("Is LSL OF DATA PROCESSING");
                    Console.WriteLine("hexaLine before : "+ hexaLine);
                    hexaLine.Append(Convert.ToInt32(HexaData.DP, 2).ToString("X") + Convert.ToInt32(HexaData.DPLSL, 2).ToString("X"));
                   
                    Console.WriteLine("hexaLine after : "+ hexaLine);
                }
            }

            else if (item.Contains(AssemblyData.ASR))
            {
                Console.WriteLine("Contains ASR");
                if (GetWordCounter(item) <= 3 || (GetWordCounter(item) == 4 && hasCondition(item)))
                {
                    Console.WriteLine("Is ASR OF DATA PROCESSING");
                    Console.WriteLine("hexaLine before : " + hexaLine);
                    hexaLine.Append(Convert.ToInt32(HexaData.DP, 2).ToString("X") + Convert.ToInt32(HexaData.DPASR, 2).ToString("X"));
                    Console.WriteLine("hexaLine before : " + hexaLine);
                    
                }
            }


            else if (item.Contains(AssemblyData.LSR))
            {
                Console.WriteLine("Contains LSR");
                if (GetWordCounter(item) <= 3 || (GetWordCounter(item) == 4 && hasCondition(item)))
                {
                    Console.WriteLine("Is LSR OF DATA PROCESSING");
                    Console.WriteLine("hexaLine before : " + hexaLine);
                    hexaLine.Append(Convert.ToInt32(HexaData.DP, 2).ToString("X") + Convert.ToInt32(HexaData.DPLSR, 2).ToString("X"));
                    Console.WriteLine("hexaLine after : " + hexaLine);
                    
                }
            }

            else if (item.Contains(AssemblyData.BIC))
            {
                Console.WriteLine("Contains BIC");
                Console.WriteLine("hexaLine before : " + hexaLine);
                hexaLine.Append(Convert.ToInt32(HexaData.DP, 2).ToString("X") + Convert.ToInt32(HexaData.BIC, 2).ToString("X"));
                Console.WriteLine("hexaLine after : " + hexaLine);
            }

            else if (item.Contains(AssemblyData.CMN))
            {
                Console.WriteLine("Contains CMN");
                Console.WriteLine("hexaLine before : " + hexaLine);
                hexaLine.Append(Convert.ToInt32(HexaData.DP, 2).ToString("X") + Convert.ToInt32(HexaData.CMN, 2).ToString("X"));
                Console.WriteLine("hexaLine after : " + hexaLine);
            }

            else if (item.Contains(AssemblyData.CMP))
            {
                Console.WriteLine("Contains CMP");
                Console.WriteLine("hexaLine before : " + hexaLine);
                hexaLine.Append(Convert.ToInt32(HexaData.DP, 2).ToString("X") + Convert.ToInt32(HexaData.CMP, 2).ToString("X"));
                Console.WriteLine("hexaLine after : " + hexaLine);
            }


            else if (item.Contains(AssemblyData.MUL))
            {
                Console.WriteLine("Contains MUL");
                Console.WriteLine("hexaLine before : " + hexaLine);
                hexaLine.Append(Convert.ToInt32(HexaData.DP, 2).ToString("X") + Convert.ToInt32(HexaData.MUL, 2).ToString("X"));
                Console.WriteLine("hexaLine after : " + hexaLine);
            }

            else if (item.Contains(AssemblyData.MVN))
            {
                Console.WriteLine("Contains MVN");
                Console.WriteLine("hexaLine before : " + hexaLine);
                hexaLine.Append(Convert.ToInt32(HexaData.DP, 2).ToString("X") + Convert.ToInt32(HexaData.MVN, 2).ToString("X"));
                Console.WriteLine("hexaLine after : " + hexaLine);
            }


            else if (item.Contains(AssemblyData.ORR))
            {
                Console.WriteLine("Contains ORR");
                Console.WriteLine("hexaLine before : " + hexaLine);
                hexaLine.Append(Convert.ToInt32(HexaData.DP, 2).ToString("X") + Convert.ToInt32(HexaData.ORR, 2).ToString("X"));
                Console.WriteLine("hexaLine after : " + hexaLine);
            }

            else if (item.Contains(AssemblyData.ROR))
            {
                Console.WriteLine("Contains ROR");
                Console.WriteLine("hexaLine before : " + hexaLine);
                hexaLine.Append(Convert.ToInt32(HexaData.DP, 2).ToString("X") + Convert.ToInt32(HexaData.ROR, 2).ToString("X"));
                Console.WriteLine("hexaLine after : " + hexaLine);
            }

            else if (item.Contains(AssemblyData.RSB))
            {
                Console.WriteLine("Contains RSB");
                Console.WriteLine("hexaLine before : " + hexaLine);
                hexaLine.Append(Convert.ToInt32(HexaData.DP, 2).ToString("X") + Convert.ToInt32(HexaData.RSB, 2).ToString("X"));
                Console.WriteLine("hexaLine after : " + hexaLine);

            }

            else if (item.Contains(AssemblyData.SBC))
            {
                Console.WriteLine("Contains SBC");
                Console.WriteLine("hexaLine before : " + hexaLine);
                hexaLine.Append(Convert.ToInt32(HexaData.DP, 2).ToString("X") + Convert.ToInt32(HexaData.SBC, 2).ToString("X"));
                Console.WriteLine("hexaLine after : " + hexaLine);
            }

            else if (item.Contains(AssemblyData.TST))
            {
                Console.WriteLine("Contains TST");
                Console.WriteLine("hexaLine before : " + hexaLine);
                hexaLine.Append(Convert.ToInt32(HexaData.DP, 2).ToString("X") + Convert.ToInt32(HexaData.TST, 2).ToString("X"));
                Console.WriteLine("hexaLine after : " + hexaLine);
            }

            else if (item.Contains(AssemblyData.EOR))
            {
                Console.WriteLine("Contains EOR");
                Console.WriteLine("hexaLine before : " + hexaLine);
                hexaLine.Append(Convert.ToInt32(HexaData.DP, 2).ToString("X") + Convert.ToInt32(HexaData.EOR, 2).ToString("X"));
                Console.WriteLine("hexaLine after : " + hexaLine);
            }
        }//EvaluateDataProcessing()

        private void EvaluateImmediateOperation(string item, ref StringBuilder hexaLine)
        {
            Console.WriteLine("In EvaluateImmediateOperation()");

            if (item.Contains(AssemblyData.MOV))
            {
                Console.WriteLine("The line contains " + AssemblyData.MOV);
                Console.WriteLine("hexaLine before : " + hexaLine);
                hexaLine.Append(Convert.ToInt32(HexaData.IM, 2).ToString("X") + Convert.ToInt32(HexaData.MOV, 2).ToString("X"));

                Console.WriteLine("hexaLine after : " + hexaLine);
            }

            else if (item.Contains(AssemblyData.LSL))
            {
                Console.WriteLine("Contains LSL");
                if ((GetWordCounter(item) ==4  && !hasCondition(item)) || GetWordCounter(item) > 4)
                {
                    Console.WriteLine("Is IMMEDIATE LSL");
                    Console.WriteLine("hexaLine before : " + hexaLine);
                    hexaLine.Append(Convert.ToInt32(HexaData.IM, 2).ToString("X") + Convert.ToInt32(HexaData.ILSL, 2).ToString("X"));
                    Console.WriteLine("hexaLine after : " + hexaLine);
                    
                }
            }

            else if (item.Contains(AssemblyData.ASR))
            {
                Console.WriteLine("Contains ASR");
                if ((GetWordCounter(item) == 4 && !hasCondition(item)) || GetWordCounter(item) > 4)
                {
                    Console.WriteLine("Is IMMEDIATE ASR");
                    Console.WriteLine("hexaLine before : " + hexaLine);
                    hexaLine.Append(Convert.ToInt32(HexaData.IM, 2).ToString("X") + Convert.ToInt32(HexaData.IASR, 2).ToString("X"));
                    Console.WriteLine("hexaLine after : " + hexaLine);
                    
                }
            }

            else if (item.Contains(AssemblyData.LSR))
            {
                Console.WriteLine("Contains LSR");
                if ((GetWordCounter(item) == 4 && !hasCondition(item)) || GetWordCounter(item) > 4)
                {
                    Console.WriteLine("Is IMMEDIATE LSR");
                    Console.WriteLine("hexaLine before : " + hexaLine);
                    hexaLine.Append(Convert.ToInt32(HexaData.IM, 2).ToString("X") + Convert.ToInt32(HexaData.ILSR, 2).ToString("X"));
                    Console.WriteLine("hexaLine after : " + hexaLine);
                    
                }
            }
                
            else if (item.Contains(AssemblyData.ADD))
            {
                Console.WriteLine("The line contains " + AssemblyData.ADD);
                Console.WriteLine("hexaLine before : " + hexaLine);
                hexaLine.Append(Convert.ToInt32(HexaData.IM, 2).ToString("X") + Convert.ToInt32(HexaData.ADD, 2).ToString("X"));

                Console.WriteLine("hexaLine after : " + hexaLine);
            }

            else  if (item.Contains(AssemblyData.SUB))
            {
                Console.WriteLine("The line contains " + AssemblyData.SUB);
                Console.WriteLine("hexaLine before : " + hexaLine);
                hexaLine.Append(Convert.ToInt32(HexaData.IM, 2).ToString("X") + Convert.ToInt32(HexaData.SUB, 2).ToString("X"));

                Console.WriteLine("hexaLine after : " + hexaLine);
            }
                     


        }//EvaluateImmediateOperation()

        private void EvaluateBranch(string item, ref StringBuilder hexaLine)
        {
            Console.WriteLine("In EvaluateBranch()");
            if (item.Contains(AssemblyData.B))
            {
                Console.WriteLine("The line contains " + AssemblyData.B);
                Console.WriteLine("hexaLine before : " + hexaLine);
                hexaLine.Append(Convert.ToInt32(HexaData.B, 2).ToString("X"));

                Console.WriteLine("hexaLine after : " + hexaLine);
            }
        }//EvaluateBranch()

        private void EvaluateCondition(string item, ref StringBuilder hexaLine)
        {
            Console.WriteLine("In EvaluateCondition()");
            if (item.Contains(AssemblyData.HI))
            {
                Console.WriteLine("The line contains " + AssemblyData.HI);
                Console.WriteLine("hexaLine before : " + hexaLine);
                hexaLine.Append(Convert.ToInt32(HexaData.HI, 2).ToString("X"));
                Console.WriteLine("hexaLine after : " + hexaLine);
            }

            else if (item.Contains(AssemblyData.GT))
            {
                Console.WriteLine("The line contains " + AssemblyData.GT);
                Console.WriteLine("hexaLine before : " + hexaLine);
                hexaLine.Append(Convert.ToInt32(HexaData.GT, 2).ToString("X"));
                Console.WriteLine("hexaLine after : " + hexaLine);
            }

            else if (item.Contains(AssemblyData.NE))
            {
                Console.WriteLine("The line contains " + AssemblyData.NE);
                Console.WriteLine("hexaLine before : " + hexaLine);
                hexaLine.Append(Convert.ToInt32(HexaData.NE, 2).ToString("X"));
                Console.WriteLine("hexaLine after : " + hexaLine);
            }



        }

        private void EvaluateRegister(string item, ref StringBuilder hexaLine, string[] splitItem)
        {
            Console.WriteLine("In EvaluateRegister()");
            List<string> registerTab = new List<string>();

            foreach (string elem in splitItem)
            {
                elem.Trim();
                Console.WriteLine("elem = " + elem);
                if (elem.Length <= 2 && elem.Contains("r"))
                {
                    Console.WriteLine("Adding " + elem + "to the registerTab");
                    registerTab.Add(elem);
                }
            }

            for (int i = registerTab.Count() - 1; i >= 0; --i )
            {
                Console.WriteLine("i = " + i);
                Console.WriteLine("hexaLine before : " + hexaLine);
                string binary = RegisterToBinary(registerTab[i]);

                hexaLine.Append(binary);

                Console.WriteLine("hexaLine after : " + hexaLine);
            }

           
           
        }//EvaluateRegister()


        private void EvaluateLabel(string item, ref StringBuilder hexaLine, string[] splitItems)
        {
            Console.WriteLine("In EvaluateLabel()");
            foreach (string elem in splitItems)
            {
                if(elem.Contains("0x"))
                {
                    Console.WriteLine("Contains 0x");
                    Console.WriteLine("hexaLine before  : " + hexaLine);
                    string temp = elem.Replace("0x", "");
                    temp.Trim();
                    int value = Convert.ToInt32(temp);
                    hexaLine.Append(Convert.ToString(value, 2));
                    Console.WriteLine("hexaLine after  : " + hexaLine);

                }
            }
        }//EvaluateImmediateValue()

        private void EvaluateImmediateValue(string item, ref StringBuilder hexaLine, string[] splitItems)
        {
            foreach (string elem in splitItems)
            {
                if(Regex.IsMatch(elem,@"^\d+$"))
                {
                    Console.WriteLine("Contains an immediate number");
                    Console.WriteLine("hexaLine before  : " + hexaLine);

                    int value = Convert.ToInt32(elem);
                    hexaLine.Append(Convert.ToString(value, 2));
                    Console.WriteLine("hexaLine after  : " + hexaLine);
                }
            }
        }//EvaluateImmediateValue()

        

        private int GetWordCounter(string item)
        {
            char[] delimiters = new char[] { ' ', ',' };
            string str = item;
            int wordsCount = str.Split(delimiters, StringSplitOptions.RemoveEmptyEntries).Length;
            Console.WriteLine("The number of words of " + item+ " =  "+ wordsCount);
            return wordsCount;
        }//GetWordCounter()

        private bool hasCondition(string item)
        {

            if(item.Contains(AssemblyData.EQ) || item.Contains(AssemblyData.NE) || item.Contains(AssemblyData.CS) 
                || item.Contains(AssemblyData.CC)  || item.Contains(AssemblyData.MI) || item.Contains(AssemblyData.PL) ||
                item.Contains(AssemblyData.VS)  || item.Contains(AssemblyData.VC)  || item.Contains(AssemblyData.HI)  || item.Contains(AssemblyData.GE) || item.Contains(AssemblyData.LT) || 
                item.Contains(AssemblyData.GT) || item.Contains(AssemblyData.LE) || item.Contains(AssemblyData.AL))
            {
                Console.WriteLine("In hasCondition(), Contains a condition ! ");
                return true;

            }

            if(item.Contains(AssemblyData.LS))
            {
                if (CountStringOccurrences(item, AssemblyData.LS) > 1)
                    return true;
            }

            return false;
        }//hasCondition()

        private int CountStringOccurrences(string text, string pattern)
        {
            // Loop through all instances of the string 'text'.
            int count = 0;
            int i = 0;
            while ((i = text.IndexOf(pattern, i)) != -1)
            {
                i += pattern.Length;
                count++;
            }
            return count;
        }//CountStringOccurences()    
        
        private string RegisterToBinary(string str)
        {
            string toReturn = "";
            if (str.Equals(AssemblyData.R0))
            {
                Console.WriteLine("Contains R0");
                toReturn = HexaData.R0;
            }

            else if (str.Equals(AssemblyData.R1))
            {
                Console.WriteLine("Contains R1");
                toReturn = HexaData.R1;
            }

            else if (str.Equals(AssemblyData.R2))
            {
                Console.WriteLine("Contains R2");
                toReturn = HexaData.R2;
            }

            else if (str.Equals(AssemblyData.R3))
            {
                Console.WriteLine("Contains R3");
                toReturn = HexaData.R3;
            }

            else if (str.Equals(AssemblyData.R4))
            {
                Console.WriteLine("Contains R4");
                toReturn = HexaData.R4;
            }


            else if (str.Equals(AssemblyData.R5))
            {
                Console.WriteLine("Contains R5");
                toReturn = HexaData.R5;
            }

            else if (str.Equals(AssemblyData.R6))
            {
                Console.WriteLine("Contains R6");
                toReturn = HexaData.R6;
            }

            else if (str.Equals(AssemblyData.R7))
            {
                Console.WriteLine("Contains R7");
                toReturn = HexaData.R7;
            }

            return toReturn;
        }//RegisterToBinary()

    }//class Parser
}//ns
