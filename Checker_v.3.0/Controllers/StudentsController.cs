using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Checker_v._3._0.Helpers;
using Checker_v._3._0.Models;
using Checker_v._3._0.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Jint;

namespace Checker_v._3._0.Controllers
{
    [Authorize]
    public class StudentsController : Controller
    {
        private DataContext dataContext;
        private IWebHostEnvironment appEnvironment;

        public StudentsController(DataContext context, IWebHostEnvironment appEnvironment)
        {
            dataContext = context;
            this.appEnvironment = appEnvironment;
        }

        public ActionResult CourseDetail(int courseId)
        {
            var course = dataContext.Courses
                .Find(courseId);

            if (course == null)
                return ResultHelper.EntityNotFound("Course");

            var student = Checker_v._3._0.Models.User.Get(dataContext, HttpContext);

            if (student == null)
                return ResultHelper.UserNotFound();

            if (!course.StudentsGroups.Contains(student.Group))
                return ResultHelper.Failed($"Курс {{{courseId}}} не прикреплен к группе текущего пользователя");

            var url = "https://" + HttpContext.Request.Host + "/Students/TaskDetail?taskId=";

            var courseDto = new CourseDto()
            {
                Id = course.Id,
                Title = course.Title,
                Name = course.Name,
                Tasks = dataContext.Tasks
                    .Where(y => y.Course_id == course.Id)
                    .Select(y => new TaskDto()
                    {
                        Id = y.Id,
                        Title = y.Title,
                        DetailUrl = url + y.Id,
                        Description = y.Description,
                        MaxResult = y.MaxResult
                    }).ToList(),
                TasksResults = dataContext.StudentsTaskTeacherResults
                    .Where(y => y.Task.Course_id == course.Id)
                    .Where(y => y.Student_id == student.Id)
                    .Select(y => new StudentTaskTeacherResultDto()
                    {
                        Id = y.Id,
                        StudentId = y.Student_id,
                        TaskId = y.Task_id,
                        TeacherResult = y.TeacherResult
                    }).ToList()
            };

            var studentCourse = new StudentCourseDto()
            {
                Student = new StudentViewModel()
                {
                    FullName = student.Title,
                    Id = student.Id,
                    ShortName = student.ShortName,
                    Email = student.Email,
                    Group = new StudentsGroupDto()
                    {
                        Id = student.Group.Id,
                        Title = student.Group.Title
                    }
                },
                Course = courseDto,
                SummStudentResult = courseDto.TasksResults.Sum(x => x.TeacherResult ?? 0),
                SummResult = courseDto.Tasks.Sum(x => x.MaxResult)
            };

            return View(studentCourse);
        }

        public ActionResult TaskDetail(int taskId)
        {
            var user = Checker_v._3._0.Models.User.Get(dataContext, HttpContext);

            if (user == null)
                return ResultHelper.UserNotFound();

            var task = dataContext.Tasks
                .Include(x => x.Course)
                .FirstOrDefault(x => x.Id == taskId);

            if (task == null)
                return ResultHelper.EntityNotFound("Task");

            var userDto = new UserDto()
            {
                Id = user.Id,
                FullName = user.Title
            };

            var tests = task.Tests
                .Select(x => new TestDto()
                {
                    Id = x.Id,
                    Title = x.Title
                })
                .ToList();

            var testsIds = tests.Select(x => x.Id).ToList();

            var testResults = dataContext.StudentsTestsResults
                .Where(x => x.Student_id == user.Id)
                .Where(x => testsIds.Contains(x.Test_id))
                .Select(x => new StudentTestResultDto() 
                { 
                    Id = x.Id,
                    Test = new TestDto()
                    {
                        Id = x.Test.Id,
                        Title = x.Test.Title
                    },
                    StudentId = user.Id,
                    State = x.TestState
                }).ToList();

            foreach(var test in tests.Where(x => !testResults.Any(y => y.Test.Id == x.Id)))
            {
                var studentTestResult = new StudentTestResult()
                {
                    Student_id = user.Id,
                    Test_id = test.Id,
                    TestState = dataContext.TestStates.First(x => x.Name == "Failed")
                };

                dataContext.Entry(studentTestResult).State = EntityState.Added;
                dataContext.SaveChanges();

                testResults.Add(new StudentTestResultDto()
                {
                    StudentId = user.Id,
                    Id = studentTestResult.Id,
                    Test = test,
                    State = studentTestResult.TestState
                });
            }

            var taskResult = dataContext.StudentsTaskTeacherResults
                .FirstOrDefault(x => x.Task_id == task.Id && x.Student_id == user.Id);

            if(taskResult == null)
            {
                var now = DateTime.UtcNow;

                taskResult = new StudentTaskTeacherResult()
                {
                    CreationDateTime = now,
                    StateDateTime = now,
                    TaskState = dataContext.TaskStates.First(x => x.Name == "Failed"),
                    Student_id = user.Id,
                    Task_id = task.Id
                };

                dataContext.Entry(taskResult).State = EntityState.Added;
                dataContext.SaveChanges();
            }

            var studentTaskDto = new StudentTaskDto()
            {
                Task = new TaskDto()
                {
                    Id = task.Id,
                    Title = task.Title,
                    Tests = tests,
                    Description = task.Description,
                    MaxResult = task.MaxResult
                },
                TestsResults = testResults,
                TeacherResult = new StudentTaskTeacherResultDto() 
                { 
                    Id = taskResult.Id,
                    StudentId = user.Id,
                    TaskId = task.Id,
                    SolutionLoadDateTime = taskResult.SolutionLoadDateTime,
                    SuccessTestsCount = testResults.Count(x => x.State.Name == "Success"),
                    TeacherResult = taskResult.TeacherResult
                }
            };

            return View(studentTaskDto);
        }

