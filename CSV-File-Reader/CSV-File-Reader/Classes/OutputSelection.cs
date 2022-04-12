using System.ComponentModel.DataAnnotations;

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
