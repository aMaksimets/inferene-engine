using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment2 {

    class Program {

        static void Main(string[] args) {

            bool? _t = true;
            bool? _f = false;
            bool? _n = null;

            bool? TT = !(_t ^ _t);
            bool? TF = !(_f ^ _t);
            bool? FF = !(_f ^ _f);
            bool? TN = !(_n ^ _t);
            bool? FN = !(_f ^ _n);

            if (!FH.FilePathwayExists(args[1])) {

                Console.WriteLine("Inccorect file or path.");
            }
            else {

                FH.ParseFile();
                KB KB = new KB(FH.Clauses);
                CL query = CL.GetClause(FH.Query);

                switch (args[0].ToUpper()) {

                    case "TEST": {
                            Console.WriteLine($"Testing file: {args[1]}");
                            foreach (string clause in FH.Clauses) {
                                Console.WriteLine(clause);
                            }
                            Console.WriteLine($"Searching: {FH.Query}");

                            Console.WriteLine("\nResults:");
                            Console.WriteLine($"BC: Backward Chain - {new BC().Search(KB, query)}");
                            Console.WriteLine($"RC: Resolution Chain - {new RFC().Search(KB, query)}");
                            Console.WriteLine($"TT: Truth Table - {new Table().Search(KB, query)}");
                            Console.WriteLine($"FC: Forward Chain - {new FC().Search(KB, query)}");
                            
                            break;
                        }

                    case "TT": {
                            Console.WriteLine(new Table().Search(KB, query));
                            break;
                        }

                    case "FC": {
                            Console.WriteLine(new FC().Search(KB, query));
                            break;
                        }

                    case "BC": {
                            Console.WriteLine(new BC().Search(KB, query));
                            break;
                        }

                    case "RFC": {
                            Console.WriteLine(new RFC().Search(KB, query));
                            break;
                        }
                }
            }

            Console.ReadLine();
        }
    }
}
