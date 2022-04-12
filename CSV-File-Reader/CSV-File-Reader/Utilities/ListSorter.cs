using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSV_File_Reader.Utilities
{
    internal class ListSorter
    {

        /// <summary>
        /// Take numeric array, sorts to requested order
        /// </summary>
        /// <param name="numericList">List of values to be sorted</param>
        /// <param name="sortOrder">Order to sort values in</param>
        /// <returns></returns>
        public List<string> Numeric(List<float> numericList, string sortOrder)
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
        /// <param name="alphaList">List of values to be sorted</param>
        /// <param name="sortOrder">Order to sort values in</param>
        /// <returns></returns>
        public List<string> Alpha(List<string> alphaList, string sortOrder)
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
