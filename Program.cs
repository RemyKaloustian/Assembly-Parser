using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/*
 * AUTHOR : Rémy KALOUSTIAN
 * DESCRIPTION : This file will be executed, it is the main file.
 */


namespace PEP___Assembly_Parser
{

    /*=========================================================
     * 
     * CONTRAINTES
     *  - Les registres doivent commencer par un R
     *  - Les labels doivent commmencer par 0x
     *  - Les valeurs des labels et immédiats sont directement en binaire et sur le bon nombre de bits
      * */
    class Program
    {
        static void Main(string[] args)
        {
            Parser p = new Parser(); //We create a new Parser

           p.ReadFromFile();//We read from the file specified

           Console.WriteLine("**************************************\n\n \t The result is in output.hex\n****************************************\n\n");

         Console.ReadLine();//The user must enter a value to shut the console, so he can see the result of pasring step by step
        }//Main()
    }//class Program
}//ns
