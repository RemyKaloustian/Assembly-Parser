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

        public Parser()
        {
            _hexaLines = new List<string>();

        }//Parser()


        public void ReadFromFile()
        {
            //Puts the lines in Assembly in a string array
            string[] lines = System.IO.File.ReadAllLines(@"D:\Documents\Desktop\TRAVAIL\POLYTECH\PEP\PROJET ARM\Assembly-Parser\test.asm");

            // _lines = new List<string>(lines); //Initiate a string List from the string array

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

        }//AddHexalines()

        public void AddHexaLine(string item, ref StringBuilder hexaLine)
        {
            this.AddInstruction(item, ref hexaLine);
            this.AddRegister(item, ref hexaLine);
            this.AddParameter(item, ref hexaLine);

          
        }//AddHexaLine()

        public void WriteInFile(string line)
        {
            System.IO.File.AppendAllText(@"D:\Documents\Desktop\TRAVAIL\POLYTECH\PEP\PROJET ARM\Assembly-Parser\output.hex", line);

            File.AppendAllText(@"D:\Documents\Desktop\TRAVAIL\POLYTECH\PEP\PROJET ARM\Assembly-Parser\output.hex", "\n");
        }//WriteInFile()

        public void AddInstruction(string item, ref StringBuilder hexaLine)
        {


            if (item.Contains(AssemblyData.AND))
                hexaLine.Append(HexaData.AND);

            else if (item.Contains(AssemblyData.LSL))
                hexaLine.Append(HexaData.LSL);

            else if (item.Contains(AssemblyData.ADC))
                hexaLine.Append(HexaData.ADC);

            else if (item.Contains(AssemblyData.ASR))
                hexaLine.Append(HexaData.ASR);

            else if (item.Contains(AssemblyData.BIC))
                hexaLine.Append(HexaData.BIC);

            else if (item.Contains(AssemblyData.CMN))
                hexaLine.Append(HexaData.CMN);

            else if (item.Contains(AssemblyData.CMP))
                hexaLine.Append(HexaData.CMP);

            else if (item.Contains(AssemblyData.LSR))
                hexaLine.Append(HexaData.LSR);

            else if (item.Contains(AssemblyData.MUL))
                hexaLine.Append(HexaData.MUL);

            else if (item.Contains(AssemblyData.MVN))
                hexaLine.Append(HexaData.MVN);

            else if (item.Contains(AssemblyData.OR))
                hexaLine.Append(HexaData.OR);

            else if (item.Contains(AssemblyData.ORR))
                hexaLine.Append(HexaData.ORR);

            else if (item.Contains(AssemblyData.ROR))
                hexaLine.Append(HexaData.ROR);

            else if (item.Contains(AssemblyData.RSB))
                hexaLine.Append(HexaData.RSB);

            else if (item.Contains(AssemblyData.SBC))
                hexaLine.Append(HexaData.SBC);

            else if (item.Contains(AssemblyData.TST))
                hexaLine.Append(HexaData.TST);
        }//AddHexaInstruction()

        public void AddRegister(string item, ref StringBuilder hexaLine)
        {
            if (item.Contains(AssemblyData.r0))
                hexaLine.Append(HexaData.r0);              

            if (item.Contains(AssemblyData.r1))            
                hexaLine.Append(HexaData.r1);               

            if (item.Contains(AssemblyData.r2))
                hexaLine.Append(HexaData.r2);

            if (item.Contains(AssemblyData.r3))
                hexaLine.Append(HexaData.r3);

            if (item.Contains(AssemblyData.r4))
                hexaLine.Append(HexaData.r4);

            if (item.Contains(AssemblyData.r5))
                hexaLine.Append(HexaData.r5);

            if (item.Contains(AssemblyData.r6))
                hexaLine.Append(HexaData.r6);

            if (item.Contains(AssemblyData.r7))
                hexaLine.Append(HexaData.r7);
            
        }//AddRegister()

        public void AddParameter(string item, ref StringBuilder hexaLine)
        {

        }//AddParameter()


    }//class Parser
}//ns
