using Checker_v._3._0.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Checker_v._3._0.ViewModels
{
    public class StudentViewModel
    {
        public int Id;
        public string Email;
        public string FullName;
        public string ShortName;
        public StudentsGroupDto Group;
        public IEnumerable<CourseDto> Courses;
    }
}
