using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Checker_v._3._0.Models.Attributes
{
    public class ListDisplay : System.Attribute
    {
        public string Name { get; set; }

        public ListDisplay(string Name)
        {
            this.Name = Name;
        }
    }
}
