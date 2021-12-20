using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Checker_v._3._0.Models
{
    public class UserDto
    {
        public int Id;
        public int GroupId;
        public string ShortName;
        public string FullName;
        public string Email;
        public string GroupTitle;
        public int Points;
        public int TotalPoints;
        public List<TaskDto> Tasks;
        public string DetailUrl;
    }
}
