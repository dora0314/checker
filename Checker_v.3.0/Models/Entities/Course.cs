using Checker_v._3._0.Models.Attributes;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Checker_v._3._0.Models
{
    [Table("Course")]
    [ListDisplay("Курсы")]
    [EditDisplay("Курс")]
    public class Course : EntityObject
    {
        private DataContext dataContext;

        public Course(DataContext context)
        {
            dataContext = context;
        }

        public Course() : base()
        {

        }

        /// <summary>
        /// Системное название
        /// </summary>
        [ListDisplay("Системное название")]
        [DetailDisplay("Системное название")]
        [EditDisplay("Системное название")]
        [InputType("text")]
        [Required(ErrorMessage = "Поле 'Системное название' обязательно для заполнения")]
        [Column("Name")]
        [StringLength(255, ErrorMessage = "Строка слишком длинная")]
        public string Name { get; set; }

        /// <summary>
        /// Владелец
        /// </summary>
        [ListDisplay("Владелец")]
        [DetailDisplay("Владелец")]
        [EditDisplay("Владелец")]
        [InputType("select")]
        [Required(ErrorMessage = "Поле 'Владелец' обязательно для заполнения")]
        [ForeignKey("Owner_id")]
        public User Owner
        {
            get
            {
                if (_owner == null)
                    _owner = dataContext.Set<User>()
                        .Where(x => x.Role.Name == "Teacher")
                        .Where(x => x.Id == this.Owner_id)
                        .FirstOrDefault();
                return _owner;
            }
            set => _owner = value;
        }
        private User _owner;
        [Column("Owner_id")]
        public int Owner_id { get; set; }

        /// <summary>
        /// Название
        /// </summary>
        [ListDisplay("Название")]
        [DetailDisplay("Название")]
        [EditDisplay("Название")]
        [InputType("text")]
        [Required(ErrorMessage = "Поле 'Название' обязательно для заполнения")]
        [Column("Title")]
        [StringLength(255, ErrorMessage = "Строка слишком длинная")]
        public string Title { get; set; }

        private ICollection<Task> _tasks;
        public virtual ICollection<Task> Tasks
        {
            get
            {
                if (_tasks == null)
                    _tasks = dataContext.Set<Task>().Where(x => x.Course_id == this.Id).ToList();
                return _tasks;
            }
            set => _tasks = value;
        }

        /// <summary>
        /// Студенческие группы
        /// </summary>
        [ListDisplay("Студ. группы")]
        [DetailDisplay("Студ. группы")]
        [NotMapped]
        public IList<StudentsGroup> StudentsGroups
        {
            get
            {
                if (_studentsGroups == null)
                {
                    if (StudentsGroupsCourses == null)
                    {
                        StudentsGroupsCourses = dataContext.StudentsGroupCourses
                            .Where(x => x.Course_id == this.Id)
                            .ToList();
                    }
                    _studentsGroups = StudentsGroupsCourses.Select(x => x.StudentsGroup).ToList();
                }
                else if (_studentsGroups == null && dataContext != null)
                    _studentsGroups = dataContext.Set<StudentsGroupCourse>().Where(x => x.Course_id == this.Id).Select(x => x.StudentsGroup).ToList();
                return _studentsGroups;
            }
            set => _studentsGroups = value;
        }
        private IList<StudentsGroup> _studentsGroups;

        public List<StudentsGroupCourse> StudentsGroupsCourses { get; set; }
    }
}
