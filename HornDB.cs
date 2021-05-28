using System.Collections.Generic;

namespace Assignment2 {

    public class HornDatabase {

        public Dictionary<string, List<string>> cSymbols = new Dictionary<string, List<string>>();
        public Dictionary<string, List<string>> clauseSymb = new Dictionary<string, List<string>>();
        public Dictionary<string, string> resultForClause = new Dictionary<string, string>();
        public Dictionary<string, List<string>> resultClauses = new Dictionary<string, List<string>>();
        public Dictionary<string, int> countUnresolve = new Dictionary<string, int>();
        public List<string> _f = new List<string>();

        public HornDatabase (KB knowledgeBase) {

            foreach (CL clause in knowledgeBase.clauses.Values) {
                if (clause is MainRule rule) {

                    ResultUp(rule.identity, rule.consequent.identity);

                    CL ptr = rule.antecedent;
                    while (ptr is MainRule nestedrule) {
                        SymbolUp(rule.identity, nestedrule.antecedent.identity);
                        ptr = nestedrule.consequent;
                    }

                    SymbolUp(rule.identity, ptr.identity);
                }

                else if (clause is Symbol symbol) {
                    _f.Add(symbol.identity);
                }
            }
        }

        private void SymbolUp (string clause, string symbol) {

            if (!cSymbols.ContainsKey(clause)) {
                cSymbols.Add(clause, new List<string>());
            }
            cSymbols[clause].Add(symbol);

            if (!clauseSymb.ContainsKey(symbol)) {
                clauseSymb.Add(symbol, new List<string>());
            }
            clauseSymb[symbol].Add(clause);

            if (!countUnresolve.ContainsKey(clause)) {
                countUnresolve.Add(clause, 0);
            }
            countUnresolve[clause]++;
        }

        private void ResultUp (string clause, string result) {

            if (!resultForClause.ContainsKey(clause)) {
                resultForClause.Add(clause, "");
            }
            resultForClause[clause] = result;

            if (!resultClauses.ContainsKey(result)) {
                resultClauses.Add(result, new List<string>());
            }
            resultClauses[result].Add(clause);
        }
    }
}
