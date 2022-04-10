using CSV_File_Reader.Classes;
using CSV_File_Reader.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSV_File_Reader
{

    internal class ClientCommunication
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string SelectFile()
        {
            FileUtilities fileUtilities = new FileUtilities();

            Console.WriteLine("CSV Sorter");

            string[] fileOptions = fileUtilities.GetFileOptions().ToArray();
            int indexSelected = GetUserSelection(fileOptions, "Please select file");

            return fileOptions[indexSelected];
        }

        public void generateRequestedOutput(OutputSelection outputSelection)
        {
            FileUtilities fileUtilities = new FileUtilities();
            string chosenFileContents = fileUtilities.LoadCSV(outputSelection.FileName);
        }

        public string SelectTypeToSort()
        {
            string[] sortByOptions = new [] { "alpha", "numeric", "both" };
            int indexSelected = GetUserSelection(sortByOptions, "Please select values to sort");

            return sortByOptions[indexSelected];
        }

        public string SelectSortOrder()
        {
            string[] sortByOptions = new[] { "ascending", "descending" };
            int indexSelected = GetUserSelection(sortByOptions, "Please select values to sort");

            return sortByOptions[indexSelected];
        }

        /// <summary>
        /// 
        /// </summary>
        private void OutputSelectOptions(string[] availableValues)
        {

            Console.WriteLine("\nSelect from available options");

            for (int i = 0; i <= availableValues.Length - 1; i++)
            {
                Console.WriteLine();
                Console.WriteLine(i + 1 + ": " + availableValues[i] + "\n");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="availableValues"></param>
        /// <param name="promptMessage"></param>
        /// <returns></returns>
        private int GetUserSelection(string[] availableValues, string promptMessage)
        {
            int fileIndexSelected;
            Console.Write("\n" + promptMessage + ": \n");
            OutputSelectOptions(availableValues);

            while (true)
            {    
                try
                {
                    fileIndexSelected = Int32.Parse(Console.ReadLine());
                    fileIndexSelected = fileIndexSelected - 1;

                    if (fileIndexSelected < 0 || fileIndexSelected >= availableValues.Length)
                    {
                        OutputSelectOptions(availableValues);
                    }
                    else
                    {
                        break;
                    }
                }
                catch
                {
                    OutputSelectOptions(availableValues);
                }
            }
            return fileIndexSelected;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private string[] FileContentsToList()
        {
            return null;
        }
    }
}
