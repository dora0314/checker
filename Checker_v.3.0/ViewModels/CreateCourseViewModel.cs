using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Checker_v._3._0.ViewModels
{
    public class CreateCourseViewModel
    {
        public string Title { get; set; }
        
        public int OwnerId { get; set; }
    }
}
