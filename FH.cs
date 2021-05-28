using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.IO;

namespace Assignment2 {

    public static class FH {

        private static string       filePathway;
        private static string[]     textArray;

        public static string Query { get; private set; }
        public static List<string> Clauses { get; private set; }

        public static bool FilePathwayExists (string passedInPath) {
            bool exists = false;
            if (File.Exists(passedInPath)) {
                filePathway = passedInPath;
                exists = true;
            }
            return exists;
        }

        public static void ParseFile() {

            textArray = File.ReadAllLines(filePathway);
            textArray[1] = textArray[1].Replace(" ", string.Empty);
            Clauses = Regex.Split(textArray[1], ";").Where(s => !string.IsNullOrEmpty(s)).ToList();

            Query = textArray[3].Replace(" ", string.Empty);
        }
    }
}
