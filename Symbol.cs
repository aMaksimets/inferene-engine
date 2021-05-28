using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment2 {

    public class Symbol : CL {

        public override string GetLiteral(bool? withPolarity = null) => ((withPolarity ?? polarity) ? "" : "~") + identity;

        public override bool? Evaluate(KB KB) => KB.GetPolarity(this);

        public override bool Assert(KB KB) => false;
        public Symbol (bool polarity, string identity) : base (polarity, identity) { }

        public static Symbol operator ~ (Symbol symbol) => GetClause(symbol.GetLiteral(!symbol.polarity)) as Symbol;
    }
}
