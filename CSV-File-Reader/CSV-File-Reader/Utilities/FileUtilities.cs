using System;
using CsvHelper;
using System.IO;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using CSV_File_Reader.Utilities;
using CSV_File_Reader.Classes;

namespace CSV_File_Reader.Utilities
{
    internal class FileUtilities
    {
        /// <summary>
        /// Grabs string value of CSV data
        /// </summary>
        /// <param name="filename">Name of file to grab data from</param>
        /// <returns>string of requested CSV data</returns>
        public string LoadCSV(string filename)
        {
            string selectedFileContents = System.IO.File.ReadAllText(Constants.FilePaths.CSVFolder + "/" + filename);
            return selectedFileContents;
        }

        /// <summary>
        /// Grabs all CSV file options from selected directory
        /// Directory location is set in FilePath constants class
        /// </summary>
        /// <returns>List of all available CSV file names within directory</returns>
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

        /// <summary>
        /// Takes fileContents, converts to an List
        /// </summary>
        /// <param name="fileContents">CSV File data string</param>
        /// <returns>List of CSV file data rows</returns>
        public List<string> GetListFromFileString(string fileContents)
        {
            List<string> fileContentList = new List<string>();
            fileContents.Trim(new char[] { ' ', '\r', '\n' });
            fileContentList = fileContents.Split(",").ToList();
            return fileContentList;
        }

        /// <summary>
        /// Takes values from outputSelect object, parses CSV file values array and orders to users request
        /// </summary>
        /// <param name="outputSelection">Object containing all neccessary values to create final sorted list</param>
        /// <returns></returns>
        public List<string> FileContentsToRequestedList(OutputSelection outputSelection)
        {
            FileUtilities fileUtilities = new FileUtilities();
            ListSorter listSorter = new ListSorter();

            List<string> fileContentsList = fileUtilities.GetListFromFileString(outputSelection.FileContents);
            List<float> numericList = new List<float>();
            List<string> alphaList = new List<string>();

            fileContentsList.ForEach(fileContents =>
            {
                bool isString = fileContents.Contains("'") || fileContents.Contains("\"");

                if (isString)
                {
                    alphaList.Add(fileContents);
                }
                else
                {
                    try
                    {
                        numericList.Add(float.Parse(fileContents));
                    }
                    catch
                    {
                        fileContents = fileContents.ToString();
                        alphaList.Add(fileContents);
                    }
                }
            });

            switch (outputSelection.SortBy)
            {
                case "numeric":
                    return listSorter.Numeric(numericList, outputSelection.SortOrder);
                case "alpha":
                    return listSorter.Alpha(alphaList, outputSelection.SortOrder);
                case "both":
                    List<string> sortedAlphaList = listSorter.Alpha(alphaList, outputSelection.SortOrder);
                    List<string> sortedNumericList = listSorter.Numeric(numericList, outputSelection.SortOrder);
                    sortedNumericList.AddRange(sortedAlphaList);
                    return sortedNumericList;
                default:
                    throw new Exception("Selected type to order does not exist");
            }
        }

    }
}
