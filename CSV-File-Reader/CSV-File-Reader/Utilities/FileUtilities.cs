using System;
using CsvHelper;
using System.IO;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using CSV_File_Reader.Utilities;

namespace CSV_File_Reader.Utilities
{
    internal class FileUtilities
    {
        /// <summary>
        /// Grabs string value of CSV data
        /// </summary>
        /// <param name="filename">Name of file to grab data from</param>
        /// <returns>string of request CSV data</returns>
        public string LoadCSV(string filename)
        {

            string selectedFileContents = System.IO.File.ReadAllText(Constants.FilePaths.CSVFolder + "/" + filename);

            return selectedFileContents;

        }

        /// <summary>
        /// Grabs all CSV file options from selected path to directory
        /// </summary>
        /// <returns>List of all available file names</returns>
        public List<string> GetFileOptions()
        {
            List<string> trimmedFileOptions = new List<string>();
            string[] fileOptions = Directory.GetFiles(Constants.FilePaths.CSVFolder, "*.csv");

            foreach (string fileOption in fileOptions)
            {
                string trimmedFile = fileOption.Remove(0, fileOption.LastIndexOf("\\") + 1);
                trimmedFileOptions.Add(trimmedFile);
            }

            return trimmedFileOptions;
        }
    }
}
