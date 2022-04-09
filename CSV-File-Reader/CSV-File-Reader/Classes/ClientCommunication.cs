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
            int fileIndexSelected;

            Console.WriteLine("CSV Sorter\n");
            Console.WriteLine("Please Select File To Sort");

            List<string> fileOptions = fileUtilities.GetFileOptions();


            while (true)
            {
                Console.Write("\nSelect File: ");
                try
                {
                    fileIndexSelected = Int32.Parse(Console.ReadLine());
                    fileIndexSelected = fileIndexSelected - 1;

                    if (fileIndexSelected < 0 || fileIndexSelected >= fileOptions.Count)
                    {
                        OutputFilesToSelect(fileOptions);
                    }
                    else
                    {
                        break;
                    }
                }
                catch
                {
                    OutputFilesToSelect(fileOptions);
                }
            }

            return fileOptions[fileIndexSelected];
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string SelectSortType()
        {
            OutputSortTypes();

            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fileOptions"></param>
        private void OutputFilesToSelect(List<String> fileOptions)
        {
            Console.WriteLine("\nPlease select available options");
            for (int i = 0; i < fileOptions.Count; i++)
            {
                Console.WriteLine();
                Console.WriteLine(i + 1 + ": " + fileOptions[i]);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void OutputSortTypes()
        {
            string[] sortTypes = new string[3] { "alpha", "numeric", "both" };

            Console.WriteLine("\nPlease select type to sort");
            for (int i = 0; i <= sortTypes.Length - 1; i++)
            {
                Console.WriteLine();
                Console.WriteLine(i + 1 + ": " + sortTypes[i]);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void OutputSortOrderOptions()
        {
            string[] sortOrderType = new string[2] { "ascending", "descending" };

            Console.WriteLine("\nPlease select order to sort by");
            for (int i = 0; i <= sortOrderType.Length - 1; i++)
            {
                Console.WriteLine();
                Console.WriteLine(i + 1 + ": " + sortOrderType[i]);
            }
        }
    }
}
