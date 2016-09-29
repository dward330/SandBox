using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StringExtensionMethods
{
    /// <summary>
    /// This class will contain some of the extension methods for List<String> Type Objects
    /// </summary>
    static class StringExtensionMethods
    {
        /// <summary>
        /// Return IEnumerable type containing items that have more characters than @numOfCharacters
        /// </summary>
        /// <param name="obj"> Object this method is applied to</param>
        /// <param name="numOfCharacters"> The number of characters list item needs to be larger than</param>
        /// <returns></returns>
        public static IEnumerable<String> ElementsLargerThan(this List<String> obj, int numOfCharacters) {
            return obj.Where(x => x.Length > numOfCharacters).ToList();
        }

        /// <summary>
        /// Prints all the items in IEnumerable to the Console Window
        /// </summary>
        /// <param name="obj"> Object this method is applied to</param>
        public static void PrintStringList(this IEnumerable<String> obj) {
            foreach (String item in obj)
                Console.WriteLine(item);
        }
    }
}
