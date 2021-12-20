using Checker_v._3._0.Models.Attributes;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

#nullable disable

namespace Checker_v._3._0.Models
{
    [Table("Task")]
    [ListDisplay("Задачи")]
    [EditDisplay("Задача")]
    public partial class Task : EntityObject
    {
        private DataContext dataContext;

        public Task(DataContext context)
        {
            dataContext = context;
        }

        public Task() : base()
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
        /// Описание
        /// </summary>
        [ListDisplay("Описание")]
        [DetailDisplay("Описание")]
        [EditDisplay("Описание")]
        [InputType("textarea")]
        [Required(ErrorMessage = "Поле 'Описание' обязательно для заполнения")]
        [Column("Description")]
        public string Description { get; set; }

        /// <summary>
        /// Курс
        /// </summary>
        [ListDisplay("Курс")]
        [DetailDisplay("Курс")]
        [EditDisplay("Курс")]
        [InputType("select")]
        [Required(ErrorMessage = "Поле 'Группа задач' обязательно для заполнения")]
        [ForeignKey("Course_id")]
        public virtual Course Course { get; set; }
        [Column("Course_id")]
        public int Course_id { get; set; }

        /// <summary>
        /// Язык программирования
        /// </summary>
        [ListDisplay("Язык программирования")]
        [DetailDisplay("Язык программирования")]
        [EditDisplay("Язык программирования")]
        [InputType("select")]
        [Required(ErrorMessage = "Поле 'Язык программирования' обязательно для заполнения")]
        [ForeignKey("ProgrammingLanguage_id")]
        public virtual ProgrammingLanguage ProgrammingLanguage { get; set; }
        [Column("ProgrammingLanguage_id")]
        public int ProgrammingLanguage_id { get; set; }

        /// <summary>
        /// Максимально возможный результат
        /// </summary>
        [ListDisplay("Макс. результат")]
        [DetailDisplay("Макс. результат")]
        [EditDisplay("Макс. результат")]
        [InputType("text")]
        [Required(ErrorMessage = "Поле 'Максю результат' обязательно для заполнения")]
        [Column("MaxResult")]
        public int MaxResult { get; set; }

        private ICollection<Test> _tests;
        public virtual ICollection<Test> Tests
        {
            get
            {
                if (_tests == null)
                    _tests = dataContext.Set<Test>().Where(x => x.Task_id == this.Id).ToList();
                return _tests;
            }
            set => _tests = value;
        }
    }
}
