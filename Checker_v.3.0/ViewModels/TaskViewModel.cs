using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Checker_v._3._0.ViewModels
{
    public class TaskViewModel
    {
        public int Id { get; set; }

        public string Title { get; set; }
        
        public string Description { get; set; }

        public int MaxResult { get; set; }
        
        public int CourseId { get; set; }

        public List<SelectListItem> Courses { get; set; }
    }
}
