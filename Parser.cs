using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Text.RegularExpressions;

/*
 * AUTHOR : Rémy KALOUSTIAN
 * DESCRIPTION : This file contains all the operations for the parsing processus.
 * It reads from an assembly file and convert it into an hexadecimal file
 */

namespace PEP___Assembly_Parser
{
    class Parser
    {
       // List<string> _lines; //The lines from the assembly file

        List<string> _hexaLines; //The instructions in hexa that we'll put in the file

        string outputpath; //The name of the output file

        public Parser() //Constructor
        {
            _hexaLines = new List<string>(); //We create the list of lines

        }//Parser()


        public void ReadFromFile()
        {
            //NB : Console.Writeline() writes on the console
            //The user enters the path of the Assembly file
            Console.WriteLine("Enter the ABSOLUTE path of the Assembly file you want to parse : \n");
            string path = Console.ReadLine();

            //The user enters the path of the hexa file
            Console.WriteLine("\nEnter the ABSOLUTE path of the output directory :\n");
            this.outputpath = Console.ReadLine();

            //Puts the lines in Assembly in a string array
            string[] lines = System.IO.File.ReadAllLines(@""+path);


            foreach (string item in lines) //We add the hexadecimal equivalent of the Assembly line in _hexaLines
            {
                this.AddHexaLine(item);
            }

            //This line is required to make the file usable by logisim
            System.IO.File.AppendAllText(@"" + this.outputpath + "\\output.hex", "v2.0 raw\n");

            foreach (string item in this._hexaLines)//We write the hexadecimal lines from _hexaLines in the output file
            {
                this.WriteInFile(item);
            }

        }//ReadFromFile()

        public void AddHexaLine(string item) //We add the hexa lines to the List _hexaLines
        {
            StringBuilder hexaLine = new StringBuilder(""); //We use a StringBuilder because it is easier to manipulate

            AddHexaLine(item.ToLower(), ref hexaLine); //We turn the Assembly line in hexa

            _hexaLines.Add(hexaLine.ToString()); //We add the hexa line to all the lines

            Console.WriteLine("Final line : " + hexaLine);
            Console.WriteLine("\n---------------__________LINE ADDED_______--------------------\n");

        }//AddHexalines()

        public void AddHexaLine(string item, ref StringBuilder hexaLine) //We call AddInstruction()
        {
            this.AddInstruction(item, ref hexaLine);

        }//AddHexaLine()

        public void AddInstruction(string item, ref StringBuilder hexaLine) //This is where the parsing happens
        {
            char[] delimiters = new char[] { ' ', ',' }; //We choose the characters that will  signify the end of a word

            string[] toEvaluate = item.Split(delimiters, StringSplitOptions.RemoveEmptyEntries); //We split the line in a table of different words

            //Evaluate on instructions
            EvaluateDataProcessing(item, ref hexaLine, toEvaluate);
            EvaluateImmediateOperation(item, ref hexaLine, toEvaluate);
            EvaluateLoadStore(item, ref hexaLine, toEvaluate);
            EvaluateBranch(item, ref hexaLine, toEvaluate);

        }//AddHexaInstruction()

               
        private void EvaluateLoadStore(string item, ref StringBuilder hexaLine,string[] spliItems) //We eveluate either load or store
        {
            Console.WriteLine("In EvaluateDataProcessing()");

            if (item.Contains(AssemblyData.LDR)) //If Load 
            {
                Console.WriteLine("The line contains " + AssemblyData.LDR);
                Console.WriteLine("hexaLine before : " + hexaLine);
                hexaLine.Append(HexaData.LDR); //We add the binary equivalent to the final line
                Console.WriteLine("hexaLine after : " + hexaLine);
                EvaluateImmediateValue(item, ref  hexaLine, spliItems); //We first evaluate the immediate value
                EvaluateRegister(item, ref  hexaLine, spliItems);//We evaluate the register(s) value
            }

            else if (item.Contains(AssemblyData.STR))//If Store
            {
                Console.WriteLine("The line contains " + AssemblyData.STR);
                Console.WriteLine("hexaLine before : " + hexaLine);
                hexaLine.Append(HexaData.STR);//We add the binary equivalent to the final line
                Console.WriteLine("hexaLine after : " + hexaLine);
                EvaluateRegister(item, ref  hexaLine, spliItems);//We first evaluate the register(s) value
                EvaluateImmediateValue(item, ref  hexaLine, spliItems); //We evaluate the immediate value
                
            }


        }//EvaluateLoadStore()

