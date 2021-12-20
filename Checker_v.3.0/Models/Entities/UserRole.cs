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
    [Table("UserRole")]
    [ListDisplay("Роли")]
    [EditDisplay("Роль")]
    public partial class UserRole : EntityObject
    {
        private DataContext dataContext;

        public UserRole(DataContext context)
        {
            dataContext = context;
        }

        public UserRole() : base()
        {

        }

        /// <summary>
        /// Системное название
        /// </summary>
        [ListDisplay("Системное название")]
        [DetailDisplay("Системное название")]
        [EditDisplay("Ситемное название")]
        [InputType("text")]
        [Required(ErrorMessage = "Поле 'Системное название' обязательно для заполнения")]
        [Column("Name")]
        [StringLength(255, ErrorMessage = "Строка слишком длинная")]
        public string Name { get; set; }

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

        private ICollection<User> _users;
        public virtual ICollection<User> Users
        {
            get
            {
                if (_users == null)
                    _users = dataContext.Set<User>().Where(x => x.Role_id == this.Id).ToList();
                return _users;
            }
            set => _users = value;
        }
    }
}
