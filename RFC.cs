using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment2 {
    class RFC : SearchMethod {

        public override string Search (KB KB, CL query) {
            bool? queryResult = query.Evaluate(KB);
            if (queryResult.HasValue) {
                path.Add(query.literal);

                if ((bool)queryResult) {
                    return "TRUE: " + string.Join(", ", path);
                } else {
                    return "FALSE";
                }
            }

            foreach (CL clause in KB.clauses.Values) {
                if (!path.Contains(clause.literal)) {
                    if (clause.Assert(KB)) {

                        path.Add(clause.literal);
                        return Search(KB, query);
                    }
                }
            }

            return "UNKNOWN";
        }
    }
}
