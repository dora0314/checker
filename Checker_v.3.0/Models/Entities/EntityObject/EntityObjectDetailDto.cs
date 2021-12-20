using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Checker_v._3._0.Models
{
    public class EntityObjectDetailDto
    {
        public int EntityId;
        public string EntityTitle;
        public string EntityName;
        public List<EntityObjectFieldDto> Fields;
    }
}
