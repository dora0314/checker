using Checker_v._3._0.Models.Attributes;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

#nullable disable

namespace Checker_v._3._0.Models
{
    [Table("User")]
    [ListDisplay("Пользователи")]
    [EditDisplay("Пользователь")]
    public partial class User : EntityObject
    {
        private DataContext dataContext;
        public User() : base() { }

        public User(DataContext context)
        {
            dataContext = context;
        }

        /// <summary>
        /// Ф.И.О.
        /// </summary>
        [ListDisplay("Ф.И.О.")]
        [DetailDisplay("Ф.И.О.")]
        [EditDisplay("Ф.И.О.")]
        [InputType("text")]
        [Required(ErrorMessage = "Поле 'Ф.И.О.' обязательно для заполнения")]
        [Column("Title")]
        [StringLength(255, ErrorMessage = "Строка слишком длинная")]
        public string Title { get; set; }

        /// <summary>
        /// Имя
        /// </summary>
        [ListDisplay("Имя")]
        [DetailDisplay("Имя")]
        [EditDisplay("Имя")]
        [InputType("text")]
        [Required(ErrorMessage = "Поле 'Короткое имя' обязательно для заполнения")]
        [Column("ShortName")]
        [StringLength(255, ErrorMessage = "Строка слишком длинная")]
        public string ShortName { get; set; }

        /// <summary>
        /// Email
        /// </summary>
        [ListDisplay("Email")]
        [DetailDisplay("Email")]
        [EditDisplay("Email")]
        [InputType("email")]
        [Required(ErrorMessage = "Поле 'Email' обязательно для заполнения")]
        [Column("Email")]
        [StringLength(255, ErrorMessage = "Строка слишком длинная")]
        public string Email { get; set; }

        /// <summary>
        /// Пароль
        /// </summary>
        [EditDisplay("Пароль")]
        [InputType("password")]
        [Required(ErrorMessage = "Поле 'пароль' обязательно для заполнения")]
        [Column("Password")]
        public string Password { get; set; }

        /// <summary>
        /// Роль
        /// </summary>
        [ListDisplay("Роль")]
        [DetailDisplay("Роль")]
        [EditDisplay("Роль")]
        [InputType("select")]
        [Required(ErrorMessage = "Поле 'Роль' обязательно для заполнения")]
        [ForeignKey("Role_id")]
        public UserRole Role
        {
            get
            {
                if (_role == null)
                    _role = dataContext.Set<UserRole>().First(x => x.Id == this.Role_id);
                return _role;
            }
            set => _role = value;
        }
        private UserRole _role;
        [Column("Role_id")]
        public int Role_id { get; set; }

        /// <summary>
        /// Группа
        /// </summary>
        [ListDisplay("Группа")]
        [DetailDisplay("Группа")]
        [EditDisplay("Группа")]
        [InputType("select")]
        [ForeignKey("StudentsGroup_id")]
        public StudentsGroup Group{ get; set; }
        [Column("StudentsGroup_id")]
        public int? StudentsGroup_id { get; set; }
    }
}
