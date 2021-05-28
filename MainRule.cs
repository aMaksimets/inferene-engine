using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment2 {
    public class MainRule : CL {

        public CL antecedent;
        public CL consequent;
        public Connective connective;

        public MainRule (bool polarity, CL antecedent, CL consequent, Connective connective, string identity) : base (polarity, identity) {
            this.antecedent = antecedent;
            this.consequent = consequent;
            this.connective = connective;
        }

        public override string GetLiteral (bool? withPolarity = null) => ((withPolarity ?? polarity) ? "" : "~(") + identity + ((withPolarity ?? polarity) ? "" : ")");

        public static MainRule operator ~ (MainRule rule) => GetClause(rule.GetLiteral(!rule.polarity)) as MainRule;

        public override bool? Evaluate (KB KnowBase) {

            return KnowBase.GetPolarity(this) ?? !(polarity ^ EvaluateRecursively(KnowBase));
        }

        private bool? EvaluateRecursively (KB KnowBase) {

            bool? evalAnte = this.antecedent.Evaluate(KnowBase);
            bool? evalCons = this.consequent.Evaluate(KnowBase);

            switch (this.connective) {
                case (Connective.CONJUNCTION):
                    return (evalAnte & evalCons);

                case (Connective.DISJUNCTION):
                    return (evalAnte | evalCons);

                case (Connective.IMPLICATION):
                    return evalAnte.Implies(evalCons);

                case (Connective.BICONDITIONAL):
                    return evalAnte.Biconditional(evalCons);


            }

            return null;
        }

        public override bool Assert (KB KnowBase) {

            bool result = false;
            bool? evalAnte;
            bool? evalCons;

            switch (this.connective) {
                case (Connective.IMPLICATION):

                    if (antecedent.Evaluate(KnowBase) == true) {
                        result |= polarity ?
                            KnowBase.ClauseAdd(consequent) :
                            KnowBase.ClauseAdd(~consequent);
                    }
                    else if (antecedent.Evaluate(KnowBase) == false) {
                        if (!polarity) {
                            Console.WriteLine("Cannot assert[" + literal + "].");
                        }
                    }

                    if (consequent.Evaluate(KnowBase) == false) {
                        result |= polarity ?
                            KnowBase.ClauseAdd(~antecedent) :
                            KnowBase.ClauseAdd(antecedent);
                    }
                    else if (consequent.Evaluate(KnowBase) == true) {
                        if (!polarity) {
                            Console.WriteLine("Assertion failed on: [" + literal + "].");
                        }
                    }

                    break;

                case (Connective.BICONDITIONAL):

                    evalAnte = antecedent.Evaluate(KnowBase);
                    if (evalAnte != null) {
                        if (result |= polarity == evalAnte)
                        {

                            KnowBase.ClauseAdd(~consequent);
                        }
                        else {
                            KnowBase.ClauseAdd(consequent);
                        }  
                    }

                    evalCons = consequent.Evaluate(KnowBase);
                    if (evalCons != null) {
                        if (result |= polarity == evalAnte)
                        {

                            KnowBase.ClauseAdd(~consequent);
                        }
                        else
                        {
                            KnowBase.ClauseAdd(consequent);
                        }
                    }

                    break;

                case (Connective.CONJUNCTION):
                    result |= polarity ?
                        KnowBase.ClauseAdd(antecedent) :
                        KnowBase.ClauseAdd(~antecedent);

                    result |= polarity ?
                        KnowBase.ClauseAdd(consequent) :
                        KnowBase.ClauseAdd(~consequent);

                    break;

                case (Connective.DISJUNCTION):

                    evalAnte = antecedent.Evaluate(KnowBase);
                    if (evalAnte == false) {
                        result |= polarity ?
                            KnowBase.ClauseAdd(consequent) :
                            KnowBase.ClauseAdd(~consequent);
                    }
                    else if (evalAnte == true) {
                        if (!polarity) {
                            Console.WriteLine("Assertion failed on: [" + literal + "].");
                        }
                    }

                    evalCons = consequent.Evaluate(KnowBase);
                    if (consequent.Evaluate(KnowBase) == false) {
                        result |= polarity ?
                            KnowBase.ClauseAdd(consequent) :
                            KnowBase.ClauseAdd(~consequent);
                    }
                    else if (evalCons == true) {
                        if (!polarity) {
                            Console.WriteLine("Assertion failed on: [" + literal + "].");
                        }
                    }

                    if (polarity) {
                        if ((evalCons & evalAnte) == false) {
                            Console.WriteLine("Assertion failed on: [" + literal + "].");
                        }
                    }
                    if (antecedent.Evaluate(KnowBase) == false) {
                        result |= KnowBase.ClauseAdd(consequent);
                    }
                    else if (consequent.Evaluate(KnowBase) == false) {
                        result |= KnowBase.ClauseAdd(antecedent);
                    }

                    break;
            }
            
            return result;
        }
    }
}
