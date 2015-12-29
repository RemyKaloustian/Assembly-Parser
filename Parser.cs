using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

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

        }//AddHexaInstruction()

        private void EvaluateLoadStore(string item, ref StringBuilder hexaLine)
        {
            Console.WriteLine("In EvaluateDataProcessing()");
            if (item.Contains(AssemblyData.LDR))
            {
                Console.WriteLine("The line contains " + AssemblyData.LDR);
                Console.WriteLine("HexaLine before : " + hexaLine);
                hexaLine.Append(Convert.ToInt32(HexaData.LDR, 2).ToString("X"));
                Console.WriteLine("HexaLine after : " + hexaLine);
            }

            else if (item.Contains(AssemblyData.STR))
            {
                Console.WriteLine("The line contains " + AssemblyData.STR);
                Console.WriteLine("HexaLine before : " + hexaLine);
                hexaLine.Append(Convert.ToInt32(HexaData.STR, 2).ToString("X"));
                Console.WriteLine("HexaLine after : " + hexaLine);
            }


        }//EvaluateLoadStore()

        public void EvaluateDataProcessing(string item, ref StringBuilder hexaLine)
        {
            Console.WriteLine("In EvaluateDataProcessing()");
            if (item.Contains(AssemblyData.AND))
            {
                Console.WriteLine("The line contains " + AssemblyData.AND);
                Console.WriteLine("HexaLine before : " + hexaLine);
                Console.WriteLine("DP = " + Convert.ToInt32(HexaData.DP).ToString("X"));
                hexaLine.Append(Convert.ToInt32(HexaData.DP,2 ).ToString("X") + Convert.ToInt32(HexaData.AND, 2).ToString("X"));

                Console.WriteLine("HexaLine after : " + hexaLine);
            }

            else if (item.Contains(AssemblyData.ADC))
                hexaLine.Append(Convert.ToInt32(HexaData.DP, 2).ToString("X") + Convert.ToInt32(HexaData.ADC, 2).ToString("X"));

           else if(item.Contains(AssemblyData.LSL))
            {
                Console.WriteLine("Contains LSL");
                if (GetWordCounter(item) <= 3 || (GetWordCounter(item) == 4 && hasCondition(item) ))
                {
                    hexaLine.Append(Convert.ToInt32(HexaData.DP, 2).ToString("X") + Convert.ToInt32(HexaData.DPLSL, 2).ToString("X"));
                    Console.WriteLine("Is LSL OF DATA PROCESSING");
                }
            }

            else if(item.Contains(AssemblyData.ASR))
            {
                Console.WriteLine("Contains ASR");
                if (GetWordCounter(item) <= 3 || (GetWordCounter(item) == 4 && hasCondition(item) ))
                {
                    hexaLine.Append(Convert.ToInt32(HexaData.DP, 2).ToString("X") + Convert.ToInt32(HexaData.DPASR, 2).ToString("X"));
                    Console.WriteLine("Is ASR OF DATA PROCESSING");
                }
            }


            else if (item.Contains(AssemblyData.LSR))
            {
                Console.WriteLine("Contains LSR");
                if (GetWordCounter(item) <= 3 || (GetWordCounter(item) == 4 && hasCondition(item)))
                {
                    hexaLine.Append(Convert.ToInt32(HexaData.DP, 2).ToString("X") + Convert.ToInt32(HexaData.DPLSR, 2).ToString("X"));
                    Console.WriteLine("Is LSR OF DATA PROCESSING");
                }
            }

            else if (item.Contains(AssemblyData.BIC))
                hexaLine.Append(Convert.ToInt32(HexaData.DP, 2).ToString("X") + Convert.ToInt32(HexaData.BIC, 2).ToString("X"));

            else if (item.Contains(AssemblyData.CMN))
                hexaLine.Append(Convert.ToInt32(HexaData.DP, 2).ToString("X") + Convert.ToInt32(HexaData.CMN, 2).ToString("X"));

            else if (item.Contains(AssemblyData.CMP))
                hexaLine.Append(Convert.ToInt32(HexaData.DP, 2).ToString("X") + Convert.ToInt32(HexaData.CMP, 2).ToString("X"));


            else if (item.Contains(AssemblyData.MUL))
                hexaLine.Append(Convert.ToInt32(HexaData.DP, 2).ToString("X") + Convert.ToInt32(HexaData.MUL, 2).ToString("X"));

            else if (item.Contains(AssemblyData.MVN))
                hexaLine.Append(Convert.ToInt32(HexaData.DP, 2).ToString("X") + Convert.ToInt32(HexaData.MVN, 2).ToString("X"));


            else if (item.Contains(AssemblyData.ORR))
                hexaLine.Append(Convert.ToInt32(HexaData.DP, 2).ToString("X") + Convert.ToInt32(HexaData.ORR, 2).ToString("X"));

            else if (item.Contains(AssemblyData.ROR))
                hexaLine.Append(Convert.ToInt32(HexaData.DP, 2).ToString("X") + Convert.ToInt32(HexaData.ROR, 2).ToString("X"));

            else if (item.Contains(AssemblyData.RSB))
                hexaLine.Append(Convert.ToInt32(HexaData.DP, 2).ToString("X") + Convert.ToInt32(HexaData.RSB, 2).ToString("X"));

            else if (item.Contains(AssemblyData.SBC))
                hexaLine.Append(Convert.ToInt32(HexaData.DP, 2).ToString("X") + Convert.ToInt32(HexaData.SBC, 2).ToString("X"));

            else if (item.Contains(AssemblyData.TST))
                hexaLine.Append(Convert.ToInt32(HexaData.DP, 2).ToString("X") + Convert.ToInt32(HexaData.TST, 2).ToString("X"));

            else if(item.Contains(AssemblyData.EOR))
                hexaLine.Append(Convert.ToInt32(HexaData.DP, 2).ToString("X") + Convert.ToInt32(HexaData.EOR, 2).ToString("X"));
        }//EvaluateDataProcessing()

        private void EvaluateImmediateOperation(string item, ref StringBuilder hexaLine)
        {
            Console.WriteLine("In EvaluateImmediateOperation()");
            if (item.Contains(AssemblyData.MOV))
            {
                Console.WriteLine("The line contains " + AssemblyData.MOV);
                Console.WriteLine("HexaLine before : " + hexaLine);
                hexaLine.Append(Convert.ToInt32(HexaData.IM, 2).ToString("X") + Convert.ToInt32(HexaData.MOV, 2).ToString("X"));

                Console.WriteLine("HexaLine after : " + hexaLine);
            }

            else if (item.Contains(AssemblyData.LSL))
            {
                Console.WriteLine("Contains LSL");
                if ((GetWordCounter(item) ==4  && !hasCondition(item)) || GetWordCounter(item) > 4)
                {
                    hexaLine.Append(Convert.ToInt32(HexaData.IM, 2).ToString("X") + Convert.ToInt32(HexaData.ILSL, 2).ToString("X"));
                    Console.WriteLine("Is IMMEDIATE LSL");
                }
            }

            else if (item.Contains(AssemblyData.ASR))
            {
                Console.WriteLine("Contains ASR");
                if ((GetWordCounter(item) == 4 && !hasCondition(item)) || GetWordCounter(item) > 4)
                {
                    hexaLine.Append(Convert.ToInt32(HexaData.IM, 2).ToString("X") + Convert.ToInt32(HexaData.IASR, 2).ToString("X"));
                    Console.WriteLine("Is IMMEDIATE ASR");
                }
            }

            else if (item.Contains(AssemblyData.LSR))
            {
                Console.WriteLine("Contains LSR");
                if ((GetWordCounter(item) == 4 && !hasCondition(item)) || GetWordCounter(item) > 4)
                {
                    hexaLine.Append(Convert.ToInt32(HexaData.IM, 2).ToString("X") + Convert.ToInt32(HexaData.ILSR, 2).ToString("X"));
                    Console.WriteLine("Is IMMEDIATE LSR");
                }
            }


             else if (item.Contains(AssemblyData.ADD))
            {
                Console.WriteLine("The line contains " + AssemblyData.ADD);
                Console.WriteLine("HexaLine before : " + hexaLine);
                hexaLine.Append(Convert.ToInt32(HexaData.IM, 2).ToString("X") + Convert.ToInt32(HexaData.ADD, 2).ToString("X"));

                Console.WriteLine("HexaLine after : " + hexaLine);
            }

            else  if (item.Contains(AssemblyData.SUB))
            {
                Console.WriteLine("The line contains " + AssemblyData.SUB);
                Console.WriteLine("HexaLine before : " + hexaLine);
                hexaLine.Append(Convert.ToInt32(HexaData.IM, 2).ToString("X") + Convert.ToInt32(HexaData.SUB, 2).ToString("X"));

                Console.WriteLine("HexaLine after : " + hexaLine);
            }
                     


        }//EvaluateImmediateOperation()

        private void EvaluateBranch(string item, ref StringBuilder hexaLine)
        {
            Console.WriteLine("In EvaluateBranch()");
            if (item.Contains(AssemblyData.B))
            {
                Console.WriteLine("The line contains " + AssemblyData.B);
                Console.WriteLine("HexaLine before : " + hexaLine);
                hexaLine.Append(Convert.ToInt32(HexaData.B, 2).ToString("X"));

                Console.WriteLine("HexaLine after : " + hexaLine);
            }
        }//EvaluateBranch()

        private void EvaluateCondition(string item, ref StringBuilder hexaLine)
        {
            Console.WriteLine("In EvaluateCondition()");
            if (item.Contains(AssemblyData.HI))
            {
                Console.WriteLine("The line contains " + AssemblyData.HI);
                Console.WriteLine("HexaLine before : " + hexaLine);
                hexaLine.Append(Convert.ToInt32(HexaData.HI, 2).ToString("X"));
                Console.WriteLine("HexaLine after : " + hexaLine);
            }

            else if (item.Contains(AssemblyData.GT))
            {
                Console.WriteLine("The line contains " + AssemblyData.GT);
                Console.WriteLine("HexaLine before : " + hexaLine);
                hexaLine.Append(Convert.ToInt32(HexaData.GT, 2).ToString("X"));
                Console.WriteLine("HexaLine after : " + hexaLine);
            }

            else if (item.Contains(AssemblyData.NE))
            {
                Console.WriteLine("The line contains " + AssemblyData.NE);
                Console.WriteLine("HexaLine before : " + hexaLine);
                hexaLine.Append(Convert.ToInt32(HexaData.NE, 2).ToString("X"));
                Console.WriteLine("HexaLine after : " + hexaLine);
            }



        }

        private void EvaluateRegister(string item, ref StringBuilder hexaLine, string[] splitItem)
        {
            foreach (string str in splitItem)
            {
                if (str.Equals(AssemblyData.R0))
                    hexaLine.Append(Convert.ToInt32(HexaData.R0, 2).ToString("X"));

                else  if (str.Equals(AssemblyData.R1))
                    hexaLine.Append(Convert.ToInt32(HexaData.R1, 2).ToString("X"));

                else  if (str.Equals(AssemblyData.R2))
                    hexaLine.Append(Convert.ToInt32(HexaData.R2, 2).ToString("X"));

                else  if (str.Equals(AssemblyData.R3))
                    hexaLine.Append(Convert.ToInt32(HexaData.R3, 2).ToString("X"));

                else if (str.Equals(AssemblyData.R4))
                    hexaLine.Append(Convert.ToInt32(HexaData.R4, 2).ToString("X"));

                else if (str.Equals(AssemblyData.R5))
                    hexaLine.Append(Convert.ToInt32(HexaData.R5, 2).ToString("X"));

                else  if (str.Equals(AssemblyData.R6))
                    hexaLine.Append(Convert.ToInt32(HexaData.R6, 2).ToString("X"));

                else if (str.Equals(AssemblyData.R0))
                    hexaLine.Append(Convert.ToInt32(HexaData.R7, 2).ToString("X"));
            }
            /*
            Console.WriteLine("In EvaluateRegister()");
            if (item.Contains(AssemblyData.R0))
            {
                Console.WriteLine("The line contains " + AssemblyData.R0);
                Console.WriteLine("HexaLine before : " + hexaLine);
                hexaLine.Append(Convert.ToInt32(HexaData.R0, 2).ToString("X"));
                Console.WriteLine("HexaLine after : " + hexaLine);
            }

            if (item.Contains(AssemblyData.R1))
                hexaLine.Append(Convert.ToInt32(HexaData.R1, 2).ToString("X"));

            if (item.Contains(AssemblyData.R2))
                hexaLine.Append(Convert.ToInt32(HexaData.R2, 2).ToString("X"));

            if (item.Contains(AssemblyData.R3))
                hexaLine.Append(Convert.ToInt32(HexaData.R3, 2).ToString("X"));*/
        }//EvaluateRegister()


        private void EvaluateImmediateValue(string item, ref StringBuilder hexaLine)
        {

        }//EvaluateImmediateValue()

        private int GetWordCounter(string item)
        {
            char[] delimiters = new char[] { ' ', ',' };
            string str = item;
            int wordsCount = str.Split(delimiters, StringSplitOptions.RemoveEmptyEntries).Length;
            Console.WriteLine("The number of words of " + item+ " =  "+ wordsCount);
            return wordsCount;
        }

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



    }//class Parser
}//ns