        //We evaluate the data processing instruction
        public void EvaluateDataProcessing(string item, ref StringBuilder hexaLine, string[] splitItems)
        {
            Console.WriteLine("In EvaluateDataProcessing()");
            if (item.Contains(AssemblyData.AND))//If the instruction is AND
            {
                Console.WriteLine("The line contains " + AssemblyData.AND);
                Console.WriteLine("hexaLine before : " + hexaLine);

                hexaLine.Append(HexaData.DP + HexaData.AND); //We add the binary equivalent to the final line 

                Console.WriteLine("hexaLine after : " + hexaLine);

                EvaluateRegister(item, ref  hexaLine, splitItems);//We evaluate the register
            }

                //All those cases follow the same principle
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
                //We determine if the LSL is a data-processing LSL ( 3 words or 4 words with a condition),  same for ASR and LSR
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

        //We evaluate the immediate instruction
        private void EvaluateImmediateOperation(string item, ref StringBuilder hexaLine, string[] spliItems)
        {
            Console.WriteLine("In EvaluateImmediateOperation()");

            if (item.Contains(AssemblyData.MOV))//If the instruction is MOV
            {
                Console.WriteLine("The line contains " + AssemblyData.MOV);
                Console.WriteLine("hexaLine before : " + hexaLine);
                hexaLine.Append(HexaData.IM + HexaData.MOV); //We add the binary equivalent to the final line

                Console.WriteLine("hexaLine after : " + hexaLine); 
                EvaluateRegister(item, ref  hexaLine, spliItems); //We evaluate the register(s) first
                EvaluateImmediateValue(item, ref  hexaLine, spliItems);//We evaluate the value of the immediate
              
            }

            else if (item.Contains(AssemblyData.LSL))
            {
                Console.WriteLine("Contains LSL");
                //We determine if the LSL is a immediate LSL ( 5 words or 4 words without a condition),  same for ASR and LSR

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

        //We evaluate the conditional branch
        private void EvaluateBranch(string item, ref StringBuilder hexaLine, string[] splitItems)
        {
            Console.WriteLine("In EvaluateBranch()");
            if (item.Contains(AssemblyData.B))
            {
                Console.WriteLine("The line contains " + AssemblyData.B);
                Console.WriteLine("hexaLine before : " + hexaLine);
                hexaLine.Append(HexaData.B);//We had the binary equivalent to the final line

                Console.WriteLine("hexaLine after : " + hexaLine);
                EvaluateCondition(item, ref hexaLine);//We evaluate the condition
                EvaluateLabel(item, ref hexaLine, splitItems); //We evaluate the label

            }
        }//EvaluateBranch()

        private void EvaluateCondition(string item, ref StringBuilder hexaLine)
        {
            Console.WriteLine("In EvaluateCondition()");
            if (item.Contains(AssemblyData.HI))//If the condition is HI
            {
                Console.WriteLine("The line contains " + AssemblyData.HI);
                Console.WriteLine("hexaLine before : " + hexaLine);
                hexaLine.Append(HexaData.HI);//We had the binary equivalent to the final line
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

        }//EvaluateCondition()

        private void EvaluateRegister(string item, ref StringBuilder hexaLine, string[] splitItem)
        {
            Console.WriteLine("In EvaluateRegister()");
            List<string> registerTab = new List<string>(); //Will contain all the registers of the Assembly line

            foreach (string elem in splitItem) //For each words in the line
            {
                elem.Trim(); //We suppress the spaces
                Console.WriteLine("elem = " + elem);
                if (elem.Length <= 2 && elem.Contains("r")) //If the word is a register( length < 2 and contains a 'r')
                {
                    Console.WriteLine("Adding " + elem + "to the registerTab");
                    registerTab.Add(elem); //We had it to the list of the registers
                }
            }

            for (int i = registerTab.Count() - 1; i >= 0; --i ) //We go through the list, backwards because in binary, the regiters are reversed
            {
                Console.WriteLine("i = " + i);
                Console.WriteLine("hexaLine before : " + hexaLine);
                string binary = RegisterToBinary(registerTab[i]); //We convert the register in binary

                hexaLine.Append(binary);//We had the binary equivalent to the final line

                Console.WriteLine("hexaLine after : " + hexaLine);
            }
           
        }//EvaluateRegister()


        private void EvaluateLabel(string item, ref StringBuilder hexaLine, string[] splitItems)
        {
            Console.WriteLine("In EvaluateLabel()");
            foreach (string elem in splitItems) //For each words of the assembly line
            {
                if(elem.Contains("0x")) //The label value has to begin with 0x
                {
                    Console.WriteLine("Contains 0x");
                    Console.WriteLine("hexaLine before  : " + hexaLine);
                    string temp = elem.Replace("0x", ""); //We suppress the 0x
                    temp.Trim();//We suppress the spaces

                    hexaLine.Append(temp);//We had the binary equivalent to the final line
                    Console.WriteLine("hexaLine after  : " + hexaLine);

                }
            }
        }//EvaluateLabel()

        private void EvaluateImmediateValue(string item, ref StringBuilder hexaLine, string[] splitItems)
        {
            Console.WriteLine();
            foreach (string elem in splitItems)//For each word in the Assembly line
            {
                //We check if the word is an immediate value (contains 0 or 1 and not 0x and not r)
                if(elem.Contains("0") || elem.Contains("1") && !elem.Contains("0x") && !elem.Contains("r"))
                {
                    Console.WriteLine("In EvaluateImmediateValue()");
                    Console.WriteLine("hexaLine before " + hexaLine);
                    hexaLine.Append(elem);//We had the binary equivalent to the final line

                    Console.WriteLine("hexaLine after : " + hexaLine);                
                }
            }
        }//EvaluateImmediateValue()

        //We write a line in the output file
        public void WriteInFile(string line)
        {
            Console.WriteLine("In WriteInFile()");
            Console.WriteLine("Binary Line added : " + line);
            
            string hexa = Convert.ToInt32(line, 2).ToString("X");//We convert the binary line to hexadecimal

            Console.WriteLine("Line in HEX : " + hexa);
            //We put the line in the file
            System.IO.File.AppendAllText(@"" + this.outputpath + "\\output.hex", hexa);

            //We put '\n' to go to the next line
            File.AppendAllText(@"" + this.outputpath + "\\output.hex", "\n");
        }//WriteInFile()

        private int GetWordCounter(string item)//Counts the number of words in the line
        {
            char[] delimiters = new char[] { ' ', ',' };
            string str = item;
            int wordsCount = str.Split(delimiters, StringSplitOptions.RemoveEmptyEntries).Length; //We keep the length of the table resulting
            Console.WriteLine("The number of words of " + item+ " =  "+ wordsCount);
            return wordsCount;
        }//GetWordCounter()

        //We determine if the lines contains a condition
        private bool hasCondition(string item)
        {

            //Does the line contains EQ ? NO --> Does the line contains NE ? NO -->Does the line contains CS ? NO -->you got the idea
            if(item.Contains(AssemblyData.EQ) || item.Contains(AssemblyData.NE) || item.Contains(AssemblyData.CS) 
                || item.Contains(AssemblyData.CC)  || item.Contains(AssemblyData.MI) || item.Contains(AssemblyData.PL) ||
                item.Contains(AssemblyData.VS)  || item.Contains(AssemblyData.VC)  || item.Contains(AssemblyData.HI)  || item.Contains(AssemblyData.GE) || item.Contains(AssemblyData.LT) || 
                item.Contains(AssemblyData.GT) || item.Contains(AssemblyData.LE) || item.Contains(AssemblyData.AL))
            {
                Console.WriteLine("In hasCondition(), Contains a condition ! ");
                return true; //The lines contains a condition

            }

            if(item.Contains(AssemblyData.LS))
            {
                //Since you can have LSL/LSR and a LS condition in the same line
                if (CountStringOccurrences(item, AssemblyData.LS) > 1)//If there is more than one word containing LS
                    return true; //The line contains a condition
            }

            return false;
        }//hasCondition()

        //Count the occurrences of a string in a text
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
        
        //Convert the register to its binary equivalent
        private string RegisterToBinary(string str)
        {
            string toReturn = "";
            if (str.Equals(AssemblyData.R0))//If the register is R0
            {
                Console.WriteLine("Contains R0");
                toReturn = HexaData.R0; //We return the binary value or R0
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
