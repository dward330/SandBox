using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StringExtensionMethods;

namespace SandBox
{
    class Program
    {
        static void Main(string[] args)
        {
            Demo_ExtensionMethods(5);
        }

        /// <summary>
        /// This Method Demos Extension Methods
        /// </summary>
        public static void Demo_ExtensionMethods(int characterLength = 0) {
            List<String> wordList = new List<string>();
            wordList.Add("Test");
            wordList.Add("Tested");
            wordList.Add("Tests");
            wordList.Add("Testing");

            wordList.ElementsLargerThan(characterLength).PrintStringList();

            PolitelyEndProgram();
        }

        public static void PolitelyEndProgram() {
            Console.WriteLine("\nProgram is finished running. Press Any Key to End Program...");
            Console.ReadKey(true);
        }
    }
}
