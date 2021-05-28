using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment2 {

    public class Table : SearchMethod {

        private int _valid = 0;

        public override string Search (KB KB, CL query) {

            List<Symbol> sym_ = new List<Symbol>();
            foreach (Symbol s in CL.GetAll<Symbol>()) {
                if (!sym_.Contains(s.polarity ? s : ~s)) {
                    sym_.Add(s.polarity ? s : ~s);
                }
            }

            KB model = new KB(new List<string>());
            
            if (Check(KB, query, sym_, model)) {
                return "YES: " + _valid.ToString();
            }

            return "NO";
        }

        private bool Check (KB origKB, CL query, List<Symbol> sym_, KB model) {

            if (sym_.Count == 0) {

                if (origKB.Entails(model)) {

                    _valid++;
                    return (query.Evaluate(model) == true);
                }
                
                else {
                    return true;
                }
            }
            else {

                Symbol p = sym_[0];
                sym_.RemoveAt(0);

                return (Check(origKB, query, new List<Symbol>(sym_), model.Extend(p)) && Check(origKB, query, new List<Symbol>(sym_), model.Extend(~p)));
            }
        }
    }
}
