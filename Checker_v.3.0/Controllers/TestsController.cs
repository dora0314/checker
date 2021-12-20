using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Checker_v._3._0.Helpers;
using Checker_v._3._0.Models;
using Checker_v._3._0.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Checker_v._3._0.Controllers
{
    [Authorize]
    public class TestsController : Controller
    {
        private DataContext dataContext;
        private IWebHostEnvironment appEnvironment;

        public TestsController(DataContext context, IWebHostEnvironment appEnvironment)
        {
            dataContext = context;
            this.appEnvironment = appEnvironment;
        }

        public ActionResult List()
        {
            var tests = dataContext.Tests
                .Select(x => new TestDto() 
                { 
                    Id = x.Id,
                    Title = x.Title,
                    TaskTitle = x.Task.Title,
                    FileName = new FileInfo(x.TestFilePath).Name
                });

            return View("List", tests);
        }

        public ActionResult _CreateForm()
        {
            var tasks = dataContext.Tasks
                .Select(x => new SelectListItem()
                {
                    Text = x.Title,
                    Value = $"{x.Id}"
                }).ToList();

            return View("_CreateForm", new TestViewModel() { Tasks = tasks });
        }

        [HttpPost]
        public ActionResult Create(TestViewModel model)
        {
            var task = dataContext.Tasks
                .Include(x => x.Course)
                .SingleOrDefault(x => x.Id == model.TaskId);

            if (task == null)
                return ResultHelper.Failed($"Задачи #{model.TaskId} не существует.");

            string path = appEnvironment.WebRootPath + $"/Files/Tests/{task.Course.Title}/{task.Title}/";
            var directory = Directory.CreateDirectory(path);

            using (var fileStream = new FileStream( path + model.TestFile.FileName, FileMode.Create))
            {
                model.TestFile.CopyTo(fileStream);
            }

            var testToCreate = new Test()
            {
                Title = model.Title,
                TestFilePath = path + model.TestFile.FileName,
                Task_id = task.Id
            };

            dataContext.Entry(testToCreate).State = EntityState.Added;
            dataContext.SaveChanges();

            return Redirect("/Tests/List");
        }
    }
}
