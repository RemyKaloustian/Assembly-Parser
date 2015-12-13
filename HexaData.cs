using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PEP___Assembly_Parser
{
    class HexaData
    {
        //Hexa equivalent for instructions
        public static readonly string AND = "0";
        public static readonly string OR = "1";
        public static readonly string LSL = "2";
        public static readonly string LSR = "3";
        public static readonly string ASR = "4";
        public static readonly string ADC = "5";
        public static readonly string SBC = "6";
        public static readonly string ROR = "7";
        public static readonly string TST = "8";
        public static readonly string RSB = "9";
        public static readonly string CMP = "A";
        public static readonly string CMN = "B";
        public static readonly string ORR = "C";
        public static readonly string MUL = "D";
        public static readonly string BIC = "E";
        public static readonly string MVN = "F";


       //Hexa equivalent for registers
        public static readonly string  r0 = "0";
        public static readonly string  r1 = "1";
        public static readonly string r2 = "2";
        public static readonly string r3 = "3";
        public static readonly string  r4 = "4";
        public static readonly string r5 = "5";
        public static readonly string r6 = "6";
        public static readonly string r7 = "7";
       


    }
}
