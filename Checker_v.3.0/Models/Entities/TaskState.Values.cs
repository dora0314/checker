using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable

namespace Checker_v._3._0.Models
{
    public partial class TaskState
    {
        public static TaskState Find(DataContext dataContext, string name)
        {
            return dataContext.Set<TaskState>()
                .FirstOrDefault(x => x.Name == name);
        }

        public static TaskState Obtain(DataContext dataContext, string name, string title)
        {
            var state = Find(dataContext, name);

            if (state == null)
            {
                state = new TaskState()
                {
                    Name = name,
                    Title = title
                };

                dataContext.TaskStates.Add(state);
                dataContext.SaveChanges();
            }

            return state;
        }

        public static TaskState Success(DataContext dataContext)
        {
            var state = Find(dataContext, "Success");

            if (state == null)
                state = Obtain(dataContext, "Success", "Сдана");

            return state;
        }

        public static TaskState Failed(DataContext dataContext)
        {
            var state = Find(dataContext, "Failed");

            if (state == null)
                state = Obtain(dataContext, "Failed", "Не сдана");

            return state;
        }

        public static TaskState InProgress(DataContext dataContext)
        {
            var state = Find(dataContext, "In progress");

            if (state == null)
                state = Obtain(dataContext, "In progress", "Ждет оценки");

            return state;
        }

        public static TaskState SolutionLoaded(DataContext dataContext)
        {
            var state = Find(dataContext, "Solution loaded");

            if (state == null)
                state = Obtain(dataContext, "Solution loaded", "Решение загружено");

            return state;
        }

        public static void Install(DataContext dataContext)
        {
            Success(dataContext);
            Failed(dataContext);
            InProgress(dataContext);
            SolutionLoaded(dataContext);
        }
    }
}
