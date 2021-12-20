using Checker_v._3._0.Models.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

#nullable disable

namespace Checker_v._3._0.Models
{
    [Table("ProgrammingLanguage")]
    [ListDisplay("Языки программирования")]
    [EditDisplay("Язык программирования")]
    public partial class ProgrammingLanguage : EntityObject
    {
        private DataContext dataContext;

        public ProgrammingLanguage(DataContext context)
        {
            dataContext = context;
        }

        public ProgrammingLanguage() : base()
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
        /// Расширение
        /// </summary>
        [ListDisplay("Расширение")]
        [DetailDisplay("Расширение")]
        [EditDisplay("Расширение")]
        [InputType("text")]
        [Column("FileExtension")]
        [StringLength(128, ErrorMessage = "Строка слишком длинная")]
        public string FileExtension { get; set; }

        /// <summary>
        /// Задачи
        /// </summary>
        [ListDisplay("Задачи")]
        [DetailDisplay("Задачи")]
        [NotMapped]
        public IList<Task> Tasks
        {
            get
            {
                if (_tasks == null)
                    _tasks = dataContext.Set<Task>().Where(x => x.ProgrammingLanguage_id == this.Id).ToList();
                return _tasks;
            }
            set => _tasks = value;
        }
        private IList<Task> _tasks;
    }
}
