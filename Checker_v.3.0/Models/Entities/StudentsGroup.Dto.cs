using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Checker_v._3._0.Models
{
    public class StudentsGroupDto
    {
        public int Id;
        public string Name;
        public string Title;
        public string DetailUrl;
        public IEnumerable<CourseDto> Courses;
        public List<UserDto> Students;
    }
}
