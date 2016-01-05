using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PEP___Assembly_Parser
{
    class Program
    {
        static void Main(string[] args)
        {
            Parser p = new Parser();

           p.ReadFromFile();

           Console.WriteLine("**************************************\n\n \t The result is in output.hex\n****************************************\n\n");

           string str = "1111";
           string strhex = Convert.ToInt32(str, 2).ToString("X");
           Console.ReadLine();
        }
    }
}
