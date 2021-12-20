using System.Linq;

namespace Checker_v._3._0.Models
{
    public partial class UserRole
    {
        public static UserRole Find(DataContext dataContext, string name)
        {
            return dataContext.Set<UserRole>()
                .FirstOrDefault(x => x.Name == name);
        }

        public static UserRole Obtain(DataContext dataContext, string name, string title)
        {
            var role = Find(dataContext, name);

            if(role == null)
            {
                role = new UserRole()
                {
                    Name = name,
                    Title = title
                };

                dataContext.UserRoles.Add(role);
                dataContext.SaveChanges();
            }

            return role;
        }

        public static UserRole Student(DataContext dataContext)
        {
            var role = Find(dataContext, "Student");

            if (role == null)
                role = Obtain(dataContext, "Student", "Студент");

            return role;
        }

        public static UserRole Teacher(DataContext dataContext)
        {
            var role = Find(dataContext, "Teacher");

            if (role == null)
                role = Obtain(dataContext, "Teacher", "Преподаватель");

            return role;
        }

        public static UserRole Admin(DataContext dataContext)
        {
            var role = Find(dataContext, "Administrator");

            if (role == null)
                role = Obtain(dataContext, "Administrator", "Администратор");

            return role;
        }

        public static void Install(DataContext dataContext)
        {
            Student(dataContext);
            Teacher(dataContext);
            Admin(dataContext);
        }
    }
}
