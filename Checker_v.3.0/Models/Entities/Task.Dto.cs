using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Checker_v._3._0.Models
{
    public class TaskDto
    {
        public int Id;
        public string DetailUrl;
        public string Title;
        public string Description;
        public string CourseTitle;
        public int CourseId;
        public int MaxResult;

        public IEnumerable<TestDto> Tests;
    }

    public class StudentTaskDto
    {
        public TaskDto Task;
        public List<StudentTestResultDto> TestsResults;
        public StudentTaskTeacherResultDto TeacherResult;
    }
}
