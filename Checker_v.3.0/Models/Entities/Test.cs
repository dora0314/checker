using Checker_v._3._0.Models.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace Checker_v._3._0.Models
{
    [Table("Test")]
    [ListDisplay("Тесты")]
    [EditDisplay("Тест")]
    public partial class Test : EntityObject
    {
        private DataContext dataContext;

        public Test(DataContext context)
        {
            dataContext = context;
        }

        public Test() : base()
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
        /// Задача
        /// </summary>
        [ListDisplay("Задача")]
        [DetailDisplay("Задача")]
        [EditDisplay("Задача")]
        [InputType("select")]
        [Required(ErrorMessage = "Поле 'Задача' обязательно для заполнения")]
        [ForeignKey("Task_id")]
        public Task Task { get; set; }
        [Column("Task_id")]
        public int Task_id { get; set; }

        /// <summary>
        /// Путь к тестовому файлу
        /// </summary>
        [ListDisplay("Тестовый файл")]
        [DetailDisplay("Тестовый файл")]
        [EditDisplay("Тестовый файл")]
        [InputType("text")]
        [Required(ErrorMessage = "Поле 'Тестовый файл' обязательно для заполнения")]
        [Column("TestFilePath")]
        [StringLength(1024, ErrorMessage = "Строка слишком длинная")]
        public string TestFilePath { get; set; }
    }
}
