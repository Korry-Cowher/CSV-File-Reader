using System;
using CsvHelper;
using System.IO;
using System.Globalization;
using System.Linq;
using CSV_File_Reader.Utilities;
using System.Collections.Generic;

namespace CSV_File_Reader
{
    internal class Program
    {
        static void Main(string[] args)
        {
            FileUtilities fileUtilities = new FileUtilities();
            ClientCommunication clientCommunication = new ClientCommunication();

            string selectedFileName = clientCommunication.SelectFile();
            fileUtilities.LoadCSV(selectedFileName);
        }
    }
}
