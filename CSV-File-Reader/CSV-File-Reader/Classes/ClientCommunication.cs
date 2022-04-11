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

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("\n\n\t\tCSV Sorter");
            Console.ForegroundColor = ConsoleColor.White;

            string[] fileOptions = fileUtilities.GetFileOptions().ToArray();
            int indexSelected = GetUserSelection(fileOptions, "Please select file");

            return fileOptions[indexSelected];
        }

        public void generateRequestedOutput(OutputSelection outputSelection)
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
                generateClientExitMenu();
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

        public string SelectTypeToSort()
        {
            string[] sortByOptions = new[] { "alpha", "numeric", "both" };
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
        /// <returns></returns>
        public bool generateClientExitMenu()
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
        /// 
        /// </summary>
        /// <returns></returns>
        private List<string> FileContentsToRequestedList(OutputSelection outputSelection)
        {
            List<string> fileContentsList = GetListFromFileString(outputSelection.FileContents);
            List<float> numericList = new List<float>();
            List<string> alphaList = new List<string>();

            fileContentsList.ForEach(fileContents =>
            {
                switch (fileContents.Contains("'") || fileContents.Contains("\""))
                {
                    case true:
                        alphaList.Add(fileContents);
                        break;
                    case false:
                        try
                        {
                            numericList.Add(float.Parse(fileContents));
                        }
                        catch
                        {
                            fileContents = fileContents.ToString();
                            alphaList.Add(fileContents);
                        }
                        break;
                    default:
                        throw new Exception("Check CSV data,/n strings must be displayed inside ''");
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

        private List<string> GetListFromFileString(string fileContents)
        {
            List<string> fileContentList = new List<string>();
            fileContents.Trim(new char[] { ' ', '\r', '\n' });
            fileContentList = fileContents.Split(",").ToList();
            return fileContentList;
        }

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
