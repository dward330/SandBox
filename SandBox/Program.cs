using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StringExtensionMethods;
using System.Windows.Input;
using System.Xml.Serialization;
using System.Xml;
using System.Xml.Schema;
using System.IO;
using SandBox.XMLDemo;

namespace SandBox
{
    public delegate void KeyPressedEventHandler(ConsoleKey key, EventArgs e);
    public delegate double Func_CalcShippingCost(double itemPrice);

    class Program
    {
        //Will Hold all Xml Schema Validation Results
        private static HashSet<XMLSchemaValidationResult> _xmlSchemaValidationResults = new HashSet<XMLSchemaValidationResult>();

        //Main Program Starting Point
        static void Main(string[] args)
        {
            
            try {
                //Demo_EntityFramework();
                //Demo_XMLDeserialization();
                //Demo_EventHandler();
                Demo_DelegateChallenge();
            }
            catch (Exception e) {
                Console.WriteLine(String.Format("An Error Occured While Running the Demo!\n\nError:\n\t{0}",e));
            }

            PolitelyEndProgram();
        }

        /// <summary>
        /// This Demo showcases XML Deserialization
        /// </summary>
        public static void Demo_XMLDeserialization() {
            //Poco to hold Book Detail Information
            BookDetails bookDetails = null;
            
            //First Acquire the Schema (XSD), to be used to verify XML file follows all the rules
            XmlReaderSettings BookDetailXmlSettings = new XmlReaderSettings();
            BookDetailXmlSettings.Schemas.Add(null, @"XmlDemo\Schema\BookDetails.xsd");
            BookDetailXmlSettings.ValidationType = ValidationType.Schema;

            //Point a Reader or Stream to the XML File
            TextReader textReader = File.OpenText(@"XmlDemo\XmlFiles\HarryPotterBook1.xml");

            //Validate the XML File against the Schema (XSD)
            BookDetailXmlSettings.ValidationEventHandler += BookDetailXmlSchemaValidationHandler;
            XmlReader validationReader = XmlReader.Create(@"XmlDemo\XmlFiles\HarryPotterBook1.xml", BookDetailXmlSettings);
            while (validationReader.Read()) { } //Read the Entire XML Document to Force Validation

            //Serialize/Deserialize the XML File into our Object/POCO (BookDetails)
            if (Program._xmlSchemaValidationResults.Count == 0)
                bookDetails = (BookDetails)(new XmlSerializer(typeof(BookDetails))).Deserialize(textReader);
            else
            {
                Console.WriteLine("The Supplied XML File Violated the Schema (XSD) in the following ways:");
                foreach (XMLSchemaValidationResult result in Program._xmlSchemaValidationResults)
                    Console.WriteLine(String.Format("{0}:\n\t{1}", result.Severity, result.Message));
            }

            //Print the Details of the Book, stored in our POCO
            if (null != bookDetails) {
                Console.WriteLine("Update: {0}\n\nTitle: {1}\nAuthor: {2}\nPublisher: {3}\nNumber Of Pages: {4}", @"XML Deserialization Complete!"
                                            ,bookDetails.Title, bookDetails.Author, bookDetails.Publisher, bookDetails.NumOfPages);
                Console.WriteLine("Character:");
                foreach (String character in bookDetails.Characters) {
                    Console.WriteLine(String.Format("\t{0}",character));
                }
            }
        }
        
        /// <summary>
        /// Handles any XML Schema Violations
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void BookDetailXmlSchemaValidationHandler(object sender, ValidationEventArgs e)
        {
            //Add the Xml Schema Validation Result
            Program._xmlSchemaValidationResults.Add(new XMLSchemaValidationResult { Severity = XmlSeverityType.Warning, Message = e.Message });
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
        /// This Method Demos the use of delegates
        /// </summary>
        /// <param name="zoneLocation"></param>
        /// <param name="itemPrice"></param>
        public static void Demo_DelegateChallenge()
        {
            String zoneLocation = "";
            Double itemPrice = 0;
            Func_CalcShippingCost calcShippingCost = null;

            #region Gather information from user
            Console.WriteLine("Enter a Zone Location: ");
            zoneLocation = Console.ReadLine();
            Console.WriteLine("Enter the item Price: ");
            #endregion

            try
            {
                if (Double.TryParse(Console.ReadLine(), out itemPrice))
                {
                    switch (zoneLocation.ToLower())
                    {
                        case "zone1":
                            calcShippingCost = Zone1ShippingCost;
                            break;
                        case "zone2":
                            calcShippingCost = Zone2ShippingCost;
                            break;
                        case "zone3":
                            calcShippingCost = Zone3ShippingCost;
                            break;
                        case "zone4":
                            calcShippingCost = Zone4ShippingCost;
                            break;
                        default:
                            Console.WriteLine("You Mentioned an unknown zone.");
                            break;
                    }

                    Console.WriteLine("Shipping Cost: " + calcShippingCost(itemPrice));
                    Console.WriteLine("Final Cost: " + (itemPrice + calcShippingCost(itemPrice)));
                }
                else
                {
                    Console.WriteLine("Could not parse your item price into a Double!");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Error Occured!\n" + e);
            }
        }

        /// <summary>
        /// Prompts user that the program has ended and asks them to press any key to end.
        /// </summary>
        public static void PolitelyEndProgram() {
            Console.WriteLine("\nProgram is finished running. Press Any Key to End Program...");
            Console.ReadKey(true);
        }

        public static double Zone1ShippingCost(double itemPrice)
        {
            return (0.25*itemPrice);
        }

        public static double Zone2ShippingCost(double itemPrice)
        {
            return (0.12*itemPrice+25);
        }

        public static double Zone3ShippingCost(double itemPrice)
        {
            return (0.08*itemPrice);
        }

        public static double Zone4ShippingCost(double itemPrice)
        {
            return (0.04*itemPrice+25);
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

    /// <summary>
    /// POCO to store a single Xml Validation Result
    /// </summary>
    public class XMLSchemaValidationResult
    {
        public XmlSeverityType Severity { get; set; }
        public String Message { get; set; }
    }
}
