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
        /// Grabs array of all file options in CSV File Directory,
        /// Displays the values,
        /// Takes in user input of selection.
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
                finalSortedList = FileContentsToRequestedList(outputSelection);
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
                Console.WriteLine("\n\n\t\tRequested Output\n\n\t\t" + string.Join(", ", finalSortedList) + "\n\n");
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
        /// Create sort by options array, calls method to output user select menu
        /// </summary>
        /// <returns>User selected sort by option</returns>
        public string SelectTypeToSort()
        {
            string[] sortByOptions = new[] { "alpha", "numeric", "both" };
            int indexSelected = GetUserSelection(sortByOptions, "Please select values to sort");

            return sortByOptions[indexSelected];
        }

        /// <summary>
        /// Create sort order options array, calls method to output user select menu
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
        /// Displays specified message, then generates select menu based off of given array
        /// </summary>
        /// <param name="availableValues">Array of string values to display in select menu</param>
        /// <param name="promptMessage">Message displayed before select menu</param>
        /// <returns>Index of selected value in array</returns>
        private int GetUserSelection(string[] availableValues, string promptMessage)
        {
            int fileIndexSelected = 0;
            Console.Write("\n" + promptMessage + ": \n");
            OutputSelectOptions(availableValues);
            bool acceptedInput = false;

            while (!acceptedInput)
            {
                try
                {
                    fileIndexSelected = Int32.Parse(Console.ReadLine());
                }
                catch
                {
                    OutputSelectOptions(availableValues);
                }

                fileIndexSelected = fileIndexSelected - 1;

                if (fileIndexSelected < 0 || fileIndexSelected >= availableValues.Length)
                {
                    OutputSelectOptions(availableValues);
                }
                else
                {
                    acceptedInput = true;
                }
            }

            return fileIndexSelected;
        }

        /// <summary>
        /// Creates select menu for an string array
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

        /// <summary>
        /// Takes values from outputSelect object, parses CSV file values array and orders to users request
        /// </summary>
        /// <param name="outputSelection">Object containing all neccessary values to create final sorted value</param>
        /// <returns></returns>
        /// <exception cref="Exception">If a user selects a non-allowed value that is not caught by logic</exception>
        private List<string> FileContentsToRequestedList(OutputSelection outputSelection)
        {
            List<string> fileContentsList = GetListFromFileString(outputSelection.FileContents);
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
                    return NumericSorter(numericList, outputSelection.SortOrder);
                case "alpha":
                    return AlphaSorter(alphaList, outputSelection.SortOrder);
                case "both":
                    List<string> sortedAlphaList = AlphaSorter(alphaList, outputSelection.SortOrder);
                    List<string> sortedNumericList = NumericSorter(numericList, outputSelection.SortOrder);
                    sortedNumericList.AddRange(sortedAlphaList);
                    return sortedNumericList;
                default:
                    throw new Exception("Selected type to order does not exist");
            }
        }

        /// <summary>
        /// Take CSV file data string, converts to an array
        /// </summary>
        /// <param name="fileContents">CSV File data string</param>
        /// <returns>List of CSV file data rows</returns>
        private List<string> GetListFromFileString(string fileContents)
        {
            List<string> fileContentList = new List<string>();
            fileContents.Trim(new char[] { ' ', '\r', '\n' });
            fileContentList = fileContents.Split(",").ToList();
            return fileContentList;
        }

        /// <summary>
        /// Take numeric array, sorts to requested order
        /// </summary>
        /// <param name="numericList">List of values to be sorted</param>
        /// <param name="sortOrder">Order to sort values in</param>
        /// <returns></returns>
        private List<string> NumericSorter(List<float> numericList, string sortOrder)
        {
            List<string> stringifiedNumericList = new List<string>();
            numericList.Sort();

            if (sortOrder.ToLower().Equals("descending"))
            {
                numericList.Reverse();
            }

            numericList.ForEach(numericValue =>
            {
                stringifiedNumericList.Add(numericValue.ToString());
            });

            return stringifiedNumericList;
        }

        /// <summary>
        /// Take alpha array, sorts to requested order
        /// </summary>
        /// <param name="numericList">List of values to be sorted</param>
        /// <param name="sortOrder">Order to sort values in</param>
        /// <returns></returns>
        private List<string> AlphaSorter(List<string> alphaList, string sortOrder)
        {
            Dictionary<string, string> valueToRemoveQuotes = new Dictionary<string, string>();

            for (int i = 0; i < alphaList.Count; i++)
            {
                string actualValue = alphaList[i];
                string sortValue = alphaList[i].Trim(' ');
                sortValue = alphaList[i].Replace("\"", "");
                sortValue = alphaList[i].Replace("\'", "");
                valueToRemoveQuotes.Add(sortValue, actualValue);
                alphaList[i] = sortValue;

            }

            alphaList.Sort();

            if (sortOrder.ToLower().Equals("descending"))
            {
                alphaList.Reverse();
            }

            foreach (var value in valueToRemoveQuotes)
            {
                int indexToUpdate = alphaList.IndexOf(value.Key.ToString());
                alphaList[indexToUpdate] = value.Value.ToString();
                alphaList[indexToUpdate] = alphaList[indexToUpdate].Trim(' ');
            }

            return alphaList;
        }
    }
}
