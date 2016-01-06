using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/*
 * AUTHOR : Rémy KALOUSTIAN
 * DESCRIPTION : This file contains all the binary values of the keywords used during parsing.
 */


namespace PEP___Assembly_Parser
{
    class HexaData
    {
        //Data-processing first value 
        public static readonly string DP = "010000";

        //Immediate first value
        public static readonly string IM = "00";

        //Binary equivalent for data processing instructions
        public static readonly string AND = "0000";
        public static readonly string EOR = "0001";
        public static readonly string DPLSL = "0010";
        public static readonly string DPLSR = "0011";
        public static readonly string DPASR = "0100";
        public static readonly string ADC = "0101";
        public static readonly string SBC = "0110";
        public static readonly string ROR = "0111";
        public static readonly string TST = "1000";
        public static readonly string RSB = "1001";
        public static readonly string CMP = "1010";
        public static readonly string CMN = "1011";
        public static readonly string ORR = "1100";
        public static readonly string MUL = "1101";
        public static readonly string BIC = "1110";
        public static readonly string MVN = "1111";

        //Binary equivalent for immediate instructions
        public static readonly string ILSL = "000";
        public static readonly string ILSR = "001";
        public static readonly string IASR = "010";
        public static readonly string ADD = "01100";
        public static readonly string SUB = "01101";
        public static readonly string MOV = "100";

        //Binary equivalent for Store and Load
        public static readonly string LDR = "01101";
        public static readonly string STR = "10010";

        //Binary for branch
        public static readonly string B = "1101";

        //Binary values for conditions
        public static readonly string EQ = "0000";
        public static readonly string NE = "0001";
        public static readonly string CS = "0010";
        public static readonly string CC = "0011";
        public static readonly string MI = "0100";
        public static readonly string PL = "0101";
        public static readonly string VS = "0110";
        public static readonly string VC = "0111";
        public static readonly string HI = "1000";
        public static readonly string LS = "1001";
        public static readonly string GE = "1010";
        public static readonly string LT = "1011";
        public static readonly string GT = "1100";
        public static readonly string LE = "1101";
        public static readonly string AL = "1110";

        //Binary equivalent for registers
        public static readonly string  R0 = "000";
        public static readonly string  R1 = "001";
        public static readonly string  R2 = "010";
        public static readonly string  R3 = "011";
        public static readonly string  R4 = "100";
        public static readonly string  R5 = "101";
        public static readonly string  R6 = "110";
        public static readonly string  R7 = "111";
       


    }
}
