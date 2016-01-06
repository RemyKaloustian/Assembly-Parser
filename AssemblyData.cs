using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/*
 * AUTHOR : Rémy KALOUSTIAN
 * DESCRIPTION : This file contains all the assembly values of the keywords used during parsing.
 */


namespace PEP___Assembly_Parser
{
    class AssemblyData
    {
        //Operations on data processing
        public static readonly string AND = "and";
        public static readonly string EOR = "eor";
        public static readonly string ADC = "adc";
        public static readonly string SBC = "sbc";
        public static readonly string ROR = "ror";
        public static readonly string TST = "tst";
        public static readonly string RSB = "rsb";
        public static readonly string CMP = "cmp";
        public static readonly string CMN = "cmn";
        public static readonly string ORR = "orr";
        public static readonly string MUL = "mul";
        public static readonly string BIC = "bic";
        public static readonly string MVN = "mvn";

        //Operations on immediate
        public static readonly string ADD = "add";
        public static readonly string SUB = "sub";
        public static readonly string MOV = "mov";

        //Operations on both data processing and immediate
        public static readonly string LSL = "lsl";
        public static readonly string LSR = "lsr";
        public static readonly string ASR = "asr";

        //Operations on Load/Store
        public static readonly string STR = "str";
        public static readonly string LDR = "ldr"; 

        //Operation on branch
        public static readonly string B = "b"; 


        //Assembly conditions values
        public static readonly string EQ = "eq";
        public static readonly string NE = "ne";
        public static readonly string CS = "cs";
        public static readonly string CC = "cc";
        public static readonly string MI = "mi";
        public static readonly string PL = "pl";
        public static readonly string VS = "vs";
        public static readonly string VC = "vc";
        public static readonly string HI = "hi";
        public static readonly string LS = "ls";
        public static readonly string GE = "ge";
        public static readonly string LT = "lt";
        public static readonly string GT = "gt";
        public static readonly string LE = "le";
        public static readonly string AL = "al";

        //Assembly register values
        public static readonly string R0 = "r0";
        public static readonly string R1 = "r1";
        public static readonly string R2 = "r2";
        public static readonly string R3 = "r3";
        public static readonly string R4 = "r4";
        public static readonly string R5 = "r5";
        public static readonly string R6 = "r6";
        public static readonly string R7 = "r7";
    
    }//class AssemblyData
}//ns
