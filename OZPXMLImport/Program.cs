using OZPXMLImport.DataModelSQL;
using OZPXMLImport.Import;
using OZPXMLImport.Import.ImportXML;
using OZPXMLImport.ImportExcel;
using System;
using System.Configuration;
using System.Data.Entity.Core.EntityClient;
using System.Data.SqlClient;
using System.IO;

namespace OZPXMLImport
{
    class Program
    {
        static void Main(string[] args)
        {
            IImporter importer;
            string command;
            string argument;

            if (args.Length != 2)
            {
                MessageAndExit("Špatný počet parametrů.");
            }

            command = args[0];
            argument = args[1];

            switch (command)
            {
                case "import":
                    string extension = Path.GetExtension(argument).ToLower();
                    if (extension == ".xml")
                    {
                        //primitive form of dependency injection
                        importer = new XMLImporter(new DataConnectorDB());
                        importer.Import(argument);
                    }
                    else if (extension == ".xls" || extension == ".xlsx")
                    {
                        importer = new ExcelImporter(new DataConnectorDB());
                        importer.Import(argument);
                    }
                    else
                    {
                        MessageAndExit("Špatný formát souboru (koncovka).");
                    }
                    break;
                case "list":
                    DateTime date;
                    bool isDate = DateTime.TryParse(argument, out date);
                    if (!isDate)
                    {
                        MessageAndExit("Špatný formát data (má být RRRR-MM-DD).");
                    }
                    new DataConnectorDB().OutputList(date);
                    break;
                default:
                    break;
            }

            Console.WriteLine("Hotovo");
            Console.WriteLine("Press Any Key To Continue");
            Console.ReadLine();
        }

        /// <summary>
        /// writes message to console and exits the application
        /// </summary>
        /// <param name="Message"></param>
        private static void MessageAndExit(string message)
        {
            Console.WriteLine(message);
            Console.WriteLine("Press Any Key To Continue");
            Console.ReadLine();
            Environment.Exit(0);
        }
    }
}
