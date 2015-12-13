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
	      
        }//AddHexaInstruction()

        public void AddRegister(string item, ref StringBuilder hexaLine)
        {
            if (item.Contains(AssemblyData.r0))
            {
                hexaLine.Append(HexaData.r0);
                Console.WriteLine("Added " + HexaData.r0);
                Console.WriteLine("HexaLine : " + hexaLine);
            }

            if (item.Contains(AssemblyData.r1))
            {
                hexaLine.Append(HexaData.r1);
                Console.WriteLine("Added " + HexaData.r1);
                Console.WriteLine("HexaLine : " + hexaLine);
            }
        }//AddRegister()

        public void AddParameter(string item, ref StringBuilder hexaLine)
        {

        }//AddParameter()


    }//class Parser
}//ns
