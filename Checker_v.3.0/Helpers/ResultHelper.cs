using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Checker_v._3._0.Helpers
{
    public static class ResultHelper
    {
        public static ActionResult Successed()
        {
            return new JsonResult(new { success = true, reason = "" });
        }

        public static ActionResult Failed(string reason)
        {
            return new JsonResult(new { success = true, reason = reason });
        }

        public static ActionResult UserNotFound()
        {
            return Failed("Пользователь не найден");
        }

        public static ActionResult EntityNotFound(string entityName)
        {
            return Failed($"Сущность с именем {entityName} не найдена");
        }
    }
}
