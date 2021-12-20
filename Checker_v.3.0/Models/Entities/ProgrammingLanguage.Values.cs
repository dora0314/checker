using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable

namespace Checker_v._3._0.Models
{
    public partial class ProgrammingLanguage
    {
        public static ProgrammingLanguage Find(DataContext dataContext, string name)
        {
            return dataContext.Set<ProgrammingLanguage>()
                .FirstOrDefault(x => x.Name == name);
        }

        public static ProgrammingLanguage Obtain(DataContext dataContext, string name, string title)
        {
            var language = Find(dataContext, name);

            if (language == null)
            {
                language = new ProgrammingLanguage()
                {
                    Name = name,
                    Title = title
                };

                dataContext.ProgrammingLanguages.Add(language);
                dataContext.SaveChanges();
            }

            return language;
        }

        public static void Install(DataContext dataContext)
        {
        }
    }
}
