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
        /// 
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        public string[] LoadCSV(string filename)
        {
            
            string selectedFile = System.IO.File.ReadAllText(Constants.FilePaths.CSVFolder + "/" + filename);
       

            return null;

        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
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
