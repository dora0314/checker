using System;
using System.Linq;
using System.Reflection;

namespace Checker_v._3._0.Models
{
    public partial class DataContext
    {
        public dynamic Set(Type entityType)
        {
            MethodInfo method = this.GetType().GetMethods().First(x => x.Name == "Set" && x.IsGenericMethod);
            MethodInfo generic = method.MakeGenericMethod(entityType);
            return generic.Invoke(this, null);
        }
    }
}
