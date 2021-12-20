using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Checker_v._3._0.Models
{
    public class StudentTaskTeacherResultDto
    {
        public int Id;
        public string DetailUrl;
        public UserDto Student;
        public int StudentId;
        public int TaskId;
        public int? TeacherResult;
        public DateTime? SolutionLoadDateTime;
        public int SuccessTestsCount;
        public string StudentSolutionFilePath;
        public string DownloadStudentSolutionUrl;
    }
}
