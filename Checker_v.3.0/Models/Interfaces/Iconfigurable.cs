using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Checker_v._3._0.Models
{
    interface IConfigurable
    {
        public string RouteList();
        public string RouteCreate();
        public string RouteDelete();
        public string RouteDetail();
    }
}
