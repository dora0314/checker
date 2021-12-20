using System;
using System.Collections.Generic;
using System.Linq;
using Checker_v._3._0.Helpers;
using Checker_v._3._0.Models;
using Checker_v._3._0.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Checker_v._3._0.Controllers
{
    [Authorize]
    public class TasksController : Controller
    {
        private DataContext dataContext;

        public TasksController(DataContext context)
        {
            dataContext = context;
        }

        public ActionResult List()
        {
            var tasks = dataContext.Tasks
                .Select(x => new TaskDto() 
                { 
                    Id = x.Id,
                    Title = x.Title,
                    Description = x.Description,
                    CourseTitle = x.Course.Title,
                    MaxResult = x.MaxResult
                });

            return View("List", tasks);
        }

        public ActionResult _CreateForm()
        {
            var courses = dataContext.Courses
                .Select(x => new SelectListItem()
                {
                    Text = x.Title,
                    Value = $"{x.Id}"
                }).ToList();

            return View("_CreateForm", new TaskViewModel() { Courses = courses });
        }

        [HttpPost]
        public ActionResult Create(TaskViewModel model)
        {
            var taskGroup = dataContext.Courses.Find(model.CourseId);

            if (taskGroup == null)
                return ResultHelper.Failed($"Курса #{model.CourseId} не существует.");

            var taskToCreate = new Task()
            {
                Title = model.Title,
                Description = model.Description,
                Course_id = taskGroup.Id,
                MaxResult = model.MaxResult
            };

            dataContext.Entry(taskToCreate).State = EntityState.Added;
            dataContext.SaveChanges();

            return Redirect("/Tasks/List");
        }
    }
}
