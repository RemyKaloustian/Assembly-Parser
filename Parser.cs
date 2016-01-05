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

            System.IO.File.AppendAllText(@"" + this.outputpath + "\\output.hex", "v2.0 raw\n");

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
            Console.WriteLine("In WriteInFile()");
            Console.WriteLine("Binary Line added : " + line);
           // Int64 value = new Int64(Int64.Parse(line));

            string hexa = Convert.ToInt32(line, 2).ToString("X");

            Console.WriteLine("Line in HEX : " + hexa);
            System.IO.File.AppendAllText(@""+this.outputpath + "\\output.hex", hexa);

            File.AppendAllText(@"" + this.outputpath + "\\output.hex", "\n");
        }//WriteInFile()

        public void AddInstruction(string item, ref StringBuilder hexaLine)
        {
            char[] delimiters = new char[] { ' ', ',' };          
           
            string[] toEvaluate = item.Split(delimiters, StringSplitOptions.RemoveEmptyEntries);

            //Evaluate on instructions
            EvaluateDataProcessing(item, ref hexaLine, toEvaluate); 
            EvaluateImmediateOperation(item, ref hexaLine, toEvaluate); 
            EvaluateLoadStore(item, ref hexaLine, toEvaluate); 
            EvaluateBranch(item, ref hexaLine, toEvaluate);

        }//AddHexaInstruction()

        private void EvaluateLoadStore(string item, ref StringBuilder hexaLine,string[] spliItems)
        {
            Console.WriteLine("In EvaluateDataProcessing()");

            if (item.Contains(AssemblyData.LDR))
            {
                Console.WriteLine("The line contains " + AssemblyData.LDR);
                Console.WriteLine("hexaLine before : " + hexaLine);
                hexaLine.Append(HexaData.LDR);
                Console.WriteLine("hexaLine after : " + hexaLine);
                EvaluateImmediateValue(item, ref  hexaLine, spliItems);
                EvaluateRegister(item, ref  hexaLine, spliItems);
            }

            else if (item.Contains(AssemblyData.STR))
            {
                Console.WriteLine("The line contains " + AssemblyData.STR);
                Console.WriteLine("hexaLine before : " + hexaLine);
                hexaLine.Append(HexaData.STR);
                Console.WriteLine("hexaLine after : " + hexaLine);
                EvaluateRegister(item, ref  hexaLine, spliItems);
                EvaluateImmediateValue(item, ref  hexaLine, spliItems);
                
            }


        }//EvaluateLoadStore()

        public void EvaluateDataProcessing(string item, ref StringBuilder hexaLine, string[] splitItems)
        {
            Console.WriteLine("In EvaluateDataProcessing()");
            if (item.Contains(AssemblyData.AND))
            {
                Console.WriteLine("The line contains " + AssemblyData.AND);
                Console.WriteLine("hexaLine before : " + hexaLine);
                
                hexaLine.Append(HexaData.DP + HexaData.AND);

                Console.WriteLine("hexaLine after : " + hexaLine);

                EvaluateRegister(item, ref  hexaLine, splitItems);
            }

            else if (item.Contains(AssemblyData.ADC))
            {
                Console.WriteLine("The line contains " + AssemblyData.ADC);
                Console.WriteLine("hexaLine before :  " + hexaLine);
                hexaLine.Append(HexaData.DP+ HexaData.ADC);
                Console.WriteLine("hexaLine after : " + hexaLine);
                EvaluateRegister(item, ref  hexaLine, splitItems);
            }

            else if (item.Contains(AssemblyData.LSL))
            {
                Console.WriteLine("Contains LSL");
                if (GetWordCounter(item) <= 3 || (GetWordCounter(item) == 4 && hasCondition(item)))
                { 
                    Console.WriteLine("Is LSL OF DATA PROCESSING");
                    Console.WriteLine("hexaLine before : "+ hexaLine);
                    hexaLine.Append(HexaData.DP + HexaData.DPLSL);
                   
                    Console.WriteLine("hexaLine after : "+ hexaLine);
                    EvaluateRegister(item, ref  hexaLine, splitItems);
                }
            }

            else if (item.Contains(AssemblyData.ASR))
            {
                Console.WriteLine("Contains ASR");
                if (GetWordCounter(item) <= 3 || (GetWordCounter(item) == 4 && hasCondition(item)))
                {
                    Console.WriteLine("Is ASR OF DATA PROCESSING");
                    Console.WriteLine("hexaLine before : " + hexaLine);
                    hexaLine.Append(HexaData.DP+ HexaData.DPASR);
                    Console.WriteLine("hexaLine before : " + hexaLine);
                    EvaluateRegister(item, ref  hexaLine, splitItems);
                    
                }
            }


            else if (item.Contains(AssemblyData.LSR))
            {
                Console.WriteLine("Contains LSR");
                if (GetWordCounter(item) <= 3 || (GetWordCounter(item) == 4 && hasCondition(item)))
                {
                    Console.WriteLine("Is LSR OF DATA PROCESSING");
                    Console.WriteLine("hexaLine before : " + hexaLine);
                    hexaLine.Append(HexaData.DP + HexaData.DPLSR);
                    Console.WriteLine("hexaLine after : " + hexaLine);
                    EvaluateRegister(item, ref  hexaLine, splitItems);
                    
                }
            }

            else if (item.Contains(AssemblyData.BIC))
            {
                Console.WriteLine("Contains BIC");
                Console.WriteLine("hexaLine before : " + hexaLine);
                hexaLine.Append(HexaData.DP+ HexaData.BIC);
                Console.WriteLine("hexaLine after : " + hexaLine);
                EvaluateRegister(item, ref  hexaLine, splitItems);
            }

            else if (item.Contains(AssemblyData.CMN))
            {
                Console.WriteLine("Contains CMN");
                Console.WriteLine("hexaLine before : " + hexaLine);
                hexaLine.Append(HexaData.DP + HexaData.CMN);
                Console.WriteLine("hexaLine after : " + hexaLine);
                EvaluateRegister(item, ref  hexaLine, splitItems);
            }

            else if (item.Contains(AssemblyData.CMP))
            {
                Console.WriteLine("Contains CMP");
                Console.WriteLine("hexaLine before : " + hexaLine);
                hexaLine.Append(HexaData.DP+ HexaData.CMP);
                Console.WriteLine("hexaLine after : " + hexaLine);
                EvaluateRegister(item, ref  hexaLine, splitItems);
            }


            else if (item.Contains(AssemblyData.MUL))
            {
                Console.WriteLine("Contains MUL");
                Console.WriteLine("hexaLine before : " + hexaLine);
                hexaLine.Append(HexaData.DP+ HexaData.MUL);
                Console.WriteLine("hexaLine after : " + hexaLine);
                EvaluateRegister(item, ref  hexaLine, splitItems);
            }

            else if (item.Contains(AssemblyData.MVN))
            {
                Console.WriteLine("Contains MVN");
                Console.WriteLine("hexaLine before : " + hexaLine);
                hexaLine.Append(HexaData.DP + HexaData.MVN);
                Console.WriteLine("hexaLine after : " + hexaLine);
                EvaluateRegister(item, ref  hexaLine, splitItems);
            }


            else if (item.Contains(AssemblyData.ORR))
            {
                Console.WriteLine("Contains ORR");
                Console.WriteLine("hexaLine before : " + hexaLine);
                hexaLine.Append(HexaData.DP+ HexaData.ORR);
                Console.WriteLine("hexaLine after : " + hexaLine);
                EvaluateRegister(item, ref  hexaLine, splitItems);
            }

            else if (item.Contains(AssemblyData.ROR))
            {
                Console.WriteLine("Contains ROR");
                Console.WriteLine("hexaLine before : " + hexaLine);
                hexaLine.Append(HexaData.DP+ HexaData.ROR);
                Console.WriteLine("hexaLine after : " + hexaLine);
                EvaluateRegister(item, ref  hexaLine, splitItems);
            }

            else if (item.Contains(AssemblyData.RSB))
            {
                Console.WriteLine("Contains RSB");
                Console.WriteLine("hexaLine before : " + hexaLine);
                hexaLine.Append(HexaData.DP+ HexaData.RSB);
                Console.WriteLine("hexaLine after : " + hexaLine);
                EvaluateRegister(item, ref  hexaLine, splitItems);

            }

            else if (item.Contains(AssemblyData.SBC))
            {
                Console.WriteLine("Contains SBC");
                Console.WriteLine("hexaLine before : " + hexaLine);
                hexaLine.Append(HexaData.DP + HexaData.SBC);
                Console.WriteLine("hexaLine after : " + hexaLine);
                EvaluateRegister(item, ref  hexaLine, splitItems);
            }

            else if (item.Contains(AssemblyData.TST))
            {
                Console.WriteLine("Contains TST");
                Console.WriteLine("hexaLine before : " + hexaLine);
                hexaLine.Append(HexaData.DP + HexaData.TST);
                Console.WriteLine("hexaLine after : " + hexaLine);
                EvaluateRegister(item, ref  hexaLine, splitItems);
            }

            else if (item.Contains(AssemblyData.EOR))
            {
                Console.WriteLine("Contains EOR");
                Console.WriteLine("hexaLine before : " + hexaLine);
                hexaLine.Append(HexaData.DP+ HexaData.EOR);
                Console.WriteLine("hexaLine after : " + hexaLine);
                EvaluateRegister(item, ref  hexaLine, splitItems);
            }
        }//EvaluateDataProcessing()

        private void EvaluateImmediateOperation(string item, ref StringBuilder hexaLine, string[] spliItems)
        {
            Console.WriteLine("In EvaluateImmediateOperation()");

            if (item.Contains(AssemblyData.MOV))
            {
                Console.WriteLine("The line contains " + AssemblyData.MOV);
                Console.WriteLine("hexaLine before : " + hexaLine);
                hexaLine.Append(HexaData.IM + HexaData.MOV);

                Console.WriteLine("hexaLine after : " + hexaLine); 
                EvaluateRegister(item, ref  hexaLine, spliItems);
                EvaluateImmediateValue(item, ref  hexaLine, spliItems);
              
            }

            else if (item.Contains(AssemblyData.LSL))
            {
                Console.WriteLine("Contains LSL");
                if ((GetWordCounter(item) ==4  && !hasCondition(item)) || GetWordCounter(item) > 4)
                {
                    Console.WriteLine("Is IMMEDIATE LSL");
                    Console.WriteLine("hexaLine before : " + hexaLine);
                    hexaLine.Append(HexaData.IM + HexaData.ILSL);
                    Console.WriteLine("hexaLine after : " + hexaLine);
                    EvaluateImmediateValue(item, ref  hexaLine, spliItems);
                    EvaluateRegister(item, ref  hexaLine, spliItems);
                }
            }

            else if (item.Contains(AssemblyData.ASR))
            {
                Console.WriteLine("Contains ASR");
                if ((GetWordCounter(item) == 4 && !hasCondition(item)) || GetWordCounter(item) > 4)
                {
                    Console.WriteLine("Is IMMEDIATE ASR");
                    Console.WriteLine("hexaLine before : " + hexaLine);
                    hexaLine.Append(HexaData.IM + HexaData.IASR);
                    Console.WriteLine("hexaLine after : " + hexaLine);
                    EvaluateImmediateValue(item, ref  hexaLine, spliItems);
                    EvaluateRegister(item, ref  hexaLine, spliItems);
                }
            }

            else if (item.Contains(AssemblyData.LSR))
            {
                Console.WriteLine("Contains LSR");
                if ((GetWordCounter(item) == 4 && !hasCondition(item)) || GetWordCounter(item) > 4)
                {
                    Console.WriteLine("Is IMMEDIATE LSR");
                    Console.WriteLine("hexaLine before : " + hexaLine);
                    hexaLine.Append(HexaData.IM + HexaData.ILSR);
                    Console.WriteLine("hexaLine after : " + hexaLine);
                    EvaluateImmediateValue(item, ref  hexaLine, spliItems);
                    EvaluateRegister(item, ref  hexaLine, spliItems);
                }
            }
                
            else if (item.Contains(AssemblyData.ADD))
            {
                Console.WriteLine("The line contains " + AssemblyData.ADD);
                Console.WriteLine("hexaLine before : " + hexaLine);
                hexaLine.Append(HexaData.IM + HexaData.ADD);

                Console.WriteLine("hexaLine after : " + hexaLine);
                EvaluateImmediateValue(item, ref  hexaLine, spliItems);
                EvaluateRegister(item, ref  hexaLine, spliItems);
            }

            else  if (item.Contains(AssemblyData.SUB))
            {
                Console.WriteLine("The line contains " + AssemblyData.SUB);
                Console.WriteLine("hexaLine before : " + hexaLine);
                hexaLine.Append(HexaData.IM + HexaData.SUB);

                Console.WriteLine("hexaLine after : " + hexaLine);
                EvaluateImmediateValue(item, ref  hexaLine, spliItems);
                EvaluateRegister(item, ref  hexaLine, spliItems);
            }
                     


        }//EvaluateImmediateOperation()

        private void EvaluateBranch(string item, ref StringBuilder hexaLine, string[] splitItems)
        {
            Console.WriteLine("In EvaluateBranch()");
            if (item.Contains(AssemblyData.B))
            {
                Console.WriteLine("The line contains " + AssemblyData.B);
                Console.WriteLine("hexaLine before : " + hexaLine);
                hexaLine.Append(HexaData.B);

                Console.WriteLine("hexaLine after : " + hexaLine);
                EvaluateCondition(item, ref hexaLine);
                EvaluateLabel(item, ref hexaLine, splitItems);

            }
        }//EvaluateBranch()

        private void EvaluateCondition(string item, ref StringBuilder hexaLine)
        {
            Console.WriteLine("In EvaluateCondition()");
            if (item.Contains(AssemblyData.HI))
            {
                Console.WriteLine("The line contains " + AssemblyData.HI);
                Console.WriteLine("hexaLine before : " + hexaLine);
                hexaLine.Append(HexaData.HI);
                Console.WriteLine("hexaLine after : " + hexaLine);
            }

            else if (item.Contains(AssemblyData.GT))
            {
                Console.WriteLine("The line contains " + AssemblyData.GT);
                Console.WriteLine("hexaLine before : " + hexaLine);
                hexaLine.Append(HexaData.GT);
                Console.WriteLine("hexaLine after : " + hexaLine);
            }

            else if (item.Contains(AssemblyData.NE))
            {
                Console.WriteLine("The line contains " + AssemblyData.NE);
                Console.WriteLine("hexaLine before : " + hexaLine);
                hexaLine.Append(HexaData.NE);
                Console.WriteLine("hexaLine after : " + hexaLine);
            }

            else if (item.Contains(AssemblyData.EQ))
            {
                Console.WriteLine("The line contains " + AssemblyData.EQ);
                Console.WriteLine("hexaLine before : " + hexaLine);
                hexaLine.Append(HexaData.EQ);
                Console.WriteLine("hexaLine after : " + hexaLine);
            }

            else if (item.Contains(AssemblyData.CS))
            {
                Console.WriteLine("The line contains " + AssemblyData.CS);
                Console.WriteLine("hexaLine before : " + hexaLine);
                hexaLine.Append(HexaData.CS);
                Console.WriteLine("hexaLine after : " + hexaLine);
            }

            else if (item.Contains(AssemblyData.CC))
            {
                Console.WriteLine("The line contains " + AssemblyData.CC);
                Console.WriteLine("hexaLine before : " + hexaLine);
                hexaLine.Append(HexaData.CC);
                Console.WriteLine("hexaLine after : " + hexaLine);
            }

            else if (item.Contains(AssemblyData.MI))
            {
                Console.WriteLine("The line contains " + AssemblyData.MI);
                Console.WriteLine("hexaLine before : " + hexaLine);
                hexaLine.Append(HexaData.MI);
                Console.WriteLine("hexaLine after : " + hexaLine);
            }

            else if (item.Contains(AssemblyData.PL))
            {
                Console.WriteLine("The line contains " + AssemblyData.PL);
                Console.WriteLine("hexaLine before : " + hexaLine);
                hexaLine.Append(HexaData.PL);
                Console.WriteLine("hexaLine after : " + hexaLine);
            }

            else if (item.Contains(AssemblyData.VS))
            {
                Console.WriteLine("The line contains " + AssemblyData.VS);
                Console.WriteLine("hexaLine before : " + hexaLine);
                hexaLine.Append(HexaData.VS);
                Console.WriteLine("hexaLine after : " + hexaLine);
            }

            else if (item.Contains(AssemblyData.VC))
            {
                Console.WriteLine("The line contains " + AssemblyData.VC);
                Console.WriteLine("hexaLine before : " + hexaLine);
                hexaLine.Append(HexaData.VC);
                Console.WriteLine("hexaLine after : " + hexaLine);
            }

            else if (item.Contains(AssemblyData.LS))
            {
                Console.WriteLine("The line contains " + AssemblyData.LS);
                Console.WriteLine("hexaLine before : " + hexaLine);
                hexaLine.Append(HexaData.LS);
                Console.WriteLine("hexaLine after : " + hexaLine);
            }

            else if (item.Contains(AssemblyData.GE))
            {
                Console.WriteLine("The line contains " + AssemblyData.GE);
                Console.WriteLine("hexaLine before : " + hexaLine);
                hexaLine.Append(HexaData.GE);
                Console.WriteLine("hexaLine after : " + hexaLine);
            }

            else if (item.Contains(AssemblyData.LT))
            {
                Console.WriteLine("The line contains " + AssemblyData.LT);
                Console.WriteLine("hexaLine before : " + hexaLine);
                hexaLine.Append(HexaData.LT);
                Console.WriteLine("hexaLine after : " + hexaLine);
            }

            else if (item.Contains(AssemblyData.LE))
            {
                Console.WriteLine("The line contains " + AssemblyData.LE);
                Console.WriteLine("hexaLine before : " + hexaLine);
                hexaLine.Append(HexaData.LE);
                Console.WriteLine("hexaLine after : " + hexaLine);
            }

            else if (item.Contains(AssemblyData.AL))
            {
                Console.WriteLine("The line contains " + AssemblyData.AL);
                Console.WriteLine("hexaLine before : " + hexaLine);
                hexaLine.Append(HexaData.AL);
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
                   //int value = Convert.ToInt32(temp);
                    hexaLine.Append(temp);
                    Console.WriteLine("hexaLine after  : " + hexaLine);

                }
            }
        }//EvaluateImmediateValue()

        private void EvaluateImmediateValue(string item, ref StringBuilder hexaLine, string[] splitItems)
        {
            Console.WriteLine();
            foreach (string elem in splitItems)
            {
                if(elem.Contains("0") || elem.Contains("1") && !elem.Contains("0x") && !elem.Contains("r"))
                {
                    Console.WriteLine("In EvaluateImmediateValue()");
                    Console.WriteLine("hexaLine before " + hexaLine);
                    hexaLine.Append(elem);

                    Console.WriteLine("hexaLine after : " + hexaLine);
                

                //if(Regex.IsMatch(elem,@"^\d+$"))
                //{
                //    Console.WriteLine("Contains an immediate number");
                //    Console.WriteLine("hexaLine before  : " + hexaLine);

                //    int value = Convert.ToInt32(elem);
                //    hexaLine.Append(Convert.ToString(value, 2));
                //    Console.WriteLine("hexaLine after  : " + hexaLine);
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
