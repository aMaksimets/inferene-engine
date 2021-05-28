using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment2 {

    public class KB {

        public Dictionary<string, CL> clauses { get; }

        public KB (List<string> raws) {
            clauses = new Dictionary<string, CL>();

            foreach (string raw in raws) {
                CL c = CL.GetClause(raw);
                if (!clauses.ContainsKey(c.identity)) {
                    clauses.Add(c.identity, c);
                }
                else {
                    if (clauses[c.identity].polarity != c.polarity) {
                        Console.WriteLine("Construction of [" + c.literal + "] failed.");
                    }
                }
            }
        }

        public KB (IEnumerable<CL> iclauses) {
            clauses = new Dictionary<string, CL>();

            foreach (CL c in iclauses) {
                if (!clauses.ContainsKey(c.identity)) {
                    clauses.Add(c.identity, c);
                }
                else {
                    if (clauses[c.identity].polarity != c.polarity) {
                        Console.WriteLine("Construction of [" + c.literal + "] failed.");
                    }
                }
            }
        }

        public bool? GetPolarity (CL clause) {
            if (clauses.ContainsKey(clause.identity)) {
                return clauses[clause.identity].polarity == clause.polarity;
            }
            return null;
        }

        public bool ClauseAdd (CL clause) {
            if (!clauses.ContainsKey(clause.identity)) {
                clauses.Add(clause.identity, clause);
                return true;
            }
            else {
                if (clauses[clause.identity].polarity != clause.polarity) {
                    Console.WriteLine("Construction of [" + clause.literal + "] failed.");
                }
            }
            return false;
        }

        public KB Extend (CL clause) {
            return new KB(clauses.Values.Concat(new[] { clause }));
        }

        public bool Entails (KB eKB) {


            foreach (CL clause in this.clauses.Values) {

                if (clause.Evaluate(eKB) == false) {
                    return false;
                }
            }

            return true;
        }

        public bool Entails (IEnumerable<CL> eClauses) {
            return Entails(new KB(eClauses));
        }

        public bool Entails (CL eClause) {
            return Entails(new[] { eClause });
        }

    }
}
