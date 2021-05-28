using System;
using System.Collections.Generic;
using System.Linq;

namespace Assignment2 {

    public abstract class CL {

        public bool polarity { get; protected set; }
        public string identity { get; protected set; }
        public string literal { get; protected set; }

        public abstract bool? Evaluate (KB KB);
        public abstract bool Assert (KB KB);
        public static CL operator ~ (CL clause) => GetClause(clause.GetLiteral(!clause.polarity));
        public abstract string GetLiteral (bool? withPolarity = null);
        private static Dictionary<string, CL> cl = new Dictionary<string, CL>();
        public static List<T> GetAll<T>() where T : CL => cl.Values.Where(a => a.GetType() == typeof(T)).Select(a => a as T).ToList();

        protected CL (bool polarity, string identity) {
            this.polarity = polarity;
            this.identity = identity;
            this.literal = GetLiteral(polarity);

            if (!cl.ContainsKey(literal)) {
                cl.Add(literal, this);
            }
        }

        public static CL GetClause (string literal) {

            if (!cl.ContainsKey(literal)) {

                CL clause = PrStr(literal);

                if (!cl.ContainsKey(clause.literal)) {
                    cl.Add(clause.literal, clause);
                }

                return cl[clause.literal];
                
            }

            return cl[literal];
        }

        private static CL PrStr (string clause) {

            Node<string> root = new Node<string>("", null);
            Node<string> current = root;

            foreach (char c in clause) {

                if (current == null) {
                    break;
                }

                if (c == '(') {
                    current.value += '■';
                    current.AddChild("");
                    current = current.ch.Last();
                }

                else if (c == ')') {
                    current = current.parent;
                }

                else {
                    current.value += c;
                }
            }

            return PrStrN(root);
        }

        private static CL PrStrN (Node<string> node) {

            string[] opSymbols = new string[5] { "<=>", "=>", "||", "&", "~" };
            string[] splitStrings = { };

            bool nodeP = node.NegateParentAndInvert(true);

            while (node.value == "■") {
                node = node.ch[0];
                nodeP = node.NegateParentAndInvert(nodeP);
            }

            for (int i = 0; i < Enum.GetNames(typeof(Connective)).Length; i++) {

                splitStrings = node.value.Split(new String[] { opSymbols[i] }, 2, StringSplitOptions.RemoveEmptyEntries);

                if (splitStrings.Count() > 1) {

                    Node<string>[] splitNodes = node.Split(opSymbols[i]);

                    return new MainRule(
                        nodeP,                   
                        PrStrN(splitNodes[0]), 
                        PrStrN(splitNodes[1]), 
                        (Connective) i,               
                        node.Rebuild()                
                        );
                }
            }

            if (splitStrings.Count() == 1) {

                return new Symbol(
                    node.value[0] != '~',                   
                    node.value.Replace("~", string.Empty)   
                    );
            }

            return null;
        }
    }
}
