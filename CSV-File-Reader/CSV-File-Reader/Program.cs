using System;
using CsvHelper;
using System.IO;
using System.Globalization;
using System.Linq;
using CSV_File_Reader.Utilities;
using System.Collections.Generic;
using CSV_File_Reader.Classes;

namespace CSV_File_Reader
{
    internal class Program
    {
        static void Main(string[] args)
        {
            FileUtilities fileUtilities = new FileUtilities();
            ClientCommunication clientCommunication = new ClientCommunication();

            while (true)
            {
                OutputSelection outputSelection = new OutputSelection();
                outputSelection.FileName = clientCommunication.SelectFile();
                outputSelection.SortBy = clientCommunication.SelectTypeToSort();
                outputSelection.SortOrder = clientCommunication.SelectSortOrder();

                clientCommunication.generateRequestedOutput(outputSelection);

                bool sortAnotherFile = clientCommunication.generateClientExitMenu();

                if (!sortAnotherFile)
                {
                    Environment.Exit(0);
                }
            }
        }
    }
}
