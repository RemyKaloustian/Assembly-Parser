using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PEP___Assembly_Parser
{
    class AssemblyData
    {

        //Assembly instruction values
        public static readonly string AND = "and";
        public static readonly string OR = "or";
        public static readonly string LSL = "lsl";
        public static readonly string LSR = "lsr";
        public static readonly string ASR = "asr";
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
        
        //Assembly register values
        public static readonly string r0 = "$r0";
        public static readonly string r1 = "$r1";
        public static readonly string r2 = "$r2";
        public static readonly string r3 = "$r3";
        public static readonly string r4 = "$r4";
        public static readonly string r5 = "$r5";
        public static readonly string r6 = "$r6";
        public static readonly string r7 = "$r7";
    }
}
