using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSV_File_Reader.Classes
{
     class OutputSelection
    {
        [Required(ErrorMessage = "File Name Rquired")]
        public string FileName { get; set; }
        public string SortBy { get; set; } = "both";
        public string SortOrder { get; set; } = "ascending";
        public string FileContents { get; set; }
    }
}
