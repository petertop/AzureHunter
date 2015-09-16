using AzureHunter.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AzureHunter.ViewModels
{
    public class ViewModelFile
    {
        // Constructor
        public ViewModelFile()
        { }


        // Properties
        public int Id { get; set; }
        public string FileName { get; set; }
        public string ContentType { get; set; }
        public FileType FileType { get; set; }
    }
}