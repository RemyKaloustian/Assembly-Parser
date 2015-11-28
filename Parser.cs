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
        List<string> _lines;

        List<string> _hexaLines;

        public Parser()
	    {
            _hexaLines = new List<string>();

	    }//Parser()


        public void ReadFromFile()
        {
            string[] lines = System.IO.File.ReadAllLines(@"D:\Documents\Desktop\TRAVAIL\POLYTECH\PEP\PROJET ARM\Assembly-Parser\test.asm");

            _lines = new List<string>(lines);

            foreach (string item in lines)
            {                
                this.AddHexaLine(item);
            }

            foreach (string item in _hexaLines)
            {
                this.WriteInFile(item);
            }

        }//ReadFromFile()

        public void AddHexaLine(string item)
        {
            StringBuilder hexaLine = new StringBuilder("");

            AddHexaInstruction(item.ToLower(), ref hexaLine);

            _hexaLines.Add(hexaLine.ToString());

        }//AddHexalines()

        public void AddHexaInstruction(string item, ref StringBuilder  hexaLine)
        {
            if (item.Contains("and"))
                hexaLine.Append("0x0000");

            else if (item.Contains("lsl"))
                hexaLine.Append("0x0002");
        }//AddHexaLine()

        public void WriteInFile(string line)
        {
            System.IO.File.AppendAllText(@"D:\Documents\Desktop\TRAVAIL\POLYTECH\PEP\PROJET ARM\Assembly-Parser\output.hex", line);

            File.AppendAllText(@"D:\Documents\Desktop\TRAVAIL\POLYTECH\PEP\PROJET ARM\Assembly-Parser\output.hex", "\n");
        }//WriteInFile()


    }//class Parser
}//ns
