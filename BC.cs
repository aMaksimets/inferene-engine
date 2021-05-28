using System.Collections.Generic;

namespace Assignment2 {
    public class BC : SearchMethod {

        public override string Search(KB KB, CL query) {

            HornDatabase hdb = new HornDatabase(KB);

            string _q = query.identity;
            string _p = "";
            string _d = "";

            Queue<string> ag = new Queue<string>(new string[] { _q });
            List<string> cl = new List<string>();
            Queue<string> op = new Queue<string>();

            if (hdb._f.Contains(_q)) {
                path.Add(_q);
                return $"YES: {string.Join(", ", path)}";
            }

            while (ag.Count > 0) {

                _p = ag.Dequeue();
                cl.Insert(0, _p);

                foreach (string _c in hdb.resultClauses.GetNew(_p)) {
                    foreach (string str in hdb.cSymbols.GetNew(_c)) {

                        if (hdb._f.Contains(str)) {

                            if (!path.Contains(str)) { path.Add(str); }

                            op.Enqueue(_c);

                            while (op.Count > 0) {

                                _d = op.Dequeue();

                                if (--hdb.countUnresolve[_d] == 0) {

                                    if (!path.Contains(hdb.resultForClause[_d])) { path.Add(hdb.resultForClause[_d]); }

                                    if (hdb.resultForClause[_d] == _q) {
                                        return $"YES: {string.Join(", ", path)}";
                                    }
                                    foreach (string e in hdb.clauseSymb.GetNew(hdb.resultForClause[_d])) { op.Enqueue(e);}
                                }
                            }
                        }
 
                        else if (!cl.Contains(str) && !ag.Contains(str)) {

                            ag.Enqueue(str);
                        }
                    }
                }
            }
            return "NO";
        }
    }
}
