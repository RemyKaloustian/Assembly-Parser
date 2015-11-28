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
        List<String> _lines;

        public Parser()
	    {
            

	    }//Parser()


        public void ReadFromFile()
        {
            String[] lines = System.IO.File.ReadAllLines(@"D:\Documents\Desktop\TRAVAIL\POLYTECH\PEP\PROJET ARM\PEP - Assembly Parser\test.asm");

            _lines = new List<string>(lines);

            foreach (string item in lines)
            {                
                this.WriteInFile(item);
            }

        }//ReadFromFile()

        public void WriteInFile(string line)
        {
            System.IO.File.AppendAllText(@"D:\Documents\Desktop\TRAVAIL\POLYTECH\PEP\PROJET ARM\PEP - Assembly Parser\output.hex", line);

            File.AppendAllText(@"D:\Documents\Desktop\TRAVAIL\POLYTECH\PEP\PROJET ARM\PEP - Assembly Parser\output.hex", "\n");
        }//WriteInFile()


    }//class Parser
}//ns
