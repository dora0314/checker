using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Checker_v._3._0.ViewModels
{
    public class TestViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int TaskId { get; set; }
        public IFormFile TestFile { get;set; }

        public string FileName { get; set; }

        public List<SelectListItem> Tasks;
    }
}
