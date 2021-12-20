using Checker_v._3._0.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Checker_v._3._0.ViewModels
{
    public class TeacherViewModel
    {
        public int Id;
        public string FullName;
        public string ShortName;
        public string Email;
        public IEnumerable<StudentsGroupDto> StudentsGroups;
        public IEnumerable<CourseDto> Courses;
    }
}