        [HttpPost]
        public JsonResult LoadSolution([FromForm] int studentResultId, [FromForm] IFormFile studentFile)
        {
            var studentResult = dataContext.StudentsTaskTeacherResults.Find(studentResultId);

            if (studentResult == null)
                return Json(new { Success = false, Message = $"Результат #{studentResultId} не существует." });

            var programmingLanguage = studentResult.Task.ProgrammingLanguage;

            if(!studentFile.FileName.EndsWith(programmingLanguage.FileExtension))
                return Json(new { Success = false, Message = $"неверный формат файла" });

            string path = appEnvironment.ContentRootPath + $"/AppData/Files/Students/{studentResult.Student.Title}/Solutions/{studentResult.Task.Title}/";
            var directory = Directory.CreateDirectory(path);

            using (var fileStream = new FileStream(path + studentFile.FileName, FileMode.Create))
            {
                studentFile.CopyTo(fileStream);
            }

            studentResult.StudentFilePath = path + studentFile.FileName;
            var now = DateTime.UtcNow;
            studentResult.SolutionLoadDateTime = now;
            studentResult.TaskState = TaskState.SolutionLoaded(dataContext);
            studentResult.StateDateTime = now;
            dataContext.Entry(studentResult).State = EntityState.Modified;
            dataContext.SaveChanges();

            return Json(new { Success = true, Message = "решение загружено", LoadDate = now.ToString("dd.MM.yy"), LoadTime = now.ToString("HH.mm")});
        }

        public JsonResult CheckSolution(int studentResultId)
        {
            var studentResult = dataContext.StudentsTaskTeacherResults.Find(studentResultId);

            if (studentResult == null)
                return Json(new { Success = false, Message = $"Результат #{studentResultId} не существует." });

            var tests = studentResult.Task.Tests;
            var studentTestsResults = dataContext.StudentsTestsResults
                .Where(x => x.Student_id == studentResult.Student_id && tests.Select(y => y.Id).ToList().Contains(x.Test_id))
                .ToList();

            var testsEngine = TestsEngine.ObtainEngine(studentResult.Task.ProgrammingLanguage);
            if(testsEngine == null)
                return Json(new { Success = false, Message = $"Не найден движок для языка программирования '{studentResult.Task.ProgrammingLanguage}'" });

            var data = new List<dynamic>();

            foreach (var test in tests)
            {
                var testResult = testsEngine.RunTest(studentResult.StudentFilePath, test.TestFilePath);

                var studentTestResult = studentTestsResults.FirstOrDefault(x => x.Test_id == test.Id);
                var testState = testResult ? TestState.Success(dataContext) : TestState.Failed(dataContext);

                if (studentTestResult == null)
                {
                    studentTestResult = new StudentTestResult()
                    {
                        Test_id = test.Id,
                        Student_id = studentResult.Student_id,
                        TestState = testState
                    };

                    dataContext.Entry(studentTestResult).State = EntityState.Added;
                }
                else
                {
                    studentTestResult.TestState = testState;
                    dataContext.Entry(studentTestResult).State = EntityState.Modified;
                }

                dataContext.SaveChanges();

                data.Add(new
                {
                    TestId = studentTestResult.Test_id,
                    TestTitle = test.Title,
                    State = studentTestResult.TestState
                });
            }

            var succesState = TaskState.Success(dataContext);
            var inProgressState = TaskState.InProgress(dataContext);

            if (data.Where(x => x.State.Title == "Success").Count() > (double)data.Count() / 2)
            {
                studentResult.TaskState = inProgressState;
                dataContext.Entry(studentResult).State = EntityState.Modified;
                dataContext.SaveChanges();
            }
            else if(studentResult.TaskState == succesState || studentResult.TaskState == inProgressState)
            {
                studentResult.TaskState = TaskState.Failed(dataContext);
                dataContext.Entry(studentResult).State = EntityState.Modified;
                dataContext.SaveChanges();
            }

            return Json(new { data = data.ToArray()});
        }
    }
}
