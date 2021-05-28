using System.Collections.Generic;

namespace Assignment2 {

    public class FC : SearchMethod {

        public override string Search (KB KB, CL query) {

            HornDatabase hornDB = new HornDatabase(KB);

            string _q = query.identity;
            string _p = "";

            Queue<string> agenda = new Queue<string>(hornDB._f);

            if (hornDB._f.Contains(_q)) {
                path.Add(_q);
                return $"YES: {string.Join(", ", path)}";
            }

            while (agenda.Count > 0) {

                _p = agenda.Dequeue();

                if (!path.Contains(_p)) {

                    path.Add(_p);

                    foreach (string c in hornDB.clauseSymb.GetNew(_p)) {

                        if (--hornDB.countUnresolve[c] == 0) {
                            if (hornDB.resultForClause[c] == _q) {
                                path.Add(_q);
                                return $"YES: {string.Join(", ", path)}";
                            }

                            agenda.Enqueue(hornDB.resultForClause[c]);
                        }
                    }
                }
            }

            return "NO";

        }
    }
}
