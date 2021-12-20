using Checker_v._3._0.Models.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

#nullable disable

namespace Checker_v._3._0.Models
{
    [Table("TaskState")]
    [ListDisplay("Состояния задач")]
    [EditDisplay("Состояние задачи")]
    public partial class TaskState : EntityObject
    {
        private DataContext dataContext;

        public TaskState(DataContext context)
        {
            dataContext = context;
        }

        public TaskState() : base()
        {

        }

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
        /// Результаты
        /// </summary>
        [ListDisplay("Результаты")]
        [DetailDisplay("Результаты")]
        [NotMapped]
        public IList<StudentTaskTeacherResult> Results
        {
            get
            {
                if (_results == null)
                    _results = dataContext.Set<StudentTaskTeacherResult>().Where(x => x.TaskState_id == this.Id).ToList();
                return _results;
            }
            set => _results = value;
        }
        private IList<StudentTaskTeacherResult> _results;
    }
}
