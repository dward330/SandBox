using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StringExtensionMethods;
using System.Windows.Input;

namespace SandBox
{
    public delegate void KeyPressedEventHandler(ConsoleKey key, EventArgs e);

    class Program
    {
        //Main Program Starting Point
        static void Main(string[] args)
        {
            Demo_EntityFramework();

            PolitelyEndProgram();
        }

        /// <summary>
        /// Demos using the Entity Framework
        /// </summary>
        public static void Demo_EntityFramework() {
            //Showing the Data from the Database Table
            using (var data = new ZeroCoolEntities()) {
                int spacePadding = 15;

                var firstRecord = data.XSDTables.FirstOrDefault<XSDTable>();
                if (firstRecord != null)
                    Console.WriteLine(String.Format("{0}{1}{2}", (nameof(firstRecord.Firstname) + "*").PadRight(spacePadding), (nameof(firstRecord.Lastname) + "*").PadRight(spacePadding), (nameof(firstRecord.Age).ToString() + "*").PadRight(spacePadding)));

                var people = data.XSDTables;
                foreach (XSDTable person in people)
                    Console.WriteLine(String.Format("{0}{1}{2}",person.Firstname.PadRight(spacePadding),person.Lastname.PadRight(spacePadding),person.Age.ToString().PadRight(spacePadding)));
            }
        }

        /// <summary>
        /// Demos Create an Event, Raising an Event and then Handling an Event that was raised
        /// </summary>
        public static void Demo_EventHandler() {
            ConsoleKeyInfo keyPressedInfo = new ConsoleKeyInfo();
            KeyboardKey key = new KeyboardKey(ConsoleKey.LeftArrow);
            key.KeyPressedEvent += new KeyPressedEventHandler(YourKeyWasPressed);

            do
            {
                Console.WriteLine("\nEnter a Key: ");
                keyPressedInfo  = Console.ReadKey();
                key.KeyPressed(keyPressedInfo.Key);
            } while (keyPressedInfo.Key != ConsoleKey.Escape);
        }

        /// <summary>
        /// Method that will respond/handle when a KeyPressedEventHandler is raised
        /// </summary>
        /// <param name="key"></param>
        /// <param name="e"></param>
        public static void YourKeyWasPressed(ConsoleKey key, EventArgs e)
        {
            Console.WriteLine("Your Key was Pressed: "+key.ToString());
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
        }

        /// <summary>
        /// Prompts user that the program has ended and asks them to press any key to end.
        /// </summary>
        public static void PolitelyEndProgram() {
            Console.WriteLine("\nProgram is finished running. Press Any Key to End Program...");
            Console.ReadKey(true);
        }              
    }

    public class KeyboardKey {
        public ConsoleKey EventKey = ConsoleKey.Escape;
        public event KeyPressedEventHandler KeyPressedEvent;

        public KeyboardKey(ConsoleKey key = ConsoleKey.Escape) {
            EventKey = key;
        }

        public void KeyPressed(ConsoleKey key, EventArgs e = null) {
            if (key == EventKey) {
                KeyPressedEvent(key, e);
            }
        }
    }
}
