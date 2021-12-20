using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable

namespace Checker_v._3._0.Models
{
    public partial class TestState
    {
        public static TestState Find(DataContext dataContext, string name)
        {
            return dataContext.Set<TestState>()
                .FirstOrDefault(x => x.Name == name);
        }

        public static TestState Obtain(DataContext dataContext, string name, string title)
        {
            var state = Find(dataContext, name);

            if (state == null)
            {
                state = new TestState()
                {
                    Name = name,
                    Title = title
                };

                dataContext.TestStates.Add(state);
                dataContext.SaveChanges();
            }

            return state;
        }

        public static TestState Success(DataContext dataContext)
        {
            var state = Find(dataContext, "Success");

            if (state == null)
                state = Obtain(dataContext, "Success", "Пройден");

            return state;
        }

        public static TestState Failed(DataContext dataContext)
        {
            var state = Find(dataContext, "Failed");

            if (state == null)
                state = Obtain(dataContext, "Failed", "Провален");

            return state;
        }

        public static void Install(DataContext dataContext)
        {
            Success(dataContext);
            Failed(dataContext);
        }
    }
}
