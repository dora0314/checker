using Checker_v._3._0.Models.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace Checker_v._3._0.Models
{
    [Table("StudentTestResult")]
    [ListDisplay("Результаты тестов")]
    [EditDisplay("Результат теста")]
    public partial class StudentTestResult
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Display(Name = "Id")]
        [Required(ErrorMessage = "Поле 'Id' обязательно для заполнения")]
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// Студент
        /// </summary>
        [ListDisplay("Студент")]
        [EditDisplay("Студент")]
        [DetailDisplay("Студент")]
        [InputType("select")]
        [Required(ErrorMessage = "Поле 'Студент' обязательно для заполнения")]
        [ForeignKey("Student_id")]
        public User Student { get; set; }
        [Column("Student_id")]
        public int Student_id { get; set; }

        /// <summary>
        /// Тест
        /// </summary>
        [ListDisplay("Тест")]
        [EditDisplay("Тест")]
        [DetailDisplay("Тест")]
        [InputType("select")]
        [Required(ErrorMessage = "Поле 'Тест' обязательно для заполнения")]
        [ForeignKey("Test_id")]
        public Test Test { get; set; }
        [Column("Test_id")]
        public int Test_id { get; set; }

        /// <summary>
        /// Статус
        /// </summary>
        [ListDisplay("Статус")]
        [EditDisplay("Статус")]
        [DetailDisplay("Статус")]
        [InputType("select")]
        [Required(ErrorMessage = "Поле 'Статус' обязательно для заполнения")]
        [ForeignKey("TestState_id")]
        public TestState TestState { get; set; }
        [Column("TestState_id")]
        public int TestState_id { get; set; }
    }
}
