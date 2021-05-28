using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment2 {
    
    public enum Connective {
        BICONDITIONAL = 0,
        IMPLICATION = 1,
        DISJUNCTION = 2,
        CONJUNCTION = 3
    }

    public class Node<T> {


        public T value;

        public Node<T> parent { get; }


        public List<Node<T>> ch { get; }

        public Node (T value, Node<T> parent = null) {
            this.value = value;
            this.parent = parent;
            this.ch = new List<Node<T>>();
        }

        public void AddChild (T value) {
            ch.Add(new Node<T>(value, this));
        }
    }


    public static class SplittableExtension {

        const char childSymbol = '■';
        public static Node<string>[] Split (this Node<string> node, string delimiter, int maxSplit = 2) {


            string[] values = node.value.Split(new string[] { delimiter }, maxSplit, StringSplitOptions.RemoveEmptyEntries);
            int child = 0;
            List<Node<string>> result = new List<Node<string>>();

            foreach (string s in values) {

                result.Add(new Node<string>(s, node.parent));

                foreach (char c in s) {
                    if (c == childSymbol) {
                        result.Last().ch.Add(node.ch[child++]);
                    }
                }
            }

            return result.ToArray();
        }

        public static string Rebuild (this Node<string> node) {

            string[] array = node.value.Split(new char[] { childSymbol });

            string result = array[0];

            for (int i = 0; (i < array.Count() - 1) && (i < node.ch.Count()); i++) {

                result += "(";
                result += node.ch[i].Rebuild();
                result += ")";
                result += array[i + 1];
            }

            return result;
        }

        public static bool NegateParentAndInvert (this Node<string> node, bool toInvert) {

            if (node.value == "~■") {
                node.value = "■";
                toInvert = !toInvert;
            }

            return toInvert;
        }
    }



    public static class BooleanNullLogicExtensions {
        public static bool? Implies(this bool? a, bool? b) {
            return (!a | b);
        }
        public static bool? Biconditional (this bool? a, bool? b) {
            return (a.Implies(b) & b.Implies(a));
        }
    }

    public static class DictionaryExtensions {

        public static T GetAdd<K,T> (this Dictionary<K,T> dictionary, K key) where T : new() {
            if (dictionary.ContainsKey(key)) {
                return dictionary[key];
            }
            else {
                dictionary.Add(key, new T());
                return dictionary[key];
            }  
        }

        public static T GetNew<K, T> (this Dictionary<K, T> dictionary, K key) where T : new() {
            if (dictionary.ContainsKey(key)) {
                return dictionary[key];
            }
            else {
                return new T();
            }
        }
    }
}
