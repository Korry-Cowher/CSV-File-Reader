using CSV_File_Reader.Classes;
using CSV_File_Reader.Utilities;
using System;
using System.Collections.Generic;

namespace CSV_File_Reader
{

    internal class ClientCommunication
    {
        /// <summary>
        /// Grabs array of all file options in CSV File Directory,
        /// Displays the values, calls GetUserSelection to get user selection
        /// </summary>
        /// <returns>Name of file selected</returns>
        public string SelectFile()
        {
            FileUtilities fileUtilities = new FileUtilities();

            string[] fileOptions = fileUtilities.GetFileOptions().ToArray();
            int indexSelected = GetUserSelection(fileOptions, "Please select file");

            return fileOptions[indexSelected];
        }

        /// <summary>
        /// Grabs all necessary data and calls necessary methods to create and order requested values
        /// </summary>
        /// <param name="outputSelection">Object which contains all values necessary to complete sorting of values</param>
        public void GenerateRequestedOutput(OutputSelection outputSelection)
        {
            FileUtilities fileUtilities = new FileUtilities();
            List<string> finalSortedList = new List<string>();
            outputSelection.FileContents = fileUtilities.LoadCSV(outputSelection.FileName);

            try
            {
                finalSortedList = fileUtilities.FileContentsToRequestedList(outputSelection);
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(ex.Message, Console.ForegroundColor);
                Console.ForegroundColor = ConsoleColor.White;
                GenerateClientExitMenu();
            }

            if (finalSortedList.Count > 0)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("\n\n\t\tRequested Output: \"" + outputSelection.FileName + "\" \"" + outputSelection.SortBy + "\" \"" + outputSelection.SortOrder + "\" \n\n\t\t Sorted Request: " + string.Join(", ", finalSortedList) + "\n\n");
                Console.ForegroundColor = ConsoleColor.White;
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("\n\n\t\tNo " + outputSelection.SortBy + " values in file selected\n\n", Console.ForegroundColor);
                Console.ForegroundColor = ConsoleColor.White;
            }
        }

        /// <summary>
        /// Creates sort by array, calls method to output user select menu and gets index of users selection
        /// </summary>
        /// <returns>User selected sort by option</returns>
        public string SelectTypeToSort()
        {
            string[] sortByOptions = new[] { "alpha", "numeric", "both" };
            int indexSelected = GetUserSelection(sortByOptions, "Please select values to sort");

            return sortByOptions[indexSelected];
        }

        /// <summary>
        /// Creates sort order array, calls method to output user select menu and gets index of users selection
        /// </summary>
        /// <returns>User selected sort order option</returns>
        public string SelectSortOrder()
        {
            string[] sortByOptions = new[] { "ascending", "descending" };
            int indexSelected = GetUserSelection(sortByOptions, "Please select values to sort");

            return sortByOptions[indexSelected];
        }

        /// <summary>
        /// Creates user select menu to either exit program or sort another file
        /// </summary>
        /// <returns>Boolean Value, if user would like to rerun program</returns>
        public bool GenerateClientExitMenu()
        {
            string[] sortByOptions = new[] { "Sort Another File", "Exit" };
            int indexSelected = GetUserSelection(sortByOptions, "Would you like to sort another file?");

            if (indexSelected == 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Displays specified message, then calls method to generate select menu based off of given array
        /// </summary>
        /// <param name="availableValues">Array of string values to display in select menu</param>
        /// <param name="promptMessage">Message displayed before select menu</param>
        /// <returns>Index of selected value in array</returns>
        private int GetUserSelection(string[] availableValues, string promptMessage)
        {
            int fileIndexSelected = 0;
            Console.Write("\n" + promptMessage + ": \n");
            OutputSelectOptions(availableValues);
            bool nonAcceptedInput = true;

            while (nonAcceptedInput)
            {
                string inputSelected = Console.ReadLine();

                if(int.TryParse(inputSelected, out int result))
                {
                    fileIndexSelected = Int32.Parse(inputSelected);
                    fileIndexSelected = fileIndexSelected - 1;

                    if (fileIndexSelected < 0 || fileIndexSelected >= availableValues.Length)
                    {
                        OutputSelectOptions(availableValues);
                    }
                    else
                    {
                        nonAcceptedInput = false;
                    }
                } 
                else
                {
                    OutputSelectOptions(availableValues);
                }
            }

            return fileIndexSelected;
        }

        /// <summary>
        /// Creates select menu for a string array
        /// </summary>
        /// <param name="availableValues">Array of values to display in select menu</param>
        private void OutputSelectOptions(string[] availableValues)
        {
            Console.WriteLine("\nSelect from available options");

            for (int i = 0; i <= availableValues.Length - 1; i++)
            {
                int menuSelectionNumber = i + 1;
                Console.WriteLine();
                Console.WriteLine("\t" + menuSelectionNumber + ": " + availableValues[i] + "\n");
            }
        }
    }
}
