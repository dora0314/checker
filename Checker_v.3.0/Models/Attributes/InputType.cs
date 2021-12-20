using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Checker_v._3._0.Models.Attributes
{
    public class InputType : System.Attribute
    {
        public string Name { get; set; }

        public InputType(string Name)
        {
            this.Name = Name;
        }
    }
}
