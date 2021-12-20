using Checker_v._3._0.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Checker_v._3._0.Models
{
    public class CourseDto
    {
        public int Id;
        public string Title;
        public string Name;
        public string DetailUrl;
        public UserDto Owner;
        public List<TaskDto> Tasks;
        public List<StudentTaskTeacherResultDto> TasksResults;
    }

    public class StudentCourseDto
    {
        public StudentViewModel Student;
        public CourseDto Course;
        public int SummResult;
        public int SummStudentResult;
    }
}
